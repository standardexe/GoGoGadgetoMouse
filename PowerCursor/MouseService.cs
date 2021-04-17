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
        private Point mDragInitialPoint;
        private Point mDragWindowPosition;
        private IntPtr mDragHwnd;

        private State mCurrentState;

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

        private void OnMouseMove(object sender, MouseEventArgs e) {
            if (mCurrentState == State.DragWindow) {
                WinAPI.SetWindowPos(mDragHwnd, 0, 
                    mDragWindowPosition.X + (e.X - mDragInitialPoint.X),
                    mDragWindowPosition.Y + (e.Y - mDragInitialPoint.Y), 
                    0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER);
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                mDragWindowPosition = Point.Empty;
                mDragInitialPoint = Point.Empty;
                mCurrentState = State.None;
                mDragHwnd = IntPtr.Zero;
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && mAltKeyPressed) {
                mDragHwnd = WinAPI.GetTopLevelHwnd(WinAPI.WindowFromPoint(e.Location));
                
                if (!WinAPI.GetWindowRect(mDragHwnd, out var windowRect)) {
                    Console.WriteLine($"ERROR: Could not read window rect for hwnd {mDragHwnd}!");
                }
                mDragWindowPosition = new Point(windowRect.Left, windowRect.Top);

                mCurrentState = State.DragWindow;
                mDragInitialPoint = e.Location;
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
