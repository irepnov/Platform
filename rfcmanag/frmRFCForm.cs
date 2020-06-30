using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using GGPlatform.DBServer;
using System.Collections;
using GGPlatform.RegManager;

namespace GGPlatform.RFCManag
{
    public partial class frmRFCForm : Form
    {
        internal class Field
        {
            private string _FieldName = "";
            private string _FieldCaption = "";
            private string _FieldType = "";
            private bool _FieldVision = true;

            public Field(string inFieldName, string inFieldCaption, string inFieldType, bool inFieldVision)
            {
                _FieldName = inFieldName;
                _FieldCaption = inFieldCaption;
                _FieldType = inFieldType;
                _FieldVision = inFieldVision;
            }

            public string FieldName { get { return _FieldName; } }
            public string FieldCaption { get { return _FieldCaption; } }
            public string FieldType { get { return _FieldType; } }
            public bool FieldVision { get { return _FieldVision; } }
        }

        private string _RFCName = "";
        private string _RFCCaption = "";
        private string _RFCQuery = "";
        private IWin32Window _sMainHandle;
        private DBSqlServer _DBSql = null;
        private int _RFCHeight = 270;
        private int _RFCWidth = 430;
        private ArrayList _RFCFields = null;
        private BindingSource bs = new BindingSource();
        public bool _RFCMultiSelect = true;
        public string _RegistrySoftName = "";

        // запись значения в реестр
        /* private void GGRegistrySetValue(string key, object value)
         {
             RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _RegistrySoftName + "\\" + "rfcmanager" + "_" + _RFCName);
             if (currRegistryKey != null)
             {
                 currRegistryKey.SetValue(key, value);
                 currRegistryKey.Close();
             }
         }
         // возвращает значение из реестра
         private object GGRegistryGetValue(string key)
         {
             object val = null;
             RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _RegistrySoftName + "\\" + "rfcmanager" + "_" + _RFCName);
             if (currRegistryKey != null)
             {
                 val = currRegistryKey.GetValue(key);
                 currRegistryKey.Close();
             }
             return val;
         }*/


        public frmRFCForm(IWin32Window inMainHandle, string inRFCName, DBSqlServer inDBServer, string inRegistrySoftName)
        {
            InitializeComponent();
            _sMainHandle = inMainHandle;
            _RFCName = inRFCName;
            _DBSql = inDBServer;
            _RegistrySoftName = inRegistrySoftName;
            _dgvcontrol.dataGridViewGG.GGRegistrySoftName = inRegistrySoftName;

            _dgvcontrol.Focus();
            _dgvcontrol.dataGridViewGG.Focus();

            SettingsRFC();

            this.Text = _RFCCaption;
            this.Height = _RFCHeight;
            this.Width = _RFCWidth;

            LoadData();
        }

        private void SettingsRFC()
        {
            try
            {
                _DBSql.SQLScript = "select ID, ObjectName as Name, ObjectCaption as Caption, ObjectExpression as Query, isnull(ObjectHeight, 330) as Height, isnull(ObjectWidth, 730) as Width from [GGPlatform].[Objects] where Upper(ObjectName) = @obj";
                _DBSql.AddParameter("@obj", EnumDBDataTypes.EDT_String, _RFCName.ToUpper());
                SqlDataReader Objects = _DBSql.ExecuteReader();
                int _obj = 0;
                while (Objects.Read())
                {
                    _obj = (int)Objects["ID"];
                    _RFCName = Objects["Name"].ToString();
                    _RFCCaption = Objects["Caption"].ToString();
                    _RFCHeight = (int)Objects["Height"];
                    _RFCWidth = (int)Objects["Width"];
                    _RFCQuery = Objects["Query"].ToString();
                }
                Objects.Close();


                GGPlatform.RegManager.RegManag _reg = new RegManag();
                string _p = "Software\\GGPlatform\\" + _RegistrySoftName + "\\" + "rfcmanager" + "_" + _RFCName;

                if (_reg.GGRegistryGetValue("Height", _p) != null)    //прочитаю размеры формы
                    _RFCHeight = (Int32)_reg.GGRegistryGetValue("Height", _p);  //иначе стандартные
                if (_reg.GGRegistryGetValue("Width", _p) != null)
                    _RFCWidth = (Int32)_reg.GGRegistryGetValue("Width", _p);
                _reg = null;


                _DBSql.SQLScript = "select ID, isnull(FieldAlias, FieldName) as FieldName, FieldCaption, FieldType, FieldVisible from GGPlatform.ObjectsDescription where ObjectsRef = @obj";
                _DBSql.AddParameter("@obj", EnumDBDataTypes.EDT_Integer, _obj.ToString());
                SqlDataReader ObjectsDescription = _DBSql.ExecuteReader();
                _RFCFields = new ArrayList();
                Field tmpfield = null;
                while (ObjectsDescription.Read())
                {
                    tmpfield = new Field(ObjectsDescription["FieldName"].ToString(),
                                         ObjectsDescription["FieldCaption"].ToString(), 
                                         ObjectsDescription["FieldType"].ToString(), 
                                         (bool)ObjectsDescription["FieldVisible"]);
                    _RFCFields.Add(tmpfield);
                    tmpfield = null;
                }
                ObjectsDescription.Close();
            }
            catch (Exception ex)
            {
                /* MessageBox.Show(_sMainHandle, "Ошибка настройки справочника \n" + ex.Message,
                                 "Ошибка [модуль rfcmanag, класс frmRFCForm, метод SettingsRFC]",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);*/
                throw new Exception("Ошибка настройки справочника \n" + ex.Message);
            }
        }

        private void LoadData()
        {
            if (_RFCQuery == "")
                return;

            //при повторном открытии не сохраняет галочки!!!!!
            _dgvcontrol.dataGridViewGG.AllowUserToAddRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToDeleteRows = false;
            _dgvcontrol.dataGridViewGG.AllowUserToOrderColumns = true;
            _dgvcontrol.dataGridViewGG.AutoGenerateColumns = false;
            _dgvcontrol.dataGridViewGG.MultiSelect = false;
           // _dgvcontrol.dataGridViewGG.ReadOnly = true;

            _dgvcontrol.dataGridViewGG.GGRegistryAssemblyName = "rfcmanager" + "_" + _RFCName;
            _DBSql.SQLScript = _RFCQuery;
            DataTable _dt = null;
            DataColumn _col = null;
            _dt = _DBSql.FillDataSet().Tables[0];
            _col = _dt.Columns.Add("IsCheckedField", System.Type.GetType("System.Boolean"));   ///добавлю Чекед поле
            _col.ReadOnly = false;
            _col.DefaultValue = "false";
            bs.DataSource = _dt;
            _dgvcontrol.dataGridViewGG.Columns.Clear();
            _dgvcontrol.dataGridViewGG.DataSource = bs;

            _dgvcontrol.dataGridViewGG.GGAddColumn("IsCheckedField", "check", typeof(Boolean));
            Field _field = null;
            string tmpFieldName = "";
            string tmpFieldCaption = "";
            string tmpFieldType = "";
            bool tmpFieldVisible = true;
            IEnumerator Enumerator = _RFCFields.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                _field = (Field)Enumerator.Current;
                tmpFieldName = _field.FieldName;
                tmpFieldCaption = _field.FieldCaption;
                tmpFieldType = _field.FieldType;
                tmpFieldVisible = _field.FieldVision;

                DataGridViewColumn colu = null;

                if (tmpFieldType == "L")
                {
                    colu = _dgvcontrol.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Boolean));
                } else
                    if (tmpFieldType == "N")
                    {
                        colu = _dgvcontrol.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(Int32));
                    }
                    else
                        if (tmpFieldType == "D")
                        {
                            colu = _dgvcontrol.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(DateTime));
                        }
                        else
                        {
                            colu = _dgvcontrol.dataGridViewGG.GGAddColumn(tmpFieldName, tmpFieldCaption, typeof(String));
                        }              
                
                if (tmpFieldName.ToUpper() == "ID")
                   tmpFieldVisible = false; //поле ИД сразу невидимо

                colu.Visible = tmpFieldVisible;
                colu.ReadOnly = true;
            }
            Enumerator = null;
            _field = null;

            _dgvcontrol.RefreshFilterFieldsGG();
            _dgvcontrol.dataGridViewGG.GGLoadGridRegistry();

            lbRecords.Text = _dt.Rows.Count.ToString() + " записей";
        }

        public string RFCName { get { return _RFCName; } }

        private void frmRFCForm_Load(object sender, EventArgs e)
        {
            _dgvcontrol.Focus();
            _dgvcontrol.dataGridViewGG.Focus();

            _dgvcontrol.dataGridViewGG.IsMultiSelectCheckedFieldGG = _RFCMultiSelect;
        }

        private void frmRFCForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _dgvcontrol.dataGridViewGG.GGSaveGridRegistry();

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _RegistrySoftName + "\\" + "rfcmanager" + "_" + _RFCName;
            _reg.GGRegistrySetValue("Height", this.Height, _p);  //размеры формы
            _reg.GGRegistrySetValue("Width", this.Width, _p);
            _reg = null;
        }

        public void RefreshRFC()
        {
            LoadData();
        }

        //return massiv values
        public string GetRFCValueMulti(string inFieldName)
        {
            string _str = "";

            List<DataGridViewRow> checkrows = _dgvcontrol.dataGridViewGG.GGGetCheckedDataGridViewRows();
            foreach (DataGridViewRow r in checkrows)
            {
                _str = _str + "'" + r.Cells[inFieldName].Value.ToString() + "', ";
            }
            checkrows.Clear();
            checkrows = null;

            if (_str.Length != 0)
                _str = _str.Substring(0, _str.Length - 2);
            else
                _str = String.Empty;

            return _str;
        }

        public ArrayList GetRFCValueArrayList(string inFieldName)
        {
            ArrayList _str = new ArrayList();

            List<DataGridViewRow> checkrows = _dgvcontrol.dataGridViewGG.GGGetCheckedDataGridViewRows();
            foreach (DataGridViewRow r in checkrows)
            {
                _str.Add(r.Cells[inFieldName].Value.ToString());
            }
            checkrows.Clear();
            checkrows = null;

            return _str;
        }

        //return one value
        public string GetRFCValueOne(string inFieldName)
        {
            string _str = "";

            List<DataGridViewRow> checkrows = _dgvcontrol.dataGridViewGG.GGGetCheckedDataGridViewRows();
            foreach (DataGridViewRow r in checkrows)
            {
                _str = r.Cells[inFieldName].Value.ToString();
            }
            checkrows.Clear();
            checkrows = null;

            if (_str.Length == 0)
                _str = String.Empty;

            return _str;
        }

        public void SetRFCValue(string inFieldName, string inFieldValue)
        {
            foreach (DataGridViewRow row in _dgvcontrol.dataGridViewGG.Rows)
            {
                row.Cells[0].Value = false;                    
            }
         //   TotalCheckedCheckBoxes = 0;
          //  HeaderCheckBox.Checked = false;
            _dgvcontrol.dataGridViewGG.Rows[0].Selected = true;
            _dgvcontrol.dataGridViewGG.CurrentCell = _dgvcontrol.dataGridViewGG[0, 0];

            if (inFieldValue != String.Empty)
            {
                string[] els = inFieldValue.Replace("'", "").Replace(" ", "").Split(',');
                int RowIndex = -1;
                foreach (string item in els)
                {
                    foreach (DataGridViewRow row in _dgvcontrol.dataGridViewGG.Rows)
                    {
                        if (row.Cells[inFieldName].Value.ToString().Replace("'", "").Replace(" ", "").Equals(item))
                        {
                            if (RowIndex == -1)
                                RowIndex = row.Index;
                            row.Cells[0].Value = true;
                          //  TotalCheckedCheckBoxes++;
                            break;
                        }
                    }
                }
                if (RowIndex != -1)//переходим на нужную строку
                {
                    _dgvcontrol.dataGridViewGG.Rows[RowIndex].Selected = true;
                    _dgvcontrol.dataGridViewGG.CurrentCell = _dgvcontrol.dataGridViewGG[0, RowIndex];
                }
                else
                {//иначе на первую строчку
                    _dgvcontrol.dataGridViewGG.Rows[0].Selected = true;
                    _dgvcontrol.dataGridViewGG.CurrentCell = _dgvcontrol.dataGridViewGG[0, 0];
                }
            }            
        }

        private void frmRFCForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.DialogResult = DialogResult.OK;
            else
                if (e.KeyCode == Keys.Escape)
                    this.DialogResult = DialogResult.Cancel;
        }

    }
}
