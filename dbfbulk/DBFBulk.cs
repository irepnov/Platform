using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace GGPlatform.DBFBulk
{
    public class DBFReader : IDataReader
    {
        System.IO.FileStream FS;
        byte[] buffer;
        int _FieldCount;
        int FieldsLength;
        System.Globalization.DateTimeFormatInfo dfi = new System.Globalization.CultureInfo("en-US", false).DateTimeFormat;
        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        string[] FieldName;
        string[] FieldType;
        byte[] FieldSize;
        byte[] FieldDigs;
        int RowsCount;
        int ReadedRow = 0;
        string UserName = "";
        string CompName = "";
        string LoadFile = "";
        string DBFFileName = "";
        Dictionary<string, object> R = new Dictionary<string, object>();
        Dictionary<int, string> FieldIndex = new Dictionary<int, string>();

        public DBFReader(string inDBFFileName, Dictionary<int, string> inFieldDBFIndex, string inUserName, string inCompName, string inLoadFile)
        {
            DBFFileName = inDBFFileName;
            FS = new System.IO.FileStream(inDBFFileName, System.IO.FileMode.Open);
            buffer = new byte[4];
            FS.Position = 4;
            FS.Read(buffer, 0, buffer.Length);
            RowsCount = buffer[0] + (buffer[1] * 0x100) + (buffer[2] * 0x10000) + (buffer[3] * 0x1000000);
            buffer = new byte[2];
            FS.Position = 8;
            FS.Read(buffer, 0, buffer.Length);
            _FieldCount = (((buffer[0] + (buffer[1] * 0x100)) - 1) / 32) - 1;
            FieldName = new string[_FieldCount];  //имена полей в стреуткруе ДБФ
            FieldType = new string[_FieldCount];
            FieldSize = new byte[_FieldCount];
            FieldDigs = new byte[_FieldCount];
            buffer = new byte[32 * _FieldCount];
            FS.Position = 32;
            FS.Read(buffer, 0, buffer.Length);
            FieldsLength = 0;
            for (int i = 0; i < _FieldCount; i++)
            {
                FieldName[i] = System.Text.Encoding.Default.GetString(buffer, i * 32, 10).TrimEnd(new char[] { (char)0x00 });
                FieldType[i] = "" + (char)buffer[i * 32 + 11];
                FieldSize[i] = buffer[i * 32 + 16];
                FieldDigs[i] = buffer[i * 32 + 17];
                FieldsLength = FieldsLength + FieldSize[i];
            }
            FS.ReadByte();

            this.FieldIndex = inFieldDBFIndex;
            this.UserName = inUserName;
            this.LoadFile = inLoadFile;
            this.CompName = inCompName;
        }
        public object GetValue(int i)
        {
            try
            {
                return R[FieldIndex[i]];
            }
            catch
            {
                return DBNull.Value;
            }
        }
        public int FieldCount { get { return _FieldCount; } }
        string GetMemoField(string recordContent, string dbtFile)
        {
            if (recordContent.Trim().Length > 0)
            {
                int block = Int32.Parse(recordContent);
                return GetMemoFieldBlock(block, dbtFile);
            }
            return ""; // memo has no content: maybe should be null for more correctness
        }
        string GetMemoFieldBlock(int blockIndex, string dbtFile)
        {
            using (var fileStream = new FileStream(dbtFile, FileMode.Open, FileAccess.Read))
            {
                long start = blockIndex * 512;
                fileStream.Seek(start, SeekOrigin.Begin);
                var bytes = new List<byte>();
                while (fileStream.Position < fileStream.Length)
                {
                    byte c = (byte)fileStream.ReadByte();
                    long pos = fileStream.Position;
                    if (c == 0x1a
                        && fileStream.ReadByte() == 0x1a)
                    {
                        int ii;
                        ii = 3;
                        return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(866), System.Text.Encoding.UTF8, bytes.ToArray()));
                    }
                        
                    fileStream.Seek(pos, SeekOrigin.Begin);
                    bytes.Add(c);
                }
            }
            throw new InvalidOperationException();
        }
        public bool Read()
        {
            if (ReadedRow >= RowsCount) return false;

            R.Clear();
            buffer = new byte[FieldsLength];
            FS.ReadByte();
            FS.Read(buffer, 0, buffer.Length);
            int Index = 0;
            for (int i = 0; i < FieldCount; i++)
            {
                string l = System.Text.Encoding.GetEncoding(866).GetString(buffer, Index, FieldSize[i]).TrimEnd(new char[] { (char)0x00 }).TrimEnd(new char[] { (char)0x20 });
                Index = Index + FieldSize[i];
                object Tr;
                if (l.Trim() != "")
                {
                    try
                    { //если ошибка в чтении записи, то пропускаю эту запись
                        switch (FieldType[i])
                        {
                            case "L":
                                Tr = l == "T" ? true : false;
                                break;
                            case "D":
                                Tr = DateTime.ParseExact(l, "yyyyMMdd", dfi);
                                break;
                            case "N":
                                {
                                    if (FieldDigs[i] == 0)
                                        Tr = int.Parse(l, nfi);
                                    else
                                        Tr = decimal.Parse(l, nfi);
                                    break;
                                }
                          /*  case "M":
                                 Tr = GetMemoField(l, DBFFileName.ToLower().Replace(".dbf", ".dbt"));
                                 break;*/
                            case "F":
                                Tr = double.Parse(l, nfi);
                                break;
                            default:
                                Tr = l;
                                break;
                        }
                    }
                    catch
                    {
                        if (FS != null)
                        {
                            FS.Close();
                            FS.Dispose();
                        }
                        throw new Exception("Ошибка преобразования значения " + l + " поля " + FieldName[i] + " к типу данных " + FieldType[i]);
                    }
                }
                else
                {
                    Tr = DBNull.Value;
                }
                R.Add(FieldName[i], Tr);
            }

            if (FieldIndex.ContainsValue("_LOADFILE_")) R.Add("_LOADFILE_", LoadFile);
            if (FieldIndex.ContainsValue("_LOADUSER_")) R.Add("_LOADUSER_", UserName);
            if (FieldIndex.ContainsValue("_LOADCOMP_")) R.Add("_LOADCOMP_", CompName);

            ReadedRow++;
            return true;
        }
        public void Dispose()
        {
            if (FS != null)
            { 
                FS.Close();
                FS.Dispose();
            }            
        }
        public int Depth { get { return -1; } }
        public bool IsClosed { get { return false; } }
        public Object this[int i] { get { return new object(); } }
        public Object this[string name] { get { return new object(); } }
        public int RecordsAffected { get { return -1; } }
        public void Close() { }
        public bool NextResult() { return true; }
        public bool IsDBNull(int i) { return false; }
        public string GetString(int i) { return ""; }
        public DataTable GetSchemaTable() { return null; }
        public int GetOrdinal(string name) { return -1; }
        public string GetName(int i) { return ""; }
        public long GetInt64(int i) { return -1; }
        public int GetInt32(int i) { return -1; }
        public short GetInt16(int i) { return -1; }
        public Guid GetGuid(int i) { return new Guid(); }
        public float GetFloat(int i) { return -1; }
        public Type GetFieldType(int i) { return typeof(string); }
        public double GetDouble(int i) { return -1; }
        public decimal GetDecimal(int i) { return -1; }
        public DateTime GetDateTime(int i) { return new DateTime(); }
        public string GetDataTypeName(int i) { return ""; }
        public IDataReader GetData(int i) { return this; }
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) { return -1; }
        public char GetChar(int i) { return ' '; }
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) { return -1; }
        public byte GetByte(int i) { return 0x00; }
        public bool GetBoolean(int i) { return false; }
        public int GetValues(Object[] values) { return -1; }
    }
    public class StructuryDBF
    {
        public string FieldName { get; set; }
        public char FieldType { get; set; }
        public int FieldSize { get; set; }

        public StructuryDBF(string inFieldName, char inFieldType, int inFieldSize)
        {
            FieldName = inFieldName;
            FieldType = inFieldType;
            FieldSize = inFieldSize;
        }
    }    

    [Guid("F785AA8E-3EDA-4619-A612-C5B554DBCD5D")]
    [ComVisible(true)]
    public interface IDBFBulk
    {
        /// <summary>
        /// импорт данных из DBF в MSSQL server
        /// </summary>
        /// <param name="_usernameLoad">имя пользователя</param>
        /// <param name="_filenameLoad">имя файла</param>
        /// <param name="_compnameLoad">имя компьютера</param>
        /// <returns></returns>
        Boolean BulkExecuteFromDBF(string _usernameLoad, string _filenameLoad, string _compnameLoad);
        /// <summary>
        /// экспорт данных из MSSQL server в DBF
        /// </summary>
        /// <returns></returns>
        Boolean BulkExecuteToDBF(bool _Batching = false);
        /// <summary>
        /// источник информации (имя DBF файла или SQL скрипт для MSSQL)
        /// </summary>
        string BulkSourceFileORScript { get; set; }
        /// <summary>
        /// получатель информации (имя DBF файла или имя SQL таблицы)
        /// </summary>
        string BulkDestinationFileORTable { get; set; }
        /// <summary>
        /// полный путь к файлу соответствия полей
        /// </summary>
        string BulkMapperFile { set; }
        /// <summary>
        /// ключ в файле соответствия
        /// </summary>
        string BulkMapperKey { set; }
        /// <summary>
        /// настройка подключения к MSSQL серверу
        /// </summary>
        /// <param name="_Servername">имя сервера</param>
        /// <param name="_Basename">имя БД</param>
        /// <param name="_Username">логин</param>
        /// <param name="_Password">пароль</param>
        /// <returns></returns>
        Boolean ConfigSQL(string _Servername, string _Basename, string _Username, string _Password);
        /// <summary>
        /// количество записей для массовой вставки в MSSQL
        /// </summary>
        Int32 BulkBatchSize { set; }
        SqlConnection SqlConnect { set; }
        List<StructuryDBF> FieldStructuryDBF(string _dbffile);
    }

    [Guid("56BC08EA-8816-41B4-BE20-9701478D87D0")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class DBFBulk : IDBFBulk, IDisposable
    {
        private SqlConnection _Connection;
        private string _sConnectionString;
        private string _sBulkSourceFileORScript;
        private string _sBulkDestinationFileORTable;
        private string _sBulkMapperFileName;
        private XmlDocument _sBulkMapperFile;
        private XmlNodeList _xnListKeys;
        private string _sBulkMapperKey;
        private Int32 _sBulkBatchSize = 200000;
        private SqlConnection _sSqlConnect;
        private string _sServername = "";
        private string _sBasename = "";
        private string _sUsername = "";
        private string _sPassword = "";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sBulkMapperFile != null)
                    _sBulkMapperFile = null;
                /*
                if (_Connection != null)
                {
                    _Connection.Close();
                    _Connection = null;
                }
                */
            }
        }
        public string BulkSourceFileORScript
        {
            get { return _sBulkSourceFileORScript; }
            set { _sBulkSourceFileORScript = value; }
        }
        public string BulkDestinationFileORTable
        {
            get { return _sBulkDestinationFileORTable; }
            set
            {               
                if (value.Contains(":\\"))
                    if (!File.Exists(value)) throw new Exception("Файл назначения " + value + " не найден");
                _sBulkDestinationFileORTable = value;
            }
        }
        public string BulkMapperKey
        {
            set
            {                
                if (_sBulkMapperFile == null) throw new Exception("Файл соответствия не загружен");
                _xnListKeys = _sBulkMapperFile.SelectNodes("//MapperKey[@key='" + value + "']");
                if (_xnListKeys.Count == 0) throw new Exception("Не найден ключ сопоставления " + value);
                _sBulkMapperKey = value;
            }
        }
        public Int32 BulkBatchSize
        {
            set { _sBulkBatchSize = value; }
        }
        public SqlConnection SqlConnect
        {
            set { _sSqlConnect = value; }
        }
        public string BulkMapperFile
        {
            set
            {
                _sBulkMapperFileName = value;
                if (!File.Exists(_sBulkMapperFileName)) throw new Exception("Конфигурационный файл сопоставления " + _sBulkMapperFileName + " не найден");
                _sBulkMapperFile = new XmlDocument();
                _sBulkMapperFile.Load(_sBulkMapperFileName);
            }
        }
        public List<StructuryDBF> FieldStructuryDBF(string _dbffile)
        {
            List<StructuryDBF> _result = new List<StructuryDBF>();
            DbfFile _DBF = new DbfFile();
            if (!File.Exists(_dbffile)) throw new Exception("Файл " + _dbffile + " не найден");
            try
            {
                _DBF.Open(_dbffile, FileMode.Open);

                foreach(DbfColumn fil in _DBF.Header.mFields)
                {
                    _result.Add(new StructuryDBF(fil.Name, fil.ColumnTypeChar, fil.Length));
                }                    
                return _result;
            }
            catch
            {
                if (_DBF != null)
                    _DBF.Close();
                return _result;
            }
            finally
            {
                if (_DBF != null)
                    _DBF.Close();                    
            }
        }
        private bool ConnectSQL()
        {
            if (_sSqlConnect != null)
            {
                _Connection = _sSqlConnect;
                return true;
            }
            else
            {

                if (string.IsNullOrEmpty(_sServername)) throw new Exception("Не указан MSSQL сервер");
                if (string.IsNullOrEmpty(_sBasename)) throw new Exception("Не указано имя базы данных MSSQL сервера");
                if (string.IsNullOrEmpty(_sUsername)) throw new Exception("Не указано имя пользователя MSSQL сервера");

                _sConnectionString = "Data Source=" + _sServername + ";";
                _sConnectionString = _sConnectionString + "Persist Security Info=True;" + "User ID=" + _sUsername + ";" + "Password=" + _sPassword + ";";
                _sConnectionString = _sConnectionString + "Connection Timeout=0;Initial Catalog=" + _sBasename;

                _Connection = new SqlConnection(_sConnectionString);
                try
                {
                    if (_Connection.State != ConnectionState.Open)
                        _Connection.Open();

                    return (_Connection.State == ConnectionState.Open);
                }
                catch //(Exception Er)
                {
                    _Connection.Close();
                    _Connection.Dispose();
                    _Connection = null;
                    throw;
                }
            }
        }
        private bool DisconnectSQL()
        {
            try
            {
              //  if (_Connection != null)
             //       _Connection.Close();
           //     _Connection.Dispose();
                return true;
            }
            catch //(Exception Er)
            {
                _Connection.Close();
                _Connection.Dispose();
                _Connection = null;
                throw;
            }
        }
        public bool ConfigSQL(string _Servername, string _Basename, string _Username, string _Password)
        {
            _sServername = _Servername;
            _sBasename = _Basename;
            _sUsername = _Username;
            _sPassword = _Password;
            return ConnectSQL();
        }
        public bool BulkExecuteFromDBF(string _usernameLoad, string _filenameLoad, string _compnameLoad)
        {
            Boolean _result = false;
            SqlBulkCopy _BulkCopy;
            OleDbCommand _DBFSelect = new OleDbCommand();
            OleDbDataReader _DBFReader;
            string _conStringDBF = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(_sBulkSourceFileORScript) + ";Extended Properties=dBASE IV;User ID=;Password=;";
            OleDbConnection _DBFConnect = new OleDbConnection(_conStringDBF);
            try
            {
                _DBFSelect.Connection = _DBFConnect;
                _DBFSelect.CommandTimeout = 0;

                if (_Connection == null) ConnectSQL();
                if (_Connection.State != ConnectionState.Open) throw new Exception("Отсутствует соединение с MSSQL сервером");
                if (!File.Exists(_sBulkSourceFileORScript)) throw new Exception("Файл источник " + _sBulkSourceFileORScript + " не найден");
                if (string.IsNullOrEmpty(_sBulkDestinationFileORTable)) throw new Exception("Таблица назначения " + _sBulkDestinationFileORTable + " не указана");

                string _SourceFields = "";
                foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes)
                    _SourceFields = _SourceFields + xn.Attributes["source"].Value + ", ";

                _SourceFields = _SourceFields.Replace("_LOADFILE_", "'" + _filenameLoad + "' as _LOADFILE_");  ///таких полей нет в DBF, их заменяю самостоятельно
                _SourceFields = _SourceFields.Replace("_LOADCOMP_", "'" + _compnameLoad + "' as _LOADCOMP_");
                _SourceFields = _SourceFields.Replace("_LOADUSER_", "'" + _usernameLoad + "' as _LOADUSER_");
                _SourceFields = _SourceFields.Trim();
                if (_SourceFields.Substring(_SourceFields.Length - 1, 1) == ",") //последняя запятая
                    _SourceFields = _SourceFields.Substring(0, _SourceFields.Length - 1);

                _DBFConnect.Open();

                _DBFSelect.CommandText = @"select " + _SourceFields + " from " + Path.GetFileName(_sBulkSourceFileORScript);
                _DBFReader = _DBFSelect.ExecuteReader();
                _BulkCopy = new SqlBulkCopy(_Connection);
                _BulkCopy.DestinationTableName = _sBulkDestinationFileORTable;
                _BulkCopy.BulkCopyTimeout = 0;
                _BulkCopy.BatchSize = _sBulkBatchSize;
                foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes) ///сопоставление
                    _BulkCopy.ColumnMappings.Add(xn.Attributes["source"].Value, xn.Attributes["target"].Value);
                _BulkCopy.WriteToServer(_DBFReader);

                _DBFSelect.Cancel();
                _DBFSelect.Dispose();
                _DBFReader.Close();
                _DBFReader.Dispose();
                _DBFConnect.Close();
                _DBFConnect.Dispose();
                //  DisconnectSQL();
                _result = true;
            }
            catch 
            {
            //    DisconnectSQL();
                throw;
            }
            finally
            {
                _DBFSelect.Dispose();
                _DBFConnect.Close();
                _DBFConnect.Dispose();
                _BulkCopy = null;
              //  DisconnectSQL();
            }
            return _result;
        }
        public bool BulkExecuteFromDBF2(string _usernameLoad, string _filenameLoad, string _compnameLoad)
        {
            Boolean _result = false;
            SqlBulkCopy _BulkCopy;
            DBFReader _DBFReader = null; 
            Dictionary<int, string> FieldDBFIndex = new Dictionary<int, string>();

            try
            {
                if (_Connection == null) ConnectSQL();
                if (_Connection.State != ConnectionState.Open) throw new Exception("Отсутствует соединение с MSSQL сервером");
                if (!File.Exists(_sBulkSourceFileORScript)) throw new Exception("Файл источник " + _sBulkSourceFileORScript + " не найден");
                if (string.IsNullOrEmpty(_sBulkDestinationFileORTable)) throw new Exception("Таблица назначения " + _sBulkDestinationFileORTable + " не указана");

                _BulkCopy = new SqlBulkCopy(_Connection);

                _BulkCopy.DestinationTableName = _sBulkDestinationFileORTable;
                _BulkCopy.BulkCopyTimeout = 0;
                _BulkCopy.BatchSize = _sBulkBatchSize;

                int i = 0;
                foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes) ///сопоставление по порядковому номеру 
                {
                    FieldDBFIndex.Add(i, xn.Attributes["source"].Value);
                    _BulkCopy.ColumnMappings.Add(i, xn.Attributes["target"].Value);
                    i = i + 1;
                }

                _DBFReader = new DBFReader(_sBulkSourceFileORScript, FieldDBFIndex, _usernameLoad, _compnameLoad, _filenameLoad);
                    
                _BulkCopy.WriteToServer(_DBFReader);
                _BulkCopy.Close();

                if (_DBFReader != null)
                {
                    _DBFReader.Close();
                    _DBFReader.Dispose();
                }
                _result = true;
            }
            catch 
            {
                if (_DBFReader != null)
                {
                    _DBFReader.Close();
                    _DBFReader.Dispose();
                }                
                throw;
            }
            finally
            {
                if (_DBFReader != null)
                {
                    _DBFReader.Close();
                    _DBFReader.Dispose();
                }
                _BulkCopy = null;
            }
            return _result;
        }
        private bool BulkExecuteToDBF(string _sBulkSourceScript, bool _Batching, ref Int32 _sRecordLastID, ref Int32 _sRecordCount)
        {
            Boolean _result = false;

            SqlCommand _SQLCommand;
            SqlDataReader _SQLDr;
            DbfFile _DBF = new DbfFile();
            DbfRecord _REC;

            if (_Connection == null) ConnectSQL();  // <- добавил

            try
            {
                if (_Batching) //если выгрузка Порционная , то добавлю зачения Последнего ИД и размер порции
                {
                    _sBulkSourceScript = String.Format(_sBulkSourceScript + ", @minid = {0} , @toprecord = {1}", _sRecordLastID, _sBulkBatchSize);
                }
                _SQLCommand = new SqlCommand(_sBulkSourceScript, _Connection);
                _SQLCommand.CommandTimeout = 0;
                _SQLDr = _SQLCommand.ExecuteReader();
                _DBF.Open(_sBulkDestinationFileORTable, FileMode.Open);
                _REC = new DbfRecord(_DBF.Header);
                _REC.AllowDecimalTruncate = true;
                _REC.AllowIntegerTruncate = true;

                while (_SQLDr.Read())
                {
                    _REC.Clear();
                    foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes) ///сопоставление
                        if (_REC.Column(_REC.FindColumn(xn.Attributes["target"].Value)).ColumnType.ToString() == "Number")
                            _REC[_REC.FindColumn(xn.Attributes["target"].Value)] = _SQLDr[xn.Attributes["source"].Value].ToString().Replace(',', '.');
                        else
                            _REC[_REC.FindColumn(xn.Attributes["target"].Value)] = _SQLDr[xn.Attributes["source"].Value].ToString();
                    _DBF.Write(_REC, true);

                    if (_Batching)  //подсчет данных для следующей порции
                    {
                        _sRecordCount = ++_sRecordCount; //кол-во записей
                        _sRecordLastID = Convert.ToInt32(_SQLDr["LAST_ID"].ToString());
                    }                    
                }

                _DBF.WriteHeader();
                if (_DBF != null)
                    _DBF.Close();
                _SQLDr.Close();
                _SQLCommand.Dispose();
            }
            catch
            {
                _REC = null;
                if (_DBF != null)
                    _DBF.Close();
                _SQLDr = null;
                _SQLCommand = null;
             //   DisconnectSQL();
                throw;
            }
            finally
            {
              ///  MessageBox.Show(_sBulkSourceScript  + " complite ");

                _REC = null;
                if (_DBF != null)
                    _DBF.Close();
                _SQLDr = null;
                _SQLCommand = null;
            }
            return _result;
        }
        public bool BulkExecuteToDBF(bool _Batching = false)
        {
            Boolean _result = false;
          //  byte[] _bytes;

            Int32 _recordCount = 0;
            Int32 _recordLastID = -1;

            try
            {
                if (_Batching)  ////порционная выгрузка циклом
                {
                    do //выполняем хотябы один раз
                    {
                        _recordCount = 0; //сюда вернется кол-во записей
                        _result = BulkExecuteToDBF(_sBulkSourceFileORScript, _Batching, ref _recordLastID, ref _recordCount);  //будем циклить и в каждой итерации передавать Последний ИД записи и 
                    }
                    while (_recordCount == _sBulkBatchSize);  //выполняем до тех пор, пока размер порции = размеру возвращаемых записей
                }
                else  //// обычная Полная выгрузка
                {
                    _result = BulkExecuteToDBF(_sBulkSourceFileORScript, _Batching, ref _recordLastID, ref _recordCount);
                }

              /*  _bytes = File.ReadAllBytes(_sBulkDestinationFileORTable);
                _bytes[29] = (byte)38;
                File.WriteAllBytes(_sBulkDestinationFileORTable, _bytes);*/
                _result = true;
            }
            catch
            {
              //  _bytes = null;
              //  DisconnectSQL();
                throw;
            }
            finally
            {
             //   _bytes = null;
            //    DisconnectSQL();
            }
            return _result;
        }

    }
}
