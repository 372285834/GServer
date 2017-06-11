using CSTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerCommon
{

    public static class DataConst
    {
        //换行符;
        public const string cSignEnter = "\r\n";
        //制表符;
        public const char cSignTab = '\t';

        //真实数据开始行
        public const int cDataStartLine = 4;
    }

    //public class RelationInfo
    //{
    //    public string ParentTableName;
    //    public string SubTableName;
    //    public string SubColName;
    //    public DataColumn ParentCol;
    //}

    //public class ITemplateMgrLoader
    //{
    //    void OnLoadTable(string tableName,DataRowCollection rows);
    //    void OnAllLoadEnd()
    //    {
    //        var s = TemplateTableLoader.mDataSet.Tables["aa"].Rows[0][""];
    //        DataTable tb;
    //        var rows = tb.Select("id>3");
    //        //mDataSet.Tables["aa"].Select();
    //        DataColumn cl;
    //        DataRow row;
    //        int id = (int)row["ID"];
    //    }
    //}

    public class TemplateTableLoader
    {
        /// <summary>
        /// 仅服务器使用
        /// </summary>
        /// <param name="path"></param>
        public static void LoadTable(string path)
        {
            mDataSet.Clear();
            DirectoryInfo parentdi = new DirectoryInfo(path);
            StaticDataManager.registerModel();
            var files = parentdi.GetFiles("*.txt");
            //List<RelationInfo> relations = new List<RelationInfo>();
            foreach (var file in files)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                using (var reader = file.OpenText())
                {
                    string dataStr = reader.ReadToEnd();
                    try
                    {
                        if (StaticDataManager.Dic.ContainsKey(name))
                        {
                            IDataModel model = (IDataModel)StaticDataManager.Dic[name];
                            model.initData(dataStr);
                        }
                    }
                    catch (System.Exception e)
                    {
                        
                    }
                }
            }

            //CreateRelation(relations, mDataSet);
        }
        public static DataSet mDataSet = new DataSet();

        public static Type GetTypeByString(string type)
        {
            switch (type.ToLower())
            {
                //浮点型数组;
                case "float[]":
                    return Type.GetType("System.String", true, true);
                //整形数组;
                case "int[]":
                    return Type.GetType("System.String", true, true);
                //长整型数组;
                case "long[]":
                    return Type.GetType("System.String", true, true);
                //字符串数组;
                case "string[]":
                    return Type.GetType("System.String", true, true);
                case "bool":
                    return Type.GetType("System.Boolean", true, true);
                case "byte":
                    return Type.GetType("System.Byte", true, true);
                case "sbyte":
                    return Type.GetType("System.SByte", true, true);
                case "char":
                    return Type.GetType("System.Char", true, true);
                case "decimal":
                    return Type.GetType("System.Decimal", true, true);
                case "double":
                    return Type.GetType("System.Double", true, true);
                case "float":
                    return Type.GetType("System.Single", true, true);
                case "int":
                    return Type.GetType("System.Int32", true, true);
                case "uint":
                    return Type.GetType("System.UInt32", true, true);
                case "long":
                    return Type.GetType("System.Int64", true, true);
                case "ulong":
                    return Type.GetType("System.UInt64", true, true);
                case "object":
                    return Type.GetType("System.Object", true, true);
                case "short":
                    return Type.GetType("System.Int16", true, true);
                case "ushort":
                    return Type.GetType("System.UInt16", true, true);
                case "string":
                    return Type.GetType("System.String", true, true);
                case "date":
                case "datetime":
                    return Type.GetType("System.DateTime", true, true);
                case "guid":
                    return Type.GetType("System.Guid", true, true);
                default:
                    return Type.GetType(type, true, true);
            }
        }
        public static string InterfaceClassName = "";
        /// <summary>
        /// 仅服务器使用
        /// </summary>
        /// <param name="path"></param>
        public static void LoadData(string path)
        {
            mDataSet.Clear();
            DirectoryInfo parentdi = new DirectoryInfo(path);

            var files = parentdi.GetFiles("*.txt");
            //List<RelationInfo> relations = new List<RelationInfo>();
            foreach( var file in files )
            {
                
                ReLoadTable(file);                               
            }

            //CreateRelation(relations, mDataSet);
        }
        public static void ReLoadTable(string fileName, string dataStr)
        {
            System.Data.DataTable tb = new System.Data.DataTable();
            tb.TableName = fileName;
            if (mDataSet.Tables.Contains(tb.TableName))
            {
                mDataSet.Tables.Remove(tb.TableName);
            }
            //using (var reader = file.OpenText())
            {
                //string dataStr = reader.ReadToEnd();

                var rowsStr = dataStr.Split(new string[] { DataConst.cSignEnter }, StringSplitOptions.RemoveEmptyEntries);
                if (rowsStr.Length < 2)
                    return;


                //第二行列名,第三行类型名
                var NoteNamesStr = rowsStr[0].Split(DataConst.cSignTab);
                var ColNamesStr = rowsStr[1].Split(DataConst.cSignTab);
                var typesStr = rowsStr[2].Split(DataConst.cSignTab);
                var paramsStr = rowsStr[3].Split(DataConst.cSignTab);

                List<DataColumn> preCol = new List<DataColumn>();

                for (int i = 0; i < ColNamesStr.Length; i++)
                {
                    var noteName = NoteNamesStr[i];
                    var fieldStr = ColNamesStr[i];
                    var typeStr = typesStr[i];
                    var paramStr = paramsStr[i];
                    if (string.IsNullOrEmpty(typeStr))
                        continue;
                    var col = tb.Columns.Add(fieldStr, GetTypeByString(typeStr));
                    col.Caption = noteName + "\n" + typeStr + "\n" + paramStr;
                    if (string.IsNullOrEmpty(paramStr))
                        continue;
                    if (paramStr.Contains("key"))
                    {
                        preCol.Add(col);
                    }
                }
                tb.PrimaryKey = preCol.ToArray();
                for (int i = DataConst.cDataStartLine; i < rowsStr.Length; i++)
                {
                    AddRow(rowsStr[i], tb);
                }

                mDataSet.Tables.Add(tb);
            }
        }
        public static void ReLoadTable(FileInfo file)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            using(var reader = file.OpenText())
            {
                string dataStr = reader.ReadToEnd();
                ReLoadTable(name, dataStr);
            }
        }
        //public static void CreateRelation(List<RelationInfo> relations,DataSet set)
        //{
        //    foreach (var info in relations)
        //    {
        //        var subTb = set.Tables[info.SubTableName];
        //        if(subTb==null) continue;                    
        //        var subCol = subTb.Columns[info.SubColName];
        //        if(subCol==null) continue;
        //        set.Relations.Add(string.Format("{0}-{1}", info.ParentCol, info.SubColName), subCol, info.ParentCol);

        //    }
        //}


        public static DataRow AddRow(string fieldStr, DataTable tb)
        {
            DataRow row = tb.NewRow();
            var feildsStr = fieldStr.Split(DataConst.cSignTab);

            for (int i = 0; i < row.Table.Columns.Count; i++)
            {
                try
                {
                    row[i] = ConvertFeildsStr(feildsStr[i], tb.Columns[i].DataType);
                }
                catch (System.Exception ex)
                {
                    if (CSCommon.CSLog.LogFun != null)
                        CSCommon.CSLog.LogFun("数据解析错误！table name={0},str={1},colomns={2}", tb.TableName, feildsStr[i], tb.Columns[i].ColumnName);
                    //Log.Log.Common.Print(ex.ToString());
                    //Log.Log.Common.Print(ex.StackTrace.ToString());
                    //Log.FileLog.Instance.Flush();
                }
            }
            tb.Rows.Add(row);
            return row;
        }
        public static object ConvertFeildsStr(string str,Type t)
        {
            if (str=="")
            {                
                if (t == typeof(Double))
                    return 0;
                if (t == typeof(Single))
                    return 0;
                if (t == typeof(Int32))
                    return 0;
                if (t == typeof(UInt32))
                    return 0;
                if (t == typeof(Int64))
                    return 0;
                if (t == typeof(UInt64))
                    return 0;
                if (t == typeof(Int16))
                    return 0;
                if (t == typeof(UInt16))
                    return 0;
                if (t == typeof(Byte))
                    return 0;
                if (t == typeof(SByte))
                    return 0;
                if (t == typeof(Char))
                    return 0;
                if (t == typeof(Decimal))
                    return 0;
                if (t == typeof(String))
                    return "";
                if (t == typeof(DateTime))
                    return DateTime.MinValue;
                if (t == typeof(Guid))
                    return Guid.Empty;
            }
            else
            {
                if (t == typeof(bool))
                {
                    if (str == "1") 
                        return true;
                    return false;
                }
                if (t == typeof(String))
                    return str;
                if (t == typeof(Double))
                    return Convert.ToDouble(str);
                if (t == typeof(Single))
                    return Convert.ToSingle(str);
                if (t == typeof(Int32))
                    return Convert.ToInt32(str);
                if (t == typeof(UInt32))
                    return Convert.ToUInt32(str);
                if (t == typeof(Int64))
                    return Convert.ToInt64(str);
                if (t == typeof(UInt64))
                    return Convert.ToUInt64(str);
                if (t == typeof(Int16))
                    return Convert.ToInt16(str);
                if (t == typeof(UInt16))
                    return Convert.ToUInt16(str);
                if (t == typeof(Byte))
                    return Convert.ToByte(str);
                if (t == typeof(SByte))
                    return Convert.ToSByte(str);
                if (t == typeof(Char))
                    return Convert.ToChar(str);
                if (t == typeof(Decimal))
                    return Convert.ToDecimal(str);
                if (t == typeof(DateTime))
                    return Convert.ToDateTime(str);
                if (t == typeof(Guid))
                    return new Guid(str);
            }



          

            //Log.Log.Common.Print("没有类型：" + str);
            return null;
        }

        
    }
}
