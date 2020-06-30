using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace SimMoveT
{
    public partial class frmBillMove : Form
    {
        private int _ObjectID = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        private string _PluginAssembly = "";
        private string _SoftName = "";
        private int _SelectedTypeMoved = -1;
        public frmBillMove()
        {
            InitializeComponent();
        }

        public frmBillMove(int inObjectID, DataRow inDataRow, DBSqlServer inDBSqlServer, string inPluginAssembly, string inSoftName)
        {
            InitializeComponent();
            _ObjectID = inObjectID;
            _dr = inDataRow;
            _dbsql = inDBSqlServer;
            _PluginAssembly = inPluginAssembly;
            _SoftName = inSoftName;
        }

        private void tbCena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }

        private void tbKol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void LoadDataCLB()
        {
            cbSkladRecipient.Items.Clear();
            cbTovar.Items.Clear();
            cbSkladSender.Items.Clear();
            cbLiability.Items.Clear();

            _dbsql.SQLScript = "select id, name from rfcRepository order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbSkladRecipient);
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbSkladSender);

            _dbsql.SQLScript = "select id, name from rfcTypeProduct order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbTovar);

            _dbsql.SQLScript = "select id, Fam + ' ' + Im + ' ' + Otch as name from rfcLiability order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbLiability);

            if (_ObjectID > -1)
            {
                SetCheckedCLB(_dr["RepositoryRecipientRef"], ref cbSkladRecipient);
                SetCheckedCLB(_dr["TypeProductRef"], ref cbTovar);
                SetCheckedCLB(_dr["LiabilityRef"], ref cbLiability);
                SetCheckedCLB(_dr["RepositorySenderRef"], ref cbSkladSender);
            }
            if (_ObjectID == -1)
            {
                _dbsql.SQLScript = "select top 1 id from rfcRepository where isMainRepository = 1";
                string _id = null;
                _id = _dbsql.ExecuteScalar();
                if (_id != null)
                    SetCheckedCLB(Convert.ToInt32(_id), ref cbSkladSender);
            }
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

        private void frmBillIncome_Load(object sender, EventArgs e)
        {
            if (_ObjectID == -1)
            {
                this.Text = "Добавить накладную";
                _dbsql.SQLScript = "select isnull(max(convert(int, numberbill)) + 1, 1) as s from uchBillMove";
                string _id = null;
                _id = _dbsql.ExecuteScalar();
                if (_id != null)
                    tbNumberBill.Text = _id;

                _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();
            }
            else
            {
                this.Text = "Изменить накладную";
                tbNumberBill.Text = _dr["NumberBill"].ToString();
                dtDateMove.Value = (DateTime)_dr["DateMove"];
                tbKol.Text = _dr["CountProduct"].ToString();
                tbNumberBill.Enabled = false;
                dtDateMove.Enabled = false;
                tbKol.Enabled = false;
                btTovar.Enabled = false;
                cbTovar.Enabled = false;
            }
            LoadDataCLB();
        }

        private void tbKol_TextChanged(object sender, EventArgs e)
        {
            btTovar.Enabled = (((tbKol.Text != "") && (_ObjectID < 0) && (Convert.ToInt32(tbKol.Text) > 0)));
        }

        private void btTovar_Click(object sender, EventArgs e)
        {
            if (!UserCheck(0))
                return;

            Int32 _tovar = -1;
            Int32 _sender = -1;
            Int32 _liab = -1;

            if (GetCheckedCLB(ref cbTovar) != "")
                Int32.TryParse(GetCheckedCLB(ref cbTovar), out _tovar);

            if (GetCheckedCLB(ref cbSkladSender) != "")
                Int32.TryParse(GetCheckedCLB(ref cbSkladSender), out _sender);

            if (GetCheckedCLB(ref cbLiability) != "")
                Int32.TryParse(GetCheckedCLB(ref cbLiability), out _liab);

            frmTovar fmTovar = new frmTovar(_dbsql, Convert.ToInt32(tbKol.Text), _tovar, _sender, _liab, _PluginAssembly, _SoftName);
            if (fmTovar.ShowDialog() == DialogResult.OK)
                _SelectedTypeMoved = fmTovar._SelectedTypeMoved;

            fmTovar.Dispose();
            fmTovar = null;

            string _count = "0";
            _dbsql.SQLScript = "select count(id) as col from tmpMoveProduct";
            _count = _dbsql.ExecuteScalar();

            if (_SelectedTypeMoved != 1)//если перемещали по кол-ву
            {
                tbKol.Text = _count; //иначе поставлю выбранное кол-во в графу Кол-во
            }
        }

        public bool UserCheck(int _idtype)
        {
            bool _result = false;
            try
            {
                _result = true;
                if (tbNumberBill.Text == "")
                {
                    ActiveControl = tbNumberBill;
                    throw new Exception("Необходимо указать номер накладной");
                }
                if (dtDateMove.Text == "")
                {
                    ActiveControl = dtDateMove;
                    throw new Exception("Необходимо указать дату накладной");
                }
                if (cbTovar.Text == "")
                {
                    ActiveControl = cbTovar;
                    throw new Exception("Необходимо выбрать товар");
                }
                if (cbSkladSender.Text == "")
                {
                    ActiveControl = cbSkladSender;
                    throw new Exception("Необходимо выбрать склад отправитель");
                }
                if (cbSkladRecipient.Text == "")
                {
                    ActiveControl = cbSkladRecipient;
                    throw new Exception("Необходимо выбрать склад получатель");
                }
                if (tbKol.Text == "")
                {
                    ActiveControl = tbKol;
                    throw new Exception("Необходимо указать количество товара");
                }

                //if (_idtype == 0)  нажатие кнопки Выбрать товар

                if (_idtype == 1) //только перед сохранением Накладной
                {
                    if (_ObjectID < 0) //только при добавлении новой накладной
                    {
                        _dbsql.SQLScript = "select top 1 id from uchBillMove where NumberBill = @num and DateMove = @date";
                        _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                        _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateMove.Value.Date.ToShortDateString());
                        string d = _dbsql.ExecuteScalar();
                        if (d != null)
                        {
                            ActiveControl = tbNumberBill;
                            throw new Exception("Накладная с указанным номером и датой уже существует в базе");
                        }

                        //сделать проверки Если перемещали по Кол-ву, то сравнить кол-во
                        //если перемещали другими способами, то в граф Кол-во заменить данные кол-вом из БД
                        string _count = "0";
                        _dbsql.SQLScript = "select count(id) as col from tmpMoveProduct";
                        _count = _dbsql.ExecuteScalar();

                        if ((_count == "0") || (_count == null) || (_count == "") || (_count == "-1"))
                            throw new Exception("Не выбран товар подлежащий перемещению");

                        if (_SelectedTypeMoved == 1)//если перемещали по кол-ву
                        {                            
                            if (tbKol.Text != _count) //сравню
                                throw new Exception("Кол-во товара в накладной не соответствует количеству перемещенного товара");
                        } else
                        {
                            tbKol.Text = _count; //иначе поставлю выбранное кол-во в графу Кол-во
                        }
                    }
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

        private void btOK_Click(object sender, EventArgs e)
        {
            if (!UserCheck(1))
                return;
            try
            {
                btOK.Enabled = false;
                if (_ObjectID < 0)
                {
                    _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 4, @NumberBill = @num, @DateMove = @date, @RepositorySenderRef = @send, @RepositoryRecipientRef = @recip, @LiabilityRef = @liab, @TypeProductRef = @prod, @CountProduct = @coun";

                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateMove.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@send", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladSender));
                    _dbsql.AddParameter("@recip", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladRecipient));
                    _dbsql.AddParameter("@liab", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbLiability));                   
                    _dbsql.AddParameter("@prod", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbTovar));
                    _dbsql.AddParameter("@coun", EnumDBDataTypes.EDT_Integer, tbKol.Text);
                    _dbsql.ExecuteNonQuery(); 
                }
                else
                {
                    _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 7, @NumberBill = @num, @DateMove = @date, @RepositorySenderRef = @send, @RepositoryRecipientRef = @recip, @LiabilityRef = @liab, @TypeProductRef = @prod, @CountProduct = @coun, @ID = @idd";

                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateMove.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@send", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladSender));
                    _dbsql.AddParameter("@recip", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladRecipient));
                    _dbsql.AddParameter("@liab", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbLiability));
                    _dbsql.AddParameter("@prod", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbTovar));
                    _dbsql.AddParameter("@coun", EnumDBDataTypes.EDT_Integer, tbKol.Text);
                    _dbsql.AddParameter("@idd", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
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

    }
}
