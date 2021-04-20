using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseDragAction {
        private readonly IntPtr mHwnd;
        private readonly Point mInitialWindowPosition;
        private readonly Point mInitialMousePosition;
        private readonly InvisibleWindow mInvisibleWindow;

        public MouseDragAction(IntPtr hwnd, Point initialMousePosition) {
            mHwnd = hwnd;
            mInitialMousePosition = initialMousePosition;

            if (!WinAPI.GetWindowRect(hwnd, out var windowRect)) {
                throw new InvalidOperationException(
                    $"Could not get window rect of window {hwnd}");
            }

            mInitialWindowPosition = new Point(windowRect.Left, windowRect.Top);

            IntPtr focusedHwnd = WinAPI.GetFocus();
            mInvisibleWindow = new InvisibleWindow();
            mInvisibleWindow.Show();
            mInvisibleWindow.CenterAt(initialMousePosition);
            mInvisibleWindow.Cursor = Cursors.Hand;
            WinAPI.SetFocus(focusedHwnd);
        }

        public void Update(Point currentMousePosition) {
            int deltaX = currentMousePosition.X - mInitialMousePosition.X;
            int deltaY = currentMousePosition.Y - mInitialMousePosition.Y;

            WinAPI.SetWindowPos(mHwnd, 0,
                mInitialWindowPosition.X + deltaX,
                mInitialWindowPosition.Y + deltaY,
                0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER);

            mInvisibleWindow.CenterAt(currentMousePosition);
        }

        public void Finish(Point currentMousePosition) {
            mInvisibleWindow.Hide();
        }
    }
}
