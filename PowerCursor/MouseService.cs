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
            DragWindow
        }

        private bool mAltKeyPressed;

        private State mCurrentState;
        private MouseDragAction mDragAction;

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
            if (mCurrentState == State.DragWindow) {
                mDragAction?.Update(e.Location);
            }
        }

        private void OnMouseUp(object sender, MouseInterceptor.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && mCurrentState != State.None) {
                mCurrentState = State.None;
                mDragAction = null;
                e.Handled = true;
            }
        }

        private void OnMouseDown(object sender, MouseInterceptor.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && mAltKeyPressed) {
                var hwndMouseOver = WinAPI.WindowFromPoint(e.Location);
                var topLevelHWnd = WinAPI.EnumWindows().First(hwnd =>
                    WinAPI.IsChild(hwnd, hwndMouseOver) ||
                    hwnd == hwndMouseOver);

                mDragAction = new MouseDragAction(topLevelHWnd, e.Location);
                mCurrentState = State.DragWindow;
                e.Handled = true;
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
