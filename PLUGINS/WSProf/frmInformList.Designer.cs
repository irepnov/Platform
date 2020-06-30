namespace WSProf
{
    partial class frmInformList
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbStep = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMetod = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbRslt = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbRslt);
            this.groupBox2.Controls.Add(this.dtDate);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cbStep);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbMetod);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(554, 136);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки информирования";
            // 
            // dtDate
            // 
            this.dtDate.Location = new System.Drawing.Point(171, 101);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(215, 20);
            this.dtDate.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Дата информирования:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Этап информирования:";
            // 
            // cbStep
            // 
            this.cbStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStep.FormattingEnabled = true;
            this.cbStep.Items.AddRange(new object[] {
            "101 - приглашение на прохождение 1 этапа диспансеризации",
            "102 - напоминание о необходимости прохождения 1 этапа диспансеризации",
            "103 - приглашение на прохождение 2 этапа диспансеризации",
            "104 - напоминание о необходимости прохождения 2 этапа диспансеризации",
            "201 - приглашение на прохождене профилактического осмотра",
            "202 - напоминание о необходимости прохождения профилактического осмотра",
            "121 - телефонный опрос по причинам непрохождения диспансеризации",
            "122 - телефонный опрос по удовлетворенности диспансеризацией",
            "221 - телефонный опрос по причинам непрохождения проф осмотра",
            "222 - телефонный опрос по удовлетворенности проф осмотром",
            "301 - приглашение на прохождение диспансерного осмотра",
            "302 - напоминание о необходимости прохождения диспансерного осмотра",
            "321 - телефонный опрос пр причинам непрохождения диспансерного осмотра",
            "322 - телефонный опрос по удовлетворенности диспансерным осмотром"});
            this.cbStep.Location = new System.Drawing.Point(171, 47);
            this.cbStep.Name = "cbStep";
            this.cbStep.Size = new System.Drawing.Size(367, 21);
            this.cbStep.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Способ информирования:";
            // 
            // cbMetod
            // 
            this.cbMetod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMetod.FormattingEnabled = true;
            this.cbMetod.Items.AddRange(new object[] {
            "01 - sms",
            "02 - Email",
            "03 - телефон",
            "04 - почта",
            "05 - прочее",
            "06 - мессенджер",
            "07 - адресный обход",
            "08 - иные способы (памятка)",
            "09 - иные способы (брошюра)",
            "10 - иные способы (листовки)",
            "11 - иные способы (визит ЗЛ в МО или в СМО)",
            "21 - телефонный опрос по причинам непрохождения",
            "22 - телефонный опрос по удовлетворенности"});
            this.cbMetod.Location = new System.Drawing.Point(171, 20);
            this.cbMetod.Name = "cbMetod";
            this.cbMetod.Size = new System.Drawing.Size(367, 21);
            this.cbMetod.TabIndex = 20;
            this.cbMetod.SelectionChangeCommitted += new System.EventHandler(this.cbMetod_SelectionChangeCommitted);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(308, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "Проинформировать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(443, 142);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 26;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Результат информирования:";
            // 
            // cbRslt
            // 
            this.cbRslt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRslt.FormattingEnabled = true;
            this.cbRslt.Location = new System.Drawing.Point(171, 74);
            this.cbRslt.Name = "cbRslt";
            this.cbRslt.Size = new System.Drawing.Size(367, 21);
            this.cbRslt.TabIndex = 27;
            // 
            // frmInformList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 173);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInformList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ручное информирование";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbMetod;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRslt;
    }
}