using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoGoGadgetoMouse {
    class SnapRectangle : Form {
        public SnapRectangle() {
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.ControlBox = false;
            this.Text = "";
        }

        public Color RectangleColor { get; set; } = Color.Red;

        protected override void OnPaintBackground(PaintEventArgs e) {
            // empty
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (var pen = new Pen(RectangleColor, 5)) {
                e.Graphics.DrawRectangle(pen, ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}
