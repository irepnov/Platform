using System;
using System.Windows.Forms;
using GGPlatform.RegManager;

namespace GGPlatform.DBServer
{
    internal partial class frmDlgConnect : Form
    {
        private IWin32Window _sMainHandle;
        private string _sUsername = "";
        private string _sPassword = "";
        private string _sServername = "";
        private string _sBasename = "";
        private string _sSoftName = "";
        private Boolean _sTrusted = false;

        public string Username
        {
            get { return _sUsername; }
        }
        public string Password
        {
            get { return _sPassword; }
        }
        public string Servername
        {
            get { return _sServername; }
        }
        public string Basename
        {
            get { return _sBasename; }
        }
        public Boolean Trusted
        {
            get { return _sTrusted; }
        }

        public frmDlgConnect(IWin32Window inMainHandle, string inServername, string inBasename, string inSoftName)
        {
            InitializeComponent();
            _sMainHandle = inMainHandle;
            _sServername = inServername;
            _sBasename = inBasename;
            _sSoftName = inSoftName;
        }

        // запись значения в реестр
       /* private void GGRegistrySetValue(string key, object value)
        {
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _sSoftName + "\\" + "dbserver");
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
            RegistryKey currRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\GGPlatform\\" + _sSoftName + "\\" + "dbserver");
            if (currRegistryKey != null)
            {
                val = currRegistryKey.GetValue(key);
                currRegistryKey.Close();
            }
            return val;
        }*/

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbServer.Text == "" || tbBase.Text == "")
            {
                MessageBox.Show(_sMainHandle, 
                                "Не указаны имя сервера или базы данных",
                                "Внимание",
                                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Height = 217;
                btnDB.Tag = 1;
                tbServer.Focus();
                return;
            }
            if (!cbWindows.Checked)
            {
                if (tbLogin.Text == "")
                {
                    MessageBox.Show(_sMainHandle,
                                    "Не указаны учётные данные пользователя",
                                    "Внимание",
                                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    tbLogin.Focus();
                    return;
                }
            }

            /*GGRegistrySetValue("Login", tbLogin.Text);
            GGRegistrySetValue("Trusted", Convert.ToInt32(cbWindows.Checked));*/

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _sSoftName + "\\" + "dbserver";
            _reg.GGRegistrySetValue("Login", tbLogin.Text, _p);
            _reg.GGRegistrySetValue("Trusted", Convert.ToInt32(cbWindows.Checked), _p);
            _reg = null;

            this._sUsername = tbLogin.Text;
            this._sPassword = tbPassword.Text;
            this._sTrusted = cbWindows.Checked;
            this._sServername = tbServer.Text;
            this._sBasename = tbBase.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void cbWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWindows.Checked)
            {
                tbLogin.Text = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
                tbPassword.Text = "";
                tbLogin.Enabled = false;
                tbPassword.Enabled = false;
            }
            else
            {
                tbLogin.Enabled = true;
                tbPassword.Enabled = true;
            }
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            switch ((int)btnDB.Tag)
            {
                    case 0:
                        this.Height = 223;
                        btnDB.Tag = 1;
                        break;
                    case 1:
                        this.Height = 137;
                        btnDB.Tag = 0;
                        break;
            }
        }

        private void frmDlgConnect_Load(object sender, EventArgs e)
        {
            btnDB.Tag = 0;
            this.Height = 137;
            tbServer.Text = _sServername;
            tbBase.Text = _sBasename;
        }

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (e.KeyChar == (char)Keys.Enter)
                btnOK_Click(null, null);
        }

        private void frmDlgConnect_Shown(object sender, EventArgs e)
        {
            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _sSoftName + "\\" + "dbserver";
            Int32 trust = Convert.ToInt32(_reg.GGRegistryGetValue("Trusted", _p));
            tbLogin.Text = (string)_reg.GGRegistryGetValue("Login", _p);
            _reg = null;

            cbWindows.Checked = (trust == 1 ? true : false);
            cbWindows_CheckedChanged(null, null);
            if (tbLogin.Text != "") btnOK.Focus();
        }
    }
}