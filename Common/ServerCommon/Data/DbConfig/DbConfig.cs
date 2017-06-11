////////////////////////////////////////////////////////////////////////////////////
/*
 * 加载数据库中的静态配置信息
*/
////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace CSCommon.Data
{
    public class CDbConfig
    {
        //位面服务器配置信息
        public static Dictionary<int, CPlanesConfig> m_PlanesConfig = new Dictionary<int, CPlanesConfig>();

        //加载数据库中的相关静态配置信息
        public static void LoadDbConfig(ServerFrame.DB.DBConnect dbConnect)
        {
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData("", new CSCommon.Data.CPlanesConfig(), "");
            System.Data.DataTable tab = dbConnect._ExecuteSelect(dbOp, "planesconfig");
            if (tab != null)
            {
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.CPlanesConfig lPlanesConfig = new CSCommon.Data.CPlanesConfig();
                    if (false == ServerFrame.DB.DBConnect.FillObject(lPlanesConfig, r))
                        continue;
                    m_PlanesConfig[lPlanesConfig.PlanesServerId] = lPlanesConfig;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("加载位面管理地图信息失败:" + dbOp.SqlCode);
            }
        }
    }
}