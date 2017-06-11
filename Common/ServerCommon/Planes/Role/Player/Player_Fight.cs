using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFrame;
using SlimDX;

namespace ServerCommon.Planes
{    
    /// <summary>
    /// 技能同步分两种，一种是客户端驱动，告诉服务器释放了一个技能，服务器计算结果，返回客户端技能释放结果
    /// 还一种是服务器驱动，服务器从别的玩家知道放了一个技能，告诉客户端，别的玩家或者怪物放了一个技能，客户端
    /// 需要做技能释放表现
    /// 
    /// 客户端驱动函数在player里面
    /// 服务器驱动函数在roleActor里面
    /// </summary>
    public partial class PlayerInstance : RPC.RPCObject
    {
        public override bool CanAttack(RoleActor target)
        {
            if (!base.CanAttack(target))
                return false;

            if (HostMap.CanAttack(this, target) == eMapAttackRule.AtkOK)
                return true;

            if (target is PlayerInstance)
            {
                if (target == this)
                    return true;

                if (target.Camp == (byte)eCamp.None)
                    return false;
            }
            else if (target is NPCInstance)
            {
                var npc = target as NPCInstance;
                if(npc.NPCData.Template.type == (byte)eNpcType.Npc)
                    return false;

                if (target.Camp == (byte)eCamp.None)
                    return true;
            }

            if (target.Camp == this.Camp)
                return false;
            
            return true;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_CastSpell(ulong targetId, int skillId, RPC.RPCForwardInfo fwd)
        {
            eSkillResult ret = eSkillResult.CastFailed;
            this.CastSpell(skillId, targetId, ref ret);
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.Write((sbyte)ret);
            retPkg.Write(skillId);
            retPkg.Write(targetId);
            retPkg.DoReturnPlanes2Client(fwd);
        }

        //发现客户端作弊的时候调用此函数
        public void OnClientCheat(int level, string reason)
        {

        }

        uint mPrevRPCUpdatePositionTime = 0;
        SlimDX.Vector3 mPrevUpdatePosition;
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_UpdatePosition(SlimDX.Vector3 pos, float dir, RPC.RPCForwardInfo fwd)
        {
            if (CheckHoldBuff())
            {
                Log.Log.Server.Warning("自己被控制，无法移动");
                return;
            }

            pos.Y = 0;

//             this.PlayerData.RoleDetail.LocationX = pos.X;
//             this.PlayerData.RoleDetail.LocationY = pos.Y;
//             this.PlayerData.RoleDetail.LocationZ = pos.Z;
//             this.PlayerData.RoleDetail.Direction = dir;


            this.Placement.SetDirection(dir);
            this.Placement.SetLocation(ref pos);

            //防止客户端告诉不靠谱的位置，这里目前没有处理连续发包，每个包位置差都控制在2米的情况
            uint nowTime = IServer.timeGetTime();
            float elapse = (float)(nowTime - mPrevRPCUpdatePositionTime) * 0.001F;
            if (elapse > 1.0)
            {
                float maxDist = this.RoleTemplate.speed * 2 * elapse;
                float dist = Util.DistanceH(pos, mPrevUpdatePosition);

                if (dist > maxDist)
                {
                    OnClientCheat(50, string.Format("移动速度异常，同步间隔最大移动距离{0}，实际移动距离{1}", maxDist, dist));

                    mPrevRPCUpdatePositionTime = nowTime;
                    mPrevUpdatePosition = pos;
                }
                else
                {
                    mPrevRPCUpdatePositionTime = nowTime;
                    mPrevUpdatePosition = pos;
                }
            }

        }

        public bool CheckHoldBuff()
        {
            if (FightState == eRoleFightState.Stun ||
                FightState == eRoleFightState.LockMove ||
                FightState == eRoleFightState.Sleep)
                return true;

            return false;
        }


        public override void OnDie()
        {
            base.OnDie();

            if (null == mLogic)
                return;

            mLogic.OnKilled(this);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_Relive(byte _mode, RPC.RPCForwardInfo fwd)
        {
            eReliveMode mode = (eReliveMode)_mode;
            Relive(mode);  
        }

        //复活
        public void Relive(eReliveMode mode)
        {
            this.CurHP = this.FinalRoleValue.MaxHP;
            this.CurMP = this.FinalRoleValue.MaxMP;

            var pkg = new RPC.PackageWriter();
            //Wuxia.H_RpcRoot.smInstance.RPC_OnChiefRoleRelive(pkg, GetPosition().X, this.GetPosition().Z);
            //pkg.DoCommandPlanes2Client(Planes2GateConnect, ClientLinkId);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Id).RPC_Relive(pkg, GetPosition().X, this.GetPosition().Z);
            HostMap.SendPkg2Clients(null, this.GetPosition(), pkg);
        }

        public override void OnKillActor(RoleActor actor)
        {
            base.OnKillActor(actor);
            if (actor is NPCInstance)
            {
                var npc = actor as NPCInstance;
                GainExp(npc.AttriData.exp);
                _ChangeMoney(eCurrenceType.Gold,CSCommon.Data.eMoneyChangeType.KillMonster,10);
                //TaskManager.mCurTask.KillMonster(npc.NPCData.TemplateId, 1);
                //CheckCopy(npc.NPCData.TemplateId.ToString());
                //mRecordMgr.CheckAchieve(eAchieveType.KillMonster, npc.NPCData.TemplateId.ToString());
            }

        }

    }
}
