using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class MouseResizeAction {
        private enum ResizeMode {
            Left, Top, Right, Bottom,
            TopLeft, TopRight, BottomLeft, BottomRight,
            All
        }

        private const float CornerSize = 0.3f;
        private const float SideSize = 0.2f;

        private readonly Dictionary<ResizeMode, RectangleF> mAreas
            = new Dictionary<ResizeMode, RectangleF> {
                [ResizeMode.TopLeft] = new RectangleF(0, 0, CornerSize, CornerSize),
                [ResizeMode.TopRight] = new RectangleF(1 - CornerSize, 0, CornerSize, CornerSize),
                [ResizeMode.BottomLeft] = new RectangleF(0, 1 - CornerSize, CornerSize, CornerSize),
                [ResizeMode.BottomRight] = new RectangleF(1 - CornerSize, 1 - CornerSize, CornerSize, CornerSize),

                [ResizeMode.Left] = new RectangleF(0, 0, SideSize, 1),
                [ResizeMode.Top] = new RectangleF(0, 0, 1, SideSize),
                [ResizeMode.Right] = new RectangleF(1 - SideSize, 0, SideSize, 1),
                [ResizeMode.Bottom] = new RectangleF(0, 1 - SideSize, 1, SideSize),
            };

        private readonly Dictionary<ResizeMode, Cursor> mCursors
            = new Dictionary<ResizeMode, Cursor> {
                [ResizeMode.All] = Cursors.SizeAll,
                [ResizeMode.Left] = Cursors.SizeWE,
                [ResizeMode.Right] = Cursors.SizeWE,
                [ResizeMode.Top] = Cursors.SizeNS,
                [ResizeMode.Bottom] = Cursors.SizeNS,
                [ResizeMode.TopLeft] = Cursors.SizeNWSE,
                [ResizeMode.BottomRight] = Cursors.SizeNWSE,
                [ResizeMode.TopRight] = Cursors.SizeNESW,
                [ResizeMode.BottomLeft] = Cursors.SizeNESW,
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

            mInitialWindowRect = new Rectangle(
                rect.Left, rect.Top,
                rect.Right - rect.Left,
                rect.Bottom - rect.Top);

            var normalizedMousePos = new PointF(
                (initialMousePosition.X - mInitialWindowRect.X) / (float)mInitialWindowRect.Width,
                (initialMousePosition.Y - mInitialWindowRect.Y) / (float)mInitialWindowRect.Height);

            if (mAreas.TryFirstOrDefault(kvp => kvp.Value.Contains(normalizedMousePos), out var areaKvp)) {
                mResizeMode = areaKvp.Key;
            } else {
                mResizeMode = ResizeMode.All;
            }

            mInvisibleWindow = new InvisibleWindow();
            mInvisibleWindow.Show();
            mInvisibleWindow.CenterAt(initialMousePosition);
            mInvisibleWindow.Cursor = mCursors[mResizeMode];
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
