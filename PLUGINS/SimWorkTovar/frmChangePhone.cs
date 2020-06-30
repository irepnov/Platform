using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace SimWorkT
{
    public partial class frmChangePhone : Form
    {
        private Int32 _ID = -1;
        private DBSqlServer _dbsql = null;
        public frmChangePhone(string _NumberPhone, Int32 _inID, DBSqlServer _inDB)
        {
            InitializeComponent();

            tbPhone.Text = _NumberPhone;
            _ID = _inID;
            _dbsql = _inDB;
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (tbPhone.Text == "")
                {
                    ActiveControl = tbPhone;
                    throw new Exception("Необходимо указать номер телефона");
                }
                if (tbPhone.Text.Length != 11)
                {
                    ActiveControl = tbPhone;
                    throw new Exception("Номер телефона должен содержать 11 цифр");
                }
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;
            try
            {
                btOK.Enabled = false;
                if (_ID > 0)
                {
                    _dbsql.SQLScript = "update uchProduct set NumberPhone = @num where ID = @id";
                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbPhone.Text.Trim());
                    _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, _ID.ToString());
                    _dbsql.ExecuteNonQuery();
                }
                btOK.Enabled = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                btOK.Enabled = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }
    }
}
