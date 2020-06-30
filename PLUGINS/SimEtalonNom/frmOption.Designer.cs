namespace SimEtalonNom
{
    partial class frmOption
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
            this.rbSebest = new System.Windows.Forms.RadioButton();
            this.rbPricePostav = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewGG1 = new DataGridViewGG.DataGridViewGG();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGG1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSebest);
            this.groupBox1.Controls.Add(this.rbPricePostav);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(582, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Выберите показатель, на основании которого будет производится расчет наценки";
            // 
            // rbSebest
            // 
            this.rbSebest.AutoSize = true;
            this.rbSebest.Location = new System.Drawing.Point(13, 42);
            this.rbSebest.Name = "rbSebest";
            this.rbSebest.Size = new System.Drawing.Size(131, 17);
            this.rbSebest.TabIndex = 1;
            this.rbSebest.TabStop = true;
            this.rbSebest.Text = "себестоимость факт";
            this.rbSebest.UseVisualStyleBackColor = true;
            // 
            // rbPricePostav
            // 
            this.rbPricePostav.AutoSize = true;
            this.rbPricePostav.Location = new System.Drawing.Point(13, 19);
            this.rbPricePostav.Name = "rbPricePostav";
            this.rbPricePostav.Size = new System.Drawing.Size(114, 17);
            this.rbPricePostav.TabIndex = 0;
            this.rbPricePostav.TabStop = true;
            this.rbPricePostav.Text = "цена поставщика";
            this.rbPricePostav.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(520, 388);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Отмена";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(439, 388);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "ОК";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewGG1);
            this.groupBox2.Location = new System.Drawing.Point(12, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(582, 290);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Конкуренты";
            // 
            // dataGridViewGG1
            // 
            this.dataGridViewGG1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGG1.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewGG1.Name = "dataGridViewGG1";
            this.dataGridViewGG1.Size = new System.Drawing.Size(570, 265);
            this.dataGridViewGG1.TabIndex = 0;
            // 
            // frmOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 423);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOption";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGG1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSebest;
        private System.Windows.Forms.RadioButton rbPricePostav;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.GroupBox groupBox2;
        private DataGridViewGG.DataGridViewGG dataGridViewGG1;
    }
}