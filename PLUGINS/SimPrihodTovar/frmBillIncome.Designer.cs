namespace SimPrihodT
{
    partial class frmBillIncome
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtDateBill = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.tbNumberBill = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbTovar = new System.Windows.Forms.ComboBox();
            this.cbSklad = new System.Windows.Forms.ComboBox();
            this.dtPostup = new System.Windows.Forms.DateTimePicker();
            this.tbCena = new System.Windows.Forms.TextBox();
            this.tbKol = new System.Windows.Forms.TextBox();
            this.tbItog = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btTovar = new System.Windows.Forms.Button();
            this.cbProvider = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbProdName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtDateBill);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbNumberBill);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 53);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Приходная накладная";
            // 
            // dtDateBill
            // 
            this.dtDateBill.Location = new System.Drawing.Point(218, 21);
            this.dtDateBill.Name = "dtDateBill";
            this.dtDateBill.Size = new System.Drawing.Size(176, 20);
            this.dtDateBill.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(191, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(21, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "от:";
            // 
            // tbNumberBill
            // 
            this.tbNumberBill.Location = new System.Drawing.Point(33, 20);
            this.tbNumberBill.Name = "tbNumberBill";
            this.tbNumberBill.Size = new System.Drawing.Size(139, 20);
            this.tbNumberBill.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "№:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Товар:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Склад получатель:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Дата поступления на склад:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Цена ед.:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Кол-во ед.:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(173, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "итого:";
            // 
            // cbTovar
            // 
            this.cbTovar.FormattingEnabled = true;
            this.cbTovar.Location = new System.Drawing.Point(56, 80);
            this.cbTovar.Name = "cbTovar";
            this.cbTovar.Size = new System.Drawing.Size(356, 21);
            this.cbTovar.TabIndex = 2;
            // 
            // cbSklad
            // 
            this.cbSklad.FormattingEnabled = true;
            this.cbSklad.Location = new System.Drawing.Point(116, 161);
            this.cbSklad.Name = "cbSklad";
            this.cbSklad.Size = new System.Drawing.Size(296, 21);
            this.cbSklad.TabIndex = 4;
            // 
            // dtPostup
            // 
            this.dtPostup.Location = new System.Drawing.Point(170, 188);
            this.dtPostup.Name = "dtPostup";
            this.dtPostup.Size = new System.Drawing.Size(242, 20);
            this.dtPostup.TabIndex = 5;
            // 
            // tbCena
            // 
            this.tbCena.Location = new System.Drawing.Point(77, 223);
            this.tbCena.Name = "tbCena";
            this.tbCena.Size = new System.Drawing.Size(335, 20);
            this.tbCena.TabIndex = 6;
            this.tbCena.Text = "0";
            this.tbCena.TextChanged += new System.EventHandler(this.tbKol_TextChanged);
            this.tbCena.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCena_KeyPress);
            // 
            // tbKol
            // 
            this.tbKol.Location = new System.Drawing.Point(77, 249);
            this.tbKol.Name = "tbKol";
            this.tbKol.Size = new System.Drawing.Size(299, 20);
            this.tbKol.TabIndex = 7;
            this.tbKol.Text = "0";
            this.tbKol.TextChanged += new System.EventHandler(this.tbKol_TextChanged);
            this.tbKol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbKol_KeyPress);
            // 
            // tbItog
            // 
            this.tbItog.Enabled = false;
            this.tbItog.Location = new System.Drawing.Point(217, 276);
            this.tbItog.Name = "tbItog";
            this.tbItog.Size = new System.Drawing.Size(195, 20);
            this.tbItog.TabIndex = 12;
            this.tbItog.TabStop = false;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(256, 311);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 14;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(337, 311);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 15;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btTovar
            // 
            this.btTovar.Location = new System.Drawing.Point(384, 246);
            this.btTovar.Name = "btTovar";
            this.btTovar.Size = new System.Drawing.Size(28, 23);
            this.btTovar.TabIndex = 16;
            this.btTovar.Text = "...";
            this.btTovar.UseVisualStyleBackColor = true;
            this.btTovar.Click += new System.EventHandler(this.btTovar_Click);
            // 
            // cbProvider
            // 
            this.cbProvider.FormattingEnabled = true;
            this.cbProvider.Location = new System.Drawing.Point(74, 134);
            this.cbProvider.Name = "cbProvider";
            this.cbProvider.Size = new System.Drawing.Size(338, 21);
            this.cbProvider.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Оператор:";
            // 
            // tbProdName
            // 
            this.tbProdName.Location = new System.Drawing.Point(67, 108);
            this.tbProdName.MaxLength = 300;
            this.tbProdName.Name = "tbProdName";
            this.tbProdName.Size = new System.Drawing.Size(345, 20);
            this.tbProdName.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Продукт:";
            // 
            // frmBillIncome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 344);
            this.Controls.Add(this.tbProdName);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbProvider);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btTovar);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbItog);
            this.Controls.Add(this.tbKol);
            this.Controls.Add(this.tbCena);
            this.Controls.Add(this.dtPostup);
            this.Controls.Add(this.cbSklad);
            this.Controls.Add(this.cbTovar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBillIncome";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приходная накладная";
            this.Load += new System.EventHandler(this.frmBillIncome_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtDateBill;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbNumberBill;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbTovar;
        private System.Windows.Forms.ComboBox cbSklad;
        private System.Windows.Forms.DateTimePicker dtPostup;
        private System.Windows.Forms.TextBox tbCena;
        private System.Windows.Forms.TextBox tbKol;
        private System.Windows.Forms.TextBox tbItog;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btTovar;
        private System.Windows.Forms.ComboBox cbProvider;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbProdName;
        private System.Windows.Forms.Label label10;
    }
}