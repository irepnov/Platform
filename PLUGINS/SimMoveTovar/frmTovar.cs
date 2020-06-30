using System;
using System.IO;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;
using System.Data.SqlClient;
using System.Drawing;
using GGPlatform.InspectorManagerSP;
using System.Collections.Generic;
using System.Data;

namespace SimMoveT
{
    public partial class frmTovar : Form
    {
        private DBSqlServer _dbsql = null;
        private ExcelManager _excel = null;
        private Int32 _Count = 0;
        private Int32 _ProductID = -1;
        private Int32 _SenderID = -1;
        private Int32 _LiabilityID = -1;
        private string _PluginAssembly = "";
        private string _SoftName = "";

        private CInspectorManager oCInspectorManagerlater = null;

        public Int32 _SelectedTypeMoved = -1;

        public frmTovar(DBSqlServer inDBSql, Int32 inCount, Int32 inProductID, Int32 inSenderID, Int32 inLiabilityID, string inPluginAssembly, string inSoftName)
        {
            InitializeComponent();
            _dbsql = inDBSql;
            _Count = inCount;
            _ProductID = inProductID;
            _SenderID = inSenderID;
            _LiabilityID = inLiabilityID;
            _PluginAssembly = inPluginAssembly;
            _SoftName = inSoftName;
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        {
                            _result = true;
                            if (tbFiles.Text == "")
                            {
                                ActiveControl = tbFiles;
                                throw new Exception("Необходимо выбрать импортируемый файл");
                            }
                            break;
                        }
                    case 1:
                        {
                            _result = true;
                            if (tbFirst.Text == "")
                            {
                                ActiveControl = tbFirst;
                                throw new Exception("Выбор товара по кол-ву не состоялся");
                            }
                            if (tbLast.Text == "")
                            {
                                ActiveControl = tbLast;
                                throw new Exception("Выбор товара по кол-ву не состоялся");
                            }
                            break;
                        }
                    case 2:
                        {
                            _result = true;
                            if (tbFirstICC_kol.Text == "")
                            {
                                ActiveControl = tbFirstICC_kol;
                                throw new Exception("Необходимо указать первый номер ICC");
                            }
                            if (tbLastICC_kol.Text == "")
                            {
                                ActiveControl = tbLastICC_kol;
                                throw new Exception("Необходимо указать последний номер ICC");
                            }
                            break;
                        }
                    case 3:
                        {
                            _result = true;
                            break;
                        }
                }

                //if ()
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private bool LoadProductXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                int _RowIndex = 2;
                string _NumberCard = "";
                lbProtocol.Items.Add("загрузка сведений...");
                while ( _excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberCard = _excel.GetValue("A" + _RowIndex.ToString()).Trim();

                    _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 11, @NumberCard = @card";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString() );
                lbProtocol.Items.Add("закрываем файл...");
                _result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
                if (_excel != null)
                    _excel.CloseDocument();
                _excel = null;                                 
            }
            return _result;            
        }

        private bool LoadProduct()
        {
            bool _result = false;
            try
            {
                _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 3, @FirstICC = @first, @LastICC = @last, @RepositorySenderRef = @send, @TypeProductRef = @prod";

                _dbsql.AddParameter("@first", EnumDBDataTypes.EDT_String, tbFirstICC_kol.Text);
                _dbsql.AddParameter("@last", EnumDBDataTypes.EDT_String, tbLastICC_kol.Text);
                _dbsql.AddParameter("@send", EnumDBDataTypes.EDT_Integer, _SenderID.ToString());
                _dbsql.AddParameter("@prod", EnumDBDataTypes.EDT_Integer, _ProductID.ToString());
                _dbsql.ExecuteNonQuery();

                _result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
            }
            return _result;
        }

        private void btFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Перемещаемый товар|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFiles.Text = openFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            DialogResult _result = DialogResult.Cancel;

            try
            {
                bool _res = false;

                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        {
                            _res = LoadProductXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 1:
                        {
                            _res = ((tbFirst.Text != "") && (tbLast.Text != "")); //проверю, сработала ли хранимка и вернула ли номера
                            break;
                        }
                    case 2:
                        {
                            // tbLastICC_kol, tbFirstICC_kol
                            _res = LoadProduct(); //проверю, сработала ли хранимка и вернула ли номера
                            break;
                        }
                    case 3:
                        {
                            _res = ((_dgCheck.dataGridViewGG.DataSource != null) && (_dgSelected.dataGridViewGG.DataSource != null));
                            break;
                        }
                }
                           
                if (!_res)
                    throw new Exception("Перемещение товара не осуществлено");

                if (_res)
                    MessageBox.Show("Товар перемещен", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _SelectedTypeMoved = tabControl1.SelectedIndex;
                _result = DialogResult.OK;
            }
            catch (Exception ex)
            {
                _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = DialogResult.Cancel;
            }
            finally
            {
                
            }
            this.DialogResult = _result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 1";
            _dbsql.ExecuteNonQuery();

            this.DialogResult = DialogResult.Cancel;
        }

        private void frmTovar_Shown(object sender, EventArgs e)
        {
            try
            {
                tabControl1_Selected(null, null); //вызываю Выбор вкладки
            }
            catch (Exception ex)
            {
                _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 1";
            _dbsql.ExecuteNonQuery();

            _dgCheck.dataGridViewGG.GGSaveGridRegistry();
            _dgSelected.dataGridViewGG.GGSaveGridRegistry();

            _dgCheck.dataGridViewGG.Columns.Clear();
            _dgCheck.dataGridViewGG.DataSource = null;
            _dgSelected.dataGridViewGG.Columns.Clear();
            _dgSelected.dataGridViewGG.DataSource = null;

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        this.Width = 674;
                        this.Height = 466;
                        tbFiles.Focus();
                        break;
                    }
                case 1:
                    {
                        this.Width = 674;
                        this.Height = 466;
                        _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 2, @CountProduct = @cou, @TypeProductRef = @tov, @RepositorySenderRef = @sen";
                        _dbsql.AddParameter("@cou", EnumDBDataTypes.EDT_Integer, _Count.ToString());
                        _dbsql.AddParameter("@tov", EnumDBDataTypes.EDT_Integer, _ProductID.ToString());
                        _dbsql.AddParameter("@sen", EnumDBDataTypes.EDT_Integer, _SenderID.ToString());
                        _dbsql.ExecuteNonQuery();

                        _dbsql.SQLScript = "select min(numbercard) as icc1, max(numbercard) as icc2 from tmpMoveProduct";
                        SqlDataReader drr = _dbsql.ExecuteReader();
                        try 
                        {
                            if (drr.HasRows)
                            {
                                while (drr.Read())
                                {
                                    tbFirst.Text = drr.GetString(0);
                                    tbLast.Text = drr.GetString(1);
                                }
                            }
                        }
                        catch 
                        {
                            drr.Close();
                        }
                        finally
                        {
                            drr.Close();
                        }
                        
                        
                        break;
                    }
                case 2:
                    {
                        this.Width = 674;
                        this.Height = 466;
                        tbFirstICC_kol.Focus();
                        break;
                    }
                case 3:
                    {                        
                        this.Width = 1200;
                        this.Height = 700;
                        CreateInspector();
                        break;
                    }
            }
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                                      (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
        }

        private void CreateInspector()
        {
            propertyGridEx1.ShowCustomProperties = true;
            propertyGridEx1.Item.Clear();
            oCInspectorManagerlater = new CInspectorManager(ref propertyGridEx1, ref _dbsql);
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfInt, "Номер телефона", "Поле", "Номер телефона", "P.NUMBERPHONE", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfInt, "Номер ICC", "Поле", "Номер ICC", "P.NUMBERCARD", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.Simple, EInspectorTypes.TypeOfInt, "Номер прих. накл.", "Поле", "Номер приходной накладной", "I.NUMBERBILL", "", "");
            oCInspectorManagerlater.Add(EInspectorModes.ListBox, EInspectorTypes.TypeOfListInt, "Склад отправитель", "Поле", "Склад отправитель", "I.REPOSITORYRECIPIENTREF", "select id, name from rfcRepository order by name", "");
            propertyGridEx1.Refresh();
        }

        private void tbFirstICC_kol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void panel5_Resize(object sender, EventArgs e)
        {
            //гриды на ширину формы 
            groupBox1.Height = (panel5.Height - panel6.Height) / 2;
            groupBox2.Height = (panel5.Height - panel6.Height) / 2;
            panel6.Width = panel5.Width;
            panel6.Location = new Point(0, groupBox1.Height);

            //кнопки перемещения на две части формы
           // panel6.Height = panel5.Height;
            btAdd.Width = panel5.Width / 2;
            btDel.Width = panel5.Width / 2;           
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> _dict = null;

            if (oCInspectorManagerlater != null)
                _dict = oCInspectorManagerlater.GetDictionaryValues();

            string _sql;
            string _phone;
            string _icc;
            string _bill;
            string _send; 
            string _where;

            _sql = _phone = _icc = _bill = _send = _where = "";

            _dict.TryGetValue("P.NUMBERPHONE", out _phone);
            _dict.TryGetValue("P.NUMBERCARD", out _icc);
            _dict.TryGetValue("I.NUMBERBILL", out _bill);
            _dict.TryGetValue("I.REPOSITORYRECIPIENTREF", out _send);

            if (_send == "") //без отправителя
                _sql = String.Format(@"select p.ID as ID, p.NumberCard, p.NumberPhone, i.NumberBill, i.DateBill from uchProduct p inner join uchBillIncome i on i.id = p.BillIncomeRef
                                       where p.NumberCard not in (select NumberCard from tmpMoveProduct) 
                                         and p.TypeProductRef = {0}
                                         and p.BillMoveRef is null
                                         and i.RepositoryRecipientRef = {1}", _ProductID, _SenderID);

            if (_send != "") //c отправителя
                _sql = String.Format(@"select p.ID as ID, p.NumberCard, p.NumberPhone, i.NumberBill, i.DateBill from uchProduct p inner join uchBillIncome i on i.id = p.BillIncomeRef inner join uchBillMove m on m.ID = p.BillMoveRef
                                       where p.NumberCard not in (select NumberCard from tmpMoveProduct) 
                                         and p.TypeProductRef = {0}
                                         and p.DateActive is null
                                         and p.DateSale is null
                                         and p.DateInputRFA is null
                                         and p.DateRFAToProvider is null
                                         and ( (m.LiabilityRef <> {1} and {2} <> -1) or ({3} = -1) )", _ProductID, _LiabilityID, _LiabilityID, _LiabilityID);

            if (_phone != "") _where = _where + String.Format(" and P.NUMBERPHONE = '{0}'", _phone);
            if (_icc != "") _where = _where + String.Format(" and P.NUMBERCARD = '{0}'", _icc);
            if (_bill != "") _where = _where + String.Format(" and I.NUMBERBILL = '{0}'", _bill);
            if (_send != "") _where = _where + String.Format(" and I.REPOSITORYRECIPIENTREF = '{0}'", _send);
            if (_where.Trim() != "") _sql = _sql + _where;
            _sql = _sql + " order by p.NumberCard";
                       
            _dgCheck.dataGridViewGG.AllowUserToAddRows = false;
            _dgCheck.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgCheck.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgCheck.dataGridViewGG.AutoGenerateColumns = false;            
            _dgCheck.dataGridViewGG.MultiSelect = false;
            _dgCheck.dataGridViewGG.GGRegistrySoftName = _SoftName;
            _dgCheck.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "Checked";
            _dgCheck.dataGridViewGG.Columns.Clear();
            _dgCheck.dataGridViewGG.GGAddColumn("IsCheckedField", "check", typeof(Boolean));
            _dgCheck.dataGridViewGG.GGAddColumn("NumberPhone", "Номер телефона", typeof(string)).ReadOnly = true;
            _dgCheck.dataGridViewGG.GGAddColumn("NumberCard", "Номер ICC", typeof(string)).ReadOnly = true;
            _dgCheck.dataGridViewGG.GGAddColumn("NumberBill", "Номер приходной накладной", typeof(string)).ReadOnly = true;
            _dgCheck.dataGridViewGG.GGAddColumn("DateBill", "Дата накладной", typeof(DateTime)).ReadOnly = true;
            _dgCheck.RefreshFilterFieldsGG();
            _dgCheck.dataGridViewGG.GGLoadGridRegistry();

            _dbsql.SQLScript = _sql;
            DataTable _dt = null;
            _dt = _dbsql.FillDataSet().Tables[0];
            BindingSource _bs = new BindingSource();
            _bs.DataSource = _dt;
            _dgCheck.dataGridViewGG.DataSource = _bs;
        }

        private void SelectedGridData(int inType)
        {
            string _numPhone = "";
            string _numCard = "";
            DataTable _dt = null;

            if (inType == 1) //Добавление номеров в список Отобранных
            {
                foreach (DataRow dr in _dgCheck.dataGridViewGG.GGGetCheckedDataRows())
                {
                    _numPhone = dr["NumberPhone"].ToString();
                    _numCard = dr["NumberCard"].ToString();

                    _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 9, @NumberCard = @card, @NumberPhone = @phone";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _numCard);
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _numPhone);
                    _dbsql.ExecuteNonQuery();
                }
            }

            if (inType == 2) //Удаление номеров из списка Отобранных
            {
                foreach (DataRow dr in _dgSelected.dataGridViewGG.GGGetCheckedDataRows())
                {
                    _numPhone = dr["NumberPhone"].ToString();
                    _numCard = dr["NumberCard"].ToString();

                    _dbsql.SQLScript = "exec uchMoveBill @IDOperation = 10, @NumberCard = @card, @NumberPhone = @phone";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _numCard);
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _numPhone);
                    _dbsql.ExecuteNonQuery();
                }
            }

            _dgSelected.dataGridViewGG.AllowUserToAddRows = false;
            _dgSelected.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgSelected.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgSelected.dataGridViewGG.AutoGenerateColumns = false;
            _dgSelected.dataGridViewGG.MultiSelect = false;
            _dgSelected.dataGridViewGG.GGRegistrySoftName = _SoftName;
            _dgSelected.dataGridViewGG.GGRegistryAssemblyName = _PluginAssembly + "_" + "Selected";
            _dgSelected.dataGridViewGG.Columns.Clear();
            _dgSelected.dataGridViewGG.GGAddColumn("IsCheckedField", "check", typeof(Boolean));
            _dgSelected.dataGridViewGG.GGAddColumn("NumberPhone", "Номер телефона", typeof(string)).ReadOnly = true;
            _dgSelected.dataGridViewGG.GGAddColumn("NumberCard", "Номер ICC", typeof(string)).ReadOnly = true;
            _dgSelected.RefreshFilterFieldsGG();
            _dgSelected.dataGridViewGG.GGLoadGridRegistry();

            _dbsql.SQLScript = "select NumberCard, NumberPhone from tmpMoveProduct order by NumberCard";

            _dt = _dbsql.FillDataSet().Tables[0];
            BindingSource _bs = new BindingSource();
            _bs.DataSource = _dt;
            _dgSelected.dataGridViewGG.DataSource = _bs;
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            SelectedGridData(1);
        }

        private void btDel_Click(object sender, EventArgs e)
        {
            SelectedGridData(2);
        }
    }
}
