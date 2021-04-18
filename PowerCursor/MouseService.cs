using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PowerCursor {
    class MouseService {
        private static MouseService mInstance;

        private enum State {
            None,
            DragWindow,
            ResizeWindow
        }

        private bool mAltKeyPressed;

        private State mCurrentState;
        private MouseDragAction mDragAction;
        private MouseResizeAction mResizeAction;

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
                mDragAction = null;
                e.Handled = true;
            } else if (e.Button == MouseButtons.Right && mCurrentState == State.ResizeWindow) {
                mCurrentState = State.None;
                mResizeAction = null;
                e.Handled = true;
            }
        }

        private void OnMouseDown(object sender, MouseInterceptor.MouseEventArgs e) {
            if (mAltKeyPressed && mCurrentState == State.None) {
                var hwndMouseOver = WinAPI.WindowFromPoint(e.Location);
                var topLevelHWnd = WinAPI.EnumWindows().First(hwnd =>
                    WinAPI.IsChild(hwnd, hwndMouseOver) ||
                    hwnd == hwndMouseOver);

                if (e.Button == MouseButtons.Left) {
                    mDragAction = new MouseDragAction(topLevelHWnd, e.Location);
                    mCurrentState = State.DragWindow;
                    e.Handled = true;
                } else if (e.Button == MouseButtons.Right) {
                    mResizeAction = new MouseResizeAction(topLevelHWnd, e.Location);
                    mCurrentState = State.ResizeWindow;
                    e.Handled = true;
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LMenu) {
                mAltKeyPressed = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.LMenu) {
                mAltKeyPressed = false;
            }
        }
    }
}
