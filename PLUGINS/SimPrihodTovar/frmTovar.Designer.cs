namespace SimPrihodT
{
    partial class frmTovar
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpFiles = new System.Windows.Forms.TabPage();
            this.lbProtocol = new System.Windows.Forms.ListBox();
            this.btFiles = new System.Windows.Forms.Button();
            this.tbFiles = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tpNumbers = new System.Windows.Forms.TabPage();
            this.tbLast = new System.Windows.Forms.TextBox();
            this.tbFirst = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tpFiles.SuspendLayout();
            this.tpNumbers.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpFiles);
            this.tabControl1.Controls.Add(this.tpNumbers);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(408, 244);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpFiles
            // 
            this.tpFiles.Controls.Add(this.lbProtocol);
            this.tpFiles.Controls.Add(this.btFiles);
            this.tpFiles.Controls.Add(this.tbFiles);
            this.tpFiles.Controls.Add(this.label1);
            this.tpFiles.Location = new System.Drawing.Point(4, 22);
            this.tpFiles.Name = "tpFiles";
            this.tpFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tpFiles.Size = new System.Drawing.Size(400, 218);
            this.tpFiles.TabIndex = 0;
            this.tpFiles.Text = "Приход товара из файла";
            this.tpFiles.UseVisualStyleBackColor = true;
            // 
            // lbProtocol
            // 
            this.lbProtocol.FormattingEnabled = true;
            this.lbProtocol.Location = new System.Drawing.Point(8, 32);
            this.lbProtocol.Name = "lbProtocol";
            this.lbProtocol.Size = new System.Drawing.Size(384, 134);
            this.lbProtocol.TabIndex = 3;
            // 
            // btFiles
            // 
            this.btFiles.Location = new System.Drawing.Point(366, 4);
            this.btFiles.Name = "btFiles";
            this.btFiles.Size = new System.Drawing.Size(26, 23);
            this.btFiles.TabIndex = 2;
            this.btFiles.Text = "...";
            this.btFiles.UseVisualStyleBackColor = true;
            this.btFiles.Click += new System.EventHandler(this.btFiles_Click);
            // 
            // tbFiles
            // 
            this.tbFiles.Location = new System.Drawing.Point(53, 6);
            this.tbFiles.Name = "tbFiles";
            this.tbFiles.Size = new System.Drawing.Size(307, 20);
            this.tbFiles.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Файл:";
            // 
            // tpNumbers
            // 
            this.tpNumbers.Controls.Add(this.tbLast);
            this.tpNumbers.Controls.Add(this.tbFirst);
            this.tpNumbers.Controls.Add(this.label3);
            this.tpNumbers.Controls.Add(this.label2);
            this.tpNumbers.Location = new System.Drawing.Point(4, 22);
            this.tpNumbers.Name = "tpNumbers";
            this.tpNumbers.Padding = new System.Windows.Forms.Padding(3);
            this.tpNumbers.Size = new System.Drawing.Size(400, 218);
            this.tpNumbers.TabIndex = 1;
            this.tpNumbers.Text = "Приход товара по диапазону";
            this.tpNumbers.UseVisualStyleBackColor = true;
            // 
            // tbLast
            // 
            this.tbLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbLast.Location = new System.Drawing.Point(178, 33);
            this.tbLast.Name = "tbLast";
            this.tbLast.ReadOnly = true;
            this.tbLast.Size = new System.Drawing.Size(214, 21);
            this.tbLast.TabIndex = 3;
            this.tbLast.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // tbFirst
            // 
            this.tbFirst.Location = new System.Drawing.Point(178, 6);
            this.tbFirst.MaxLength = 20;
            this.tbFirst.Name = "tbFirst";
            this.tbFirst.Size = new System.Drawing.Size(214, 20);
            this.tbFirst.TabIndex = 2;
            this.tbFirst.TextChanged += new System.EventHandler(this.tbFirst_TextChanged);
            this.tbFirst.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox2_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Последний номер ICC:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Укажите первый номер ICC:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 49);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(321, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(246, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmTovar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 244);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTovar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приход товара";
            this.Shown += new System.EventHandler(this.frmTovar_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tpFiles.ResumeLayout(false);
            this.tpFiles.PerformLayout();
            this.tpNumbers.ResumeLayout(false);
            this.tpNumbers.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpFiles;
        private System.Windows.Forms.TabPage tpNumbers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lbProtocol;
        private System.Windows.Forms.Button btFiles;
        private System.Windows.Forms.TextBox tbFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLast;
        private System.Windows.Forms.TextBox tbFirst;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}