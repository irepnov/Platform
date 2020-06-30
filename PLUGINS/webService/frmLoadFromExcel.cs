using System.Windows.Forms;
using GGPlatform.DBServer;
using System;
using System.IO;
using GGPlatform.ExcelManagers;
using System.Data;
using GGPlatform.RegManager;

namespace webService
{
    public partial class frmLoadFromExcel : Form
    {
        private DBSqlServer _dbsql = null;
        private Int32 _ActionID = -1;
        private ExcelManager _excel = null;
        private RegManag _reg = new RegManag();
        private string _SoftName, _PluginAssembly;
        private string _templatename = "\\reports\\template\\";
        public frmLoadFromExcel(object[] inParams)
        {
            InitializeComponent();
            _dbsql = (DBSqlServer)inParams[0];
            _ActionID = Convert.ToInt32(inParams[1]);
            _SoftName = inParams[2].ToString();
            _PluginAssembly = inParams[3].ToString();

            if (_ActionID == 8)
            {
                openFileDialog1.Multiselect = true;
            }
                
            ClearTmpProduct();
            LoadDataCLB();
        }

        private void btFiles_Click(object sender, System.EventArgs e)
        {
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _PluginAssembly + "\\Options";

            try
            {
                openFileDialog1.InitialDirectory = _reg.GGRegistryGetValue("LastDir", _p).ToString();
            }            
            catch
            {
                openFileDialog1.InitialDirectory = Application.StartupPath;
            }
            openFileDialog1.Filter = "Обработываемый товар|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (_ActionID == 8)
                    tbFiles.Text = String.Join(";", openFileDialog1.FileNames);
                    else
                        tbFiles.Text = openFileDialog1.FileName;

                _reg.GGRegistrySetValue("LastDir", Path.GetDirectoryName(openFileDialog1.FileName), _p);
            }
        }

        private void LoadDataCLB()
        {
            cbPostav.Items.Clear();

            _dbsql.SQLScript = "select id, name from rfcPostav order by 2";
            LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbPostav);
        }

        private void LoadCLB(DataTable inObjects, ref ComboBox inCLB)
        {
            if (inObjects == null)
                return;

            inCLB.ValueMember = "ID";
            inCLB.DisplayMember = "Name";
            inCLB.DataSource = inObjects;
            inCLB.SelectedValue = DBNull.Value;
        }

        private string GetCheckedCLB(ref ComboBox inCLB)
        {
            if ((inCLB.SelectedValue == null) || (inCLB.SelectedValue == DBNull.Value))
                return "";
            else return inCLB.SelectedValue.ToString();
        }

        private void SetCheckedCLB(object inObjects, ref ComboBox inCLB)
        {
            inCLB.SelectedValue = inObjects;
        }

        private bool LoadPostavXLS(string _inFiles)
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

                string _Name, _NameRodn, _Price;

                _Name = _NameRodn = _Price = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while (_excel.GetValue("B" + _RowIndex.ToString()).Trim() != "")
                {
                    _NameRodn = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _Name = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _Price = _excel.GetValue("C" + _RowIndex.ToString()).Trim();

                    if ((_NameRodn == "-") || (_NameRodn.Length == 0) || (String.IsNullOrEmpty(_NameRodn)) || (_NameRodn.Trim() == "")) _NameRodn = "";
                    if ((_Name == "-") || (_Name.Length == 0) || (String.IsNullOrEmpty(_Name)) || (_Name.Trim() == "")) _Name = "";
                    if ((_Price == "-") || (_Price.Length == 0) || (String.IsNullOrEmpty(_Price)) || (_Price.Trim() == "")) _Price = "";

                    _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 1, @Name = @n, @RodnName = @rn, @PricePostav = @price, @PostavRef = @pr, @DatePrice = @d";
                    _dbsql.AddParameter("@n", EnumDBDataTypes.EDT_String, _Name);
                    _dbsql.AddParameter("@rn", EnumDBDataTypes.EDT_String, _NameRodn);
                    _dbsql.AddParameter("@price", EnumDBDataTypes.EDT_String, _Price);
                    _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbPostav));
                    _dbsql.AddParameter("@d", EnumDBDataTypes.EDT_DateTime, DateTime.Now.ToString());
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
            _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 3";
            _dbsql.ExecuteNonQuery();
        }

        private void FillTmpProductEtalon()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 2, @PostavRef = @post";
            _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbPostav));
            _dbsql.ExecuteNonQuery();
        }

        private DataTable SelectEmpryTmpProduct()//очиу вренменную таблицу
        {
            _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 4, @PostavRef = @post";
            _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbPostav));
            return _dbsql.FillDataSet().Tables[0];
        }


        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (cbPostav.Text == "")
                {
                    ActiveControl = cbPostav;
                    throw new Exception("Необходимо указать поставщика");
                }
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
                            _res = LoadPostavXLS(tbFiles.Text);//загружу их Екселя
                            FillTmpProductEtalon();

                            DataTable _DTEmpty = SelectEmpryTmpProduct();
                            if (_DTEmpty.Rows.Count > 0)
                            {

                                frmSootvet fmSoot = new frmSootvet(new object[] { _dbsql, _DTEmpty, GetCheckedCLB(ref cbPostav), chbIntellect.Checked });
                                if (fmSoot.ShowDialog() != DialogResult.OK)
                                    ClearTmpProduct();
                                fmSoot.Dispose();
                                fmSoot = null;
                            }
                            
                            _dbsql.SQLScript = @"exec uchPostavNomenc @IDOperation = 5, @PostavRef = @post";
                            _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbPostav));
                            _dbsql.ExecuteNonQuery();

                            break;
                        }
                    //case 2:
                    //    {
                    //        ClearTmpProduct();

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
