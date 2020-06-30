namespace WSProf
{
    partial class frmRassilka
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbMonth2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbYear = new System.Windows.Forms.NumericUpDown();
            this.cbMonth1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbb = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMetod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbProvider = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbProvider2 = new System.Windows.Forms.ComboBox();
            this.panel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbYear)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(545, 0);
            this.panel2.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(458, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Закрыть";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 423);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(545, 49);
            this.panel3.TabIndex = 5;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.cbMonth2);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.cbYear);
            this.groupBox6.Controls.Add(this.cbMonth1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(545, 55);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Отчетный период";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(199, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Месяц по:";
            // 
            // cbMonth2
            // 
            this.cbMonth2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonth2.FormattingEnabled = true;
            this.cbMonth2.Items.AddRange(new object[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"});
            this.cbMonth2.Location = new System.Drawing.Point(263, 19);
            this.cbMonth2.Name = "cbMonth2";
            this.cbMonth2.Size = new System.Drawing.Size(121, 21);
            this.cbMonth2.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Месяц c:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Год:";
            // 
            // cbYear
            // 
            this.cbYear.Location = new System.Drawing.Point(424, 19);
            this.cbYear.Maximum = new decimal(new int[] {
            2021,
            0,
            0,
            0});
            this.cbYear.Minimum = new decimal(new int[] {
            2018,
            0,
            0,
            0});
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(109, 20);
            this.cbYear.TabIndex = 12;
            this.cbYear.Value = new decimal(new int[] {
            2019,
            0,
            0,
            0});
            // 
            // cbMonth1
            // 
            this.cbMonth1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonth1.FormattingEnabled = true;
            this.cbMonth1.Items.AddRange(new object[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"});
            this.cbMonth1.Location = new System.Drawing.Point(70, 19);
            this.cbMonth1.Name = "cbMonth1";
            this.cbMonth1.Size = new System.Drawing.Size(123, 21);
            this.cbMonth1.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbb);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbMetod);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbProvider);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(545, 267);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки формирования рассылки для информирования";
            // 
            // lbb
            // 
            this.lbb.FormattingEnabled = true;
            this.lbb.Items.AddRange(new object[] {
            "101 - приглашение на прохождение 1 этапа диспансеризации",
            "102 - напоминание о необходимости прохождения 1 этапа диспансеризации",
            "103 - приглашение на прохождение 2 этапа диспансеризации",
            "104 - напоминание о необходимости прохождения 2 этапа диспансеризации",
            "201 - приглашение на прохождение профилактического осмотра",
            "202 - напоминание о необходимости прохождения профилактического осмотра",
            "301 - приглашение на прохождение диспансерного осмотра",
            "302 - напоминание о необходимости прохождения диспансерного осмотра"});
            this.lbb.Location = new System.Drawing.Point(15, 92);
            this.lbb.Name = "lbb";
            this.lbb.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbb.Size = new System.Drawing.Size(518, 134);
            this.lbb.TabIndex = 29;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(438, 237);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "Сформировать";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Этапы информирования:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 53);
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
            "1 - SMS",
            "4 - Почта",
            "6 - Viber"});
            this.cbMetod.Location = new System.Drawing.Point(156, 45);
            this.cbMetod.Name = "cbMetod";
            this.cbMetod.Size = new System.Drawing.Size(377, 21);
            this.cbMetod.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Провайдер:";
            // 
            // cbProvider
            // 
            this.cbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProvider.FormattingEnabled = true;
            this.cbProvider.Items.AddRange(new object[] {
            "INFOBIT",
            "AIR",
            "POST"});
            this.cbProvider.Location = new System.Drawing.Point(81, 18);
            this.cbProvider.Name = "cbProvider";
            this.cbProvider.Size = new System.Drawing.Size(452, 21);
            this.cbProvider.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.tbFile);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbProvider2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 321);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 102);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Прием результатов рассылки";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(438, 70);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(95, 23);
            this.button5.TabIndex = 25;
            this.button5.Text = "Загрузить";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(509, 41);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 23);
            this.button4.TabIndex = 24;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tbFile
            // 
            this.tbFile.Location = new System.Drawing.Point(81, 44);
            this.tbFile.Name = "tbFile";
            this.tbFile.Size = new System.Drawing.Size(422, 20);
            this.tbFile.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Файл:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Провайдер:";
            // 
            // cbProvider2
            // 
            this.cbProvider2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProvider2.FormattingEnabled = true;
            this.cbProvider2.Items.AddRange(new object[] {
            "INFOBIT",
            "AIR",
            "POST"});
            this.cbProvider2.Location = new System.Drawing.Point(81, 17);
            this.cbProvider2.Name = "cbProvider2";
            this.cbProvider2.Size = new System.Drawing.Size(452, 21);
            this.cbProvider2.TabIndex = 20;
            this.cbProvider2.SelectionChangeCommitted += new System.EventHandler(this.cbProvider2_SelectionChangeCommitted);
            // 
            // frmRassilka
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 472);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.panel2);
            this.Name = "frmRassilka";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Информирование граждан";
            this.panel3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbYear)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown cbYear;
        private System.Windows.Forms.ComboBox cbMonth1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbMetod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProvider;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbProvider2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbMonth2;
        private System.Windows.Forms.ListBox lbb;
    }
}