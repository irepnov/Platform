using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSAttach
{
    public partial class frmPeriod : Form
    {
        public frmPeriod()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbMonth.Text == "")
            {
                ActiveControl = cbMonth;
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
