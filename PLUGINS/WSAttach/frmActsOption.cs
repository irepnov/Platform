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
using GGPlatform.DBFBulk;

namespace WSAttach
{
    public partial class frmActsOption : Form
    {

        private DBSqlServer _dbsql = null;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\template\\";
        private string _mnth = "";
        private string _year = "";
        private string _type = "";
        private Int16 _action = 0;

        public frmActsOption(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _action = Convert.ToInt16(inParams[1]);

            if (_action == 1)
            {                
                this.Text = "Рассчет актов сверки численности";
                button1.Text = "Рассчитать";
            }
            if (_action == 2)
            {
                this.Text = "Выгрузка актов сверки численности для МО";
                button1.Text = "Выгрузить";
            }
            if (_action == 3)
            {
                this.Text = "Выгрузка актов сверки численности для ПК МЭК";
                button1.Text = "Выгрузить";
            }
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (cbMonth.Text == "")
                {
                    ActiveControl = cbMonth;
                    throw new Exception("Необходимо указать месяц");
                }

                if (cbType.SelectedItems.Count == 0)
                {
                    ActiveControl = cbType;
                    throw new Exception("Необходимо выбрать типы рассчитываемых актов сверки");
                }

                _mnth = (cbMonth.SelectedIndex + 1).ToString();
                _year = Convert.ToInt16(cbYear.Value).ToString();
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void _actRash()
        {
                Cursor = Cursors.WaitCursor;
                button1.Enabled = false;
                if (cbType.GetItemChecked(0))
                {
                    _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 3, @year = @y, @mnth = @m, @TypeAct = @t";
                    _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                    _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                    _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 1.ToString());
                    _dbsql.ExecuteNonQueryVisual("Рассчет актов сверки прикрепленного населения к АПУ");
                }
                if (cbType.GetItemChecked(1))
                {
                    _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 3, @year = @y, @mnth = @m, @TypeAct = @t";
                    _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                    _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                    _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 2.ToString());
                    _dbsql.ExecuteNonQueryVisual("Рассчет актов сверки обслуживаемого населения ССМП");
                }
                Cursor = Cursors.Default;
                button1.Enabled = true;
                MessageBox.Show("Акты сверки численности расчитаны", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private string _getMonth(string _mm)
        {
            string _mes = "";
            switch (_mm)
            {
                case "1": _mes = "Января"; break;
                case "2": _mes = "Февраля"; break;
                case "3": _mes = "Марта"; break;
                case "4": _mes = "Апреля"; break;
                case "5": _mes = "Мая"; break;
                case "6": _mes = "Июня"; break;
                case "7": _mes = "Июля"; break;
                case "8": _mes = "Августа"; break;
                case "9": _mes = "Сентября"; break;
                case "10": _mes = "Октября"; break;
                case "11": _mes = "Ноября"; break;
                case "12": _mes = "Декабря"; break;
            }
            return _mes;
        }

        private void ExportPDF_APU(DataRow _r, int _type)
        {
            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 7, @year = @y, @mnth = @m, @TypeAct = @t, @code_mo = @code";
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
            _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, _type.ToString());
            _dbsql.AddParameter("@code", EnumDBDataTypes.EDT_String, _r["code"].ToString());
            DataTable _ddt = _dbsql.FillDataSet().Tables[0];

            ExcelManager _excel = new ExcelManager(Application.StartupPath + _templatename + "act_prick_apu.xlt");
            try
            {
                _excel.SetWorkSheetByIndex = 1;
                _excel.SetCalculation(ECalculation.xlCalculationManual);
                _excel.SetScreenUpdating(false);
                _excel.SetDisplayAlerts(false);
                _excel.SetValueToRange("A9", _ddt.Rows[0]["orgName"].ToString());
                _excel.SetValueToRange("A11", _ddt.Rows[0]["code"].ToString());
                _excel.SetValueToRange("C11", _ddt.Rows[0]["nameshort"].ToString());
                _excel.SetValueToRange("A7", "на 1 " + _getMonth(_mnth) + " " + _year + " года");

                _excel.SetValueToRange("B18", _ddt.Rows[0]["code"].ToString());
                _excel.SetValueToRange("C18", _ddt.Rows[0]["nameshort"].ToString());
                _excel.SetValueToRange("D18", _ddt.Rows[0]["c_all"].ToString());
                _excel.SetValueToRange("E18", _ddt.Rows[0]["c0_1m"].ToString());
                _excel.SetValueToRange("F18", _ddt.Rows[0]["c0_1g"].ToString());
                _excel.SetValueToRange("G18", _ddt.Rows[0]["c1_4m"].ToString());
                _excel.SetValueToRange("H18", _ddt.Rows[0]["c1_4g"].ToString());
                _excel.SetValueToRange("I18", _ddt.Rows[0]["c5_17m"].ToString());
                _excel.SetValueToRange("J18", _ddt.Rows[0]["c5_17g"].ToString());
                _excel.SetValueToRange("K18", _ddt.Rows[0]["c18_59m"].ToString());
                _excel.SetValueToRange("L18", _ddt.Rows[0]["c18_54g"].ToString());
                _excel.SetValueToRange("M18", _ddt.Rows[0]["c60m"].ToString());
                _excel.SetValueToRange("N18", _ddt.Rows[0]["c55g"].ToString());
                _excel.SetBorderStyle("B18:N18", BorderLineStyle.xlContinuous);


                int t = 18 + 3;
                _excel.SetValueToRange("B" + t.ToString(), "____________________________________________________________________________________________________________");
                _excel.SetValueToRange("B" + (t + 1).ToString(), "   (подпись)                                                       (Ф.И.О руководителя МО)");
                _excel.SetValueToRange("B" + (t + 2).ToString(), "   МП");
                _excel.SetValueToRange("B" + (t + 3).ToString(), "____________________________________________________________" + _ddt.Rows[0]["orgDir"].ToString() + "__________________________________");
                _excel.SetValueToRange("B" + (t + 4).ToString(), "     (подпись)                                                   (Ф.И.О руководителя СМО)");
                _excel.SetValueToRange("B" + (t + 5).ToString(), "   МП");

                //сохраняем выходим          
                string _file = "C:\\Temp\\prick\\APU\\actsAPU_" + _year + "_" + _mnth + "_" + _ddt.Rows[0]["code"].ToString() + ".pdf";
                _excel.SaveDocument(_file.Replace(".pdf", ".xls"), true, ESaveFormats.xlNormal);
                _excel.SaveDocument(_file, true, ESaveFormats.XlFixedFormatTypePDF);
                _excel.CloseDocument();
                _ddt = null;
            }
            catch
            {
                _excel.CloseDocument();
                _excel = null;
                _ddt = null;
            }



            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 7, @year = @y, @mnth = @m, @TypeAct = 11, @code_mo = @code";
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
       //     _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, _type.ToString());
            _dbsql.AddParameter("@code", EnumDBDataTypes.EDT_String, _r["code"].ToString());
            DataTable _ddts = _dbsql.FillDataSet().Tables[0];

            ExcelManager _excels = new ExcelManager(Application.StartupPath + _templatename + "act_prick_apu_str.xlt");
            try
            {
                _excels.SetWorkSheetByIndex = 1;
                _excels.SetCalculation(ECalculation.xlCalculationManual);
                _excels.SetScreenUpdating(false);
                _excels.SetDisplayAlerts(false);
                _excels.SetValueToRange("A9", _ddts.Rows[0]["orgName"].ToString());
                _excels.SetValueToRange("A11", _ddts.Rows[0]["urcode"].ToString());
                _excels.SetValueToRange("C11", _ddts.Rows[0]["urname"].ToString());
                _excels.SetValueToRange("A7", "на 1 " + _getMonth(_mnth) + " " + _year + " года");

                int t = 18;
                foreach (DataRow ddr in _ddts.Rows)
                {
                    _excels.SetValueToRange("B" + t.ToString(), ddr["code"].ToString().Replace("99999", ""));
                    _excels.SetValueToRange("C" + t.ToString(), ddr["nameshort"].ToString());
                    _excels.SetValueToRange("D" + t.ToString(), ddr["c_all"].ToString());
                    _excels.SetValueToRange("E" + t.ToString(), ddr["c0_1m"].ToString());
                    _excels.SetValueToRange("F" + t.ToString(), ddr["c0_1g"].ToString());
                    _excels.SetValueToRange("G" + t.ToString(), ddr["c1_4m"].ToString());
                    _excels.SetValueToRange("H" + t.ToString(), ddr["c1_4g"].ToString());
                    _excels.SetValueToRange("I" + t.ToString(), ddr["c5_17m"].ToString());
                    _excels.SetValueToRange("J" + t.ToString(), ddr["c5_17g"].ToString());
                    _excels.SetValueToRange("K" + t.ToString(), ddr["c18_59m"].ToString());
                    _excels.SetValueToRange("L" + t.ToString(), ddr["c18_54g"].ToString());
                    _excels.SetValueToRange("M" + t.ToString(), ddr["c60m"].ToString());
                    _excels.SetValueToRange("N" + t.ToString(), ddr["c55g"].ToString());
                    t += 1;
                }
                _excels.SetBorderStyle("B18:N" + (t - 1).ToString(), BorderLineStyle.xlContinuous);

                t += 3;
                _excels.SetValueToRange("B" + t.ToString(), "____________________________________________________________________________________________________________");
                _excels.SetValueToRange("B" + (t + 1).ToString(), "   (подпись)                                                       (Ф.И.О руководителя МО)");
                _excels.SetValueToRange("B" + (t + 2).ToString(), "   МП");
                _excels.SetValueToRange("B" + (t + 3).ToString(), "____________________________________________________________" + _ddts.Rows[0]["orgDir"].ToString() + "__________________________________");
                _excels.SetValueToRange("B" + (t + 4).ToString(), "     (подпись)                                                   (Ф.И.О руководителя СМО)");
                _excels.SetValueToRange("B" + (t + 5).ToString(), "   МП");

                //сохраняем выходим          
                string _file = "C:\\Temp\\prick\\APU\\actsAPU_str_" + _year + "_" + _mnth + "_" + _ddts.Rows[0]["urcode"].ToString() + ".pdf";
                _excels.SaveDocument(_file.Replace(".pdf", ".xls"), true, ESaveFormats.xlNormal);
                _excels.SaveDocument(_file, true, ESaveFormats.XlFixedFormatTypePDF);
                _excels.CloseDocument();
                _ddts = null;
            }
            catch
            {
                _excels.CloseDocument();
                _excels = null;
                _ddts = null;
            }
        }

        private void ExportPDF_SMP(DataRow _r, int _type)
        {
            _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 7, @year = @y, @mnth = @m, @TypeAct = @t, @code_mo = @code";
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
            _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, _type.ToString());
            _dbsql.AddParameter("@code", EnumDBDataTypes.EDT_String, _r["code"].ToString());
            DataTable _ddt = _dbsql.FillDataSet().Tables[0];

            ExcelManager _excel = new ExcelManager(Application.StartupPath + _templatename + "act_prick_smp.xlt");
            try
            {
                _excel.SetWorkSheetByIndex = 1;
                _excel.SetCalculation(ECalculation.xlCalculationManual);
                _excel.SetScreenUpdating(false);
                _excel.SetDisplayAlerts(false);
                _excel.SetValueToRange("A9", _ddt.Rows[0]["orgName"].ToString());
                _excel.SetValueToRange("A11", _ddt.Rows[0]["code"].ToString());
                _excel.SetValueToRange("C11", _ddt.Rows[0]["nameshort"].ToString());
                _excel.SetValueToRange("A7", "на 1 " + _getMonth(_mnth) + " " + _year + " года");

                int t = 18;
                foreach(DataRow ddr in _ddt.Rows)
                {
                    _excel.SetValueToRange("B" + t.ToString(), ddr["stcode"].ToString().Replace("99999",""));
                    _excel.SetValueToRange("C" + t.ToString(), ddr["stname"].ToString());
                    _excel.SetValueToRange("D" + t.ToString(), ddr["c_all"].ToString());
                    _excel.SetValueToRange("E" + t.ToString(), ddr["c0_1m"].ToString());
                    _excel.SetValueToRange("F" + t.ToString(), ddr["c0_1g"].ToString());
                    _excel.SetValueToRange("G" + t.ToString(), ddr["c1_4m"].ToString());
                    _excel.SetValueToRange("H" + t.ToString(), ddr["c1_4g"].ToString());
                    _excel.SetValueToRange("I" + t.ToString(), ddr["c5_17m"].ToString());
                    _excel.SetValueToRange("J" + t.ToString(), ddr["c5_17g"].ToString());
                    _excel.SetValueToRange("K" + t.ToString(), ddr["c18_59m"].ToString());
                    _excel.SetValueToRange("L" + t.ToString(), ddr["c18_54g"].ToString());
                    _excel.SetValueToRange("M" + t.ToString(), ddr["c60m"].ToString());
                    _excel.SetValueToRange("N" + t.ToString(), ddr["c55g"].ToString());
                    t += 1;
                }
                _excel.SetBorderStyle("B18:N" + (t - 1).ToString(), BorderLineStyle.xlContinuous);

                t += 3;
                _excel.SetValueToRange("B" + t.ToString(), "____________________________________________________________________________________________________________");
                _excel.SetValueToRange("B" + (t + 1).ToString(), "   (подпись)                                                       (Ф.И.О руководителя МО)");
                _excel.SetValueToRange("B" + (t + 2).ToString(), "   МП");
                _excel.SetValueToRange("B" + (t + 3).ToString(), "____________________________________________________________" + _ddt.Rows[0]["orgDir"].ToString() + "__________________________________");
                _excel.SetValueToRange("B" + (t + 4).ToString(), "     (подпись)                                                   (Ф.И.О руководителя СМО)");
                _excel.SetValueToRange("B" + (t + 5).ToString(), "   МП");

                //сохраняем выходим          
                string _file = "C:\\Temp\\prick\\SMP\\actsSMP_" + _year + "_" + _mnth + "_" + _ddt.Rows[0]["code"].ToString() + ".pdf";
                _excel.SaveDocument(_file.Replace(".pdf", ".xls"), true, ESaveFormats.xlNormal);
                _excel.SaveDocument(_file, true, ESaveFormats.XlFixedFormatTypePDF);
                _excel.CloseDocument();
                _ddt = null;
            }
            catch
            {
                _excel.CloseDocument();
                _excel = null;
                _ddt = null;
            }
        }

        private void _actExportPDF()
        {
            Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
            if (cbType.GetItemChecked(0))
            {
                _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 6, @year = @y, @mnth = @m, @TypeAct = @t";
                _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 1.ToString());
                DataTable _dt = _dbsql.FillDataSet().Tables[0];
                if (_dt.Rows.Count == 0)
                {
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация по АПУ для выгрузки отсутствует", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
                progressBar1.Maximum = _dt.Rows.Count;
                foreach(DataRow _dr in _dt.Rows)
                {
                    ExportPDF_APU(_dr, 1);
                    progressBar1.Value += 1;
                }
            }

            if (cbType.GetItemChecked(1))
            {
                _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 6, @year = @y, @mnth = @m, @TypeAct = @t";
                _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 2.ToString());
                DataTable _dtt = _dbsql.FillDataSet().Tables[0];
                if (_dtt.Rows.Count == 0)
                {
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация по СМП для выгрузки отсутствует", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                progressBar1.Minimum = 0;
                progressBar1.Value = 0;
                progressBar1.Maximum = _dtt.Rows.Count;
                foreach (DataRow _dr in _dtt.Rows)
                {
                    ExportPDF_SMP(_dr, 2);
                    progressBar1.Value += 1;
                }
            }

            Cursor = Cursors.Default;
            button1.Enabled = true;
            MessageBox.Show(@"Акты сверки численности сформированы в каталоге c:\temp\prick", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        private void _actExportDBF()
        {
            Cursor = Cursors.WaitCursor;
            button1.Enabled = false;
            if (cbType.GetItemChecked(0))
            {
                _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 8, @year = @y, @mnth = @m, @TypeAct = @t";
                _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 1.ToString());
                DataTable _dt = _dbsql.FillDataSet().Tables[0];
                if (_dt.Rows.Count == 0)
                {
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация по АПУ для выгрузки отсутствует", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string _mm = ("0" + _mnth).Length > 2 ? ("0" + _mnth).Substring(1, 2) : ("0" + _mnth);
                Directory.CreateDirectory("C:\\temp\\prick\\APU\\");
                string _destf = "C:\\temp\\prick\\APU\\PAYAPUYYYYMM.DBF".Replace("YYYY", _year).Replace("MM", _mm);
                File.Copy(Application.StartupPath + "\\resource\\PAYAPUYYYYMM.DBF", _destf, true);
                DBFBulk _bulk = new DBFBulk();                
                _bulk.BulkDestinationFileORTable = _destf;
                _bulk.SqlConnect = _dbsql.Connection;
                _bulk.BulkSourceFileORScript = "exec [WSAttach].[spAttach] @IDOper = 8, @year = " + _year + ", @mnth = " + _mnth + ", @TypeAct = 1";
                _bulk.BulkMapperFile = Application.StartupPath + "\\resource\\BulkMapper.xml";
                _bulk.BulkMapperKey = @"APU";
                _bulk.BulkExecuteToDBF();
                _bulk.Dispose();
                _bulk = null;
            }
            if (cbType.GetItemChecked(1))
            {
                _dbsql.SQLScript = "exec [WSAttach].[spAttach] @IDOper = 8, @year = @y, @mnth = @m, @TypeAct = @t";
                _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_String, _year);
                _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_String, _mnth);
                _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_Integer, 2.ToString());
                DataTable _dt = _dbsql.FillDataSet().Tables[0];
                if (_dt.Rows.Count == 0)
                {
                    Cursor = Cursors.Default;
                    button1.Enabled = true;
                    MessageBox.Show("Информация по СМП для выгрузки отсутствует", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string _mm = ("0" + _mnth).Length > 2 ? ("0" + _mnth).Substring(1, 2) : ("0" + _mnth);
                Directory.CreateDirectory("C:\\temp\\prick\\SMP\\");
                string _destf = "C:\\temp\\prick\\SMP\\PAYSMPYYYYMM.DBF".Replace("YYYY", _year).Replace("MM", _mm);
                File.Copy(Application.StartupPath + "\\resource\\PAYSMPYYYYMM.DBF", _destf, true);
                DBFBulk _bulk = new DBFBulk();
                _bulk.BulkDestinationFileORTable = _destf;
                _bulk.SqlConnect = _dbsql.Connection;
                _bulk.BulkSourceFileORScript = "exec [WSAttach].[spAttach] @IDOper = 8, @year = " + _year + ", @mnth = " + _mnth + ", @TypeAct = 2";
                _bulk.BulkMapperFile = Application.StartupPath + "\\resource\\BulkMapper.xml";
                _bulk.BulkMapperKey = @"SMP";
                _bulk.BulkExecuteToDBF();
                _bulk.Dispose();
                _bulk = null;
            }
            Cursor = Cursors.Default;
            button1.Enabled = true;
            MessageBox.Show(@"Акты сверки численности для ПК МЭК выгружены в каталог c:\temp\prick", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;
           
            if (_action == 1)
            {
                _actRash();
            }

            if (_action == 2)
            {
                _actExportPDF();
            }

            if (_action == 3)
            {
                _actExportDBF();
            }

        }
    }
}
