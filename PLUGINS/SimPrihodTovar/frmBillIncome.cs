using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace SimPrihodT
{
    public partial class frmBillIncome : Form
    {
        private int _ObjectID = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        private Int32 _SelectedTypeTovar = -1;
        public frmBillIncome()
        {
            InitializeComponent();
        }

        public frmBillIncome(int inObjectID, DataRow inDataRow, DBSqlServer inDBSqlServer)
        {
            InitializeComponent();
            _ObjectID = inObjectID;
            _dr = inDataRow;
            _dbsql = inDBSqlServer;
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
            cbSklad.Items.Clear();
            cbTovar.Items.Clear();
            cbProvider.Items.Clear();

            _dbsql.SQLScript = "select id, name from rfcRepository order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbSklad);

            _dbsql.SQLScript = "select id, name from rfcTypeProduct order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbTovar);

            _dbsql.SQLScript = "select id, name from rfcProvider order by name";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbProvider);

            if (_ObjectID > -1)
            {
                SetCheckedCLB(_dr["RepositoryRecipientRef"], ref cbSklad);
               // SetCheckedCLB(_dr["TypeProductRef"], ref cbTovar);
                SetCheckedCLB(_dr["ProviderRef"], ref cbProvider);
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
                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();
            }
            else
            {
                this.Text = "Изменить накладную";
                tbNumberBill.Text = _dr["NumberBill"].ToString();
                dtDateBill.Value = (DateTime)_dr["DateBill"];
                dtPostup.Value = (DateTime)_dr["DateIncomeRepository"];
                //  tbCena.Text = _dr["PriceProduct"].ToString();
                tbCena.Enabled = false;
                cbTovar.Enabled = false;
                tbProdName.Enabled = false;
                tbKol.Text = _dr["CountProduct"].ToString();
                tbKol.Enabled = false;
                tbNumberBill.Enabled = false;
                dtDateBill.Enabled = false;
                btTovar.Enabled = false;
            }
            LoadDataCLB();
        }

        private void tbKol_TextChanged(object sender, EventArgs e)
        {
            decimal _itog, _cena, _kol;
            _itog = _cena = _kol = 0;
            if (tbCena.Text != "")
                _cena = Convert.ToDecimal(tbCena.Text.Replace(",", "."));
            if (tbKol.Text != "")
                _kol = Convert.ToDecimal(tbKol.Text.Replace(",", "."));
            _itog = _cena * _kol;
            tbItog.Text = _itog.ToString();

           // btTovar.Enabled = (((tbKol.Text != "") && (_ObjectID < 0) && (Convert.ToInt32(tbKol.Text) > 0)));
         }

        private void FillControl()
        {
            DataTable _dt = null;
            try
            {
                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 7";
                _dt = _dbsql.FillDataSet().Tables[0];

                if (_dt.Rows.Count > 0)
                    throw new Exception("В загруженном файле присутствует товар, который отсутствует в справочнике номенклатур товара\nОбновите справочник номенклатуры товара\nПовторите обработку");

                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 8";
                _dt = _dbsql.FillDataSet().Tables[0];

                if (_dt.Rows.Count == 0)
                    throw new Exception("В загруженном файле отсутствует список товара");

                if (_dt.Rows.Count > 1)
                    throw new Exception("В загруженном файле присутсвует несколько разных накладных");

                tbNumberBill.Text = _dt.Rows[0]["NumberBillIncome"].ToString();
                dtDateBill.Value = Convert.ToDateTime(_dt.Rows[0]["DateBillIncome"]);
                tbKol.Text = _dt.Rows[0]["CountProd"].ToString();
                tbCena.Text = "0";
                tbItog.Text = _dt.Rows[0]["SumPriceProduct"].ToString();

                if (Convert.ToInt32(_dt.Rows[0]["TypeProductCount"]) == 1)
                {
                    _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 9";
                    _dt = _dbsql.FillDataSet().Tables[0];

                    tbProdName.Text = _dt.Rows[0]["ProductName"].ToString();
                    tbProdName.Text = _dt.Rows[0]["ProductName"].ToString();
                    tbProdName.Text = _dt.Rows[0]["ProductName"].ToString();
                    SetCheckedCLB(_dt.Rows[0]["TypeProductRef"].ToString(), ref cbTovar);
                    tbCena.Text = _dt.Rows[0]["PriceProduct"].ToString();
                }
                //блокирую выбор этих полей
                tbProdName.Enabled = false;
                cbTovar.Enabled = false;
                tbCena.Enabled = false;
                tbNumberBill.Enabled = false;
                dtDateBill.Enabled = false;
                tbKol.Enabled = false;
            }
            catch (Exception ex)
            {
                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                _dt.Dispose();
                _dt = null;
            }           
        }

        private void btTovar_Click(object sender, EventArgs e)
        {
            /*if (tbKol.Text == "")
                return;*/

            Int32 _c = 0;
            if (tbKol.Text != "")
                _c = Convert.ToInt32(tbKol.Text);
            frmTovar fmTovar = new frmTovar(_dbsql, _c, 
                                            GetCheckedCLB(ref cbTovar) == "" ? -1 : Convert.ToInt32(GetCheckedCLB(ref cbTovar)), 
                                            Convert.ToDouble(tbCena.Text), 
                                            tbProdName.Text.Trim());
            if (fmTovar.ShowDialog() == DialogResult.OK)
               _SelectedTypeTovar = fmTovar._SelectedType;

            fmTovar.Dispose();

            if (_SelectedTypeTovar == 0) ///если загружали из Учсудя, то заполню пола формы
                FillControl();
        }

        public bool UserCheck()
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
                if (dtDateBill.Text == "")
                {
                    ActiveControl = dtDateBill;
                    throw new Exception("Необходимо указать дату накладной");
                }
                if ((cbTovar.Text == "") && (_SelectedTypeTovar == 1) && (_ObjectID < 0))
                {
                    ActiveControl = cbTovar;
                    throw new Exception("Необходимо выбрать товар");
                }
                if (cbProvider.Text == "")
                {
                    ActiveControl = cbProvider;
                    throw new Exception("Необходимо выбрать оператора");
                }
                if (cbSklad.Text == "")
                {
                    ActiveControl = cbSklad;
                    throw new Exception("Необходимо выбрать склад");
                }
                if (dtPostup.Text == "")
                {
                    ActiveControl = dtPostup;
                    throw new Exception("Необходимо указать дату поступления на склад");
                }
                if ((tbCena.Text == "") && (_SelectedTypeTovar == 1) && (_ObjectID < 0))
                {
                    ActiveControl = tbCena;
                    throw new Exception("Необходимо указать цену за единицу товара");
                }
                if ((tbKol.Text == "") && (_SelectedTypeTovar == 1))
                {
                    ActiveControl = tbKol;
                    throw new Exception("Необходимо указать количество товара");
                }

                if (_ObjectID < 0)
                {
                    _dbsql.SQLScript = "select top 1 id from uchBillIncome where NumberBill = @num and DateBill = @date";
                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateBill.Value.Date.ToShortDateString());
                    string d = _dbsql.ExecuteScalar();
                    if (d != null)
                    {
                        ActiveControl = tbNumberBill;
                        throw new Exception("Накладная с указанным номером и датой уже существует в базе");
                    }

                    string _count = "0";
                    _dbsql.SQLScript = "select count(id) as col from tmpIncomeProduct";
                    _count = _dbsql.ExecuteScalar();
                    if (tbKol.Text != _count)
                        throw new Exception("Кол-во товара в накладной не соответствует оприходованному товару");
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
            if (!UserCheck())
                return;
            try
            {
                btOK.Enabled = false;
                if (_ObjectID < 0)
                {
                    _dbsql.SQLScript = @"exec uchIncomeBill @IDOperation = 3, @NumberBill = @num, @DateBill = @date, @ProviderRef = @prov, 
                                            @DateIncomeRepository = @dateinc, @RepositoryRecipientRef = @rep, @TypeProductRef = @prod, 
                                            @PriceProduct = @cena, @CountProduct = @coun, @ProductName = @prrname";

                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateBill.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbProvider));
                    _dbsql.AddParameter("@dateinc", EnumDBDataTypes.EDT_DateTime, dtPostup.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@rep", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSklad));
                    _dbsql.AddParameter("@prod", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbTovar));
                    _dbsql.AddParameter("@cena", EnumDBDataTypes.EDT_Decimal, tbCena.Text);
                    _dbsql.AddParameter("@coun", EnumDBDataTypes.EDT_Integer, tbKol.Text);
                    _dbsql.AddParameter("@prrname", EnumDBDataTypes.EDT_String, tbProdName.Text.Trim());
                    _dbsql.ExecuteNonQuery();
                }
                else
                {
                    _dbsql.SQLScript = @"exec uchIncomeBill @IDOperation = 5, @NumberBill = @num, @DateBill = @date, @ProviderRef = @prov, 
                                            @DateIncomeRepository = @dateinc, @RepositoryRecipientRef = @rep, @TypeProductRef = @prod, 
                                            @PriceProduct = @cena, @CountProduct = @coun, @ID = @idob, @ProductName = @prrname";

                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_String, tbNumberBill.Text.Trim());
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, dtDateBill.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbProvider));
                    _dbsql.AddParameter("@dateinc", EnumDBDataTypes.EDT_DateTime, dtPostup.Value.Date.ToShortDateString());
                    _dbsql.AddParameter("@rep", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSklad));
                    _dbsql.AddParameter("@prod", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbTovar));
                    _dbsql.AddParameter("@cena", EnumDBDataTypes.EDT_Decimal, tbCena.Text);
                    _dbsql.AddParameter("@coun", EnumDBDataTypes.EDT_Integer, tbKol.Text);
                    _dbsql.AddParameter("@idob", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                    _dbsql.AddParameter("@prrname", EnumDBDataTypes.EDT_String, tbProdName.Text.Trim());
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
