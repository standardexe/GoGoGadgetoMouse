using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                [ResizeMode.All] = System.Windows.Forms.Cursors.SizeAll,
                [ResizeMode.Left] = System.Windows.Forms.Cursors.SizeWE,
                [ResizeMode.Right] = System.Windows.Forms.Cursors.SizeWE,
                [ResizeMode.Top] = System.Windows.Forms.Cursors.SizeNS,
                [ResizeMode.Bottom] = System.Windows.Forms.Cursors.SizeNS,
                [ResizeMode.TopLeft] = System.Windows.Forms.Cursors.SizeNWSE,
                [ResizeMode.BottomRight] = System.Windows.Forms.Cursors.SizeNWSE,
                [ResizeMode.TopRight] = System.Windows.Forms.Cursors.SizeNESW,
                [ResizeMode.BottomLeft] = System.Windows.Forms.Cursors.SizeNESW,
            };

        private readonly Rectangle mInitialWindowRect;
        private readonly Point mInitialMousePosition;
        private readonly ResizeMode mResizeMode;
        private readonly IntPtr mHwnd;

        private readonly InvisibleWindow mInvisibleWindow;

        public MouseResizeAction(IntPtr hwnd, Point initialMousePosition) {
            mInitialMousePosition = initialMousePosition;
            mHwnd = hwnd;

            if (!WinAPI.GetWindowRect(hwnd, out var rect)) {
                throw new InvalidOperationException($"Could not get window rect of window {hwnd}");
            }

            var cornerSize = Properties.Settings.Default.sideWidth / 2;
            var sideSize = cornerSize;

            mInitialWindowRect = new Rectangle(
                rect.Left, rect.Top,
                rect.Right - rect.Left,
                rect.Bottom - rect.Top);

            var normalizedMousePos = new PointF(
                (initialMousePosition.X - mInitialWindowRect.X) / (float)mInitialWindowRect.Width,
                (initialMousePosition.Y - mInitialWindowRect.Y) / (float)mInitialWindowRect.Height);

            var areas = new Dictionary<ResizeMode, RectangleF> {
                [ResizeMode.TopLeft] = new RectangleF(0, 0, cornerSize, cornerSize),
                [ResizeMode.TopRight] = new RectangleF(1 - cornerSize, 0, cornerSize, cornerSize),
                [ResizeMode.BottomLeft] = new RectangleF(0, 1 - cornerSize, cornerSize, cornerSize),
                [ResizeMode.BottomRight] = new RectangleF(1 - cornerSize, 1 - cornerSize, cornerSize, cornerSize),

                [ResizeMode.Left] = new RectangleF(0, 0, sideSize, 1),
                [ResizeMode.Top] = new RectangleF(0, 0, 1, sideSize),
                [ResizeMode.Right] = new RectangleF(1 - sideSize, 0, sideSize, 1),
                [ResizeMode.Bottom] = new RectangleF(0, 1 - sideSize, 1, sideSize),
            };

            if (areas.TryFirstOrDefault(kvp => kvp.Value.Contains(normalizedMousePos), out var areaKvp)) {
                mResizeMode = areaKvp.Key;
            } else {
                mResizeMode = ResizeMode.All;
            }

            mInvisibleWindow = new InvisibleWindow();
            mInvisibleWindow.Show();
            mInvisibleWindow.CenterAt(initialMousePosition);
            mInvisibleWindow.Cursor = Cursors[mResizeMode];
        }

        public void Update(Point currentMousePosition) {
            mInvisibleWindow.CenterAt(currentMousePosition);

            var deltaX = currentMousePosition.X - mInitialMousePosition.X;
            var deltaY = currentMousePosition.Y - mInitialMousePosition.Y;

            Rectangle newWindowRect;

            switch (mResizeMode) {
                case ResizeMode.Left:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX, 
                        mInitialWindowRect.Top, 
                        mInitialWindowRect.Width - deltaX, 
                        mInitialWindowRect.Height);
                    break;
                case ResizeMode.Top:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.Right:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height);
                    break;
                case ResizeMode.Bottom:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.TopLeft:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width - deltaX,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.TopRight:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top + deltaY,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height - deltaY);
                    break;
                case ResizeMode.BottomLeft:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left + deltaX,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width - deltaX,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.BottomRight:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left,
                        mInitialWindowRect.Top,
                        mInitialWindowRect.Width + deltaX,
                        mInitialWindowRect.Height + deltaY);
                    break;
                case ResizeMode.All:
                default:
                    newWindowRect = new Rectangle(
                        mInitialWindowRect.Left - deltaX,
                        mInitialWindowRect.Top - deltaY,
                        mInitialWindowRect.Width + 2*deltaX,
                        mInitialWindowRect.Height + 2*deltaY);
                    break;
            }

            WinAPI.SetWindowPos(mHwnd, 0,
                newWindowRect.X,
                newWindowRect.Y,
                newWindowRect.Width,
                newWindowRect.Height,
                WinAPI.SWP_NOZORDER);
        }

        public void Finish(Point currentMousePosition) {
            mInvisibleWindow.Hide();
        }
    }
}
