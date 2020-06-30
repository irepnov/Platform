using System;
using System.IO;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.ExcelManagers;

namespace SimPrihodT
{
    public partial class frmTovar : Form
    {
        private DBSqlServer _dbsql = null;
        private ExcelManager _excel = null;
        private Int32 _CountICC = 0;

        public Int32 _SelectedType = -1;
        private Int32 _TypeProduct = -1;
        private double _PriceProduct = 0;
        private string _ProductName = "";

        public frmTovar(DBSqlServer inDBSql, Int32 inCountICC, Int32 inTypeProduct, double inPriceProduct, String inProductName)
        {
            InitializeComponent();
            _dbsql = inDBSql;
            _CountICC = inCountICC;
            _SelectedType = -1;
            _TypeProduct = inTypeProduct;
            _PriceProduct = inPriceProduct;
            _ProductName = inProductName;
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        {
                            _result = true;
                            if (tbFiles.Text == "")
                            {
                                ActiveControl = tbFiles;
                                throw new Exception("Необходимо выбрать импортируемый файл");
                            }
                            break;
                        }
                    case 1:
                        {
                            _result = true;
                            if (_TypeProduct < 1)
                            {
                                throw new Exception("В параметрах накладной необходимо указать номенклатуру товара");
                            }
                            if (_PriceProduct < 0)
                            {
                                throw new Exception("В параметрах накладной необходимо указать цену товара");
                            }
                            if (tbFirst.Text == "")
                            {
                                ActiveControl = tbFirst;
                                throw new Exception("Необходимо указать первый номер ICC");
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _result = false;
                MessageBox.Show(ex.Message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return _result;
            //проверки
        }

        private bool LoadProductXLS(string _inFiles)
        {
            bool _result = false;
            try
            {
                if (!File.Exists(_inFiles))
                    throw new Exception("Указанный файл не найден");

                lbProtocol.Items.Add("открываем файл для загрузки...");
                _excel = new ExcelManager(_inFiles);
                int _RowIndex = 2;
                string _NumberCard, _NumberPhone, _ProductName, _NumberBill, _DateBill, _ProductType, _Price;
                _NumberCard = _NumberPhone = _ProductName = _NumberBill = _DateBill = _ProductType = _Price = "";

                lbProtocol.Items.Add("загрузка сведений...");
                while ( _excel.GetValue("A" + _RowIndex.ToString()).Trim() != "")
                {
                    _NumberBill = _excel.GetValue("A" + _RowIndex.ToString()).Trim();
                    _DateBill = _excel.GetValue("B" + _RowIndex.ToString()).Trim();
                    _ProductType = _excel.GetValue("C" + _RowIndex.ToString()).Trim();
                    _NumberCard = _excel.GetValue("D" + _RowIndex.ToString()).Trim();
                    _Price = _excel.GetValue("E" + _RowIndex.ToString()).Trim();
                    _NumberPhone = _excel.GetValue("F" + _RowIndex.ToString()).Trim();
                    _ProductName = _excel.GetValue("G" + _RowIndex.ToString()).Trim();

                    _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 6, @NumberCard = @card, @NumberPhone = @phone, @ProductName = @prname, @NumberBill = @num, @DateBill = @date, @TypeProductName = @name, @PriceProduct = @cena";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _NumberCard);
                    _dbsql.AddParameter("@phone", EnumDBDataTypes.EDT_String, _NumberPhone);
                    _dbsql.AddParameter("@prname", EnumDBDataTypes.EDT_String, _ProductName);
                    _dbsql.AddParameter("@num", EnumDBDataTypes.EDT_Integer, _NumberBill);
                    _dbsql.AddParameter("@date", EnumDBDataTypes.EDT_DateTime, _DateBill);
                    _dbsql.AddParameter("@name", EnumDBDataTypes.EDT_String, _ProductType);
                    _dbsql.AddParameter("@cena", EnumDBDataTypes.EDT_Decimal, _Price);
                    _dbsql.ExecuteNonQuery();

                    _RowIndex = _RowIndex + 1;
                }
                lbProtocol.Items.Add("загружено записей " + (_RowIndex - 2).ToString() );
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

        private bool LoadProduct()
        {
            bool _result = false;
            try
            {
                double tmpICC = Convert.ToDouble(tbFirst.Text.Substring(8, 12));
                Int32 i = 0;
                string _number = "";
                for (i = 0; i <= _CountICC - 1; i++)
                {                    
                    _number = tbFirst.Text.Substring(0, 7) + tmpICC.ToString();
                    _dbsql.SQLScript = @"exec uchIncomeBill @IDOperation = 2, @NumberCard = @card, @PriceProduct = @pr, 
                                                @TypeProductRef = @typ, @ProductName = @TypeP";
                    _dbsql.AddParameter("@card", EnumDBDataTypes.EDT_String, _number);
                    _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_Decimal, _PriceProduct.ToString());
                    _dbsql.AddParameter("@typ", EnumDBDataTypes.EDT_String, _TypeProduct.ToString());                    
                    _dbsql.AddParameter("@TypeP", EnumDBDataTypes.EDT_String, _ProductName);
                    _dbsql.ExecuteNonQuery();

                    tmpICC = tmpICC + 1;
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
            }
            return _result;
        }

        private void btFiles_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Перечень товара|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFiles.Text = openFileDialog1.FileName;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private string GetLastICC(string FirstICC, Int32 CountICC)
        {
            double tmpICC = Convert.ToDouble(FirstICC.Substring(8, 12));
            tmpICC = tmpICC + CountICC - 1;
            return FirstICC.Substring(0, 7) + tmpICC.ToString();           
        }

        private void tbFirst_TextChanged(object sender, EventArgs e)
        {
            if ((tbFirst.Text.Length != 20) || (_CountICC < 1))
                return;
            tbLast.Text = GetLastICC(tbFirst.Text, _CountICC);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;

            DialogResult _result = DialogResult.Cancel;

            try
            {
                bool _res = false;

                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();

                if (tabControl1.SelectedIndex == 0)
                    _res = LoadProductXLS(tbFiles.Text);

                if (tabControl1.SelectedIndex == 1)
                    _res = LoadProduct();

                if (!_res)
                    throw new Exception("Приход товара не осуществлен");

                _SelectedType = tabControl1.SelectedIndex;

                if (_res)
                    MessageBox.Show("Товар оприходован", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _result = DialogResult.OK;
            }
            catch (Exception ex)
            {
                _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
                _dbsql.ExecuteNonQuery();

                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _result = DialogResult.Cancel;
            }
            finally
            {
                
            }
            this.DialogResult = _result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
            _dbsql.ExecuteNonQuery();

            this.DialogResult = DialogResult.Cancel;
        }

        private void frmTovar_Shown(object sender, EventArgs e)
        {
            _dbsql.SQLScript = "exec uchIncomeBill @IDOperation = 1";
            _dbsql.ExecuteNonQuery();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedTab == tpNumbers) && (_CountICC < 1))
                MessageBox.Show("В параметрах накладной не заданы обязательные параметры\nУкажите параметры накладной", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
