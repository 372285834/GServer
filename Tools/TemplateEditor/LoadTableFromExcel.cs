using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace TemplateEditor
{
    public class LoadTableFromExcel
    {

        static DataSet mDataSet = new DataSet();
        /// <summary>
        /// 读取Excel文件到DataSet中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        /// 
        public static void ToDataTable(string filePath)
        {
            string connStr = "";            
            string fileType = System.IO.Path.GetExtension(filePath);
            string tbName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            if (string.IsNullOrEmpty(fileType))
                return;
 
            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath+ ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [{0}]";
 
            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            DataTable dtSheetName= null;
 
            try
            {
                // 初始化连接，并打开
                conn = new OleDbConnection(connStr);
                conn.Open();
 
                // 获取数据源的表定义元数据                        
                string SheetName = "";
                dtSheetName= conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
 
                // 初始化适配器
                da = new OleDbDataAdapter();

                SheetName = (string)dtSheetName.Rows[0]["TABLE_NAME"];

                if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                {
                    return;
                }

                da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                da.Fill(mDataSet, tbName); 

            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 关闭连接
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
        }

        public static void ExportByOle(string fromPath, string toPath)
        {
            var files = System.IO.Directory.GetFiles(fromPath, "*.xlsx", System.IO.SearchOption.AllDirectories);
            foreach (var filePath in files)
            {
                if (filePath.Contains("~"))
                    continue;
                ToDataTable(filePath);
            }


        }

        


        /// <summary>
        /// 用原始excel方式打开，效率太低了
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        public static void ExportByExcelApp(string fromPath,string toPath)
        {

            var files = System.IO.Directory.GetFiles(fromPath, "*.xlsx", System.IO.SearchOption.AllDirectories);

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.Visible = false;
            foreach (var filePath in files)
            {
                if (filePath.Contains("~"))
                    continue;
                Microsoft.Office.Interop.Excel.Workbook xlBook = excel.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                var workSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlBook.Worksheets.get_Item(1);

                SaveAsTxt(toPath, workSheet, xlBook.Name);

                excel.Workbooks.Close();
            }

            excel.Quit();
            ExportToExcel.KillExcel(excel);
        }

        public static void SaveAsTxt(string savePath, Microsoft.Office.Interop.Excel.Worksheet workSheet,string xlName)
        {
            FileStream saveFile = new System.IO.FileStream(savePath + "\\" + xlName.Split('.')[0] + ".txt", System.IO.FileMode.OpenOrCreate);
            using (StreamWriter sw = new StreamWriter(saveFile))
            {
                int rowIndex = 1;
                while (true)
                {
                    var c1 = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, 1];
                    if (c1 == Type.Missing || c1.Value == null)
                        break;
                    string rowString = "";
                    int colIndex = 1;
                    while (true)
                    {
                        var c = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[rowIndex, colIndex];
                        if (c == Type.Missing || c.Value == null)
                            break;
                        rowString += c.Value + '\t';
                        colIndex++;
                    }
                    sw.WriteLine(rowString);
                    rowIndex++;
                }
            }
        }
    }
}
