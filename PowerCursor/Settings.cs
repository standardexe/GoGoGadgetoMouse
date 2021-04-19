using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    public partial class Settings : Form {
        public Settings() {
            InitializeComponent();
        }

        public void DrawResizers(Graphics g, float sidePercent) {
            var width = g.VisibleClipBounds.Width;
            var height = g.VisibleClipBounds.Height;
            var sideWidth = width * sidePercent / 2;
            var sideHeight = height * sidePercent / 2;

            g.Clear(Color.DarkBlue);
            
            g.FillRectangle(Brushes.AliceBlue, 0, 0, width, sideHeight);
            g.FillRectangle(Brushes.AliceBlue, 0, 0, sideWidth, height);
            g.FillRectangle(Brushes.AliceBlue, width - sideWidth, 0, sideWidth, height);
            g.FillRectangle(Brushes.AliceBlue, 0, height - sideHeight, width, sideHeight);

            g.DrawLine(Pens.DarkGray, 0, sideHeight, width, sideHeight);
            g.DrawLine(Pens.DarkGray, 0, height - sideHeight, width, height - sideHeight);
            g.DrawLine(Pens.DarkGray, sideWidth, 0, sideWidth, height);
            g.DrawLine(Pens.DarkGray, width - sideWidth, 0, width - sideWidth, height);

            g.FillRectangle(Brushes.LightBlue, 0, 0, sideWidth, sideHeight);
            g.FillRectangle(Brushes.LightBlue, width  - sideWidth, 0, sideWidth, sideHeight);
            g.FillRectangle(Brushes.LightBlue, 0, height - sideHeight, sideWidth, sideHeight);
            g.FillRectangle(Brushes.LightBlue, width - sideWidth, height - sideHeight, sideWidth, sideHeight);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            DrawResizers(e.Graphics, (float)numericUpDownSideWidth.Value / 100f);
        }

        private void pictureBox1_Resize(object sender, EventArgs e) {
            pictureBox1.Invalidate();
        }

        private void numericUpDownSideWidth_ValueChanged(object sender, EventArgs e) {
            pictureBox1.Invalidate();
        }

        private void numericUpDownsideWidth_ValueChanged(object sender, EventArgs e) {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            var percent = (float)numericUpDownSideWidth.Value / 100f;
            var width = pictureBox1.ClientSize.Width;
            var height = pictureBox1.ClientSize.Height;
            var sideWidth = width * percent / 2;
            var sideHeight = height * percent / 2;

            var rectangles = new Dictionary<MouseResizeAction.ResizeMode, RectangleF> {
                { MouseResizeAction.ResizeMode.TopLeft, new RectangleF(0, 0, sideWidth, sideHeight) },
                { MouseResizeAction.ResizeMode.TopRight, new RectangleF(width - sideWidth, 0, sideWidth, sideHeight) },
                { MouseResizeAction.ResizeMode.BottomLeft, new RectangleF(0, height - sideHeight, sideWidth, sideHeight) },
                { MouseResizeAction.ResizeMode.BottomRight, new RectangleF(width - sideWidth, height - sideHeight, sideWidth, sideHeight) },
                { MouseResizeAction.ResizeMode.Top, new RectangleF(0, 0, width, sideHeight) },
                { MouseResizeAction.ResizeMode.Left, new RectangleF(0, 0, sideWidth, height) },
                { MouseResizeAction.ResizeMode.Right, new RectangleF(width - sideWidth, 0, sideWidth, height) },
                { MouseResizeAction.ResizeMode.Bottom, new RectangleF(0, height - sideHeight, width, sideHeight) },
            };

            var cursor = MouseResizeAction.Cursors[MouseResizeAction.ResizeMode.All];

            if (rectangles.TryFirstOrDefault(rc => rc.Value.Contains(e.Location), out var kvpRc)) {
                cursor = MouseResizeAction.Cursors[kvpRc.Key];
            }

            pictureBox1.Cursor = cursor;
        }
    }
}
