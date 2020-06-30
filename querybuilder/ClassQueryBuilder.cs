using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using GGPlatform.DBServer;
using GGPlatform.RegManager;

namespace GGPlatform.QueryBuilder
{
    public enum TypeReturnScript
    {
        trsScript = 1,
        trsSubQuery = 2
    }
    public class Field
    {
        private string _FieldName = "";
        private string _FieldCaption = "";
        private string _FieldType;
        private int _FieldReference = 0;

        public Field(string inFieldName, string inFieldCaption, string inFieldType, int inFieldReference)
        {
            _FieldName = inFieldName;
            _FieldCaption = inFieldCaption;
            _FieldType = inFieldType;
            _FieldReference = inFieldReference;
        } //констуктор

        public string FieldName { get { return _FieldName; } }
        public string FieldCaption { get { return _FieldCaption; } }
        public string FieldType { get { return _FieldType; } }
        public int FieldReference { get { return _FieldReference; } }
    }

    public class QueryBuilder
    {
        //private DataColumnCollection _sQueryColumns;
        private int _sObjectID = 0;
        private string _sObjectName = "";
        private Field _sColumn = null;
        private ArrayList _sQueryColumns = new ArrayList();
        private IWin32Window _sMainHandle;
        private DBSqlServer _sDBSql = null;
        private frmQueryBuilder fmQueryBuilder = null;
        private string _sSQLScript, _sSQLScriptSubQuery = null;
        private TypeReturnScript _sTypeGetScript;
        private string _AssemblyName = null;
        private string _SoftName = null;

        public QueryBuilder(int inObjectID, IWin32Window inMainHandle, DBSqlServer inDBSql, TypeReturnScript inTypeReturnScript = TypeReturnScript.trsScript)
        {
            _sObjectID = inObjectID;
            _sMainHandle = inMainHandle;
            _sDBSql = inDBSql;
            _sTypeGetScript = inTypeReturnScript;
            ObjectDescription();
        }

        public QueryBuilder(string inObjectName, IWin32Window inMainHandle, DBSqlServer inDBSql, TypeReturnScript inTypeReturnScript = TypeReturnScript.trsScript)
        {            
            _sMainHandle = inMainHandle;
            _sDBSql = inDBSql;
            _sObjectName = inObjectName;
            _sTypeGetScript = inTypeReturnScript;
            ObjectDescription();
        }

        public QueryBuilder(int inObjectID, DBSqlServer inDBSql, TypeReturnScript inTypeReturnScript = TypeReturnScript.trsScript)
        {
            _sObjectID = inObjectID;
            _sDBSql = inDBSql;
            _sTypeGetScript = inTypeReturnScript;
            ObjectDescription();
        }

        public QueryBuilder(string inObjectName, DBSqlServer inDBSql, TypeReturnScript inTypeReturnScript = TypeReturnScript.trsScript)
        {
            _sDBSql = inDBSql;
            _sObjectName = inObjectName;
            _sTypeGetScript = inTypeReturnScript;
            ObjectDescription();
        }

        private void ObjectDescription()
        {
            int countrow = 0;

            string _MaxRowCount = "";
            _sDBSql.SQLScript = "select [value] from GGPlatform.Settings where [key] = 'MaxRowCount'";
            _MaxRowCount = _sDBSql.ExecuteScalar();

            _sDBSql.SQLScript = "select id, ObjectName, ObjectExpression, ObjectExpressionSubQuery from GGPlatform.Objects where ((id = @ObjectsRef) or (ObjectName = @ObjectName))";
            _sDBSql.AddParameter("@ObjectsRef", EnumDBDataTypes.EDT_Integer, _sObjectID.ToString());  
            _sDBSql.AddParameter("@ObjectName", EnumDBDataTypes.EDT_String, _sObjectName.ToString());                                                                                                 
            SqlDataReader Objects = _sDBSql.ExecuteReader();
            while (Objects.Read())
            {
                _sSQLScript = Objects["ObjectExpression"].ToString().Replace("MaxRowCount", _MaxRowCount);
                _sSQLScriptSubQuery = Objects["ObjectExpressionSubQuery"].ToString();
                _sObjectID = Convert.ToInt32(Objects["id"].ToString());
                _sObjectName = Objects["ObjectName"].ToString();
                countrow++;
            }
            if (countrow != 1)
                throw new Exception("Объект фильтрации (по идентификатору или имени объекта) уникально не идентифицирован в БД");

            Objects.Close();

            if (_sDBSql.InfoAboutConnection.UserIsAdmin)
                _sDBSql.SQLScript = "select * from GGPlatform.ObjectsDescription where ObjectsRef = @ObjectsRef order by id";
            else
                _sDBSql.SQLScript = "select * from GGPlatform.ObjectsDescription where ObjectsRef = @ObjectsRef and FieldVisible = 1 order by id";

            _sDBSql.AddParameter("@ObjectsRef", EnumDBDataTypes.EDT_Integer, _sObjectID.ToString());
            SqlDataReader ObjectsDescription = _sDBSql.ExecuteReader();

            string tmpFieldName = "";
            string tmpFieldCaption = "";
            string tmpFieldType = "";
            int tmpFieldReference = 0;

             //остальные поля7
            while (ObjectsDescription.Read())
            {
                tmpFieldName = ObjectsDescription["FieldName"].ToString();
                tmpFieldCaption = ObjectsDescription["FieldCaption"].ToString();
                tmpFieldReference = ObjectsDescription["ObjectsRefOrSubRef"].ToString() == "" ? 0 : Convert.ToInt32(ObjectsDescription["ObjectsRefOrSubRef"].ToString());
                tmpFieldType = "C";
                tmpFieldType = ObjectsDescription["FieldType"].ToString();
                if ((tmpFieldType != "L") &
                    (tmpFieldType != "C") &
                    (tmpFieldType != "D") &
                    (tmpFieldType != "N") &
                    (tmpFieldType != "R") &
                    (tmpFieldType != "Q"))
                    throw new Exception("В настройках фильтра задан неизвестный тип поля");
                               
                _sColumn = new Field(tmpFieldName, tmpFieldCaption, tmpFieldType, tmpFieldReference);
                _sQueryColumns.Add(_sColumn);
            }
            ObjectsDescription.Close();
        }

        public string GetQuery()
        {
            if (fmQueryBuilder == null)
            {
                if (_sTypeGetScript == TypeReturnScript.trsScript)
                    fmQueryBuilder = new frmQueryBuilder(_sMainHandle, _sQueryColumns, _sDBSql, _sSQLScript, _sTypeGetScript);
                if (_sTypeGetScript == TypeReturnScript.trsSubQuery)
                    fmQueryBuilder = new frmQueryBuilder(_sMainHandle, _sQueryColumns, _sDBSql, _sSQLScriptSubQuery, _sTypeGetScript);
            }

            GGLoadQueryBuilderRegistry();                
            if (fmQueryBuilder.ShowDialog(_sMainHandle) == DialogResult.OK)
            {
                _sSQLScript = fmQueryBuilder.generatedSQL; //перезатру прошлое значеие
                GGSaveQueryBuilderRegistry();
                return _sSQLScript;
            }
            else
            {
                GGSaveQueryBuilderRegistry();
                return null;
            }            
        }

        public string GGRegistrySoftName
        {
            set { _SoftName = value; }
        }
        public string GGRegistryAssemblyName
        {
            set { _AssemblyName = value; }
        }
        private void GGLoadQueryBuilderRegistry()
        {
            if ((String.IsNullOrEmpty(_SoftName)) || (String.IsNullOrEmpty(_AssemblyName)))
                return;

            if (fmQueryBuilder == null)
                return;

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _AssemblyName + "\\QueryBuilder\\" + _sObjectID;

            object ob = _reg.GGRegistryGetValue("Width", _p);
            if (ob != null)
               try { fmQueryBuilder.Width = Convert.ToInt32(ob); }
               catch { }

            ob = _reg.GGRegistryGetValue("Height", _p);
            if (ob != null)
                try { fmQueryBuilder.Height = Convert.ToInt32(ob); }
                catch { }

            _reg = null;
        }

        private void GGSaveQueryBuilderRegistry()
        {
            if ((String.IsNullOrEmpty(_SoftName)) || (String.IsNullOrEmpty(_AssemblyName)))
                return;

            if (fmQueryBuilder == null)
                return;

            GGPlatform.RegManager.RegManag _reg = new RegManag();
            string _p = "Software\\GGPlatform\\" + _SoftName + "\\" + _AssemblyName + "\\QueryBuilder\\" + _sObjectID;

            _reg.GGRegistrySetValue("Width", fmQueryBuilder.Width, _p);
            _reg.GGRegistrySetValue("Height", fmQueryBuilder.Height, _p);

            _reg = null;
        }
    }
}
