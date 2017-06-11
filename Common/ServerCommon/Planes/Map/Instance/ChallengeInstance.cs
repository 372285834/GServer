using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 副本
    /// </summary>
    public class ChallengeInstance: SingleInstanceMap
    {
        public override eMapType MapType
        {
            get { return eMapType.Challenge; }
        }

        //挑战副本数据
        public CSTable.CopyTplData mCopyTplData;
        public int mTotalWave = 0;
        public List<int> mLevels = null;//不同波数的不同等级
        public int mWave = 0;
        public int mKilledMobNum; //击杀怪物数量
        public int mTotalMobNum; //怪物总数
        public bool mIsFinished; //是否已结束副本

        public override void OnInit()
        {
            //初始化副本npc
            mCopyTplData = CSTable.StaticDataManager.CopyTpl[Owner.mInstanceId];
            if (mCopyTplData == null)
                return;

            mTotalWave = 1;
            if (!string.IsNullOrEmpty(mCopyTplData.arg1))
            {
                if (mCopyTplData.logic == (int)eInstanceLogic.KillMonster)
                {
                    mLevels = new List<int>();
                    var lvs = mCopyTplData.arg1.Split('|');
                    foreach(var i in lvs)
                    {
                        if (!string.IsNullOrEmpty(i))
                        {
                            mLevels.Add(Convert.ToInt32(i));
                        }
                    }
                    mTotalWave = mLevels.Count;
                }
            }

            InitLogic();
        }

        //副本目标逻辑
        IInstanceObjective mObjective;
        public IInstanceObjective Objective
        {
            get { return mObjective; }
        }

        public bool InitLogic()
        {
            if (null == mCopyTplData) return false;

            BaseGameLogic<IInstanceObjective> bgLogic = (BaseGameLogic<IInstanceObjective>)GameLogicManager.Instance.GetGameLogic(eGameLogicType.InstanceLogic, (short)mCopyTplData.logic);
            if (null == bgLogic) return false;

            mObjective = bgLogic.Logic;

            return mObjective.OnInit(this);
        }

        public void RebornNpc()
        {
            if (null == mCopyTplData) return;

            mWave++;
            Owner.TellToClientCopyCountDown(5.0f);
            TimerManager.doOnce(5.0f, OnInitNpc);
        }

        public void ClearNpc()
        {
            foreach (var i in NpcDictionary.Values)
            {
                i.OnDie();
            }
        }

        public bool IsReady;
        public override void OnClientEnterMapOver(PlayerInstance role)
        {
            IsReady = true;
            RebornNpc();
            CopyCountDown();
        }

        //副本规定时间
        object CopyTimeOverMgr = null;
        public void CopyCountDown()
        {
            if (mCopyTplData == null || mCopyTplData.time <= 0)
                return;

            float second = (float)mCopyTplData.time * 60;
            CopyTimeOverMgr = ServerFrame.TimerManager.doOnce(second, CopyTimeOver);
        }

        public void CopyTimeOver(ServerFrame.TimerEvent evt)
        {
            if (mIsFinished) return;

            OnFinish(false);
        }

        public override eMapAttackRule CanAttack(RoleActor atk, RoleActor def)
        {
            if (atk.Camp == def.Camp)
            {
                return eMapAttackRule.AtkNO;
            }
            else
            {
                return eMapAttackRule.AtkOK;
            }
        }

        
        public void OnInitNpc(TimerEvent evt)
        {
            mTotalMobNum = 0;
            mKilledMobNum = 0;
            //不同的副本不同的初始化逻辑
            eInstanceLogic type = (eInstanceLogic)mCopyTplData.logic;
            switch (type)
            {
                case eInstanceLogic.KillMonster:
                case eInstanceLogic.CollectItem:
                    CreateNpcs();
                    break;
                case eInstanceLogic.ProtectTarget:
                    ulong targetId = CreateProtectNpc();
                    CreateMonsters(targetId);
                    break;
            }
        }

        private NPCInstance CreateNpc(MapInfo_Npc NpcData)
        {
            NpcData.rebornTime = -1;
            NpcData.guardRange = 1000;
            if (NpcData.camp == (int)eCamp.Friend)
            {
                NpcData.camp = (int)Owner.Camp;
            }
            return NPCInstance.CreateNPCInstance(NpcData, this);
        }

        private void CreateNpcs()
        {
            foreach (var NpcData in MapInfo.MapDetail.NpcList)
            {
                try
                {
                    if (mTotalWave > 0 && mWave < mTotalWave)//有波数概念的副本
                    {
                        NpcData.level = mLevels[mWave];
                    }
                    var npc = CreateNpc(NpcData);
                    if (null == npc) continue;

                    mTotalMobNum++;
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Info("CreateNPCInstance Error,id={0}", NpcData.tid);
                }
            }
        }       

        private ulong CreateProtectNpc()
        {
            foreach (var NpcData in MapInfo.MapDetail.NpcList)
            {
                try
                {
                    if (NpcData.tid == mCopyTplData.targetId)
                    {
                        var npc = CreateNpc(NpcData);
                        if (null == npc) continue;

                        return npc.Id;
                    }
                    
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Info("CreateNPCInstance Error,id={0}", NpcData.tid);
                }
            }
            return 0;
        }

        private void CreateMonsters(ulong targetId)
        {
            foreach (var NpcData in MapInfo.MapDetail.NpcList)
            {
                try
                {
                    if (NpcData.tid == mCopyTplData.targetId)
                    {
                        continue;
                    }
                    var npc = CreateNpc(NpcData);
                    if (null == npc) continue;

                    mTotalMobNum++;
                    npc.UpdateEnmityList(targetId, 1);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Server.Info("CreateNPCInstance Error,id={0}", NpcData.tid);
                }
            }
        }

        public void OnFinish(bool isPass)
        {
            if (isPass && mWave < mTotalWave)
            {
                RebornNpc();
            }
            else
            {
                mIsFinished = true;
                if (null != mObjective)
                    mObjective.OnFinish(this);

                Owner.TellToClientCopyEnd(isPass);
            }
        }

    }
}
