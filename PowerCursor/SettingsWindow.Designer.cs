
namespace GoGoGadgetoMouse
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageResizer = new System.Windows.Forms.TabPage();
            this.labelPercent = new System.Windows.Forms.Label();
            this.numericUpDownSideWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPageLists = new System.Windows.Forms.TabPage();
            this.buttonChangeExclude = new System.Windows.Forms.Button();
            this.buttonRemoveExclude = new System.Windows.Forms.Button();
            this.buttonAddExclude = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxExclusions = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPageResizer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSideWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPageLists.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageResizer);
            this.tabControl.Controls.Add(this.tabPageLists);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(616, 424);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageResizer
            // 
            this.tabPageResizer.Controls.Add(this.labelPercent);
            this.tabPageResizer.Controls.Add(this.numericUpDownSideWidth);
            this.tabPageResizer.Controls.Add(this.label2);
            this.tabPageResizer.Controls.Add(this.pictureBox1);
            this.tabPageResizer.Location = new System.Drawing.Point(4, 22);
            this.tabPageResizer.Name = "tabPageResizer";
            this.tabPageResizer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResizer.Size = new System.Drawing.Size(608, 398);
            this.tabPageResizer.TabIndex = 0;
            this.tabPageResizer.Text = "Resizing";
            this.tabPageResizer.UseVisualStyleBackColor = true;
            // 
            // labelPercent
            // 
            this.labelPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPercent.AutoSize = true;
            this.labelPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPercent.Location = new System.Drawing.Point(502, 36);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(16, 13);
            this.labelPercent.TabIndex = 7;
            this.labelPercent.Text = "%";
            // 
            // numericUpDownSideWidth
            // 
            this.numericUpDownSideWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownSideWidth.Location = new System.Drawing.Point(451, 34);
            this.numericUpDownSideWidth.Name = "numericUpDownSideWidth";
            this.numericUpDownSideWidth.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownSideWidth.TabIndex = 5;
            this.numericUpDownSideWidth.Value = new decimal(new int[] {
            66,
            0,
            0,
            0});
            this.numericUpDownSideWidth.ValueChanged += new System.EventHandler(this.numericUpDownSideWidth_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(448, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Side width:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(9, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 322);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.Resize += new System.EventHandler(this.pictureBox1_Resize);
            // 
            // tabPageLists
            // 
            this.tabPageLists.Controls.Add(this.buttonChangeExclude);
            this.tabPageLists.Controls.Add(this.buttonRemoveExclude);
            this.tabPageLists.Controls.Add(this.buttonAddExclude);
            this.tabPageLists.Controls.Add(this.label1);
            this.tabPageLists.Controls.Add(this.listBoxExclusions);
            this.tabPageLists.Location = new System.Drawing.Point(4, 22);
            this.tabPageLists.Name = "tabPageLists";
            this.tabPageLists.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLists.Size = new System.Drawing.Size(608, 398);
            this.tabPageLists.TabIndex = 1;
            this.tabPageLists.Text = "Excludes";
            this.tabPageLists.UseVisualStyleBackColor = true;
            // 
            // buttonChangeExclude
            // 
            this.buttonChangeExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChangeExclude.Location = new System.Drawing.Point(437, 77);
            this.buttonChangeExclude.Name = "buttonChangeExclude";
            this.buttonChangeExclude.Size = new System.Drawing.Size(163, 36);
            this.buttonChangeExclude.TabIndex = 4;
            this.buttonChangeExclude.Text = "Change selected...";
            this.buttonChangeExclude.UseVisualStyleBackColor = true;
            this.buttonChangeExclude.Click += new System.EventHandler(this.buttonChangeExclude_Click);
            // 
            // buttonRemoveExclude
            // 
            this.buttonRemoveExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveExclude.Location = new System.Drawing.Point(437, 119);
            this.buttonRemoveExclude.Name = "buttonRemoveExclude";
            this.buttonRemoveExclude.Size = new System.Drawing.Size(163, 36);
            this.buttonRemoveExclude.TabIndex = 3;
            this.buttonRemoveExclude.Text = "Remove selected";
            this.buttonRemoveExclude.UseVisualStyleBackColor = true;
            this.buttonRemoveExclude.Click += new System.EventHandler(this.buttonRemoveExclude_Click);
            // 
            // buttonAddExclude
            // 
            this.buttonAddExclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddExclude.Location = new System.Drawing.Point(437, 35);
            this.buttonAddExclude.Name = "buttonAddExclude";
            this.buttonAddExclude.Size = new System.Drawing.Size(163, 36);
            this.buttonAddExclude.TabIndex = 2;
            this.buttonAddExclude.Text = "Add new...";
            this.buttonAddExclude.UseVisualStyleBackColor = true;
            this.buttonAddExclude.Click += new System.EventHandler(this.buttonAddExclude_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Exclude window names:";
            // 
            // listBoxExclusions
            // 
            this.listBoxExclusions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxExclusions.FormattingEnabled = true;
            this.listBoxExclusions.Location = new System.Drawing.Point(8, 35);
            this.listBoxExclusions.Name = "listBoxExclusions";
            this.listBoxExclusions.Size = new System.Drawing.Size(423, 251);
            this.listBoxExclusions.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 424);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 39);
            this.panel1.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(379, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(112, 30);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(497, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(616, 463);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SettingsWindow";
            this.ShowIcon = false;
            this.Text = "Settings - GoGoGadgetoMouse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.Shown += new System.EventHandler(this.SettingsWindow_Shown);
            this.tabControl.ResumeLayout(false);
            this.tabPageResizer.ResumeLayout(false);
            this.tabPageResizer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSideWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPageLists.ResumeLayout(false);
            this.tabPageLists.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageResizer;
        private System.Windows.Forms.TabPage tabPageLists;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxExclusions;
        private System.Windows.Forms.Button buttonRemoveExclude;
        private System.Windows.Forms.Button buttonAddExclude;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.NumericUpDown numericUpDownSideWidth;
        private System.Windows.Forms.Button buttonChangeExclude;
    }
}

