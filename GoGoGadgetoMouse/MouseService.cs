using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseService {
        private static MouseService mInstance;

        private enum State {
            None,
            DragWindow,
            ResizeWindow,
            OnTopWindow
        }

        private bool mAltKeyPressed;
        private bool mAltKeyUsed;

        private State mCurrentState;
        private MouseDragAction mDragAction;
        private MouseResizeAction mResizeAction;
        private WindowsInput.InputSimulator inputSim = new WindowsInput.InputSimulator();

        public static MouseService The() {
            return mInstance ?? (mInstance = new MouseService());
        }

        private MouseService() {
            MouseInterceptor.The().MouseMove += OnMouseMove;
            MouseInterceptor.The().MouseDown += OnMouseDown;
            MouseInterceptor.The().MouseUp += OnMouseUp;
            KeyInterceptor.The().KeyDown += OnKeyDown;
            KeyInterceptor.The().KeyUp += OnKeyUp;
        }

        private void OnMouseMove(object sender, MouseInterceptor.MouseEventArgs e) {
            switch (mCurrentState) {
                case State.DragWindow:
                    mDragAction?.Update(e.Location);
                    break;
                case State.ResizeWindow:
                    mResizeAction?.Update(e.Location);
                    break;
            }
        }

        private void OnMouseUp(object sender, MouseInterceptor.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && mCurrentState == State.DragWindow) {
                mCurrentState = State.None;
                mDragAction.Finish(e.Location);
                mDragAction = null;
                e.Handled = true;
            } else if (e.Button == MouseButtons.Right && mCurrentState == State.ResizeWindow) {
                mCurrentState = State.None;
                mResizeAction.Finish(e.Location);
                mResizeAction = null;
                e.Handled = true;
            } else if (e.Button == MouseButtons.Middle && mCurrentState == State.OnTopWindow) {
                mCurrentState = State.None;
                e.Handled = true;
            }
        }

        private void OnMouseDown(object sender, MouseInterceptor.MouseEventArgs e) {
            if (mAltKeyPressed && mCurrentState == State.None) {
                var hwndMouseOver = WinAPI.WindowFromPoint(e.Location);
                var topLevelHwnd = GetTopMostHwnd(hwndMouseOver);
                if (topLevelHwnd == default) return;

                var windowText = WinAPI.GetWindowTitle(topLevelHwnd);
                if (Properties.Settings.Default.excludes.ToEnumerable().Any(
                    exclude => Regex.IsMatch(windowText, exclude))) {
                    return;
                }

                if (e.Button == MouseButtons.Left) {
                    mDragAction = new MouseDragAction(topLevelHwnd, e.Location);
                    mCurrentState = State.DragWindow;
                    mAltKeyUsed = true;
                    e.Handled = true;

                } else if (e.Button == MouseButtons.Middle) {
                    mCurrentState = State.OnTopWindow;
                    mAltKeyUsed = true;
                    e.Handled = true;
                    ToggleTopmost(topLevelHwnd);

                } else if (e.Button == MouseButtons.Right) {
                    mResizeAction = new MouseResizeAction(topLevelHwnd, e.Location);
                    mCurrentState = State.ResizeWindow;
                    mAltKeyUsed = true;
                    e.Handled = true;
                }
            }
        }

        private static void ToggleTopmost(IntPtr hwnd) {
            WinAPI.SetWindowPos(
                hwnd,
                WinAPI.IsWindowTopMost(hwnd)
                    ? WinAPI.HWND_NOTOPMOST
                    : WinAPI.HWND_TOPMOST,
                0, 0, 0, 0,
                WinAPI.SWP_NOMOVE | WinAPI.SWP_NOSIZE);
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LMenu) {
                mAltKeyPressed = true;
            } else if (mAltKeyPressed && !mAltKeyUsed) {
                mAltKeyPressed = false;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LMenu) {
                if (mAltKeyUsed) {
                    inputSim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.CONTROL);
                }
                mAltKeyPressed = false;
                mAltKeyUsed = false;
            }
        }

        private static IntPtr GetTopMostHwnd(IntPtr hwnd) {
            return WinAPI.EnumWindows().FirstOrDefault(topLevelHwnd =>
                WinAPI.IsChild(topLevelHwnd, hwnd) ||
                topLevelHwnd == hwnd);
        }
    }
}
