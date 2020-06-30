using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace GGPlatform.UserControls
{
    public partial class ReferenceEditingControl: UserControl, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;
        private int rowIndex;
      //  private string _CODEs = "";
        private DBSqlServer _DBSql;
        private Int32 _ReferenceID;
        frmReference fmReference = null;

        public ReferenceEditingControl()
        {
            this.TabStop = false;
            InitializeComponent();
        }

        [Category("Appearance"), Description("Gets or sets Reference Value"), DisplayName("ReferenceValue")]
        public string Value
        {
            get { return tbEdit.Text; }
            set 
            { 
              //  _CODEs = value;
                tbEdit.Text = value;
            }
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
        public DBSqlServer DBSqlServer
        {
            set
            {
                _DBSql = value;
            }
        }

        private void tbEdit_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (fmReference == null)
                fmReference = new frmReference();
            fmReference.ReferenceCODEs = tbEdit.Text;
            fmReference.ReferenceDBSqlServer = _DBSql;
            fmReference.ReferenceID = _ReferenceID;
            if (fmReference.ShowDialog() == DialogResult.OK)
            {
                tbEdit.Text = fmReference.ReferenceCODEs;
              //  _CODEs = fmReference.ReferenceCODEs;
            }
         //   fmReference.Dispose();
         //   fmReference = null;
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
 
        protected override void OnTextChanged(EventArgs e)
        {
            this.valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }

    }
}