using CSCommon;
using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public class OffPlayerData
    {
        public CSCommon.Data.RoleValue value;// = new CSCommon.Data.RoleValue();
        public int WeapFacdeid;
        public List<CSCommon.Data.SkillData> skills = new List<CSCommon.Data.SkillData>();

        public byte[] Serialize()
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            value.Serialize(dw);
            dw.Write(WeapFacdeid);
            RPC.IAutoSaveAndLoad.DaraWriteList<CSCommon.Data.SkillData>(skills, dw, false);
            return dw.Trim();
        }

        public void Deserizle(CSCommon.Data.RoleCom rc)
        {
            var buffer = rc.FinalValue;
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            value.Deserizle(dr);
            WeapFacdeid = dr.ReadInt32();
            RPC.IAutoSaveAndLoad.DaraReadList<CSCommon.Data.SkillData>(skills, dr, false);
        }
    }

    public partial class PlayerInstance : RPC.RPCObject
    {
        #region 获得数据
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetPathFinderPoints(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            //if (null == HostMap)
            //{
            //    retPkg.Write((SByte)(-1));//该玩家没有在任何地图
            //    retPkg.DoReturnPlanes2Client(fwd);
            //    return;
            //}
            //UInt16 count = (UInt16)HostMap.NpcDictionary.Count;
            //retPkg.Write((SByte)(1));
            //retPkg.Write(count);
            //foreach (var i in HostMap.NpcDictionary)
            //{
            //    var name = System.String.Format("[Text@Param@R=0@G=0@B=255,{0}]", i.Value.NPCData.Name);
            //    retPkg.Write(name);
            //    retPkg.Write(i.Value.NPCData.Position);
            //}

            //count = (UInt16)HostMap.MapData.m_polygonDatas.Count;
            //retPkg.Write(count);
            //foreach (var i in HostMap.MapData.m_polygonDatas)
            //{
            //    var name = System.String.Format("[Text@Param@R=0@G=255@B=255,传送点:{0}]", i.Name);
            //    retPkg.Write(name);
            //    float cx = 0;
            //    float cy = 0;
            //    foreach (var j in i.PointList)
            //    {
            //        cx += i.PointList[0].X;
            //        cy += i.PointList[0].Y;
            //    }
            //    cx /= (float)i.PointList.Count;
            //    cy /= (float)i.PointList.Count;
            //    retPkg.Write(cx);
            //    retPkg.Write(cy);
            //}
            retPkg.DoReturnPlanes2Client(fwd);
            return;
        }

        // 取得玩家的基本信息
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetChiefRoleCalValue(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            InitSkillTime();
            WriteRoleValueToClient(fwd, retPkg);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_AddRoleBasePoint(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            if (this.PlayerData.RoleDetail.RemainPoint <= 0)
            {
                retPkg.Write((sbyte)-1);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            retPkg.Write((sbyte)1);
            _AddBasePoint(type);
            this.PlayerData.RoleDetail.RemainPoint--;
            CalcAllValues();
            WriteRoleValueToClient(fwd, retPkg);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        public void _AddBasePoint(byte type)
        {
            switch ((eEquipValueType)type)
            {
                case eEquipValueType.Power:
                    this.PlayerData.RoleDetail.PowerPoint++;
                    break;
                case eEquipValueType.Body:
                    this.PlayerData.RoleDetail.BodyPoint++;
                    break;
                case eEquipValueType.Dex:
                    this.PlayerData.RoleDetail.DexPoint++;
                    break;
                default:
                    break;
            }

        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_ResetRoleValue(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            this.PlayerData.RoleDetail.RemainPoint += this.PlayerData.RoleDetail.PowerPoint;
            this.PlayerData.RoleDetail.RemainPoint += this.PlayerData.RoleDetail.BodyPoint;
            this.PlayerData.RoleDetail.RemainPoint += this.PlayerData.RoleDetail.DexPoint;
            this.PlayerData.RoleDetail.PowerPoint = 0;
            this.PlayerData.RoleDetail.BodyPoint = 0;
            this.PlayerData.RoleDetail.DexPoint = 0;
            CalcAllValues();
            WriteRoleValueToClient(fwd, retPkg);
        }

        private void WriteRoleValueToClient(RPC.RPCForwardInfo fwd, RPC.PackageWriter retPkg)
        {
            ////一级属性
            //retPkg.Write(FinalPoint.Power);
            //retPkg.Write(FinalPoint.Body);
            //retPkg.Write(FinalPoint.Dex);
            ////二级属性
            //retPkg.Write(mFinalRoleValue.Atk);
            //retPkg.Write(mFinalRoleValue.MaxHP);
            //retPkg.Write(mFinalRoleValue.MaxMP);
            //retPkg.Write(mFinalRoleValue.Hit);
            //retPkg.Write(mFinalRoleValue.Dodge);
            //retPkg.Write(mFinalRoleValue.Crit);
            //retPkg.Write(mFinalRoleValue.CritDef);
            //retPkg.Write(mFinalRoleValue.DeadlyHitRate);
            //retPkg.Write(mFinalRoleValue.Def[0]);
            //retPkg.Write(mFinalRoleValue.Def[1]);
            //retPkg.Write(mFinalRoleValue.Def[2]);
            //retPkg.Write(mFinalRoleValue.Def[3]);
            //retPkg.Write(mFinalRoleValue.Def[4]);
            //retPkg.Write(mFinalRoleValue.UpHurtRate);
            //retPkg.Write(mFinalRoleValue.DownHurtRate);
            //retPkg.Write(mFinalRoleValue.UnusualDefRate);

            //一级属性
            retPkg.Write(FinalRoleValue.Power);
            retPkg.Write(FinalRoleValue.Body);
            retPkg.Write(FinalRoleValue.Dex);
            //二级属性
            retPkg.Write(FinalRoleValue.Atk);
            retPkg.Write(FinalRoleValue.MaxHP);
            retPkg.Write(FinalRoleValue.MaxMP);
            retPkg.Write(FinalRoleValue.Hit);
            retPkg.Write(FinalRoleValue.Dodge);
            retPkg.Write(FinalRoleValue.Crit);
            retPkg.Write(FinalRoleValue.CritDef);
            retPkg.Write(FinalRoleValue.DeadlyHitRate);
            retPkg.Write(FinalRoleValue.GetDef(eElemType.Gold));
            retPkg.Write(FinalRoleValue.GetDef(eElemType.Wood));
            retPkg.Write(FinalRoleValue.GetDef(eElemType.Water));
            retPkg.Write(FinalRoleValue.GetDef(eElemType.Fire));
            retPkg.Write(FinalRoleValue.GetDef(eElemType.Earth));
            retPkg.Write(FinalRoleValue.UpHurtRate);
            retPkg.Write(FinalRoleValue.DownHurtRate);
            retPkg.Write(FinalRoleValue.UnusualDefRate);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        // 取得玩家的基本信息
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetPlayerInfo(ulong id, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            PlayerInstance player = HostMap.FindPlayer(id);
            if (player == null)
            {
                retPkg.Write((sbyte)(-1));
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            retPkg.Write((sbyte)1);
            ////一级属性
            //retPkg.Write(player.FinalPoint.Power);
            //retPkg.Write(player.FinalPoint.Body);
            //retPkg.Write(player.FinalPoint.Dex);
            ////二级属性
            //retPkg.Write(player.mFinalRoleValue.Atk);
            //retPkg.Write(player.mFinalRoleValue.MaxHP);
            //retPkg.Write(player.mFinalRoleValue.MaxMP);
            //retPkg.Write(player.mFinalRoleValue.Hit);
            //retPkg.Write(player.mFinalRoleValue.Dodge);
            //retPkg.Write(player.mFinalRoleValue.Crit);
            //retPkg.Write(player.mFinalRoleValue.CritDef);
            //retPkg.Write(player.mFinalRoleValue.DeadlyHitRate);
            //retPkg.Write(player.mFinalRoleValue.Def[0]);
            //retPkg.Write(player.mFinalRoleValue.Def[1]);
            //retPkg.Write(player.mFinalRoleValue.Def[2]);
            //retPkg.Write(player.mFinalRoleValue.Def[3]);
            //retPkg.Write(player.mFinalRoleValue.Def[4]);
            //retPkg.Write(player.mFinalRoleValue.UpHurtRate);
            //retPkg.Write(player.mFinalRoleValue.DownHurtRate);
            //retPkg.Write(player.mFinalRoleValue.UnusualDefRate);

            retPkg.Write(player.FinalRoleValue.Power);
            retPkg.Write(player.FinalRoleValue.Body);
            retPkg.Write(player.FinalRoleValue.Dex);
            //二级属性
            retPkg.Write(player.FinalRoleValue.Atk);
            retPkg.Write(player.FinalRoleValue.MaxHP);
            retPkg.Write(player.FinalRoleValue.MaxMP);
            retPkg.Write(player.FinalRoleValue.Hit);
            retPkg.Write(player.FinalRoleValue.Dodge);
            retPkg.Write(player.FinalRoleValue.Crit);
            retPkg.Write(player.FinalRoleValue.CritDef);
            retPkg.Write(player.FinalRoleValue.DeadlyHitRate);
            retPkg.Write(player.FinalRoleValue.GetDef(eElemType.Gold));
            retPkg.Write(player.FinalRoleValue.GetDef(eElemType.Wood));
            retPkg.Write(player.FinalRoleValue.GetDef(eElemType.Water));
            retPkg.Write(player.FinalRoleValue.GetDef(eElemType.Fire));
            retPkg.Write(player.FinalRoleValue.GetDef(eElemType.Earth));
            retPkg.Write(player.FinalRoleValue.UpHurtRate);
            retPkg.Write(player.FinalRoleValue.DownHurtRate);
            retPkg.Write(player.FinalRoleValue.UnusualDefRate);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        // 取得NPC的基本信息
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetNPCInfo(ulong id, RPC.RPCForwardInfo fwd)
        {
//             RPC.PackageWriter retPkg = new RPC.PackageWriter();
// 
//             NPCInstance npc = HostMap.GetNPC(id);
//             if (npc == null)
//             {
//                 retPkg.Write((SByte)(-1));
//                 retPkg.DoReturnPlanes2Client(fwd);
//                 return;
//             }
// 
//             retPkg.Write((SByte)1);
//             //var npcTemplate = CSCommon.Data.RoleTemplateManager.Instance.FindRoleTemplate(npc.NPCData.TemplateId);
//             retPkg.Write(npc.NPCData);
//             
//             retPkg.DoReturnPlanes2Client(fwd);
        }
        #endregion

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SelectCamp(byte camp, RPC.RPCForwardInfo fwd)
        {
            if (PlayerData.RoleDetail.Camp != (byte)eCamp.None)
            {
                return;
            }
            PlayerData.RoleDetail.Camp = camp;

            this.DispatchEvent(EventType.SelectCamp, camp);

            //TaskManager.mCurTask.SelectCamp(camp);
            //跳转地图
            string maps = mCurTask.TaskData.Template.arg3;
            List<ushort> ids = new List<ushort>();
            var strids = maps.Split('|');
            foreach (var i in strids)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    ids.Add(Convert.ToUInt16(i));
                }
            }
            if (camp > ids.Count)
            {
                Log.Log.Common.Info("SelectCamp tpl error");
                return;
            }
            ushort mapid = ids[((int)camp - 1)];
            var x = CSTable.StaticDataManager.Maps[mapid].startX;
            var y = CSTable.StaticDataManager.Maps[mapid].startY;
            JumpToMap(mapid, x, 0, y, fwd);
            return;
        }

    }
}