using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GGPlatform.UserControls
{
    public enum TypeDate
    {
        OneDate = 1,
        BetweenDate = 2
    }

    public partial class DateBetweenEditingControl : UserControl, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;
        private int rowIndex;

        private TypeDate _style = TypeDate.OneDate;

        public DateBetweenEditingControl()
        {
            this.TabStop = false;
            InitializeComponent();
        }
        public TypeDate StyleDate
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                SetBetweenStyle(_style);
            }
        }
        public void SetBetweenStyle(TypeDate inStyle)
        {
            _style = inStyle;
            switch (_style)
            {
                case TypeDate.OneDate:
                    date1.Enabled = true;
                    date2.Enabled = false;

                    panel2.Dock = DockStyle.None;
                    panel2.Visible = false;
                    panel1.Dock = DockStyle.Fill; //заполню все одним ДатеТиме
                    label1.Width = 1; //уберу с экрана

                    break;
                case TypeDate.BetweenDate:
                    date1.Enabled = true;
                    date2.Enabled = true;

                    panel2.Dock = DockStyle.Fill;
                    panel2.Visible = true;
                    panel1.Dock = DockStyle.Left; //делаю видимыми два ДатаТайма
                    label1.Width = 26; //делаю видимыми два ДатаТайма

                    break;
            }
           // MessageBox.Show("set style " + _style.ToString());          
        }
        public string Value
        {
            get
            {
                switch (_style)
                {
                    case TypeDate.OneDate:
                        return date1.Value.ToShortDateString();
                    case TypeDate.BetweenDate:
                        return date1.Value.ToShortDateString() + "/" + date2.Value.ToShortDateString();
                    default:
                        return date1.Value.ToShortDateString() + "/" + date2.Value.ToShortDateString();
                }                
            }
            set
            {
                date2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                date2.CustomFormat = " ";

                switch (_style)
                {
                    case TypeDate.OneDate:
                        if (String.IsNullOrEmpty(value))
                        {
                            date1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                            date1.CustomFormat = " ";
                        }
                        else
                        {
                            date1.Format = System.Windows.Forms.DateTimePickerFormat.Long;
                            date1.Value = Convert.ToDateTime(value);
                        }
                        break;
                    case TypeDate.BetweenDate:
                        if (String.IsNullOrEmpty(value))
                        {
                            date1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                            date1.CustomFormat = " ";

                            date2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                            date2.CustomFormat = " ";
                        }
                        else
                        {
                            if (value.Length == 21)
                            {
                                date1.Format = System.Windows.Forms.DateTimePickerFormat.Long;
                                date1.Value = Convert.ToDateTime(value.Substring(0, 10));

                                date2.Format = System.Windows.Forms.DateTimePickerFormat.Long;
                                date2.Value = Convert.ToDateTime(value.Substring(11, 10));
                            }
                            else
                                throw new Exception("User Control DateBetween Ошибка в формате передаваемых данных");
                        }
                        break;
                }
            }
        }

        private void date_Changed(object sender, EventArgs e)
        {
            if ((sender == date1) && ((date1.Format == System.Windows.Forms.DateTimePickerFormat.Custom)))
                date1.Format = System.Windows.Forms.DateTimePickerFormat.Long;

            if ((sender == date2) && ((date2.Format == System.Windows.Forms.DateTimePickerFormat.Custom)))
                date2.Format = System.Windows.Forms.DateTimePickerFormat.Long;

            this.valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        /////////////////////////////////////////////
        public virtual DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }

        public virtual object EditingControlFormattedValue
        {
            get
            {
                return GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting);
            }
            set
            {
                this.Value = (string)value;
            }
        }

        public virtual int EditingControlRowIndex
        {
            get
            {
                return this.rowIndex;
            }
            set
            {
                this.rowIndex = value;
            }
        }

        public virtual bool EditingControlValueChanged
        {
            get
            {
                return this.valueChanged;
            }
            set
            {
                this.valueChanged = value;
            }
        }

        public virtual Cursor EditingPanelCursor
        {
            get
            {
                return Cursors.Default;
            }
        }

        public virtual bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public virtual void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
        }

        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Tab:
                case Keys.Enter:
                    dataGridViewWantsInputKey = true;
                    return false;
                default:
                    dataGridViewWantsInputKey = false;
                    return true;

            }
        }

        public virtual object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Value;
        }

        public virtual void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        private void NotifyDataGridViewOfValueChange()
        {
            if (!this.valueChanged)
            {
                this.valueChanged = true;
                this.dataGridView.NotifyCurrentCellDirty(true);
            }
        }

        private void DateBetweenEditingControl_Resize(object sender, EventArgs e)
        {
            int _width = (this.Width / 2);
            panel1.Width = _width;
        }

        //protected override void OnTextChanged(EventArgs e)
        //{
        //    this.valueChanged = true;
        //    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        //    base.OnTextChanged(e);
        //}

        //private void date1_ValueChanged(object sender, EventArgs e)
        //{

        //}
    }
}
