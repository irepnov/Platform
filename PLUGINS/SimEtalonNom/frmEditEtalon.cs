using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.DBServer;

namespace SimEtalonNom
{
    public partial class frmEditEtalon : Form
    {
        private Int32 _ID = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        private int _typeNac = 2;
        public frmEditEtalon(DataRow inDR, Int32 _inID, DBSqlServer _inDB, string _inTypeNacenka)
        {
            InitializeComponent();

            _dr = inDR;
            _ID = _inID;
            _dbsql = _inDB;

            if (_inTypeNacenka == "PRICEPOSTAV")
                _typeNac = 1;
            if (_inTypeNacenka == "SEBESTFACT")
                _typeNac = 2;

            tbProiz.Text = _dr["Proizv"].ToString();
            tbName.Text = _dr["Name"].ToString();
            chbActiv.Checked = (bool)_dr["isActive"];
            tbType.Text = _dr["TypeP"].ToString();
            tbOS.Text = _dr["OS"].ToString();
            tbDisplayType.Text = _dr["DisplayType"].ToString();
            tbDisplaySize.Text = _dr["DisplaySize"].ToString();
            tbSDCard.Text = _dr["SDCARD"].ToString();
            tbCameraBasic.Text = _dr["CameraBasic"].ToString();
            tbCameraSecond.Text = _dr["CameraSecond"].ToString();
            tbROM.Text = _dr["ROM"].ToString();
            tbCPU.Text = _dr["CPU"].ToString();
            tbRAM.Text = _dr["RAM"].ToString();
            tbMP.Text = _dr["mp3_rad"].ToString();
            tbJAVA.Text = _dr["java_mms"].ToString();
            tbBluth.Text = _dr["bluth_wifi"].ToString();
            tbStand.Text = _dr["stand"].ToString();
            tbNavi.Text = _dr["navi"].ToString();
            tbAKB.Text = _dr["akum"].ToString();
            tbRRC.Text = _dr["rrc"].ToString();
            tbNewRozn.Text = _dr["NewRozn"].ToString();
        }

        public bool UserCheck()
        {
            bool _result = false;
            try
            {
                _result = true;
                if (tbName.Text == "")
                {
                    ActiveControl = tbName;
                    throw new Exception("Необходимо указать наименование");
                }
                if (tbProiz.Text == "")
                {
                    ActiveControl = tbProiz;
                    throw new Exception("Необходимо указать производителя");
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (!UserCheck())
                return;
            try
            {
                btOK.Enabled = false;
                if (_ID > 0)
                {
                    _dbsql.SQLScript = @"exec uchEtalonNomenc @IDOperation = 4, @ID = @id, @Proizv = @pr, @Name = @n, @isActive = @a, @TypeP = @t, @OS = @o, 
                                              @DisplayType = @dt, @DisplaySize = @ds, @SDCARD = @sd, @CameraBasic = @cb, @CameraSecond = @cs, 
                                              @ROM = @ro, @CPU = @cp, @RAM = @ra, @mp3_rad = @mp, @java_mms = @j, @bluth_wifi = @b, @stand = @sta, @navi = @na, @akum = @ak, @rrc = @rr, @NewRozn = @NewRozn, @TypeNacenka = @typ";
                    _dbsql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, _ID.ToString());
                    _dbsql.AddParameter("@pr", EnumDBDataTypes.EDT_String, tbProiz.Text);
                    _dbsql.AddParameter("@n", EnumDBDataTypes.EDT_String, tbName.Text);
                    _dbsql.AddParameter("@a", EnumDBDataTypes.EDT_Bit, chbActiv.Checked.ToString());
                    _dbsql.AddParameter("@t", EnumDBDataTypes.EDT_String, tbType.Text);
                    _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, tbOS.Text);
                    _dbsql.AddParameter("@dt", EnumDBDataTypes.EDT_String, tbDisplayType.Text);
                    _dbsql.AddParameter("@ds", EnumDBDataTypes.EDT_String, tbDisplaySize.Text);
                    _dbsql.AddParameter("@sd", EnumDBDataTypes.EDT_String, tbSDCard.Text);
                    _dbsql.AddParameter("@cb", EnumDBDataTypes.EDT_String, tbCameraBasic.Text);
                    _dbsql.AddParameter("@cs", EnumDBDataTypes.EDT_String, tbCameraSecond.Text);
                    _dbsql.AddParameter("@ro", EnumDBDataTypes.EDT_String, tbROM.Text);
                    _dbsql.AddParameter("@cp", EnumDBDataTypes.EDT_String, tbCPU.Text);
                    _dbsql.AddParameter("@ra", EnumDBDataTypes.EDT_String, tbRAM.Text);
                    _dbsql.AddParameter("@mp", EnumDBDataTypes.EDT_String, tbMP.Text);
                    _dbsql.AddParameter("@j", EnumDBDataTypes.EDT_String, tbJAVA.Text);
                    _dbsql.AddParameter("@b", EnumDBDataTypes.EDT_String, tbBluth.Text);
                    _dbsql.AddParameter("@sta", EnumDBDataTypes.EDT_String, tbStand.Text);
                    _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbNavi.Text);
                    _dbsql.AddParameter("@ak", EnumDBDataTypes.EDT_Integer, tbAKB.Text);
                    _dbsql.AddParameter("@rr", EnumDBDataTypes.EDT_Integer, tbRRC.Text);
                    _dbsql.AddParameter("@NewRozn", EnumDBDataTypes.EDT_Decimal, tbNewRozn.Text);
                    _dbsql.AddParameter("@typ", EnumDBDataTypes.EDT_Integer, _typeNac.ToString());
                    _dbsql.ExecuteNonQuery();
                }
                btOK.Enabled = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                btOK.Enabled = true;
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbAKB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8) && e.KeyChar != Convert.ToChar(44) && e.KeyChar != Convert.ToChar(46))
            {
                e.Handled = true;
            }
        }
    }
}
