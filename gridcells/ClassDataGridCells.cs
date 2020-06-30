using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.UserControls;


namespace GGPlatform.GridCells
{
    /// <summary>
    /// DataGridViewReferenceColumn
    /// </summary>
    public class DataGridViewReferenceColumn : DataGridViewColumn
    {
       // private static DBSqlServer _DBSql = null;
       // private static Int32 _ReferenceID = 0;

        public DataGridViewReferenceColumn(DBSqlServer inDBSqlServer, Int32 inReferenceID)
        {
            this.CellTemplate = new DataGridViewReferenceCell(inDBSqlServer, inReferenceID);
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewReferenceCell)))
                {
                    throw new InvalidCastException("Ячейка должна быть типа DataGridViewReferenceCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class DataGridViewReferenceCell : DataGridViewTextBoxCell
    {
        private DBSqlServer _DBSql;
        private Int32 _ReferenceID;

        public DataGridViewReferenceCell(DBSqlServer inDBSqlServer, Int32 inReferenceID)
        {
            _DBSql = inDBSqlServer;
            _ReferenceID = inReferenceID;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            ReferenceEditingControl ctl = DataGridView.EditingControl as ReferenceEditingControl;
            ctl.ReferenceID = _ReferenceID;
            ctl.DBSqlServer = _DBSql;
            if (this.Value == null)
            {
                ctl.Value = (string)this.DefaultNewRowValue;
            }
            else
            {
                ctl.Value = (string)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(ReferenceEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
    }


    /// <summary>
    /// DataGridViewSubQueryColumn
    /// </summary>
    public class DataGridViewSubQueryColumn : DataGridViewColumn
    {
       // private static DBSqlServer _DBSql = null;
       // private static Int32 _ObjectID = 0;

        public DataGridViewSubQueryColumn(DBSqlServer inDBSqlServer, Int32 inObjectID)
        {
            this.CellTemplate = new DataGridViewReferenceCell(inDBSqlServer, inObjectID);
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewSubQueryCell)))
                {
                    throw new InvalidCastException("Ячейка должна быть типа DataGridViewSubQueryCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class DataGridViewSubQueryCell : DataGridViewTextBoxCell
    {
        private DBSqlServer _DBSql;
        private Int32 _ObjectID;

        public DataGridViewSubQueryCell(DBSqlServer inDBSqlServer, Int32 inObjectID)
        {
            _DBSql = inDBSqlServer;
            _ObjectID = inObjectID;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            SubQueryEditingControl ctl = DataGridView.EditingControl as SubQueryEditingControl;
            ctl.ObjectID = _ObjectID;
            ctl.DBSqlServer = _DBSql;
            if (this.Value == null)
            {
                ctl.Value = (string)this.DefaultNewRowValue;
            }
            else
            {
                ctl.Value = (string)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(SubQueryEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
    }




    /// <summary>
    /// DataGridViewDateBetweenColumn
    /// </summary>
    public class DataGridViewDateBetweenColumn : DataGridViewColumn
    {
        public DataGridViewDateBetweenColumn(TypeDate inStyleBetween)
        {
            this.CellTemplate = new DataGridViewDateBetweenCell(inStyleBetween);
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewDateBetweenCell)))
                {
                    throw new InvalidCastException("Ячейка должна быть типа DataGridViewDateBetweenCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class DataGridViewDateBetweenCell : DataGridViewTextBoxCell
    {
        private TypeDate _style = TypeDate.OneDate;
        public DataGridViewDateBetweenCell(TypeDate inStyleBetween)
        {
            _style = inStyleBetween;
        }

        public TypeDate StyleBetweenDate
        {
            set
            {
                _style = value;
            }
            get
            {
                return _style;
            }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DateBetweenEditingControl ctl = DataGridView.EditingControl as DateBetweenEditingControl;

            ctl.StyleDate = _style;

           // MessageBox.Show("initial style " + ctl.StyleDate.ToString());

            if (this.Value == null)
            {
                ctl.Value = (string)this.DefaultNewRowValue;
            }
            else
            {
                ctl.Value = (string)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(DateBetweenEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return null;
            }
        }
    }






    /// <summary>
    /// DataGridViewCalendarColumn
    /// </summary>
    public class DataGridViewCalendarColumn : DataGridViewColumn
    {
        public DataGridViewCalendarColumn()
            : base(new DataGridViewCalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell. 
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
                {
                    throw new InvalidCastException("Ячейка должна быть DataGridViewCalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class DataGridViewCalendarCell : DataGridViewTextBoxCell
    {

        public DataGridViewCalendarCell()
            : base()
        {
            // Use the short date format. 
            this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value. 
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewCalendarEditingControl ctl =
                DataGridView.EditingControl as DataGridViewCalendarEditingControl;
            // Use the default row value when Value property is null. 
            if (this.Value == null)
            {
                ctl.Value = (DateTime)this.DefaultNewRowValue;
            }
            else
            {
                ctl.Value = (DateTime)this.Value;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses. 
                return typeof(DataGridViewCalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains. 

                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value. 
                return DateTime.Now;
            }
        }
    }
    class DataGridViewCalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public DataGridViewCalendarEditingControl()
        {
            this.Format = DateTimePickerFormat.Short;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue  
        // property. 
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is  
                        // null, empty, or not in the format of a date. 
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch
                    {
                        // In the case of an exception, just use the  
                        // default value so we're not left with a null 
                        // value. 
                        this.Value = DateTime.Now;
                    }
                }
            }
        }

        // Implements the  
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method. 
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the  
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method. 
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex  
        // property. 
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey  
        // method. 
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed. 
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit  
        // method. 
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl 
        // .RepositionEditingControlOnValueChange property. 
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlDataGridView property. 
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingControlValueChanged property. 
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl 
        // .EditingPanelCursor property. 
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell 
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}
