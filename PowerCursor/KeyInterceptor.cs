using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerCursor {
    class KeyInterceptor {
        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        private static KeyInterceptor mInstance;

        private static IntPtr mHookID = IntPtr.Zero;

        private static WinAPI.LowLevelKeyboardProc mCallback;

        public static KeyInterceptor The() {
            return mInstance ?? (mInstance = new KeyInterceptor());
        }

        private KeyInterceptor() {
            mCallback = HookCallback;
            mHookID = SetHook(mCallback);
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private static IntPtr SetHook(WinAPI.LowLevelKeyboardProc proc) {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                return WinAPI.SetWindowsHookEx(WinAPI.WH_KEYBOARD_LL, proc,
                    WinAPI.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            bool gotHandled = false;

            if (nCode >= 0) {
                Keys vkCode = (Keys)Marshal.ReadInt32(lParam);
                int keyFlags = Marshal.ReadInt32(lParam + 8);
                bool isKeyInjected = (keyFlags & WinAPI.LLKHF_INJECTED) > 0;

                if (!isKeyInjected) {
                    if ((int)wParam == WinAPI.WM_KEYDOWN || (int)wParam == WinAPI.WM_SYSKEYDOWN) {
                        var ev = new KeyEventArgs(vkCode);
                        The().KeyDown?.Invoke(The(), ev);
                        gotHandled = !ev.Handled;
                    } else if ((int)wParam == WinAPI.WM_KEYUP || (int)wParam == WinAPI.WM_SYSKEYUP) {
                        var ev = new KeyEventArgs(vkCode);
                        The().KeyUp?.Invoke(The(), ev);
                        gotHandled = !ev.Handled;
                    }
                }
            }

            if (gotHandled || nCode < 0) {
                return WinAPI.CallNextHookEx(mHookID, nCode, wParam, lParam);
            } else {
                return new IntPtr(1);
            }
        }

        private void OnProcessExit(object sender, EventArgs e) {
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
            WinAPI.UnhookWindowsHookEx(mHookID);
        }
    }
}
