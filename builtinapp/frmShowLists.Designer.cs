namespace GGPlatform.BuiltInApp
{
    partial class frmShowLists
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowLists));
            this.clb = new System.Windows.Forms.CheckedListBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // clb
            // 
            this.clb.CheckOnClick = true;
            this.clb.Dock = System.Windows.Forms.DockStyle.Top;
            this.clb.FormattingEnabled = true;
            this.clb.Location = new System.Drawing.Point(0, 0);
            this.clb.Name = "clb";
            this.clb.Size = new System.Drawing.Size(440, 394);
            this.clb.TabIndex = 0;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(273, 405);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(354, 405);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // cbCheck
            // 
            this.cbCheck.AutoSize = true;
            this.cbCheck.Checked = true;
            this.cbCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheck.Location = new System.Drawing.Point(12, 412);
            this.cbCheck.Name = "cbCheck";
            this.cbCheck.Size = new System.Drawing.Size(100, 17);
            this.cbCheck.TabIndex = 4;
            this.cbCheck.Text = "снять отметки";
            this.cbCheck.UseVisualStyleBackColor = true;
            this.cbCheck.CheckedChanged += new System.EventHandler(this.cbCheck_CheckedChanged);
            // 
            // frmShowLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 440);
            this.Controls.Add(this.cbCheck);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.clb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShowLists";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Текущие списки программы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clb;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox cbCheck;
    }
}