namespace GGPlatform.UserControls
{
    partial class ReferenceEditingControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btOK = new System.Windows.Forms.Button();
            this.tbEdit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btOK.Location = new System.Drawing.Point(307, 0);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(26, 21);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "...";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // tbEdit
            // 
            this.tbEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEdit.Location = new System.Drawing.Point(0, 0);
            this.tbEdit.Name = "tbEdit";
            this.tbEdit.Size = new System.Drawing.Size(307, 20);
            this.tbEdit.TabIndex = 1;
            this.tbEdit.TextChanged += new System.EventHandler(this.tbEdit_TextChanged);
            // 
            // ReferenceEditingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbEdit);
            this.Controls.Add(this.btOK);
            this.Name = "ReferenceEditingControl";
            this.Size = new System.Drawing.Size(333, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.TextBox tbEdit;
    }
}
