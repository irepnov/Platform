using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using GGPlatform.GridCells;
using GGPlatform.DBServer;
using System.IO;

namespace GGPlatform.QueryBuilder
{
    public partial class frmQueryBuilder : Form
    {
        private ArrayList _sQueryColumns = null;
        private Field _sColumn = null;
        private IWin32Window _sMainHandle = null;
        private DBSqlServer _sDBSql = null;
        private string _sSQLScript = null;
        private TypeReturnScript _sTypeGetScript;

        private string _sFieldType = "";
        private string _sFieldName = "";
        private string _sFieldCaption = "";
        private int _sFieldReference = 0;

        private Icon sortAsc = GGPlatform.QueryBuilder.Properties.Resources.sort_ascending;
        private Icon sortDesc = GGPlatform.QueryBuilder.Properties.Resources.sort_descending;
        public frmQueryBuilder(IWin32Window inMainHandle, ArrayList inQueryColumns, DBSqlServer inDBSql, string inSQLScript, TypeReturnScript inTypeReturnScript = TypeReturnScript.trsScript)
        {
            InitializeComponent();
 
            _sQueryColumns = inQueryColumns;
            _sMainHandle = inMainHandle;
            _sDBSql = inDBSql;
            _sTypeGetScript = inTypeReturnScript;
            if (_sTypeGetScript == TypeReturnScript.trsScript)
            {
                _sSQLScript = "set dateformat dmy \n" + inSQLScript;
            } 
            if (_sTypeGetScript == TypeReturnScript.trsSubQuery)
            {
                _sSQLScript = inSQLScript;
            }
            

            if (!_sDBSql.InfoAboutConnection.UserIsAdmin)
                tabControl1.TabPages.Remove(tabPage2);

            rtbSQL.Text = _sSQLScript;
            
            GenerateFilter();
            SetSize();
        }

        public string generatedSQL 
        { 
            get 
                {
                    return _sSQLScript;
                }
        }
        private void SetSize()
        {
            if (_sQueryColumns.Count > 6)
                this.Size = new Size(this.Width, (_sQueryColumns.Count * 30) + 70);
            this.StartPosition = FormStartPosition.CenterScreen;        
        }
        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (DataGridView.CurrentCellAddress.X == 2) ///если пользователь щелкнул по колонка Условие отбора
            {                
                ComboBox com = (e.Control as ComboBox); //и это точно комбобох, то назначю собыытие на этот комбобох
                if (com != null)
                {
                    e.CellStyle.BackColor = Color.White;  //убираю Черный фон
                    com.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
                    com.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                }
            }
        }
        /// <summary>
        /// изменяет внешний вид Контрола, который используется для ввода даты. в Конкретной строке. 
        /// Если в условии отбора Выбрано условие Диапазоан, то внешний вид будет - Диапазон, иначе - Одиночная дата
        /// </summary>
        /// <param name="inRow"></param>
        private void ChangeDateValueStyle(DataGridViewRow inRow)
        {
            Field fil = inRow.Tag as Field;
            if (fil != null) //для фильтров Дата меняю тип ячейки, Диапазон или Одинарное значение
            {
                if (fil.FieldType.ToUpper() == "D")
                {
                    DataGridViewDateBetweenCell cell3_date = null;
                    string inFilterType = inRow.Cells[2].EditedFormattedValue.ToString();  //значение которое выбрано в КомбоБоксе
                    UserControls.TypeDate inActiveStyle = (inRow.Cells[3] as DataGridViewDateBetweenCell).StyleBetweenDate; //тип котнрола который Был

                    if (
                           ((inFilterType == "в диапазоне от/до") || (inFilterType == "не в диапазоне от/до"))
                        && (inActiveStyle == UserControls.TypeDate.OneDate)
                       )//если Выбран поиск по диапазону, а текущяя ячейка Однинарная, то меняю на диапазон
                    {
                        inRow.Cells[3].Dispose();
                        cell3_date = new DataGridViewDateBetweenCell(UserControls.TypeDate.BetweenDate);
                        cell3_date.StyleBetweenDate = UserControls.TypeDate.BetweenDate;
                    }

                    if (
                           (inFilterType != "в диапазоне от/до") 
                        && (inFilterType != "не в диапазоне от/до")
                        && (inActiveStyle == UserControls.TypeDate.BetweenDate)
                       )//если Выбран поиск НЕ ПО диапазону, а текущяя ячейка Диапазон, то меняю на одинарное
                    {
                        inRow.Cells[3].Dispose();
                        cell3_date = new DataGridViewDateBetweenCell(UserControls.TypeDate.OneDate);
                        cell3_date.StyleBetweenDate = UserControls.TypeDate.OneDate;
                    }

                    if (cell3_date != null)
                    {
                        inRow.Cells[3].Dispose();///удалю прошлый контрол
                        inRow.Cells[3] = cell3_date; //присвою Новый
                    }                     
                }
            }
        }
        void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (DataGridView.CurrentCell.EditedFormattedValue.ToString().Contains("пусто"))            
                DataGridView.Rows[DataGridView.CurrentCellAddress.Y].Cells[3].Value = null;
            DataGridView.Rows[DataGridView.CurrentCellAddress.Y].Cells[5].Value = DataGridView.CurrentCell.EditedFormattedValue.ToString();*/

            ChangeDateValueStyle(DataGridView.CurrentRow);///изменю тип контрола для ввода Даты
        }
        private void DataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {          
            Point po = ((DataGridView)sender).CurrentCellAddress;
         
            if ((po.X == 2) && (po.Y > -1)) //колонка where , если в списке выбрали Пусто, Не пусто или ничего не выбрали, то очищу графу Значение поиска
            {
                if (((DataGridView)sender).CurrentCell is DataGridViewComboBoxCell)
                {
                    DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    DataGridView.EndEdit();

                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)((DataGridView)sender).CurrentCell;
                    string tmp = (c.Value != null ? c.Value.ToString() : "");

                    if ((tmp.Contains("пусто")) || (tmp == "")) //выбрали Пусто, Не пусто или ничего не выбрали
                        DataGridView.Rows[po.Y].Cells[3].Value = null; //то очищу графу Значение поиска
                }
            }
        }
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 1) && (e.RowIndex > - 1))  //sorting column
            {
                DataGridViewImageCell cell = (DataGridViewImageCell)DataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.ReadOnly) return;   ///например для подзапросов
                if (cell.Value == null)
                    cell.Value = sortAsc;
                else
                    if (cell.Value == sortAsc)
                        cell.Value = sortDesc;
                    else
                        cell.Value = null;
            }
        }
        private void GenerateFilter()
        {
            DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            DataGridView.EditMode = DataGridViewEditMode.EditOnEnter;

            DataGridView.EditingControlShowing += DataGridView_EditingControlShowing;  //присоеденю событие отображение контрола

            DataGridView.AllowUserToAddRows = false;
            DataGridView.RowHeadersVisible = false;
            DataGridView.ColumnCount = 4;

            DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);

            DataGridView.Columns[0].HeaderText = "Наименование поля";
            DataGridView.Columns[0].Width = 250;
            DataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            DataGridView.Columns[0].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;
            DataGridView.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            DataGridView.Columns[1].HeaderText = "";
            DataGridView.Columns[1].Width = 20;
            DataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridView.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            DataGridView.Columns[1].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;
            DataGridView.Columns[1].DefaultCellStyle.NullValue = null;
           // DataGridView.Columns[1].HeaderCell.Value = System.Drawing.Image.FromFile("f:\\WORK\\Platform\\ResourceFiles\\sort_ascending.ico");

            DataGridView.Columns[2].HeaderText = "Условие отбора";
            DataGridView.Columns[2].Width = 145;
            DataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            DataGridView.Columns[2].DefaultCellStyle.BackColor = DataGridView.DefaultBackColor;

            DataGridView.Columns[3].HeaderText = "Значение";
            DataGridView.Columns[3].Width = 465;
            DataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            DataGridView.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;

            for (int i = 0; i <= _sQueryColumns.Count - 1; i++)
            {
                _sColumn = (Field)_sQueryColumns[i];  //беру один столбец

                _sFieldType = _sColumn.FieldType.ToString();
                _sFieldCaption = _sColumn.FieldCaption;
                _sFieldName = _sColumn.FieldName;
                _sFieldReference = _sColumn.FieldReference;

                DataGridViewRow row = new DataGridViewRow();
                row.Tag = _sColumn; //сохраню временно для дальнейшего анализа

                DataGridViewTextBoxCell cell0 = new DataGridViewTextBoxCell();
                DataGridViewImageCell cell1 = new DataGridViewImageCell();
                DataGridViewComboBoxCell cell2 = new DataGridViewComboBoxCell();
                DataGridViewTextBoxCell cell3_text = new DataGridViewTextBoxCell();
                DataGridViewReferenceCell cell3_ref = new DataGridViewReferenceCell(_sDBSql, _sFieldReference);
                DataGridViewSubQueryCell cell3_sub = new DataGridViewSubQueryCell(_sDBSql, _sFieldReference);
                DataGridViewDateBetweenCell cell3_date = new DataGridViewDateBetweenCell(UserControls.TypeDate.OneDate); //по умолчанию одинарная дата
                DataGridViewComboBoxCell cell3_com = new DataGridViewComboBoxCell();

                cell0.Value = _sFieldCaption;

                if (_sFieldType == "D")
                {
                    row.Cells.Add(cell0);
                    row.Cells.Add(cell1);
                    row.Cells.Add(cell2);
                    row.Cells.Add(cell3_date);
                }
                else
                    if ((_sFieldType == "N") || /*(_sFieldType == "L") ||*/ (_sFieldType == "C"))
                    {
                        row.Cells.Add(cell0);
                        row.Cells.Add(cell1);
                        row.Cells.Add(cell2);
                        row.Cells.Add(cell3_text);
                    }
                    else
                        if (_sFieldType == "R")  ////не работает, т.к. Тип Стринг воспринимает
                        {
                            row.Cells.Add(cell0);
                            row.Cells.Add(cell1);
                            row.Cells.Add(cell2);
                            row.Cells.Add(cell3_ref);
                        }
                        else
                            if (_sFieldType == "Q")  ////не работает, т.к. Тип Стринг воспринимает
                            {
                                row.Cells.Add(cell0);
                                row.Cells.Add(cell1);
                                row.Cells.Add(cell2);
                                row.Cells.Add(cell3_sub);
                            }
                            else
                                if (_sFieldType == "L")  ////не работает, т.к. Тип Стринг воспринимает
                                {
                                    row.Cells.Add(cell0);
                                    row.Cells.Add(cell1);
                                    row.Cells.Add(cell2);
                                    row.Cells.Add(cell3_com);
                                }

                if (_sFieldType == "N")
                {
                    cell2.Items.AddRange(new string[] { "", "равно", "не равно", "больше", "больше или равно", "меньше", "меньше или равно", "в диапазоне от/до", "не в диапазоне от/до", "пусто", "не пусто" });
                }
                else
                    if (_sFieldType == "D")
                    {
                        cell2.Items.AddRange(new string[] { "", "равно", "не равно", "больше", "больше или равно", "меньше", "меньше или равно", "в диапазоне от/до", "не в диапазоне от/до", "пусто", "не пусто" });
                    }
                    else
                        if (_sFieldType == "L")
                        {
                            cell2.Items.AddRange(new string[] { "", "равно", "не равно", "пусто", "не пусто" });
                            cell3_com.Items.AddRange(new string[] { "", "Да", "Нет" });
                        }
                        else
                            if (_sFieldType == "C")
                            {
                                cell2.Items.AddRange(new string[] { "", "равно", "не равно", "содержит", "не содержит", "начинается с", "не начинается с", "заканчивается на", "не заканчивается на", "в диапазоне от/до", "не в диапазоне от/до", "пусто", "не пусто" });
                            }
                            else
                                if (_sFieldType == "R")
                                {
                                    cell2.Items.AddRange(new string[] { "", "из перечня", "не из перечня", "пусто", "не пусто" });
                                }
                                else
                                    if (_sFieldType == "Q")
                                    {
                                        cell2.Items.AddRange(new string[] { "", "содержит список", "не содержит список" });
                                        cell1.ReadOnly = true;
                                    }

                cell0.ReadOnly = true;
                DataGridView.Rows.Add(row);
            }        
        }


        private string GetBetweenValue(string inValue, string inType)
        {
            string Value = inValue.Trim();
            int PosDelimeter = Value.IndexOf("/") + 1;  //учитываю /
            int LenValue = Value.Length;

            if ((inType == "D") || (inType == "C"))
            {
                return Value.Substring(0, PosDelimeter - 1) +
                       "' and '" +
                       Value.Substring(PosDelimeter, LenValue - PosDelimeter);
            }

            if (inType == "N")
            {
                return Value.Substring(0, PosDelimeter - 1) +
                       " and " +
                       Value.Substring(PosDelimeter, LenValue - PosDelimeter);
            }
            else return "";
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (GenerateSQL())
            {
                _sSQLScript = rtbSQL.Text;
                this.DialogResult = DialogResult.OK;
            };
        }
        private bool CheckUser()
        {
            for (int i = 0; i <= (DataGridView.RowCount - 1); i++)
            {
                string _where = "";
                string _value = "";
                string _type = "";

                _type = ((Field)_sQueryColumns[i]).FieldType.ToString();
                if (DataGridView.Rows[i].Cells[2].Value != null) 
                    _where = DataGridView.Rows[i].Cells[2].Value.ToString();
                if (DataGridView.Rows[i].Cells[3].Value != null)
                    _value = DataGridView.Rows[i].Cells[3].Value.ToString();
                if ( (_where == "") & (_value != "") ) return true;
                if ( (_where != "") & (_value == "") & (_where != "пусто") & (_where != "не пусто") ) return true;
                if ( ((_where == "пусто") || (_where == "не пусто")) & (_value != "") ) return true;

                if ( (_where == "в диапазоне от/до") & (_value.IndexOf("/") == -1) ) return true;
                if ( (_where == "не в диапазоне от/до") & (_value.IndexOf("/") == -1) ) return true;

                if ((_type == "D") & (_value != ""))
                {
                    _value = _value.Replace(",", ".").Replace("б", ".").Replace("ю", ".");
                    DataGridView.Rows[i].Cells[3].Value = _value;
                }

                if ( (_type == "D") & (_where != "в диапазоне от/до") & (_where != "не в диапазоне от/до") & (_value.IndexOf("/") > -1) ) return true;
                if ( (_type == "N") & (_where != "в диапазоне от/до") & (_where != "не в диапазоне от/до") & (_value.IndexOf("/") > -1) ) return true;

                if ( (_type == "D") & (_value != "") & (_value.Length != 10) & (_where != "в диапазоне от/до") & (_where != "не в диапазоне от/до") & (_where != "не пусто") & (_where != "пусто") ) return true;
                if ( (_type == "D") & (_value != "") & (_value.Length != 21) & ((_where == "в диапазоне от/до") || (_where == "не в диапазоне от/до")) ) return true;
            }
            return false; 
        }
        private string GenerateWhere()
        {
            string _sSQLWhere = " where ";
            for (int i = 0; i <= (DataGridView.RowCount - 1); i++)
            {
                _sFieldName = "";
                _sFieldType = "";
                string _sWhere = "";
                string _sValue = "";
                string _sValueTmp = "";

                _sFieldType = ((Field)_sQueryColumns[i]).FieldType.ToString();
                _sFieldName = ((Field)_sQueryColumns[i]).FieldName;

                if (_sFieldType == "D")
                    _sFieldName = "DATEADD(day, 0, DATEDIFF(day, 0, " + _sFieldName + "))"; //если поле типа Дата, то обрежу время

                if (DataGridView.Rows[i].Cells[2].Value != null)
                   _sWhere = DataGridView.Rows[i].Cells[2].Value.ToString();

                if (DataGridView.Rows[i].Cells[3].Value != null)
                {
                    if ((_sFieldType == "D") || (_sFieldType == "C"))
                    {
                        _sValue = "'" + DataGridView.Rows[i].Cells[3].Value.ToString() + "'";
                    } else
                        if (_sFieldType == "N")
                        { 
                            _sValue = DataGridView.Rows[i].Cells[3].Value.ToString().Replace(",", "."); 
                        } else
                            if (_sFieldType == "L")
                            {
                                _sValueTmp = DataGridView.Rows[i].Cells[3].Value.ToString().Replace(",", ".");
                                if (_sValueTmp.ToUpper() == "ДА")
                                    _sValue = "1";
                                else
                                        if (_sValueTmp.ToUpper() == "НЕТ") 
                                            _sValue = "0";
                                        else
                                            _sValue = _sValueTmp;
                            }
                                else _sValue = DataGridView.Rows[i].Cells[3].Value.ToString();
                }

                if ((_sValue == "") && ((_sWhere == "пусто") || (_sWhere == "не пусто")))
                {
                    if (_sWhere == "пусто")
                    {
                        _sSQLWhere += "(" + _sFieldName + " is Null " + ") and ";
                    }
                    else
                        if (_sWhere == "не пусто")
                        {
                            _sSQLWhere += "(" + _sFieldName + " is not Null " + ") and ";
                        }
                }

                if ((_sValue != "") && (_sWhere != "")) ///если указано значение Поиска и Условие
                {
                    if (_sWhere == "равно")  ////where
                    {
                        _sSQLWhere += "(" + _sFieldName + " = " + _sValue + ") and ";
                    }
                    else
                        if (_sWhere == "не равно")
                        {
                            _sSQLWhere += "(" + _sFieldName + " <> " + _sValue + ") and ";
                        }
                        else
                            if (_sWhere == "больше")
                            {
                                _sSQLWhere += "(" + _sFieldName + " > " + _sValue + ") and ";
                            }
                            else
                                if (_sWhere == "больше или равно")
                                {
                                    _sSQLWhere += "(" + _sFieldName + " >= " + _sValue + ") and ";
                                }
                                else
                                    if (_sWhere == "меньше")
                                    {
                                        _sSQLWhere += "(" + _sFieldName + " < " + _sValue + ") and ";
                                    }
                                    else
                                        if (_sWhere == "меньше или равно")
                                        {
                                            _sSQLWhere += "(" + _sFieldName + " <= " + _sValue + ") and ";
                                        }
                                        else
                                            if (_sWhere == "в диапазоне от/до")
                                            {
                                                _sSQLWhere += "(" + _sFieldName + " between " + GetBetweenValue(_sValue, _sFieldType) + ") and ";
                                            }
                                            else
                                                if (_sWhere == "не в диапазоне от/до")
                                                {
                                                    _sSQLWhere += "(" + _sFieldName + " not between " + GetBetweenValue(_sValue, _sFieldType) + ") and ";
                                                }
                                                else
                                                    if (_sWhere == "содержит")
                                                    {
                                                        _sSQLWhere += "(" + _sFieldName + " like '%" + _sValue.Replace("'", "") + "%') and ";
                                                    }
                                                    else
                                                        if (_sWhere == "не содержит")
                                                        {
                                                            _sSQLWhere += "(" + _sFieldName + " not like '%" + _sValue.Replace("'", "") + "%') and ";
                                                        }
                                                        else
                                                            if (_sWhere == "начинается с")
                                                            {
                                                                _sSQLWhere += "(" + _sFieldName + " like '" + _sValue.Replace("'", "") + "%') and ";
                                                            }
                                                            else
                                                                if (_sWhere == "не начинается с")
                                                                {
                                                                    _sSQLWhere += "(" + _sFieldName + " not like '" + _sValue.Replace("'", "") + "%') and ";
                                                                }
                                                                else
                                                                    if (_sWhere == "заканчивается на")
                                                                    {
                                                                        _sSQLWhere += "(" + _sFieldName + " like '%" + _sValue.Replace("'", "") + "') and ";
                                                                    }
                                                                    else
                                                                        if (_sWhere == "не заканчивается на")
                                                                        {
                                                                            _sSQLWhere += "(" + _sFieldName + " not like '%" + _sValue.Replace("'", "") + "') and ";
                                                                        }
                                                                        else
                                                                            if (_sWhere == "из перечня")
                                                                            {
                                                                                _sSQLWhere += "(" + _sFieldName + " in (" + _sValue + ")) and ";
                                                                            }
                                                                            else
                                                                                if (_sWhere == "не из перечня")
                                                                                {
                                                                                    _sSQLWhere += "(" + _sFieldName + " not in (" + _sValue + ")) and ";
                                                                                }
                                                                                else
                                                                                    if (_sWhere == "содержит список")
                                                                                    {
                                                                                        _sSQLWhere += "(" + _sFieldName + " in (" + _sValue + ")) and ";
                                                                                    }
                                                                                    else
                                                                                        if (_sWhere == "не содержит список")
                                                                                        {
                                                                                            _sSQLWhere += "(" + _sFieldName + " not in (" + _sValue + ")) and ";
                                                                                        }
                                                                                        else
                                                                                        { }
                }
            }

            if (_sSQLWhere == " where ")
                return "";
            else
            {
                _sSQLWhere = "\n" + _sSQLWhere.Substring(0, _sSQLWhere.Length - 4); //уберу последний and
                return _sSQLWhere;
            }
        }
        private string GenerateOrder()
        {
            string _sSQLOrder = " order by ";
            for (int i = 0; i <= (DataGridView.RowCount - 1); i++)
            {
                _sFieldName = "";
                string _sOrder = "";

                _sFieldName = ((Field)_sQueryColumns[i]).FieldName;

                DataGridViewImageCell cell = (DataGridViewImageCell)DataGridView.Rows[i].Cells[1];
                if (cell.Value == sortAsc)
                    _sOrder = "по возрастанию";
                else
                    if (cell.Value == sortDesc)
                        _sOrder = "по убыванию";
                    else
                        _sOrder = "";

                //if (DataGridView.Rows[i].Cells[1].Value != null)
                //    if (DataGridView.Rows[i].Cells[1].Value = )

                //    _sOrder = DataGridView.Rows[i].Cells[1].Value.ToString();

                if (_sOrder != "") ///если указано значение Поиска и Условие
                {
                    if (_sOrder == "по возрастанию") 
                    {
                        _sSQLOrder += _sFieldName + " asc, ";
                    }
                    else
                        if (_sOrder == "по убыванию")
                        {
                            _sSQLOrder += _sFieldName + " desc, ";
                        }
                        else
                            { }
                }
            }
            if (_sSQLOrder == " order by ")
                return "";
            else
            {
                _sSQLOrder = "\n" + _sSQLOrder.Substring(0, _sSQLOrder.Length - 2); //уберу последнюю ,
                return _sSQLOrder;
            }
        }
        private string GenerateSelectFrom()
        {
            string Value = _sSQLScript.Trim();
            int PosDelimeterWhere = Value.IndexOf("where");
            int PosDelimeterOrder = Value.IndexOf("order");

            if (PosDelimeterWhere != -1)
                return Value.Substring(0, PosDelimeterWhere);
            else
                if (PosDelimeterOrder != -1)
                    return Value.Substring(0, PosDelimeterOrder);
                else return Value;
        }
        private Boolean GenerateSQL()
        {
            if (CheckUser())
            {
                MessageBox.Show(_sMainHandle,
                    "Допущена ошибка при указании параметров фильтра",
                    "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            rtbSQL.Clear();                          
            if (_sTypeGetScript == TypeReturnScript.trsScript)
            {
                rtbSQL.Text = GenerateSelectFrom() + GenerateWhere() + GenerateOrder();
            }
            if (_sTypeGetScript == TypeReturnScript.trsSubQuery)
            {
                rtbSQL.Text = GenerateSelectFrom() + GenerateWhere();
            }            

            return true;
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            GenerateSQL();
        }
        private void btClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= (DataGridView.RowCount - 1); i++)
            {
                DataGridView.Rows[i].Cells[1].Value = null;
                DataGridView.Rows[i].Cells[2].Value = null;
                DataGridView.Rows[i].Cells[3].Value = null;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void frmQueryBuilder_Resize(object sender, EventArgs e)
        {
            DataGridView.Columns[3].Width = DataGridView.Width - DataGridView.Columns[0].Width - DataGridView.Columns[1].Width - DataGridView.Columns[2].Width - 20;
        }


    }
}