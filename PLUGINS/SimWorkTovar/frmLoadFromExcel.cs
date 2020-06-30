using System.Windows.Forms;
using GGPlatform.DBServer;
using System;
using System.IO;
using GGPlatform.ExcelManagers;
using System.Data;

namespace SimWorkT
{
    public partial class frmLoadFromExcel : Form
    {
        private DBSqlServer _dbsql = null;
        private Int32 _ActionID = -1;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\template\\";
        public frmLoadFromExcel(object[] inParams)
        {
            InitializeComponent();
            _dbsql = (DBSqlServer)inParams[0];
            _ActionID = Convert.ToInt32(inParams[1]);

            if (_ActionID == 8)
            {
openFileDialog1.Multiselect = true;
                label2.Visible = true;
                dtRFA.Visible = true;

            }
                
            ClearTmpProduct();
        }

        private void btFiles_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Обработываемый товар|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (_ActionID == 8)
                    tbFiles.Text = String.Join(";", openFileDialog1.FileNames);
                    else
                        tbFiles.Text = openFileDialog1.FileName;
            }
        }

        private bool LoadSummXLS(string _inFiles)
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
                string _NumberPhone = "";
                string _SummAll = "0";
                string _SummPeriod = "0";
                string _IsActive = "0";
                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("F" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("E" + _RowIndex.ToString()).Trim();

                    _SummAll = _excel.GetValue("H" + _RowIndex.ToString()).Trim();
                    _SummPeriod = _excel.GetValue("I" + _RowIndex.ToString()).Trim();

                    if ((_NumberPhone == "-") || (_NumberPhone.Length == 0)
                            || (String.IsNullOrEmpty(_NumberPhone)) || (_NumberPhone.Trim() == ""))
                        _NumberPhone = "";

                    if ((_NumberCard == "-") || (_NumberCard.Length == 0)
                            || (String.IsNullOrEmpty(_NumberCard)) || (_NumberCard.Trim() == ""))
                        _NumberCard = "";

                    if ((_SummAll == "-") || (_SummAll.Length == 0)
                            || (String.IsNullOrEmpty(_SummAll)) || (_SummAll.Trim() == ""))
                        _SummAll = "0"; //установим время в 0 

                    if ((_SummPeriod == "-") || (_SummPeriod.Length == 0)
                            || (String.IsNullOrEmpty(_SummPeriod)) || (_SummPeriod.Trim() == ""))
                        _SummPeriod = "0"; //установим время в 0 

                    if (_SummPeriod != "0")
                        _IsActive = "1";

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, 
                                              @NumberCard = @card, @SumAll = @sa, @SumPeriod = @sp, @isActivePeriod = @ac";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@sa", EnumDBDataTypes.EDT_Decimal, _SummAll);
                    _dbsql.AddParameter("@sp", EnumDBDataTypes.EDT_Decimal, _SummPeriod);                   
                    _dbsql.AddParameter("@ac", EnumDBDataTypes.EDT_Bit, _IsActive);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
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

        private bool LoadChangeXLS(string _inFiles)
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
                string _Price = "";
                string _ProductName = "";
                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberCard = _excel.GetValue("A" + _RowIndex.ToString()).Trim();

                    _ProductName = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _Price = _excel.GetValue("C" + _RowIndex.ToString()).Trim();

                    if ((_ProductName == "-") || (_ProductName.Length == 0)
                            || (String.IsNullOrEmpty(_ProductName)) || (_ProductName.Trim() == ""))
                        _ProductName = "";

                    if ((_Price == "-") || (_Price.Length == 0)
                            || (String.IsNullOrEmpty(_Price)) || (_Price.Trim() == ""))
                        _Price = "";

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberCard = @card, @PriceProduct = @pr, @ProductName = @name";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_Decimal, _Price);
                    _dbsql.AddParameter("@name", EnumDBDataTypes.EDT_String, _ProductName);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
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

        private bool LoadActiveXLS(string _inFiles)
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
                string _NumberPhone = "";
                string _DateActivate = "";
                string _TimeActivate = "";
                string _TarifPlan = "";
                string _IMEI = "";
                string _NameStation = "";
                DateTime? _DateAct = null;
                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _DateActivate = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _TimeActivate = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _TarifPlan = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _IMEI = _excel.GetValue("F" + _RowIndex.ToString()).Trim();
                    _NameStation = _excel.GetValue("G" + _RowIndex.ToString()).Trim();

                    if ((_TarifPlan == "-") || (_TarifPlan.Length == 0)
                            || (String.IsNullOrEmpty(_TarifPlan)) || (_TarifPlan.Trim() == ""))
                        _TarifPlan = "";

                    if ((_IMEI == "-") || (_IMEI.Length == 0)
                            || (String.IsNullOrEmpty(_IMEI)) || (_IMEI.Trim() == ""))
                        _IMEI = "";

                    if ((_NameStation == "-") || (_NameStation.Length == 0)
                            || (String.IsNullOrEmpty(_NameStation)) || (_NameStation.Trim() == ""))
                        _NameStation = "";

                    if ((_TimeActivate == "-") || (_TimeActivate.Length == 0)  
                            || (String.IsNullOrEmpty(_TimeActivate)) || (_TimeActivate.Trim() == ""))
                        _TimeActivate = "0"; //установим время в 0 
                  
                    _DateActivate = _DateActivate.Replace("-", ".").Replace("/","."); //приведу Даты к единообразию

                    TimeSpan? _Time = null;
                    try
                    {
                        _Time = DateTime.FromOADate(Convert.ToDouble(_TimeActivate)).TimeOfDay;
                    }                    
                    catch { _Time = null; }

                    try
                    {
                        _DateAct = Convert.ToDateTime(_DateActivate) + _Time;  //получу Дата + Время
                    }                        
                    catch { _DateAct = null; }

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @DateActive = @activ, 
                                                          @NumberCard = @card, @IMEI = @im, @NameStation = @stat, @TarifPlan = @tar";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@activ", EnumDBDataTypes.EDT_DateTime, _DateAct.ToString());
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@im", EnumDBDataTypes.EDT_String, _IMEI);
                    _dbsql.AddParameter("@stat", EnumDBDataTypes.EDT_String, _NameStation);
                    _dbsql.AddParameter("@tar", EnumDBDataTypes.EDT_String, _TarifPlan);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
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

        private bool LoadExportXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                this.Cursor = Cursors.WaitCursor;

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                _excel.SetScreenUpdating(false);
                int _RowIndex = 1;
                string _NumberCard = "";
                string _NumberPhone = "";
                string _Number = "";
                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _Number = _excel.GetValue("A" + _RowIndex.ToString()).Trim();

                    _NumberCard = _NumberPhone = "";

                    if ((_Number == "-") || (_Number.Length == 0)
                            || (String.IsNullOrEmpty(_Number)) || (_Number.Trim() == ""))
                        _Number = "";

                    if (_Number.Length == 11)
                        _NumberPhone = _Number;  //в файле номера телефонов

                    if (_Number.Length == 20)
                        _NumberCard = _Number; //в файле номера АИСИСИ

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @NumberCard = @card, @OrderID = @ord";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@ord", EnumDBDataTypes.EDT_Integer, _RowIndex.ToString());
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
                lbProtocol.Items.Add("сопоставляем загруженную информация с базой данных...");

                /**/
                _dbsql.SQLScript = @"select t.OrderID, p.NumberPhone, p.NumberCard
                                     from tmpProduct t
                                        left join uchProduct p on (p.NumberPhone = t.NumberPhone and t.NumberPhone is not null)
                                                                   or (p.NumberCard = t.NumberCard and t.NumberCard is not null)
                                     order by t.OrderID";
                DataTable dt = _dbsql.FillDataSetVisual("Cопоставление загруженной информации...").Tables[0];                
                foreach(DataRow dr in dt.Rows)
                {
                    _excel.SetColumnWidth("B:C", 30);
                    _excel.SetFormat("B" + dr["OrderID"].ToString(), RangeFormat.xlTextFormat);
                    _excel.SetFormat("C" + dr["OrderID"].ToString(), RangeFormat.xlTextFormat);
                    _excel.SetFontBold("B" + dr["OrderID"].ToString());
                    _excel.SetFontBold("C" + dr["OrderID"].ToString());
                    _excel.SetValueToRange("B" + dr["OrderID"].ToString(), dr["NumberPhone"].ToString());
                    _excel.SetValueToRange("C" + dr["OrderID"].ToString(), dr["NumberCard"].ToString());
                }
                _excel.SetScreenUpdating(true);
                this.Cursor = Cursors.Default;
                //сохраняем выходим           
                if (_excel.SaveDocument("c:\\temp\\uchSopostavl.xls", true, ESaveFormats.xlNormal))
                    MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
                _excel = null;                
                _result = true;                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if (_excel != null)
                    _excel.CloseDocument();
                _excel = null;
            }
            return _result;
        }

        private bool LoadExportTovarSKLADXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                this.Cursor = Cursors.WaitCursor;

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                _excel.SetScreenUpdating(false);
                int _RowIndex = 2;
                string _NumberCard = "";
                string _NumberPhone = "";
                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("B" + _RowIndex.ToString()).Trim();

                    if ((_NumberPhone == "-") || (_NumberPhone.Length == 0)
                            || (String.IsNullOrEmpty(_NumberPhone)) || (_NumberPhone.Trim() == ""))
                        _NumberPhone = "";

                    if ((_NumberCard == "-") || (_NumberCard.Length == 0)
                            || (String.IsNullOrEmpty(_NumberCard)) || (_NumberCard.Trim() == ""))
                        _NumberCard = "";

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @NumberCard = @card";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
                lbProtocol.Items.Add("поиск загруженной информации в базе данных...");

                /**/
                _dbsql.SQLScript = @"exec uchProducts @IDOperation = 9";
                DataTable dt = _dbsql.FillDataSetVisual("Поиск загруженной информации...").Tables[0];
                _RowIndex = 2;
                foreach (DataRow dr in dt.Rows)
                {
                    _excel.SetValueToRange("C" + _RowIndex.ToString(), dr["Name"].ToString());
                    _RowIndex = _RowIndex + 1;
                }
                _excel.SetScreenUpdating(true);
                this.Cursor = Cursors.Default;
                //сохраняем выходим           
                if (_excel.SaveDocument("c:\\temp\\uchTovarSklad.xls", true, ESaveFormats.xlNormal))
                    MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
                _excel = null;
                _result = true;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if (_excel != null)
                    _excel.CloseDocument();
                _excel = null;
            }
            return _result;
        }

        private bool LoadRFA_WDXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                int _RowIndex = 11;
                string _NumberCard = "";
                string _NumberPhone = "";
                string _User = "";
                string _Abonent = "";

                string _DateRegistr = "";
                string _DateSale = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("B" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _User = _excel.GetValue("K" + _RowIndex.ToString()).Trim();
                    _Abonent = "отсутствует";
                    _DateRegistr = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _DateSale = _excel.GetValue("L" + _RowIndex.ToString()).Trim();

                    if ((_NumberPhone == "-") || (_NumberPhone.Length == 0)
                            || (String.IsNullOrEmpty(_NumberPhone)) || (_NumberPhone.Trim() == ""))
                        _NumberPhone = "";

                    if ((_NumberCard == "-") || (_NumberCard.Length == 0)
                            || (String.IsNullOrEmpty(_NumberCard)) || (_NumberCard.Trim() == ""))
                        _NumberCard = "";

                    if ((_User == "-") || (_User.Length == 0)
                            || (String.IsNullOrEmpty(_User)) || (_User.Trim() == ""))
                        _User = "";

                    if ((_DateRegistr == "-") || (_DateRegistr.Length == 0)
                            || (String.IsNullOrEmpty(_DateRegistr)) || (_DateRegistr.Trim() == ""))
                        _DateRegistr = "";

                    if ((_DateSale == "-") || (_DateSale.Length == 0)
                            || (String.IsNullOrEmpty(_DateSale)) || (_DateSale.Trim() == ""))
                        _DateSale = "";

                    _DateRegistr = _DateRegistr.Replace("-", ".").Replace("/", "."); //приведу Даты к единообразию
                    _DateSale = _DateSale.Replace("-", ".").Replace("/", "."); //приведу Даты к единообразию

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @DateSale = @dsale, 
                                            @NumberCard = @card, @FIOAbonent = @abon, @DateRegisterRFA = @dreg, @UserRemote = @user";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@dsale", EnumDBDataTypes.EDT_DateTime, _DateSale.ToString());
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@abon", EnumDBDataTypes.EDT_String, _Abonent);
                    _dbsql.AddParameter("@dreg", EnumDBDataTypes.EDT_String, _DateRegistr.ToString());
                    _dbsql.AddParameter("@user", EnumDBDataTypes.EDT_String, _User);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
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

        private bool LoadRFA_SdaniOperXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);

                string _tmp = _excel.GetValue("A1").Trim().ToUpper();

                if (!_tmp.Contains("АКТ"))
                    throw new Exception("Содержимое файла не соответствует эталонному шаблону");
                
                string _NumberCard = "";
                string _NumberPhone = "";
                string _DateRegistr = "";
                _DateRegistr = _excel.GetValue("D2").Trim();

                if ((_DateRegistr == "-") || (_DateRegistr.Length == 0)
                            || (String.IsNullOrEmpty(_DateRegistr)) || (_DateRegistr.Trim() == ""))
                     _DateRegistr = "";
                _DateRegistr = _DateRegistr.Replace("-", ".").Replace("/", "."); //приведу Даты к единообразию

                int _RowIndex = 19;
                int _LoadCount = 0;

                lbProtocol.Items.Add("загрузка сведений...");

                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("C" + _RowIndex.ToString()).Trim();

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @NumberCard = @card, @DateRFAToProvider = @dreg";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@dreg", EnumDBDataTypes.EDT_String, _DateRegistr.ToString());
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                    _LoadCount = _LoadCount + 1;
                }

                _RowIndex = 19;
                while (_excel.GetValue("D" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberPhone = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("F" + _RowIndex.ToString()).Trim();

                    _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @NumberCard = @card, @DateRFAToProvider = @dreg";
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@dreg", EnumDBDataTypes.EDT_String, _DateRegistr.ToString());
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                    _LoadCount = _LoadCount + 1;
                }

                lbProtocol.Items.Add("загружено записей " + _LoadCount.ToString());
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

        private bool LoadRFAPrin(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл " + _inFiles + " не найден");

                lbProtocol.Items.Add("открываем файл " + _inFiles + " для загрузки...");
                _excel = new ExcelManager(_inFiles);
                int _RowIndex = 2;
                string _Number = "";

                DateTime? _DatePrin = null;

                _excel.SetWorkSheetByIndex = 2;

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("B" + _RowIndex.ToString()).Trim() != "")
                {
                    _Number = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _DatePrin = dtRFA.Value.Date;

                    if ((_Number == "-") || (_Number.Length == 0)
                            || (String.IsNullOrEmpty(_Number)) || (_Number.Trim() == ""))
                        _Number = "";

                    if (_Number.Length == 20)
                    {
                        _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberCard = @card, @DateInputRFA = @dreg";
                        _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _Number);
                        _dbsql.AddParameter("@dreg", EnumDBDataTypes.EDT_String, _DatePrin.ToString());
                        _dbsql.ExecuteNonQuery();
                    }

                    if (_Number.Length == 11)
                    {
                        _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @card, @DateInputRFA = @dreg";
                        _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _Number);
                        _dbsql.AddParameter("@dreg", EnumDBDataTypes.EDT_String, _DatePrin.ToString());
                        _dbsql.ExecuteNonQuery();
                    }

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
                lbProtocol.Items.Add("закрываем файл " + _inFiles + " ...");


                _dbsql.SQLScript = "exec uchProducts @IDOperation = 8";
                DataTable _dt = _dbsql.FillDataSet().Tables[0];
                if ((_dt != null) && (_dt.Rows.Count > 0))
                {
                    lbProtocol.Items.Add("РАНЕЕ ПРИНЯТО В БД ИЛИ ОТСУТСТВУЮТ В БД " + _dt.Rows.Count.ToString() + " форм РФА");
                    GetRFADouble(_dt, _inFiles);
                }

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

        private bool GetRFADouble(DataTable inDT, string inFile)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(inFile))
                    throw new Exception("Указанный файл не найден");

                this.Cursor = Cursors.WaitCursor;

                _excel = new ExcelManager(Application.StartupPath + _templatename + "uchNotFound.xlt");
                _excel.SetScreenUpdating(false);
                

                _excel.SetValueToRange("A1", "По данным номерам РФА приняты ранее или отсутствуют в БД. Файл: " + inFile);
                _excel.SetValueToRange("C2", "Дата приема РФА");

                int _RowIndex = 3;
                foreach (DataRow dr in inDT.Rows)
                {
                    _excel.SetValueToRange("A" + _RowIndex.ToString(), "Птах");
                    _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberPhone"].ToString());
                    _excel.SetValueToRange("C" + _RowIndex.ToString(), dr["DateInputRFA"].ToString());
                    _excel.SetValueToRange("D" + _RowIndex.ToString(), dr["NumberCard"].ToString());
                    _excel.SetValueToRange("E" + _RowIndex.ToString(), dr["IMEI"].ToString());
                    _excel.SetValueToRange("F" + _RowIndex.ToString(), dr["NameStation"].ToString());
                    _excel.SetValueToRange("G" + _RowIndex.ToString(), dr["Status"].ToString());

                    _RowIndex = _RowIndex + 1;
                }
                
                _excel.SetScreenUpdating(true);
                this.Cursor = Cursors.Default;

                //сохраняем выходим           
                _excel.SaveDocument("c:\\temp\\uchNotFound_" + Path.GetFileNameWithoutExtension(inFile) + ".xls", true, ESaveFormats.xlNormal);
                                    //MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    //MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
                _excel = null;
                _result = true;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if (_excel != null)
                    _excel.CloseDocument();
                _excel = null;
            }
            return _result;
        }

        private bool CreateAct()
        {
            bool _result = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _excel = new ExcelManager(Application.StartupPath + _templatename + "uchActRFA.xlt");
                _excel.SetScreenUpdating(false);

                int _RowIndex = 0;
                int _RecNo = 1;
                _excel.SetWorkSheetByIndex = 1;

                _excel.SetValueToRange("A6", "Дата приема РФА - " + dtRFA.Value.Date.ToLongDateString());
                
                _dbsql.SQLScript = "select isnull(NumberCard, NumberPhone) as NumberCard from tmpProduct order by isnull(NumberCard, NumberPhone)";
                DataTable inDT = _dbsql.FillDataSet().Tables[0];
                
                _excel.SetValueToRange("A7", "Кол-во принятых РФА: " + inDT.Rows.Count.ToString());

                foreach (DataRow dr in inDT.Rows)
                {
                    if ((_RecNo == 1) || (_RecNo == 51) || (_RecNo == 101) || (_RecNo == 151)) _RowIndex = 15;
                    if ((_RecNo == 201) || (_RecNo == 266) || (_RecNo == 331) || (_RecNo == 396)) _RowIndex = 70;
                    if ((_RecNo == 461) || (_RecNo == 526) || (_RecNo == 591) || (_RecNo == 656)) _RowIndex = 140;
                    if ((_RecNo == 721) || (_RecNo == 786) || (_RecNo == 851) || (_RecNo == 916)) _RowIndex = 210;

                    if ((_RecNo >= 1) && (_RecNo <= 50)) _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 51) && (_RecNo <= 100)) _excel.SetValueToRange("D" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 101) && (_RecNo <= 150)) _excel.SetValueToRange("F" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 151) && (_RecNo <= 200)) _excel.SetValueToRange("H" + _RowIndex.ToString(), dr["NumberCard"]);

                    if ((_RecNo >= 201) && (_RecNo <= 265)) _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 266) && (_RecNo <= 330)) _excel.SetValueToRange("D" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 331) && (_RecNo <= 395)) _excel.SetValueToRange("F" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 396) && (_RecNo <= 460)) _excel.SetValueToRange("H" + _RowIndex.ToString(), dr["NumberCard"]);

                    if ((_RecNo >= 461) && (_RecNo <= 525)) _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 526) && (_RecNo <= 590)) _excel.SetValueToRange("D" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 591) && (_RecNo <= 655)) _excel.SetValueToRange("F" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 656) && (_RecNo <= 720)) _excel.SetValueToRange("H" + _RowIndex.ToString(), dr["NumberCard"]);

                    if ((_RecNo >= 721) && (_RecNo <= 785)) _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 786) && (_RecNo <= 850)) _excel.SetValueToRange("D" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 851) && (_RecNo <= 915)) _excel.SetValueToRange("F" + _RowIndex.ToString(), dr["NumberCard"]);
                    if ((_RecNo >= 916) && (_RecNo <= 980)) _excel.SetValueToRange("H" + _RowIndex.ToString(), dr["NumberCard"]);

                    _RowIndex = _RowIndex + 1;
                    _RecNo = _RecNo + 1;
                }

                _excel.SetWorkSheetByIndex = 2;
                _RowIndex = 2;
                foreach (DataRow dr in inDT.Rows)
                {
                    _excel.SetValueToRange("B" + _RowIndex.ToString(), dr["NumberCard"]);
                    _RowIndex = _RowIndex + 1;
                }

                _excel.SetWorkSheetByIndex = 1;

                _excel.SetScreenUpdating(true);
                this.Cursor = Cursors.Default;

                //сохраняем выходим           
                if (_excel.SaveDocument("c:\\temp\\uchActRFA__" + dtRFA.Value.Date.ToShortDateString().Replace(".","_") + ".xls", true, ESaveFormats.xlNormal))
                    MessageBox.Show("Акт приема РФА сформирован и сохранён в каталоге c:\\temp\\", "Информация",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _excel.CloseExcelDocumentAndShow();
                _excel = null;
                _result = true;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                if (_excel != null)
                    _excel.CloseDocument();
                _excel = null;
            }
            return _result;
        }

        private void ClearTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchProducts @IDOperation = 1";
            _dbsql.ExecuteNonQuery();
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (tbFiles.Text == "")
                {
                    ActiveControl = tbFiles;
                    throw new Exception("Необходимо выбрать импортируемый файл");
                }

                if ((dtRFA.Visible) && (dtRFA.Text == ""))
                {
                    ActiveControl = dtRFA;
                    throw new Exception("Необходимо указать дату приема РФА");
                }
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            ClearTmpProduct();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            bool _res = false;
            DialogResult _result = DialogResult.Cancel;
            try
            {
                switch (_ActionID) //загрузка Активаций
                {
                    case 1:
                        {
                            ClearTmpProduct();
                            _res = LoadActiveXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 2:
                        {
                            ClearTmpProduct();
                            _res = LoadSummXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 3:
                        {
                            ClearTmpProduct();
                            _res = LoadExportXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 4:
                        {
                            ClearTmpProduct();
                            _res = LoadChangeXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 5:
                        {
                            ClearTmpProduct();
                            _res = LoadExportTovarSKLADXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 6:
                        {
                            ClearTmpProduct();
                            _res = LoadRFA_WDXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 7:
                        {
                            ClearTmpProduct();
                            _res = LoadRFA_SdaniOperXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 8:
                        {
                            ClearTmpProduct();
                            foreach (string file in openFileDialog1.FileNames)
                                _res = LoadRFAPrin(file);//загружу их Екселя

                            _res = CreateAct();

                            _dbsql.SQLScript = "exec uchProducts @IDOperation = 3";
                            _dbsql.ExecuteNonQuery();

                            break;
                        }
                }
                _result = DialogResult.OK;
            }
            catch (Exception ex)
            {
                ClearTmpProduct();

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = DialogResult.Cancel;
            }
            this.DialogResult = _result;
        }
    }
}
