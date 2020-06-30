using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;
using SharpCompress.Common;
using SharpCompress.Reader;
using SharpCompress.Writer;
using System.Data.SqlClient;
using GGPlatform.Zip7Manag;



namespace prip_pl
{
    public partial class frmExecuteMIAC : Form
    {
        private DBSqlServer _dbsql;
        private string _PrikFileName = "";
        public DateTime _PrikDate;
        private Boolean _PrikLoad = false;
        private Stream myStream = null;
        private Zip7Manager _arh = null;
        
        public frmExecuteMIAC(DBSqlServer inDBSql)
        {
            InitializeComponent();
            _dbsql = inDBSql;
        }

      //  public DateTime _
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_PrikLoad)
            {
                e.Cancel = true;
                MessageBox.Show("Нельзя закрыть форму пока идет процесс обработки сведений от ГБУЗ МИАЦ!", "Внимание", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MyExtract(string inZipFileName, string inExtractDir)
        {
            using (Stream stream = File.OpenRead(inZipFileName))  // WinRAR
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(inExtractDir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
        }

        private void MyArhive(string inZipFileName, List<string> inFiles)
        {
            _arh = new Zip7Manager();
            _arh.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            _arh.ToArhiveFileList(inZipFileName, inFiles, false);
        }

        private void MyDataExportToDBF(string inFileName)
        {
            string _prik = " select ID, IDSTR, SMO, DPFS, _DPFS_S, _DPFS_N, _FAM, _IM, _OT, SEX, _DATR, SNILS, DOC, _DOC_S, _DOC_N, CODE_MO, OID_MO, PRIK, PRIK_D, OTKR_D, UCH, "+
	                        " R_NAME, C_NAME, NP_NAME, UL_NAME, DOM, KOR, KV, SMORES, S_COMM, MIACRES, M_COMM, TFOMSRES, "+
	                        " F_ENP, F_SMO, F_DPFS, F_DPFS_S, F_DPFS_N, F_FAM, F_IM, F_OT, F_SEX, F_DATR, F_DSTOP, F_DATS, F_SNILS, F_DOC, F_DOC_S, F_DOC_N, F_COMM "+
	                        " from PRIK_MIAC ";
            string _uch = " select CODE_MO, UCH, D_SPEC, D_FAM, D_IM, D_OT, D_SNILS, D_PRIK_UCH, D_OTKR_UCH, MIACRES from UCH_MIAC ";

            if (inFileName.Contains("PRIK1")) _dbsql.SQLScript = "insert into dblink...prik1 " + _prik + " where idd between 1 and 1000000 ";
            if (inFileName.Contains("PRIK2")) _dbsql.SQLScript = "insert into dblink...prik2 " + _prik + " where idd between 1000001 and 2000000 ";
            if (inFileName.Contains("PRIK3")) _dbsql.SQLScript = "insert into dblink...prik3 " + _prik + " where idd between 2000001 and 3000000 ";
            if (inFileName.Contains("PRIK4")) _dbsql.SQLScript = "insert into dblink...prik4 " + _prik + " where idd between 3000001 and 4000000 ";
            if (inFileName.Contains("PRIK5")) _dbsql.SQLScript = "insert into dblink...prik5 " + _prik + " where idd between 4000001 and 5000000 ";
            if (inFileName.Contains("PRIK6")) _dbsql.SQLScript = "insert into dblink...prik6 " + _prik + " where idd between 5000001 and 6000000 ";
            if (inFileName.Contains("PRIK7")) _dbsql.SQLScript = "insert into dblink...prik7 " + _prik + " where idd >= 6000001 ";
            if (inFileName.Contains("UCH")) _dbsql.SQLScript = "insert into dblink...uch " + _uch;

            _dbsql.ExecuteNonQuery("Выгрузка данных в файл " + inFileName);
        }

        private void MyExportToDBF()
        {        
            try
            {
                if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
                string _PRIK_EXPORT = Application.StartupPath + "\\resources\\PRIK_EXPORT.dbf";
                if (!File.Exists(_PRIK_EXPORT))
                {
                    MessageBox.Show("Шаблон экспорта " + _PRIK_EXPORT + " отсутствует");
                    return;
                }
                _PRIK_EXPORT = Application.StartupPath + "\\resources\\UCH_EXPORT.dbf";
                if (!File.Exists(_PRIK_EXPORT))
                {
                    MessageBox.Show("Шаблон экспорта " + _PRIK_EXPORT + " отсутствует");
                    return;
                }
                _PRIK_EXPORT = Application.StartupPath + "\\resources\\PRIK_EXPORT.dbf";
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK1.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK2.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK3.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK4.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK5.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK6.dbf", true);
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\PRIK7.dbf", true);
                _PRIK_EXPORT = Application.StartupPath + "\\resources\\UCH_EXPORT.dbf";
                File.Copy(_PRIK_EXPORT, "\\\\s-db3\\FLK\\UCH.dbf", true);

                lb1.Items.Add(DateTime.Now.ToString() + " Выгрузка ответных файлов");
                Application.DoEvents();
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK1");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK1.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK2");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK2.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK3");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK3.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK4");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK4.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK5");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK5.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK6");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK6.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла PRIK7");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\PRIK7.dbf");
                lb1.Items.Add(DateTime.Now.ToString() + "    выгрузка файла UCH");
                Application.DoEvents();
                MyDataExportToDBF("\\\\s-db3\\FLK\\UCH.dbf");

                lb1.Items.Add(DateTime.Now.ToString() + " Архивирование ответных файлов на сервере");
                Application.DoEvents();
             /*   MyArhive("\\\\s-db3\\FLK\\PRIK_.7z", new List<string> 
                                                     {   "\\\\s-db3\\FLK\\PRIK?.dbf",
                                                         "\\\\s-db3\\FLK\\UCH.dbf"
                                                     } );*/
                //копирвоание файла
                ///File.Copy("\\\\s-db3\\FLK\\PRIK_.7z", Path.GetDirectoryName(tbPrip.Text) + "\\" + PRIK )
            }
            catch(Exception ex)
            {
                _PrikLoad = false;
                MessageBox.Show(ex.Message);                
            }        
        }

        private void btExecute_Click(object sender, EventArgs e)
        {
            string _PripExtractDir = Path.GetDirectoryName(_PrikFileName) + "\\" + Path.GetFileNameWithoutExtension(_PrikFileName);
            _PrikLoad = true;
            btExecute.Enabled = false;

            /*try
            {
                lb1.Items.Add(DateTime.Now.ToString() + " Разархивирование архивного файла");
                Application.DoEvents();
                MyExtract(_PrikFileName, _PripExtractDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка разархивирования " + ex.Message);
                _PrikLoad = false;
                return;
            }*/

            List<string> _ListPrikDBFFiles = new List<string>();

            lb1.Items.Add(DateTime.Now.ToString() + " Проверка наличия файлов в архиве");
            Application.DoEvents();
            string _testDBF = "";
            for (int i = 0; i < 9; i++)
            {
                _testDBF = _PripExtractDir + "\\PRIK" + i.ToString() + ".dbf";
                if (File.Exists(_testDBF))
                    _ListPrikDBFFiles.Add(_testDBF);
            }
          
            if (_ListPrikDBFFiles.Count == 0)
            {
                MessageBox.Show("В архиве отсутствуют файлы для импорта");
                return;
            }

            _testDBF = _PripExtractDir + "\\UCH.dbf";
            if (!(File.Exists(_testDBF)))
            {
                MessageBox.Show("В архиве отсутствуют файл UCH.dbf");
                return;
            }

            lb1.Items.Add(DateTime.Now.ToString() + " Подготовка БД для загрузки сведений от ГБУЗ МИАЦ");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 1"; ///создадим новую таблицу
            _dbsql.ExecuteNonQuery();

            lb1.Items.Add(DateTime.Now.ToString() + " Импорт файлов PRIK*.dbf");
            Application.DoEvents();
            _dbsql.BulkDestinationTable = "PRIK_MIAC";
            _dbsql.BulkMapperKey = "PRIK_MIAC";
            _dbsql.BulkBatchSize = 100000;
            DBBulkInfo _bulkinfo = new DBBulkInfo();
            foreach (string _file in _ListPrikDBFFiles)
            {
                _dbsql.BulkSourceFileName = _file;
                _bulkinfo = _dbsql.BulkExecuteFromDBF("Импорт " + Path.GetFileName(_file));
                if (string.IsNullOrEmpty(_bulkinfo.ProcessError))
                {
                    lb1.Items.Add(DateTime.Now.ToString() + "    " + _bulkinfo.ProcessMessage);
                    Application.DoEvents();
                }
                else
                {
                    _PrikLoad = false;
                    return;
                }
            }

            lb1.Items.Add(DateTime.Now.ToString() + " Импорт файла UCH.dbf");
            Application.DoEvents();
            _dbsql.BulkDestinationTable = "UCH_MIAC";
            _dbsql.BulkMapperKey = "UCH_MIAC";
            _dbsql.BulkBatchSize = 100000;
            _dbsql.BulkSourceFileName = _testDBF;
            _bulkinfo = _dbsql.BulkExecuteFromDBF("Импорт " + Path.GetFileName(_testDBF));
             if (string.IsNullOrEmpty(_bulkinfo.ProcessError))
             {
                 lb1.Items.Add(DateTime.Now.ToString() + "    " + _bulkinfo.ProcessMessage);
                 Application.DoEvents();
             }
             else
             {
                 _PrikLoad = false;
                 return;
             }

            lb1.Items.Add(DateTime.Now.ToString() + " Резервные копии ключевых полей");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 2"; ///копии полей
            _dbsql.ExecuteNonQuery("Выполняется копирование данных");

            lb1.Items.Add(DateTime.Now.ToString() + " Индексация загруженных данных");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 3"; ///индексация
            _dbsql.ExecuteNonQuery("Выполняется индексация данных");
       

   
            
            
            /*lb1.Items.Add(DateTime.Now.ToString() + " Подготовка БД для загрузки сведений из СРЗ");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 4"; ///создадим новую таблицу
            _dbsql.ExecuteNonQuery();

            lb1.Items.Add(DateTime.Now.ToString() + " Перенос данных из СРЗ");
            Application.DoEvents();
            lb1.Items.Add(DateTime.Now.ToString() + "    Перенос из СРЗ застрахованных граждан");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 5"; ///
            _dbsql.ExecuteNonQuery("Перенос из СРЗ застрахованных граждан");

            lb1.Items.Add(DateTime.Now.ToString() + "    Перенос из СРЗ документов ОМС");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 6"; ///
            _dbsql.ExecuteNonQuery("Перенос из СРЗ документов ОМС");

            lb1.Items.Add(DateTime.Now.ToString() + "    Перенос из СРЗ истории ФИО, УДЛ и ЕНП");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 7"; ///
            _dbsql.ExecuteNonQuery("Перенос из СРЗ истории ЕНП");
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 8"; ///
            _dbsql.ExecuteNonQuery("Перенос из СРЗ истории ФИО");
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 9"; ///
            _dbsql.ExecuteNonQuery("Перенос из СРЗ истории УДЛ");

            lb1.Items.Add(DateTime.Now.ToString() + " Индексация данных о застрахованных гражданах");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 10"; ///индексация
            _dbsql.ExecuteNonQuery("Выполняется индексация данных о застрахованных");
           */



            lb1.Items.Add(DateTime.Now.ToString() + " Идентификация данных ГБУЗ МИАЦ в БД СРЗ");
            Application.DoEvents();
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по актуальным данным застрахованных граждан");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 11"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по актуальным данных застрахованных граждан");            
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по истории полисов застрахованных граждан");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 12"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по истории полисов застрахованных граждан");            
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по истории ЕНП застрахованных граждан");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 13"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по истории ЕНП застрахованных граждан");
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по истории ФИО и УДЛ");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 14"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по истории ФИО и УДЛ");
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по истории ФИО и ОМС");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 15"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по истории ФИО и ОМС");
            lb1.Items.Add(DateTime.Now.ToString() + "    Идентификация по истории ФИО и ЕНП");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 16"; ///индексация
            _dbsql.ExecuteNonQuery("Идентификация по истории ФИО и ЕНП и старой БД застрахованных"); 
            
           
            lb1.Items.Add(DateTime.Now.ToString() + " Поиск дублирующих записей в БД МИАЦ");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 17"; ///дубли
            _dbsql.ExecuteNonQuery("Поиск дублирующих записей в БД МИАЦ");

            lb1.Items.Add(DateTime.Now.ToString() + " Актуализация персональных данных");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 18"; ///перенос персональных данных
            _dbsql.ExecuteNonQuery("Актуализация персональных данных");

            lb1.Items.Add(DateTime.Now.ToString() + " Формирование списка ошибок");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 19"; ///формирование списка ошибок
            _dbsql.ExecuteNonQuery("Формирование списка ошибок");
            
            
            //////
            lb1.Items.Add(DateTime.Now.ToString() + " Обработка результатов сверки, определение юридических лиц в сведениях МИАЦ");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 20"; ///формирование списка ошибок
            _dbsql.ExecuteNonQuery("Обработка результатов сверки МИАЦ");

            lb1.Items.Add(DateTime.Now.ToString() + " Обработка результатов сверки, определение юридических лиц в сведениях ТФОМС");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 21"; ///формирование списка ошибок
            _dbsql.ExecuteNonQuery("Обработка результатов сверки ТФОМС");
            
            lb1.Items.Add(DateTime.Now.ToString() + " Подготовка данных для ФФОМС");
            Application.DoEvents();
            _dbsql.SQLScript = "exec prikExecuteMIAC @TypeQuery = 24"; ///формирование списка ошибок
            _dbsql.ExecuteNonQuery("Подготовка данных для ФФОМС");            

            Application.DoEvents();
          //  MyExportToDBF();
            Application.DoEvents();

            lb1.Items.Add(DateTime.Now.ToString() + " Копирование данных");
            Application.DoEvents();
            _dbsql.SQLScript = " select * into PEOPLE_SRZ_" + _PrikDate.ToShortDateString().Replace(".", "") + " from people_srz where lpu_str_miac is not null; " +
                               " select * into PRIK_MIAC_" + _PrikDate.ToShortDateString().Replace(".", "") + " from prik_miac; " + 
                               " select * into UCH_MIAC_" + _PrikDate.ToShortDateString().Replace(".", "") + " from uch_miac; "; ///формирование списка ошибок
            _dbsql.ExecuteNonQuery("Копирование данных");
  
            lb1.Items.Add(DateTime.Now.ToString() + " Обработка закончена");
            Application.DoEvents();
            btExecute.Enabled = true;
            _PrikLoad = false;
            MessageBox.Show("!!!! Обработка сведений от ГБУЗ МИАЦ закончена. Сформируйте отчеты и файлы неприкрепленных !!!! " );
        }

        private void btPath_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "PRIK файлы (PRIK*.*)|PRIK*.*|Все файлы (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        _PrikFileName = openFileDialog1.FileName;
                        _PrikDate = dateTimePicker1.Value.Date;

                        tbPrip.Text = _PrikFileName;
                        using (myStream)
                        {
                            // Insert code to read the stream here.
                        }
                        btExecute.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не возможно прочитать файл. Ошибка " + ex.Message);
                }
            }
        }


    }
}
