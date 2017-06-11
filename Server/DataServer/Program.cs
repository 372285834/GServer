using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
//            string DBConnectStr = System.String.Format("server={0};database=wuxia;uid=root;pwd=123456;", "192.168.1.251");
//            var mConnection = new MySql.Data.MySqlClient.MySqlConnection(DBConnectStr);
//            mConnection.Open();
//            byte[] buffer = new byte[6] { 1, 2, 3, 4, 5, 6 };

//            var sql = @"UPDATE RoleInfo
//                        SET CreateTime = '2014/12/3 19:47:17',aaaa = @tt
//                        WHERE RoleId='22c01381780f41b1bf24e98afb1dde99'";
//            var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, mConnection);
//            cmd.Parameters.Add("@tt", buffer);
//            System.Data.DataSet result = new System.Data.DataSet();
//            var mDataAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();
//            mDataAdapter.SelectCommand = cmd;
//            mDataAdapter.Fill(result, "ttt");


//            mConnection.Close();



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DataServerFrm());
        }
    }
}
