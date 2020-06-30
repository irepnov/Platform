using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.RegManager;

namespace ConfGrid
{
    public partial class frmOptionInter : Form
    {
        private string _SoftName = "";
        private string _AssemblyName = "";
        private GGPlatform.RegManager.RegManag _reg = new RegManag();
        private string _p = null;
        public frmOptionInter(object[] inParams)
        {
            InitializeComponent();

            _SoftName = (string)inParams[0];
            _AssemblyName = (string)inParams[1];

            if ((_SoftName != "")&&(_AssemblyName != ""))
            {
                _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _AssemblyName + "\\Options";
                object _min = null;
                _min = _reg.GGRegistryGetValue("MinDateRFA", _p);
                if (_min != null)
                    dateTimePicker1.Value = Convert.ToDateTime(_min);
            }           
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            _reg.GGRegistrySetValue("MinDateRFA", dateTimePicker1.Value.Date, _p);
            _reg = null;
            this.DialogResult = DialogResult.OK;
        }
    }
}
