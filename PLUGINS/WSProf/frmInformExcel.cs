using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSProf
{
    public partial class frmInformExcel : Form
    {
        private DBSqlServer _dbsql = null;
        private Int32 _ActionID = -1;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\template\\";

        public frmInformExcel(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _dbsql.SQLScript = "delete from [WSProf].[transferInfosExcel]";
            _dbsql.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Cписки проинформированных|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFiles.Text = openFileDialog1.FileName;
            }
        }

        private string NullWithEmpty(string inst)
        {
            if ((inst == "-") || (inst.Length == 0) || (String.IsNullOrEmpty(inst)) || (inst.Trim() == ""))
                return "";
            else
                return inst;
        }

        private bool LoadEtalonXLS(string _inFiles)
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

                string _pols, _poln, _enp, _Fam, _Im, _Otch, _datr, _doct, _docs, _docn, 
                    _InfoMeth, _InfoStep, _InfoRslt, _InfoDate, _code_mo, _year;

                _pols= _poln= _enp= _Fam= _Im= _Otch= _datr= _doct= _docs= _docn= 
                    _InfoMeth= _InfoStep= _InfoRslt= _InfoDate= _code_mo = _year = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("K" + _RowIndex.ToString()).Trim() != "" && _excel.GetValue("O" + _RowIndex.ToString()).Trim() != "")
                {
                    _pols = NullWithEmpty(_excel.GetValue("A" + _RowIndex.ToString()).Trim());
                    _poln = NullWithEmpty(_excel.GetValue("B" + _RowIndex.ToString()).Trim());
                    _enp = NullWithEmpty(_excel.GetValue("C" + _RowIndex.ToString()).Trim());
                    _Fam = NullWithEmpty(_excel.GetValue("D" + _RowIndex.ToString()).Trim());
                    _Im = NullWithEmpty(_excel.GetValue("E" + _RowIndex.ToString()).Trim());
                    _Otch = NullWithEmpty(_excel.GetValue("F" + _RowIndex.ToString()).Trim());
                    _datr = NullWithEmpty(_excel.GetValue("G" + _RowIndex.ToString()).Trim());
                    _doct = NullWithEmpty(_excel.GetValue("H" + _RowIndex.ToString()).Trim());

                    _docs = NullWithEmpty(_excel.GetValue("I" + _RowIndex.ToString()).Trim());
                    _docn = NullWithEmpty(_excel.GetValue("J" + _RowIndex.ToString()).Trim());
                    _code_mo = NullWithEmpty(_excel.GetValue("K" + _RowIndex.ToString()).Trim());
                    _InfoMeth = NullWithEmpty(_excel.GetValue("L" + _RowIndex.ToString()).Trim());
                    _InfoStep = NullWithEmpty(_excel.GetValue("M" + _RowIndex.ToString()).Trim());

                    _InfoRslt = NullWithEmpty(_excel.GetValue("N" + _RowIndex.ToString()).Trim());
                    _InfoDate = NullWithEmpty(_excel.GetValue("O" + _RowIndex.ToString()).Trim());
                    _year = NullWithEmpty(_excel.GetValue("P" + _RowIndex.ToString()).Trim());


                    _dbsql.SQLScript = @"exec [WSProf].[spFactInfos]
				                                @IDOper = 8, 
                                                @pols = @q1, @poln = @q2, @enp = @q3, 
                                                @Fam = @q4, @Im = @q5, @Otch = @q6, 
		                                        @datr = @q7, @doct = @q8, @docs = @q9, @docn = @q10, 
                                                @InfoMeth = @q11, @InfoStep = @q12, @InfoRslt = @q13, 
                                                @InfoDate = @q14, @code_mo = @q15, @year = @q16";
                    _dbsql.AddParameter("@q1", EnumDBDataTypes.EDT_String, _pols);
                    _dbsql.AddParameter("@q2", EnumDBDataTypes.EDT_String, _poln);
                    _dbsql.AddParameter("@q3", EnumDBDataTypes.EDT_String, _enp);
                    _dbsql.AddParameter("@q4", EnumDBDataTypes.EDT_String, _Fam);
                    _dbsql.AddParameter("@q5", EnumDBDataTypes.EDT_String, _Im);
                    _dbsql.AddParameter("@q6", EnumDBDataTypes.EDT_String, _Otch);
                    _dbsql.AddParameter("@q7", EnumDBDataTypes.EDT_DateTime, _datr);
                    _dbsql.AddParameter("@q8", EnumDBDataTypes.EDT_Integer, _doct);
                    _dbsql.AddParameter("@q9", EnumDBDataTypes.EDT_String, _docs);
                    _dbsql.AddParameter("@q10", EnumDBDataTypes.EDT_String, _docn);
                    _dbsql.AddParameter("@q11", EnumDBDataTypes.EDT_Integer, _InfoMeth);
                    _dbsql.AddParameter("@q12", EnumDBDataTypes.EDT_Integer, _InfoStep);
                    _dbsql.AddParameter("@q13", EnumDBDataTypes.EDT_Integer, _InfoRslt);
                    _dbsql.AddParameter("@q14", EnumDBDataTypes.EDT_DateTime, _InfoDate);
                    _dbsql.AddParameter("@q15", EnumDBDataTypes.EDT_String, _code_mo);
                    _dbsql.AddParameter("@q16", EnumDBDataTypes.EDT_Integer, _year);

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

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            button1.Enabled = false;

            if (LoadEtalonXLS(tbFiles.Text)) {
                _dbsql.SQLScript = "exec WSProf.spFactInfos @IDOper = 9";
                DataTable _dt = _dbsql.FillDataSetVisual("Обработка списков").Tables[0];
                if ((_dt == null) || (_dt.Rows.Count < 1))
                {
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация о проинформированных гражданах обработана",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string _file = tbFiles.Text.Replace(".xls", "_errors.xls");
                    _excel.SaveDocument(_file, true, ESaveFormats.xlNormal);
                    _excel.CloseExcelDocumentAndShow();
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация о проинформированных гражданах обработана с ошибками\n" + _file + "\nвнесите исправления в отобранные записи",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch
                {
                    _excel.CloseDocument();
                    _excel = null;
                    _dt = null;
                }



            };

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
