using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GGPlatform.BuiltInApp
{
    public partial class frmShowLists : Form
    {
        private List<ListItem> _Lists = null;

        public frmShowLists(ref List<ListItem> inLists)
        {
            InitializeComponent();

            _Lists = inLists;
            clb.Items.Clear();
            if (_Lists != null)
            {
                foreach (ListItem item in _Lists)
                    clb.Items.Add(item.ListName, item.ListIsChecked);
            }
        }

        private void cbCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                cbCheck.Text = "снять отметки";
            }
            else
            {
                cbCheck.Text = "отметить всё";
            }

            for (int i = 0; i <= clb.Items.Count - 1; i++)
                clb.SetItemChecked(i, ((CheckBox)sender).Checked);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (_Lists == null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }

            //уберу пометки по предудущим спискам
            foreach (ListItem item in _Lists)
                item.ListIsChecked = false;

            //помучу выбранные списки
            foreach (object itemChecked in clb.CheckedItems)
            {
                foreach (ListItem item in _Lists)
                {
                    if (item.ListName == itemChecked.ToString())
                        item.ListIsChecked = true;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK; 
        }
    }
}
