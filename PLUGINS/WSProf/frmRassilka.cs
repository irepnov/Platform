using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSProf
{
    public partial class frmRassilka : Form
    {

        private DBSqlServer _dbsql = null;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\template\\";
        private string _mnth1 = "";
        private string _mnth2 = "";
        private string _year = "";
        private string _provide = "";
        private string _metod = "";
        private string _step = "";
        private string _session = "";
        private string _file = "";
        private Int16 _action = 0;

        public frmRassilka(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _action = Convert.ToInt16(inParams[1]);

            if (_action == 1)
            {                
                this.Text = "Формирование списков рассылки";
            }
            if (_action == 2)
            {
                this.Text = "Обработка протоколов рассылки";
            }

                groupBox2.Enabled = (_action == 1);
             /*   label1.Enabled = (_action == 1);
                label4.Enabled = (_action == 1);
                label5.Enabled = (_action == 1);
                cbProvider.Enabled = (_action == 1);
                cbStep.Enabled = (_action == 1);
                button3.Enabled = (_action == 1);*/

            groupBox1.Enabled = (_action == 2);
           /* label1.Enabled = (_action == 2);
            cbProvider2.Enabled = (_action == 2);
            label7.Enabled = (_action == 2);
            tbFile.Enabled = (_action == 2);
            button4.Enabled = (_action == 2);
            button5.Enabled = (_action == 2);*/
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (cbMonth1.Text == "")
                {
                    ActiveControl = cbMonth1;
                    throw new Exception("Необходимо указать месяц начала");
                }
                if (cbMonth2.Text == "")
                {
                    ActiveControl = cbMonth2;
                    throw new Exception("Необходимо указать месяц окончания");
                }

                if (cbProvider.Text == "")
                {
                    ActiveControl = cbProvider;
                    throw new Exception("Необходимо указать провайдера");
                }

                if (cbMetod.Text == "")
                {
                    ActiveControl = cbMetod;
                    throw new Exception("Необходимо указать метод информирования");
                }

                if (lbb.SelectedItems.Count == 0)
                {
                    ActiveControl = lbb;
                    throw new Exception("Необходимо выбрать этап информирования (CTRL + мышь)");
                }

                _mnth1 = (cbMonth1.SelectedIndex + 1).ToString();
                _mnth2 = (cbMonth2.SelectedIndex + 1).ToString();
                _year = Convert.ToInt16(cbYear.Value).ToString();
                _provide = cbProvider.Text;
                _metod = cbMetod.Text.Substring(0, 1);
                _step = "";
                foreach (var item in lbb.SelectedItems)
                {
                    _step += (lbb.GetItemText(item).Substring(0, 3)) + ",";
                }
                _step = '(' + _step.Substring(0, _step.Length - 1) + ')';
                _session = Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        public string ReverseString(string srtVarable)
        {
            return new string(srtVarable.Reverse().ToArray());
        }

        public bool UserCheck2()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (cbMonth1.Text == "")
                {
                    ActiveControl = cbMonth1;
                    throw new Exception("Необходимо указать месяц начала");
                }
                if (cbMonth2.Text == "")
                {
                    ActiveControl = cbMonth2;
                    throw new Exception("Необходимо указать месяц окончания");
                }

                if (cbProvider2.Text == "")
                {
                    ActiveControl = cbProvider2;
                    throw new Exception("Необходимо указать провайдера");
                }

                if (!File.Exists(tbFile.Text))
                {
                    ActiveControl = tbFile;
                    throw new Exception("Необходимо выбрать файл протокола");
                }

                if (cbProvider2.Text == "INFOBIT")
                {
                    if (openFileDialog1.FileNames.Length > 2)
                    {
                        throw new Exception("Выбрано более двух протоколов одновременно");
                    }
                    if (openFileDialog1.FileNames.Length == 2)
                    {
                        string _f1 = ReverseString(Path.GetFileNameWithoutExtension(openFileDialog1.FileNames[0])).Substring(3, 5);
                        string _f2 = ReverseString(Path.GetFileNameWithoutExtension(openFileDialog1.FileNames[1])).Substring(3, 5);
                        if (_f1 != _f2)
                            throw new Exception("Выбраны протоколы из разных рассылок");
                    }
                }

                _mnth1 = (cbMonth1.SelectedIndex + 1).ToString();
                _mnth2 = (cbMonth2.SelectedIndex + 1).ToString();
                _year = Convert.ToInt16(cbYear.Value).ToString();
                _provide = cbProvider2.Text;
                _file = tbFile.Text;
                _session = Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            Cursor = Cursors.WaitCursor;
            button3.Enabled = false;

            _dbsql.SQLScript = "exec [WSProf].[spFactInfos] @IDOper = 3, @sessionGuid = @guid, @Mnth1 = @m1, @Mnth2 = @m2, @year = @y, @TypeProvider = @prov, @InfoMeth = @met, @InfoStepList = @step";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
            _dbsql.AddParameter("@m1", EnumDBDataTypes.EDT_Integer, _mnth1);
            _dbsql.AddParameter("@m2", EnumDBDataTypes.EDT_Integer, _mnth2);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
            _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_String, _provide);
            _dbsql.AddParameter("@met", EnumDBDataTypes.EDT_Integer, _metod);
            _dbsql.AddParameter("@step", EnumDBDataTypes.EDT_String, _step);
            string _col = _dbsql.ExecuteScalarVisual("Поиск граждан для включения в список рассылки");

            if (_col == null || _col == "0")
            {
                Cursor = Cursors.Default;
                button3.Enabled = true;

                MessageBox.Show("Сведения для выгрузки отсутствуют",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string _file = @"c:\temp\" + _session.Substring(0, 13).Replace("-","_") + "_" + DateTime.Now.ToShortDateString().Replace(".", "_").Replace("-", "_");

            _dbsql.SQLScript = "exec [WSProf].[spFactInfos] @IDOper = 4, @sessionGuid = @guid, @TypeProvider = @prov";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
            _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_String, _provide);
            if (_provide == "AIR")
            {
                StringBuilder sbRtn = new StringBuilder();
                using (SqlDataReader rdr = _dbsql.ExecuteReader())
                {
                    while (rdr.Read())
                        sbRtn.AppendLine(rdr["_col"].ToString());
                }
                _file = _file + "_AIR.csv";
                File.WriteAllText(_file, sbRtn.ToString(), Encoding.GetEncoding(1251));
                sbRtn = null;
            }

            if (_provide == "INFOBIT")
            {
                    DataTable _dt = _dbsql.FillDataSet().Tables[0];
                    if ((_dt == null) || (_dt.Rows.Count < 1))
                    {
                        Cursor = Cursors.Default;
                        button3.Enabled = true;
                        return;
                    }
                       
                    ExcelManager _excel = new ExcelManager();
                    try
                    {
                        _excel.SetWorkSheetByIndex = 1;
                        _excel.SetCalculation(ECalculation.xlCalculationManual);
                        _excel.SetScreenUpdating(false);
                        //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
                        int indexStartColumn = 0; //если например из результирующего набора не выводить Первый столбец ID
                        object[,] DataArray = _excel.GetDataArrayFromDataTable(_dt, indexStartColumn, null);
                        //перенести массива в Excel
                        //стартовая ячейка
                        int indexFirstExcelRow = 1; //    1
                        int indexFirstExcelColumn = 1; // A               
                        string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                                                                  ":" +
                                        CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + _dt.Columns.Count - indexStartColumn) - 1] + ((_dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
                        _excel.SetValueToRange(_Range, DataArray);
                        _excel.SetScreenUpdating(true);
                        DataArray = null;
                        _dt = null;
                        //сохраняем выходим          
                        _file = _file + "_INFOBIT.xls";
                        _excel.SaveDocument(_file, true, ESaveFormats.xlNormal);
                        _excel.CloseExcelDocumentAndShow();
                    }
                    catch
                    {
                        _excel.CloseDocument();
                        _excel = null;
                        _dt = null;
                    }
            }

            if (_provide == "POST")
            {
                DataTable _dt = _dbsql.FillDataSet().Tables[0];
                if ((_dt == null) || (_dt.Rows.Count < 1))
                    return;
                ExcelManager _excel = new ExcelManager();
                try
                {
                    _excel.SetWorkSheetByIndex = 1;
                    _excel.SetCalculation(ECalculation.xlCalculationManual);
                    _excel.SetScreenUpdating(false);
                    //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
                    int indexStartColumn = 0; //если например из результирующего набора не выводить Первый столбец ID
                    object[,] DataArray = _excel.GetDataArrayFromDataTable(_dt, indexStartColumn, null);
                    //перенести массива в Excel
                    //стартовая ячейка
                    int indexFirstExcelRow = 1; //    1
                    int indexFirstExcelColumn = 1; // A               
                    string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                                                              ":" +
                                    CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + _dt.Columns.Count - indexStartColumn) - 1] + ((_dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
                    _excel.SetValueToRange(_Range, DataArray);
                    _excel.SetScreenUpdating(true);
                    DataArray = null;
                    _dt = null;
                    //сохраняем выходим          
                    _file = _file + "_POST.xls";
                    _excel.SaveDocument(_file, true, ESaveFormats.xlNormal);
                    _excel.CloseExcelDocumentAndShow();
                }
                catch
                {
                    _excel.CloseDocument();
                    _excel = null;
                    _dt = null;
                }
            }
            Cursor = Cursors.Default;
            button3.Enabled = true;

            MessageBox.Show("Выгружено " + _col + " записей\nВ файл " + _file + "\nОсуществите информирование через рассылку и обработайте протоколы",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Протокол обработки|*.xls;*.xlsx;*.csv";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = openFileDialog1.FileNames[0];
            }
        }



        private string NullWithEmpty(string inst)
        {
            if ((inst == "-") || (inst.Length == 0) || (String.IsNullOrEmpty(inst)) || (inst.Trim() == ""))
                return "";
            else
                return inst;
        }

        private bool LoadINFOBIT(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                _excel = new ExcelManager(_inFiles);
                _excel.SetWorkSheetByIndex = 1;

                int _RowIndex = 2;

                string _phone, _date, _stat;
                _phone = _date = _stat = "";

                if (NullWithEmpty(_excel.GetValue("A1").Trim()) != "Название учётной записи")
                    throw new Exception("Указанный файл не является протоколом INFOBITa");

                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _phone = NullWithEmpty(_excel.GetValue("E" + _RowIndex.ToString()).Trim());
                    _date = NullWithEmpty(_excel.GetValue("G" + _RowIndex.ToString()).Trim()).Substring(0, 10);
                    _stat = NullWithEmpty(_excel.GetValue("M" + _RowIndex.ToString()).Trim());

                    if (_stat.ToUpper() == "DELIVERED" || _stat.ToUpper() == "DELIVERED_TO_HANDSET")
                    {
                        _dbsql.SQLScript = @"exec [WSProf].[spFactInfos] @IDOper = 5, @TypeProvider = @prov, @sessionGuid = @guid, @InfoDate = @dat, @PHONE_PROT = @ph, @filename = @ff";
                        _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_String, _provide);
                        _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
                        _dbsql.AddParameter("@dat", EnumDBDataTypes.EDT_DateTime, _date);
                        _dbsql.AddParameter("@ph", EnumDBDataTypes.EDT_String, _phone);
                        _dbsql.AddParameter("@ff", EnumDBDataTypes.EDT_String, Path.GetFileNameWithoutExtension(_inFiles));
                        _dbsql.ExecuteNonQuery();
                    }

                    _RowIndex = _RowIndex + 1;
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

        private bool LoadAIR(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                _excel = new ExcelManager(_inFiles);
                _excel.SetWorkSheetByIndex = 1;

                int _RowIndex = 1;

                string _phone, _date, _stat, _mes;
                _phone = _date = _stat = _mes = "";

                if (NullWithEmpty(_excel.GetValue("D1").Trim()) != "Доставлено")
                    throw new Exception("Указанный файл не является протоколом AIRa");

                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _phone = NullWithEmpty(_excel.GetValue("B" + _RowIndex.ToString()).Trim());
                    _date = NullWithEmpty(_excel.GetValue("A" + _RowIndex.ToString()).Trim()).Substring(0, 10);
                    _stat = NullWithEmpty(_excel.GetValue("D" + _RowIndex.ToString()).Trim());
                    _mes = NullWithEmpty(_excel.GetValue("C" + _RowIndex.ToString()).Trim());

                    if (_stat.ToUpper() == "ДОСТАВЛЕНО")
                    {
                        _dbsql.SQLScript = @"exec [WSProf].[spFactInfos] @IDOper = 5, 
                                                  @TypeProvider = @prov, @sessionGuid = @guid, @InfoDate = @dat, @PHONE_PROT = @ph, @MES_PROT = @m, @filename = @ff";
                        _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_String, _provide);
                        _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
                        _dbsql.AddParameter("@dat", EnumDBDataTypes.EDT_DateTime, _date);
                        _dbsql.AddParameter("@ph", EnumDBDataTypes.EDT_String, _phone);
                        _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mes);
                        _dbsql.AddParameter("@ff", EnumDBDataTypes.EDT_String, Path.GetFileNameWithoutExtension(_inFiles));
                        _dbsql.ExecuteNonQuery();
                    }

                    _RowIndex = _RowIndex + 1;
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

        private bool LoadPOST(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                _excel = new ExcelManager(_inFiles);
                _excel.SetWorkSheetByIndex = 1;

                int _RowIndex = 1;

                string _ID, _phone, _date, _stat, _mes;
                _ID = _phone = _date = _stat = _mes = "";

                if (NullWithEmpty(_excel.GetValue("J1").Trim()).ToUpper() != "ОТПРАВЛЕНО")
                    throw new Exception("Указанный файл не является протоколом POSTa");

                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _ID = NullWithEmpty(_excel.GetValue("A" + _RowIndex.ToString()).Trim());
                    _date = NullWithEmpty(_excel.GetValue("I" + _RowIndex.ToString()).Trim()).Substring(0, 10);
                    _stat = NullWithEmpty(_excel.GetValue("J" + _RowIndex.ToString()).Trim());
                    _mes = NullWithEmpty(_excel.GetValue("H" + _RowIndex.ToString()).Trim());

                    if (_stat.ToUpper() == "ОТПРАВЛЕНО")
                    {
                        _dbsql.SQLScript = @"exec [WSProf].[spFactInfos] @IDOper = 5, 
                                                  @TypeProvider = @prov, @sessionGuid = @guid, @InfoDate = @dat, @ID_PROT = @ph, @MES_PROT = @m, @filename = @ff";
                        _dbsql.AddParameter("@prov", EnumDBDataTypes.EDT_String, _provide);
                        _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
                        _dbsql.AddParameter("@dat", EnumDBDataTypes.EDT_DateTime, _date);
                        _dbsql.AddParameter("@ph", EnumDBDataTypes.EDT_Integer, _ID);
                        _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mes);
                        _dbsql.AddParameter("@ff", EnumDBDataTypes.EDT_String, Path.GetFileNameWithoutExtension(_inFiles));
                        _dbsql.ExecuteNonQuery();
                    }

                    _RowIndex = _RowIndex + 1;
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (!UserCheck2())
                return;

            Cursor = Cursors.WaitCursor;
            button5.Enabled = false;

               _dbsql.SQLScript = "delete from WSProf.transferProtocol where TypeProvider = @pro and sessionGuid <> @gui";
                 _dbsql.AddParameter("@pro", EnumDBDataTypes.EDT_String, _provide);
                 _dbsql.AddParameter("@gui", EnumDBDataTypes.EDT_String, _session);
                 _dbsql.ExecuteNonQuery();
             
                 bool _r = false;

                 // удалить этого проайдера но с другими гуидами из таблицы загрузки
                 if (_provide == "INFOBIT")
                    foreach(string file in openFileDialog1.FileNames)  //загрузка нескольких протоколов с одним именем
                     _r = LoadINFOBIT(file);

                 if (_provide == "AIR")
                     _r = LoadAIR(_file);

                 if (_provide == "POST")
                     _r = LoadPOST(_file);

                 Cursor = Cursors.Default;
                 button5.Enabled = true;

                 if (_r)
                 {
                     _dbsql.SQLScript = @"exec [WSProf].[spFactInfos] @IDOper = 6, @sessionGuid = @guid";
                     _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _session);
                     _dbsql.ExecuteNonQueryVisual("Обработка протоколов рассылки");

                     MessageBox.Show("Протокол рассылки обработан", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
        }

        private void cbProvider2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            button4.Enabled = (cbProvider2.Text != "");
            tbFile.Text = "";
            openFileDialog1.Multiselect = (cbProvider2.Text == "INFOBIT");
        }
    }
}
