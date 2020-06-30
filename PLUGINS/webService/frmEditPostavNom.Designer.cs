namespace webService
{
    partial class frmEditPostavNom
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
            this.cbRName = new System.Windows.Forms.ComboBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbPrice = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.cbPostav = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbRName
            // 
            this.cbRName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRName.FormattingEnabled = true;
            this.cbRName.Location = new System.Drawing.Point(204, 64);
            this.cbRName.Name = "cbRName";
            this.cbRName.Size = new System.Drawing.Size(431, 21);
            this.cbRName.TabIndex = 2;
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(204, 38);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(431, 20);
            this.tbName.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(186, 13);
            this.label17.TabIndex = 45;
            this.label17.Text = "Наименование товара поставщика";
            // 
            // tbPrice
            // 
            this.tbPrice.Location = new System.Drawing.Point(204, 91);
            this.tbPrice.Name = "tbPrice";
            this.tbPrice.Size = new System.Drawing.Size(176, 20);
            this.tbPrice.TabIndex = 3;
            this.tbPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbAKB_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 98);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "Цена поставщика";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 41;
            this.label5.Text = "Родное наименование";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(560, 119);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(476, 119);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbPostav
            // 
            this.cbPostav.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPostav.FormattingEnabled = true;
            this.cbPostav.Location = new System.Drawing.Point(204, 11);
            this.cbPostav.Name = "cbPostav";
            this.cbPostav.Size = new System.Drawing.Size(431, 21);
            this.cbPostav.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Поставщик";
            // 
            // frmEditPostavNom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 152);
            this.Controls.Add(this.cbPostav);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbRName);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tbPrice);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditPostavNom";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменение информации о товаре";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbPrice;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.ComboBox cbPostav;
        private System.Windows.Forms.Label label1;
    }
}