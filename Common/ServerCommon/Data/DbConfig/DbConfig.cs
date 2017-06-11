////////////////////////////////////////////////////////////////////////////////////
/*
 * �������ݿ��еľ�̬������Ϣ
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
        //λ�������������Ϣ
        public static Dictionary<int, CPlanesConfig> m_PlanesConfig = new Dictionary<int, CPlanesConfig>();

        //�������ݿ��е���ؾ�̬������Ϣ
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
                System.Diagnostics.Debug.WriteLine("����λ������ͼ��Ϣʧ��:" + dbOp.SqlCode);
            }
        }
    }
}