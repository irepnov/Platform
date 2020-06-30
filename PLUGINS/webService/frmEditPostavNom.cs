using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace webService
{
    public partial class frmEditPostavNom : Form
    {

        private Int32 _ID = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        public frmEditPostavNom(DataRow inDR, Int32 _inID, DBSqlServer _inDB)
        {
            InitializeComponent();

            _dr = inDR;
            _ID = _inID;
            _dbsql = _inDB;

            tbName.Text = _dr["Name"].ToString();
            tbPrice.Text = _dr["PricePostav"].ToString();

            LoadDataCLB();
            SetCheckedCLB(_dr["RodnNomenRef"], ref cbRName);
            SetCheckedCLB(_dr["PostavRef"], ref cbPostav);
        }

        private void LoadDataCLB()
        {
            cbRName.Items.Clear();
            cbPostav.Items.Clear();

            _dbsql.SQLScript = "select id, name from uchRodnNomen order by 2";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbRName);

            _dbsql.SQLScript = "select id, name from rfcPostav order by 2";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbPostav);
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
                if (cbPostav.Text == "")
                {
                    ActiveControl = cbPostav;
                    throw new Exception("Необходимо указать поставщика");
                }
                if (tbName.Text == "")
                {
                    ActiveControl = tbName;
                    throw new Exception("Необходимо указать наименование товара поставщика");
                }
                if (cbRName.Text == "")
                {
                    ActiveControl = cbRName;
                    throw new Exception("Необходимо указать родное наименование");
                }
                if (tbPrice.Text == "")
                {
                    ActiveControl = tbPrice;
                    throw new Exception("Необходимо указать цену поставщика");
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
                    _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 7, @Name = @name, @PricePostav = @PricePostav, 
                                            @PostavRef = @PostavRef, @RodnNomenRef = @RodnNomenRef, @ID = @ID";
                    _dbsql.AddParameter("@name", EnumDBDataTypes.EDT_String, tbName.Text);
                    _dbsql.AddParameter("@PricePostav", EnumDBDataTypes.EDT_Decimal, tbPrice.Text);
                    _dbsql.AddParameter("@PostavRef", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbPostav));
                    _dbsql.AddParameter("@RodnNomenRef", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbRName));
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
    }
}
