namespace SimRFC
{
    partial class frmElement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmElement));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageTovar = new System.Windows.Forms.TabPage();
            this.tbTovarName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPageProvider = new System.Windows.Forms.TabPage();
            this.tbProviderComment = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbProviderName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageSklad = new System.Windows.Forms.TabPage();
            this.tbSkladAnaliz = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSkladOtvets = new System.Windows.Forms.ComboBox();
            this.cbSkladType = new System.Windows.Forms.ComboBox();
            this.chSkladOsnovnoy = new System.Windows.Forms.CheckBox();
            this.tbSkladCodePoint = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSkladComment = new System.Windows.Forms.RichTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSkladName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageOtvLico = new System.Windows.Forms.TabPage();
            this.btAdresPr = new System.Windows.Forms.Button();
            this.btAdresReg = new System.Windows.Forms.Button();
            this.tbLicoAdreRg = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbLicoAdrePr = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cbLicoSkald = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbLicoDocNum = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbLicoDocSer = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbLicoPhone3 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbLicoPhone2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbLicoIm = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbLicoOtch = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbLicoPhone1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbLicoFam = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tabPageTypeSklad = new System.Windows.Forms.TabPage();
            this.tbTypeSkladName = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tabPagePostav = new System.Windows.Forms.TabPage();
            this.dtPostavPrice = new System.Windows.Forms.DateTimePicker();
            this.label23 = new System.Windows.Forms.Label();
            this.chbPostavActual = new System.Windows.Forms.CheckBox();
            this.tbPostavName = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageTovar.SuspendLayout();
            this.tabPageProvider.SuspendLayout();
            this.tabPageSklad.SuspendLayout();
            this.tabPageOtvLico.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageTypeSklad.SuspendLayout();
            this.tabPagePostav.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "users.ico");
            this.imageList1.Images.SetKeyName(1, "objects.ico");
            this.imageList1.Images.SetKeyName(2, "tumbs.ico");
            this.imageList1.Images.SetKeyName(3, "01594.ico");
            this.imageList1.Images.SetKeyName(4, "adim.ico");
            this.imageList1.Images.SetKeyName(5, "application_tile_horizontal_2349.ico");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 413);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 50);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(410, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(329, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "ОК";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(494, 413);
            this.panel2.TabIndex = 2;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageTovar);
            this.tabControl.Controls.Add(this.tabPageProvider);
            this.tabControl.Controls.Add(this.tabPageSklad);
            this.tabControl.Controls.Add(this.tabPageOtvLico);
            this.tabControl.Controls.Add(this.tabPageTypeSklad);
            this.tabControl.Controls.Add(this.tabPagePostav);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList1;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(494, 413);
            this.tabControl.TabIndex = 1;
            // 
            // tabPageTovar
            // 
            this.tabPageTovar.Controls.Add(this.tbTovarName);
            this.tabPageTovar.Controls.Add(this.label8);
            this.tabPageTovar.ImageIndex = 2;
            this.tabPageTovar.Location = new System.Drawing.Point(4, 23);
            this.tabPageTovar.Name = "tabPageTovar";
            this.tabPageTovar.Size = new System.Drawing.Size(486, 386);
            this.tabPageTovar.TabIndex = 5;
            this.tabPageTovar.Text = "Товар";
            this.tabPageTovar.UseVisualStyleBackColor = true;
            // 
            // tbTovarName
            // 
            this.tbTovarName.Location = new System.Drawing.Point(98, 17);
            this.tbTovarName.MaxLength = 100;
            this.tbTovarName.Name = "tbTovarName";
            this.tbTovarName.Size = new System.Drawing.Size(371, 20);
            this.tbTovarName.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Наименование";
            // 
            // tabPageProvider
            // 
            this.tabPageProvider.Controls.Add(this.tbProviderComment);
            this.tabPageProvider.Controls.Add(this.label1);
            this.tabPageProvider.Controls.Add(this.tbProviderName);
            this.tabPageProvider.Controls.Add(this.label2);
            this.tabPageProvider.ImageIndex = 0;
            this.tabPageProvider.Location = new System.Drawing.Point(4, 23);
            this.tabPageProvider.Name = "tabPageProvider";
            this.tabPageProvider.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProvider.Size = new System.Drawing.Size(486, 386);
            this.tabPageProvider.TabIndex = 0;
            this.tabPageProvider.Text = "Оператор";
            this.tabPageProvider.UseVisualStyleBackColor = true;
            // 
            // tbProviderComment
            // 
            this.tbProviderComment.Location = new System.Drawing.Point(98, 52);
            this.tbProviderComment.MaxLength = 300;
            this.tbProviderComment.Name = "tbProviderComment";
            this.tbProviderComment.Size = new System.Drawing.Size(371, 105);
            this.tbProviderComment.TabIndex = 7;
            this.tbProviderComment.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Описание";
            // 
            // tbProviderName
            // 
            this.tbProviderName.Location = new System.Drawing.Point(98, 17);
            this.tbProviderName.MaxLength = 100;
            this.tbProviderName.Name = "tbProviderName";
            this.tbProviderName.Size = new System.Drawing.Size(371, 20);
            this.tbProviderName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Наименование";
            // 
            // tabPageSklad
            // 
            this.tabPageSklad.Controls.Add(this.tbSkladAnaliz);
            this.tabPageSklad.Controls.Add(this.label20);
            this.tabPageSklad.Controls.Add(this.label7);
            this.tabPageSklad.Controls.Add(this.label4);
            this.tabPageSklad.Controls.Add(this.cbSkladOtvets);
            this.tabPageSklad.Controls.Add(this.cbSkladType);
            this.tabPageSklad.Controls.Add(this.chSkladOsnovnoy);
            this.tabPageSklad.Controls.Add(this.tbSkladCodePoint);
            this.tabPageSklad.Controls.Add(this.label3);
            this.tabPageSklad.Controls.Add(this.tbSkladComment);
            this.tabPageSklad.Controls.Add(this.label6);
            this.tabPageSklad.Controls.Add(this.tbSkladName);
            this.tabPageSklad.Controls.Add(this.label5);
            this.tabPageSklad.ImageIndex = 3;
            this.tabPageSklad.Location = new System.Drawing.Point(4, 23);
            this.tabPageSklad.Name = "tabPageSklad";
            this.tabPageSklad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSklad.Size = new System.Drawing.Size(486, 386);
            this.tabPageSklad.TabIndex = 1;
            this.tabPageSklad.Text = "Склад";
            this.tabPageSklad.UseVisualStyleBackColor = true;
            // 
            // tbSkladAnaliz
            // 
            this.tbSkladAnaliz.Location = new System.Drawing.Point(97, 109);
            this.tbSkladAnaliz.MaxLength = 50;
            this.tbSkladAnaliz.Name = "tbSkladAnaliz";
            this.tbSkladAnaliz.Size = new System.Drawing.Size(371, 20);
            this.tbSkladAnaliz.TabIndex = 12;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 116);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(44, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "Анализ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Тип склада";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Ответственное лицо";
            // 
            // cbSkladOtvets
            // 
            this.cbSkladOtvets.FormattingEnabled = true;
            this.cbSkladOtvets.Location = new System.Drawing.Point(130, 144);
            this.cbSkladOtvets.Name = "cbSkladOtvets";
            this.cbSkladOtvets.Size = new System.Drawing.Size(338, 21);
            this.cbSkladOtvets.TabIndex = 8;
            // 
            // cbSkladType
            // 
            this.cbSkladType.FormattingEnabled = true;
            this.cbSkladType.Location = new System.Drawing.Point(97, 76);
            this.cbSkladType.Name = "cbSkladType";
            this.cbSkladType.Size = new System.Drawing.Size(371, 21);
            this.cbSkladType.TabIndex = 7;
            // 
            // chSkladOsnovnoy
            // 
            this.chSkladOsnovnoy.AutoSize = true;
            this.chSkladOsnovnoy.Location = new System.Drawing.Point(97, 48);
            this.chSkladOsnovnoy.Name = "chSkladOsnovnoy";
            this.chSkladOsnovnoy.Size = new System.Drawing.Size(107, 17);
            this.chSkladOsnovnoy.TabIndex = 6;
            this.chSkladOsnovnoy.Text = "основной склад";
            this.chSkladOsnovnoy.UseVisualStyleBackColor = true;
            // 
            // tbSkladCodePoint
            // 
            this.tbSkladCodePoint.Location = new System.Drawing.Point(97, 182);
            this.tbSkladCodePoint.MaxLength = 100;
            this.tbSkladCodePoint.Name = "tbSkladCodePoint";
            this.tbSkladCodePoint.Size = new System.Drawing.Size(371, 20);
            this.tbSkladCodePoint.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Код точки";
            // 
            // tbSkladComment
            // 
            this.tbSkladComment.Location = new System.Drawing.Point(97, 219);
            this.tbSkladComment.MaxLength = 300;
            this.tbSkladComment.Name = "tbSkladComment";
            this.tbSkladComment.Size = new System.Drawing.Size(371, 105);
            this.tbSkladComment.TabIndex = 3;
            this.tbSkladComment.Text = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Описание";
            // 
            // tbSkladName
            // 
            this.tbSkladName.Location = new System.Drawing.Point(97, 15);
            this.tbSkladName.MaxLength = 100;
            this.tbSkladName.Name = "tbSkladName";
            this.tbSkladName.Size = new System.Drawing.Size(371, 20);
            this.tbSkladName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Наименование";
            // 
            // tabPageOtvLico
            // 
            this.tabPageOtvLico.Controls.Add(this.btAdresPr);
            this.tabPageOtvLico.Controls.Add(this.btAdresReg);
            this.tabPageOtvLico.Controls.Add(this.tbLicoAdreRg);
            this.tabPageOtvLico.Controls.Add(this.label19);
            this.tabPageOtvLico.Controls.Add(this.tbLicoAdrePr);
            this.tabPageOtvLico.Controls.Add(this.label18);
            this.tabPageOtvLico.Controls.Add(this.label16);
            this.tabPageOtvLico.Controls.Add(this.cbLicoSkald);
            this.tabPageOtvLico.Controls.Add(this.groupBox1);
            this.tabPageOtvLico.Controls.Add(this.tbLicoPhone3);
            this.tabPageOtvLico.Controls.Add(this.label13);
            this.tabPageOtvLico.Controls.Add(this.tbLicoPhone2);
            this.tabPageOtvLico.Controls.Add(this.label12);
            this.tabPageOtvLico.Controls.Add(this.tbLicoIm);
            this.tabPageOtvLico.Controls.Add(this.label11);
            this.tabPageOtvLico.Controls.Add(this.tbLicoOtch);
            this.tabPageOtvLico.Controls.Add(this.label10);
            this.tabPageOtvLico.Controls.Add(this.tbLicoPhone1);
            this.tabPageOtvLico.Controls.Add(this.label9);
            this.tabPageOtvLico.Controls.Add(this.tbLicoFam);
            this.tabPageOtvLico.Controls.Add(this.label17);
            this.tabPageOtvLico.ImageIndex = 4;
            this.tabPageOtvLico.Location = new System.Drawing.Point(4, 23);
            this.tabPageOtvLico.Name = "tabPageOtvLico";
            this.tabPageOtvLico.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOtvLico.Size = new System.Drawing.Size(486, 386);
            this.tabPageOtvLico.TabIndex = 2;
            this.tabPageOtvLico.Text = "Ответственное лицо";
            this.tabPageOtvLico.UseVisualStyleBackColor = true;
            // 
            // btAdresPr
            // 
            this.btAdresPr.Location = new System.Drawing.Point(439, 351);
            this.btAdresPr.Name = "btAdresPr";
            this.btAdresPr.Size = new System.Drawing.Size(29, 23);
            this.btAdresPr.TabIndex = 22;
            this.btAdresPr.Text = "...";
            this.btAdresPr.UseVisualStyleBackColor = true;
            this.btAdresPr.Click += new System.EventHandler(this.btAdresPr_Click);
            // 
            // btAdresReg
            // 
            this.btAdresReg.Location = new System.Drawing.Point(439, 318);
            this.btAdresReg.Name = "btAdresReg";
            this.btAdresReg.Size = new System.Drawing.Size(29, 23);
            this.btAdresReg.TabIndex = 21;
            this.btAdresReg.Text = "...";
            this.btAdresReg.UseVisualStyleBackColor = true;
            this.btAdresReg.Click += new System.EventHandler(this.btAdresReg_Click);
            // 
            // tbLicoAdreRg
            // 
            this.tbLicoAdreRg.Enabled = false;
            this.tbLicoAdreRg.Location = new System.Drawing.Point(110, 318);
            this.tbLicoAdreRg.MaxLength = 100;
            this.tbLicoAdreRg.Name = "tbLicoAdreRg";
            this.tbLicoAdreRg.Size = new System.Drawing.Size(323, 20);
            this.tbLicoAdreRg.TabIndex = 20;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(8, 325);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(96, 13);
            this.label19.TabIndex = 19;
            this.label19.Text = "Зарегистрирован";
            // 
            // tbLicoAdrePr
            // 
            this.tbLicoAdrePr.Enabled = false;
            this.tbLicoAdrePr.Location = new System.Drawing.Point(110, 353);
            this.tbLicoAdrePr.MaxLength = 100;
            this.tbLicoAdrePr.Name = "tbLicoAdrePr";
            this.tbLicoAdrePr.Size = new System.Drawing.Size(323, 20);
            this.tbLicoAdrePr.TabIndex = 18;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 360);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 17;
            this.label18.Text = "Проживает";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 289);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(38, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "Склад";
            // 
            // cbLicoSkald
            // 
            this.cbLicoSkald.FormattingEnabled = true;
            this.cbLicoSkald.Location = new System.Drawing.Point(97, 281);
            this.cbLicoSkald.Name = "cbLicoSkald";
            this.cbLicoSkald.Size = new System.Drawing.Size(371, 21);
            this.cbLicoSkald.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLicoDocNum);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.tbLicoDocSer);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Location = new System.Drawing.Point(13, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(455, 47);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "УДЛ";
            // 
            // tbLicoDocNum
            // 
            this.tbLicoDocNum.Location = new System.Drawing.Point(287, 19);
            this.tbLicoDocNum.MaxLength = 8;
            this.tbLicoDocNum.Name = "tbLicoDocNum";
            this.tbLicoDocNum.Size = new System.Drawing.Size(154, 20);
            this.tbLicoDocNum.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(242, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(39, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "номер";
            // 
            // tbLicoDocSer
            // 
            this.tbLicoDocSer.Location = new System.Drawing.Point(124, 19);
            this.tbLicoDocSer.MaxLength = 5;
            this.tbLicoDocSer.Name = "tbLicoDocSer";
            this.tbLicoDocSer.Size = new System.Drawing.Size(99, 20);
            this.tbLicoDocSer.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(81, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(37, 13);
            this.label14.TabIndex = 6;
            this.label14.Text = "серия";
            // 
            // tbLicoPhone3
            // 
            this.tbLicoPhone3.Location = new System.Drawing.Point(97, 196);
            this.tbLicoPhone3.MaxLength = 11;
            this.tbLicoPhone3.Name = "tbLicoPhone3";
            this.tbLicoPhone3.Size = new System.Drawing.Size(371, 20);
            this.tbLicoPhone3.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 203);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(72, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Телефон №3";
            // 
            // tbLicoPhone2
            // 
            this.tbLicoPhone2.Location = new System.Drawing.Point(97, 160);
            this.tbLicoPhone2.MaxLength = 11;
            this.tbLicoPhone2.Name = "tbLicoPhone2";
            this.tbLicoPhone2.Size = new System.Drawing.Size(371, 20);
            this.tbLicoPhone2.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 167);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Телефон №2";
            // 
            // tbLicoIm
            // 
            this.tbLicoIm.Location = new System.Drawing.Point(97, 52);
            this.tbLicoIm.MaxLength = 20;
            this.tbLicoIm.Name = "tbLicoIm";
            this.tbLicoIm.Size = new System.Drawing.Size(371, 20);
            this.tbLicoIm.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Имя";
            // 
            // tbLicoOtch
            // 
            this.tbLicoOtch.Location = new System.Drawing.Point(97, 89);
            this.tbLicoOtch.MaxLength = 30;
            this.tbLicoOtch.Name = "tbLicoOtch";
            this.tbLicoOtch.Size = new System.Drawing.Size(371, 20);
            this.tbLicoOtch.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 96);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Отчество";
            // 
            // tbLicoPhone1
            // 
            this.tbLicoPhone1.Location = new System.Drawing.Point(97, 125);
            this.tbLicoPhone1.MaxLength = 11;
            this.tbLicoPhone1.Name = "tbLicoPhone1";
            this.tbLicoPhone1.Size = new System.Drawing.Size(371, 20);
            this.tbLicoPhone1.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Телефон №1";
            // 
            // tbLicoFam
            // 
            this.tbLicoFam.Location = new System.Drawing.Point(97, 17);
            this.tbLicoFam.MaxLength = 30;
            this.tbLicoFam.Name = "tbLicoFam";
            this.tbLicoFam.Size = new System.Drawing.Size(371, 20);
            this.tbLicoFam.TabIndex = 3;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 24);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(56, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Фамилия";
            // 
            // tabPageTypeSklad
            // 
            this.tabPageTypeSklad.Controls.Add(this.tbTypeSkladName);
            this.tabPageTypeSklad.Controls.Add(this.label21);
            this.tabPageTypeSklad.ImageIndex = 1;
            this.tabPageTypeSklad.Location = new System.Drawing.Point(4, 23);
            this.tabPageTypeSklad.Name = "tabPageTypeSklad";
            this.tabPageTypeSklad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTypeSklad.Size = new System.Drawing.Size(486, 386);
            this.tabPageTypeSklad.TabIndex = 3;
            this.tabPageTypeSklad.Text = "Тип склада";
            this.tabPageTypeSklad.UseVisualStyleBackColor = true;
            // 
            // tbTypeSkladName
            // 
            this.tbTypeSkladName.Location = new System.Drawing.Point(98, 17);
            this.tbTypeSkladName.MaxLength = 200;
            this.tbTypeSkladName.Name = "tbTypeSkladName";
            this.tbTypeSkladName.Size = new System.Drawing.Size(371, 20);
            this.tbTypeSkladName.TabIndex = 5;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(9, 24);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(83, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Наименование";
            // 
            // tabPagePostav
            // 
            this.tabPagePostav.Controls.Add(this.dtPostavPrice);
            this.tabPagePostav.Controls.Add(this.label23);
            this.tabPagePostav.Controls.Add(this.chbPostavActual);
            this.tabPagePostav.Controls.Add(this.tbPostavName);
            this.tabPagePostav.Controls.Add(this.label22);
            this.tabPagePostav.ImageIndex = 5;
            this.tabPagePostav.Location = new System.Drawing.Point(4, 23);
            this.tabPagePostav.Name = "tabPagePostav";
            this.tabPagePostav.Size = new System.Drawing.Size(486, 386);
            this.tabPagePostav.TabIndex = 6;
            this.tabPagePostav.Text = "Поставщик";
            this.tabPagePostav.UseVisualStyleBackColor = true;
            // 
            // dtPostavPrice
            // 
            this.dtPostavPrice.Enabled = false;
            this.dtPostavPrice.Location = new System.Drawing.Point(126, 77);
            this.dtPostavPrice.Name = "dtPostavPrice";
            this.dtPostavPrice.Size = new System.Drawing.Size(200, 20);
            this.dtPostavPrice.TabIndex = 11;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(10, 84);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(110, 13);
            this.label23.TabIndex = 10;
            this.label23.Text = "Последний прайс от";
            // 
            // chbPostavActual
            // 
            this.chbPostavActual.AutoSize = true;
            this.chbPostavActual.Location = new System.Drawing.Point(99, 48);
            this.chbPostavActual.Name = "chbPostavActual";
            this.chbPostavActual.Size = new System.Drawing.Size(145, 17);
            this.chbPostavActual.TabIndex = 9;
            this.chbPostavActual.Text = "актуальный поставщик";
            this.chbPostavActual.UseVisualStyleBackColor = true;
            // 
            // tbPostavName
            // 
            this.tbPostavName.Location = new System.Drawing.Point(99, 15);
            this.tbPostavName.MaxLength = 100;
            this.tbPostavName.Name = "tbPostavName";
            this.tbPostavName.Size = new System.Drawing.Size(371, 20);
            this.tbPostavName.TabIndex = 8;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(10, 22);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(83, 13);
            this.label22.TabIndex = 7;
            this.label22.Text = "Наименование";
            // 
            // frmElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 463);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmElement";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавить";
            this.Load += new System.EventHandler(this.frmElement_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageTovar.ResumeLayout(false);
            this.tabPageTovar.PerformLayout();
            this.tabPageProvider.ResumeLayout(false);
            this.tabPageProvider.PerformLayout();
            this.tabPageSklad.ResumeLayout(false);
            this.tabPageSklad.PerformLayout();
            this.tabPageOtvLico.ResumeLayout(false);
            this.tabPageOtvLico.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageTypeSklad.ResumeLayout(false);
            this.tabPageTypeSklad.PerformLayout();
            this.tabPagePostav.ResumeLayout(false);
            this.tabPagePostav.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageTovar;
        private System.Windows.Forms.TabPage tabPageProvider;
        private System.Windows.Forms.TabPage tabPageSklad;
        private System.Windows.Forms.TabPage tabPageOtvLico;
        private System.Windows.Forms.TabPage tabPageTypeSklad;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSkladName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox tbSkladComment;
        private System.Windows.Forms.TextBox tbTovarName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbLicoFam;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbTypeSkladName;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.RichTextBox tbProviderComment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbProviderName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSkladOtvets;
        private System.Windows.Forms.ComboBox cbSkladType;
        private System.Windows.Forms.CheckBox chSkladOsnovnoy;
        private System.Windows.Forms.TextBox tbSkladCodePoint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLicoAdreRg;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbLicoAdrePr;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbLicoSkald;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbLicoDocNum;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbLicoDocSer;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbLicoPhone3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbLicoPhone2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbLicoIm;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbLicoOtch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbLicoPhone1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btAdresPr;
        private System.Windows.Forms.Button btAdresReg;
        private System.Windows.Forms.TextBox tbSkladAnaliz;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage tabPagePostav;
        private System.Windows.Forms.CheckBox chbPostavActual;
        private System.Windows.Forms.TextBox tbPostavName;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DateTimePicker dtPostavPrice;
        private System.Windows.Forms.Label label23;
    }
}