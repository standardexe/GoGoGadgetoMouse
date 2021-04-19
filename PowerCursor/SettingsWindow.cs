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
    public partial class SettingsWindow : Form {
        public SettingsWindow() {
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

        private void buttonAddExclude_Click(object sender, EventArgs e) {
            var result = Microsoft.VisualBasic.Interaction.InputBox(
                "Input window name to exlude (Regex):", "Exclude window");

            listBoxExclusions.Items.Add(result);
        }

        private void SettingsWindow_Shown(object sender, EventArgs e) {
            LoadSettings();
        }

        private void SaveSettings() {
            Properties.Settings.Default.sideWidth =
                (float)numericUpDownSideWidth.Value / 100;

            if (Properties.Settings.Default.excludes == null) {
                Properties.Settings.Default.excludes =
                    new System.Collections.Specialized.StringCollection();
            }

            Properties.Settings.Default.excludes.Clear();
            Properties.Settings.Default.excludes.AddRange(
                listBoxExclusions.Items
                    .ToEnumerable<string>()
                    .ToArray());

            Properties.Settings.Default.Save();
        }

        private void LoadSettings() {
            numericUpDownSideWidth.Value = (decimal)Math.Round(
                Properties.Settings.Default.sideWidth * 100);

            listBoxExclusions.Items.AddRange(
                Properties.Settings.Default.excludes
                    .ToEnumerable()
                    .ToArray());
        }

        private void buttonChangeExclude_Click(object sender, EventArgs e) {
            var result = Microsoft.VisualBasic.Interaction.InputBox(
                "Change window name (Regex allowed):", "Exclude window",
                (string)listBoxExclusions.SelectedItem);

            listBoxExclusions.Items[listBoxExclusions.SelectedIndex] = result;
        }

        private void buttonRemoveExclude_Click(object sender, EventArgs e) {
            listBoxExclusions.Items.RemoveAt(listBoxExclusions.SelectedIndex);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e) {
            if (DialogResult == DialogResult.OK) {
                SaveSettings();
            }
        }
    }
}
