using System.Windows.Forms;
using GGPlatform.DBServer;
using System;
using System.IO;
using GGPlatform.ExcelManagers;
using System.Data;

namespace SimRodnNom
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

        private bool LoadRodnXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                _excel.SetWorkSheetByIndex = 1;

                int _RowIndex = 2;

                string _Name, _NameEtal, _sebest, _rozn, _Assort;

                _Name = _NameEtal = _sebest = _rozn = _Assort = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("B" + _RowIndex.ToString()).Trim() != "")
                {
                    _NameEtal = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _Name = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _sebest = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _rozn = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _Assort = _excel.GetValue("E" + _RowIndex.ToString()).Trim();

                    if ((_NameEtal == "-") || (_NameEtal.Length == 0) || (String.IsNullOrEmpty(_NameEtal)) || (_NameEtal.Trim() == "")) _NameEtal = "";
                    if ((_Name == "-") || (_Name.Length == 0) || (String.IsNullOrEmpty(_Name)) || (_Name.Trim() == "")) _Name = "";
                    if ((_sebest == "-") || (_sebest.Length == 0) || (String.IsNullOrEmpty(_sebest)) || (_sebest.Trim() == "")) _sebest = "";
                    if ((_rozn == "-") || (_rozn.Length == 0) || (String.IsNullOrEmpty(_rozn)) || (_rozn.Trim() == "")) _rozn = "";
                    if ((_Assort == "-") || (_Assort.Length == 0) || (String.IsNullOrEmpty(_Assort)) || (_Assort.Trim() == "")) _Assort = "";

                    _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 1, @Name = @rn, @EtalonName = @en, @isAssort = @as, @SebestFact = @s, @RoznPrice = @r";
                    _dbsql.AddParameter("@rn", EnumDBDataTypes.EDT_String, _Name);
                    _dbsql.AddParameter("@en", EnumDBDataTypes.EDT_String, _NameEtal);
                    _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_Bit, _Assort);
                    _dbsql.AddParameter("@s", EnumDBDataTypes.EDT_String, _sebest);
                    _dbsql.AddParameter("@r", EnumDBDataTypes.EDT_String, _rozn);
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



        //private bool LoadExportXLS(string _inFiles)
        //{
        //    bool _result = false;
        //    try
        //    {
        //        if (!File.Exists(_inFiles))
        //            throw new Exception("Указанный файл не найден");

        //        this.Cursor = Cursors.WaitCursor;

        //        lbProtocol.Items.Add("открываем файл для загрузки...");
        //        _excel = new ExcelManager(_inFiles);
        //        _excel.SetScreenUpdating(false);
        //        int _RowIndex = 1;
        //        string _NumberCard = "";
        //        string _NumberPhone = "";
        //        string _Number = "";
        //        lbProtocol.Items.Add("загрузка сведений...");
        //        while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
        //        {
        //            _Number = _excel.GetValue("A" + _RowIndex.ToString()).Trim();

        //            _NumberCard = _NumberPhone = "";

        //            if ((_Number == "-") || (_Number.Length == 0)
        //                    || (String.IsNullOrEmpty(_Number)) || (_Number.Trim() == ""))
        //                _Number = "";

        //            if (_Number.Length == 11)
        //                _NumberPhone = _Number;  //в файле номера телефонов

        //            if (_Number.Length == 20)
        //                _NumberCard = _Number; //в файле номера АИСИСИ

        //            _dbsql.SQLScript = @"exec uchProducts @IDOperation = 2, @NumberPhone = @phone, @NumberCard = @card, @OrderID = @ord";
        //            _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
        //            _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
        //            _dbsql.AddParameter("@ord", EnumDBDataTypes.EDT_Integer, _RowIndex.ToString());
        //            _dbsql.ExecuteNonQuery();

        //            _RowIndex = _RowIndex + 1;
        //        }
        //        lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString());
        //        lbProtocol.Items.Add("сопоставляем загруженную информация с базой данных...");

        //        /**/
        //        _dbsql.SQLScript = @"select t.OrderID, p.NumberPhone, p.NumberCard
        //                             from tmpProduct t
        //                                left join uchProduct p on (p.NumberPhone = t.NumberPhone and t.NumberPhone is not null)
        //                                                           or (p.NumberCard = t.NumberCard and t.NumberCard is not null)
        //                             order by t.OrderID";
        //        DataTable dt = _dbsql.FillDataSetVisual("Cопоставление загруженной информации...").Tables[0];                
        //        foreach(DataRow dr in dt.Rows)
        //        {
        //            _excel.SetColumnWidth("B:C", 30);
        //            _excel.SetFormat("B" + dr["OrderID"].ToString(), RangeFormat.xlTextFormat);
        //            _excel.SetFormat("C" + dr["OrderID"].ToString(), RangeFormat.xlTextFormat);
        //            _excel.SetFontBold("B" + dr["OrderID"].ToString());
        //            _excel.SetFontBold("C" + dr["OrderID"].ToString());
        //            _excel.SetValueToRange("B" + dr["OrderID"].ToString(), dr["NumberPhone"].ToString());
        //            _excel.SetValueToRange("C" + dr["OrderID"].ToString(), dr["NumberCard"].ToString());
        //        }
        //        _excel.SetScreenUpdating(true);
        //        this.Cursor = Cursors.Default;
        //        //сохраняем выходим           
        //        if (_excel.SaveDocument("c:\\temp\\uchSopostavl.xls", true, ESaveFormats.xlNormal))
        //            MessageBox.Show("Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация",
        //                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        _excel.CloseExcelDocumentAndShow();
        //        _excel = null;                
        //        _result = true;                
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Cursor = Cursors.Default;
        //        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        _result = false;
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //        if (_excel != null)
        //            _excel.CloseDocument();
        //        _excel = null;
        //    }
        //    return _result;
        //}

        private void ClearTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 3";
            _dbsql.ExecuteNonQuery();
        }

        private void FillTmpProductEtalon()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 2";
            _dbsql.ExecuteNonQuery();
        }

        private DataTable SelectEmpryTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 4";
            return _dbsql.FillDataSet().Tables[0];
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
                            _res = LoadRodnXLS(tbFiles.Text);//загружу их Екселя
                            FillTmpProductEtalon();

                            DataTable _DTEmpty = SelectEmpryTmpProduct();
                            if (_DTEmpty.Rows.Count > 0)
                            {
                            /*
                            заполнение ручное
                            проставление в временной таблице
                            */
                                frmSootvet fmSoot = new frmSootvet(new object[] { _dbsql, _DTEmpty, chbIntellect.Checked });
                                if (fmSoot.ShowDialog() != DialogResult.OK)
                                    ClearTmpProduct();
                                fmSoot.Dispose();
                                fmSoot = null;
                            }
                            
                            _dbsql.SQLScript = @"exec uchRodnNomenc @IDOperation = 5";//перенос в постоянные
                            _dbsql.ExecuteNonQuery();

                            break;
                        }
                    //case 2:
                    //    {
                    //        ClearTmpProduct();
                    //        _res = LoadExportXLS(tbFiles.Text);//загружу их Екселя
                    //        break;
                    //    }
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
