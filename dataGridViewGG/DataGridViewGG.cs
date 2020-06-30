using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Data;
using System.Linq;
using GGPlatform.RegManager;


namespace DataGridViewGG
{
    public class DataGridViewGG : DataGridView
    {
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

        public class FieldVisible
        {
            public string Name { get; set; }
            public string HeaderText { get; set; }
            public bool Visible { get; set; }
            public override string ToString()
            {
                return HeaderText;
            }
        }

        public class ColumnRegistry
        {
            public string Name { get; set; }
            public Int32 Width { get; set; }
            public Int32 DisplayIndex { get; set; }
            public bool Visible { get; set; }
        }

        protected override void OnColumnRemoved(DataGridViewColumnEventArgs e)
        {
            base.OnColumnRemoved(e);

            if ((this.Columns.Count == 0) && (HeaderCheckBox != null))//если удалены столбцы, то и есть ЧекБокс в заголовке, то удалю его
            {
                this.Controls.Remove(HeaderCheckBox);
                if (HeaderCheckBox != null)
                {
                    HeaderCheckBox.Dispose();
                    HeaderCheckBox = null;
                }
            }               
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex == -1 && e.ColumnIndex == -1)
            {
                mCheckedListBox.Items.Clear();



                var cc = from col in this.Columns.Cast<DataGridViewColumn>()
                         orderby col.DisplayIndex
                         select new FieldVisible { Name = col.Name, HeaderText = col.HeaderText, Visible = col.Visible };
                foreach (FieldVisible c in cc)
                {
                    //if (c.Name.ToUpper() != "ISCHECKEDFIELD")
                        mCheckedListBox.Items.Add(c, c.Visible);
                }



                /*
                 * было так
                 * foreach (DataGridViewColumn c in this.Columns)
                {
                    //if (c.Name.ToUpper() != "ISCHECKEDFIELD")
                    mCheckedListBox.Items.Add(c.HeaderText, c.Visible);
                }*/


                int PreferredHeight = (mCheckedListBox.Items.Count * 16) + 7;
                mCheckedListBox.Height = (PreferredHeight < HeightWindowFields) ? PreferredHeight : HeightWindowFields;
                mCheckedListBox.Width = this.WidthWindowFields;
                mPopup.Show(this.PointToScreen(new Point(e.X, e.Y)));
            }
            else base.OnCellMouseClick(e);
        }

        //protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.RowIndex == -1 && e.ColumnIndex == -1)
        //    {
        //       // MessageBox.Show("ggg");
        //    }
        //    else base.OnCellMouseClick(e);
        //}

        // переопределяю коснтруктор
        public DataGridViewGG()
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
        // событие видимость-невидимость столбца
        private void mCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {            
            this.Columns[(mCheckedListBox.Items[e.Index] as FieldVisible).Name].Visible = (e.NewValue == CheckState.Checked);
          //было так  this.Columns[e.Index].Visible = (e.NewValue == CheckState.Checked);
        }




        // имя сборки для сохранения настроек в реестре
        public string GGRegistryAssemblyName = "";
        // имя программы для сохранения настроек в реестре
        public string GGRegistrySoftName = "";
        //имя объекта запроса из БД
        public string GGObjectName = "";
        // пишет значение в реестр
        /*  private void GGRegistrySetValue(string key, object value)
          {
              RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + GGRegistrySoftName + "\\" + GGRegistryAssemblyName + "\\Grids\\" + this.Name);
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
              RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + GGRegistrySoftName + "\\" + GGRegistryAssemblyName + "\\Grids\\" + this.Name);
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
            if ((GGRegistryAssemblyName == "") || (GGRegistrySoftName == ""))
               return;

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + GGRegistrySoftName + "\\" + GGRegistryAssemblyName + "\\Grids\\" + this.Name;

            foreach (DataGridViewColumn c in this.Columns)
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
            string _p = "Software\\GGPlatform\\" + GGRegistrySoftName + "\\" + GGRegistryAssemblyName + "\\Grids\\" + this.Name;

            List<ColumnRegistry> list = new List<ColumnRegistry>();
            
            //читаю настройки из реестра и Сразу задаю Видимость и Длину столбцов
            foreach (DataGridViewColumn c in this.Columns)
            {
                object ob = new object();

                bool _vis = false;
                Int32 _ind = 0;
                Int32 _wid = 0;

                ob = _reg.GGRegistryGetValue(c.Name + "_Visible", _p);
                if (ob != null)
                    try
                    {
                        _vis = Convert.ToBoolean(ob);
                        c.Visible = Convert.ToBoolean(ob);
                    }
                    catch { _vis = false; }

                ob = _reg.GGRegistryGetValue(c.Name + "_Width", _p);
                if (ob != null)
                    try
                    {
                        _wid = Convert.ToInt32(ob);
                        c.Width = Convert.ToInt32(ob);
                    }
                    catch { _wid = 0; }

                ob = _reg.GGRegistryGetValue(c.Name + "_DisplayIndex", _p);
                if (ob != null)
                    try { _ind = Convert.ToInt32(ob); }
                    catch { _ind = 0; }

                list.Add(new ColumnRegistry {Name = c.Name, Visible = _vis, Width = _wid, DisplayIndex = _ind });

                /*ob = _reg.GGRegistryGetValue(c.Name + "_Visible", _p);
                if (ob != null)
                    try { c.Visible = Convert.ToBoolean(ob); }
                    catch { }
                ob = _reg.GGRegistryGetValue(c.Name + "_Width", _p);
                if (ob != null)
                    try { c.Width = Convert.ToInt32(ob); }
                    catch { }
                ob = _reg.GGRegistryGetValue(c.Name + "_DisplayIndex", _p);

                if (c.Name == "IMEI")              MessageBox.Show(c.Name + "_DisplayIndex = " + Convert.ToString(ob));

                if (ob != null)
                    try { c.DisplayIndex = Convert.ToInt32(ob); }
                    catch (Exception e) {
                        MessageBox.Show(e.Message);
                    }*/
            }

            //затем только для Видимых солбцов, в порядке нумерации Задаю Очереднолсть
            var cc = from col in list.Cast<ColumnRegistry>()
                     where col.Visible == true
                     orderby col.DisplayIndex
                     select col;

            foreach (ColumnRegistry c in cc)
            {
                try
                { this.Columns[c.Name].DisplayIndex = c.DisplayIndex; }
                catch { };
            }

            cc = null;
            list.Clear();
            list = null;
            _reg = null;
        }




        //выгрузка содержимого в Ексель
        private frmCheckFields _fmCheckFields = null;
        public void GGExportToExcel()
        {
            if (this.RowCount < 0)
            {
                MessageBox.Show("Отсутствуют данные для экспорта",
                                "Ошибка [модуль DataGridViewGG, класс DataGridViewGG, метод ExportToExcel]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_fmCheckFields == null)
            {
                _fmCheckFields = new frmCheckFields(this, 1);
                _fmCheckFields.ShowDialog();
                _fmCheckFields.Dispose();
                _fmCheckFields = null;
            }
        }
        public void GGExportToWord()
        {
            if (this.RowCount < 0)
            {
                MessageBox.Show("Отсутствуют данные для экспорта",
                                "Ошибка [модуль DataGridViewGG, класс DataGridViewGG, метод ExportToWord]",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_fmCheckFields == null)
            {
                _fmCheckFields = new frmCheckFields(this, 2);
                _fmCheckFields.ShowDialog();
                _fmCheckFields.Dispose();
                _fmCheckFields = null;
            }
        }
        public void GGExportToHTML()
        {
            if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table>");
            sb.AppendLine("  <tr>");
            foreach (DataGridViewColumn column in this.Columns)
            {
                sb.AppendLine("    <td>" + column.HeaderText + "</td>");
            }
            sb.AppendLine("  </tr>");
            foreach (DataGridViewRow row in this.Rows)
            {
                sb.AppendLine("  <tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.AppendLine("    <td width=\"" + cell.Size.Width.ToString() + "px\">" + cell.Value + "</td>");
                }
                sb.AppendLine("  </tr>");
            }
            sb.AppendLine("</table>");

            File.WriteAllText(@"c:\temp\" + this.GGRegistryAssemblyName + "_" + this.Name + ".html", sb.ToString());
            sb = null;
            MessageBox.Show("Сформирован файл " + @"c:\temp\" + this.GGRegistryAssemblyName + "_" + this.Name + ".html", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void GGExportToXML()
        {
            if (!Directory.Exists("C:\\temp\\")) Directory.CreateDirectory("C:\\temp\\");
            BindingSource bs = null;
            DataTable dt = null;
            bs = ((BindingSource)this.DataSource);
            dt = (DataTable)bs.DataSource;
            dt.WriteXml(@"c:\temp\" + this.GGRegistryAssemblyName + "_" + this.Name + ".xml");
            MessageBox.Show("Сформирован файл " + @"c:\temp\" + this.GGRegistryAssemblyName + "_" + this.Name + ".xml", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// добавление колонок
        /// </summary>
        private int TotalCheckBoxes = 0;
        private int TotalCheckedCheckBoxes = 0;
        private CheckBox HeaderCheckBox = null;
        private bool IsHeaderCheckBoxClicked = false;
        public bool IsMultiSelectCheckedFieldGG = true; 

        public DataGridViewColumn GGAddColumn(string inFieldName, string inHeaderName, Type inValueType, bool inMultiSelectCheckedField)
        {
            IsMultiSelectCheckedFieldGG = inMultiSelectCheckedField;
            return GGAddColumn(inFieldName, inHeaderName, inValueType);
        }

        public DataGridViewColumn GGAddColumn(string inFieldName, string inHeaderName, Type inValueType)
        {
            if (inFieldName.ToUpper() == "ISCHECKEDFIELD")
            {
                AddHeaderCheckBox();
                HeaderCheckBox.Checked = false;

                HeaderCheckBox.KeyUp += new KeyEventHandler(HeaderCheckBox_KeyUp);
                HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);
                this.CellValueChanged += new DataGridViewCellEventHandler(dataGridView_CellValueChanged);
                this.CurrentCellDirtyStateChanged += new EventHandler(dataGridView_CurrentCellDirtyStateChanged);
                this.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView_CellPainting);

                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                col.DataPropertyName = inFieldName;
                col.Name = inFieldName;
                col.ValueType = inValueType;
                col.ReadOnly = false;
                col.Width = 30;
                col.HeaderText = "";
                col.Frozen = true;
                this.Columns.Add(col);
                TotalCheckBoxes = this.Rows.Count; //установим кол-во всех строк
                return (DataGridViewColumn)col;
            }
            else 
            {
                switch (inValueType.ToString())
                {
                    case "System.String":
                    case "System.Int32":
                    case "System.DateTime":
                        {
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                            col.Name = inFieldName;
                            col.ValueType = inValueType;
                            col.DataPropertyName = inFieldName;
                            col.HeaderText = inHeaderName;
                            this.Columns.Add(col);
                            return (DataGridViewColumn)col;
                        }
                    case "System.Boolean":
                        {
                            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                            col.Name = inFieldName;
                            col.ValueType = inValueType;
                            col.DataPropertyName = inFieldName;
                            col.HeaderText = inHeaderName;
                            this.Columns.Add(col);
                            return (DataGridViewColumn)col;
                        }
                    default: return null;
                }
            }
        }

        public List<DataGridViewRow> GGGetCheckedDataGridViewRows()
        {
            if (this.Columns.Contains("IsCheckedField")) 
            {
                List<DataGridViewRow> _rows = new List<DataGridViewRow>();

                foreach (DataGridViewRow row in this.Rows)
                {
                    if ((row.Cells["IsCheckedField"].Value != null) && (row.Cells["IsCheckedField"].Value.ToString().ToUpper() == "TRUE"))
                   {
                       _rows.Add(row);
                   }
                }

                return _rows;
            } 
            else
                return null;
        }

        public List<DataRow> GGGetCheckedDataRows()
        {
            if (this.Columns.Contains("IsCheckedField"))
            {
                List<DataRow> _rows = new List<DataRow>();

                foreach (DataGridViewRow row in this.Rows)
                {
                    if ((row.Cells["IsCheckedField"].Value != null) && (row.Cells["IsCheckedField"].Value.ToString().ToUpper() == "TRUE"))
                    {
                        _rows.Add((row.DataBoundItem as DataRowView).Row);
                    }
                }

                return _rows;
            }
            else
                return null;
        }

        public string GGGetFieldValuesCheckedRows(string inFieldName)
        {
            string sel = "";
            foreach (DataGridViewRow row in GGGetCheckedDataGridViewRows())
            {
                sel += "'" + row.Cells[inFieldName].Value.ToString() + "', ";
            }

            if (sel.Length != 0)
                sel = sel.Substring(0, sel.Length - 2);
            else
                sel = String.Empty;

            return sel;
        }





        public void GGSetCheckedRows(bool inState)
        {
            if (!IsMultiSelectCheckedFieldGG)
            {
                MessageBox.Show("Множественный выбор для данного справочника запрещен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.Columns.Contains("IsCheckedField"))
            {
                foreach (DataGridViewRow row in this.Rows)
                {
                    row.Cells["IsCheckedField"].Value = inState;
                }
                
                HeaderCheckBox.Checked = inState;

                if (inState)
                    TotalCheckedCheckBoxes = TotalCheckBoxes;
                else
                    TotalCheckedCheckBoxes = 0;
            }
        }

        //сам переопределил
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Space) && (this.Columns.Contains("IsCheckedField")))
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                bool ch = false;
                DataGridViewCheckBoxCell cel = (DataGridViewCheckBoxCell)(this.CurrentRow.Cells["IsCheckedField"]);
                if (cel.Value != null)
                {
                    if ((cel.Value.ToString() == "") || (cel.Value.ToString() == "false"))
                        ch = false;
                    else
                        ch = (bool)(cel.Value);
                }
                else ch = false;


                if ((!IsMultiSelectCheckedFieldGG) && (TotalCheckedCheckBoxes > 0) && (!ch))
                {
                    MessageBox.Show("Множественный выбор для данного справочника запрещен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cel.Value = false;
                    TotalCheckedCheckBoxes = 1;
                    Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }


                ch = !ch;
                cel.Value = ch;

                //перемещение по гриду, для того что бы комбо-бокс прорисовался
                if (this.CurrentRow.Index < this.RowCount - 1)
                    OnKeyDown(new KeyEventArgs(Keys.Down));
                else
                {                    //поледня строка, перейду вверх - вниз
                    OnKeyDown(new KeyEventArgs(Keys.Up));
                    OnKeyDown(new KeyEventArgs(Keys.Down));
                }

                Cursor = System.Windows.Forms.Cursors.Default;
            }
            else base.OnKeyUp(e);
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((!IsHeaderCheckBoxClicked) && (this.Columns.Contains("IsCheckedField")))
                RowCheckBoxClick((DataGridViewCheckBoxCell)this[e.ColumnIndex, e.RowIndex]);
        }

        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if ((this.IsCurrentCellDirty) && (this.CurrentCell is DataGridViewCheckBoxCell) && (this.Columns.Contains("IsCheckedField")))
            {
                
                bool ch = false;
                DataGridViewCheckBoxCell cel = (DataGridViewCheckBoxCell)(this.CurrentRow.Cells["IsCheckedField"]);
                if (cel.Value != null)
                {
                    if ((cel.Value.ToString() == "") || (cel.Value.ToString() == "false"))
                        ch = false;
                    else
                        ch = (bool)(cel.Value);
                }
                else ch = false;


                if ((!IsMultiSelectCheckedFieldGG) && (TotalCheckedCheckBoxes > 0) && (!ch))
                {
                    MessageBox.Show("Множественный выбор для данного справочника запрещен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.CurrentCell.Value = false;
                    TotalCheckedCheckBoxes = 1;
                    this.CancelEdit();
                    Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }

            }
          
            //иначе фиксируем галочку, для множестенного выбора
            if ((this.CurrentCell is DataGridViewCheckBoxCell) && (this.Columns.Contains("IsCheckedField")))
                this.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
            if (!(this.Controls.ContainsKey("CheckBox")))
            {
                HeaderCheckBox = new CheckBox();
                HeaderCheckBox.Size = new Size(15, 15);
                this.Controls.Add(HeaderCheckBox);
            }
            else HeaderCheckBox.Checked = false;
        }

        private void ResetHeaderCheckBoxLocation(int ColumnIndex, int RowIndex)
        {
            Rectangle oRectangle = this.GetCellDisplayRectangle(ColumnIndex, RowIndex, true);
            Point oPoint = new Point();
            oPoint.X = oRectangle.Location.X + (oRectangle.Width - HeaderCheckBox.Width) / 2 + 1;
            oPoint.Y = oRectangle.Location.Y + (oRectangle.Height - HeaderCheckBox.Height) / 2 + 1;
            HeaderCheckBox.Location = oPoint;
        }

        private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            if (!IsMultiSelectCheckedFieldGG)
            {
                MessageBox.Show("Множественный выбор для данного справочника запрещен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HCheckBox.Checked = false;
                Cursor = System.Windows.Forms.Cursors.Default;
                return;
            }

            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            IsHeaderCheckBoxClicked = true;
            foreach (DataGridViewRow Row in this.Rows)
                ((DataGridViewCheckBoxCell)Row.Cells["IsCheckedField"]).Value = HCheckBox.Checked;
            this.RefreshEdit();
            TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;
            IsHeaderCheckBoxClicked = false;
            Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if (RCheckBox != null)
            {
                Cursor = System.Windows.Forms.Cursors.WaitCursor;
                bool val = false;
                if (RCheckBox.Value != null)
                    val = (bool)RCheckBox.Value;
                else val = false;

                if (val && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                    HeaderCheckBox.Checked = false;
                else if (TotalCheckedCheckBoxes == TotalCheckBoxes)
                    HeaderCheckBox.Checked = true;
                Cursor = System.Windows.Forms.Cursors.Default;
            }
        }



    }
}

