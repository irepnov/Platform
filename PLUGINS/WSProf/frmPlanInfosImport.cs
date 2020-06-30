using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace WSProf
{
    public partial class frmPlanInfosImport : Form
    {

        private DBSqlServer _dbsql = null;
        private Int32 _ActionID = -1;
        private ExcelManager _excel = null;
        private string _templatename = "\\reports\\template\\";
        private string _mnth = "";
        private string _year = "";
        private string _sessionGuid = Guid.NewGuid().ToString();
        private string _WSSender;
        private string _WSUsername;
        private string _WSPassword;
        private string _WSUrl;



        public frmPlanInfosImport(object[] inParams)
        {
            InitializeComponent();

            _dbsql = (DBSqlServer)inParams[0];
            _WSSender = inParams[1].ToString();
            _WSUsername = inParams[2].ToString();
            _WSPassword = inParams[3].ToString();
            _WSUrl = inParams[4].ToString();

            _dbsql.SQLScript = "delete from [WSProf].[transferPlanInfos]";
            _dbsql.ExecuteNonQuery();
        }


        public static T DeserializeXmlToObject<T>(string data) where T : class
        {
            if ((data == null) || (data.Trim().Length == 0))
                return null;

            var xs = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(data))
                return (T)xs.Deserialize(sr);
        }

        public static string SerializeObjectToXml(Object data)
        {
            System.IO.StringWriter output = new System.IO.StringWriter();
            XmlSerializer xs = new XmlSerializer(data.GetType());
            xs.Serialize(output, data);
            return output.ToString();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Дополнительные списки|*.xls;*.xlsx";
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

                int _RowIndex = 10;

                string _kind, _code_mo, _a10_dct, _a11_dcs, _a12_dcn, _a13_smcd, 
											 _a15_pfio, _a16_pnm, _a17_pln, _a18_ps, _a19_pbd, 
											 _a20_pph, _a21_ps, _a22_pn, _a23_dt, _a24_sl, _a25_enp;

                _kind = _code_mo = _a10_dct = _a11_dcs = _a12_dcn = _a13_smcd = 
											 _a15_pfio = _a16_pnm = _a17_pln = _a18_ps = _a19_pbd = 
											 _a20_pph = _a21_ps = _a22_pn = _a23_dt = _a24_sl = _a25_enp = "";                

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _a10_dct = NullWithEmpty(_excel.GetValue("M" + _RowIndex.ToString()).Trim());
                    _a11_dcs = NullWithEmpty(_excel.GetValue("N" + _RowIndex.ToString()).Trim());
                    _a12_dcn = NullWithEmpty(_excel.GetValue("O" + _RowIndex.ToString()).Trim());
                    _a13_smcd = NullWithEmpty(_excel.GetValue("K" + _RowIndex.ToString()).Trim());
                    _a15_pfio = NullWithEmpty(_excel.GetValue("A" + _RowIndex.ToString()).Trim());
                    _a16_pnm = NullWithEmpty(_excel.GetValue("B" + _RowIndex.ToString()).Trim());
                    _a17_pln = NullWithEmpty(_excel.GetValue("C" + _RowIndex.ToString()).Trim());

                    _a18_ps = NullWithEmpty(_excel.GetValue("A" + _RowIndex.ToString()).Trim() == "Ж" ? "2" : "1");

                    _a19_pbd = NullWithEmpty(_excel.GetValue("D" + _RowIndex.ToString()).Trim());
                    _a20_pph = NullWithEmpty(_excel.GetValue("J" + _RowIndex.ToString()).Trim());
                    _a21_ps = NullWithEmpty(_excel.GetValue("H" + _RowIndex.ToString()).Trim());
                    _a22_pn = NullWithEmpty(_excel.GetValue("I" + _RowIndex.ToString()).Trim());
                    _a23_dt = NullWithEmpty(_excel.GetValue("G" + _RowIndex.ToString()).Trim());

                    _a24_sl = NullWithEmpty(_excel.GetValue("F" + _RowIndex.ToString()).Trim());
                    _a25_enp = NullWithEmpty(_excel.GetValue("L" + _RowIndex.ToString()).Trim());
                    _kind = NullWithEmpty(_excel.GetValue("P" + _RowIndex.ToString()).Trim());
                    _code_mo = NullWithEmpty(_excel.GetValue("Q" + _RowIndex.ToString()).Trim());

                    _dbsql.SQLScript = @"exec [WSProf].[spPlanInfos] @IDOper = 5, 
                                                @sessionGuid = @q1, @kind = @q2, 
                                                @code_mo = @q3, 
                                                @a10_dct = @q4, @a11_dcs = @q5, 
                                                @a12_dcn = @q6, @a13_smcd = @q7, 
											    @a15_pfio = @q8, @a16_pnm = @q9, 
                                                @a17_pln = @q10, @a18_ps = @q11, @a19_pbd = @q12, 
											    @a20_pph = @q13, @a21_ps = @q14, @a22_pn = @q15, 
                                                @a23_dt = @q16, @a24_sl = @q17, @a25_enp = @q18, 
                                                @mnth = @q19, @year = @q20";
                    _dbsql.AddParameter("@q1", EnumDBDataTypes.EDT_String, _sessionGuid);
                    _dbsql.AddParameter("@q2", EnumDBDataTypes.EDT_Integer, _kind);
                    _dbsql.AddParameter("@q3", EnumDBDataTypes.EDT_String, _code_mo);
                    _dbsql.AddParameter("@q4", EnumDBDataTypes.EDT_Integer, _a10_dct);
                    _dbsql.AddParameter("@q5", EnumDBDataTypes.EDT_String, _a11_dcs);
                    _dbsql.AddParameter("@q6", EnumDBDataTypes.EDT_String, _a12_dcn);
                    _dbsql.AddParameter("@q7", EnumDBDataTypes.EDT_String, _a13_smcd);
                    _dbsql.AddParameter("@q8", EnumDBDataTypes.EDT_String, _a15_pfio);
                    _dbsql.AddParameter("@q9", EnumDBDataTypes.EDT_String, _a16_pnm);
                    _dbsql.AddParameter("@q10", EnumDBDataTypes.EDT_String, _a17_pln);
                    _dbsql.AddParameter("@q11", EnumDBDataTypes.EDT_Integer, _a18_ps);
                    _dbsql.AddParameter("@q12", EnumDBDataTypes.EDT_DateTime, _a19_pbd);
                    _dbsql.AddParameter("@q13", EnumDBDataTypes.EDT_String, _a20_pph);
                    _dbsql.AddParameter("@q14", EnumDBDataTypes.EDT_String, _a21_ps);
                    _dbsql.AddParameter("@q15", EnumDBDataTypes.EDT_String, _a22_pn);
                    _dbsql.AddParameter("@q16", EnumDBDataTypes.EDT_Integer, _a23_dt);
                    _dbsql.AddParameter("@q17", EnumDBDataTypes.EDT_String, _a24_sl);
                    _dbsql.AddParameter("@q18", EnumDBDataTypes.EDT_String, _a25_enp);
                    _dbsql.AddParameter("@q19", EnumDBDataTypes.EDT_Integer, _mnth);
                    _dbsql.AddParameter("@q20", EnumDBDataTypes.EDT_Integer, _year);

                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 10).ToString());
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

        private bool LoadWS()
        {
            bool _result = false;
            try
            {
                lbProtocol2.Items.Add("обращение к Web-сервису ТФОМС...");

                String url = _WSUrl;
                EndpointAddress address = new EndpointAddress(url);
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.OpenTimeout = new TimeSpan(0, 1, 30);
                binding.CloseTimeout = new TimeSpan(0, 1, 30);
                binding.SendTimeout = new TimeSpan(0, 2, 0);
                binding.ReceiveTimeout = new TimeSpan(0, 3, 0);
                binding.MaxReceivedMessageSize = 2147483647;
                binding.MaxBufferPoolSize = 2147483647;
                XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
                readerQuotas.MaxArrayLength = 2147483647;
                readerQuotas.MaxStringContentLength = 2147483647;
                binding.ReaderQuotas = readerQuotas;

                ////_dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 3, @mnth = @m, @year = @y";
                ////_dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _m.ToString());
                ////_dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _y.ToString());
                ////if (_dbsql.ExecuteScalar() == "1")
                ////{
                ////    MessageBox.Show("За указанный отчетный период список граждан ранее загружен из ТФОМС КК и проинформирован\nИмпорт списка невозможен",
                ////                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ////    address = null;
                ////    binding = null;
                ////    return;
                ////}

                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                WS.DCExchangeSrv ws = new WS.DCExchangeSrvClient(binding, address);
                WS.getEvPlanListRequest request = new WS.getEvPlanListRequest();
                WS.getEvPlanListResponse response = null;
                int it = 1;
                string session = _sessionGuid;

                _dbsql.SQLScript = "delete from [WSProf].[transferPlanInfos] where sessionGuid = @guid";
                _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
                _dbsql.ExecuteNonQuery();
                int _col = 0;
                do
                {
                    request.username = _WSUsername;
                    request.password = _WSPassword;
                    request.sendercode = _WSSender;
                    request.mnth = Convert.ToInt16(_mnth);
                    request.year = Convert.ToInt16(_year); ;
                    request.page = it;
                    response = ws.getEvPlanList(request);
                    string returnXml = SerializeObjectToXml(response);
                    if (response.orderpack.evPlanList.Length > 0)
                    {
                        string _IDPackage = "";
                        _dbsql.SQLScript = @"insert into WSProf.Packages(packagesTypeID, pdate_send, pbody_send, perr_response, perrmes_response, pbody_response, sessionGuid) 
                                        values (2, @pdate, @pbody_send, @perr, @perrmes, @pbody_resp, @guid); 
                                        SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;";
                        _dbsql.AddParameter("@pdate", EnumDBDataTypes.EDT_DateTime, response.orderpack.p10_packinf.p10_pakagedate.ToString());
                        _dbsql.AddParameter("@pbody_send", EnumDBDataTypes.EDT_String, SerializeObjectToXml(request));
                        _dbsql.AddParameter("@perr", EnumDBDataTypes.EDT_String, response.orderpack.p10_packinf.p13_zerrpkg.ToString());
                        _dbsql.AddParameter("@perrmes", EnumDBDataTypes.EDT_String, response.orderpack.p10_packinf.p14_errmsg == null ? String.Empty : response.orderpack.p10_packinf.p14_errmsg.ToString());
                        _dbsql.AddParameter("@pbody_resp", EnumDBDataTypes.EDT_String, returnXml);
                        _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
                        _IDPackage = _dbsql.ExecuteScalar();

                        _dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 1, @sessionGuid = @guid, @IDObject = @idobj, @mnth = @m, @year = @y";
                        _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, session);
                        _dbsql.AddParameter("@idobj", EnumDBDataTypes.EDT_Integer, _IDPackage);
                        _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
                        _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
                        _dbsql.ExecuteNonQuery();
                        it++;
                    }
                    _col = _col + response.orderpack.evPlanList.Length;
                } while (response.orderpack.evPlanList.Length > 0);                
                response = null;
                request = null;
                lbProtocol2.Items.Add("загружено записей " + _col.ToString());

                lbProtocol2.Items.Add("обновление плановых показателей, утвержденных МЗ КК");
                string _need = "0";
                _dbsql.SQLScript = "exec WSProf.spPlanQtys @IDOper = 2";
                _need = _dbsql.ExecuteScalar();
                if (_need == "1")
                {
                    //также закачаю установленные квоты МЗ КК, если они еще не закачивались сегодян
                    WS.getEvPlanQtysResponse responseQ = null;
                    WS.getEvPlanQtysRequest requestQ = new WS.getEvPlanQtysRequest(_WSUsername, _WSPassword, _WSSender, Convert.ToInt16(_year));
                    responseQ = ws.getEvPlanQtys(requestQ);
                    string returnXmlQ = SerializeObjectToXml(responseQ);
                    if (responseQ.orderpack.evPlanQtys.Length > 0)
                    {
                        string _IDPackageQ = "";
                        _dbsql.SQLScript = @"insert into WSProf.Packages(packagesTypeID, pdate_send, pbody_send, perr_response, perrmes_response, pbody_response) 
                                            values (1, @pdate, @pbody_send, @perr, @perrmes, @pbody_resp); 
                                            SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;";
                        _dbsql.AddParameter("@pdate", EnumDBDataTypes.EDT_DateTime, responseQ.orderpack.p10_packinf.p10_pakagedate.ToString());
                        _dbsql.AddParameter("@pbody_send", EnumDBDataTypes.EDT_String, SerializeObjectToXml(requestQ));
                        _dbsql.AddParameter("@perr", EnumDBDataTypes.EDT_String, responseQ.orderpack.p10_packinf.p13_zerrpkg.ToString());
                        _dbsql.AddParameter("@perrmes", EnumDBDataTypes.EDT_String, responseQ.orderpack.p10_packinf.p14_errmsg == null ? String.Empty : responseQ.orderpack.p10_packinf.p14_errmsg.ToString());
                        _dbsql.AddParameter("@pbody_resp", EnumDBDataTypes.EDT_String, returnXmlQ);
                        _IDPackageQ = _dbsql.ExecuteScalar();

                        _dbsql.SQLScript = "exec WSProf.spPlanQtys @IDOper = 1, @IDObject = @idobj";
                        _dbsql.AddParameter("@idobj", EnumDBDataTypes.EDT_Integer, _IDPackageQ);
                        _dbsql.ExecuteNonQuery();
                    }
                    responseQ = null;
                    responseQ = null;
                }

                lbProtocol2.Items.Add("обновление контактных данных МО");
                //также закачаю контактные данные МО
                WS.getEvContactsResponse responseC = null;
                WS.getEvContactsRequest requestC = new WS.getEvContactsRequest(_WSUsername, _WSPassword, _WSSender);
                responseC = ws.getEvContacts(requestC);
                string returnXmlС = SerializeObjectToXml(responseC);
                if (responseC.orderpack.evContacts.Length > 0)
                {
                    string _IDPackageQ = "";
                    _dbsql.SQLScript = @"insert into WSProf.Packages(packagesTypeID, pdate_send, pbody_send, perr_response, perrmes_response, pbody_response) 
                                            values (7, @pdate, @pbody_send, @perr, @perrmes, @pbody_resp); 
                                            SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;";
                    _dbsql.AddParameter("@pdate", EnumDBDataTypes.EDT_DateTime, responseC.orderpack.p10_packinf.p10_pakagedate.ToString());
                    _dbsql.AddParameter("@pbody_send", EnumDBDataTypes.EDT_String, SerializeObjectToXml(requestC));
                    _dbsql.AddParameter("@perr", EnumDBDataTypes.EDT_String, responseC.orderpack.p10_packinf.p13_zerrpkg.ToString());
                    _dbsql.AddParameter("@perrmes", EnumDBDataTypes.EDT_String, responseC.orderpack.p10_packinf.p14_errmsg == null ? String.Empty : responseC.orderpack.p10_packinf.p14_errmsg.ToString());
                    _dbsql.AddParameter("@pbody_resp", EnumDBDataTypes.EDT_String, returnXmlС);
                    _IDPackageQ = _dbsql.ExecuteScalar();

                    _dbsql.SQLScript = "exec WSProf.spContactsMO @IDOper = 1, @IDObject = @idobj";
                    _dbsql.AddParameter("@idobj", EnumDBDataTypes.EDT_Integer, _IDPackageQ);
                    _dbsql.ExecuteNonQuery();
                }
                responseC = null;
                responseC = null;

                ws = null;
                binding = null;
                address = null;

                Cursor = System.Windows.Forms.Cursors.Default;
                _result = true;
            }
            catch (Exception ex)
            {
                Cursor = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = false;
            }
            finally
            {

            }
            return _result;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

           /* if (((DateTime.Now.Year == Convert.ToInt16(_year)) && (DateTime.Now.Month > Convert.ToInt16(_mnth))) || (DateTime.Now.Year < Convert.ToInt16(_year)))
            {
                MessageBox.Show("Попытка загрузить данные за минувший отчетный месяц\nИмпорт списка невозможен",
                                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }*/

            if (tabControl1.SelectedIndex == 1)
            {
                LoadEtalonXLS(tbFiles.Text);
            }

            //MessageBox.Show("ggg");
            if (tabControl1.SelectedIndex == 0)
            {
                LoadWS();
            }

            //удаление старых списков от лпу
            _dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 7, @mnth = @m, @year = @y";
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
            _dbsql.ExecuteNonQuery();

            //идентификация и пополнение людей
            _dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 2, @sessionGuid = @guid";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _sessionGuid);
            _dbsql.ExecuteNonQueryVisual("Идентификация списков запланированных граждан");

            //обновление плановых списков
            _dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 4, @sessionGuid = @guid, @mnth = @m, @year = @y";
            _dbsql.AddParameter("@guid", EnumDBDataTypes.EDT_String, _sessionGuid);
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
            _dbsql.ExecuteNonQueryVisual("Обновление запланированных МО списков");

            _dbsql.SQLScript = "exec WSProf.spPlanInfos @IDOper = 8, @mnth = @m, @year = @y";
            _dbsql.AddParameter("@m", EnumDBDataTypes.EDT_Integer, _mnth);
            _dbsql.AddParameter("@y", EnumDBDataTypes.EDT_Integer, _year);
            _dbsql.ExecuteNonQueryVisual("Обновление дополнительных списков на основании информации об информировании");

            MessageBox.Show("Информация о запланированных МО персональных списках граждан, подлежащих проведению профилактических мероприятий получена",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
