using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseResizeAction {
        public enum ResizeMode {
            Left, Top, Right, Bottom,
            TopLeft, TopRight, BottomLeft, BottomRight,
            All
        }

        public static readonly Dictionary<ResizeMode, Cursor> Cursors
            = new Dictionary<ResizeMode, Cursor> {
                [ResizeMode.All]         = System.Windows.Forms.Cursors.SizeAll,
                [ResizeMode.Top]         = System.Windows.Forms.Cursors.SizeNS,
                [ResizeMode.Left]        = System.Windows.Forms.Cursors.SizeWE,
                [ResizeMode.Right]       = System.Windows.Forms.Cursors.SizeWE,
                [ResizeMode.Bottom]      = System.Windows.Forms.Cursors.SizeNS,
                [ResizeMode.TopLeft]     = System.Windows.Forms.Cursors.SizeNWSE,
                [ResizeMode.TopRight]    = System.Windows.Forms.Cursors.SizeNESW,
                [ResizeMode.BottomLeft]  = System.Windows.Forms.Cursors.SizeNESW,
                [ResizeMode.BottomRight] = System.Windows.Forms.Cursors.SizeNWSE,
            };

        private readonly InvisibleWindow mInvisibleWindow;
        private readonly Rectangle mInitialWindowRect;
        private readonly Point mInitialMousePosition;
        private readonly ResizeMode mResizeMode;
        private readonly Timer mUpdateTimer;
        private readonly IntPtr mHwnd;

        private Rectangle mNewWindowRect;

        public MouseResizeAction(IntPtr hwnd, Point initialMousePosition) {
            mInitialMousePosition = initialMousePosition;
            mHwnd = hwnd;

            if (!WinAPI.GetWindowRect(hwnd, out var rect)) {
                throw new InvalidOperationException($"Could not get window rect of window {hwnd}");
            }

            var cornerSize = Properties.Settings.Default.sideWidth / 2;
            var sideSize = cornerSize;

            mInitialWindowRect = mNewWindowRect =
                new Rectangle(rect.Left, rect.Top,
                              rect.Right - rect.Left,
                              rect.Bottom - rect.Top);

            var normalizedMousePos = new PointF(
                (initialMousePosition.X - mInitialWindowRect.X)
                    / (float)mInitialWindowRect.Width,
                (initialMousePosition.Y - mInitialWindowRect.Y)
                    / (float)mInitialWindowRect.Height);

            var areas = new Dictionary<ResizeMode, RectangleF> {
                [ResizeMode.TopLeft]     = new RectangleF(0, 0, cornerSize, cornerSize),
                [ResizeMode.TopRight]    = new RectangleF(1 - cornerSize, 0, cornerSize, cornerSize),
                [ResizeMode.BottomLeft]  = new RectangleF(0, 1 - cornerSize, cornerSize, cornerSize),
                [ResizeMode.BottomRight] = new RectangleF(1 - cornerSize, 1 - cornerSize, cornerSize, cornerSize),

                [ResizeMode.Top]         = new RectangleF(0, 0, 1, sideSize),
                [ResizeMode.Left]        = new RectangleF(0, 0, sideSize, 1),
                [ResizeMode.Right]       = new RectangleF(1 - sideSize, 0, sideSize, 1),
                [ResizeMode.Bottom]      = new RectangleF(0, 1 - sideSize, 1, sideSize),
            };

            if (areas.TryFirstOrDefault(kvp => kvp.Value.Contains(normalizedMousePos), out var areaKvp)) {
                mResizeMode = areaKvp.Key;
            } else {
                mResizeMode = ResizeMode.All;
            }

            mInvisibleWindow = new InvisibleWindow();
            mInvisibleWindow.CenterAt(initialMousePosition);
            mInvisibleWindow.Cursor = Cursors[mResizeMode];
            mInvisibleWindow.Show();

            mUpdateTimer = new Timer();
            mUpdateTimer.Interval = 100;
            mUpdateTimer.Tick += UpdateTimer_Tick;
            mUpdateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e) {
            WinAPI.SetWindowPos(mHwnd, 0,
                mNewWindowRect.X,
                mNewWindowRect.Y,
                mNewWindowRect.Width,
                mNewWindowRect.Height,
                WinAPI.SWP_NOZORDER);
        }

        public void Update(Point currentMousePosition) {
            mInvisibleWindow.CenterAt(currentMousePosition);

            var deltaX = currentMousePosition.X - mInitialMousePosition.X;
            var deltaY = currentMousePosition.Y - mInitialMousePosition.Y;

            switch (mResizeMode) {
                case ResizeMode.Left:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX, 
                        mInitialWindowRect.Top, 
                        mInitialWindowRect.Width - deltaX, 
                        mInitialWindowRect.Height);
                    break;
                case ResizeMode.Top:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.Right:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height);
                    break;
                case ResizeMode.Bottom:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.TopLeft:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width - deltaX,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.TopRight:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.BottomLeft:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width - deltaX,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.BottomRight:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.All:
                default:
                    mNewWindowRect = new Rectangle(
                        mInitialWindowRect.Left - deltaX,
                        mInitialWindowRect.Top - deltaY,
                        mInitialWindowRect.Width + 2*deltaX,
                        mInitialWindowRect.Height + 2*deltaY);
                    break;
            }
        }

        public void Finish(Point currentMousePosition) {
            mUpdateTimer.Stop();
            mInvisibleWindow.Hide();
            UpdateTimer_Tick(null, null);
        }
    }
}
