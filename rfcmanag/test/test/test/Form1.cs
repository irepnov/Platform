using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GGPlatform.RFCManager;
using System.Xml;

namespace test
{
    public partial class Form1 : Form
    {
        private RFCManager _rfc = null;

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }





        private void button1_Click(object sender, EventArgs e)
        {
            _rfc.LoadRFC("spr1", "sprav1");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show((_rfc.ShowRFC("spr1")).ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _rfc.RefreshRFC("spr1");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_rfc.GetRFCValue("spr1"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _rfc.UnLoadRFC("spr1");
        }






        private void button10_Click(object sender, EventArgs e)
        {
            _rfc.LoadRFC("spr2", "sprav2");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show((_rfc.ShowRFC("spr2")).ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _rfc.RefreshRFC("spr2");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(_rfc.GetRFCValue("spr2"));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            _rfc.UnLoadRFC("spr2");
        }





        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
          //  _rfc = new RFCManager(this, null);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            _rfc = null;
        }

        private void SettingsRFC(string _RFCName)
        {

            XmlTextReader xml = null;
            try
            {
                xml = new XmlTextReader("c:\\WORK\\Platform\\BIN\\Debug\\rfcmanag.xml");
                xml.WhitespaceHandling = WhitespaceHandling.None;

                while (xml.Read())
                    if (xml.NodeType == XmlNodeType.Element)
                        if (xml.Name.ToString().ToUpper() == "REFERENCE")
                        {
                            if (xml.GetAttribute("name").ToUpper() == _RFCName.ToUpper())
                            {
                                richTextBox1.Text = richTextBox1.Text + " name = " + xml.GetAttribute("name");
                                richTextBox1.Text = richTextBox1.Text + " caption = " + xml.GetAttribute("caption");

                                //получаем query
                                while (xml.Read())
                                    if (xml.NodeType == XmlNodeType.Element)
                                        if (xml.Name.ToString().ToUpper() == "QUERY")
                                        {
                                            richTextBox1.Text = richTextBox1.Text + " query = " + xml.GetAttribute("sql");

                                            //получаем fields
                                            while (xml.Read())
                                                if (xml.NodeType == XmlNodeType.Element)
                                                    if (xml.Name.ToString().ToUpper() == "FIELDS")
                                                    {

                                                        // получаем field
                                                        while (xml.Read())
                                                            if (xml.NodeType == XmlNodeType.Element)
                                                                if (xml.Name.ToString().ToUpper() == "FIELD")
                                                                {
                                                                    richTextBox1.Text = richTextBox1.Text + " field = " + xml.GetAttribute("name");
                                                                }
                                                                else break;
                                                        break;
                                                    }
                                            break;
                                        }
                                break;
                            }
                        }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            { 
                if (xml != null)
                    xml.Close();
            }
            

        }

        private void button12_Click(object sender, EventArgs e)
        {
            SettingsRFC(textBox1.Text);
        }




    }
}
