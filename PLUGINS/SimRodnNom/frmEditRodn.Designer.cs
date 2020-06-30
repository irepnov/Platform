namespace SimRodnNom
{
    partial class frmEditRodn
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
            this.btOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSebest = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbRozn = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbRName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.chbAsort = new System.Windows.Forms.CheckBox();
            this.cbEName = new System.Windows.Forms.ComboBox();
            this.tbCountOrder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(476, 171);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(560, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Себестоимость";
            // 
            // tbSebest
            // 
            this.tbSebest.Location = new System.Drawing.Point(165, 81);
            this.tbSebest.Name = "tbSebest";
            this.tbSebest.Size = new System.Drawing.Size(176, 20);
            this.tbSebest.TabIndex = 3;
            this.tbSebest.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSebest_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Эталонное наименование";
            // 
            // tbRozn
            // 
            this.tbRozn.Location = new System.Drawing.Point(165, 110);
            this.tbRozn.Name = "tbRozn";
            this.tbRozn.Size = new System.Drawing.Size(176, 20);
            this.tbRozn.TabIndex = 4;
            this.tbRozn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSebest_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(88, 13);
            this.label15.TabIndex = 30;
            this.label15.Text = "Розничная цена";
            // 
            // tbRName
            // 
            this.tbRName.Location = new System.Drawing.Point(165, 8);
            this.tbRName.Name = "tbRName";
            this.tbRName.Size = new System.Drawing.Size(470, 20);
            this.tbRName.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(121, 13);
            this.label17.TabIndex = 34;
            this.label17.Text = "Родное наименование";
            // 
            // chbAsort
            // 
            this.chbAsort.AutoSize = true;
            this.chbAsort.Location = new System.Drawing.Point(165, 61);
            this.chbAsort.Name = "chbAsort";
            this.chbAsort.Size = new System.Drawing.Size(156, 17);
            this.chbAsort.TabIndex = 2;
            this.chbAsort.Text = "ассортиментная матрица";
            this.chbAsort.UseVisualStyleBackColor = true;
            // 
            // cbEName
            // 
            this.cbEName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEName.FormattingEnabled = true;
            this.cbEName.Location = new System.Drawing.Point(165, 34);
            this.cbEName.Name = "cbEName";
            this.cbEName.Size = new System.Drawing.Size(470, 21);
            this.cbEName.TabIndex = 1;
            // 
            // tbCountOrder
            // 
            this.tbCountOrder.Location = new System.Drawing.Point(165, 141);
            this.tbCountOrder.Name = "tbCountOrder";
            this.tbCountOrder.Size = new System.Drawing.Size(176, 20);
            this.tbCountOrder.TabIndex = 5;
            this.tbCountOrder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Заказ";
            // 
            // frmEditRodn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 206);
            this.Controls.Add(this.tbCountOrder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbEName);
            this.Controls.Add(this.chbAsort);
            this.Controls.Add(this.tbRName);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tbRozn);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbSebest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditRodn";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменить информацию о товаре";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSebest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbRozn;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbRName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chbAsort;
        private System.Windows.Forms.ComboBox cbEName;
        private System.Windows.Forms.TextBox tbCountOrder;
        private System.Windows.Forms.Label label2;
    }
}