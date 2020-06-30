using System;
using PropertyGridEx;
using GGPlatform.NSPropertyGridAddClasses;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GGPlatform.DBServer;
using GGPlatform.RFCManag;

namespace GGPlatform.InspectorManagerSP
{
    #region InformationStruct
    //--------------------------------------------------------------------------------------------------------------
    public struct InspectorInitInformation
    {
        public InspectorInitInformation(EInspectorModes inDisplayMode,
                                        EInspectorTypes inValueTypes,
                                        string inDisplayName,
                                        string inFieldName,
                                        string inDescription,
                                        CustomProperty inCustProp,
                                        string inCategory,
                                        string inSQLQuery,
                                        string inRFCName,
                                        Boolean inRFCEnableMultiSelect)
        {
            DisplayMode = inDisplayMode;
            ValueTypes = inValueTypes;
            Category = inCategory;
            DisplayName = inDisplayName;
            FieldName = inFieldName;
            Description = inDescription;
            InsItem = inCustProp;
            SQLQuery = inSQLQuery;
            RFCName = inRFCName;
            RFCMultiSelect = inRFCEnableMultiSelect;
        }

        public InspectorInitInformation(EInspectorModes inDisplayMode,
                                        EInspectorTypes inValueTypes,
                                        string inDisplayName,
                                        string inFieldName,
                                        string inDescription,
                                        CustomProperty inCustProp,
                                        string inCategory,
                                        string inSQLQuery,
                                        string inRFCName)
        {
            DisplayMode = inDisplayMode;
            ValueTypes = inValueTypes;
            Category = inCategory;
            DisplayName = inDisplayName;
            FieldName = inFieldName;
            Description = inDescription;
            InsItem = inCustProp;
            SQLQuery = inSQLQuery;
            RFCName = inRFCName;
            RFCMultiSelect = false;
        }
        //--------------------------------------------------------------------------------------------------------------
        public EInspectorModes PDisplayMode
        {
            get { return DisplayMode; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public EInspectorTypes PValueTypes
        {
            get { return ValueTypes; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PDisplayName
        {
            get { return DisplayName; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PCategory
        {
            get { return Category; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PFieldName
        {
            get { return FieldName; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PDescription
        {
            get { return Description; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public CustomProperty PInsItem
        {
            get { return InsItem; }
            set { InsItem = value; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PSQLQuery
        {
            get { return SQLQuery; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public string PRFCName
        {
            get { return RFCName; }
        }
        //--------------------------------------------------------------------------------------------------------------
        public Boolean PRFCMultiSelect
        {
            get { return RFCMultiSelect; }
        }
        //--------------------------------------------------------------------------------------------------------------
        private EInspectorModes DisplayMode;
        private EInspectorTypes ValueTypes;
        private string DisplayName;
        private string FieldName;
        private string Description;
        private CustomProperty InsItem;
        private string SQLQuery;
        private string Category;
        private string RFCName;
        private Boolean RFCMultiSelect;
    }
    //--------------------------------------------------------------------------------------------------------------
    #endregion
    //--------------------------------------------------------------------------------------------------------------
    #region EnumTypesRegion
    public enum EInspectorTypes
    {
        TypeOfInt = 0,
        TypeOfDouble = 1,
        TypeOfString = 2,
        TypeOfDate = 3,

        TypeOfListInt = 4,
        TypeOfListString = 5,

        TypeOfExpandInt = 6,
        TypeOfExpandDate = 7,

        TypeOfReferenceInt = 8,
        TypeOfReferenceString = 9,

        TypeOfStringLike = 10
    }
    //--------------------------------------------------------------------------------------------------------------
    public enum EInspectorModes
    {
        Simple = 0,
        ListBox = 1,
        Reference = 2,
        ExpandType = 3
    };
    //--------------------------------------------------------------------------------------------------------------
    #endregion
    //--------------------------------------------------------------------------------------------------------------
    public class CInspectorManager
    {
        private PropertyGridEx.PropertyGridEx refPropertyGrid;
        private DBSqlServer refDBSql;
        private RFCManager refRFC;
        private List<InspectorInitInformation> InspectorFieldsInfoList;
        //--------------------------------------------------------------------------------------------------------------
        public CInspectorManager(ref PropertyGridEx.PropertyGridEx inPropertyGrid)
        {
            refPropertyGrid = inPropertyGrid;
            InspectorFieldsInfoList = new List<InspectorInitInformation>();
        }
        public CInspectorManager(ref PropertyGridEx.PropertyGridEx inPropertyGrid, ref DBSqlServer inDBSql)
        {
            refPropertyGrid = inPropertyGrid;
            refDBSql = inDBSql;
            InspectorFieldsInfoList = new List<InspectorInitInformation>();
        }
        public CInspectorManager(ref PropertyGridEx.PropertyGridEx inPropertyGrid, ref DBSqlServer inDBSql, ref RFCManager inRFCManager)
        {
            refPropertyGrid = inPropertyGrid;
            refDBSql = inDBSql;
            refRFC = inRFCManager;
            InspectorFieldsInfoList = new List<InspectorInitInformation>();
        }
        //--------------------------------------------------------------------------------------------------------------
        public List<InspectorInitInformation> PInspectorFieldsInfoList
        {
            get { return InspectorFieldsInfoList; }
        }
        //--------------------------------------------------------------------------------------------------------------
        private object CustomEventItem_OnClick(object sender, EventArgs e)
        {
            CustomProperty prop = (CustomProperty)((CustomProperty.CustomPropertyDescriptor)sender).CustomProperty;
            string RFCName = "";
            Boolean RFCEnableMultiselect = false;

            for (int i = 0; i < InspectorFieldsInfoList.Count; ++i)
            {
                if (InspectorFieldsInfoList[i].PDisplayName == prop.Name)
                {
                    RFCName = string.Format(InspectorFieldsInfoList[i].PRFCName, "");
                    RFCEnableMultiselect = InspectorFieldsInfoList[i].PRFCMultiSelect;
                }
            }

            try
            {
                string t = prop.Tag.ToString();
            }
            catch
            { prop.Tag = ""; }

            refRFC.LoadRFC(RFCName); //если не загружен, от загрузить
            refRFC.SetRFCValue(RFCName, "ID", prop.Tag.ToString());

            if (refRFC.ShowRFC(RFCName, RFCEnableMultiselect) == DialogResult.OK)
            {
                prop.Tag = refRFC.GetRFCValueMulti(RFCName, "ID");
                return refRFC.GetRFCValueMulti(RFCName, "Code");
            }
            else
            {
                prop.Tag = "";
                return "";
            }
        }
        //--------------------------------------------------------------------------------------------------------------
        /*  public void Add(EInspectorTypes Type_, 
                          InspectorManagerSP.EInspectorModes FieldType_,
                          string DisplayName, 
                          string Category, 
                          string Discription, 
                          string FieldName, 
                          string QuerStr, 
                          int rPosID, 
                          bool rIsInit, 
                          CallBackPREx rCallBackPREx)
          {
              switch (FieldType_)
              {
                  case EInspectorModes.ListBox__:
                      {
                          switch (Type_)
                          {
                              case EInspectorTypes.TypeOfListInt:
                                  {
                                      if (!rIsInit) oDBServer.PCDBServer.SetCommand(" select null as id,'' as name");
                                      else oDBServer.PCDBServer.SetCommand(QuerStr.Replace("select id", " select null as id,'' as name union select id"));
                                      DataTable TmpDS = oDBServer.PCDBServer.ExecuteDataTable();

                                      CListKeyValue<string, long>[] ListValues = new CListKeyValue<string, long>[TmpDS.Rows.Count];

                                      int i = 0;
                                      foreach (DataRow row in TmpDS.Rows)
                                      {
                                          ListValues[i++] = new CListKeyValue<string, long>(Convert.ToString(row["name"]),
                                                  Convert.ToInt32(row["id"]));
                                      }
                                      refPropertyGrid.Item.Add(DisplayName, "", false, Category, Discription, true);
                                      refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].ValueMember = "PKey";
                                      refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].DisplayMember = "PValue";
                                      refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].PCallBack = rCallBackPREx;
                                      refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Datasource = ListValues;
                                  } break;
                          }
                      } break;
              }

              InspectorFieldsInfoList.Add(new InspectorInitInformation(FieldType_, Type_, DisplayName,
                      FieldName, Discription, refPropertyGrid.Item[refPropertyGrid.Item.Count - 1], FieldName, QuerStr, rPosID));
          }*/

        public void Add(EInspectorModes inDisplayMode,
                        EInspectorTypes inValueTypes,
                        string inDisplayName,
                        string inCategory,
                        string inDescription,
                        string inFieldName,
                        string inSQLQuery,
                        string inRFCName)
        {
            switch (inDisplayMode)
            {
                case EInspectorModes.Reference:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfReferenceString:
                            case EInspectorTypes.TypeOfReferenceInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (string)"", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].OnClick += this.CustomEventItem_OnClick;
                                } break;
                        }
                    } break;
                case EInspectorModes.Simple:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfDate:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, DateTime.MinValue, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterDateTime();
                                } break;
                            case EInspectorTypes.TypeOfInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (int)0, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterInt32();
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Value = "";
                                } break;
                            case EInspectorTypes.TypeOfDouble:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (double)0.00, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterDouble();
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Value = "";
                                } break;
                            case EInspectorTypes.TypeOfStringLike:
                            case EInspectorTypes.TypeOfString:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (string)"", false, inCategory, inDescription, true);
                                } break;
                        }
                    } break;
                case EInspectorModes.ExpandType:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfExpandDate:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (object)(new CExpandCustomType<DateTime>()), false, inCategory, inDescription, true);
                                } break;
                            case EInspectorTypes.TypeOfExpandInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (object)(new CExpandCustomType<long>()), false, inCategory, inDescription, true);
                                } break;
                        }
                    } break;
                case EInspectorModes.ListBox:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfListInt:
                                {
                                    refDBSql.SQLScript = (inSQLQuery.Replace("select id", " select null as id,'' as name union select id"));
                                    DataTable TmpDS = refDBSql.FillDataSet().Tables[0];
                                    CListKeyValue<string, long>[] ListValues = new CListKeyValue<string, long>[TmpDS.Rows.Count];
                                    int i = 0;
                                    foreach (DataRow row in TmpDS.Rows)
                                    {
                                        ListValues[i++] = new CListKeyValue<string, long>(Convert.ToString(row["name"]), Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]));
                                    }
                                    refPropertyGrid.Item.Add(inDisplayName, "", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].ValueMember = "PKey";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].DisplayMember = "PValue";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Datasource = ListValues;
                                } break;
                            case EInspectorTypes.TypeOfListString:
                                {
                                    refDBSql.SQLScript = inSQLQuery;
                                    DataTable TmpDS = refDBSql.FillDataSet().Tables[0];
                                    CListKeyValue<string, string>[] ListValues = new CListKeyValue<string, string>[TmpDS.Rows.Count];
                                    int i = 0;
                                    foreach (DataRow row in TmpDS.Rows)
                                    {
                                        ListValues[i++] = new CListKeyValue<string, string>(Convert.ToString(row[1]), Convert.ToString(row[0]));
                                    }
                                    refPropertyGrid.Item.Add(inDisplayName, "", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].ValueMember = "PKey";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].DisplayMember = "PValue";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Datasource = ListValues;
                                } break;
                        }
                    } break;
            }

            InspectorFieldsInfoList.Add(new InspectorInitInformation(inDisplayMode, inValueTypes, inDisplayName,
                                                                    inFieldName, inDescription,
                                                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1],
                                                                    inFieldName, inSQLQuery, inRFCName));
        }


        public void Add(EInspectorModes inDisplayMode,
                        EInspectorTypes inValueTypes,
                        string inDisplayName,
                        string inCategory,
                        string inDescription,
                        string inFieldName,
                        string inSQLQuery,
                        string inRFCName,
                        Boolean inRFCEnableMultiSelect)
        {
            switch (inDisplayMode)
            {
                case EInspectorModes.Reference:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfReferenceString:
                            case EInspectorTypes.TypeOfReferenceInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (string)"", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].OnClick += this.CustomEventItem_OnClick;
                                } break;
                        }
                    } break;
                case EInspectorModes.Simple:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfDate:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, DateTime.MinValue, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterDateTime();
                                } break;
                            case EInspectorTypes.TypeOfInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (int)0, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterInt32();
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Value = "";
                                } break;
                            case EInspectorTypes.TypeOfDouble:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (double)0.00, false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].CustomTypeConverter = new CTypeConverterDouble();
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Value = "";
                                } break;
                            case EInspectorTypes.TypeOfStringLike:
                            case EInspectorTypes.TypeOfString:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (string)"", false, inCategory, inDescription, true);
                                } break;
                        }
                    } break;
                case EInspectorModes.ExpandType:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfExpandDate:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (object)(new CExpandCustomType<DateTime>()), false, inCategory, inDescription, true);
                                } break;
                            case EInspectorTypes.TypeOfExpandInt:
                                {
                                    refPropertyGrid.Item.Add(inDisplayName, (object)(new CExpandCustomType<long>()), false, inCategory, inDescription, true);
                                } break;
                        }
                    } break;
                case EInspectorModes.ListBox:
                    {
                        switch (inValueTypes)
                        {
                            case EInspectorTypes.TypeOfListInt:
                                {
                                    refDBSql.SQLScript = (inSQLQuery.Replace("select id", " select null as id,'' as name union select id"));
                                    DataTable TmpDS = refDBSql.FillDataSet().Tables[0];

                                    CListKeyValue<string, long>[] ListValues = new CListKeyValue<string, long>[TmpDS.Rows.Count];

                                    int i = 0;
                                    foreach (DataRow row in TmpDS.Rows)
                                    {
                                        ListValues[i++] = new CListKeyValue<string, long>(Convert.ToString(row["name"]), Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]));
                                    }
                                    refPropertyGrid.Item.Add(inDisplayName, "", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].ValueMember = "PKey";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].DisplayMember = "PValue";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Datasource = ListValues;
                                } break;
                            case EInspectorTypes.TypeOfListString:
                                {
                                    refDBSql.SQLScript = inSQLQuery;
                                    DataTable TmpDS = refDBSql.FillDataSet().Tables[0];

                                    CListKeyValue<string, string>[] ListValues = new CListKeyValue<string, string>[TmpDS.Rows.Count];

                                    int i = 0;
                                    foreach (DataRow row in TmpDS.Rows)
                                    {
                                        ListValues[i++] = new CListKeyValue<string, string>(Convert.ToString(row[0]), Convert.ToString(row[1]));
                                    }
                                    refPropertyGrid.Item.Add(inDisplayName, "", false, inCategory, inDescription, true);
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].ValueMember = "PKey";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].DisplayMember = "PValue";
                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1].Datasource = ListValues;
                                } break;
                        }
                    } break;
            }

            InspectorFieldsInfoList.Add(new InspectorInitInformation(inDisplayMode, inValueTypes, inDisplayName,
                                                                    inFieldName, inDescription,
                                                                    refPropertyGrid.Item[refPropertyGrid.Item.Count - 1],
                                                                    inFieldName, inSQLQuery, inRFCName, inRFCEnableMultiSelect));
        }




        //--------------------------------------------------------------------------------------------------------------
        public void DeinitPossition(int rPosID)
        {
            /*  oDBServer.PCDBServer.SetCommand(" select null as id,'' as name");
              DataTable TmpDS = oDBServer.PCDBServer.ExecuteDataTable();

              CListKeyValue<string, long>[] ListValues = new CListKeyValue<string, long>[TmpDS.Rows.Count];

              int i = 0;
              foreach (DataRow row in TmpDS.Rows)
              {
                  ListValues[i++] = new CListKeyValue<string, long>(Convert.ToString(row["name"]),
                          Convert.ToInt32(row["id"]));
              }
              refPropertyGrid.Item[rPosID].Value = "";
              refPropertyGrid.Item[rPosID].Datasource = ListValues;
              refPropertyGrid.Refresh();*/
        }
        //--------------------------------------------------------------------------------------------------------------
        public void InitPossition(int rPosID, string Param)
        {
            /*  oDBServer.PCDBServer.SetCommand((InspectorFieldsInfoList[rPosID].PQuerStr + Param).Replace("select id", " select null as id,'' as name union select id").
                          Replace("select distinct p.id", " select null as id,'' as name union select p.id"));
              DataTable TmpDS = oDBServer.PCDBServer.ExecuteDataTable();

              CListKeyValue<string, long>[] ListValues = new CListKeyValue<string, long>[TmpDS.Rows.Count];

              int i = 0;
              foreach (DataRow row in TmpDS.Rows)
              {
                  ListValues[i++] = new CListKeyValue<string, long>(Convert.ToString(row["name"]),
                          Convert.ToInt32(row["id"]));
              }
              refPropertyGrid.Item[rPosID].Value = "";
              refPropertyGrid.Item[rPosID].Datasource = ListValues;
              refPropertyGrid.Refresh();*/
        }
        //--------------------------------------------------------------------------------------------------------------

        //--------------------------------------------------------------------------------------------------------------
        public void ClearPropertyGrid()
        {
            for (int i = 0; i < InspectorFieldsInfoList.Count; ++i)
            {
                switch (InspectorFieldsInfoList[i].PDisplayMode)
                {
                    case EInspectorModes.Reference:
                    case EInspectorModes.Simple:
                    case EInspectorModes.ListBox:
                        {
                            switch (InspectorFieldsInfoList[i].PValueTypes)
                            {
                                case EInspectorTypes.TypeOfDate:
                                    {
                                        InspectorFieldsInfoList[i].PInsItem.Value = DateTime.MinValue;
                                    } break;
                                case EInspectorTypes.TypeOfInt:
                                case EInspectorTypes.TypeOfString:
                                case EInspectorTypes.TypeOfDouble:
                                case EInspectorTypes.TypeOfReferenceInt:
                                case EInspectorTypes.TypeOfReferenceString:                                
                                case EInspectorTypes.TypeOfStringLike:
                                    {
                                        InspectorFieldsInfoList[i].PInsItem.Value = "";
                                    } break;
                                case EInspectorTypes.TypeOfListString:
                                case EInspectorTypes.TypeOfListInt:
                                    {
                                        InspectorFieldsInfoList[i].PInsItem.Value = "";
                                        InspectorFieldsInfoList[i].PInsItem.SelectedItem = null;
                                        InspectorFieldsInfoList[i].PInsItem.SelectedValue = null;
                                    } break;
                            }
                        } break;
                    case EInspectorModes.ExpandType:
                        {
                            switch (InspectorFieldsInfoList[i].PValueTypes)
                            {
                                case EInspectorTypes.TypeOfExpandDate:
                                    {
                                        InspectorFieldsInfoList[i].PInsItem.Value = (object)(new CExpandCustomType<DateTime>());
                                    } break;
                                case EInspectorTypes.TypeOfExpandInt:
                                    {
                                        InspectorFieldsInfoList[i].PInsItem.Value = (object)(new CExpandCustomType<long>());
                                    } break;
                            }
                        } break;
                }
            }
            refPropertyGrid.Refresh();
        }
        //--------------------------------------------------------------------------------------------------------------
        public object GetValueByIndex(int inIndex)
        {
            object oReturnValue = null;

            InspectorInitInformation tmpItem;
            try
            {
                tmpItem = (InspectorInitInformation)PInspectorFieldsInfoList[inIndex];
            }
            catch
            {
                return oReturnValue;
            }

            if (tmpItem.PDisplayMode == EInspectorModes.Simple && tmpItem.PValueTypes == EInspectorTypes.TypeOfDate)
            {
                if (Convert.ToDateTime(tmpItem.PInsItem.Value) != DateTime.MinValue)
                {
                    oReturnValue = tmpItem.PInsItem.Value.ToString().Substring(0, 10);
                }
            }
            else if ((tmpItem.PDisplayMode == EInspectorModes.Simple || tmpItem.PDisplayMode == EInspectorModes.Reference) && Convert.ToString(tmpItem.PInsItem.Value).Trim() != "")
            {
                oReturnValue = tmpItem.PInsItem.Value.ToString();
            }
            else if (tmpItem.PDisplayMode == EInspectorModes.ListBox && Convert.ToString(tmpItem.PInsItem.Value).Trim() != "")
            {
                oReturnValue = tmpItem.PInsItem.SelectedValue;
            }
            else if (tmpItem.PDisplayMode == EInspectorModes.ExpandType && tmpItem.PValueTypes == EInspectorTypes.TypeOfExpandDate)
            {
                if (((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement == DateTime.MinValue
                    && ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement == DateTime.MinValue)
                {
                    oReturnValue = null;
                }
                else if (((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement != DateTime.MinValue
                         && ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement != DateTime.MinValue)
                {
                    oReturnValue = ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement.ToShortDateString() + ";" + ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement.ToShortDateString();
                }
                else if (((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement != DateTime.MinValue
                         && ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement == DateTime.MinValue)
                {
                    oReturnValue = ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement.ToShortDateString();
                }
                else if (((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).FirstElement == DateTime.MinValue
                         && ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement != DateTime.MinValue)
                {
                    oReturnValue = ((CExpandCustomType<DateTime>)(tmpItem.PInsItem.Value)).LasElement.ToShortDateString();
                }
            }
            else if (tmpItem.PDisplayMode == EInspectorModes.ExpandType && tmpItem.PValueTypes == EInspectorTypes.TypeOfExpandInt)
            {
                oReturnValue = Convert.ToString(((CExpandCustomType<long>)(tmpItem.PInsItem.Value)).FirstElement) + ";" + Convert.ToString(((CExpandCustomType<long>)(tmpItem.PInsItem.Value)).LasElement);
            }

            return oReturnValue;
        }
        //--------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> GetDictionaryValues() //для менеджера отчетов
        {     
            Dictionary<string, string> dict = new Dictionary<string,string>();

            for (int i = 0; i < InspectorFieldsInfoList.Count; ++i)
            {
                if (InspectorFieldsInfoList[i].PDisplayMode == EInspectorModes.Simple &&
                    InspectorFieldsInfoList[i].PValueTypes == EInspectorTypes.TypeOfDate)
                {
                    if (Convert.ToDateTime(InspectorFieldsInfoList[i].PInsItem.Value) == DateTime.MinValue)
                    {
                        dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), "");
                    }
                    else
                    {
                        dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), InspectorFieldsInfoList[i].PInsItem.Value.ToString().Substring(0, 10));
                    }
                }
                else
                {
                    switch (InspectorFieldsInfoList[i].PDisplayMode)
                    {
                        case EInspectorModes.Simple:
                            {
                                switch (InspectorFieldsInfoList[i].PValueTypes)
                                {
                                    case EInspectorTypes.TypeOfDouble:
                                    case EInspectorTypes.TypeOfInt:
                                    case EInspectorTypes.TypeOfString:
                                    case EInspectorTypes.TypeOfStringLike:
                                        {
                                            dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), InspectorFieldsInfoList[i].PInsItem.Value.ToString());
                                        } break;
                                }
                            } break;
                        case EInspectorModes.ExpandType:
                            {
                                switch (InspectorFieldsInfoList[i].PValueTypes)
                                {
                                    case EInspectorTypes.TypeOfExpandDate:
                                        {
                                            if (((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement == DateTime.MinValue
                                                && ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement == DateTime.MinValue)
                                            {
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "F", "");
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "L", "");
                                            }
                                            else if (((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement != DateTime.MinValue
                                                     && ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement != DateTime.MinValue)
                                            {
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "F", ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement.ToShortDateString());
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "L", ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement.ToShortDateString());
                                            }
                                            else if (((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement != DateTime.MinValue
                                                     && ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement == DateTime.MinValue)
                                            {
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "F", ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement.ToShortDateString());
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "L", "");
                                            }
                                            else if (((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement == DateTime.MinValue
                                                     && ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement != DateTime.MinValue)
                                            {
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "F", "");
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "L", ((CExpandCustomType<DateTime>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement.ToShortDateString());
                                            }
                                        } break;
                                    case EInspectorTypes.TypeOfExpandInt:
                                        {
                                            string f, l = "";
                                            f = Convert.ToString(((CExpandCustomType<long>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement).Equals("0") ? "" : Convert.ToString(((CExpandCustomType<long>)(InspectorFieldsInfoList[i].PInsItem.Value)).FirstElement);
                                            l = Convert.ToString(((CExpandCustomType<long>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement).Equals("0") ? "" : Convert.ToString(((CExpandCustomType<long>)(InspectorFieldsInfoList[i].PInsItem.Value)).LasElement);
                                            dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "F", f);
                                            dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper() + "L", l);
                                        } break;
                                }
                            } break;
                        case EInspectorModes.ListBox:
                            {
                                switch (InspectorFieldsInfoList[i].PValueTypes)
                                {
                                    case EInspectorTypes.TypeOfListString:
                                        {
                                            try
                                            {
                                                string t = "";
                                                t = ((CListKeyValue<string, string>)(InspectorFieldsInfoList[i].PInsItem.SelectedItem)).PKey.Equals("0") ? "" : ((CListKeyValue<string, string>)(InspectorFieldsInfoList[i].PInsItem.SelectedItem)).PKey;
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), t);
                                            }
                                            catch { dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), ""); }
                                        } break;
                                    case EInspectorTypes.TypeOfListInt:
                                        {
                                            try
                                            {
                                                string j = "";
                                                j = ((CListKeyValue<string, long>)(InspectorFieldsInfoList[i].PInsItem.SelectedItem)).PKey.ToString().Equals("0") ? "" : ((CListKeyValue<string, long>)(InspectorFieldsInfoList[i].PInsItem.SelectedItem)).PKey.ToString();
                                                dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), j);
                                            }
                                            catch { dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), ""); }
                                        } break;
                                }
                            } break;
                        case EInspectorModes.Reference:
                            {
                                switch (InspectorFieldsInfoList[i].PValueTypes)
                                {
                                    case EInspectorTypes.TypeOfReferenceString:
                                    case EInspectorTypes.TypeOfReferenceInt:
                                        {
                                            dict.Add(InspectorFieldsInfoList[i].PFieldName.ToUpper(), Convert.ToString(InspectorFieldsInfoList[i].PInsItem.Value));
                                        } break;
                                }
                            } break;
                    }
                }
            }

            return dict;
        }
        //------------------------------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------------------------------
}
