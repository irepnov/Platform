using System;
using System.Data;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.AdresManag;

namespace SimRFC
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
        private AdresInfo _adresREG = new AdresInfo();
        private AdresInfo _adresPR = new AdresInfo();

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
                this.Text = "Добавить позицию в справочник";
            else
                this.Text = "Изменить позицию справочника";          

            switch (_ObjectType)
            {
                case 1: { CreateTovar(); } break;
                case 2: { CreateProvider(); } break;
                case 3: { CreateSklad(); } break;
                case 4: { CreateOtvLico(); } break;
                case 5: { CreateTypeSklad(); } break;
                case 6: { CreatePostav(); } break;
            }

            LoadDataCLB();
        }

        private void CreateTovar()
        {
            tabControl.TabPages.Remove(tabPageOtvLico);
            tabControl.TabPages.Remove(tabPageProvider);
            tabControl.TabPages.Remove(tabPageSklad);
            tabControl.TabPages.Remove(tabPageTypeSklad);
            tabControl.TabPages.Remove(tabPagePostav);

            if (_ObjectID > 0)
            {
                tbTovarName.Text = _dr["name"].ToString();
            }
        }

        private void CreateProvider()
        {
            tabControl.TabPages.Remove(tabPageTovar);
            tabControl.TabPages.Remove(tabPageSklad);
            tabControl.TabPages.Remove(tabPageOtvLico);
            tabControl.TabPages.Remove(tabPageTypeSklad);
            tabControl.TabPages.Remove(tabPagePostav);

            if (_ObjectID > 0)
            {
                tbProviderName.Text = _dr["name"].ToString();
                tbProviderComment.Text = _dr["comment"].ToString();             
            }
        }

        private void CreateSklad()
        {
            tabControl.TabPages.Remove(tabPageTovar);
            tabControl.TabPages.Remove(tabPageProvider);
            tabControl.TabPages.Remove(tabPageOtvLico);
            tabControl.TabPages.Remove(tabPageTypeSklad);
            tabControl.TabPages.Remove(tabPagePostav);

            if (_ObjectID > 0)
            {
                tbSkladName.Text = _dr["name"].ToString();
                chSkladOsnovnoy.Checked = (bool)_dr["isMainRepository"];

                tbSkladAnaliz.Text = _dr["Analytics"].ToString();

                //cbSkladOtvets
                //cbSkladType
                tbSkladCodePoint.Text = _dr["codepoint"].ToString();
                tbSkladComment.Text = _dr["comment"].ToString();
            }
        }

        private void CreateOtvLico()
        {
            tabControl.TabPages.Remove(tabPageTovar);
            tabControl.TabPages.Remove(tabPageProvider);
            tabControl.TabPages.Remove(tabPageSklad);
            tabControl.TabPages.Remove(tabPageTypeSklad);
            tabControl.TabPages.Remove(tabPagePostav);

            if (_ObjectID > 0)
            {
                tbLicoFam.Text = _dr["fam"].ToString();
                tbLicoIm.Text = _dr["im"].ToString();
                tbLicoOtch.Text = _dr["otch"].ToString();
                tbLicoPhone1.Text = _dr["phone_1"].ToString();
                tbLicoPhone2.Text = _dr["phone_2"].ToString();
                tbLicoPhone3.Text = _dr["phone_3"].ToString();
                tbLicoDocSer.Text = _dr["docseria"].ToString();
                tbLicoDocNum.Text = _dr["docnumber"].ToString();
                //cbLicoSkald
                tbLicoAdreRg.Text = _dr["adresreg"].ToString();
                tbLicoAdrePr.Text = _dr["adrespr"].ToString();

                    _adresREG.Gorod = Convert.ToInt32(_dr["adrregGOROD"]);
                    _adresREG.House = _dr["adrregDOM"].ToString();
                    _adresREG.Korp = _dr["adrregKORP"].ToString();
                    _adresREG.Kvart = _dr["adrregKVART"].ToString();
                    _adresREG.Naspunkt = Convert.ToInt32(_dr["adrregNASPUNKT"]);
                    _adresREG.Raion = Convert.ToInt32(_dr["adrregRAION"]);
                    _adresREG.Region = Convert.ToInt32(_dr["adrregREGION"]);
                    _adresREG.Street = Convert.ToInt32(_dr["adrregSTREET"]);
                    _adresREG.Title = _dr["AdresREG"].ToString();

                    _adresPR.Gorod = Convert.ToInt32(_dr["adrprGOROD"]);
                    _adresPR.House = _dr["adrprDOM"].ToString();
                    _adresPR.Korp = _dr["adrprKORP"].ToString();
                    _adresPR.Kvart = _dr["adrprKVART"].ToString();
                    _adresPR.Naspunkt = Convert.ToInt32(_dr["adrprNASPUNKT"]);
                    _adresPR.Raion = Convert.ToInt32(_dr["adrprRAION"]);
                    _adresPR.Region = Convert.ToInt32(_dr["adrprREGION"]);
                    _adresPR.Street = Convert.ToInt32(_dr["adrprSTREET"]);
                    _adresPR.Title = _dr["AdresPR"].ToString();
            }
        }

        private void CreateTypeSklad()
        {
            tabControl.TabPages.Remove(tabPageTovar);
            tabControl.TabPages.Remove(tabPageProvider);
            tabControl.TabPages.Remove(tabPageSklad);
            tabControl.TabPages.Remove(tabPageOtvLico);
            tabControl.TabPages.Remove(tabPagePostav);

            if (_ObjectID > 0)
            {
                tbTypeSkladName.Text = _dr["Name"].ToString();
            }
        }

        private void CreatePostav()
        {
            tabControl.TabPages.Remove(tabPageTovar);
            tabControl.TabPages.Remove(tabPageProvider);
            tabControl.TabPages.Remove(tabPageSklad);
            tabControl.TabPages.Remove(tabPageOtvLico);
            tabControl.TabPages.Remove(tabPageTypeSklad);

            dtPostavPrice.Enabled = false;

            dtPostavPrice.ValueChanged += new System.EventHandler(dtPostavPrice_ValueChanged);
            


            if (_ObjectID > 0)
            {
                tbPostavName.Text = _dr["Name"].ToString();
                chbPostavActual.Checked = (bool)_dr["isActive"];
                if (_dr["DateLastPrice"].ToString() != "")
                    dtPostavPrice.Value = (DateTime)_dr["DateLastPrice"];
                else {
                    dtPostavPrice.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                    dtPostavPrice.CustomFormat = " ";
                }
            }
        }

        private void dtPostavPrice_ValueChanged(object sender, EventArgs e)
        {
            if (dtPostavPrice.Format == System.Windows.Forms.DateTimePickerFormat.Custom)
                dtPostavPrice.Format = System.Windows.Forms.DateTimePickerFormat.Long;
        }
        private void LoadDataCLB()
        {            
            cbSkladOtvets.Items.Clear();
            cbSkladType.Items.Clear();           
            cbLicoSkald.Items.Clear();

            switch (_ObjectType)
            {
                case 3: 
                    {
                        _dbsql.SQLScript = "select id, fam + ' ' + im + ' ' + otch as name from rfcLiability order by 2";
                        LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbSkladOtvets);

                        _dbsql.SQLScript = "select id, name from rfcTypeRepository order by name";
                        LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbSkladType);

                        if (_ObjectID > 0)
                        {
                            SetCheckedCLB(_dr["LiabilityCuratorRef"], ref cbSkladOtvets);
                            SetCheckedCLB(_dr["TypeRepositoryRef"], ref cbSkladType);
                        }
                    } break;
                case 4: 
                    {
                        _dbsql.SQLScript = "select id, name from rfcRepository order by name";
                        LoadCLB(_dbsql.FillDataSet().Tables[0], ref cbLicoSkald);

                         if (_ObjectID > 0)
                         {
                             SetCheckedCLB(_dr["RepositoryRef"], ref cbLicoSkald);
                         }
                    }
                    break;
            }
        }

        private void LoadCLB(DataTable inObjects, ref ComboBox inCLB)
        {
            if (inObjects == null)
                return;

            inCLB.ValueMember = "ID";
            inCLB.DisplayMember = "Name";            
            inCLB.DataSource = inObjects;
            inCLB.SelectedValue = DBNull.Value;
        }

        private string GetCheckedCLB(ref ComboBox inCLB)
        {
            if ((inCLB.SelectedValue == null) || (inCLB.SelectedValue == DBNull.Value))
                return "";
            else return inCLB.SelectedValue.ToString();
        }

        private void SetCheckedCLB(object inObjects, ref ComboBox inCLB)
        {
            inCLB.SelectedValue = inObjects;
        }

        private bool UserCheck()
        {
            bool _result = false;
              
            try
            {
                switch (_ObjectType)
                {
                    case 1:
                        {
                            _result = true;
                            if (tbTovarName.Text == "")
                            {
                                ActiveControl = tbTovarName;
                                throw new Exception("Необходимо указать наименование");
                            }
                            break;    
                        }
                    case 2:
                        {
                            _result = true;
                            if (tbProviderName.Text == "")
                            {
                                ActiveControl = tbProviderName;
                                throw new Exception("Необходимо указать наименование");
                            }
                            break;
                        }
                    case 3:
                        {
                            _result = true;
                            if (tbSkladName.Text == "")
                            {
                                ActiveControl = tbSkladName;
                                throw new Exception("Необходимо указать наименование");
                            }
                            if (cbSkladType.Text == "")
                            {
                                ActiveControl = cbSkladType;
                                throw new Exception("Необходимо указать тип");
                            }
                            if (cbSkladOtvets.Text == "")
                            {
                                ActiveControl = cbSkladOtvets;
                                throw new Exception("Необходимо назначить ответственное лицо");
                            }
                            break;    
                        }
                    case 4:
                        {
                            _result = true;
                            if (tbLicoFam.Text == "")
                            {
                                ActiveControl = tbLicoFam;
                                throw new Exception("Необходимо указать фамилию");
                            }
                            if (tbLicoIm.Text == "")
                            {
                                ActiveControl = tbLicoIm;
                                throw new Exception("Необходимо указать имя");
                            }
                            if (tbLicoOtch.Text == "")
                            {
                                ActiveControl = tbLicoOtch;
                                throw new Exception("Необходимо указать отчество");
                            }
                            if (tbLicoPhone1.Text == "")
                            {
                                ActiveControl = tbLicoPhone1;
                                throw new Exception("Необходимо указать телефон №1");
                            }
                            if (tbLicoPhone2.Text == "")
                            {
                                ActiveControl = tbLicoPhone2;
                                throw new Exception("Необходимо указать телефон №2");
                            }
                            if (tbLicoPhone3.Text == "")
                            {
                                ActiveControl = tbLicoPhone3;
                                throw new Exception("Необходимо указать телефон №3");
                            }
                            if (tbLicoDocSer.Text == "")
                            {
                                ActiveControl = tbLicoDocSer;
                                throw new Exception("Необходимо указать серию УДЛ");
                            }
                            if (tbLicoDocNum.Text == "")
                            {
                                ActiveControl = tbLicoDocNum;
                                throw new Exception("Необходимо указать номер УДЛ");
                            }
                            if (cbLicoSkald.Text == "")
                            {
                                ActiveControl = cbLicoSkald;
                                throw new Exception("Необходимо назначить склад");
                            }
                            if (tbLicoAdreRg.Text == "")
                            {
                                ActiveControl = tbLicoAdreRg;
                                throw new Exception("Необходимо указать адрес регистрации");
                            }
                            if (tbLicoAdrePr.Text == "")
                            {
                                ActiveControl = tbLicoAdrePr;
                                throw new Exception("Необходимо указать адрес проживания");
                            }

                            break;
                        }
                    case 5:
                        {
                            _result = true;
                            if (tbTypeSkladName.Text == "")
                            {
                                ActiveControl = tbTypeSkladName;
                                throw new Exception("Необходимо указать наименование");
                            }
                            break;
                        }
                    case 6:
                        {
                            _result = true;
                            if (tbPostavName.Text == "")
                            {
                                ActiveControl = tbPostavName;
                                throw new Exception("Необходимо указать наименование");
                            }
                            break;
                        }
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

        private void btnOk_Click(object sender, EventArgs e)
        {
           if (!UserCheck()) return;
           try
            {
                switch (_ObjectType)
                {
                    case 1:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 16, @ID = @ID2, @Name = @Name2";
                                _dbsql.AddParameter("@ID2", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@Name2", EnumDBDataTypes.EDT_String, tbTovarName.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 111, @Name = @Name";
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbTovarName.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                    case 2:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 17, @ID = @ID, @Name = @Name, @Comment = @Comment";
                                _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbProviderName.Text.ToString());
                                _dbsql.AddParameter("@Comment", EnumDBDataTypes.EDT_String, tbProviderComment.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 112, @Name = @Name, @Comment = @Comment";
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbProviderName.Text.ToString());
                                _dbsql.AddParameter("@Comment", EnumDBDataTypes.EDT_String, tbProviderComment.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                    case 3:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 18, @ID = @ID, @Name = @Name, @Comment = @Comment, @TypeRepositoryRef = @TRR, @isMainRepository = @Main, @CodePoint = @CP, @Analytics = @Anal, @LiabilityCuratorRef = @LCR";
                                _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbSkladName.Text.ToString());
                                _dbsql.AddParameter("@Comment", EnumDBDataTypes.EDT_String, tbSkladComment.Text.ToString());
                                _dbsql.AddParameter("@TRR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladType));
                                _dbsql.AddParameter("@Main", EnumDBDataTypes.EDT_Bit, chSkladOsnovnoy.Checked.ToString());
                                _dbsql.AddParameter("@CP", EnumDBDataTypes.EDT_String, tbSkladCodePoint.Text.ToString());
                                _dbsql.AddParameter("@Anal", EnumDBDataTypes.EDT_String, tbSkladAnaliz.Text.ToString());
                                _dbsql.AddParameter("@LCR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladOtvets));
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 113, @Name = @Name, @Comment = @Comment, @TypeRepositoryRef = @TRR, @isMainRepository = @Main, @CodePoint = @CP, @Analytics = @Anal, @LiabilityCuratorRef = @LCR";
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbSkladName.Text.ToString());
                                _dbsql.AddParameter("@Comment", EnumDBDataTypes.EDT_String, tbSkladComment.Text.ToString());
                                _dbsql.AddParameter("@TRR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladType));
                                _dbsql.AddParameter("@Main", EnumDBDataTypes.EDT_Bit, chSkladOsnovnoy.Checked.ToString());
                                _dbsql.AddParameter("@CP", EnumDBDataTypes.EDT_String, tbSkladCodePoint.Text.ToString());
                                _dbsql.AddParameter("@Anal", EnumDBDataTypes.EDT_String, tbSkladAnaliz.Text.ToString());
                                _dbsql.AddParameter("@LCR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbSkladOtvets));
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                    case 4:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = @"exec uchReference @IDOperation = 19, @ID = @ID, @RepositoryRef = @RR, @Fam = @Fam, @Im = @Im, @Otch = @Otch, @Phone_1 = @Phone1, @Phone_2 = @Phone2, @Phone_3 = @Phone3, @DocSeria = @DS, @DocNumber = @DN, 
                                                     @URAdres = @uadr, @URDom = @udom, @URKorp = @ukorp, @URKvart = @ukvart, @URRegion = @uregion, @URRaion = @uraion, @URGorod = @ugorod, @URNaspunkt = @unasp, @URStreet = @ustr, 
                                                     @REAdres = @padr, @REDom = @pdom, @REKorp = @pkorp, @REKvart = @pkvart, @RERegion = @pregion, @RERaion = @praion, @REGorod = @pgorod, @RENaspunkt = @pnasp, @REStreet = @pstr";
                                _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@RR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbLicoSkald));
                                _dbsql.AddParameter("@Fam", EnumDBDataTypes.EDT_String, tbLicoFam.Text.ToString());
                                _dbsql.AddParameter("@Im", EnumDBDataTypes.EDT_String, tbLicoIm.Text.ToString());
                                _dbsql.AddParameter("@Otch", EnumDBDataTypes.EDT_String, tbLicoOtch.Text.ToString());
                                _dbsql.AddParameter("@Phone1", EnumDBDataTypes.EDT_String, tbLicoPhone1.Text.ToString());
                                _dbsql.AddParameter("@Phone2", EnumDBDataTypes.EDT_String, tbLicoPhone2.Text.ToString());
                                _dbsql.AddParameter("@Phone3", EnumDBDataTypes.EDT_String, tbLicoPhone3.Text.ToString());
                                _dbsql.AddParameter("@DS", EnumDBDataTypes.EDT_String, tbLicoDocSer.Text.ToString());
                                _dbsql.AddParameter("@DN", EnumDBDataTypes.EDT_String, tbLicoDocNum.Text.ToString());

                                _dbsql.AddParameter("@uadr", EnumDBDataTypes.EDT_String, _adresPR.Title.ToString());
                                _dbsql.AddParameter("@udom", EnumDBDataTypes.EDT_String, _adresPR.House.ToString());
                                _dbsql.AddParameter("@ukorp", EnumDBDataTypes.EDT_String, _adresPR.Korp.ToString());
                                _dbsql.AddParameter("@ukvart", EnumDBDataTypes.EDT_String, _adresPR.Kvart.ToString());
                                _dbsql.AddParameter("@uregion", EnumDBDataTypes.EDT_String, _adresPR.Region.ToString());
                                _dbsql.AddParameter("@uraion", EnumDBDataTypes.EDT_String, _adresPR.Raion.ToString());
                                _dbsql.AddParameter("@ugorod", EnumDBDataTypes.EDT_String, _adresPR.Gorod.ToString());
                                _dbsql.AddParameter("@unasp", EnumDBDataTypes.EDT_String, _adresPR.Naspunkt.ToString());
                                _dbsql.AddParameter("@ustr", EnumDBDataTypes.EDT_String, _adresPR.Street.ToString());

                                _dbsql.AddParameter("@padr", EnumDBDataTypes.EDT_String, _adresREG.Title.ToString());
                                _dbsql.AddParameter("@pdom", EnumDBDataTypes.EDT_String, _adresREG.House.ToString());
                                _dbsql.AddParameter("@pkorp", EnumDBDataTypes.EDT_String, _adresREG.Korp.ToString());
                                _dbsql.AddParameter("@pkvart", EnumDBDataTypes.EDT_String, _adresREG.Kvart.ToString());
                                _dbsql.AddParameter("@pregion", EnumDBDataTypes.EDT_String, _adresREG.Region.ToString());
                                _dbsql.AddParameter("@praion", EnumDBDataTypes.EDT_String, _adresREG.Raion.ToString());
                                _dbsql.AddParameter("@pgorod", EnumDBDataTypes.EDT_String, _adresREG.Gorod.ToString());
                                _dbsql.AddParameter("@pnasp", EnumDBDataTypes.EDT_String, _adresREG.Naspunkt.ToString());
                                _dbsql.AddParameter("@pstr", EnumDBDataTypes.EDT_String, _adresREG.Street.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = @"exec uchReference @IDOperation = 114, @RepositoryRef = @RR, @Fam = @Fam, @Im = @Im, @Otch = @Otch, @Phone_1 = @Phone1, @Phone_2 = @Phone2, @Phone_3 = @Phone3, @DocSeria = @DS, @DocNumber = @DN, 
                                                       @URAdres = @uadr, @URDom = @udom, @URKorp = @ukorp, @URKvart = @ukvart, @URRegion = @uregion, @URRaion = @uraion, @URGorod = @ugorod, @URNaspunkt = @unasp, @URStreet = @ustr, 
                                                       @REAdres = @padr, @REDom = @pdom, @REKorp = @pkorp, @REKvart = @pkvart, @RERegion = @pregion, @RERaion = @praion, @REGorod = @pgorod, @RENaspunkt = @pnasp, @REStreet = @pstr";
                                _dbsql.AddParameter("@RR", EnumDBDataTypes.EDT_Integer, GetCheckedCLB(ref cbLicoSkald));
                                _dbsql.AddParameter("@Fam", EnumDBDataTypes.EDT_String, tbLicoFam.Text.ToString());
                                _dbsql.AddParameter("@Im", EnumDBDataTypes.EDT_String, tbLicoIm.Text.ToString());
                                _dbsql.AddParameter("@Otch", EnumDBDataTypes.EDT_String, tbLicoOtch.Text.ToString());
                                _dbsql.AddParameter("@Phone1", EnumDBDataTypes.EDT_String, tbLicoPhone1.Text.ToString());
                                _dbsql.AddParameter("@Phone2", EnumDBDataTypes.EDT_String, tbLicoPhone2.Text.ToString());
                                _dbsql.AddParameter("@Phone3", EnumDBDataTypes.EDT_String, tbLicoPhone3.Text.ToString());
                                _dbsql.AddParameter("@DS", EnumDBDataTypes.EDT_String, tbLicoDocSer.Text.ToString());
                                _dbsql.AddParameter("@DN", EnumDBDataTypes.EDT_String, tbLicoDocNum.Text.ToString());

                                _dbsql.AddParameter("@uadr", EnumDBDataTypes.EDT_String, _adresPR.Title.ToString());
                                _dbsql.AddParameter("@udom", EnumDBDataTypes.EDT_String, _adresPR.House.ToString());
                                _dbsql.AddParameter("@ukorp", EnumDBDataTypes.EDT_String, _adresPR.Korp.ToString());
                                _dbsql.AddParameter("@ukvart", EnumDBDataTypes.EDT_String, _adresPR.Kvart.ToString());
                                _dbsql.AddParameter("@uregion", EnumDBDataTypes.EDT_String, _adresPR.Region.ToString());
                                _dbsql.AddParameter("@uraion", EnumDBDataTypes.EDT_String, _adresPR.Raion.ToString());
                                _dbsql.AddParameter("@ugorod", EnumDBDataTypes.EDT_String, _adresPR.Gorod.ToString());
                                _dbsql.AddParameter("@unasp", EnumDBDataTypes.EDT_String, _adresPR.Naspunkt.ToString());
                                _dbsql.AddParameter("@ustr", EnumDBDataTypes.EDT_String, _adresPR.Street.ToString());

                                _dbsql.AddParameter("@padr", EnumDBDataTypes.EDT_String, _adresREG.Title.ToString());
                                _dbsql.AddParameter("@pdom", EnumDBDataTypes.EDT_String, _adresREG.House.ToString());
                                _dbsql.AddParameter("@pkorp", EnumDBDataTypes.EDT_String, _adresREG.Korp.ToString());
                                _dbsql.AddParameter("@pkvart", EnumDBDataTypes.EDT_String, _adresREG.Kvart.ToString());
                                _dbsql.AddParameter("@pregion", EnumDBDataTypes.EDT_String, _adresREG.Region.ToString());
                                _dbsql.AddParameter("@praion", EnumDBDataTypes.EDT_String, _adresREG.Raion.ToString());
                                _dbsql.AddParameter("@pgorod", EnumDBDataTypes.EDT_String, _adresREG.Gorod.ToString());
                                _dbsql.AddParameter("@pnasp", EnumDBDataTypes.EDT_String, _adresREG.Naspunkt.ToString());
                                _dbsql.AddParameter("@pstr", EnumDBDataTypes.EDT_String, _adresREG.Street.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                    case 5:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 110, @ID = @ID, @Name = @Name";
                                _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbTypeSkladName.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 115, @Name = @Name";
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbTypeSkladName.Text.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                    case 6:
                        {
                            if (_ObjectID > -1)
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 1110, @ID = @ID, @Name = @Name, @isActive = @act";
                                _dbsql.AddParameter("@ID", EnumDBDataTypes.EDT_Integer, _ObjectID.ToString());
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbPostavName.Text.ToString());
                                _dbsql.AddParameter("@act", EnumDBDataTypes.EDT_Bit, chbPostavActual.Checked.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                            else
                            {
                                _dbsql.SQLScript = "exec uchReference @IDOperation = 116, @Name = @Name, @isActive = @act";
                                _dbsql.AddParameter("@Name", EnumDBDataTypes.EDT_String, tbPostavName.Text.ToString());
                                _dbsql.AddParameter("@act", EnumDBDataTypes.EDT_Bit, chbPostavActual.Checked.ToString());
                                _dbsql.ExecuteNonQuery();
                            }
                        }
                        break;
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btAdresReg_Click(object sender, EventArgs e)
        {              
            AdresManager _adrman = new AdresManager(ref _adresREG, _dbsql);
            _adrman.GetAdresInfo();
            _adrman = null;
            tbLicoAdreRg.Text = _adresREG.Title;
        }

        private void btAdresPr_Click(object sender, EventArgs e)
        {
            AdresManager _adrman = new AdresManager(ref _adresPR, _dbsql);
            _adrman.GetAdresInfo();
            _adrman = null;
            tbLicoAdrePr.Text = _adresPR.Title;
        }
    }
}