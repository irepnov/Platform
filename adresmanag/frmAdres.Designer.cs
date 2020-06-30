namespace GGPlatform.AdresManag
{
    partial class frmAdres
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdres));
            this.tbAdres = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbKvart = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbKor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbHouse = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbStreet = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbNaspunkt = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbGorod = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRaion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbRegion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAdres
            // 
            this.tbAdres.Enabled = false;
            this.tbAdres.Location = new System.Drawing.Point(56, 10);
            this.tbAdres.Name = "tbAdres";
            this.tbAdres.Size = new System.Drawing.Size(664, 20);
            this.tbAdres.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Адрес";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbKvart);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbKor);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbHouse);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbStreet);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbNaspunkt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbGorod);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbRaion);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbRegion);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(708, 104);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Реквизиты адреса";
            // 
            // tbKvart
            // 
            this.tbKvart.Location = new System.Drawing.Point(652, 77);
            this.tbKvart.Name = "tbKvart";
            this.tbKvart.Size = new System.Drawing.Size(40, 20);
            this.tbKvart.TabIndex = 15;
            this.tbKvart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            this.tbKvart.Leave += new System.EventHandler(this.tbHouse_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(624, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "кв:";
            // 
            // tbKor
            // 
            this.tbKor.Location = new System.Drawing.Point(578, 77);
            this.tbKor.Name = "tbKor";
            this.tbKor.Size = new System.Drawing.Size(40, 20);
            this.tbKor.TabIndex = 13;
            this.tbKor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            this.tbKor.Leave += new System.EventHandler(this.tbHouse_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(544, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "кор:";
            // 
            // tbHouse
            // 
            this.tbHouse.Location = new System.Drawing.Point(498, 77);
            this.tbHouse.Name = "tbHouse";
            this.tbHouse.Size = new System.Drawing.Size(40, 20);
            this.tbHouse.TabIndex = 11;
            this.tbHouse.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            this.tbHouse.Leave += new System.EventHandler(this.tbHouse_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(462, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "дом:";
            // 
            // cbStreet
            // 
            this.cbStreet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbStreet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbStreet.FormattingEnabled = true;
            this.cbStreet.Location = new System.Drawing.Point(67, 74);
            this.cbStreet.Name = "cbStreet";
            this.cbStreet.Size = new System.Drawing.Size(389, 21);
            this.cbStreet.TabIndex = 9;
            this.cbStreet.SelectionChangeCommitted += new System.EventHandler(this.cbRegion_SelectionChangeCommitted);
            this.cbStreet.Enter += new System.EventHandler(this.cbStreet_Enter);
            this.cbStreet.Leave += new System.EventHandler(this.cbStreet_Leave);
            this.cbStreet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Улица:";
            // 
            // cbNaspunkt
            // 
            this.cbNaspunkt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbNaspunkt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbNaspunkt.FormattingEnabled = true;
            this.cbNaspunkt.Location = new System.Drawing.Point(414, 47);
            this.cbNaspunkt.Name = "cbNaspunkt";
            this.cbNaspunkt.Size = new System.Drawing.Size(278, 21);
            this.cbNaspunkt.TabIndex = 7;
            this.cbNaspunkt.SelectionChangeCommitted += new System.EventHandler(this.cbRegion_SelectionChangeCommitted);
            this.cbNaspunkt.Enter += new System.EventHandler(this.cbNaspunkt_Enter);
            this.cbNaspunkt.Leave += new System.EventHandler(this.cbNaspunkt_Leave);
            this.cbNaspunkt.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(351, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Нас.пункт:";
            // 
            // cbGorod
            // 
            this.cbGorod.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbGorod.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbGorod.FormattingEnabled = true;
            this.cbGorod.Location = new System.Drawing.Point(67, 47);
            this.cbGorod.Name = "cbGorod";
            this.cbGorod.Size = new System.Drawing.Size(278, 21);
            this.cbGorod.TabIndex = 5;
            this.cbGorod.SelectionChangeCommitted += new System.EventHandler(this.cbRegion_SelectionChangeCommitted);
            this.cbGorod.Enter += new System.EventHandler(this.cbGorod_Enter);
            this.cbGorod.Leave += new System.EventHandler(this.cbGorod_Leave);
            this.cbGorod.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Город:";
            // 
            // cbRaion
            // 
            this.cbRaion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbRaion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbRaion.FormattingEnabled = true;
            this.cbRaion.Location = new System.Drawing.Point(414, 19);
            this.cbRaion.Name = "cbRaion";
            this.cbRaion.Size = new System.Drawing.Size(278, 21);
            this.cbRaion.TabIndex = 3;
            this.cbRaion.SelectionChangeCommitted += new System.EventHandler(this.cbRegion_SelectionChangeCommitted);
            this.cbRaion.Enter += new System.EventHandler(this.cbRaion_Enter);
            this.cbRaion.Leave += new System.EventHandler(this.cbRaion_Leave);
            this.cbRaion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(351, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Район:";
            // 
            // cbRegion
            // 
            this.cbRegion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbRegion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbRegion.FormattingEnabled = true;
            this.cbRegion.Location = new System.Drawing.Point(67, 20);
            this.cbRegion.Name = "cbRegion";
            this.cbRegion.Size = new System.Drawing.Size(278, 21);
            this.cbRegion.TabIndex = 1;
            this.cbRegion.SelectionChangeCommitted += new System.EventHandler(this.cbRegion_SelectionChangeCommitted);
            this.cbRegion.Enter += new System.EventHandler(this.cbRegion_Enter);
            this.cbRegion.Leave += new System.EventHandler(this.cbRegion_Leave);
            this.cbRegion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cbRegion_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Регион:";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(564, 148);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 3;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(645, 148);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // frmAdres
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 178);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAdres);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdres";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Справочник адресов";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAdres_FormClosed);
            this.Load += new System.EventHandler(this.frmAdres_Load);
            this.Shown += new System.EventHandler(this.frmAdres_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAdres;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbKvart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbKor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbHouse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbStreet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbNaspunkt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbGorod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbRaion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRegion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
    }
}