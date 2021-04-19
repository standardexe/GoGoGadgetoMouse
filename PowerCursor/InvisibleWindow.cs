using System.Drawing;
using System.Windows.Forms;

namespace PowerCursor {
    class InvisibleWindow : Form {
        public InvisibleWindow() {
            FormBorderStyle = FormBorderStyle.None;
            AutoScaleMode = AutoScaleMode.Dpi;
            ShowInTaskbar = false;
            ControlBox = false;
            TopMost = true;
            Opacity = 0.01;
            Height = 100;
            Width = 100;
        }

        public void CenterAt(Point location) {
            Location = new Point(location.X - Width / 2, location.Y - Height / 2);
        }
    }
}
