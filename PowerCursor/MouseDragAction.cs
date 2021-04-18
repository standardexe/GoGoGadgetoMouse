using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PowerCursor {
    class MouseDragAction {
        private readonly IntPtr mHwnd;
        private readonly Point mInitialWindowPosition;
        private readonly Point mInitialMousePosition;

        public MouseDragAction(IntPtr hwnd, Point initialMousePosition) {
            mHwnd = hwnd;
            mInitialMousePosition = initialMousePosition;

            // TODO: look into DwmGetWindowAttribute to get coordinates
            // without drop shadow and not adjusted for DPI
            if (!WinAPI.GetWindowRect(hwnd, out var windowRect)) {
                throw new InvalidOperationException($"Could not get window rect of window {hwnd}");
            }

            mInitialWindowPosition = new Point(windowRect.Left, windowRect.Top);
        }

        public void Update(Point currentMousePosition) {
            int deltaX = currentMousePosition.X - mInitialMousePosition.X;
            int deltaY = currentMousePosition.Y - mInitialMousePosition.Y;

            WinAPI.SetWindowPos(mHwnd, 0,
                mInitialWindowPosition.X + deltaX,
                mInitialWindowPosition.Y + deltaY,
                0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER | WinAPI.SWP_SHOWWINDOW);
        }
    }
}
