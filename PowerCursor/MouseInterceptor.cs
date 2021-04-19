﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseInterceptor {
        public class MouseEventArgs : System.Windows.Forms.MouseEventArgs {
            public MouseEventArgs(MouseButtons buttons, int x, int y) 
                : base(buttons, 1, x, y, 0) { }

            public bool Handled { get; set; } = false;
        }

        public event EventHandler<MouseEventArgs> MouseMove;
        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;

        static MouseInterceptor mInstance;

        private static IntPtr mHookID = IntPtr.Zero;

        private static WinAPI.LowLevelMouseProc mCallback;

        private static MouseButtons mButtons;

        public static MouseInterceptor The() {
            return mInstance ?? (mInstance = new MouseInterceptor());
        }

        private MouseInterceptor() {
            mCallback = HookCallback;
            mHookID = SetHook(mCallback);
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        private static IntPtr SetHook(WinAPI.LowLevelMouseProc proc) {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                return WinAPI.SetWindowsHookEx(WinAPI.WH_MOUSE_LL, proc,
                    WinAPI.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            Func<EventHandler<MouseEventArgs>, MouseButtons, int, int, bool> handleEvent
                = (handler, buttons, x, y) => {
                var ev = new MouseEventArgs(buttons, x, y);
                handler?.Invoke(The(), ev);
                return ev.Handled;
            };

            bool gotHandled = false;

            if (nCode >= 0) {
                int mx = Marshal.ReadInt32(lParam + 0);
                int my = Marshal.ReadInt32(lParam + 4);

                if ((int)wParam == WinAPI.WM_LBUTTONDOWN) {
                    mButtons |= MouseButtons.Left;
                    gotHandled = handleEvent(The().MouseDown, MouseButtons.Left, mx, my);
                } else if ((int)wParam == WinAPI.WM_RBUTTONDOWN) {
                    mButtons |= MouseButtons.Right;
                    gotHandled = handleEvent(The().MouseDown, MouseButtons.Right, mx, my);
                } else if ((int)wParam == WinAPI.WM_LBUTTONUP) {
                    mButtons &= ~MouseButtons.Left;
                    gotHandled = handleEvent(The().MouseUp, MouseButtons.Left, mx, my);
                } else if ((int)wParam == WinAPI.WM_RBUTTONUP) {
                    mButtons &= ~MouseButtons.Right;
                    gotHandled = handleEvent(The().MouseUp, MouseButtons.Right, mx, my);
                } else if ((int)wParam == WinAPI.WM_MOUSEMOVE) {
                    gotHandled = handleEvent(The().MouseMove, mButtons, mx, my);
                }
            }

            if (!gotHandled || nCode < 0) {
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
