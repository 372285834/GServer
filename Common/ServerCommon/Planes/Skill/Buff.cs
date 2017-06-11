using CSCommon;
using CSCommon.Data;
using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ServerCommon.Planes
{
    public class Buff : BuffData
    {
        public bool Tick(long elapsedMillisecond)
        {
            if (mIntervalTime != -1000)
            {
                mIntervalTime -= elapsedMillisecond;
                if (mIntervalTime <= 0)
                {
                    ContinualBuffEffect();
                    if (null == Owner || Owner.IsDie)
                        return false;

                    mIntervalTime = LevelData.affectRate * 1000;
                    mIntervalTime = mIntervalTime != 0 ? mIntervalTime : -1000;
                }
            }

            if (!CheckRemindTime(elapsedMillisecond))
                return false;

            return true;
        }

        /// <summary>
        /// 注册buff影响类型
        /// </summary>
        public void RegisteBuffEffectType()
        {
            foreach (var p in mEffectValue)
            {
                eBuffEffectType type = (eBuffEffectType)p.type;
                if (type == eBuffEffectType.Unknown || p.value == 0) continue;

                if (type == eBuffEffectType.免疫控制)
                {
                    Owner.PlayerStatus.AddImmunityType((int)eImmunityType.眩晕);
                    Owner.PlayerStatus.AddImmunityType((int)eImmunityType.击跪);
                    Owner.PlayerStatus.AddImmunityType((int)eImmunityType.击飞);
                }
             }

            ReComputeAffectValue();
        }

        /// <summary>
        /// 重新计算影响值
        /// </summary>
        public void ReComputeAffectValue()
        {
            foreach (var p in mEffectValue)
            {
                if (p.value == 0) continue;

                eBuffEffectType type = (eBuffEffectType)p.type;
                float tempValue = p.value;
                SetEffectValue(type, tempValue);
                if (LogicType == eBuffLogicType.Fixed)
                {
                    BuffMgr.RefreshAffectValue(type);
                }
            }
        }

        /// <summary>
        /// 检查buff生存时间
        /// </summary>
        public bool CheckRemindTime(long elapsedMillisecond)
        {
            if (mRemindTime == -1000)
                return true;

            mRemindTime -= elapsedMillisecond;
            if (mRemindTime <= 0)
            {
                mRemindTime = 0;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 持续型buff生效
        /// </summary>
        public void ContinualBuffEffect()
        {
            if (null == Owner || Owner.IsDie) return;

            if (LogicType == eBuffLogicType.Continual)
            {
                eBuffEffectType tempType;
                float tempValue;
                foreach (var p in mEffectValue)
                {
                    if (p.value == 0) continue;

                    tempType = (eBuffEffectType)p.type;
                    tempValue = GetEffectValue(tempType);
                    AddResultValue(tempType, tempValue);
                    if (null == Owner || Owner.IsDie)
                        return;
                }
            }
        }

        /// <summary>
        /// 删除buff触发
        /// </summary>
        public void OnBuffEnd()
        {
            if (LogicType == eBuffLogicType.Static) return;

            if (LogicType == eBuffLogicType.Fixed)
            {
                foreach (var p in mEffectValue)
                {
                    BuffMgr.RefreshAffectValue((eBuffEffectType)p.type);
                }
            }
            if (Data.type == (int)eBuffEffectType.免疫控制)
            {
                Owner.PlayerStatus.DelmmunityType((int)eImmunityType.眩晕);
                Owner.PlayerStatus.DelmmunityType((int)eImmunityType.击飞);
                Owner.PlayerStatus.DelmmunityType((int)eImmunityType.击跪);
            }
        }

        /// <summary>
        /// 发送添加buff
        /// </summary>
        public void SendAddBuff()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Owner.Id).RPC_AddBuff(pkg, BuffId, Level, (int)mRemindTime);
            Owner.HostMap.SendPkg2Clients(null, Owner.GetPosition(), pkg);
        }

        /// <summary>
        /// 发送移除buff
        /// </summary>
        public void SendRemoveBuff()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Owner.Id).RPC_RemoveBuff(pkg, BuffId, Level);
            Owner.HostMap.SendPkg2Clients(null, Owner.GetPosition(), pkg);
        }

        /// <summary>
        /// 发送更新buff
        /// </summary>
        public void SendUpdateBuff()
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Owner.Id).RPC_UpdateBuff(pkg, BuffId, Level, (int)mRemindTime);
            Owner.HostMap.SendPkg2Clients(null, Owner.GetPosition(), pkg);
        }
    }
}
