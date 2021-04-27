using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseDragAction {
        enum Side {
            Left, Right, Top, Bottom, None
        }

        private readonly IntPtr mHwnd;
        private readonly Point mInitialMousePosition;
        private readonly Point mInitialWindowPosition;
        private readonly SnapRectangle mSnapRectangle;
        private readonly InvisibleWindow mInvisibleWindow;

        private const int MaxSnapDistancePx = 10;

        public MouseDragAction(IntPtr hwnd, Point initialMousePosition) {
            mHwnd = hwnd;
            mInitialMousePosition = initialMousePosition;

            var windowPlacement = WinAPI.GetPlacement(hwnd);
            if (windowPlacement == WinAPI.ShowWindowCommands.SW_MAXIMIZE) {
                WinAPI.ShowWindow(hwnd, WinAPI.ShowWindowCommands.SW_RESTORE);
            }

            if (!WinAPI.GetWindowRect(hwnd, out var windowRect)) {
                throw new InvalidOperationException(
                    $"Could not get window rect of window {hwnd}");
            }

            mInitialWindowPosition = new Point(windowRect.Left, windowRect.Top);

            mInvisibleWindow = new InvisibleWindow();
            mInvisibleWindow.CenterAt(initialMousePosition);
            mInvisibleWindow.Cursor = Cursors.Hand;
            mInvisibleWindow.Show();

            mSnapRectangle = new SnapRectangle() {
                Visible = false
            };
        }

        public void Update(Point currentMousePosition) {
            int deltaX = currentMousePosition.X - mInitialMousePosition.X;
            int deltaY = currentMousePosition.Y - mInitialMousePosition.Y;

            WinAPI.SetWindowPos(mHwnd, 0,
                mInitialWindowPosition.X + deltaX,
                mInitialWindowPosition.Y + deltaY,
                0, 0, WinAPI.SWP_NOSIZE | WinAPI.SWP_NOZORDER);

            var nearestSide = GetNearestSide(currentMousePosition);
            switch (nearestSide.Side) {
                case Side.Left:
                    mSnapRectangle.Width    = nearestSide.screen.WorkingArea.Width / 2;
                    mSnapRectangle.Height   = nearestSide.screen.WorkingArea.Height;
                    mSnapRectangle.Location = nearestSide.screen.WorkingArea.Location;
                    break;
                case Side.Top:
                    mSnapRectangle.Width    = nearestSide.screen.WorkingArea.Width;
                    mSnapRectangle.Height   = nearestSide.screen.WorkingArea.Height / 2;
                    mSnapRectangle.Location = nearestSide.screen.WorkingArea.Location;
                    break;
                case Side.Right:
                    mSnapRectangle.Width    = nearestSide.screen.WorkingArea.Width / 2;
                    mSnapRectangle.Height   = nearestSide.screen.WorkingArea.Height;
                    mSnapRectangle.Location = nearestSide.screen.WorkingArea.Location.Add(
                        nearestSide.screen.WorkingArea.Width / 2, 0);
                    break;
                case Side.Bottom:
                    mSnapRectangle.Width    = nearestSide.screen.WorkingArea.Width;
                    mSnapRectangle.Height   = nearestSide.screen.WorkingArea.Height / 2;
                    mSnapRectangle.Location = nearestSide.screen.WorkingArea.Location.Add(
                        0, nearestSide.screen.WorkingArea.Height / 2);
                    break;
                case Side.None:
                    break;
            }

            if (nearestSide.Side == Side.None) {
                mSnapRectangle.Hide();
            } else {
                mSnapRectangle.Show();
            }

            mInvisibleWindow.CenterAt(currentMousePosition);
        }

        static (Side Side, Screen screen, int Distance) GetNearestSide(Point currentMousePosition) {
            var screen = Screen.FromPoint(currentMousePosition.Add(-1, -1));

            var distToLeft   = Math.Abs(screen.Bounds.Left   - currentMousePosition.X);
            var distToRight  = Math.Abs(screen.Bounds.Right  - currentMousePosition.X);
            var distToTop    = Math.Abs(screen.Bounds.Top    - currentMousePosition.Y);
            var distToBottom = Math.Abs(screen.Bounds.Bottom - currentMousePosition.Y);

            var nearestToBorder = new[] {
                (Side: Side.Left,   Distance: distToLeft),
                (Side: Side.Right,  Distance: distToRight),
                (Side: Side.Top,    Distance: distToTop),
                (Side: Side.Bottom, Distance: distToBottom)
            }.OrderBy(x => x.Distance)
             .First();

            if (nearestToBorder.Distance > MaxSnapDistancePx) {
                nearestToBorder.Side = Side.None;
            }

            return (nearestToBorder.Side, screen, nearestToBorder.Distance);
        }

        public void Finish(Point currentMousePosition) {
            var nearestToBorder = GetNearestSide(currentMousePosition);

            switch (nearestToBorder.Side) {
                case Side.Left:
                    WinAPI.SetWindowPos(mHwnd, 0,
                        nearestToBorder.screen.WorkingArea.X,
                        nearestToBorder.screen.WorkingArea.Y,
                        nearestToBorder.screen.WorkingArea.Width / 2,
                        nearestToBorder.screen.WorkingArea.Height,
                        WinAPI.SWP_NOZORDER);
                    break;
                case Side.Top:
                    WinAPI.SetWindowPos(mHwnd, 0,
                        nearestToBorder.screen.WorkingArea.X,
                        nearestToBorder.screen.WorkingArea.Y,
                        nearestToBorder.screen.WorkingArea.Width,
                        nearestToBorder.screen.WorkingArea.Height / 2,
                        WinAPI.SWP_NOZORDER);
                    break;
                case Side.Right:
                    WinAPI.SetWindowPos(mHwnd, 0,
                        nearestToBorder.screen.WorkingArea.X
                            + nearestToBorder.screen.WorkingArea.Width / 2,
                        nearestToBorder.screen.WorkingArea.Y,
                        nearestToBorder.screen.WorkingArea.Width / 2,
                        nearestToBorder.screen.WorkingArea.Height,
                        WinAPI.SWP_NOZORDER);
                    break;
                case Side.Bottom:
                    WinAPI.SetWindowPos(mHwnd, 0,
                        nearestToBorder.screen.WorkingArea.X,
                        nearestToBorder.screen.WorkingArea.Y
                            + nearestToBorder.screen.WorkingArea.Height / 2,
                        nearestToBorder.screen.WorkingArea.Width,
                        nearestToBorder.screen.WorkingArea.Height / 2,
                        WinAPI.SWP_NOZORDER);
                    break;
            }

            mInvisibleWindow.Hide();
            mSnapRectangle.Hide();
        }
    }
}
