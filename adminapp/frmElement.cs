using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using GGPlatform.DBServer;
using System.Reflection;
using System.Linq;

namespace GGPlatform.adminapp
{
    public partial class frmElement : Form
    {
        public class CheckListItem
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public CheckListItem(int inID, string inName)
            {
                ID = inID;
                Name = inName;
            }
        }


        private int _ObjectID = -1;
        private int _ObjectType = -1;
        private DBSqlServer _dbsql = null;
        private DataRow _dr = null;
        private string _fullpathreport = null;
        private string _fullpathass = null;
        private string _password = "";

        public frmElement(int inObjectID, DataRow inDR, DBSqlServer inDBSql, int inObjectType)
        {
            InitializeComponent();
            _ObjectID = inObjectID;
            _ObjectType = inObjectType;
            _dbsql = inDBSql;
            _dr = inDR;
        }

        private void frmElement_Load(object sender, EventArgs e)
        {
            if (_ObjectID == -1)
                this.Text = "Добавить объект системы";
            else
                this.Text = "Изменить объект системы";          

            switch (_ObjectType)
            {
                case 0: { CreateSettings(); } break;
                case 1: { CreateUsers(); } break;
                case 2: { CreateWorkplaces(); } break;
                case 3: { CreateAssemblys(); } break;
                case 4: { CreateGroupReports(); } break;
                case 5: { CreateReports(); } break;
            }

            LoadDataCLB();
        }

        private void CreateSettings()
        {
            tabControl.TabPages.Remove(tabPageUser);
            tabControl.TabPages.Remove(tabPageWorkplace);
            tabControl.TabPages.Remove(tabPageReport);
            tabControl.TabPages.Remove(tabPageAssemblys);
            tabControl.TabPages.Remove(tabPageGroupReport);
            tabControl.TabPages.Remove(tabPageListGroupReports);
            tabControl.TabPages.Remove(tabPageListUsers);
            tabControl.TabPages.Remove(tabPageListWorkplace);
            tabControl.TabPages.Remove(tabPageListAssemblys);
            tabControl.TabPages.Remove(tabPageListReports);

            if (_ObjectID > 0)
            {
                tbSettingsName.Text = _dr["name"].ToString();
                tbSettingsKey.Text = _dr["key"].ToString();
                tbSettingsValue.Text = _dr["value"].ToString();
                tbSettingsDesc.Text = _dr["description"].ToString();
            }
        }

        private void CreateUsers()
        {
            tabControl.TabPages.Remove(tabPageSettings);
            tabControl.TabPages.Remove(tabPageWorkplace);
            tabControl.TabPages.Remove(tabPageReport);
            tabControl.TabPages.Remove(tabPageAssemblys);
            tabControl.TabPages.Remove(tabPageGroupReport);
            tabControl.TabPages.Remove(tabPageListUsers);
            tabControl.TabPages.Remove(tabPageListAssemblys);
            tabControl.TabPages.Remove(tabPageListReports);
            
            cbUserDropped.Enabled = false;            
            if (_ObjectID > 0)
            {
                tbUserName.Text = _dr["login"].ToString();
                tbUserFam.Text = _dr["fam"].ToString();
                tbUserIm.Text = _dr["im"].ToString();
                tbUserOtch.Text = _dr["otch"].ToString();

                cbUserAdmin.Checked = (bool)_dr["isAdmin"];
                cbUserWindows.Checked = (bool)_dr["isWindowsUser"];
                cbUserDropped.Checked = (bool)_dr["isDropped"];

                tbUserName.Enabled = false;
                cbUserWindows.Enabled = false;
                btPass.Enabled = false;             
            }
        }

        private void CreateWorkplaces()
        {
            tabControl.TabPages.Remove(tabPageSettings);
            tabControl.TabPages.Remove(tabPageUser);
            tabControl.TabPages.Remove(tabPageReport);
            tabControl.TabPages.Remove(tabPageAssemblys);
            tabControl.TabPages.Remove(tabPageGroupReport);
            tabControl.TabPages.Remove(tabPageListGroupReports);
            tabControl.TabPages.Remove(tabPageListWorkplace);
            tabControl.TabPages.Remove(tabPageListReports);

            if (_ObjectID > 0)
            {
                tbWorkplaceName.Text = _dr["name"].ToString();
                tbWorkplaceDesc.Text = _dr["description"].ToString();
            }
        }

        private void CreateAssemblys()
        {
            tabControl.TabPages.Remove(tabPageSettings);
            tabControl.TabPages.Remove(tabPageUser);
            tabControl.TabPages.Remove(tabPageWorkplace);
            tabControl.TabPages.Remove(tabPageReport);
            tabControl.TabPages.Remove(tabPageGroupReport);
            tabControl.TabPages.Remove(tabPageListGroupReports);
            tabControl.TabPages.Remove(tabPageListUsers);
            tabControl.TabPages.Remove(tabPageListAssemblys);
            tabControl.TabPages.Remove(tabPageListReports);

            if (_ObjectID > 0)
            {
                tbAssemAss.Text = _dr["AssemblyName"].ToString();
                tbAssemName.Text = _dr["Name"].ToString();
                tbAssemCut.Text = _dr["ShortCut"].ToString();
            }
        }

        private void CreateGroupReports()
        {
            tabControl.TabPages.Remove(tabPageSettings);
            tabControl.TabPages.Remove(tabPageUser);
            tabControl.TabPages.Remove(tabPageWorkplace);
            tabControl.TabPages.Remove(tabPageReport);
            tabControl.TabPages.Remove(tabPageAssemblys);
            tabControl.TabPages.Remove(tabPageListGroupReports);
            tabControl.TabPages.Remove(tabPageListWorkplace);
            tabControl.TabPages.Remove(tabPageListAssemblys);

            if (_ObjectID > 0)
            {
                tbReportGroupName.Text = _dr["Name"].ToString();
            }
        }

        private void CreateReports()
        {
            tabControl.TabPages.Remove(tabPageSettings);
            tabControl.TabPages.Remove(tabPageUser);
            tabControl.TabPages.Remove(tabPageWorkplace);
            tabControl.TabPages.Remove(tabPageAssemblys);
            tabControl.TabPages.Remove(tabPageGroupReport);
            //tabControl.TabPages.Remove(tabPageListGroupReports);
            tabControl.TabPages.Remove(tabPageListUsers);
            tabControl.TabPages.Remove(tabPageListWorkplace);
            tabControl.TabPages.Remove(tabPageListAssemblys);
            tabControl.TabPages.Remove(tabPageListReports);

            if (_ObjectID > 0)
            {
                tbReportName.Text = _dr["Name"].ToString();
                tbReportAss.Text = _dr["AssemblyName"].ToString();
            }
        }

        private void LoadDataCLB()
        {
            clbAssemblys.Items.Clear();
            clbWorkplaces.Items.Clear();
            clbGroupReports.Items.Clear();
            clbUsers.Items.Clear();
            clbReports.Items.Clear();

            switch (_ObjectType)
            {
                case 1: 
                    {
                        _dbsql.SQLScript = "select id, name from GGPlatform.Workplaces order by name";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbWorkplaces);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_UsersManager @TypeQuery = 1, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbWorkplaces);
                        }

                        _dbsql.SQLScript = "select id, name from GGPlatform.ReportGroups order by name";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbGroupReports);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_UsersManager @TypeQuery = 2, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbGroupReports);
                        }
                    } break;
                case 2: 
                    {
                        _dbsql.SQLScript = "select id, isnull(Login,'') + ' - ' + isnull(Fam,'') + ' ' + isnull(Im,'') + ' ' + isnull(Otch,'') as name from GGPlatform.Users where isDropped = 0 order by isnull(Fam,'') + ' ' + isnull(Im,'') + ' ' + isnull(Otch,'')";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbUsers);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_WorkplacesManager @TypeQuery = 1, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbUsers);
                        }

                        _dbsql.SQLScript = "select id, name from GGPlatform.Assemblys order by name";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbAssemblys);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_WorkplacesManager @TypeQuery = 2, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbAssemblys);
                        }
                    } break;
                case 3: 
                    { 
                        _dbsql.SQLScript = "select id, name from GGPlatform.Workplaces order by name";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbWorkplaces);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_AssemblysManager @TypeQuery = 1, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbWorkplaces);
                        }
                    } break;
                case 4: 
                    {
                        _dbsql.SQLScript = "select id, isnull(Login,'') + ' - ' + isnull(Fam,'') + ' ' + isnull(Im,'') + ' ' + isnull(Otch,'') as name from GGPlatform.Users where isDropped = 0 order by isnull(Fam,'') + ' ' + isnull(Im,'') + ' ' + isnull(Otch,'')";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbUsers);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_ReportGroupsManager @TypeQuery = 1, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbUsers);
                        }

                        _dbsql.SQLScript = "select id, name from GGPlatform.Reports order by name, orderid";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbReports);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_ReportGroupsManager @TypeQuery = 2, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbReports);
                        }
                    } break;
                case 5:
                    {
                        _dbsql.SQLScript = "select id, name from GGPlatform.ReportGroups order by name";
                        LoadCLB(_dbsql.ExecuteReader(), ref clbGroupReports);

                        if (_ObjectID > 0)
                        {
                            _dbsql.SQLScript = "exec GGPlatform.usp_ReportsManager @TypeQuery = 1, @ObjectID = @objID";
                            _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                            SetCheckedCLB(_dbsql.ExecuteReader(), ref clbGroupReports);
                        }
                    } break;
            }
        }

        private void LoadCLB(SqlDataReader inObjects, ref CheckedListBox inCLB)
        {
            if (inObjects == null)
                return;
            ((ListBox)inCLB).DisplayMember = "Name";
            ((ListBox)inCLB).ValueMember = "ID";

            inCLB.Items.Clear();
            while (inObjects.Read())
            {
                inCLB.Items.Add(new CheckListItem((int)inObjects["ID"], inObjects["name"].ToString()));
            }
            inObjects.Close();
            inObjects.Dispose();
        }

        private string GetCheckedCLB(ref CheckedListBox inCLB)
        {
            string t = "";

            foreach (CheckListItem it in inCLB.CheckedItems)
                t += ";" + it.ID.ToString();

            if (t != "")
                return t.Substring(1);
            else return t;
        }

        private void SetCheckedCLB(SqlDataReader inObjects, ref CheckedListBox inCLB)
        {
            if (inObjects == null)
                return;
            while (inObjects.Read())
            {
                for (int i = 0; i <= inCLB.Items.Count - 1; i++)
                {
                    if (((CheckListItem)(inCLB.Items[i])).ID.ToString().Equals(inObjects["ID"].ToString()))
                    {
                        inCLB.SetItemChecked(i, true);
                        break;
                    }
                }
            }
            inObjects.Close();
            inObjects.Dispose();
        }

        //public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        //{
        //    if (assembly == null) throw new ArgumentNullException("assembly");
        //    try
        //    {
        //        return assembly.GetTypes();
        //    }
        //    catch (ReflectionTypeLoadException e)
        //    {
        //        return e.Types.Where(t => t != null);
        //    }
        //}

        private bool UserCheck()
        {
            bool _result = false;
            switch (_ObjectType)
            {
                case 0:
                    {
                        _result = true;
                        break;
                    }
                case 1:
                    {
                        _result = true;
                        if ((!cbUserWindows.Checked) && (_ObjectID < 0))
                        {
                            if (tbUserName.Text == "")
                            {
                                _result = false;
                                MessageBox.Show("Не указано имя пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return _result;
                            }
                            if (_password == "")
                            {
                                _result = false;
                                MessageBox.Show("Пароль не указан", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return _result;
                            }
                            if ((_password != "") && (_password.Length < 8))
                            {
                                _result = false;
                                MessageBox.Show("Длина пароля меньше 8 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    }
                case 2:                
                case 4:
                    {
                        _result = true;
                        break;
                    }
                case 3:
                    {
                        _result = true;
                        if (_ObjectID == -1)
                        {
                            if (_fullpathass.Contains(".dll"))
                            {
                                _result = false;
                                Assembly _Assembly = null;
                                _Assembly = Assembly.LoadFrom(_fullpathass);
                                foreach (Type type in _Assembly.GetTypes())
                                {
                                    Type iface = type.GetInterface("IPlugin", true);
                                    if (iface != null)
                                    {
                                        _result = true;
                                        iface = null;
                                        break;
                                    }
                                }
                                if (_Assembly != null)
                                    _Assembly = null;
                                if (!_result)
                                    MessageBox.Show("Файл сборки не реализует интерфейс IPlugin",
                                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }                        
                        break;
                    }
                case 5:
                    {
                        _result = true;
                        if (_ObjectID == -1)
                        {
                            if (_fullpathreport.Contains(".xml"))
                            {
                                _result = false;
                                validXml.XmlSchemaValidator xmlv = new validXml.XmlSchemaValidator();
                                string xmlstr = File.ReadAllText(_fullpathreport);
                                string xsdfil = Application.StartupPath + "\\resource\\reportXml.xsd";
                                _result = xmlv.ValidXmlDoc(xmlstr, "", xsdfil);
                                if (!_result)
                                    MessageBox.Show("Файл конфигурации отчета не соответствует схеме \n" + xsdfil + "\n" + xmlv.ValidationError,
                                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                xmlv = null;
                            }
                            if (_fullpathreport.Contains(".dll"))
                            {
                                _result = false;
                                Assembly _Assembly = null;
                                _Assembly = Assembly.LoadFrom(_fullpathreport);
                                foreach (Type type in _Assembly.GetTypes())
                                {
                                    Type iface = type.GetInterface("IReport", true);
                                    if (iface != null)
                                    {
                                        _result = true;
                                        iface = null;
                                        break;
                                    }
                                }
                                if (_Assembly != null)
                                    _Assembly = null;
                                if (!_result)
                                    MessageBox.Show("Файл сборки не реализует интерфейс IReport",
                                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }                            
                        break;
                    }
            }
            return _result;
            //проверки
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
           switch (_ObjectType)
           {
               case 0: 
                   {
                       if (!UserCheck()) return; 
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_SettingsManager] @TypeQuery = 3, @ObjectID = @objID, @Name = @f, @Description = @i, @Key = @o, @Value = @post";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@f", EnumDBDataTypes.EDT_String, tbSettingsName.Text.ToString());
                           _dbsql.AddParameter("@i", EnumDBDataTypes.EDT_String, tbSettingsDesc.Text.ToString());
                           _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, tbSettingsKey.Text.ToString());
                           _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_String, tbSettingsValue.Text.ToString());
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_SettingsManager] @TypeQuery = 4, @Name = @f, @Description = @i, @Key = @o, @Value = @post";
                           _dbsql.AddParameter("@f", EnumDBDataTypes.EDT_String, tbSettingsName.Text.ToString());
                           _dbsql.AddParameter("@i", EnumDBDataTypes.EDT_String, tbSettingsDesc.Text.ToString());
                           _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, tbSettingsKey.Text.ToString());
                           _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_String, tbSettingsValue.Text.ToString());
                           _dbsql.ExecuteScalar();
                       }
                   } break;
               case 1:
                   {
                       if (!UserCheck()) return;
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_UsersManager] @TypeQuery = 3, @ObjectID = @objID, @Fam = @f, @Im = @i, @Otch = @o, @PostRef = @post, @SectionRef = @sect, @isAdmin = @adm, @Workplaces = @work, @ReportGroups = @rep";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@f", EnumDBDataTypes.EDT_String, tbUserFam.Text.ToString());
                           _dbsql.AddParameter("@i", EnumDBDataTypes.EDT_String, tbUserIm.Text.ToString());
                           _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, tbUserOtch.Text.ToString());
                           _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, "0");
                           _dbsql.AddParameter("@sect", EnumDBDataTypes.EDT_Integer, "0");
                           _dbsql.AddParameter("@adm", EnumDBDataTypes.EDT_Bit, cbUserAdmin.Checked.ToString());
                           _dbsql.AddParameter("@work", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbWorkplaces));
                           _dbsql.AddParameter("@rep", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbGroupReports));
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_UsersManager] @TypeQuery = 4, @Login = @log, @Fam = @f, @Im = @i, @Otch = @o, @PostRef = @post, @SectionRef = @sect, @isWindowsUser = @win, @isAdmin = @adm, @Workplaces = @work, @ReportGroups = @rep, @Password = @passw";
                           _dbsql.AddParameter("@log", EnumDBDataTypes.EDT_String, tbUserName.Text.ToString());
                           _dbsql.AddParameter("@f", EnumDBDataTypes.EDT_String, tbUserFam.Text.ToString());
                           _dbsql.AddParameter("@i", EnumDBDataTypes.EDT_String, tbUserIm.Text.ToString());
                           _dbsql.AddParameter("@o", EnumDBDataTypes.EDT_String, tbUserOtch.Text.ToString());
                           _dbsql.AddParameter("@post", EnumDBDataTypes.EDT_Integer, "0");
                           _dbsql.AddParameter("@sect", EnumDBDataTypes.EDT_Integer, "0");
                           _dbsql.AddParameter("@win", EnumDBDataTypes.EDT_Bit, cbUserWindows.Checked.ToString());
                           _dbsql.AddParameter("@adm", EnumDBDataTypes.EDT_Bit, cbUserAdmin.Checked.ToString());
                           _dbsql.AddParameter("@work", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbWorkplaces));
                           _dbsql.AddParameter("@rep", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbGroupReports));
                           _dbsql.AddParameter("@passw", EnumDBDataTypes.EDT_String, _password);
                           _dbsql.ExecuteScalar();
                       }
                   } break;
               case 2:
                   {
                       if (!UserCheck()) return;
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_WorkplacesManager] @TypeQuery = 3, @ObjectID = @objID, @Name = @na, @Description = @de, @Users = @us, @Assemblys = @as";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbWorkplaceName.Text.ToString());
                           _dbsql.AddParameter("@de", EnumDBDataTypes.EDT_String, tbWorkplaceDesc.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbUsers));
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbAssemblys));
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_WorkplacesManager] @TypeQuery = 4, @Name = @na, @Description = @de, @Users = @us, @Assemblys = @as";
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbWorkplaceName.Text.ToString());
                           _dbsql.AddParameter("@de", EnumDBDataTypes.EDT_String, tbWorkplaceDesc.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbUsers));
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbAssemblys));
                           _dbsql.ExecuteScalar();
                       }
                   } break;
               case 3:
                   {
                       if (!UserCheck()) return;
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_AssemblysManager] @TypeQuery = 3, @ObjectID = @objID, @Name = @na, @ShortCut = @de, @AssemblyName = @us, @Workplaces = @as";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbAssemName.Text.ToString());
                           _dbsql.AddParameter("@de", EnumDBDataTypes.EDT_String, tbAssemCut.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, tbAssemAss.Text.ToString());
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbWorkplaces));
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_AssemblysManager] @TypeQuery = 4, @Name = @na, @ShortCut = @de, @AssemblyName = @us, @Workplaces = @as";
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbAssemName.Text.ToString());
                           _dbsql.AddParameter("@de", EnumDBDataTypes.EDT_String, tbAssemCut.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, tbAssemAss.Text.ToString());
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbWorkplaces));
                           _dbsql.ExecuteScalar();
                       }
                   } break;
               case 4:
                   {
                       if (!UserCheck()) return;
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportGroupsManager] @TypeQuery = 3, @ObjectID = @objID, @Name = @na, @Users = @us, @Reports = @as";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbReportGroupName.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbUsers));
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbReports));
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportGroupsManager] @TypeQuery = 4, @Name = @na, @Users = @us, @Reports = @as";
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbReportGroupName.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbUsers));
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbReports));
                           _dbsql.ExecuteScalar();
                       }

                   } break;
               case 5:
                   {
                       if (!UserCheck()) return;
                       if (_ObjectID > -1)
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportsManager] @TypeQuery = 3, @ObjectID = @objID, @Name = @na, @AssemblyName = @us, @ReportGroups = @as";
                           _dbsql.AddParameter("@objID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbReportName.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, tbReportAss.Text.ToString());
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbGroupReports));
                           _dbsql.ExecuteScalar();
                       }
                       else
                       {
                           _dbsql.SQLScript = "exec [GGPlatform].[usp_ReportsManager] @TypeQuery = 4, @Name = @na, @AssemblyName = @us, @ReportGroups = @as";
                           _dbsql.AddParameter("@na", EnumDBDataTypes.EDT_String, tbReportName.Text.ToString());
                           _dbsql.AddParameter("@us", EnumDBDataTypes.EDT_String, tbReportAss.Text.ToString());
                           _dbsql.AddParameter("@as", EnumDBDataTypes.EDT_String, GetCheckedCLB(ref clbGroupReports));
                           _dbsql.ExecuteScalar();
                       }
                   } break;
           }
           this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btAss_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Интерфейсы|*.dll";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbAssemAss.Text = Path.GetFileName(openFileDialog1.FileName).ToLower(); 
                _fullpathass = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\reports";
            openFileDialog1.Filter = "Отчетные формы|report*.dll;report*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbReportAss.Text = Path.GetFileName(openFileDialog1.FileName).ToLower();
                _fullpathreport = openFileDialog1.FileName;
            }
        }

        private void cbUserWindows_CheckedChanged(object sender, EventArgs e)
        {
            btPass.Enabled = !(cbUserWindows.Checked);
        }

        private void btPass_Click(object sender, EventArgs e)
        {
            _password = Microsoft.VisualBasic.Interaction.InputBox("Введите пароль (не менее 8 символов)", "Создание учетной записи", _password);
            if (_password == "")
            {
                MessageBox.Show("Пароль не указан", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((_password != "") && (_password.Length < 8))
            {
                MessageBox.Show("Длина пароля меньше 8 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}