namespace webService
{
    partial class frmLoadFromExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoadFromExcel));
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbProtocol = new System.Windows.Forms.ListBox();
            this.btFiles = new System.Windows.Forms.Button();
            this.tbFiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPostav = new System.Windows.Forms.ComboBox();
            this.chbIntellect = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(546, 349);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(627, 349);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbProtocol
            // 
            this.lbProtocol.FormattingEnabled = true;
            this.lbProtocol.Location = new System.Drawing.Point(12, 58);
            this.lbProtocol.Name = "lbProtocol";
            this.lbProtocol.Size = new System.Drawing.Size(690, 277);
            this.lbProtocol.TabIndex = 9;
            // 
            // btFiles
            // 
            this.btFiles.Location = new System.Drawing.Point(676, 29);
            this.btFiles.Name = "btFiles";
            this.btFiles.Size = new System.Drawing.Size(26, 23);
            this.btFiles.TabIndex = 8;
            this.btFiles.Text = "...";
            this.btFiles.UseVisualStyleBackColor = true;
            this.btFiles.Click += new System.EventHandler(this.btFiles_Click);
            // 
            // tbFiles
            // 
            this.tbFiles.Location = new System.Drawing.Point(82, 32);
            this.tbFiles.Name = "tbFiles";
            this.tbFiles.Size = new System.Drawing.Size(588, 20);
            this.tbFiles.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Файл:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Поставщик:";
            // 
            // cbPostav
            // 
            this.cbPostav.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPostav.FormattingEnabled = true;
            this.cbPostav.Location = new System.Drawing.Point(82, 5);
            this.cbPostav.Name = "cbPostav";
            this.cbPostav.Size = new System.Drawing.Size(620, 21);
            this.cbPostav.TabIndex = 11;
            // 
            // chbIntellect
            // 
            this.chbIntellect.AutoSize = true;
            this.chbIntellect.Location = new System.Drawing.Point(12, 357);
            this.chbIntellect.Name = "chbIntellect";
            this.chbIntellect.Size = new System.Drawing.Size(514, 17);
            this.chbIntellect.TabIndex = 12;
            this.chbIntellect.Text = "при сопоставлении товара использователь интеллектуальный выбор эталонной номенкла" +
    "туры";
            this.chbIntellect.UseVisualStyleBackColor = true;
            // 
            // frmLoadFromExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 384);
            this.Controls.Add(this.chbIntellect);
            this.Controls.Add(this.cbPostav);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbProtocol);
            this.Controls.Add(this.btFiles);
            this.Controls.Add(this.tbFiles);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLoadFromExcel";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Импорт и обработка информации";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ListBox lbProtocol;
        private System.Windows.Forms.Button btFiles;
        private System.Windows.Forms.TextBox tbFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPostav;
        private System.Windows.Forms.CheckBox chbIntellect;
    }
}