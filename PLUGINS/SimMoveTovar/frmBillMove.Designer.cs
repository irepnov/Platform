namespace SimMoveT
{
    partial class frmBillMove
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
            this.dtDateMove = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.tbNumberBill = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbTovar = new System.Windows.Forms.ComboBox();
            this.cbSkladRecipient = new System.Windows.Forms.ComboBox();
            this.tbKol = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btTovar = new System.Windows.Forms.Button();
            this.cbSkladSender = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbLiability = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtDateMove);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbNumberBill);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 53);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Накладная перемещения";
            // 
            // dtDateMove
            // 
            this.dtDateMove.Location = new System.Drawing.Point(218, 21);
            this.dtDateMove.Name = "dtDateMove";
            this.dtDateMove.Size = new System.Drawing.Size(176, 20);
            this.dtDateMove.TabIndex = 1;
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
            this.label1.TabIndex = 0;
            this.label1.Text = "Товар:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Склад получатель:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Кол-во ед.:";
            // 
            // cbTovar
            // 
            this.cbTovar.FormattingEnabled = true;
            this.cbTovar.Location = new System.Drawing.Point(56, 80);
            this.cbTovar.Name = "cbTovar";
            this.cbTovar.Size = new System.Drawing.Size(356, 21);
            this.cbTovar.TabIndex = 0;
            // 
            // cbSkladRecipient
            // 
            this.cbSkladRecipient.FormattingEnabled = true;
            this.cbSkladRecipient.Location = new System.Drawing.Point(129, 136);
            this.cbSkladRecipient.Name = "cbSkladRecipient";
            this.cbSkladRecipient.Size = new System.Drawing.Size(283, 21);
            this.cbSkladRecipient.TabIndex = 2;
            // 
            // tbKol
            // 
            this.tbKol.Location = new System.Drawing.Point(77, 196);
            this.tbKol.Name = "tbKol";
            this.tbKol.Size = new System.Drawing.Size(299, 20);
            this.tbKol.TabIndex = 5;
            this.tbKol.TextChanged += new System.EventHandler(this.tbKol_TextChanged);
            this.tbKol.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbKol_KeyPress);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(256, 234);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 7;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(337, 234);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 8;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btTovar
            // 
            this.btTovar.Enabled = false;
            this.btTovar.Location = new System.Drawing.Point(384, 193);
            this.btTovar.Name = "btTovar";
            this.btTovar.Size = new System.Drawing.Size(28, 23);
            this.btTovar.TabIndex = 6;
            this.btTovar.Text = "...";
            this.btTovar.UseVisualStyleBackColor = true;
            this.btTovar.Click += new System.EventHandler(this.btTovar_Click);
            // 
            // cbSkladSender
            // 
            this.cbSkladSender.FormattingEnabled = true;
            this.cbSkladSender.Location = new System.Drawing.Point(129, 109);
            this.cbSkladSender.Name = "cbSkladSender";
            this.cbSkladSender.Size = new System.Drawing.Size(283, 21);
            this.cbSkladSender.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 117);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Склад отправитель:";
            // 
            // cbLiability
            // 
            this.cbLiability.FormattingEnabled = true;
            this.cbLiability.Location = new System.Drawing.Point(129, 163);
            this.cbLiability.Name = "cbLiability";
            this.cbLiability.Size = new System.Drawing.Size(283, 21);
            this.cbLiability.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Ответственное лицо:";
            // 
            // frmBillMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 267);
            this.Controls.Add(this.cbLiability);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbSkladSender);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btTovar);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbKol);
            this.Controls.Add(this.cbSkladRecipient);
            this.Controls.Add(this.cbTovar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBillMove";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Накладная перемещения";
            this.Load += new System.EventHandler(this.frmBillIncome_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtDateMove;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbNumberBill;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbTovar;
        private System.Windows.Forms.ComboBox cbSkladRecipient;
        private System.Windows.Forms.TextBox tbKol;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btTovar;
        private System.Windows.Forms.ComboBox cbSkladSender;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbLiability;
        private System.Windows.Forms.Label label4;
    }
}