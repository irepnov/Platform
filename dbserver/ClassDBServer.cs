using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Windows.Forms;
using System.ComponentModel;
using System.Xml;
using System.IO;


namespace GGPlatform.DBServer
{
    public struct DBBulkInfo
    {
        public DateTime ProcessBegin;
        public DateTime ProcessEnd;
        public Int32 SourceRowsCount;
        public string ProcessError;
        public string ProcessMessage;
        public string LoadFile;
        public string LoadUser;
        public string LoadComp;
    }

    public enum EnumDBDataTypes
    {
        EDT_Bit = 1,
        EDT_Integer = 2,
        EDT_Decimal = 3,
        EDT_DateTime = 4,
        EDT_String = 5,
        EDT_Xml = 6
    } //типы данных для параметров в запросах

    public class DBSqlServer
    {
        public struct AboutConnection
        {
            public string ServerName;
            public string BaseName;
            public string SoftName;
            public string UserLogin;
            public string UserPassword;
            public string UserFam;
            public string UserIm;
            public string UserOtch;
            public string UserShortFIO;
            public string UserFullFIO;
            public string UserPost;
            public string UserSection;
            public Boolean UserIsAdmin;
            public Int32 SessionID;
            public Boolean UserIsAudit;
        } //храню информацию о соединении

        private class Parametr
        {
            private string _ParamName;
            private SqlDbType _ParamType;
            private int _ParamSize;
            private string _ParamValue;

            public Parametr(string inParamName, SqlDbType inParamType, int inParamSize, string inParamValue)
            {
                _ParamName = inParamName;
                _ParamType = inParamType;
                _ParamSize = inParamSize;
                _ParamValue = inParamValue;
            } //констуктор

            public string ParameterName { get { return _ParamName; } }
            public SqlDbType ParameterType { get { return _ParamType; } }
            public int ParameterSize { get { return _ParamSize; } }
            public string ParameterValue { get { return _ParamValue; } }
        } //использую для описания параметров запроса

        private string _sServername = "";
        private string _sBasename = "";
        private string _sUsername = "";
        private string _sPassword = "";
        private string _sConnectionString = "";
        private string _sSQLScript = "";
        private string _sSoftName = "";
        private IWin32Window _sMainHandle;
        private Boolean _sTrusted = false;
        private Int32 _AuditID, _SessionID;

        private ArrayList _ParameterList = new ArrayList();
        private SqlConnection _Connection = null;
        private SqlCommand _Command = null;
        private AboutConnection _InfoAboutConnection;
        private Exception _Exception = new Exception();

        private string _sourceError = "DBServer ";
     
        //констуркторы
        public DBSqlServer(string inServername, string inBasename, IWin32Window inMainHandle, string inSoftName)
        {
            _sServername = inServername;
            _sBasename = inBasename;
            _sMainHandle = inMainHandle;
            _sSoftName = inSoftName;
        }

        public DBSqlServer(string inConnectionString, IWin32Window inMainHandle, string inSoftName)
        {
            _sConnectionString = inConnectionString;
            _sMainHandle = inMainHandle;
            _sSoftName = inSoftName;
        }

        public void Disconnect()
        {
            try
            {
                if (_Connection != null)
                {
                    if (_Connection.State == ConnectionState.Open)
                    {
                        if (!String.IsNullOrEmpty(_InfoAboutConnection.UserLogin))
                        {
                            //логирую выход
                            _Command.Parameters.Clear();
                            _Command.CommandText = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 11, @Login = @pLogin, @CommandText = @pCommandText, @Status = 1, @SessionID = @pSessionID, @DateEnd = @pDateEnd";
                            _Command.Parameters.Add("@pLogin", SqlDbType.NVarChar);
                            _Command.Parameters["@pLogin"].Value = _InfoAboutConnection.UserLogin;
                            _Command.Parameters.Add("@pCommandText", SqlDbType.NVarChar);
                            _Command.Parameters["@pCommandText"].Value = "Выход из программы";
                            _Command.Parameters.Add("@pDateEnd", SqlDbType.DateTime);
                            _Command.Parameters["@pDateEnd"].Value = DateTime.Now;
                            _Command.Parameters.Add("@pSessionID", SqlDbType.Int);
                            _Command.Parameters["@pSessionID"].Value = _SessionID;
                            _AuditID = Convert.ToInt32(_Command.ExecuteScalar().ToString());
                        }

                       _Connection.Close();
                    }
                } 
            }
            catch (Exception ex)
            {
                /*MessageBox.Show(_sMainHandle,
                                "Ошибка отсоединения от БД: \n" + ex.Message,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод Disconnect]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + "Ошибка отсоединения от БД: \n" + ex.Message);
            }
        }

        ~DBSqlServer()
        {
            try
            {
                if (_Command != null)
                {
                   _Command.Dispose();
                   _Command = null;
                }
                _ParameterList = null;
                _Exception = null;
                if (_Connection != null)
                {
                    if (_Connection.State == ConnectionState.Open)
                        _Connection.Close();
                    _Connection.Dispose();
                    _Connection = null;
                }
            }
            catch (Exception ex)
            {
               /* MessageBox.Show(_sMainHandle,
                                "Ошибка деструктора: \n" + ex.Message,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод ~DBSqlServer]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
               //// throw new Exception(_sourceError + "Ошибка деструктора: \n" + ex.Message);
            }
        } //деструктор

        private void CreateConnectionString()
        {
            _sConnectionString = "Data Source=" + _sServername + ";";
            if (_sTrusted)
            {
                _sConnectionString = _sConnectionString + "Persist Security Info=False;Integrated Security=SSPI;";
            }
            else
            {
                _sConnectionString = _sConnectionString + "Persist Security Info=True;" +
                                     "User ID=" + _sUsername + ";" +
                                     "Password=" + _sPassword + ";";
            }
            _sConnectionString = _sConnectionString + "Initial Catalog=" + _sBasename;
        } //собираем строку подключения

        private void InternalConnect()
        {
            if (_sConnectionString == "")
            {
                CreateConnectionString();
            }
            _Connection = new SqlConnection(_sConnectionString);
            try
            {
                if (_Connection.State != ConnectionState.Open)
                    _Connection.Open();

                //один раз инициализирую объект Комманд
                _Command = _Connection.CreateCommand();
                _Command.CommandText = "";
                _Command.CommandTimeout = 0;
                _Command.CommandType = CommandType.Text;
                //



                if (_Connection.State == ConnectionState.Open)
                {

                    _Command.Parameters.Clear();
                    _Command.CommandText = "IF (OBJECT_ID(N'GGPlatform.usp_AuditManager') IS NOT NULL) and (OBJECT_ID(N'GGPlatform.Users') IS NOT NULL) begin   select 1 as Ex  end";
                    if (_Command.ExecuteNonQuery() > 1)
                    {
                        CreateInfoAboutConnection(); //соединение прошло, читаю инфу о пользователе и т.п...
                    }

                    if (!String.IsNullOrEmpty(_InfoAboutConnection.UserLogin))
                    {
                        //логирую вход
                        _Command.Parameters.Clear();
                        _Command.CommandText = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 11, @Login = @pLogin, @CommandText = @pCommandText, @Status = 1, @DateEnd = @pDateEnd";
                        _Command.Parameters.Add("@pLogin", SqlDbType.NVarChar);
                        _Command.Parameters["@pLogin"].Value = _InfoAboutConnection.UserLogin;
                        _Command.Parameters.Add("@pCommandText", SqlDbType.NVarChar);
                        _Command.Parameters["@pCommandText"].Value = "Запуск программы";
                        _Command.Parameters.Add("@pDateEnd", SqlDbType.DateTime);
                        _Command.Parameters["@pDateEnd"].Value = DateTime.Now;
                        _SessionID = Convert.ToInt32(_Command.ExecuteScalar().ToString());
                        _InfoAboutConnection.SessionID = _SessionID; //ИД сессии, что бы потом использовать ее в плагинах
                    } 
                  
                }
            }
            catch (Exception ex)
            {
                _sConnectionString = "";
                    /*MessageBox.Show(_sMainHandle,
                                    "Ошибка подключения к базе данных (" + _sBasename + ") сервера (" + _sServername + "): \n" + ex.Message,
                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод InternalConnect]",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + "Ошибка подключения к базе данных (" + _sBasename + ") сервера (" + _sServername + "): \n" + ex.Message);
            }
        } //внутреннее соединение

        public void Connect(string inUsername, string inPassword, Boolean inTrusted)
        {
            if (_Connection != null)
            {
                if (_Connection.State == ConnectionState.Open)
                    return;
            } 

            _sUsername = inUsername;
            _sPassword = inPassword;
            _sTrusted = inTrusted;
            InternalConnect();
        } //инициализация соединения через параметры

        public void ConnectVisual()
        {
            if (_Connection != null)
            {
                if (_Connection.State == ConnectionState.Open)
                    return;
            } 

            frmDlgConnect fmDlgConnect = new frmDlgConnect(_sMainHandle, _sServername, _sBasename, _sSoftName);
            if (fmDlgConnect.ShowDialog(_sMainHandle) == DialogResult.OK)
            {
                _sUsername = fmDlgConnect.Username;
                _sPassword = fmDlgConnect.Password;
                _sBasename = fmDlgConnect.Basename;
                _sServername = fmDlgConnect.Servername;
                _sTrusted = fmDlgConnect.Trusted;          
                InternalConnect();
            }
            fmDlgConnect.Dispose();
            fmDlgConnect = null;
        } //инициализация соединения через экранную форму

        private void CreateInfoAboutConnection()
        {
            _sSQLScript = "SELECT ID,Login,Fam,Im,Otch,PostRef,SectionRef,IsWindowsUser,IsAdmin,IsDropped,IsAudit FROM GGPlatform.[Users] WHERE Login=suser_sname()";
            //AddParameter("@pLogin", EnumDBDataTypes.EDT_String, _sUsername);
            SqlDataReader UserInfo = ExecuteReader();

            _InfoAboutConnection = new AboutConnection();
            _InfoAboutConnection.BaseName = _sBasename;
            _InfoAboutConnection.ServerName = _sServername;
            _InfoAboutConnection.SoftName = _sSoftName;
            
            _InfoAboutConnection.UserPassword = _sPassword;            
            while (UserInfo.Read())
            {
                _InfoAboutConnection.UserLogin = UserInfo["Login"].ToString();
                _InfoAboutConnection.UserFam = UserInfo["Fam"].ToString();
                _InfoAboutConnection.UserIm = UserInfo["Im"].ToString();
                _InfoAboutConnection.UserOtch = UserInfo["Otch"].ToString();
                //учесть пусто !!
                if ((UserInfo["Im"].ToString() != "") && (UserInfo["Otch"].ToString() != ""))
                    _InfoAboutConnection.UserShortFIO = UserInfo["Fam"].ToString() + " " + UserInfo["Im"].ToString().Substring(1, 1).ToUpper() + "." + UserInfo["Otch"].ToString().Substring(1, 1).ToUpper() + ".";
                if ((UserInfo["Im"].ToString() != "") && (UserInfo["Otch"].ToString() == ""))
                    _InfoAboutConnection.UserShortFIO = UserInfo["Fam"].ToString() + " " + UserInfo["Im"].ToString().Substring(1, 1).ToUpper() + ".";
                if ((UserInfo["Im"].ToString() == "") && (UserInfo["Otch"].ToString() != ""))
                    _InfoAboutConnection.UserShortFIO = UserInfo["Fam"].ToString() + " " + UserInfo["Otch"].ToString().Substring(1, 1).ToUpper() + ".";

                _InfoAboutConnection.UserFullFIO = UserInfo["Fam"].ToString() + " " + UserInfo["Im"].ToString() + UserInfo["Otch"].ToString();
                _InfoAboutConnection.UserPost = UserInfo["PostRef"].ToString();
                _InfoAboutConnection.UserSection = UserInfo["SectionRef"].ToString();

                _InfoAboutConnection.UserIsAdmin = false;
                try
                {
                    _InfoAboutConnection.UserIsAdmin = (Boolean)UserInfo["IsAdmin"];
                }                
                catch
                { }
                _InfoAboutConnection.UserIsAudit = false;
                try
                {
                    _InfoAboutConnection.UserIsAudit = (Boolean)UserInfo["IsAudit"];
                }
                catch
                { }
            }
            UserInfo.Close();

            if (String.IsNullOrEmpty(_InfoAboutConnection.UserLogin))
                throw new Exception("Пользователь не определен в системе");
        } //заполняю структуру, которая хранит информацию о текущем соединении: юзер, сервер и т.п....

        public void AddParameter(string inParamName, int inParamType, string inParamValue)
        {
            if (inParamValue == null) inParamValue = "";
            try
            {
                SqlDbType sDBParamType = SqlDbType.Int; //фигачу по умолчанию
                int sDBParamSize = 0;
                switch (inParamType)
                {
                    case 1:
                        sDBParamType = SqlDbType.Bit;
                        sDBParamSize = sizeof(Boolean);
                        break;
                    case 2:
                        sDBParamType = SqlDbType.BigInt;
                        sDBParamSize = sizeof(Int64);
                        break;                       
                    case 3:
                        sDBParamType = SqlDbType.Decimal;
                        sDBParamSize = sizeof(Decimal);
                        inParamValue.Replace(",", ".").Replace(" ", "");
                        Decimal trydec = 0;
                        try { Decimal.TryParse(inParamValue, out trydec); }
                        catch
                        {
                            trydec = 0;
                            /*MessageBox.Show(_sMainHandle,
                                            "Недопустимое значение параметра, ошибка преобразования (" + inParamName + ")",
                                            "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;*/
                            throw new Exception(_sourceError + "Недопустимое значение параметра, ошибка преобразования (" + inParamName + ")");
                        }
                        break;                 
                    case 4:
                        sDBParamType = SqlDbType.DateTime;
                        sDBParamSize = 8;
                        break;
                    case 5:
                        sDBParamType = SqlDbType.NVarChar;
                        sDBParamSize = inParamValue.Length;
                        break;
                    case 6:
                        sDBParamType = SqlDbType.Xml;
                        sDBParamSize = 0;
                        break;
                    default:
                        /* MessageBox.Show(_sMainHandle,
                                        "Неизвестный тип параметра (" + inParamName + ") к запросу",
                                        "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                        throw new Exception(_sourceError + "Неизвестный тип параметра (" + inParamName + ") к запросу");
                    // break;
                }

                Parametr Param = new Parametr(inParamName, sDBParamType, sDBParamSize, inParamValue);
                _ParameterList.Add(Param);
                Param = null;
            }
            catch (Exception ex)
            {
                /* MessageBox.Show(_sMainHandle,
                                "Ошибка добавления параметра (" + inParamName + ") к запросу \n" + ex.Message,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + "Ошибка добавления параметра (" + inParamName + ") к запросу \n" + ex.Message);
            }
        } //добавление параметра к запросу 

        public void AddParameter(string inParamName, EnumDBDataTypes inParamType, string inParamValue)
        {
            if (inParamValue == null) inParamValue = "";
            try
            {
                SqlDbType sDBParamType = SqlDbType.Int; //фигачу по умолчанию
                int sDBParamSize = 0;
                switch (inParamType)
                {
                    case EnumDBDataTypes.EDT_Bit:
                        sDBParamType = SqlDbType.Bit;
                        sDBParamSize = sizeof(Boolean);
                        break;
                    case EnumDBDataTypes.EDT_DateTime:
                        sDBParamType = SqlDbType.DateTime;
                        sDBParamSize = 8;
                        break;
                    case EnumDBDataTypes.EDT_Decimal:
                        sDBParamType = SqlDbType.Decimal;
                        sDBParamSize = sizeof(Decimal);
                        inParamValue.Replace(",", ".").Replace(" ", "");
                        Decimal trydec = 0;
                        try { Decimal.TryParse(inParamValue, out trydec); }
                        catch
                        {
                            trydec = 0;
                            /*MessageBox.Show(_sMainHandle,
                                            "Недопустимое значение параметра, ошибка преобразования (" + inParamName + ")",
                                            "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;*/
                            throw new Exception(_sourceError + "Недопустимое значение параметра, ошибка преобразования (" + inParamName + ")");
                        }
                        break;
                    case EnumDBDataTypes.EDT_Integer:
                        sDBParamType = SqlDbType.BigInt;
                        sDBParamSize = sizeof(Int64);
                        break;
                    case EnumDBDataTypes.EDT_String:
                        sDBParamType = SqlDbType.NVarChar;
                        sDBParamSize = inParamValue.Length;
                        break;
                    case EnumDBDataTypes.EDT_Xml:
                        sDBParamType = SqlDbType.Xml;
                       // sDBParamSize = 0;
                        break;
                    default:
                       /* MessageBox.Show(_sMainHandle,
                                        "Неизвестный тип параметра (" + inParamName + ") к запросу",
                                        "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                        throw new Exception(_sourceError + "Неизвестный тип параметра (" + inParamName + ") к запросу");
                       // break;
                }

                Parametr Param = new Parametr(inParamName, sDBParamType, sDBParamSize, inParamValue);
                _ParameterList.Add(Param);
                Param = null;
            }
            catch (Exception ex)
            {
               /* MessageBox.Show(_sMainHandle,
                                "Ошибка добавления параметра (" + inParamName + ") к запросу \n" + ex.Message,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод AddParameter]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + "Ошибка добавления параметра (" + inParamName + ") к запросу \n" + ex.Message);
            }
        } //добавление параметра к запросу 

        private void SetParameters()
        {
            try
            {
                Parametr Param = null;
                IEnumerator Enumerator = _ParameterList.GetEnumerator();
                _Command.Parameters.Clear();
                while (Enumerator.MoveNext())
                {
                    Param = (Parametr)Enumerator.Current;
                    SqlParameter _Parameter = new SqlParameter();

                    _Parameter.ParameterName = Param.ParameterName;                   
                    _Parameter.SqlDbType = Param.ParameterType;
                    switch (Param.ParameterType)
                    {
                        case SqlDbType.BigInt:
                            _Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (Param.ParameterValue != "") ? Convert.ToInt64(Param.ParameterValue) : (object)DBNull.Value;
                            break;
                        case SqlDbType.DateTime:
                            _Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (Param.ParameterValue != "") ? Convert.ToDateTime(Param.ParameterValue) : (object)DBNull.Value;
                            break;
                        case SqlDbType.Bit:
                            _Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (object)DBNull.Value;
                            if (Param.ParameterValue != "")
                            {                              
                                if ((Param.ParameterValue == "0") || (Param.ParameterValue.ToUpper() == "FALSE") || (Param.ParameterValue.ToUpper() == "НЕТ")) 
                                   _Parameter.Value = false;

                                if ((Param.ParameterValue == "1") || (Param.ParameterValue.ToUpper() == "TRUE") || (Param.ParameterValue.ToUpper() == "ДА"))
                                    _Parameter.Value = true;
                            }
                            else _Parameter.Value = (object)DBNull.Value;
                            break;
                        case SqlDbType.Decimal:
                            _Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (Param.ParameterValue != "") ? Convert.ToDecimal(Param.ParameterValue) : (object)DBNull.Value;
                            break;
                        case SqlDbType.NVarChar:
                            _Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (Param.ParameterValue != "") ? Param.ParameterValue : (object)DBNull.Value;
                            break;
                        case SqlDbType.Xml:
                            //_Parameter.Size = Param.ParameterSize;
                            _Parameter.Value = (Param.ParameterValue != "") ? Param.ParameterValue : (object)DBNull.Value;
                            break;
                    }
                    _Command.Parameters.Add(_Parameter);
                    _Parameter = null;
                }
                _ParameterList.Clear();
                Enumerator = null;
            }
            catch (Exception ex)
            {
                /*if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                                      "Ошибка при назначении параметров к запросу \n" + ex.Message,
                                                                      "Ошибка [модуль DBServer, класс DBSqlServer, метод SetParameters]",
                                                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                    "Ошибка при назначении параметров к запросу \n" + ex.Message,
                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод SetParameters]",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + "Ошибка при назначении параметров к запросу \n" + ex.Message);
            }
        } //добавлем параметры к объекту Комманд

        public string SQLScript
        {
            get { return _sSQLScript; }
            set
            {
                _Command.Parameters.Clear(); 
                _sSQLScript = value; 
            }
        } //текст SQL запроса к БД: чтение, запись

        public AboutConnection InfoAboutConnection
        {
            get { return _InfoAboutConnection; }
        } //вывожу информацию о подключении

        public SqlConnection Connection
        {
            get
            {
                if (_Connection != null)
                {
                    if (_Connection.State == ConnectionState.Open)
                        return _Connection;
                    else
                        return null;
                }
                else
                    return null;  
            } 
        } //возвращает объект Коннекшион

        public SqlCommand Command
        {
            get 
            {
                if (_Connection != null)
                {
                    if (_Connection.State == ConnectionState.Open)
                        return _Command;
                    else
                        return null;
                }
                else
                    return null;
            } 
        } //возвращает объект Комманд

        private void AuditBegin()
        {
            /*try
            {
                //логирую начало выполнения
                _Command.Parameters.Clear();
                _Command.CommandText = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 11, @CommandText = @pCommandText, @Status = 0, @SessionID = @pSessionID";
                _Command.Parameters.Add("@pCommandText", SqlDbType.NVarChar);
                _Command.Parameters["@pCommandText"].Value = _sSQLScript;
                _Command.Parameters.Add("@pSessionID", SqlDbType.Int);
                _Command.Parameters["@pSessionID"].Value = _SessionID;
                _Command.ExecuteNonQuery();
                _Command.Parameters.Clear();
            }
            catch
            { }*/
        }

        private void AuditEndSucceeded()
        {
            /*try
            {
                //логирую начало выполнения
                _Command.Parameters.Clear();
                _Command.CommandText = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 12, @ObjectID = @pObjectID";
                _Command.Parameters.Add("@pObjectID", SqlDbType.Int);
                _Command.Parameters["@pObjectID"].Value = _AuditID;                          
                _Command.ExecuteNonQuery();
                _Command.Parameters.Clear();    
            }
            catch
            { }*/
        }

        private void AuditEndFailed()
        {
            /*try
            {
                //логирую начало выполнения
                _Command.Parameters.Clear();
                _Command.CommandText = "exec [GGPlatform].[usp_AuditManager] @TypeQuery = 13, @ObjectID = @pObjectID";
                _Command.Parameters.Add("@pObjectID", SqlDbType.Int);
                _Command.Parameters["@pObjectID"].Value = _AuditID;                          
                _Command.ExecuteNonQuery();
                _Command.Parameters.Clear(); 
            }
            catch
            { }
            */
        }


        public int ExecuteNonQuery()
        {
            if (_Connection.State != ConnectionState.Open)
            {
               /* if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle, "Соединение закрыто", "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteNonQuery]",
                                                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle, "Соединение закрыто", "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteNonQuery]",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;*/
                throw new Exception(_sourceError + "Соединение с БД закрыто");
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            int tmp = -1;
            try
            {                
                tmp = _Command.ExecuteNonQuery();
                AuditEndSucceeded();
                return tmp;
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + ex.Message 
                                    + "\n источник: " + SourceException 
                                    + "\n строка №: " + ex.LineNumber);
            }
        } //выполняет команды Delete Update exec
        
        private int _ExecuteNonQuery = -1;
        public int ExecuteNonQueryVisual(string inVisualMessage)
        {
            BackgroundWorker backgroundWorkerExecuteNonQuery = new BackgroundWorker();
            backgroundWorkerExecuteNonQuery.DoWork += new DoWorkEventHandler(backgroundWorkerExecuteNonQuery_DoWork);
            backgroundWorkerExecuteNonQuery.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
           
            _ExecuteNonQuery = -1;
            if (backgroundWorkerExecuteNonQuery.IsBusy != true)
            {
                _fmProgress = new ShowProgressTask();
                _fmProgress.Message = inVisualMessage;
                backgroundWorkerExecuteNonQuery.RunWorkerAsync();
                if ((_fmProgress != null) && (backgroundWorkerExecuteNonQuery.IsBusy)) _fmProgress.ShowDialog();
                Application.DoEvents();
            }
            if (_Exception != null)
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + _Exception.Message);
            return _ExecuteNonQuery;
            //return 1;
        }// Визуальное заполнение таблицы  ExecuteNonQuery
        private void backgroundWorkerExecuteNonQuery_DoWork(object sender, DoWorkEventArgs e)
        {
            _ExecuteNonQuery = ExecuteNonQuery();
        }



        public string ExecuteScalar()
        {
         /*   if (_Connection.State != ConnectionState.Open)
            {
                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                                    "Соединение закрыто",
                                                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteScalar]",
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Соединение закрыто",
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteScalar]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            try
            {
                string tmp = "";
                tmp = _Command.ExecuteScalar().ToString();
                AuditEndSucceeded();
                return tmp;
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;

                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                          "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                                          "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteScalar]",
                                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteScalar]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }*/

            if (_Connection.State != ConnectionState.Open)
            {
                throw new Exception(_sourceError + "Соединение с БД закрыто");
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();            
            try
            {   
                string tmp = null;
                tmp = _Command.ExecuteScalar().ToString();
                AuditEndSucceeded();
                return tmp;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" 
                                    + ex.Message + "\n источник: " + SourceException 
                                    + "\n строка №: " + ex.LineNumber);
            }
        } //выполняет команды которые возвращают одно значение
        
        private string _ExecuteScalar = null;
        public string ExecuteScalarVisual(string inVisualMessage)
        {
            BackgroundWorker backgroundWorkerExecuteScalar = new BackgroundWorker();
            backgroundWorkerExecuteScalar.DoWork += new DoWorkEventHandler(backgroundWorkerExecuteScalar_DoWork);
            backgroundWorkerExecuteScalar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            _ExecuteScalar = null;
            if (backgroundWorkerExecuteScalar.IsBusy != true)
            {
                _fmProgress = new ShowProgressTask();
                _fmProgress.Message = inVisualMessage;
                backgroundWorkerExecuteScalar.RunWorkerAsync();
                if ((_fmProgress != null) && (backgroundWorkerExecuteScalar.IsBusy)) _fmProgress.ShowDialog();
                Application.DoEvents();
            }
            if (_Exception != null)
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + _Exception.Message);
            return _ExecuteScalar;
        }// Визуальное заполнение таблицы  ExecuteScalar
        private void backgroundWorkerExecuteScalar_DoWork(object sender, DoWorkEventArgs e)
        {
            _ExecuteScalar = ExecuteScalar();
        }



        public SqlDataReader ExecuteReader()
        {
            /*if (_Connection.State != ConnectionState.Open)
            {
                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                                    "Соединение закрыто",
                                                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteReader]",
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Соединение закрыто",
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteReader]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            try
            {
               // SqlDataReader tmp = null;
              //  tmp = _Command.ExecuteReader();
             //   AuditEndSucceeded();
                return _Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;

                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                          "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                                          "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteReader]",
                                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод ExecuteReader]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }*/
            if (_Connection.State != ConnectionState.Open)
            {
                throw new Exception(_sourceError + "Соединение с БД закрыто");
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            try
            {
                return _Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber);
            }
        } //выполняет команды которые возвращают recordset
        
        private SqlDataReader _ExecuteReader = null;
        public SqlDataReader ExecuteReaderVisual(string inVisualMessage)
        {
            BackgroundWorker backgroundWorkerExecuteReader = new BackgroundWorker();
            backgroundWorkerExecuteReader.DoWork += new DoWorkEventHandler(backgroundWorkerExecuteReader_DoWork);
            backgroundWorkerExecuteReader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            _ExecuteReader = null;
            if (backgroundWorkerExecuteReader.IsBusy != true)
            {
                _fmProgress = new ShowProgressTask();
                _fmProgress.Message = inVisualMessage;
                backgroundWorkerExecuteReader.RunWorkerAsync();
                if ((_fmProgress != null) && (backgroundWorkerExecuteReader.IsBusy)) _fmProgress.ShowDialog();
                Application.DoEvents();
            }
            if (_Exception != null)
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + _Exception.Message);
            return _ExecuteReader;
        }// Визуальное заполнение таблицы  ExecuteReader
        private void backgroundWorkerExecuteReader_DoWork(object sender, DoWorkEventArgs e)
        {
            _ExecuteReader = ExecuteReader();
        }




        public DataSet FillDataSet()
        {
            /*if (_Connection.State != ConnectionState.Open)
            {
                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                                    "Соединение закрыто",
                                                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод FillDataSet]",
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Соединение закрыто",
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод FillDataSet]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if (_Command.CommandText == "")
            {
                return null;
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command);
            try
            {
                DataSet _DataSet = new DataSet();           
                _DataAdapter.Fill(_DataSet);
                _DataAdapter.Dispose();
                AuditEndSucceeded();
                return _DataSet;
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;

                if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                          "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                                          "Ошибка [модуль DBServer, класс DBSqlServer, метод SQLFillDataSet]",
                                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод SQLFillDataSet]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }*/
            if (_Connection.State != ConnectionState.Open)
            {
                throw new Exception(_sourceError + "Соединение с БД закрыто");
            }
            if (_Command.CommandText == "")
            {
                return null;
            }
            AuditBegin();
            _Command.CommandText = _sSQLScript;
            SetParameters();
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(_Command);
           // _DataAdapter.UpdateCommand
            
            try
            {                
                DataSet _DataSet = new DataSet();
                _DataAdapter.Fill(_DataSet);
                _DataAdapter.Dispose();
                AuditEndSucceeded();
                return _DataSet;
            }
            catch (SqlException ex)
            {
                AuditEndFailed();
                string SourceException = "";
                if (ex.Procedure.Length != 0)
                    SourceException = ex.Procedure;
                else
                    SourceException = _sSQLScript;
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + ex.Message + "\n источник: " + SourceException + "\n строка №: " + ex.LineNumber);
            }
        } //Наполняем DataSet

        private ShowProgressTask _fmProgress;
        private DataSet _FillDataSet = new DataSet();
        public DataSet FillDataSetVisual(string inVisualMessage)
        {
            BackgroundWorker backgroundWorkerFillDataSet = new BackgroundWorker();
            backgroundWorkerFillDataSet.DoWork += new DoWorkEventHandler(backgroundWorkerFillDataSet_DoWork);
            backgroundWorkerFillDataSet.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            _FillDataSet = null;
            if (backgroundWorkerFillDataSet.IsBusy != true)
            {
                _fmProgress = new ShowProgressTask();
                _fmProgress.Message = inVisualMessage;
                backgroundWorkerFillDataSet.RunWorkerAsync();
                if ((_fmProgress != null) && (backgroundWorkerFillDataSet.IsBusy)) _fmProgress.ShowDialog();
                Application.DoEvents();
            }
           /* while (backgroundWorkerFillDataSet.IsBusy)
            {
                _fmProgress.progressBar1.Increment(1);
                Application.DoEvents();
            }  */
            if (_Exception != null)
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + _Exception.Message);
            return _FillDataSet;
        }// Визуальное заполнение таблицы  DataSet
        private void backgroundWorkerFillDataSet_DoWork(object sender, DoWorkEventArgs e)
        {
            _FillDataSet = FillDataSet();
        }



        public ADODB.Recordset GetADODBRecordset()
        {
            return ConvertToRecordset(FillDataSet().Tables[0]);
        }
        public ADODB.Recordset GetADODBRecordsetVisual(string inVisualMessage)
        {
            return ConvertToRecordset(FillDataSetVisual(inVisualMessage).Tables[0]);
        }



        private ADODB.Recordset ConvertToRecordset(DataTable inTable)
        {
            ADODB.Recordset result = new ADODB.Recordset();
            result.CursorLocation = ADODB.CursorLocationEnum.adUseClient;

            ADODB.Fields resultFields = result.Fields;
            System.Data.DataColumnCollection inColumns = inTable.Columns;

            foreach (DataColumn inColumn in inColumns)
            {
                resultFields.Append(inColumn.ColumnName
                    , TranslateType(inColumn.DataType)
                    , inColumn.MaxLength
                    , inColumn.AllowDBNull ? ADODB.FieldAttributeEnum.adFldIsNullable :
                                             ADODB.FieldAttributeEnum.adFldUnspecified
                    , null);
            }

            result.Open(System.Reflection.Missing.Value
                    , System.Reflection.Missing.Value
                    , ADODB.CursorTypeEnum.adOpenStatic
                    , ADODB.LockTypeEnum.adLockOptimistic, 0);

            foreach (DataRow dr in inTable.Rows)
            {
                result.AddNew(System.Reflection.Missing.Value,
                              System.Reflection.Missing.Value);

                for (int columnIndex = 0; columnIndex < inColumns.Count; columnIndex++)
                {
                    resultFields[columnIndex].Value = dr[columnIndex];
                }
            }

            if (result != null && result.RecordCount > 0)
            {
                result.MoveFirst();
            }
                
            return result;
        }
        private ADODB.DataTypeEnum TranslateType(Type columnType)
        {
            switch (columnType.UnderlyingSystemType.ToString())
            {
                case "System.Boolean":
                    return ADODB.DataTypeEnum.adBoolean;
                case "System.Byte":
                    return ADODB.DataTypeEnum.adUnsignedTinyInt;
                case "System.Char":
                    return ADODB.DataTypeEnum.adChar;
                case "System.DateTime":
                    return ADODB.DataTypeEnum.adDate;
                case "System.Decimal":
                    return ADODB.DataTypeEnum.adCurrency;
                case "System.Double":
                    return ADODB.DataTypeEnum.adDouble;
                case "System.Int16":
                    return ADODB.DataTypeEnum.adSmallInt;
                case "System.Int32":
                    return ADODB.DataTypeEnum.adInteger;
                case "System.Int64":
                    return ADODB.DataTypeEnum.adBigInt;
                case "System.SByte":
                    return ADODB.DataTypeEnum.adTinyInt;
                case "System.Single":
                    return ADODB.DataTypeEnum.adSingle;
                case "System.UInt16":
                    return ADODB.DataTypeEnum.adUnsignedSmallInt;
                case "System.UInt32":
                    return ADODB.DataTypeEnum.adUnsignedInt;
                case "System.UInt64":
                    return ADODB.DataTypeEnum.adUnsignedBigInt;
                case "System.String":
                default:
                    return ADODB.DataTypeEnum.adVarChar;
            }
        }


        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            _fmProgress.Close();
            _fmProgress.Dispose();
            _fmProgress = null;

            _Exception = null;
            _Exception = e.Error;
        }






        #region BULK
        /// <summary>
        /// булк
        /// </summary>
        private string _sBulkSourceFileName;
        private string _sBulkDestinationTable;
        private XmlDocument _sBulkMapperFile;
        private string _sBulkMapperKey;
        private Int32 _sBulkBatchSize = 100000;
        public string BulkSourceFileName
        {
            get { return _sBulkSourceFileName; }
            set { _sBulkSourceFileName = value; }
        }
        public string BulkDestinationTable
        {
            get { return _sBulkDestinationTable; }
            set { _sBulkDestinationTable = value; }
        }
        public string BulkMapperKey
        {
            get { return _sBulkMapperKey; }
            set { _sBulkMapperKey = value; }
        }
        public Int32 BulkBatchSize
        {
            get { return _sBulkBatchSize; }
            set { _sBulkBatchSize = value; }
        }

        public DBBulkInfo BulkExecuteFromDBF()
        {
            DBBulkInfo _result = new DBBulkInfo();
            XmlNodeList _xnListKeys;

            Int32 _rows = 0;

            string _conStringDBF = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(_sBulkSourceFileName) + ";Extended Properties=dBASE IV;User ID=;Password=;";
            OleDbConnection _DBFConnect = new OleDbConnection(_conStringDBF);
            OleDbCommand _DBFSelect = new OleDbCommand();
            _DBFSelect.Connection = _DBFConnect;
            _DBFSelect.CommandTimeout = 0;
            OleDbDataReader _DBFReader;

            SqlBulkCopy _BulkCopy;

            try
            {
                _result.ProcessBegin = DateTime.Now;
                if (!File.Exists(_sBulkSourceFileName)) throw new Exception(_sourceError + "Файл источник " + _sBulkSourceFileName + " не найден");
                if (string.IsNullOrEmpty(_sBulkDestinationTable)) throw new Exception(_sourceError + "Таблица назначения " + _sBulkDestinationTable + " не указана");

                string _sBulkMapperFileName = Application.StartupPath + "\\BulkMapper.xml";
                if (!File.Exists(_sBulkMapperFileName)) throw new Exception(_sourceError + "Конфигурационный файл сопоставления " + _sBulkMapperFileName + " не найден");
                _sBulkMapperFile = new XmlDocument();
                _sBulkMapperFile.Load(_sBulkMapperFileName);
                _xnListKeys = _sBulkMapperFile.SelectNodes("//MapperKey[@key='" + _sBulkMapperKey + "']");
                if (_xnListKeys.Count == 0) throw new Exception(_sourceError + "Не найден ключ сопоставления " + _sBulkMapperKey);

                string _SourceFields = "";
                foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes)
                {
                    _SourceFields = _SourceFields + xn.Attributes["source"].Value + ", ";
                    // _TargetFields = _SourceFields + xn.Attributes["target"].Value + ", ";
                }

                _SourceFields = _SourceFields.Replace("_LOADFILE_", "'" + Path.GetFileName(_sBulkSourceFileName) + "' as _LOADFILE_");  ///таких полей нет в DBF, их заменяю самостоятельно
                _SourceFields = _SourceFields.Replace("_LOADCOMP_", "'" + Environment.MachineName + "' as _LOADCOMP_");
                _SourceFields = _SourceFields.Replace("_LOADUSER_", "'" + _InfoAboutConnection.UserLogin + "' as _LOADUSER_");
                _SourceFields = _SourceFields.Trim();
                if (_SourceFields.Substring(_SourceFields.Length - 1, 1) == ",") //последняя запятая
                {
                    _SourceFields = _SourceFields.Substring(0, _SourceFields.Length - 1);
                }

                ///соединение с ДБФ  
                _DBFConnect.Open();

                _DBFSelect.CommandText = @"select count(*) as RowsCount from " + Path.GetFileName(_sBulkSourceFileName); ///кол-во записей из источника
                _rows = (Int32)_DBFSelect.ExecuteScalar();
                _result.SourceRowsCount = _rows;

                _DBFSelect.CommandText = @"select " + _SourceFields + " from " + Path.GetFileName(_sBulkSourceFileName);
                _DBFReader = _DBFSelect.ExecuteReader();

                _BulkCopy = new SqlBulkCopy(_Connection);
                _BulkCopy.DestinationTableName = _sBulkDestinationTable;
                _BulkCopy.BulkCopyTimeout = 0;
                _BulkCopy.BatchSize = _sBulkBatchSize;
                foreach (XmlNode xn in _xnListKeys.Item(0).ChildNodes) ///сопоставление
                {
                    _BulkCopy.ColumnMappings.Add(xn.Attributes["source"].Value, xn.Attributes["target"].Value);
                }
                _BulkCopy.WriteToServer(_DBFReader);

                _DBFReader.Close();
                _DBFReader.Dispose();

                _result.LoadComp = Environment.MachineName;
                _result.LoadFile = Path.GetFileName(_sBulkSourceFileName);
                _result.LoadUser = _InfoAboutConnection.UserLogin;
                _result.ProcessEnd = DateTime.Now;
                _result.ProcessMessage = "Сведения загружены в таблицу " + _sBulkDestinationTable +
                                         " (источник " + Path.GetFileName(_sBulkSourceFileName) + " - " + _rows.ToString() + " записей)." +
                                         " Время выполнения: " +
                                            ((_result.ProcessEnd - _result.ProcessBegin).Hours.ToString()).PadLeft(2, '0') + ":" +
                                            ((_result.ProcessEnd - _result.ProcessBegin).Minutes.ToString()).PadLeft(2, '0') + ":" +
                                            ((_result.ProcessEnd - _result.ProcessBegin).Seconds.ToString()).PadLeft(2, '0');
            }
            catch (Exception e)
            {
                _result.ProcessError = e.Message;
                /*if (((Form)(_sMainHandle)).InvokeRequired)
                    ((Form)(_sMainHandle)).Invoke(
                                                  new Action(() =>
                                                  {
                                                      MessageBox.Show(_sMainHandle,
                                                                    e.Message,
                                                                    "Ошибка [модуль DBServer, класс DBSqlServer, метод BulkExecute]",
                                                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                  })
                                                 );
                else
                    MessageBox.Show(_sMainHandle,
                                e.Message,
                                "Ошибка [модуль DBServer, класс DBSqlServer, метод BulkExecute]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception(_sourceError + e.Message);
            }
            finally
            {
                _DBFSelect.Dispose();
                _DBFConnect.Close();
                _DBFConnect.Dispose();
                _BulkCopy = null;
                
            }
            return _result;
        }


        /// <summary>
        /// визуальный Булк
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private DBBulkInfo _DBBulkInfo = new DBBulkInfo();
        public DBBulkInfo BulkExecuteFromDBFVisual(string inVisualMessage)
        {
            BackgroundWorker backgroundWorkerBulkExecute = new BackgroundWorker();
            backgroundWorkerBulkExecute.DoWork += new DoWorkEventHandler(backgroundWorkerBulkExecute_DoWork);
            backgroundWorkerBulkExecute.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            if (backgroundWorkerBulkExecute.IsBusy != true)
            {
                _fmProgress = new ShowProgressTask();
                _fmProgress.Message = inVisualMessage;
                backgroundWorkerBulkExecute.RunWorkerAsync();
                if ((_fmProgress != null) && (backgroundWorkerBulkExecute.IsBusy)) _fmProgress.ShowDialog();
                Application.DoEvents();
            }
            if (_Exception != null)
                throw new Exception(_sourceError + "Ошибка выполнения запроса: \n" + _Exception.Message);
            return _DBBulkInfo;
        }
        private void backgroundWorkerBulkExecute_DoWork(object sender, DoWorkEventArgs e)
        {
            _DBBulkInfo = BulkExecuteFromDBF();
        }        
        #endregion




    }


}
