using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GGPlatform.DBServer;
using GGPlatform.RegManager;

namespace GGPlatform.UserControls
{
    public partial class frmReference : Form
    {
        private Int32 _ReferenceID;
        private DBSqlServer _DBSql;
        private string _ReferenceName;

        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        CheckBox HeaderCheckBox = null;
        bool IsHeaderCheckBoxClicked = false;
        private string _CODEs = String.Empty;

        public frmReference()
        {
            InitializeComponent();

            _CODEs = String.Empty;
            dataGridSet();
        }

        [Category("Appearance"), Description("Sets Reference ID"), DisplayName("ReferenceID")]
        public Int32 ReferenceID
        {
            set
            {
                _ReferenceID = value;
            }
        }

        [Category("Appearance"), Description("Sets Reference DBSqlServer"), DisplayName("ReferenceDBSqlServer")]
        public DBSqlServer ReferenceDBSqlServer
        {
            set
            {
                _DBSql = value;
            }
        }

        private void dataGridSet()
        {
            AddHeaderCheckBox();
            AddCheckColumn();

            HeaderCheckBox.KeyUp += new KeyEventHandler(HeaderCheckBox_KeyUp);
            HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
            dataGridView.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
            dataGridView.CurrentCellDirtyStateChanged += new EventHandler(dataGridView_CurrentCellDirtyStateChanged);
            dataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_CellPainting);
            dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(OnCellMouseClick); 
        }

        public string ReferenceCODEs
        {
            get { return _CODEs; }
            set { _CODEs = value; }
        }

        // запись значения в реестр
      /*  private void GGRegistrySetValue(string key, object value)
        {
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _DBSql.InfoAboutConnection.SoftName + "\\" + "querybuilder_" + _ReferenceName + "\\Grids\\dataGridView");
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
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _DBSql.InfoAboutConnection.SoftName + "\\" + "querybuilder_" + _ReferenceName + "\\Grids\\dataGridView");
            if (currRegistryKey != null)
            {
                val = currRegistryKey.GetValue(key);
                currRegistryKey.Close();
            }
            return val;
        }*/

        // сохраняем грид в реестре
        public void GGSaveGridRegistry()
        {
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _DBSql.InfoAboutConnection.SoftName + "\\" + "querybuilder_" + _ReferenceName + "\\Grids\\dataGridView";
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                _reg.GGRegistrySetValue(c.Name + "_Visible", c.Visible, _p);
                _reg.GGRegistrySetValue(c.Name + "_Width", c.Width, _p);
                _reg.GGRegistrySetValue(c.Name + "_DisplayIndex", c.DisplayIndex, _p);
            }
            _reg = null;
        }
        // загружаем грид из реестра
        public void GGLoadGridRegistry()
        {
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _DBSql.InfoAboutConnection.SoftName + "\\" + "querybuilder_" + _ReferenceName + "\\Grids\\dataGridView";
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                object ob = _reg.GGRegistryGetValue(c.Name + "_Visible", _p);
                if (ob != null)
                    try { c.Visible = Convert.ToBoolean(ob); }
                    catch { }
                ob = _reg.GGRegistryGetValue(c.Name + "_Width", _p);
                if (ob != null)
                    try { c.Width = Convert.ToInt32(ob); }
                    catch { }
                ob = _reg.GGRegistryGetValue(c.Name + "_DisplayIndex", _p);
                if (ob != null)
                    try { c.DisplayIndex = Convert.ToInt32(ob); }
                    catch { }
            }
            _reg = null;
        }

        public void LoadReference()
        {
            HeaderCheckBox.Checked = false;

            string ObjectsSQL = "";
            string ObjectsCaption = "";
            string ObjectsID = "";
            string tmpFieldName = "";
            string tmpFieldCaption = "";

            _DBSql.SQLScript = "select * from GGPlatform.Objects where id = @ReferenceID";
            _DBSql.AddParameter("@ReferenceID", EnumDBDataTypes.EDT_Integer, _ReferenceID.ToString()); ///Скрипт Справочника
            SqlDataReader Objects = _DBSql.ExecuteReader();
            while (Objects.Read())
            {
                ObjectsSQL = Objects["ObjectExpression"].ToString();
                _ReferenceName = Objects["ObjectName"].ToString();
                ObjectsID = Objects["id"].ToString();
                ObjectsCaption = Objects["ObjectCaption"].ToString();
            }
            Objects.Close();

            this.Text = ObjectsCaption;

            DataTable _dt = null;
            DataColumn _col = null;
            _DBSql.SQLScript = ObjectsSQL;
            _dt = _DBSql.FillDataSet().Tables[0];
            _col = _dt.Columns.Add("IsChecked", System.Type.GetType("System.Boolean"));   ///добавлю Чекед поле
            _col.ReadOnly = false;
            _col.DefaultValue = "false";

            dataGridView.Columns.Clear();

            TotalCheckBoxes = dataGridView.RowCount;
            TotalCheckedCheckBoxes = 0;

            _DBSql.SQLScript = "select * from GGPlatform.ObjectsDescription where ObjectsRef = @ObjectsRef order by id";
            _DBSql.AddParameter("@ObjectsRef", EnumDBDataTypes.EDT_Integer, ObjectsID);
            SqlDataReader ObjectsDescription = _DBSql.ExecuteReader();

            dataGridView.AutoGenerateColumns = false;
            DataGridViewColumn column;
            DataGridViewCell cell;
            dataGridView.ReadOnly = false;

            //галочка
            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "IsChecked";
            column.Name = "IsChecked";
            column.ReadOnly = false;
            column.Width = 30;
            column.HeaderText = "";
            column.Frozen = true;
            dataGridView.Columns.Add(column);

            //остальные поля
            while (ObjectsDescription.Read())
            {
                tmpFieldName = ObjectsDescription["FieldName"].ToString().Replace("xxx", "Code");
                tmpFieldCaption = ObjectsDescription["FieldCaption"].ToString();
                _dt.Columns[tmpFieldName].Caption = tmpFieldCaption;

                if ((ObjectsDescription["FieldType"].ToString() == "C") || (ObjectsDescription["FieldType"].ToString() == "D") || (ObjectsDescription["FieldType"].ToString() == "N") || (ObjectsDescription["FieldType"].ToString() == "R"))
                {
                    column = new DataGridViewTextBoxColumn();
                    cell = new DataGridViewTextBoxCell();
                }
                else
                    if (ObjectsDescription["FieldType"].ToString() == "L")
                    {
                        column = new DataGridViewCheckBoxColumn();
                        cell = new DataGridViewCheckBoxCell();
                    }
                    else
                    {
                        column = new DataGridViewTextBoxColumn();
                        cell = new DataGridViewTextBoxCell();
                    }

                column.ReadOnly = true;
                column.DataPropertyName = tmpFieldName;
                column.Name = tmpFieldName;
                column.HeaderText = tmpFieldCaption;
                column.CellTemplate = cell;
                dataGridView.Columns.Add(column);
            }
            ObjectsDescription.Close();
            dataGridView.DataSource = _dt;
        }

        private void GetReference()
        {
            _CODEs = "";
            for (int i = 0; i <= (dataGridView.RowCount - 1); i++)
            {
                if ((dataGridView.Rows[i].Cells[0].Value != null) && (dataGridView.Rows[i].Cells[0].Value.ToString() == "True"))
                {
                    _CODEs = _CODEs + "'" + dataGridView.Rows[i].Cells["CODE"].Value.ToString() + "', ";
                }
            }

            if (_CODEs.Length != 0)
                _CODEs = _CODEs.Substring(0, _CODEs.Length - 2);
            else
                _CODEs = String.Empty;
        }

        private void SetReference()
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Cells[0].Value = false;
            }
            HeaderCheckBox.Checked = false;
            TotalCheckedCheckBoxes = 0;
            dataGridView.Rows[0].Selected = true;
            dataGridView.CurrentCell = dataGridView[0, 0];

            if (_CODEs != String.Empty)
            {
                string[] els = _CODEs.Replace("'","").Replace(" ", "").Split(',');
                int RowIndex = -1;
                foreach (string item in els)
                {
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.Cells["CODE"].Value.ToString().Replace("'", "").Replace(" ", "").Equals(item))
                        {
                            if (RowIndex == -1)
                                RowIndex = row.Index;
                            row.Cells[0].Value = true;
                            TotalCheckedCheckBoxes++;
                            break;
                        }
                    }
                }
                if (RowIndex != -1)//переходим на нужную строку
                {
                    dataGridView.Rows[RowIndex].Selected = true;
                    dataGridView.CurrentCell = dataGridView[0, RowIndex];
                }
                else
                {//иначе на первую строчку
                    dataGridView.Rows[0].Selected = true;
                    dataGridView.CurrentCell = dataGridView[0, 0];
                }
            }
        }

        private void frmReference_Load(object sender, EventArgs e)
        {
            LoadReference();
            SetReference();
            GGLoadGridRegistry();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsHeaderCheckBoxClicked)
                RowCheckBoxClick((DataGridViewCheckBoxCell)dataGridView[e.ColumnIndex, e.RowIndex]);
        }

        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentCell is DataGridViewCheckBoxCell)
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            HeaderCheckBoxClick((CheckBox)sender);
        }

        private void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                HeaderCheckBoxClick((CheckBox)sender);
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
                ResetHeaderCheckBoxLocation(e.ColumnIndex, e.RowIndex);
        }



        private void AddHeaderCheckBox()
        {
            HeaderCheckBox = new CheckBox();
            HeaderCheckBox.Size = new Size(15, 15);
            this.dataGridView.Controls.Add(HeaderCheckBox);
        }

        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            Rectangle oRectangle = this.dataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);
            Point oPoint = new Point();
            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;
            HeaderCheckBox.Location = oPoint;
        }

        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            dataGridView.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IsHeaderCheckBoxClicked = true;
            foreach (DataGridViewRow Row in dataGridView.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["IsChecked"]).Value = HCheckBox.Checked;
            dataGridView.RefreshEdit();
            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;
            IsHeaderCheckBoxClicked = false;
            dataGridView.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {      
                if ((bool)RCheckBox.Value && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBox.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBox.Checked = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GetReference();
            this.DialogResult = DialogResult.OK;
        }

        [DllImport("User32.dll")]
        extern static int PostMessage(IntPtr hWnd, UInt32 msg, Int32 wParam, Int32 lParam);

        private void dataGridView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                bool ch = !(bool)((DataGridViewCheckBoxCell)((DataGridView)sender).CurrentRow.Cells["IsChecked"]).Value;

                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)((DataGridView)sender).CurrentRow.Cells["IsChecked"];
                cell.Value = ch;
                RowCheckBoxClick(cell);

                //перемещение по гриду, для того что бы комбо-бокс прорисовался
                if (dataGridView.CurrentRow.Index < dataGridView.RowCount - 1)
                    PostMessage(dataGridView.Handle, 0x0104, 0x28, 0);
                else
                {                    //поледня строка, перейду вверх 0x26 - вниз 0x28
                    PostMessage(dataGridView.Handle, 0x0104, 0x26, 0);
                    PostMessage(dataGridView.Handle, 0x0104, 0x28, 0);
                }

            }
        }

        private void frmReference_FormClosed(object sender, FormClosedEventArgs e)
        {
            GGSaveGridRegistry();
        }

        private void frmReference_Shown(object sender, EventArgs e)
        {
            //dataGridView.Focus();
        }



        private void AddCheckColumn()
        {
            mCheckedListBox = new CheckedListBox();
            mCheckedListBox.ForeColor = SystemColors.HotTrack;
            mCheckedListBox.CheckOnClick = true;
            mCheckedListBox.ItemCheck += new ItemCheckEventHandler(mCheckedListBox_ItemCheck);

            ToolStripControlHost mControlHost = new ToolStripControlHost(mCheckedListBox);
            mControlHost.Padding = Padding.Empty;
            mControlHost.Margin = Padding.Empty;
            mControlHost.AutoSize = false;

            mPopup = new ToolStripDropDown();
            mPopup.Padding = Padding.Empty;
            mPopup.Items.Add(mControlHost);
        }
        // CheckedListBox содержующий наши колонки
        private CheckedListBox mCheckedListBox;
        // ToolStripDropDown - контейнер для mCheckedListBox
        private ToolStripDropDown mPopup;
        // Максимальная высота всплывающего окна
        public int HeightWindowFields = 300;
        // Максимальная ширина всплывающего окна
        public int WidthWindowFields = 200;
        // переопределяю событие, если щелчек в левом верхнем улгу, то мое событие иначе типовое
        // список видимых/невидимых полей
        private void OnCellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                mCheckedListBox.Items.Clear();
                foreach (DataGridViewColumn c in dataGridView.Columns)
                {
                    //if (c.Name.ToUpper() != "ISCHECKEDFIELD")
                    mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                }
                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < HeightWindowFields) ? PreferredHeight : HeightWindowFields;
                mCheckedListBox.Width = this.WidthWindowFields;
                mPopup.Show(this.PointToScreen(new Point(e.X, e.Y)));
            }
        }
        // событие видимость-невидимость столбца
        private void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            dataGridView.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }

    }
}