using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using GGPlatform.BuiltInApp;
using GGPlatform.DBServer;
using GGPlatform.QueryBuilder;
using GGPlatform.RFCManag;
using System.Reflection;
using GGPlatform.ExcelManagers;
using System.IO;
using Ionic.Zip;
using System.Xml;

namespace prip_pl
{
    public partial class formPrip : Form, GGPlatform.BuiltInApp.IPlugin
    {
        object _MainApp = null;
        Form _MainForm = null;
        int _PluginID;
        int _WorkplaceID;
        string _PluginAssembly, _PluginName, _SoftName = "";
        DelegClosePlugin _ClosePlugin;
        DBSqlServer _dbsql = null;
        RFCManager _rfc = null;
        private BindingSource bs = new BindingSource();

        private DateTime _PrikDate = DateTime.Now.Date;

        public formPrip() 
        {      
            InitializeComponent();
        }

        int IPlugin.plugPluginButtons { get { return (int)EnumPluginButtons.EPB_Refresh; } }

        ToolStrip IPlugin.plugToolStrip { get { return toolStrip1; } }

        void IPlugin.plugInit(object MainApp, DBSqlServer inDBSqlServer, RFCManager inRFCManager, ref Lists inActiveLists)
        {
            _MainApp = MainApp;
            _MainForm = (Form)_MainApp;
            _dbsql = inDBSqlServer;
            _rfc = inRFCManager;

            this.MdiParent = (Form)_MainApp;
            this.Show();
            this.WindowState = FormWindowState.Maximized;
            this.Text = _PluginName;
        }
        string IPlugin.plugAssemblyName
        {
            get { return _PluginAssembly; }
            set { _PluginAssembly = value; }
        }
        string IPlugin.plugSoftName
        {
            get { return _SoftName; }
            set { _SoftName = value; }
        }
        string IPlugin.plugName
        {
            get { return _PluginName; }
            set { _PluginName = value; }
        }       
        void IPlugin.plugDeinit()
        {
         //   MessageBox.Show(" plugin1  deinit ");
        }

        void IPlugin.appListAdd() {}

        void IPlugin.appListDel() { }

        void IPlugin.appRefresh() { }

        void IPlugin.appRefreshFilter() { }

        void IPlugin.appOption() { }

        private void mySubMenu1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню1, плагин 1");
        }

        private void mySubMenu2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("мое меню2, плагин 1");
        }

        int IPlugin.plugWorkplaceID { get { return _WorkplaceID; } 
                                      set { _WorkplaceID = value; } }
        int IPlugin.plugPluginID    { get { return _PluginID; }
                                      set { _PluginID = value; } }
        DelegClosePlugin IPlugin.plugEventClosed { set { _ClosePlugin = value; } }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ClosePlugin(_WorkplaceID, _PluginID);
        }


        private void tsb_Import_Click(object sender, EventArgs e)
        {
            frmExecuteMIAC fmExecuteMIAC = new frmExecuteMIAC(_dbsql);
            fmExecuteMIAC.ShowDialog();
            _PrikDate = fmExecuteMIAC._PrikDate;
            fmExecuteMIAC.Dispose();
        }

        private void tb_reportMIAC_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 22";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по данным ГБУЗ МИАЦ").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            _excel = new ExcelManager(Application.StartupPath + _templatename);
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:P", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))

                MessageBox.Show(new Form() { TopMost = true }, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
               // MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void результатыСверкиССРЗToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_SRZ.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 23";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            _excel = new ExcelManager(Application.StartupPath + _templatename);
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:O", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_SRZ_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void DropCreateDir(string inDir)
        { 
            if (Directory.Exists(inDir))
            {
                string[] allFoundFiles;
                allFoundFiles = Directory.GetFiles(inDir, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in allFoundFiles)
                    File.Delete(file);
                Directory.Delete(inDir);
            }
            if (!Directory.Exists(inDir)) Directory.CreateDirectory(inDir);
        }

        private void tsb_notPrik_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(_MainForm, "Выгрузить данные по неприкрепленному населению?", "Прикрепленное население", 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1207_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1507_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1807_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\4407_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1107_PR");

            _dbsql.SQLScript = @"select '23507' SMO
                                union
                                select '23807' SMO
                                union
                                select '23107' SMO
                                union
                                select '23407' SMO
                                union
                                select '23207' SMO";
            DataTable SmoTab = _dbsql.FillDataSet().Tables[0];

            string _prTableName = "PEOPLE_SRZ s ";

            foreach (DataRow rw in SmoTab.Rows)
            {
                _dbsql.SQLScript = @"set dateformat dmy select distinct lpu_itog as lpu from " + _prTableName + @"
                                    where q = '" + rw["SMO"].ToString() +
                                    "' and ds is null and (dstop is null or dstop > '22.03.2016') and lpu_itog is not null and lpu_str_miac is null ";

                DataTable LpuTab = _dbsql.FillDataSet().Tables[0];

                foreach (DataRow r in LpuTab.Rows)
                {
                    String ZipFileName_ = "";

                    File.Delete("\\" + "\\s-db3\\flk\\prikDP.dbf");
                    File.Copy("\\" + "\\s-db3\\flk\\To_SMO\\prik.dbf", "\\" + "\\s-db3\\flk\\prikDP.dbf");

                    _dbsql.SQLScript = @"set dateformat dmy
                            insert into dblink...prikDP(ID,IDSTR,SMO,DPFS,DPFS_S,DPFS_N,FAM,IM,OT,SEX,DATR,SNILS,DOC,DOC_S,DOC_N,CODE_MO,PRIK,PRIK_D,R_NAME,C_NAME,NP_NAME,UL_NAME,DOM,KOR,KV ,PR_NAME,PC_NAME,PNP_NAME,PUL_NAME,PDOM,PKOR,PKV ,MIACRES,F_ENP)
                            select  ID, 
		                            ID as IDSTR, 
		                            KSMO as SMO, 
		                            (case when OPDOC = 1 then 'С' when OPDOC = 2 then 'В' when OPDOC = 3 then 'П' end) as DPFS,
		                            left(SPOL, 12) as DPFS_S, 
		                            left((case when OPDOC in (1,2) then NPOL when OPDOC = 3 then ENP end),20) as DPFS_N,
		                            left(FAM, 40) as FAM, 
		                            left(IM, 40) as IM, 
		                            left(OT, 40) as OT, 
		                            (case when W = 1 then 'М' else 'Ж' end) as SEX, 
		                            DR as DATR,
		                            SS as SNILS,
                                    left(DOCTP, 2) as DOC, 
		                            left(DOCS, 10) as DOC_S, 
		                            left(DOCN, 15) as DOC_N, 
                                    isnull(lpu_str_miac, lpu_itog) as CODE_MO, 
	                                prik_miac as PRIK, 
		                            prik_d_miac as PRIK_D, 

                                    /*case when isnull(BOMJ,0) = 0 then left(isnull(isnull(RNNAME, city), NP), 30) else left(isnull(isnull(pRNNAME, pcity), pNP), 30) end as R_NAME, 
									case when isnull(BOMJ,0) = 0 then left(CITY, 30) else left(pCITY, 30) end as C_NAME, 
									case when isnull(BOMJ,0) = 0 then left(NP, 40) else left(pNP, 40) end as NP_NAME, 
									case when isnull(BOMJ,0) = 0 then left(UL, 40) else left(pUL, 40) end as UL_NAME,
									case when isnull(BOMJ,0) = 0 then left(DOM, 7) else left(pDOM, 7) end as DOM, 
									case when isnull(BOMJ,0) = 0 then left(KOR, 5) else left(pKOR, 5) end as KOR, 
									case when isnull(BOMJ,0) = 0 then left(KV, 5) else left(pKV, 5) end as KV,*/
                                    
                                    left(isnull(isnull(RNNAME, city), NP), 30) as RN_NAME,
                                    left(CITY, 30) as C_NAME,
                                    left(NP, 40) as NP_NAME,
                                    left(UL, 40) as UL_NAME,
                                    left(DOM, 7) as DOM,
                                    left(KOR, 5) as KOR,
                                    left(KV, 5) as KV,

                                    left(isnull(isnull(pRNNAME, pcity), pNP), 30) as PRN_NAME,
                                    left(pCITY, 30) as PC_NAME,
                                    left(pNP, 40) as PNP_NAME,
                                    left(pUL, 40) as PUL_NAME,
                                    left(pDOM, 7) as PDOM,
                                    left(pKOR, 5) as PKOR,
                                    left(pKV, 5) as PKV,

		                            '001' as MIACRES, 
		                            ENP as F_ENP                /*left(Adress, 250) as adres*/
                            from " + _prTableName + @"                             
                            where q='" + rw["SMO"].ToString() + "' and lpu_itog='" + r["lpu"].ToString() + "' and ds is null and (dstop is null or dstop > '22.03.2016') and lpu_itog is not null and lpu_str_miac is null";
                            
                    _dbsql.ExecuteNonQuery();

                    if (rw["smo"].ToString() == "23507")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prikDP.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\prik1507_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\prik1507_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23807")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prikDP.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\prik1807_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\prik1807_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23107")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prikDP.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\prik1107_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\prik1107_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23407")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prikDP.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\prik4407_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\prik4407_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23207")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prikDP.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\prik1207_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\prik1207_" + r["lpu"].ToString() + ".dbf";
                    }

                    ZipFile zf = null;
                    try
                    {
                        zf = new ZipFile(Path.ChangeExtension(ZipFileName_, "zip"));
                        zf.AddFile(ZipFileName_, "");
                        zf.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_MainForm, "Ошибка создания архива: " + ex.Message, "Прикрепленное население", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        zf.Dispose();
                        File.Delete(ZipFileName_);
                    }
                }

            }
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();

            MessageBox.Show(_MainForm, "Сведения выгружены, заархивируйте и передайте в МО!");
        }

        private void tsb_PrikToMO_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(_MainForm, "Выгрузить данные по прикреплению к медицинским организациям?", "Прикрепленное население",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            string _fileOut = "c:\\Temp\\"+"MO123207" + "20150831" /*DateTime.Now.Year.ToString() + ("0"+DateTime.Now.Month.ToString()). + DateTime.Now.Day.ToString()*/ + ".csv";
            if (File.Exists(_fileOut)) File.Delete(_fileOut);

            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 24";
            _dbsql.ExecuteNonQuery("Анализ и подготовка данных для выгрузки");
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 25";
            SqlDataReader restab = _dbsql.ExecuteReader();

            StreamWriter w = new StreamWriter(_fileOut, true, Encoding.GetEncoding("Windows-1251"));
            while (restab.Read())
            {
                w.WriteLine(restab.GetString(0) + ";" + restab.GetString(1) + ";" + restab.GetString(2) + ";" + restab.GetString(3) + ";" + restab.GetString(4) + ";" + restab.GetString(5) + ";" + 
                            restab.GetString(6) + ";" + restab.GetString(7) + ";" + restab.GetString(8) + ";" + restab.GetString(9) + ";" + restab.GetString(10) + ";" + restab.GetString(11) + ";" +
                            restab.GetString(12) + ";" + restab.GetString(13) + ";" + restab.GetString(14) + ";" + restab.GetString(15) + ";" + restab.GetString(16) + ";" + restab.GetString(17));
                w.Flush();
            }
            restab.Close();
            w.Flush();
            w.Close();

            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();
            MessageBox.Show(_MainForm, "Файл прикрепления к МО сформирован. Загрузите его в СРЗ " + _fileOut);
        }

        private void ExportCSV_vr(string inSMO)
        {
            string _fileOut = "c:\\Temp\\" + "MO1" + inSMO + "20150911" /*DateTime.Now.Year.ToString() + ("0"+DateTime.Now.Month.ToString()). + DateTime.Now.Day.ToString()*/ + ".csv";
            if (File.Exists(_fileOut)) File.Delete(_fileOut);

            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 26, @SMO = " + inSMO;
            SqlDataReader restab = _dbsql.ExecuteReader();

            StreamWriter w = new StreamWriter(_fileOut, true, Encoding.GetEncoding("Windows-1251"));
            while (restab.Read())
            {
                w.WriteLine(
                            restab.GetString(0) + ";" + restab.GetString(1) + ";" + restab.GetString(2) + ";" + restab.GetString(3) + ";" + restab.GetString(4) + ";" + restab.GetString(5) + ";" +
                            restab.GetString(6) + ";" + restab.GetString(7) + ";" + restab.GetString(8) + ";" + restab.GetString(9) + ";" + restab.GetString(10) + ";" + restab.GetString(11) + ";" +
                            restab.GetString(12) + ";" + restab.GetString(13) + ";" + restab.GetString(14) + ";" + restab.GetString(15) + ";" + restab.GetString(16) + ";" + restab.GetString(17) + ";" +
                            restab.GetString(18) + ";" + restab.GetString(19) + ";" + restab.GetString(20) + ";" + restab.GetString(21) + ";" + restab.GetString(22) + ";" + restab.GetString(23)
                           );
                w.Flush();
            }
            restab.Close();
            w.Flush();
            w.Close();        
        }

        private void tsb_PrikToMEDR_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(_MainForm, "Выгрузить данные по прикреплению к врачу?", "Прикрепленное население",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 24";
            _dbsql.ExecuteNonQuery("Анализ и подготовка данных для выгрузки");

            ExportCSV_vr("23107");
            ExportCSV_vr("23807");
            ExportCSV_vr("23507");
            ExportCSV_vr("23407");
            ExportCSV_vr("23207");

            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();
            MessageBox.Show(_MainForm, "Файл прикрепления к врачу сформирован. Загрузите его в СРЗ");
        }

        private void tsb_DoctorPower_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;            
            string _templatename = "\\reports\\reportPrikMIAC_SNILS.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 27";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по врачам").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:H", 11, true,
                                          _MainForm);
           // _excel.MergeEqualCellsInColumn("C", indexFirstExcelRow, (dt.Rows.Count + indexFirstExcelRow) - 1);
           // _excel.MergeEqualCellsInColumn("E", indexFirstExcelRow, (dt.Rows.Count + indexFirstExcelRow) - 1);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_SNILS_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsbUchReport_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_UCH.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 28";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по участкам").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:H", 11, true,
                                          _MainForm);
            //_excel.MergeEqualCellsInColumn("C", indexFirstExcelRow, (dt.Rows.Count + indexFirstExcelRow) - 1);
            //_excel.MergeEqualCellsInColumn("F", indexFirstExcelRow, (dt.Rows.Count + indexFirstExcelRow) - 1);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_UCH_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsbSRZ_mun_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_SRZ_mun.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 30";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:M", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_SRZ_MUN_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsbNoprik_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_NOPRIK.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 29";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:C", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_NOPRIK_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsb_polAge_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_AGE_POL_mun.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 31";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 0; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 6; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("C" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "C" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:N", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_AGE_POL_MUN_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsbError_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_ERROR.xlt";

            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 32";
            DataTable dt = null;
                dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];


            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("E" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "E" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:F", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_ERROR_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsbMRF_SRZ_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_SRZ_MRF.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 33";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }
            
            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 1; //если например из результирующего набора не выводить Первый столбец ID
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 8; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("A" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "A" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:M", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_SRZ_MRF_" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void tsb_prikSMO_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(_MainForm, "Выгрузить данные по прикрепленному населению для СМО?", "Прикрепленное население",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Application.DoEvents();

            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1207_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1507_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1807_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\4407_PR");
            DropCreateDir("\\\\s-db3\\flk\\To_SMO\\1107_PR");

            _dbsql.SQLScript = @"select '23507' SMO
                                union
                                select '23807' SMO
                                union
                                select '23107' SMO
                                union
                                select '23407' SMO
                                union
                                select '23207' SMO";
            DataTable SmoTab = _dbsql.FillDataSet().Tables[0];

            string _prTableName = "PEOPLE_SRZ_22032016 s ";
            string _ucTableName = "UCH_MIAC_22032016 s ";

            foreach (DataRow rw in SmoTab.Rows)
            {
                _dbsql.SQLScript = @"set dateformat dmy select distinct lpu_str_miac as lpu from " + _prTableName + @"
                                    where q = '" + rw["SMO"].ToString() +
                                    "' and ds is null and (dstop is null or dstop > '22.03.2016') and lpuur is not null and lpu_str_miac is not null and lpu_str_miac like '19%' ";

                DataTable LpuTab = _dbsql.FillDataSet().Tables[0];

                foreach (DataRow r in LpuTab.Rows)
                {
                    String ZipFileName_ = "";
                    String ZipFileNameUc_ = "";

                    File.Delete("\\" + "\\s-db3\\flk\\prik_smo.dbf");
                    File.Copy("\\" + "\\s-db3\\flk\\To_SMO\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\prik_smo.dbf");

                    File.Delete("\\" + "\\s-db3\\flk\\uch_smo.dbf");
                    File.Copy("\\" + "\\s-db3\\flk\\To_SMO\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\uch_smo.dbf");

                    _dbsql.SQLScript = @"set dateformat dmy
                            insert into dblink...prik_smo(ID,IDSTR,SMO,DPFS,DPFS_S,DPFS_N,FAM,IM,OT,SEX,DATR,SNILS,DOC,DOC_S,DOC_N,CODE_MO,PRIK,PRIK_D,UCH,R_NAME,C_NAME,NP_NAME,UL_NAME,DOM,KOR,KV,MIACRES,
                                                          F_ENP,F_SMO, F_DPFS, F_DPFS_S, F_DPFS_N, F_FAM, F_IM, F_OT, F_SEX, F_DATR, F_SNILS, F_DOC, F_DOC_S, F_DOC_N)
                            select  ID, 
		                            ID as IDSTR, 
		                            KSMO as SMO, 
		                            (case when OPDOC = 1 then 'С' when OPDOC = 2 then 'В' when OPDOC = 3 then 'П' end) as DPFS,
		                            left(SPOL, 12) as DPFS_S, 
		                            left((case when OPDOC in (1,2) then NPOL when OPDOC = 3 then ENP end),20) as DPFS_N,
		                            left(FAM, 40) as FAM, 
		                            left(IM, 40) as IM, 
		                            left(OT, 40) as OT, 
		                            (case when W = 1 then 'М' else 'Ж' end) as SEX, 
		                            DR as DATR,
		                            SS as SNILS,
                                    left(DOCTP, 2) as DOC, 
		                            left(DOCS, 10) as DOC_S, 
		                            left(DOCN, 15) as DOC_N, 
                                    lpu_str_miac as CODE_MO, 
	                                prik_miac as PRIK, 
		                            prik_d_miac as PRIK_D, 
                                    UCH_MIAC as UCH,                                 
                                    left(isnull(isnull(RNNAME, city), NP), 30) as RN_NAME, left(CITY, 30) as C_NAME, left(NP, 40) as NP_NAME, left(UL, 40) as UL_NAME, left(DOM, 7) as DOM, left(KOR, 5) as KOR, left(KV, 5) as KV,
		                            '001' as MIACRES, 
		                            ENP as F_ENP,
                                    KSMO as F_SMO, 
		                            (case when OPDOC = 1 then 'С' when OPDOC = 2 then 'В' when OPDOC = 3 then 'П' end) as F_DPFS,
		                            left(SPOL, 12) as F_DPFS_S, 
		                            left((case when OPDOC in (1,2) then NPOL when OPDOC = 3 then ENP end),20) as F_DPFS_N,
                                    left(FAM, 40) as F_FAM, 
		                            left(IM, 40) as F_IM, 
		                            left(OT, 40) as F_OT, 
		                            (case when W = 1 then 'М' else 'Ж' end) as F_SEX, 
		                            DR as F_DATR,
                                    SS as F_SNILS,
                                    left(DOCTP, 2) as F_DOC, 
		                            left(DOCS, 10) as F_DOC_S, 
		                            left(DOCN, 15) as F_DOC_N
                            from " + _prTableName + @"                             
                            where q='" + rw["SMO"].ToString() + "' and lpu_str_miac='" + r["lpu"].ToString() + "' and ds is null and (dstop is null or dstop > '22.03.2016') and lpuur is not null and lpu_str_miac is not null";
                    _dbsql.ExecuteNonQuery();

                    _dbsql.SQLScript = @"set dateformat dmy
                            insert into dblink...uch_smo(code_mo, uch, d_spec, d_fam, d_im, d_ot, d_snils, d_prik_uch, d_otkr_uch, miacres)
                            select code_mo, uch, d_spec, d_fam, d_im, d_ot, d_snils, d_prik_uch, d_otkr_uch, '001' as miacres
                            from " + _ucTableName + @"                             
                            where code_mo='" + r["lpu"].ToString() + "' ";
                    _dbsql.ExecuteNonQuery();

                    if (rw["smo"].ToString() == "23507")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\prik1507_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\prik1507_" + r["lpu"].ToString() + ".dbf";

                        File.Copy("\\" + "\\s-db3\\flk\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\uch1507_" + r["lpu"].ToString() + ".dbf");
                        ZipFileNameUc_ = "\\" + "\\s-db3\\flk\\To_SMO\\1507_PR\\uch1507_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23807")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\prik1807_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\prik1807_" + r["lpu"].ToString() + ".dbf";

                        File.Copy("\\" + "\\s-db3\\flk\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\uch1807_" + r["lpu"].ToString() + ".dbf");
                        ZipFileNameUc_ = "\\" + "\\s-db3\\flk\\To_SMO\\1807_PR\\uch1807_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23107")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\prik1107_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\prik1107_" + r["lpu"].ToString() + ".dbf";

                        File.Copy("\\" + "\\s-db3\\flk\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\uch1107_" + r["lpu"].ToString() + ".dbf");
                        ZipFileNameUc_ = "\\" + "\\s-db3\\flk\\To_SMO\\1107_PR\\uch1107_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23407")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\prik4407_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\prik4407_" + r["lpu"].ToString() + ".dbf";

                        File.Copy("\\" + "\\s-db3\\flk\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\uch4407_" + r["lpu"].ToString() + ".dbf");
                        ZipFileNameUc_ = "\\" + "\\s-db3\\flk\\To_SMO\\4407_PR\\uch4407_" + r["lpu"].ToString() + ".dbf";
                    }
                    else if (rw["smo"].ToString() == "23207")
                    {
                        File.Copy("\\" + "\\s-db3\\flk\\prik_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\prik1207_" + r["lpu"].ToString() + ".dbf");
                        ZipFileName_ = "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\prik1207_" + r["lpu"].ToString() + ".dbf";

                        File.Copy("\\" + "\\s-db3\\flk\\uch_smo.dbf", "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\uch1207_" + r["lpu"].ToString() + ".dbf");
                        ZipFileNameUc_ = "\\" + "\\s-db3\\flk\\To_SMO\\1207_PR\\uch1207_" + r["lpu"].ToString() + ".dbf";
                    }

                    ZipFile zf = null;
                    try
                    {
                        zf = new ZipFile(Path.ChangeExtension(ZipFileName_, "zip"));
                        zf.AddFile(ZipFileName_, "");
                        zf.AddFile(ZipFileNameUc_, "");
                        zf.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(_MainForm, "Ошибка создания архива: " + ex.Message, "Прикрепленное население", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    finally
                    {
                        zf.Dispose();
                        File.Delete(ZipFileName_);
                        File.Delete(ZipFileNameUc_);
                    }
                }

            }
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            Application.DoEvents();

            MessageBox.Show(_MainForm, "Сведения выгружены, заархивируйте и передайте в CМО!");
        }

        private void прикрепленноеНаселениеВРазрезеЛПУИСМОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelManager _excel = null;
            string _templatename = "\\reports\\reportPrikMIAC_SMO_LPU.xlt";
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 34";
            DataTable dt = _dbsql.FillDataSet("Формирование статистики по результатам сверки").Tables[0];
            //создаем ексел
            _MainForm.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            { _excel = new ExcelManager(Application.StartupPath + _templatename); }
            catch (ExceptionManeger E)
            {
                MessageBox.Show(E.PMessageString);
            }

            _excel.SetCalculation(ECalculation.xlCalculationManual);
            _excel.SetScreenUpdating(false);
            // _excel.SetValueToRange("C3", inParams["DATEREPORT"]);            
            //массив данных, который начинается с столбца №2, т.е. указав номер столбца, можно исключить первые стоблцы
            int indexStartColumn = 0; //1 - если например из результирующего набора не выводить Первый столбец ID ,   0 - если выводить все столбцы
            object[,] DataArray = _excel.GetDataArrayFromDataTable(dt, indexStartColumn, _MainForm);
            dt.Dispose();
            //перенести массива в Excel
            //стартовая ячейка
            int indexFirstExcelRow = 5; //    7
            int indexFirstExcelColumn = 1; // A            
            string _Range = CExcelRanges.Ranges[indexFirstExcelColumn - 1] + indexFirstExcelRow.ToString() +   //начало диапазона  J10   -1 т.к.массив с 0
                            ":" +
                            CExcelRanges.Ranges[(indexFirstExcelColumn - 1 + dt.Columns.Count - indexStartColumn) - 1] + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString();  //конец диапазона
            _excel.SetValueToRange(_Range, DataArray);
            _excel.SetFormatToFindInRange("B" + indexFirstExcelRow.ToString() +
                                          ":" +
                                          "B" + ((dt.Rows.Count + indexFirstExcelRow) - 1).ToString(),
                                          "ИТОГО",
                                          "A:D", 11, true,
                                          _MainForm);
            _excel.SetBorderStyle(_Range, BorderLineStyle.xlContinuous);
            _excel.SetScreenUpdating(true);
            DataArray = null;
            _MainForm.Cursor = System.Windows.Forms.Cursors.Default;
            //сохраняем выходим           
            if (_excel.SaveDocument("c:\\temp\\reportPrikMIAC_SMO_LPU" + _PrikDate.ToShortDateString().Replace(".", "") + ".xls", true, ESaveFormats.xlNormal))
                MessageBox.Show(_MainForm, "Отчет сформирован и сохранён в каталоге c:\\temp\\", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _excel.Visible = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string[] allFoundFiles = Directory.GetFiles(Path.GetDirectoryName("d:\\load\\"), "*.xml", SearchOption.TopDirectoryOnly);
            foreach (string file in allFoundFiles)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    string XMLText = doc.InnerXml;
                    _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 35, @DOC = @pDOC, @filename = @pFilename";
                    _dbsql.AddParameter("@pDOC", EnumDBDataTypes.EDT_String, XMLText);
                    _dbsql.AddParameter("@pFilename", EnumDBDataTypes.EDT_String, Path.GetFileNameWithoutExtension(file));
                    int st = 0;                
                    st = _dbsql.ExecuteNonQuery();
                }
                catch(Exception errr)
                {
                    MessageBox.Show(errr.Message);
                    File.Copy(file, "d:\\load\\error\\" + Path.GetFileName(file) + "E",true);
                    continue;
                }
                File.Copy(file, "d:\\load\\good\\" + Path.GetFileName(file),true);
            }
            MessageBox.Show(_MainForm, "Файлы загружены");
        }




       
    }
}