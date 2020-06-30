using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace SimRodnNom
{
    public partial class frmEditRodn : Form
    {
        private Int32 _ID = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        public frmEditRodn(DataRow inDR, Int32 _inID, DBSqlServer _inDB)
        {
            InitializeComponent();

            _dr = inDR;
            _ID = _inID;
            _dbsql = _inDB;

            tbSebest.Text = _dr["SebestFact"].ToString();
            tbRName.Text = _dr["Name"].ToString();
            chbAsort.Checked = (bool)_dr["isAssort"];
            tbRozn.Text = _dr["RoznPrice"].ToString();
            tbCountOrder.Text = _dr["CountOrder"].ToString();

            LoadDataCLB();
            SetCheckedCLB(_dr["EtalonNomenRef"], ref cbEName);
        }

        private void LoadDataCLB()
        {
            cbEName.Items.Clear();

            _dbsql.SQLScript = "select id, name from uchEtalonNomen order by 2";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbEName);
        }

        private void LoadCLB(DataTable inObjects, ref ComboBox inCLB)
        {
            if (inObjects == null)
                return;

            inCLB.ValueMember = "ID";
            inCLB.DisplayMember = "Name";
            inCLB.DataSource = inObjects;
            inCLB.SelectedValue = DBNull.Value;
        }

        private string GetCheckedCLB(ref ComboBox inCLB)
        {
            if ((inCLB.SelectedValue == null) || (inCLB.SelectedValue == DBNull.Value))
                return "";
            else return inCLB.SelectedValue.ToString();
        }

        private void SetCheckedCLB(object inObjects, ref ComboBox inCLB)
        {
            inCLB.SelectedValue = inObjects;
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (tbRName.Text == "")
                {
                    ActiveControl = tbRName;
                    throw new Exception("Необходимо указать родное наименование");
                }
                if (cbEName.Text == "")
                {
                    ActiveControl = cbEName;
                    throw new Exception("Необходимо указать эталонное наименование");
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
                    _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 7, @Name = @name, @SebestFact = @SebestFact, @CountOrder = @countorder,
                                            @RoznPrice = @RoznPrice, @isAssort = @isAssort, @EtalonNomenRef = @EtalonNomenRef, @ID = @ID";
                    _dbsql.AddParameter("@name", EnumDBDataTypes.EDT_String, tbRName.Text);
                    _dbsql.AddParameter("@SebestFact", EnumDBDataTypes.EDT_Decimal, tbSebest.Text);
                    _dbsql.AddParameter("@countorder", EnumDBDataTypes.EDT_Integer, tbCountOrder.Text);
                    _dbsql.AddParameter("@RoznPrice", EnumDBDataTypes.EDT_Decimal, tbRozn.Text);
                    _dbsql.AddParameter("@isAssort", EnumDBDataTypes.EDT_Bit, chbAsort.Checked.ToString());
                    _dbsql.AddParameter("@EtalonNomenRef", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbEName));
                    _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ID.ToString());
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

        private void tbAKB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }

        private void tbSebest_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }
    }
}
