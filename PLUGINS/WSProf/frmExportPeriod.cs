using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSProf
{
    public partial class frmExportPeriod : Form
    {
        public frmExportPeriod()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbMonth.Text == "" && cbMonth2.Text == "" && cbMonthPlan_1.Text == "" && cbMonthPlan_2.Text == "")
            {
                ActiveControl = cbMonth;
                return;
            }

            if (cbMonth.Text != "" && cbMonth2.Text == "")
            {
                ActiveControl = cbMonth2;
                return;
            }

            if (cbMonth2.Text != "" && cbMonth.Text == "")
            {
                ActiveControl = cbMonth;
                return;
            }

            if (cbMonthPlan_1.Text != "" && cbMonthPlan_2.Text == "")
            {
                ActiveControl = cbMonthPlan_2;
                return;
            }

            if (cbMonthPlan_2.Text != "" && cbMonthPlan_1.Text == "")
            {
                ActiveControl = cbMonthPlan_1;
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
