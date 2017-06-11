using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_FetchCity(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.SetSinglePkg();
            byte succeed = 1;
            retPkg.Write(succeed);
            retPkg.Write((UInt16)1);
            for (int i = 0; i < 1; i++)
            {
                CSCommon.Data.CampForceData data = new CSCommon.Data.CampForceData();
                data.CityId = 1;
                data.Camp = 1;
                data.MyForce = 500;
                retPkg.Write(data);
            }
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_FetchAddForceShortcutData(int cityId, RPC.RPCForwardInfo fwd)
        {
            byte succeed = 1;
            int Level = 1;
            int cityTotalForce = 400000;
            int cityMyForceNum = 20000;
            int addForceTime = 3;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write(succeed);
            retPkg.Write(Level);
            retPkg.Write(cityTotalForce);
            retPkg.Write(cityMyForceNum);
            retPkg.Write(addForceTime);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SuggestCamp(RPC.RPCForwardInfo fwd)
        {
            byte succeed = 1;
            byte camp = (byte)eCamp.Jing;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write(succeed);
            retPkg.Write(camp);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_FetchBarrack(RPC.RPCForwardInfo fwd)
        {
            byte succeed = 1;
            int totalForce = 50000;
            int captiveNum = 50000;
            int efficiency = 200;
            int level = 1;
            int forceLevel = 150;
            int totalStationed = 100000;
            int todayStationed = 10000;
            List<CSCommon.Data.CityForceData> cityData = new List<CityForceData>();
            for (int i = 1; i <= 1; ++i)
            {
                CityForceData data = new CityForceData();
                data.CityId = i;
                data.TotalForce = i * 10000;
                data.MyForceNum = i * 1000;
                cityData.Add(data);
            }
            int stationExp = 9999;
            int damageExp = 8888;
            int damageExpTime = 20;
            int damageItem = 10;
            int damageItemTime = 5;
            int addForceTime = 2;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write(succeed);
            retPkg.Write(level);
            retPkg.Write(forceLevel);
            retPkg.Write(totalForce);
            retPkg.Write(captiveNum);
            retPkg.Write(efficiency);
            retPkg.Write(totalStationed);
            retPkg.Write(todayStationed);
            retPkg.Write((UInt16)cityData.Count);
            for (int i = 0; i < cityData.Count; ++i)
            {
                retPkg.Write(cityData[i].CityId);
                retPkg.Write(cityData[i].TotalForce);
                retPkg.Write(cityData[i].MyForceNum);
            }
            retPkg.Write(stationExp);
            retPkg.Write(damageExp);
            retPkg.Write(damageExpTime);
            retPkg.Write(damageItem);
            retPkg.Write(damageItemTime);
            retPkg.Write(addForceTime);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_AddForce(int cityId, byte forceType, RPC.RPCForwardInfo fwd)
        {
            int succeed = 1;
            int totalForce = 100000000;
            int totalStationed = 300000;
            int todayStationed = 100000;
            int cityTotalForce = 400000;
            int cityMyForceNum = 20000;
            int addForceTime = 3;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write(succeed);
            retPkg.Write(totalForce);
            retPkg.Write(totalStationed);
            retPkg.Write(todayStationed);
            retPkg.Write(cityTotalForce);
            retPkg.Write(cityMyForceNum);
            retPkg.Write(addForceTime);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_AddForceShortcut(int cityId, byte forceType, RPC.RPCForwardInfo fwd)
        {
            byte succeed = 1;
            int cityTotalForce = 800000;
            int cityMyForceNum = 40000;
            int addForceTime = 5;
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write(succeed);
            retPkg.Write(cityTotalForce);
            retPkg.Write(cityMyForceNum);
            retPkg.Write(addForceTime);
            retPkg.DoReturnPlanes2Client(fwd);
        }
    }
}