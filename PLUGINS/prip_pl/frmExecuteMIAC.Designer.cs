namespace prip_pl
{
    partial class frmExecuteMIAC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExecuteMIAC));
            this.label3 = new System.Windows.Forms.Label();
            this.lb1 = new System.Windows.Forms.ListBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btPath = new System.Windows.Forms.Button();
            this.tbPrip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(255, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Протокол загрузки информации от ГБУЗ МИАЦ:";
            // 
            // lb1
            // 
            this.lb1.FormattingEnabled = true;
            this.lb1.Location = new System.Drawing.Point(9, 107);
            this.lb1.Name = "lb1";
            this.lb1.ScrollAlwaysVisible = true;
            this.lb1.Size = new System.Drawing.Size(732, 550);
            this.lb1.TabIndex = 23;
            this.lb1.TabStop = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(99, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(163, 20);
            this.dateTimePicker1.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Дата сведений";
            // 
            // btPath
            // 
            this.btPath.Location = new System.Drawing.Point(711, 47);
            this.btPath.Name = "btPath";
            this.btPath.Size = new System.Drawing.Size(30, 23);
            this.btPath.TabIndex = 20;
            this.btPath.Text = "...";
            this.btPath.UseVisualStyleBackColor = true;
            this.btPath.Click += new System.EventHandler(this.btPath_Click_1);
            // 
            // tbPrip
            // 
            this.tbPrip.Location = new System.Drawing.Point(99, 50);
            this.tbPrip.Name = "tbPrip";
            this.tbPrip.ReadOnly = true;
            this.tbPrip.Size = new System.Drawing.Size(606, 20);
            this.tbPrip.TabIndex = 21;
            this.tbPrip.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Архив сведений";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(666, 668);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 17;
            this.btCancel.Text = "Закрыть";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btExecute
            // 
            this.btExecute.Enabled = false;
            this.btExecute.Image = ((System.Drawing.Image)(resources.GetObject("btExecute.Image")));
            this.btExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExecute.Location = new System.Drawing.Point(547, 668);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(109, 23);
            this.btExecute.TabIndex = 16;
            this.btExecute.Text = "Обработать";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.btExecute_Click);
            // 
            // frmExecuteMIAC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 703);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btPath);
            this.Controls.Add(this.tbPrip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btExecute);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExecuteMIAC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Обработка сведений от ГБУЗ МИАЦ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lb1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btPath;
        private System.Windows.Forms.TextBox tbPrip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btExecute;


    }
}