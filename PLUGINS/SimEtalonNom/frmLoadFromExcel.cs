using System.Windows.Forms;
using GGPlatform.DBServer;
using System;
using System.IO;
using GGPlatform.ExcelManagers;
using System.Data;

namespace SimEtalonNom
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

                string _Proizv, _Name, _isActive, _TypeP, _OS, _DisplayType, _DisplaySize, _SDCARD;
                string _CameraBasic, _CameraSecond, _ROM, _CPU, _RAM, _mp3_rad, _java_mms, _bluth_wifi, _stand, _navi, _akum, _rrc;

                _Proizv = _Name = _isActive = _TypeP = _OS = _DisplayType = _DisplaySize = _SDCARD = "";
                _CameraBasic = _CameraSecond = _ROM = _CPU = _RAM = _mp3_rad = _java_mms = _bluth_wifi = _stand = _navi = _akum = _rrc = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _Proizv = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _Name = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _isActive = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _TypeP = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _OS = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _DisplayType = _excel.GetValue("F" + _RowIndex.ToString()).Trim();
                    _DisplaySize = _excel.GetValue("G" + _RowIndex.ToString()).Trim();
                    _SDCARD = _excel.GetValue("H" + _RowIndex.ToString()).Trim();
                    _CameraBasic = _excel.GetValue("I" + _RowIndex.ToString()).Trim();
                    _CameraSecond = _excel.GetValue("J" + _RowIndex.ToString()).Trim();
                    _ROM = _excel.GetValue("K" + _RowIndex.ToString()).Trim();
                    _CPU = _excel.GetValue("L" + _RowIndex.ToString()).Trim();
                    _RAM = _excel.GetValue("M" + _RowIndex.ToString()).Trim();
                    _mp3_rad = _excel.GetValue("N" + _RowIndex.ToString()).Trim();
                    _java_mms = _excel.GetValue("O" + _RowIndex.ToString()).Trim();
                    _bluth_wifi = _excel.GetValue("P" + _RowIndex.ToString()).Trim();
                    _stand = _excel.GetValue("Q" + _RowIndex.ToString()).Trim();
                    _navi = _excel.GetValue("R" + _RowIndex.ToString()).Trim();
                    _akum = _excel.GetValue("S" + _RowIndex.ToString()).Trim();
                    _rrc = _excel.GetValue("T" + _RowIndex.ToString()).Trim();

                    if ((_Proizv == "-") || (_Proizv.Length == 0) || (String.IsNullOrEmpty(_Proizv)) || (_Proizv.Trim() == "")) _Proizv = "";
                    if ((_Name == "-") || (_Name.Length == 0) || (String.IsNullOrEmpty(_Name)) || (_Name.Trim() == "")) _Name = "";
                    if ((_isActive == "-") || (_isActive.Length == 0) || (String.IsNullOrEmpty(_isActive)) || (_isActive.Trim() == "")) _isActive = "";
                    if ((_TypeP == "-") || (_TypeP.Length == 0) || (String.IsNullOrEmpty(_TypeP)) || (_TypeP.Trim() == "")) _TypeP = "";
                    if ((_OS == "-") || (_OS.Length == 0) || (String.IsNullOrEmpty(_OS)) || (_OS.Trim() == "")) _OS = "";
                    if ((_DisplayType == "-") || (_DisplayType.Length == 0) || (String.IsNullOrEmpty(_DisplayType)) || (_DisplayType.Trim() == "")) _DisplayType = "";
                    if ((_DisplaySize == "-") || (_DisplaySize.Length == 0) || (String.IsNullOrEmpty(_DisplaySize)) || (_DisplaySize.Trim() == "")) _DisplaySize = "";
                    if ((_SDCARD == "-") || (_SDCARD.Length == 0) || (String.IsNullOrEmpty(_SDCARD)) || (_SDCARD.Trim() == "")) _SDCARD = "";
                    if ((_CameraBasic == "-") || (_CameraBasic.Length == 0) || (String.IsNullOrEmpty(_CameraBasic)) || (_CameraBasic.Trim() == "")) _CameraBasic = "";
                    if ((_CameraSecond == "-") || (_CameraSecond.Length == 0) || (String.IsNullOrEmpty(_CameraSecond)) || (_CameraSecond.Trim() == "")) _CameraSecond = "";
                    if ((_ROM == "-") || (_ROM.Length == 0) || (String.IsNullOrEmpty(_ROM)) || (_ROM.Trim() == "")) _ROM = "";
                    if ((_CPU == "-") || (_CPU.Length == 0) || (String.IsNullOrEmpty(_CPU)) || (_CPU.Trim() == "")) _CPU = "";
                    if ((_RAM == "-") || (_RAM.Length == 0) || (String.IsNullOrEmpty(_RAM)) || (_RAM.Trim() == "")) _RAM = "";
                    if ((_mp3_rad == "-") || (_mp3_rad.Length == 0) || (String.IsNullOrEmpty(_mp3_rad)) || (_mp3_rad.Trim() == "")) _mp3_rad = "";
                    if ((_java_mms == "-") || (_java_mms.Length == 0) || (String.IsNullOrEmpty(_java_mms)) || (_java_mms.Trim() == "")) _java_mms = "";
                    if ((_bluth_wifi == "-") || (_bluth_wifi.Length == 0) || (String.IsNullOrEmpty(_bluth_wifi)) || (_bluth_wifi.Trim() == "")) _bluth_wifi = "";
                    if ((_stand == "-") || (_stand.Length == 0) || (String.IsNullOrEmpty(_stand)) || (_stand.Trim() == "")) _stand = "";
                    if ((_navi == "-") || (_navi.Length == 0) || (String.IsNullOrEmpty(_navi)) || (_navi.Trim() == "")) _navi = "";
                    if ((_akum == "-") || (_akum.Length == 0) || (String.IsNullOrEmpty(_akum)) || (_akum.Trim() == "")) _akum = "";
                    if ((_rrc == "-") || (_rrc.Length == 0) || (String.IsNullOrEmpty(_rrc)) || (_rrc.Trim() == "")) _rrc = "";

                    _dbsql.SQLScript = @"exec uchEtalonNomenc @IDOperation = 1, @Proizv = @pr, @Name = @n, @isActive = @a, @TypeP = @t, @OS = @o, 
                                              @DisplayType = @dt, @DisplaySize = @ds, @SDCARD = @sd, @CameraBasic = @cb, @CameraSecond = @cs, 
                                              @ROM = @ro, @CPU = @cp, @RAM = @ra, @mp3_rad = @mp, @java_mms = @j, @bluth_wifi = @b, @stand = @sta, @navi = @na, @akum = @ak, @rrc = @rr";
                    _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_String, _Proizv);
                    _dbsql.AddParameter("@n", EnumDBDataTypes.EDT_String, _Name);
                    _dbsql.AddParameter("@a", EnumDBDataTypes.EDT_Bit, _isActive);
                    _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_String, _TypeP);
                    _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, _OS);
                    _dbsql.AddParameter("@dt", EnumDBDataTypes.EDT_String, _DisplayType);
                    _dbsql.AddParameter("@ds", EnumDBDataTypes.EDT_String, _DisplaySize);
                    _dbsql.AddParameter("@sd", EnumDBDataTypes.EDT_String, _SDCARD);
                    _dbsql.AddParameter("@cb", EnumDBDataTypes.EDT_String, _CameraBasic);
                    _dbsql.AddParameter("@cs", EnumDBDataTypes.EDT_String, _CameraSecond);
                    _dbsql.AddParameter("@ro", EnumDBDataTypes.EDT_String, _ROM);
                    _dbsql.AddParameter("@cp", EnumDBDataTypes.EDT_String, _CPU);
                    _dbsql.AddParameter("@ra", EnumDBDataTypes.EDT_String, _RAM);
                    _dbsql.AddParameter("@mp", EnumDBDataTypes.EDT_String, _mp3_rad);
                    _dbsql.AddParameter("@j", EnumDBDataTypes.EDT_String, _java_mms);
                    _dbsql.AddParameter("@b", EnumDBDataTypes.EDT_String, _bluth_wifi);
                    _dbsql.AddParameter("@sta", EnumDBDataTypes.EDT_String, _stand);
                    _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, _navi);
                    _dbsql.AddParameter("@ak", EnumDBDataTypes.EDT_Integer, _akum);
                    _dbsql.AddParameter("@rr", EnumDBDataTypes.EDT_Integer, _rrc);
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

        private bool LoadMonitoringXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                _excel.SetWorkSheetByIndex = 1;

                int _RowIndex = 5;

                string _NameEtal, _c1, _c2, _c3, _c4, _c5, _c6, _c7, _c8, _c9, _c10;

                _NameEtal = _c1 = _c2 = _c3 = _c4 = _c5 = _c6 = _c7 = _c8 = _c9 = _c10 = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NameEtal = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _c1 = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _c2 = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _c3 = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _c4 = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _c5 = _excel.GetValue("F" + _RowIndex.ToString()).Trim();
                    _c6 = _excel.GetValue("G" + _RowIndex.ToString()).Trim();
                    _c7 = _excel.GetValue("H" + _RowIndex.ToString()).Trim();
                    _c8 = _excel.GetValue("I" + _RowIndex.ToString()).Trim();
                    _c9 = _excel.GetValue("J" + _RowIndex.ToString()).Trim();
                    _c10 = _excel.GetValue("K" + _RowIndex.ToString()).Trim();

                    if ((_NameEtal == "-") || (_NameEtal.Length == 0) || (String.IsNullOrEmpty(_NameEtal)) || (_NameEtal.Trim() == "")) _NameEtal = "";
                    if ((_c1 == "-") || (_c1.Length == 0) || (String.IsNullOrEmpty(_c1)) || (_c1.Trim() == "")) _c1 = "";
                    if ((_c2 == "-") || (_c2.Length == 0) || (String.IsNullOrEmpty(_c2)) || (_c2.Trim() == "")) _c2 = "";
                    if ((_c3 == "-") || (_c3.Length == 0) || (String.IsNullOrEmpty(_c3)) || (_c3.Trim() == "")) _c3 = "";
                    if ((_c4 == "-") || (_c4.Length == 0) || (String.IsNullOrEmpty(_c4)) || (_c4.Trim() == "")) _c4 = "";
                    if ((_c5 == "-") || (_c5.Length == 0) || (String.IsNullOrEmpty(_c5)) || (_c5.Trim() == "")) _c5 = "";
                    if ((_c6 == "-") || (_c6.Length == 0) || (String.IsNullOrEmpty(_c6)) || (_c6.Trim() == "")) _c6 = "";
                    if ((_c7 == "-") || (_c7.Length == 0) || (String.IsNullOrEmpty(_c7)) || (_c7.Trim() == "")) _c7 = "";
                    if ((_c8 == "-") || (_c8.Length == 0) || (String.IsNullOrEmpty(_c8)) || (_c8.Trim() == "")) _c8 = "";
                    if ((_c9 == "-") || (_c9.Length == 0) || (String.IsNullOrEmpty(_c9)) || (_c9.Trim() == "")) _c9 = "";
                    if ((_c10 == "-") || (_c10.Length == 0) || (String.IsNullOrEmpty(_c10)) || (_c10.Trim() == "")) _c10 = "";

                    _dbsql.SQLScript = @"exec uchEtalonNomenc @IDOperation = 1, @Name = @n, @con1 = @c1,@con2 = @c2,@con3 = @c3,@con4 = @c4,@con5 = @c5,@con6 = @c6,@con7 = @c7,@con8 = @c8,@con9 = @c9,@con10 = @c10";
                    _dbsql.AddParameter("@n", EnumDBDataTypes.EDT_String, _NameEtal);
                    _dbsql.AddParameter("@c1", EnumDBDataTypes.EDT_Decimal, _c1);
                    _dbsql.AddParameter("@c2", EnumDBDataTypes.EDT_Decimal, _c2);
                    _dbsql.AddParameter("@c3", EnumDBDataTypes.EDT_Decimal, _c3);
                    _dbsql.AddParameter("@c4", EnumDBDataTypes.EDT_Decimal, _c4);
                    _dbsql.AddParameter("@c5", EnumDBDataTypes.EDT_Decimal, _c5);
                    _dbsql.AddParameter("@c6", EnumDBDataTypes.EDT_Decimal, _c6);
                    _dbsql.AddParameter("@c7", EnumDBDataTypes.EDT_Decimal, _c7);
                    _dbsql.AddParameter("@c8", EnumDBDataTypes.EDT_Decimal, _c8);
                    _dbsql.AddParameter("@c9", EnumDBDataTypes.EDT_Decimal, _c9);
                    _dbsql.AddParameter("@c10", EnumDBDataTypes.EDT_Decimal, _c10);
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

        private void ClearTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchEtalonNomenc @IDOperation = 3";
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
                            _res = LoadEtalonXLS(tbFiles.Text);//загружу их Екселя
                            break;
                        }
                    case 2:
                        {
                            ClearTmpProduct();
                            _res = LoadMonitoringXLS(tbFiles.Text);//загружу их Екселя
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
