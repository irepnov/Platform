using GGPlatform.DBServer;
using System;
using System.Data;
using System.Windows.Forms;

namespace GGPlatform.AdresManag
{
    internal partial class frmAdres : Form
    {
        public DBSqlServer _dbSql { get; set; }
        public AdresInfo Adreses { get; set; }

        public frmAdres()
        {
            InitializeComponent();
        }

        public void ClearCLB(ref ComboBox inCLB)
        {
            inCLB.DataSource = null;
            inCLB.Items.Clear();
            inCLB.Text = "";

            CreateAdres();
        }

        private void LoadCLB(DataTable inObjects, ref ComboBox inCLB)
        {
            if (inObjects == null)
                return;

            #region clear combo
            switch (inCLB.Name.ToUpper())
            {
               case "CBREGION":
                {
                    ClearCLB(ref cbRaion);
                    ClearCLB(ref cbGorod);
                    ClearCLB(ref cbNaspunkt);
                    ClearCLB(ref cbStreet);

                    tbHouse.Text = "";
                    tbKor.Text = "";
                    tbKvart.Text = "";
                    break;
                }
                case "CBRAION":
                    {
                        ClearCLB(ref cbGorod);
                        ClearCLB(ref cbNaspunkt);
                        ClearCLB(ref cbStreet);

                        tbHouse.Text = "";
                        tbKor.Text = "";
                        tbKvart.Text = "";
                        break;
                    }
                case "CBGOROD":
                    {
                        ClearCLB(ref cbNaspunkt);
                        ClearCLB(ref cbStreet);

                        tbHouse.Text = "";
                        tbKor.Text = "";
                        tbKvart.Text = "";
                        break;
                    }
                case "CBNASPUNKT":
                    {
                        ClearCLB(ref cbStreet);

                        tbHouse.Text = "";
                        tbKor.Text = "";
                        tbKvart.Text = "";
                        break;
                    }
                case "CBSTREET":
                    {
                        tbHouse.Text = "";
                        tbKor.Text = "";
                        tbKvart.Text = "";
                        break;
                    }
            }
            #endregion

            inCLB.DataSource = null;
            inCLB.Items.Clear();
            inCLB.Text = "";
            inCLB.ValueMember = "ID";
            inCLB.DisplayMember = "Name";
            inCLB.DataSource = inObjects;
            inCLB.SelectedValue = DBNull.Value;
        }

        private string GetCheckedCLB(ref ComboBox inCLB)
        {
            if ((inCLB.SelectedValue == null) || (inCLB.SelectedValue == DBNull.Value))
                return null;
            else return inCLB.SelectedValue.ToString();
        }

        private void SetCheckedCLB(object inObjects, ref ComboBox inCLB)
        {
            inCLB.SelectedValue = inObjects;
        }

        private void frmAdres_Load(object sender, EventArgs e)
        {

        }

        private void cbRegion_Enter(object sender, EventArgs e)
        {
            RegionLoad();
            CreateAdres();
        }

        private void RegionLoad()
        {
            _dbSql.SQLScript = "SELECT r.IDR as ID, r.NAME +' '+a.SNAME AS NAME FROM kldREGION r INNER JOIN kldABBR a ON r.IDA=a.IDA ORDER BY r.NAME";
            LoadCLB(_dbSql.FillDataSet().Tables[0], ref cbRegion);
        }
        private void RaionLoad()
        {
            _dbSql.SQLScript = "SELECT ra.IDD as ID, ra.NAME +' '+a.SNAME AS NAME FROM kldRAION ra INNER JOIN kldABBR a ON ra.IDA=a.IDA WHERE ra.IDR = @id ORDER BY ra.NAME";
            _dbSql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbRegion));
            LoadCLB(_dbSql.FillDataSet().Tables[0], ref cbRaion);
        }
        private void GorodLoad()
        {
            _dbSql.SQLScript = "SELECT g.INDEX_KLD, g.IDC as ID, a.SNAME, g.NAME +' '+a.SNAME AS NAME FROM kldGOROD g INNER JOIN kldABBR a ON g.IDA=a.IDA WHERE g.IDD = @id ORDER BY g.NAME";
            _dbSql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbRaion));
            LoadCLB(_dbSql.FillDataSet().Tables[0], ref cbGorod);
        }
        private void NaspunktLoad()
        {
            _dbSql.SQLScript = "SELECT n.INDEX_KLD, n.IDN as ID, a.SNAME, n.NAME +' '+a.SNAME AS NAME FROM kldNASPUNKT n INNER JOIN kldABBR a ON n.IDA=a.IDA WHERE n.IDC = @id ORDER BY n.NAME";
            _dbSql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbGorod));
            LoadCLB(_dbSql.FillDataSet().Tables[0], ref cbNaspunkt);
        }
        private void StreetLoad()
        {
            _dbSql.SQLScript = "SELECT s.IDS as ID, s.NAME +' '+a.SNAME AS NAME FROM kldSTREET s INNER JOIN kldABBR a ON s.IDA=a.IDA WHERE IDN = @id ORDER BY s.NAME";
            _dbSql.AddParameter("@id", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbNaspunkt));
            LoadCLB(_dbSql.FillDataSet().Tables[0], ref cbStreet);
        }

        private void CreateAdres()
        {
            tbAdres.Text = "";
            string _region = "";
            string _raion = "";
            string _gorod = "";
            string _naspunkt = "";
            string _street = "";
            string _house = "";
            string _korp = "";
            string _kvart = "";

            if ((cbRegion.Text.Trim() != "") && (cbRegion.Text.Trim().ToUpper() != "0ТСУТСТВУЕТ")) _region = cbRegion.Text.Trim();
            if ((cbRaion.Text.Trim() != "") && (cbRaion.Text.Trim().ToUpper() != "0ТСУТСТВУЕТ")) _raion = cbRaion.Text.Trim();
            if ((cbGorod.Text.Trim() != "") && (cbGorod.Text.Trim().ToUpper() != "0ТСУТСТВУЕТ")) _gorod = cbGorod.Text.Trim();
            if ((cbNaspunkt.Text.Trim() != "") && (cbNaspunkt.Text.Trim().ToUpper() != "0ТСУТСТВУЕТ")) _naspunkt = cbNaspunkt.Text.Trim();
            if ((cbStreet.Text.Trim() != "") && (cbStreet.Text.Trim().ToUpper() != "0ТСУТСТВУЕТ")) _street = cbStreet.Text.Trim();

            if (tbHouse.Text.Trim() != "") _house = "д." + tbHouse.Text.Trim();
            if (tbKor.Text.Trim() != "") _korp = "кор." + tbKor.Text.Trim();
            if (tbKvart.Text.Trim() != "") _kvart = "кв." + tbKvart.Text.Trim();

            tbAdres.Text = _region + ", " + _raion + ", " + _gorod + ", " + _naspunkt + ", " + _street + ", " + _house + ", " + _korp + ", " + _kvart;

            if (tbAdres.Text.Replace(" ", "") == ",,,,,,,")
                tbAdres.Text = "";
        }

        private void SetAdresInfo()
        {
            if (Adreses == null)
                throw new Exception("Не задан объект описания адреса");

            if (tbAdres.Text == "")
            {
                Adreses.Region = -1;
                Adreses.Raion = -1;
                Adreses.Gorod = -1;
                Adreses.Naspunkt = -1;
                Adreses.Street = -1;
                Adreses.House = "";
                Adreses.Korp = "";
                Adreses.Kvart = "";
                Adreses.Title = "";
                return;
            } else
            {
                Adreses.Title = tbAdres.Text;
                Adreses.Region = Convert.ToInt32(GetCheckedCLB(ref cbRegion));
                Adreses.Raion = Convert.ToInt32(GetCheckedCLB(ref cbRaion));
                Adreses.Gorod = Convert.ToInt32(GetCheckedCLB(ref cbGorod));

                if (cbNaspunkt.Text != "")
                    if (GetCheckedCLB(ref cbNaspunkt) == null)
                        Adreses.Naspunkt = -1;
                    else
                        Adreses.Naspunkt = Convert.ToInt32(GetCheckedCLB(ref cbNaspunkt));

                if (cbStreet.Text != "")
                    if (GetCheckedCLB(ref cbStreet) == null)
                        Adreses.Street = -1;
                    else
                        Adreses.Street = Convert.ToInt32(GetCheckedCLB(ref cbStreet));

                Adreses.House = tbHouse.Text;
                Adreses.Korp = tbKor.Text;
                Adreses.Kvart = tbKvart.Text;
                return; 
            }
        }

        private void cbRaion_Enter(object sender, EventArgs e)
        {
            if (GetCheckedCLB(ref cbRegion) == "")
            {
                cbRegion.Focus();
            }
            RaionLoad();
            CreateAdres();
        }

        private void cbGorod_Enter(object sender, EventArgs e)
        {
            if (GetCheckedCLB(ref cbRaion) == "")
            {
                cbRaion.Focus();
            }
            GorodLoad();
            CreateAdres();
        }

        private void cbNaspunkt_Enter(object sender, EventArgs e)
        {
            if (GetCheckedCLB(ref cbGorod) == "")
            {
                cbGorod.Focus();
            }
            NaspunktLoad();
            CreateAdres();
        }

        private void cbStreet_Enter(object sender, EventArgs e)
        {
            if (GetCheckedCLB(ref cbNaspunkt) == "")
            {
                cbNaspunkt.Focus();
            }
            StreetLoad();
            CreateAdres();
        }

        private void cbRegion_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void cbRaion_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void cbGorod_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void cbNaspunkt_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void cbStreet_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void tbHouse_Leave(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void cbRegion_MouseClick(object sender, MouseEventArgs e)
        {
           // CreateAdres();
        }

        private void cbRegion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CreateAdres();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            SetAdresInfo();
            this.DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAdres_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAdres_Shown(object sender, EventArgs e)
        {
            if (Adreses.Region != -1)
            {
                RegionLoad();
                SetCheckedCLB(Adreses.Region, ref cbRegion);
            }
            if (Adreses.Raion != -1)
            {
                RaionLoad();
                SetCheckedCLB(Adreses.Raion, ref cbRaion);
            }
            if (Adreses.Gorod != -1)
            {
                GorodLoad();
                SetCheckedCLB(Adreses.Gorod, ref cbGorod);
            }

            NaspunktLoad();
            if (Adreses.Naspunkt != -1)
                SetCheckedCLB(Adreses.Naspunkt, ref cbNaspunkt);

            StreetLoad();
            if (Adreses.Street != -1)
                SetCheckedCLB(Adreses.Street, ref cbStreet);

            tbHouse.Text = Adreses.House;
            tbKor.Text = Adreses.Korp;
            tbKvart.Text = Adreses.Kvart;

          //  tbAdres.Text = Adreses.Title;
            btOK.Focus();
        }
    }
}
