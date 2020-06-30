using System;
using System.Windows.Forms;

namespace GGPlatform.DBServer
{
    public partial class ShowProgressTask : Form
    {
        private DateTime _dt_start = DateTime.Now;

        public string Message
        {
            set { labelMessage.Text = value; }
        }

       // public int ProgressValue
      //  {
      //      set { progressBar1.Value = value; }
      //  }

        public ShowProgressTask()
        {
            InitializeComponent();
        }

        public ShowProgressTask(string inMessage)
        {
            InitializeComponent();
            this.Message = inMessage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime _dt_tick = DateTime.Now;            
            labelTimer.Text = "время выполнения: " + 
                              ((_dt_tick - _dt_start).Hours.ToString()).PadLeft(2, '0') + ":" +
                              ((_dt_tick - _dt_start).Minutes.ToString()).PadLeft(2, '0') + ":" +
                              ((_dt_tick - _dt_start).Seconds.ToString()).PadLeft(2, '0');            
            Application.DoEvents();
        }

        private void frmProgress_Shown(object sender, EventArgs e)
        {
            labelTimer.Text = "время выполнения: 00:00:00";
            Application.DoEvents();
            timer1.Enabled = true;
        }

        private void frmProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
