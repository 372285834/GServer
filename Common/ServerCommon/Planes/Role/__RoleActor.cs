using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Role
{
    //public class Return_SelectAttackSkill
    //{
    //    UInt16 mSkillId = UInt16.MaxValue;
    //    [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
    //    public UInt16 SkillId
    //    {
    //        get { return mSkillId; }
    //        set { mSkillId = value; }
    //    }
    //    UInt16 mRuneId = UInt16.MaxValue;
    //    [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
    //    public UInt16 RuneId
    //    {
    //        get { return mRuneId; }
    //        set { mRuneId = value; }
    //    }

    //    bool mNeedMove = false;
    //    [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
    //    public bool NeedMove
    //    {
    //        get { return mNeedMove; }
    //        set { mNeedMove = value; }
    //    }
    //    SlimDX.Vector3 mMoveTarget;
    //    [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
    //    public SlimDX.Vector3 MoveTarget
    //    {
    //        get { return mMoveTarget; }
    //        set { mMoveTarget = value; }
    //    }
    //}
    public partial class RoleActor : CSCommon.Component.IActorBase, CSCommon.AISystem.StateHost
    {
        public RoleActor()
        {
            mHatredManager = new Fight.Hatred(this);
            Bag.InventoryType = CSCommon.eItemInventory.ItemBag;
        }

        #region 所属地图管理
        protected MapInstance mHostMap = NullMapInstance.Instance;
        public MapInstance HostMap
        {
            get { return mHostMap; }
        }

        MapCellInstance mHostMapCell;
        public MapCellInstance HostMapCell
        {
            get { return mHostMapCell; }
            set { mHostMapCell = value; }
        }

        public virtual void OnEnterMap(MapInstance map)
        {
            mHostMap = map;
            
            var loc = this.Placement.GetLocation();
            var mapCell_ = mHostMap.GetMapCell(loc.X, loc.Z);
            mapCell_.AddRole(this);

            if (this.CurrentState != null)
            {
                this.CurrentState.OnEnterMap(map.MapName);
            }
        }

        public virtual void OnLeaveMap()
        {
            // 广播给客户端
            HostMap.BroadcastRoleLeave(this);

            var loc = this.Placement.GetLocation();
            var mapCell = mHostMap.GetMapCell(loc.X, loc.Z);
            mapCell.RemoveRole(this);

            mHostMap = NullMapInstance.Instance;

            foreach (var i in mSummons)
            {
                //从地图里清除
                i.OnLeaveMap();
            }
            mSummons.Clear();
        }
        #endregion       

        #region 底层抽象
        public virtual string RoleName
        {
            get { return "无名氏"; }
        }

        int mRoleLevel;
        public virtual int RoleLevel
        {
            get{ return mRoleLevel;}
            set{ mRoleLevel = value;}
        }

        int mRoleStrength =0;
        public virtual int RoleStrength
        {
            get { return mRoleStrength; }
            set { mRoleStrength = value; }
        }

        int mRoleIntellect;
        public virtual int RoleIntellect
        {
            get { return mRoleIntellect; }
            set { mRoleIntellect = value; }
        }

        int mRoleSkillful;
        public virtual int RoleSkillful
        {
            get { return mRoleSkillful; }
            set { mRoleSkillful = value; }
        }

        int mRoleTenacity;
        public virtual int RoleTenacity
        {
            get { return mRoleTenacity; }
            set { mRoleTenacity = value; }
        }

        int mRolePhysical;
        public virtual int RolePhysical
        {
            get { return mRolePhysical; }
            set { mRolePhysical = value; }
        }

        CSCommon.Data.eRoleCreateType mRoleCreateType =CSCommon.Data.eRoleCreateType.Unknown;
        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public CSCommon.Data.eRoleCreateType RoleCreateType
        {
            get { return mRoleCreateType; }
            set { mRoleCreateType = value; }
        }

        //[CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        //[CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        //public virtual CSCommon.Data.eProfession RoleVoation
        //{
        //    get;
        //    set;
        //}
        public virtual SlimDX.Vector3 BirthPosition
        {
            get;
            set;
        }
        float mSpeedRate = 1.0F;
        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public float SpeedRate
        {
            get { return mSpeedRate; }
            set 
            { 
                //mSpeedRate = value; 
                //RPC.PackageWriter pkg = new RPC.PackageWriter();
                //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateRoleSpeedRate(pkg,value);
                //HostMap.SendPkg2Clients(this, this.Placement.GetLocation(), pkg);
            }
        }

        SByte mIsRun = 0;
        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public SByte IsRun
        {
            get { return mIsRun; }
            set { mIsRun = value; }
        }


        public virtual float MoveSpeed
        {
            get
            {
                return this.RoleTemplate.MoveSpeed * mSpeedRate;
            }
        }
        public virtual float WanderLength
        {
            //get { return this.RoleTemplate.WanderRadius; }
            get { return 0; }
        }
        public virtual float LockOnRadius
        {
            //get { return this.RoleTemplate.LockOnRadius; }
            get { return 0; }
        }

        int mPreWanderAI = 30;
        public virtual int PreWanderAI
        {
            get { return mPreWanderAI; }
            set
            {
                mPreWanderAI = value;
            }
        }

        #endregion

        #region 怪物关系
        ServerCommon.Planes.Role.RoleActor mFatherRole =null;
        public virtual ServerCommon.Planes.Role.RoleActor FatherRole
        {
            get { return mFatherRole; }
            set { mFatherRole = value; }
        }
        public List<ServerCommon.Planes.Role.RoleActor > mSonRole =new List<ServerCommon.Planes.Role.RoleActor>();

        string mFallowedRole;
        public virtual string FallowedRole
        {
            get { return mFallowedRole; }
            set { mFallowedRole = value; }
        }

        public virtual Role.RoleActor FindTargetTemplateRole(float length)
        {
            return null;
        }
        #endregion

        #region 召唤生物管理
        public virtual ServerCommon.Planes.Role.RoleActor OwnerRole
        {
            get { return null; }
        }

        List<Role.Summon.SummonRole> mSummons = new List<Role.Summon.SummonRole>();
        public List<Role.Summon.SummonRole> Summons
        {
            get { return mSummons; }
        }
        public void AddSummon(Role.Summon.SummonRole role)
        {
            foreach (var i in mSummons)
            {
                if (i == role)
                    return;
            }
            mSummons.Add(role);
        }
        public void RemoveSummon(Role.Summon.SummonRole role)
        {
            mSummons.Remove(role);
        }
        public Role.Summon.SummonRole FindSummon(UInt32 singleId)
        {
            foreach (var i in mSummons)
            {
                if (i.Id == singleId)
                {
                    return i;
                }
                else
                {
                    var result = i.FindSummon(singleId);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        #endregion

        #region 战斗处理
        static System.Random smRandom = new System.Random(IServer.timeGetTime());
        public static float GetRandomUnit()
        {
            return (float)smRandom.NextDouble();
        }
        public static System.Random Random
        {
            get { return smRandom; }
        }

        public CSCommon.Helper.RoleValue mFinalRoleValue = new CSCommon.Helper.RoleValue();
        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public CSCommon.Helper.RoleValue FinalRoleValue//最终的属性
        {
            get { return mFinalRoleValue; }
        }

        public CSCommon.Helper.RoleValue mBuffRoleValue = new CSCommon.Helper.RoleValue();
        public CSCommon.Helper.RoleValue BuffRoleValue//有buff提供的属性加成
        {
            get { return mBuffRoleValue; }
        }

        public CSCommon.Helper.RoleValue mDevelopValue = new CSCommon.Helper.RoleValue();
        public CSCommon.Helper.RoleValue DevelopValue //由成长,特殊npc加成所得的属性加成
        {
            get { return mDevelopValue; }
        }
        public CSCommon.Helper.RoleValue mNonDevelopValue = new CSCommon.Helper.RoleValue();
        public CSCommon.Helper.RoleValue NonDevelopValue  //由装备提供的属性加成
        {
            get { return mNonDevelopValue; }
        }

        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public virtual int RoleHP
        {
            get;
            set;
        }

        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public virtual int RoleMP
        {
            get;
            set;
        }

        int mRoleDex =1;
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public virtual int RoleDex
        {
            get
            {
                return mRoleDex;
            }
            set 
            {
                mRoleDex = value;
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateRoleDex(pkg, value);

            }
        }

        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public virtual UInt16 FactionId
        {
            get;//玩家为0号阵营
            set;
        }

        //角色是否会进仇恨列表
        public virtual bool HasHatred
        {
            get { return true; }
        }

        public virtual bool StopMoveOnBlock
        {
            get { return false; }
        }

        public virtual void GainExp(int exp)
        {

        }

        public virtual void ChangeMoney(CSCommon.eCurrenceType ctype, CSCommon.Data.eMoneyChangeType mtype, int value)
        {

        }

        public virtual void GainExistentialPower(int power)
        {

        }

        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public virtual void FreshRoleValue()
        {//当换装，升级，装备强化等导致角色属性变化的时候调用
            //计算角色属性，装备属性，buff等的最终属性
            //mFinalRoleValue.ClearValue();
            mFinalRoleValue = new CSCommon.Helper.RoleValue();
            mDevelopValue = new CSCommon.Helper.RoleValue();
            mNonDevelopValue = new CSCommon.Helper.RoleValue();
          
            for (CSCommon.Data.eBagInfo i = CSCommon.Data.eBagInfo.Helmet; i < CSCommon.Data.eBagInfo.EquipmentCount; i++)
            {
                var item = Equip.GetEquipment(i);
                if (item == null)
                    continue;

        //        mNonDevelopValue.AddValue(item.GetItemRoleValue(ref this));
            }
            
            mFinalRoleValue.AddValue(DevelopValue);
            mFinalRoleValue.AddValue(NonDevelopValue);
            mFinalRoleValue.AddValue(BuffRoleValue);

            RoleHP = mFinalRoleValue.RoleMaxHp;
            RoleMP = mFinalRoleValue.RoleMaxMp;
        }

        public virtual void FreshEquipBaseValue()
        {
            for (CSCommon.Data.eBagInfo i = CSCommon.Data.eBagInfo.Helmet; i < CSCommon.Data.eBagInfo.EquipmentCount; i++)
            {
                var item = Equip.GetEquipment(i);
                if (item == null)
                    continue;
                AffixBaseValueAdd(item);
            }
        }

        public virtual void FreshEquipFinalValue()
        {
            for (CSCommon.Data.eBagInfo i = CSCommon.Data.eBagInfo.Helmet; i < CSCommon.Data.eBagInfo.EquipmentCount; i++)
            {
                var item = Equip.GetEquipment(i);
                if (item == null)
                    continue;
                NonDevelopValue.AddValue(GetItemRoleValue(item));
            }
        }

        Fight.Hatred mHatredManager = null;
        public Fight.Hatred HatredManager
        {
            get { return mHatredManager; }
        }

        protected bool mIsDeath = false;
        public bool IsDeath()
        {
            return mIsDeath;
        }

        public virtual void ProcDeath()
        {
            mIsDeath = true;
            int index = 0;
            try
            {
                var curNode = mHatredManager.Hatreds.First;
                while (curNode!=null)
                {
                    var i = curNode.Value;
                    if (curNode.Value != null)
                    {
                        if (i.Attacker == this)
                            continue;
                        i.Attacker.TargetDeath(this, index);
                        index++;
                    }
                    curNode = curNode.Next;
                }
                //foreach (var i in mHatredManager.Hatreds)
                //{
                //    if (i.Attacker == this)
                //        continue;
                //    i.Attacker.TargetDeath(this, index);
                //    index++;
                //}
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.StackTrace.ToString());
            }
            
            mHatredManager.ClearHatred();
        }

        public virtual void TargetDeath(RoleActor target,int index)
        {
            mHatredManager.RemoveHatred(target);

            if (target.CurrentState == null)
                return;
            
            target.CurrentState.OnTargetDeath(target,index);
        }
        
        //发现客户端作弊的时候调用此函数
        public virtual void OnClientCheat(int level,string reason)
        {
            
        }

        public virtual bool IsValidMoveSpeed(float speed)
        {
            if (speed != this.MoveSpeed)
                return false;
            return true;
        }

        public virtual bool CanLockon(RoleActor target)
        {//可以主动选择作为攻击目标
            if (target == null)
                return false;
            if (target == this)
                return false;
            if (target.IsDeath())
                return false;
            if (target == this.OwnerRole)//你不能杀死发射自己的人角色
                return false;
            if (FatherRole !=null && target == this.FatherRole)
                return false;
            if (mSonRole != null)
            {
                foreach (var role in mSonRole)
                {
                    if (target.Id == role.Id)
                        return false;
                }
            }
            //if (target.GetType() == typeof(DropedItem.DropedItemRole))
            //    return false;
            if (OwnerRole != null && target.OwnerRole != null)
            {
                if ( target.OwnerRole.Id == OwnerRole.Id)
                    return false;                
            }
            var faction = CSCommon.Helper.FactionManager.Instance.FindFaction(this.FactionId);
            if (faction != null)
            {//天生仇恨的人
                if (faction.IsEnemy(target.FactionId))
                    return true;
            }
            faction = CSCommon.Helper.FactionManager.Instance.FindFaction(target.FactionId);
            if (faction != null)
            {//别人仇恨我，我就主动锁定别人
                if (faction.IsEnemy(this.FactionId))
                    return true;
            }
            return true;
        }

        public virtual bool CanAttack(RoleActor target)
        {
            if (target == null)
                return false;
            if (target == this)
                return false;
            if (target.IsDeath())
                return false;
            if (target == this.OwnerRole)//你不能杀死发射自己的人角色
                return false;
            if (FatherRole !=null && target == this.FatherRole)
                return false;
            if (mSonRole!=null)
            {
                foreach (var role in mSonRole)
                {
                    if (target.Id == role.Id)
                        return false;
                }
            }

            //if (target.GetType()==typeof(DropedItem.DropedItemRole))
            //    return false;
            if (target.OwnerRole != null && target.OwnerRole.Id == OwnerRole.Id)
                return false;
            var faction = CSCommon.Helper.FactionManager.Instance.FindFaction(this.FactionId);
            if (faction != null)
            {
                if (faction.IsFriend(target.FactionId))
                    return false;
            }
            return true;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public virtual SlimDX.Vector3 GetWanderTarget()
        {
            return Placement.GetLocation();
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public virtual Role.RoleActor SelectAttackTarget()
        {//选择一个当前状态下最应该攻击的目标
            var target = AttackTarget;

            if (target != null)
            {
//                 RPC.PackageWriter pkg = new RPC.PackageWriter();
//                 ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateRoleAttacker(pkg, target.Id);
//                 HostMap.SendPkg2Clients(this, this.Placement.GetLocation(), pkg);
                return target;
            }
            return null;
        }

        //[CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        //public virtual Return_SelectAttackSkill SelectAttackSkill(Role.RoleActor target,ushort skillid =0,ushort runeid =0)
        //{
        //    var result = new Return_SelectAttackSkill();
        //    if (target == null)
        //        return result;

        //    if (skillid != 0 && runeid !=0)
        //    {
        //        result.SkillId = skillid;
        //        result.RuneId = runeid;
        //    }
        //    else
        //    {
        //        // 从可用技能列表中选一个，如果没有则用3号普通攻击
        //        if (RoleTemplate.SkillInfos.Count > 0)
        //        {
        //            result.SkillId = RoleTemplate.SkillInfos[0].SkillId;
        //            result.RuneId = RoleTemplate.SkillInfos[0].RuneId;
        //        }
        //        else
        //        {
        //            result.SkillId = 11100;
        //            result.RuneId = 11300;
        //        }
        //    }
        //    var skillData = FindSkillData(result.SkillId);
        //    if (skillData == null)
        //        return result;
        //    var runeData = FindRuneData(result.RuneId);
        //    if (runeData == null)
        //        return result;

        //    //如果射程都不够返回needMove，让AI去走过去
        //    float dist = SlimDX.Vector3.Distance(target.Placement.GetLocation(),Placement.GetLocation());
        //    var attackRadius = runeData.Template.AttackRadius + target.RoleTemplate.Radius;
        //    if (dist > attackRadius)
        //    {
        //        //需要移动，MoveTarge是一个位置
        //        result.NeedMove = true;
        //        var dir = Placement.GetLocation() - target.Placement.GetLocation();
        //        dir.Normalize();
        //        result.MoveTarget = target.Placement.GetLocation() + dir * (runeData.Template.AttackRadius + target.RoleTemplate.Radius - 0.2F) * 0.8F;
        //    }
        //    else
        //    {
        //        //不需要移动，MoveTarget是一个发射方向
        //        var dir = target.Placement.GetLocation() - Placement.GetLocation();
        //        dir.Normalize();
        //        result.MoveTarget = dir;
        //    }

        //    return result;
        //}

        public virtual Role.RoleActor AttackTarget
        {
            get
            {
                return mHatredManager.GetFirstTarget();
            }
        }

        bool bHasPlayer = false;
        bool OnSearchClosePlayer(Role.RoleActor role, object arg)
        {
            var player = role as PlayerInstance;
            if (player == null)
                return true;

            bHasPlayer = true;
            return false;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoWanderAIBehavior()
        {
            //var center = Placement.GetLocation();
            //UInt32 actorTypes = (1 << (Int32)CSCommon.Component.EActorGameType.Player);
            //bHasPlayer = false;
            //mHostMap.TourRoles(ref center, 50, actorTypes, this.OnSearchClosePlayer, null);
            //if (false == bHasPlayer)
            //{
            //    Placement.SetLocation(ref center);
            //    return;
            //}
            //var ran=smRandom.Next(0, 100);
            //if (ran >= PreWanderAI)
            //    return;

            //var walkState = this.AIStates.GetState("Walk");
            //if (walkState != null)
            //{
            //    var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            //    walkParam.TargetPosition = GetWanderTarget();
            //    walkParam.MaxCloseDistance = 0.05f;
            //    var start = Placement.GetLocation();

            //    var pkg = new RPC.PackageWriter();
            //    H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //    pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //    {
            //        if (bTimeOut)
            //        {

            //        }
            //        else
            //        {
            //            //Byte pathFindResult;
            //            //_io.Read(out pathFindResult);
            //            //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //            //{
            //            //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //            //        {
            //            //            int count = 0;
            //            //            _io.Read(out count);
            //            //            for (int i = 0; i < count; i++)
            //            //            {
            //            //                SlimDX.Vector2 pathPt;
            //            //                _io.Read(out pathPt);

            //            //                var pt = new SlimDX.Vector3();
            //            //                pt.X = pathPt.X;
            //            //                pt.Z = pathPt.Y;
            //            //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //            //                walkParam.TargetPositions.Enqueue(pt);
            //            //            }

            //            //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //            //            walkParam.Run = 0;
            //            //            this.IsRun = 0;
            //            //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //            //            this.CurrentState.ToState("Walk", walkParam);
            //            //        }
            //            //        break;
            //            //    default:
            //            //        {
            //            //            this.CurrentState.ToState("Idle", null);
            //            //        }
            //            //        break;
            //            //}
            //        }
            //    };
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoFallowAIBehavior()
        {
            //if (FatherRole == null && !string.IsNullOrEmpty(FallowedRole))
            //{
            //    var role = FindTargetTemplateRole(30);
            //    if (role != null)
            //    {
            //        FatherRole = role;
            //        if (this as ServerCommon.Planes.Role.RoleActor != null)
            //        {
            //            if (role.mSonRole != null)
            //            {
            //                role.mSonRole.Add(this);
            //            }
            //        }
            //    }
            //}
            //if (FatherRole ==null)
            //{
            //    return;
            //}
            //if (FatherRole.Id == this.Id)
            //{
            //    FatherRole = null;
            //    return;
            //}
            //var dist =SlimDX.Vector3.Distance(FatherRole.Placement.GetLocation(),Placement.GetLocation());
            //if (dist <= 3)
            //    return;         
            //var center = Placement.GetLocation();
            //UInt32 actorTypes = (1 << (Int32)CSCommon.Component.EActorGameType.Player);
            //bHasPlayer = false;
            //mHostMap.TourRoles(ref center, 50, actorTypes, this.OnSearchClosePlayer, null);
            //if (false == bHasPlayer)
            //{
            //    Placement.SetLocation(ref center);
            //    return;
            //}
            //if (FatherRole.CurrentState.StateName == "Idle")
            //{
            //    var ran = smRandom.Next(0, 100);
            //    if (ran >= PreWanderAI)
            //        return;
            //}
            //var walkState = this.AIStates.GetState("Walk");
            //if (walkState != null)
            //{
            //    var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;

            //    var runto = Placement.GetLocation() - FatherRole.Placement.GetLocation();
            //    runto.Normalize();
            //    runto.Y = 0;
            //    int zf =(int)Id % 2;
            //    if (zf !=1)
            //    {
            //        zf = -1;
            //    }        
            //    SlimDX.Quaternion.RotationAxis(runto,zf*3.14f / 6);
            //    SlimDX.Vector3 pos = FatherRole.Placement.GetLocation() + runto * 3;
            //    pos.Y = HostMap.GetAltitude(pos.X, pos.Z);
            //    walkParam.TargetPosition = pos;
            //    walkParam.MoveSpeed = MoveSpeed * 1.5f;
            //    walkParam.MaxCloseDistance = 0.05f;
            //    var start = Placement.GetLocation();

            //    var pkg = new RPC.PackageWriter();
            //    H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //    pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //    {
            //        if (bTimeOut)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            //Byte pathFindResult;
            //            //_io.Read(out pathFindResult);
            //            //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //            //{
            //            //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //            //        {
            //            //            int count = 0;
            //            //            _io.Read(out count);
            //            //            for (int i = 0; i < count; i++)
            //            //            {
            //            //                SlimDX.Vector2 pathPt;
            //            //                _io.Read(out pathPt);

            //            //                var pt = new SlimDX.Vector3();
            //            //                pt.X = pathPt.X;
            //            //                pt.Z = pathPt.Y;
            //            //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //            //                walkParam.TargetPositions.Enqueue(pt);
            //            //            }

            //            //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //            //            walkParam.Run = 0;
            //            //            this.IsRun = 0;
            //            //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //            //            this.CurrentState.ToState("Walk", walkParam);
            //            //        }
            //            //        break;
            //            //    default:
            //            //        {
            //            //            this.CurrentState.ToState("Idle", null);
            //            //        }
            //            //        break;
            //            //}
            //        }
            //    };
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoAttackCurTargetAIBehavior()
        {
            //if (CurrentState.StateName != "Idle")
            //    return;
            //var target = this.AttackTarget;
            //if (target != null)
            //{
            //    var selector = SelectAttackSkill(target);
            //    if (selector.NeedMove)
            //    {
            //        var walkState = this.AIStates.GetState("Walk");
            //        if (walkState != null)
            //        {
            //            var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            //            walkParam.TargetPosition = selector.MoveTarget;
            //            walkParam.MaxCloseDistance = 0.05f;
            //            var start = Placement.GetLocation();

            //            var pkg = new RPC.PackageWriter();
            //            H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //            pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //            {
            //                if (bTimeOut)
            //                {
            //                    return;
            //                }
            //                //Byte pathFindResult;
            //                //_io.Read(out pathFindResult);
            //                //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //                //{
            //                //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //                //        {
            //                //            int count = 0;
            //                //            _io.Read(out count);
            //                //            for (int i = 0; i < count; i++)
            //                //            {
            //                //                SlimDX.Vector2 pathPt;
            //                //                _io.Read(out pathPt);

            //                //                var pt = new SlimDX.Vector3();
            //                //                pt.X = pathPt.X;
            //                //                pt.Z = pathPt.Y;
            //                //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //                //                walkParam.TargetPositions.Enqueue(pt);
            //                //            }

            //                //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //                //            walkParam.Run = 1;
            //                //            this.IsRun = 1;
            //                //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //                //            this.CurrentState.ToState("Walk", walkParam);

            //                //            var ecb = new CSCommon.Helper.EventCallBack(CSCommon.Helper.enCSType.Server)
            //                //            {
            //                //                CBType = typeof(CSCommon.AISystem.FOnStateExit),
            //                //                Id = Guid.NewGuid(),
            //                //                Callee = (CSCommon.AISystem.FOnStateExit)this.OnStateExit_AttackAfterWalk
            //                //            };

            //                //            this.PushStateExit(ecb);
            //                //        }
            //                //        break;
            //                //    default:
            //                //        {
            //                //            this.CurrentState.ToState("Idle", null);
            //                //        }
            //                //        break;
            //                //}
            //            };
            //        }
            //    }
            //    else
            //    {
            //        Placement.SetRotationY(selector.MoveTarget.Z, selector.MoveTarget.X, RoleTemplate.MeshFixAngle);
            //        //NPCData.Direction = Placement.GetRotationY();

            //        var pkg = new RPC.PackageWriter();
            //        //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateDirection(pkg, this.Placement.GetRotationY());
            //        HostMap.SendPkg2Clients(this, this.Placement.GetLocation(), pkg);

            //        this.CurrentState.DoFireSkill(selector.SkillId, selector.RuneId,selector.MoveTarget, SlimDX.Vector3.Zero);
            //    }
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoAttackFireSkill(ushort skillid, ushort runeid)
        {
            //var target = SelectAttackTarget();
            //if (target != null)
            //{
            //    var selector = SelectAttackSkill(target, skillid, runeid);
            //    if (selector.NeedMove)
            //    {
            //        var walkState = this.AIStates.GetState("Walk");
            //        if (walkState != null)
            //        {
            //            var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            //            walkParam.TargetPosition = selector.MoveTarget;
            //            walkParam.MaxCloseDistance = 0.05f;
            //            var start = Placement.GetLocation();

            //            var pkg = new RPC.PackageWriter();
            //            H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //            pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //            {
            //                if (bTimeOut)
            //                    return;

            //                //Byte pathFindResult;
            //                //_io.Read(out pathFindResult);
            //                //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //                //{
            //                //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //                //        {
            //                //            int count = 0;
            //                //            _io.Read(out count);
            //                //            for (int i = 0; i < count; i++)
            //                //            {
            //                //                SlimDX.Vector2 pathPt;
            //                //                _io.Read(out pathPt);

            //                //                var pt = new SlimDX.Vector3();
            //                //                pt.X = pathPt.X;
            //                //                pt.Z = pathPt.Y;
            //                //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //                //                walkParam.TargetPositions.Enqueue(pt);
            //                //            }

            //                //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //                //            walkParam.Run = 1;
            //                //            this.IsRun = 1;
            //                //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //                //            this.CurrentState.ToState("Walk", walkParam);

            //                //            var ecb = new CSCommon.Helper.EventCallBack(CSCommon.Helper.enCSType.Server)
            //                //            {
            //                //                CBType = typeof(CSCommon.AISystem.FOnStateExit),
            //                //                Id = Guid.NewGuid(),
            //                //                Callee = (CSCommon.AISystem.FOnStateExit)this.OnStateExit_AttackAfterWalk
            //                //            };
            //                //        }
            //                //        break;
            //                //    default:
            //                //        {
            //                //            this.CurrentState.ToState("Idle", null);
            //                //        }
            //                //        break;
            //                //}
            //            };
            //        }
            //    }
            //    else
            //    {
            //        Placement.SetRotationY(selector.MoveTarget.Z, selector.MoveTarget.X, RoleTemplate.MeshFixAngle);
            //        //NPCData.Direction = Placement.GetRotationY();

            //        var pkg = new RPC.PackageWriter();
            //        //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateDirection(pkg, this.Placement.GetRotationY());
            //        HostMap.SendPkg2Clients(this, this.Placement.GetLocation(), pkg);

            //        this.CurrentState.DoFireSkill(selector.SkillId, selector.RuneId, selector.MoveTarget, SlimDX.Vector3.Zero);
            //    }
            //}
        }

        private bool mIfDoAttackAI = false;
        [CSCommon.AISystem.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        public bool IfDaAttackAI
        {
            get{ return mIfDoAttackAI; }
            set { mIfDoAttackAI = value; }
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoAttackTargetAIBehavior()
        {
            var target = SelectAttackTarget();
            if (target == null)
            {
                if (CurrentState.StateName != "Walk")
                {
                    DoWanderAIBehavior();
                }
            }
            else
            {
                DoAttackCurTargetAIBehavior();
            }
        }

        private void OnStateExit_AttackAfterWalk(CSCommon.AISystem.State preState, CSCommon.AISystem.State curState)
        {
            //if (curState.StateName == "Idle" || curState.StateName == "Walk")
            {
                DoAttackCurTargetAIBehavior();
            }
        }

        public void ProcHurt(Role.RoleActor attacker, CSCommon.Data.Skill.SkillData skillData, CSCommon.Data.Skill.RuneData runeData, byte effectIndex)
        {
            //if (runeData == null)
            //    return;
            //if (CurrentState.StateName == "Death")
            //    return;
            //if (RoleHP <=0)
            //{

            //}

            //RPC.DataWriter dwParam = new RPC.DataWriter();
            //dwParam.Write(runeData.Template.RuneId);
            //RPC.PackageWriter pkg = new RPC.PackageWriter();
            ////ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_ProcHurt(pkg, dwParam);
            //HostMap.SendPkg2Clients(null, this.Placement.GetLocation(), pkg);

            //int damage = attacker.GetAttackResult(this, skillData, runeData, effectIndex);
            //RoleHP -= damage;
            //var hatredValue = Fight.Hatred.GetHatredValueByDamage(damage, attacker, this,skillData);
            //mHatredManager.AddHatred(attacker, hatredValue);
            //if (FatherRole !=null)
            //{
            //    FatherRole.mHatredManager.AddHatred(attacker, hatredValue);//告诉老爹被打了
            //    if (FatherRole.mSonRole != null)
            //    {
            //        foreach (var tar in FatherRole.mSonRole)
            //        {
            //            tar.mHatredManager.AddHatred(attacker, hatredValue);//告诉兄弟被打了
            //        }
            //    }
            //}
            //if (mSonRole != null)
            //{
            //    foreach (var tar in mSonRole)
            //    {
            //        tar.mHatredManager.AddHatred(attacker, hatredValue);//告诉儿子被打了
            //    }
            //}

            //if (attacker != null)
            //{
            //    attacker.HatredManager.AddHatred(this, 1);//攻击者对被攻击着增加1点点仇恨，这样召唤物就可以找到当前攻击目标了
            //    if (attacker.OwnerRole != null)
            //    {
            //        attacker.OwnerRole.HatredManager.AddHatred(this, 1);

            //        var play = this as PlayerInstance;
            //        if (play != null)
            //        {
            //            if (play.PlayerData.RoleDetail.RoleState != (byte)CSCommon.Data.RState.RedName ||
            //                play.PlayerData.RoleDetail.RoleState != (byte)CSCommon.Data.RState.PinkName)
            //            {
            //                RoleChangePinkName(attacker.OwnerRole);       //攻击者变粉名
            //            }
            //        }
            //    }
            //    else
            //    {
            //        var play = this as PlayerInstance;
            //        if (play != null)
            //        {
            //            if (play.PlayerData.RoleDetail.RoleState != (byte)CSCommon.Data.RState.RedName ||
            //                play.PlayerData.RoleDetail.RoleState != (byte)CSCommon.Data.RState.PinkName)
            //            {
            //                RoleChangePinkName(attacker);       //攻击者变粉名
            //            }
            //        }
            //    }
            //}
            
            //if (CurrentState != null)
            //{
            //    CurrentState.OnBeHurt(attacker);
            //}

            //// TODO: 向客户端通知伤害数字、特殊状态等
            ////if (damage > 0)
            ////{
            ////    //CurrentState.ToState("BeAttack", null);
            ////    CSCommon.AISystem.States.IBeAttackParameter param = new CSCommon.AISystem.States.IBeAttackParameter();
            ////    param.AttackerPos = attacker.Placement.GetLocation();
            ////    CSCommon.AISystem.State.TargetToState(this, "BeAttack", param);
            ////}

            //// TODO： 创建自身的受伤害技能产生的BUFF 和 攻击者命中后产生的BUFF
            //foreach( var buffParam in runeData.Template.RuneLevelParams[runeData.RuneLevel].BuffParams )
            //{
            //    var buff = CSCommon.Data.Skill.BuffTemplateManager.Instance.FindBuff((ushort)buffParam.BuffId);
            //    if (buff == null)
            //        continue;

            //    if (buff.BuffState == CSCommon.Data.Skill.BuffState.Other || buff.BuffState == CSCommon.Data.Skill.BuffState.OtherAll)
            //        this.BuffBag.CreateBuffAndAutoAdd2Bag(this, buffParam);
            //}

            //if (RoleHP < 1)
            //{
            //    RoleHP = 0;

            //    if (runeData.Template.SummonOffsetType == CSCommon.Data.Skill.EOffsetType.None)
            //        CSCommon.AISystem.State.TargetToState(this, "Death", null);
            //}
        }

        public void RoleChangePinkName(Role.RoleActor attacker)
        {
            //var play = attacker as PlayerInstance;
            //if (play == null)
            //    return;

            //if (play.PlayerData.RoleDetail.RoleState == (Byte)CSCommon.Data.RState.RedName)
            //    return;

            //if (play.PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.PinkName)
            //{
            //    play.PlayerData.RoleDetail.PinkNameTime = System.DateTime.Now;
            //    return;
            //}

            //play.PlayerData.RoleDetail.RoleState = (byte)CSCommon.Data.RState.PinkName;
            //play.PlayerData.RoleDetail.PinkNameTime = System.DateTime.Now;
        }

        public void RoleChangeBlueName(Role.RoleActor attacker)
        {
            //var play = attacker as PlayerInstance;
            //if (play == null)
            //    return;

            //if (play.PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.BlueName)
            //{
            //    play.PlayerData.RoleDetail.PinkNameTime = System.DateTime.Now;
            //    return;
            //}

            //play.PlayerData.RoleDetail.RoleState = (byte)CSCommon.Data.RState.BlueName;
            //play.PlayerData.RoleDetail.PinkNameTime = System.DateTime.Now;
        }

        public void RoleChangeRedName(Role.RoleActor attacker)
        {
            //var play = attacker as PlayerInstance;
            //if (play == null)
            //    return;

            //if (play.PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.RedName)
            //    return;

            //if (play.PlayerData.RoleDetail.EvilValue >= CSCommon.Data.Fight.PvPTemplateManager.Instance.PvPCommonProperty.NeedEvilRedName)
            //{
            //    play.PlayerData.RoleDetail.RoleState = (byte)CSCommon.Data.RState.RedName;
            //}
        }

        public void RoleChangeWhiteName(Role.RoleActor attacker)
        {
            //var play = attacker as PlayerInstance;
            //if (play == null)
            //    return;

            //if (play.PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.WhiteName)
            //    return;

            //if (play.PlayerData.RoleDetail.EvilValue < CSCommon.Data.Fight.PvPTemplateManager.Instance.PvPCommonProperty.NeedEvilRedName)
            //{
            //    play.PlayerData.RoleDetail.RoleState = (byte)CSCommon.Data.RState.WhiteName;
            //}
        }

        //public void AddEvil(Role.RoleActor attacker)
        //{
        //    var play = attacker as PlayerInstance;
        //    if (play == null)
        //        return;

        //    foreach (var data in CSCommon.Data.Fight.PvPTemplateManager.Instance.PvPCommonProperty.AddEvilValueList)
        //    {
        //        if (play.PlayerData.RoleDetail.EvilValue < data.MaxEvilValue)
        //        {
        //            play.PlayerData.RoleDetail.EvilValue += data.AddEvilValue;
        //            if (play.PlayerData.RoleDetail.EvilValue >= UInt32.MaxValue)
        //                play.PlayerData.RoleDetail.EvilValue = UInt32.MaxValue;

        //            RoleChangeRedName(attacker);
        //            break;
        //        }
        //    }
        //}

        //public bool SubEvil(Role.RoleActor attacker, Int64 seconds)
        //{
        //    var play = attacker as PlayerInstance;
        //    if (play == null)
        //        return false;

        //    foreach (var data in CSCommon.Data.Fight.PvPTemplateManager.Instance.PvPCommonProperty.SubEvilValueList)
        //    {
        //        if (data.OnHookTime < (UInt64)seconds)
        //        {
        //            if (play.PlayerData.RoleDetail.EvilValue < data.MaxEvilValue)
        //            {
        //                play.PlayerData.RoleDetail.EvilValue -= data.SubEvilValueOne;
        //                if (play.PlayerData.RoleDetail.EvilValue <= 0)
        //                    play.PlayerData.RoleDetail.EvilValue = 0;

        //                RoleChangeWhiteName(attacker);
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public bool SubEvil(Role.RoleActor attacker, UInt32 monsters)
        //{
        //    var play = attacker as PlayerInstance;
        //    if (play == null)
        //        return false;

        //    foreach (var data in CSCommon.Data.Fight.PvPTemplateManager.Instance.PvPCommonProperty.SubEvilValueList)
        //    {
        //        if (data.MonsterValue < monsters)
        //        {
        //            if (play.PlayerData.RoleDetail.EvilValue < data.MaxEvilValue)
        //            {
        //                play.PlayerData.RoleDetail.EvilValue -= data.SubEvilValueTwo;
        //                if (play.PlayerData.RoleDetail.EvilValue <= 0)
        //                    play.PlayerData.RoleDetail.EvilValue = 0;

        //                RoleChangeWhiteName(attacker);
        //                return true ;
        //            }
        //        }
        //    }
        //    return false;
        //}

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void DoBeAttack()
        {
            CurrentState.ToState("BeAttack", null);
        }

        public virtual void OnFireSkill(UInt16 skillId, UInt16 runeId, SlimDX.Vector3 dir, SlimDX.Vector3 summonPos)
        {
            //var skillData = FindSkillData(skillId);
            //if (skillData == null)
            //    return;
            //if (skillData.SkillLevel == 0)
            //    return;

            //var runeData = FindRuneData(runeId);
            //if (runeData == null)
            //    return;
            //if (runeData.RuneLevel == 0)
            //    return;
            
            //if (skillData.Template == null)
            //    return;

            //if (runeData.Template == null)
            //    return;

            ////if (HostMap != null)
            ////{
            ////    var loc = Placement.GetLocation();
            ////    HostMap.RolePositionChanged(this, ref loc);
            ////}

            //if (runeData.Template.ThrowRoleCount > 0)
            //{
            //    for (UInt16 j = 0; j < runeData.Template.ThrowRoleCount; j++)
            //    {
            //        var fireDir = dir;
            //        switch(runeData.Template.EmissionType)
            //        {
            //            case CSCommon.Data.Skill.EEmissionType.Bunch:
            //                break;
            //            case CSCommon.Data.Skill.EEmissionType.Distribute:
            //                {
            //                    if(runeData.Template.ThrowRoleCount>1)
            //                    {                                    
            //                        double emissionAngle = (double)(runeData.Template.EmissionAngle) / 180.0 * System.Math.PI;
            //                        double partAngle = emissionAngle / (runeData.Template.ThrowRoleCount-1);
            //                        double startAngle = -emissionAngle/2.0;
            //                        float angle = (float)(startAngle + partAngle*j);
            //                        var unitY = SlimDX.Vector3.UnitY;
            //                        SlimDX.Quaternion quat = new SlimDX.Quaternion();
            //                        SlimDX.Quaternion.RotationAxis(ref unitY, angle, out quat);
            //                        fireDir = SlimDX.Vector3.TransformCoordinate(fireDir, quat);
            //                    }
            //                }
            //                break;
            //        }

            //        CSCommon.Data.SummonData smData = new CSCommon.Data.SummonData();
            //        smData.Init(skillData, runeData);
            //        SlimDX.Vector3 loc;
            //        if(runeData.Template.RuneType == CSCommon.Data.Skill.ERuneType.Skillshot)     // 如果是指向技能，则使用玩家指定位置
            //        {
            //            loc = summonPos;
            //            // TODO： 安全判断， 看玩家传上来的位置是否合法
            //        }
            //        else                                                           // 如果是非指向技能，则计算目的地
            //        {
            //            loc = Placement.GetLocation() + fireDir * runeData.Template.ThrowOffset;
            //            loc.Y += runeData.Template.HeightOffset;
            //        }
            //        if (this.OwnerRole != null)
            //        {
            //            ServerCommon.Planes.Role.Summon.SummonRole.CreateSummonInstance(this.OwnerRole, smData, HostMap, ref loc, skillData, runeData, ref fireDir);
            //        }
            //        else
            //        {
            //            ServerCommon.Planes.Role.Summon.SummonRole.CreateSummonInstance(this, smData, HostMap, ref loc, skillData, runeData, ref fireDir);
            //        }

            //        Placement.SetRotationY(dir.Z, dir.X, RoleTemplate.MeshFixAngle);
            //        smData.Direction = Placement.GetRotationY();
            //        RPC.PackageWriter pkg = new RPC.PackageWriter();
            //        //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, Id).RPC_UpdateDirection(pkg, smData.Direction);
            //        HostMap.SendPkg2Clients(this, Placement.GetLocation(), pkg);
            //    }
            //}

        }

        class BeHurtArgument
        {
            public CSCommon.Animation.NotifyPoint NotifyPt;
            public CSCommon.Data.Skill.SkillData skillData;
            public CSCommon.Data.Skill.RuneData runeData;
            public int hurtNumber;
            public List<ulong> roleIds = new List<ulong>();
        }
        byte mHitCount;
        protected bool OnVisitRole_ProcBeHurt(Role.RoleActor role, object arg)
        {
            if (role == this)
                return true;

            var hurArg = arg as BeHurtArgument;
            hurArg.roleIds.Add(role.Id);
            hurArg.hurtNumber++;

            if (hurArg.NotifyPt!=null)
            {//这里今后要根据notifyPt的形状位置来判断
                mHitCount++;
                role.ProcHurt(this, hurArg.skillData, hurArg.runeData, mHitCount);
            }

            // 先在HURT中做BEATTACK的处理，
            // TODO：加入各种判断条件：如技能是否产生硬直、是否钢体
            var state = role.AIStates.GetState("BeAttack");
            if (state != null)
            {
                CSCommon.AISystem.States.IBeAttackParameter param = new CSCommon.AISystem.States.IBeAttackParameter();
                param.AttackerPos = this.Placement.GetLocation();
                if (CurrentState.StateName == "StayAttack")
                {
                    var stayAttackParam = CurrentState.Parameter as CSCommon.AISystem.States.IStayAttackParameter;
                    param.SkillId = stayAttackParam.SkillId;
                }
                CSCommon.AISystem.State.TargetToState(role, "BeAttack", param);
            }

            // 超过一次攻击个数上限或总个数超过了上限，则停止遍历
            if (hurArg.roleIds.Count >= hurArg.runeData.Template.RuneTargetNumber  || hurArg.hurtNumber >= hurArg.runeData.Template.MaxDamageTargetNumber)
                return false;

            return true;
        }
        public void DoTargetBeHurt(CSCommon.Animation.NotifyPoint ntp, CSCommon.Data.Skill.SkillData skill, CSCommon.Data.Skill.RuneData rune, int runeHandle)
        {
            //if (rune.Template.RuneType == CSCommon.Data.Skill.ERuneType.OneChain || rune.Template.RuneType == CSCommon.Data.Skill.ERuneType.MultiChain)
            //{
            //    if (rune.Template.ChainNotifyPointName != "")
            //    {
            //        if (ntp.NotifyName != rune.Template.ChainNotifyPointName)
            //            return;
            //    }
            //}
            //else
            //{
            //    if (ntp.NotifyName == "Chain01")
            //        return;
            //}

            //UInt32 actorTypes = (1 << (Int32)CSCommon.Component.EActorGameType.Player) | (1 << (Int32)CSCommon.Component.EActorGameType.Npc);
            //var arg = new BeHurtArgument();
            //arg.NotifyPt = ntp;
            //arg.skillData = skill;
            //arg.runeData = rune;
            //arg.hurtNumber = 0;

            //var absBoxMatrixList = new List<SlimDX.Matrix>();
            //SlimDX.Matrix absMatrix = new SlimDX.Matrix();
            //Placement.GetAbsMatrix(out absMatrix);  //取得变换矩阵
            //// 如果是AttackNotifyPoint，则用Box来做检测
            //var atkPoint = ntp as CSCommon.Animation.AttackNotifyPoint;
            //if(atkPoint!=null && atkPoint.BoxList.Count>0)
            //{
            //    SlimDX.BoundingBox absBox = new SlimDX.BoundingBox();           // 判定范围
            //    for (int iB = 0; iB < atkPoint.BoxList.Count; ++iB )
            //    {
            //        var boxAbsM = atkPoint.BoxList[iB] * absMatrix;//矩阵变换，动作做角色矩阵的变换

            //        SlimDX.Vector3 minBox = SlimDX.Vector3.UnitXYZ * -0.5f;
            //        SlimDX.Vector3 maxBox = SlimDX.Vector3.UnitXYZ * 0.5f;
            //        var bBox = new SlimDX.BoundingBox(minBox, maxBox);//单位大小
            //        var corners = bBox.GetCorners();
            //        for (int i = 0; i < 8; ++i)
            //        {
            //            corners[i] = SlimDX.Vector3.TransformCoordinate(corners[i], boxAbsM);//向量点乘矩阵
            //        }

            //        if (iB == 0)
            //        {
            //            absBox = SlimDX.BoundingBox.FromPoints(corners);
            //        }
            //        else
            //        {
            //            absBox = SlimDX.BoundingBox.Merge(absBox, SlimDX.BoundingBox.FromPoints(corners));
            //        }

            //        absBoxMatrixList.Add(SlimDX.Matrix.Invert(ref boxAbsM));//变换矩阵求逆后加进去
            //    }

            //    var hitvalue = rune.Template.DamageCalculationCount;
            //    for (int i = 0; i < hitvalue; ++i)
            //    {
            //        HostMap.TourRoles(absBox, absBoxMatrixList, actorTypes, this.OnVisitRole_ProcBeHurt, arg);
            //        arg.roleIds.Clear();
            //        if (arg.hurtNumber >= rune.Template.MaxDamageTargetNumber)   
            //            return;
            //    }
            //}
            //// 如果是普通的Notifier，则不处理。 现在只处理AttackNotifier(Box)
            ////else // 如果是普通的Notifier，则用技能攻击radius来判断
            ////{
            ////    var loc = Placement.GetLocation();
            ////    if (rune == null)
            ////        return;
            ////    float radius = rune.Template.AttackRadius;
            ////    mHitCount = 0;
            ////    HostMap.TourRoles(ref loc, radius, actorTypes, this.OnVisitRole_ProcBeHurt, arg);
            ////}

            //// 通知客户端本次攻击到的ROLEID
            //NotifyByChain(ntp, rune, arg, runeHandle);
        }

        void NotifyByChain(CSCommon.Animation.NotifyPoint ntp, CSCommon.Data.Skill.RuneData rune, BeHurtArgument arg, int runeHandle)
        {
            if (rune.Template.RuneType == CSCommon.Data.Skill.ERuneType.OneChain || rune.Template.RuneType == CSCommon.Data.Skill.ERuneType.MultiChain)
            {
                if(rune.Template.ChainNotifyPointName!="")
                {
                    if (ntp.NotifyName != rune.Template.ChainNotifyPointName)
                        return;
                }

                if (arg.roleIds.Count > 0)
                {
                    RPC.DataWriter dwParam = new RPC.DataWriter();
                    dwParam.Write(rune.Template.RuneId);
                    dwParam.Write(runeHandle);
                    dwParam.Write(arg.roleIds.Count);
                    foreach (var id in arg.roleIds)
                    {
                        dwParam.Write(id);
                    }
                    RPC.PackageWriter pkg = new RPC.PackageWriter();
                    //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateHurtRole(pkg, dwParam);
                    HostMap.SendPkg2Clients(null, this.Placement.GetLocation(), pkg);
                }
            }
        }

        protected bool OnVisitRole_ProcBeAttacks(Role.RoleActor role, object arg)
        {
            if (role == this)
                return true;

            var ntp = arg as CSCommon.Animation.NotifyPoint;
            
            var state = role.AIStates.GetState("BeAttack");
            if (state != null)
            {
                CSCommon.AISystem.States.IBeAttackParameter param = new CSCommon.AISystem.States.IBeAttackParameter();
                param.AttackerPos = this.Placement.GetLocation();
                CSCommon.AISystem.State.TargetToState(role, "BeAttack", param);
            }

            return true;
        }
        protected void DoTargetBeAttacks(CSCommon.Animation.NotifyPoint ntp)
        {
            var loc = Placement.GetLocation();
            UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Player) | (1 << (Int32)CSCommon.Component.eActorGameType.Npc);
            float radius = 2;//临时代码，正确的做法是从ntp里面获得当前打击范围
            HostMap.TourRoles(ref loc, radius, actorTypes, this.OnVisitRole_ProcBeAttacks, ntp);
        }
        public void ProcTargetBeAttackNotifiers(CSCommon.Animation.AnimationTree anim)
        {
            if (CurrentState == null)
                return;
            if (anim.Action != null)
            {
                var ntf = anim.Action.GetNotifier("TargetBeAttack");
                if (ntf != null)
                {
                    var nplist = ntf.GetNotifyPoints(CurrentState.NotifyPrevTime, anim.CurNotifyTime);
                    if (nplist != null)
                    {
                        foreach (var i in nplist)
                        {
                            this.DoTargetBeAttacks(i);
                        }
                    }
                }
            }

            var subAnims = anim.GetAnimations();
            foreach (var i in subAnims)
            {
                if (i != null)
                {
                    ProcTargetBeAttackNotifiers(i);
                }
            }
        }

        #endregion

        #region 虚拟接口定义
        public virtual bool AIServerControl
        {
            get { return true; }
        }

        public virtual void OnPlacementUpdatePosition(ref SlimDX.Vector3 pos)
        {
            // 更新地图格
            if (!this.HostMap.IsNullMap)
            {
                var mapCell = this.HostMap.GetMapCell(pos.X, pos.Z);
                if (mapCell != null)
                {
                    if (this.HostMapCell != mapCell)
                    {
                        mapCell.AddRole(this);
                    }
                }
            }
        }

        public virtual void OnPlacementUpdateDirectionY(float angle)
        {

        }

        public virtual void OnRoleWalkToBlock()
        {

        }

        public virtual void ResetDefaultState()
        {
            FreshRoleValue();//感觉没必要重新计算属性，恢复要恢复的就好了

            RoleHP = mFinalRoleValue.RoleMaxHp;
            RoleMP = mFinalRoleValue.RoleMaxMp;
            mHatredManager.ClearHatred();
            if (AIStates.DefaultState != null)
            {
                CurrentState = AIStates.DefaultState;
            }
            if(CurrentState != null)
                CurrentState.OnEnterState();
            mIsDeath = false;
        }



        public virtual void OnExitedState(CSCommon.AISystem.State curState)
        {
//             if ( this.AIServerControl && curState.StateName == "StayAttack")
//             {
//                 var target = AttackTarget;
//                 if (target!=null)
//                 {
//                     DoAttackTargetAIBehavior();
//                 }
//             }
        }

        Int64 mPrevUpdateTime;
        public override void Tick(long elapsedMillisecond)
        {
            Int64 time = IServer.timeGetTime();

            if (time - mPrevUpdateTime > 3000)//3秒钟必然同步一次
            {      
                if (this.CurrentState != null &&  this.CurrentState.StateName  != "Death")
                {
                    mPrevUpdateTime = time;
                    var loc = Placement.GetLocation();
                    Placement.SetLocation(loc);
                }
            }

            //    mCurrentAnimation.Update(elapsedMillisecond);
            if (mFSM.FSMTemplate != null)
            {
                if (mCurFSMVersion != mFSM.FSMTemplate.Version)
                {
                    //InitFSM(mFSM.FSMTemplate.Id,false);
                    InitFSM(mFSM.FSMTemplate.Id, true);
                }
            }

            TimerManager.Tick(elapsedMillisecond);

            mHatredManager.Tick(elapsedMillisecond);
            if(BuffBag !=null)
                BuffBag.Tick(elapsedMillisecond);
            if(InitiativeSkillBag != null)
                InitiativeSkillBag.Tick(elapsedMillisecond);
            if (mCurrentState != null)
                mCurrentState.Tick(elapsedMillisecond);

            // 不处理BeAttack Notify， 在Hurt Notify的时候处理是否转换BeAttack状态
            //var anim = this.FSMGetCurrentAnimationTree();
            //if (anim != null)
            //{
            //    ProcTargetBeAttackNotifiers(anim);
            //}
            if (time - mPrevUpdateTime > 1000  &&  RoleCreateType == CSCommon.Data.eRoleCreateType.Player && CurrentState.StateName != "Death")//1秒钟生命恢复一次,怪物生命恢复先不考虑
            {
                if (FinalRoleValue != null && FinalRoleValue.HpRecovery != 0)
                {
                    RoleHP += FinalRoleValue.HpRecovery;
                    mPrevUpdateTime = time;
                    if (RoleHP >= FinalRoleValue.RoleMaxHp)
                    {
                        RoleHP = FinalRoleValue.RoleMaxHp;
                    }
                }
                if (FinalRoleValue != null && FinalRoleValue.MpRecovery != 0)
                {
                    RoleMP += FinalRoleValue.MpRecovery;
                    mPrevUpdateTime = time;
                    if (RoleMP >= FinalRoleValue.RoleMaxMp)
                    {
                        RoleMP = FinalRoleValue.RoleMaxMp;
                    }
                }
            }
         
            //临时代码
            try
            {
                for (int i = 0; i < mSummons.Count;i++ )
                {
                    if (mSummons[i].IsLeaveMap)
                    {
                        mSummons[i].OnLeaveMap();
                        mSummons.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        mSummons[i].Tick(elapsedMillisecond);
                    }
                }
            }
            catch (System.Exception ex)
            {
            	//防止迭代中删除
                System.Diagnostics.Debug.Write(ex.ToString());
                System.Diagnostics.Debug.Write(ex.StackTrace.ToString());
            }            
        }

        #endregion

        #region 一些可以被逻辑连线工具调用的接口
        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Common)]
        [CSCommon.AISystem.Attribute.ToolTip("为角色添加一个逻辑计时器")]
        public void AddLogicTimer(string name, Int64 interval, Guid cbGuid)
        {
            var onTimer = CSCommon.Helper.EventCallBackManager.Instance.GetEventCallee_FOnTimer(cbGuid);//从cbGuid找到一个OnTimer
            if (onTimer!=null)
                TimerManager.AddLogicTimer(name, interval, onTimer);
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Common)]
        [CSCommon.AISystem.Attribute.ToolTip("移除角色制定名字的逻辑计时器")]
        public void RemoveLogicTimer(string name)
        {
            TimerManager.RemoveLogicTimer(name);
        }
        #endregion

        #region StateHost

        ulong mCurFSMVersion;
        public bool InitFSM(Guid fsmId,bool bResetCurrentState)
        {
            var tpl = CSCommon.AISystem.FStateMachineTemplateManager.Instance.GetFSMTemplate(fsmId, CSCommon.Helper.enCSType.Server);
            if(tpl==null)
                return false;
            mFSM.InitFSM(this, tpl, CSCommon.Helper.enCSType.Server);
            mCurFSMVersion = tpl.Version;
            
            if (bResetCurrentState)
                mCurrentState = mFSM.DefaultState;//mFSM.GetState("Idle");
            if (mCurrentState == null)
            {
                Log.Log.Common.Print(string.Format("FSM {0} DefaultState is null!", fsmId));

      //          mCurrentState = new FrameSet.ServerStates.Idle();
            }
            return true;
        }

        CSCommon.AISystem.FStateMachine mFSM = new CSCommon.AISystem.FStateMachine();

        public CSCommon.AISystem.FStateMachine AIStates
        {
            get { return mFSM; }
        }
        CSCommon.AISystem.State mCurrentState;
        public CSCommon.AISystem.State CurrentState
        {
            get { return mCurrentState; }
            set
            {
                if (value == null)
                {
                    System.Diagnostics.Debugger.Break();
                }
                mCurrentState = value;
            }
        }
        CSCommon.AISystem.State mTargetState;
        public CSCommon.AISystem.State TargetState
        {
            get { return mTargetState; }
            set
            {
                mTargetState = value;
            }
        }

        public CSCommon.Component.IActorBase Actor
        {
            get
            {
                return this;
            }
        }

        public virtual CSCommon.RoleTemplate RoleTemplate
        {
            get { return null; }
        }

        bool mStateNotify2Remote = true;
        public virtual bool StateNotify2Remote
        {
            get { return mStateNotify2Remote; }
            set { mStateNotify2Remote = value; }
        }

        public void FSMOnToState(CSCommon.AISystem.State curState, CSCommon.AISystem.StateParameter param, CSCommon.AISystem.State newCurState, CSCommon.AISystem.State newTarState)
        {
            RPC.DataWriter dwParam = new RPC.DataWriter();
            dwParam.Write( param , true);
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_FSMChangeState(pkg, curState.StateName, newCurState.StateName, newTarState != null ? newTarState.StateName : "", dwParam);
            if (this.StateNotify2Remote)
            {
                HostMap.SendPkg2Clients(null, Placement.GetLocation(), pkg);
            }
            else
            {
                HostMap.SendPkg2Clients(this.OwnerRole, Placement.GetLocation(), pkg);                    
            }
        }
        public void FSMSetAction(string name, bool bLoop, float playRate)
        {
            CSCommon.Animation.AnimationTree anim = FSMGetAnimationTreeByActionName(name);

            if (anim != null)
            {
                anim.SetLoop(bLoop);
                anim.SetPlayRate(playRate);
                anim.CurNotifyTime = 0;
            }

            FSMSetCurrentAnimationTree(anim);
        }

        CSCommon.Animation.AnimationTree mCurrentAnimation = null;
        CSCommon.Animation.AnimationTree mIdleAnim = null;
        CSCommon.Animation.AnimationTree mWalkAnim = null;
        CSCommon.Animation.AnimationTree mDeathAnim = null;
        CSCommon.Animation.AnimationTree mBeAttackAnim = null;
        CSCommon.Animation.AnimationTree mMoveAttackAnim = null;
        CSCommon.Animation.AnimationTree mStayAttackAnim = null;
        CSCommon.Animation.AnimationTree mStayChannelAnim = null;
        CSCommon.Animation.AnimationTree mLostControlAnim = null;
        
        
        public void FSMSetBlendAction(string lowHalf, string highHalf)
        {
            List<CSCommon.Animation.AnimationTree> anims = new List<CSCommon.Animation.AnimationTree>{
                FSMGetAnimationTreeByActionName(lowHalf),
                FSMGetAnimationTreeByActionName(highHalf),
            };
            //mBlendActionAnim.SetAnimations(anims);

            FSMSetCurrentAnimationTree(null);
        }

        //public Animation.ActionNode LoadAnimTree( string name )
        //{
        //    if (RoleTemplate == null)
        //    {
        //        Log.Log.Common.Print("Role {0} LoadAnimTree {1} Failed", this.RoleName, name);
        //        return null;
        //    }
        //    var action = RoleTemplate.GetActionNamePair(name);
        //    if (action == null)
        //    {
        //        return null;
        //    }

        //    var anim_action = new Animation.ActionNode();
        //    //anim_action.Initialize();
        //    anim_action.ActionName = action.ActionFile;
        //    anim_action.PlayRate = action.PlayRate;
        //    anim_action.PlayerMode = CSCommon.Animation.EActionPlayerMode.Default;
        //    anim_action.XRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
        //    anim_action.YRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
        //    anim_action.ZRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
        //    anim_action.CurNotifyTime = 0;

        //    return anim_action;
        //}

        public CSCommon.Animation.AnimationTree FSMGetAnimationTreeByActionName(string name)
        {
            //CSCommon.Animation.AnimationTree anim = null;
            ////这里暂时可以用switch，以后可以用Dictionary
            //switch (name)
            //{
            //    case "Idle":
            //        if (mIdleAnim == null)
            //            mIdleAnim = LoadAnimTree("Idle");
            //        return mIdleAnim;
            //    case "Walk":
            //        if (mWalkAnim == null)
            //            mWalkAnim = LoadAnimTree("Walk");
            //        return mWalkAnim;
            //    case "Death":
            //        if (mDeathAnim == null)
            //            mDeathAnim = LoadAnimTree("Death");
            //        return mDeathAnim;
            //    case "BeAttack":
            //        if (mBeAttackAnim == null)
            //            mBeAttackAnim = LoadAnimTree("BeAttack");
            //        return mBeAttackAnim;
            //    case "MoveAttack":
            //        if (mMoveAttackAnim == null)
            //            mMoveAttackAnim = LoadAnimTree("MoveAttack");
            //        return mMoveAttackAnim;
            //    case "StayAttack":
            //        if (mStayAttackAnim == null)
            //            mStayAttackAnim = LoadAnimTree("StayAttack");
            //        return mStayAttackAnim;
            //    case "StayChannel":
            //        if (mStayChannelAnim == null)
            //            mStayChannelAnim = LoadAnimTree("StayChannel");
            //        return mStayChannelAnim;
            //    case "LostControl":
            //        if (mLostControlAnim == null)
            //            mLostControlAnim = LoadAnimTree("LostControl");
            //        return mLostControlAnim;
            //    default:
            //        break;
            //}

            //if (anim == null && RoleTemplate != null)
            //{
            //    var action = RoleTemplate.GetActionNamePair(name);
            //    if (action == null)
            //    {
            //        return null;
            //    }
            //    //这里用 action.ActionFile 去设置动作文件
            //    var anim_action = new Animation.ActionNode();
            //    //anim_action.Initialize();
            //    anim_action.ActionName = action.ActionFile;
            //    anim_action.PlayRate = action.PlayRate;
            //    anim_action.PlayerMode = CSCommon.Animation.EActionPlayerMode.Default;
            //    anim_action.XRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
            //    anim_action.YRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
            //    anim_action.ZRootmotionType = CSCommon.Animation.AxisRootmotionType.ART_Default;
            //    anim_action.CurNotifyTime = 0;
            //    //anim_action.DelegateOnActionFinish = 
            //    anim = anim_action;
            //    anim.CurNotifyTime = 0;
            //}

            //return anim;
            return null;
        }

        public CSCommon.Animation.AnimationTree FSMGetCurrentAnimationTree()
        {
            return mCurrentAnimation;
        }

        public void FSMSetCurrentAnimationTree(CSCommon.Animation.AnimationTree anim)
        {
            mCurrentAnimation = anim;
        }

        public CSCommon.Animation.AnimationTree CreateAnimationNode()
        {
            //return new Planes.Role.Animation.AnimNode();
            return null;
        }

        public CSCommon.Animation.BaseAction CreateBaseAction()
        {
            //return new Planes.Role.Animation.ActionNode();
            return null;
        }

        public void SetAnimTree(CSCommon.Animation.AnimationTree animtree)
        {
            mCurrentAnimation = animtree;
        }

        CSCommon.Helper.LogicTimerManager mTimerManager = new CSCommon.Helper.LogicTimerManager();
        public CSCommon.Helper.LogicTimerManager TimerManager 
        {
            get
            {
                return mTimerManager;
            }
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        public void PushStateExitCallee(Guid cbId)
        {
            var cb = CSCommon.Helper.EventCallBackManager.Instance.GetCallee(typeof(CSCommon.AISystem.FOnStateExit), cbId);
            if (cb!=null)
                PushStateExit(cb);
        }

        public void PushStateExit(CSCommon.Helper.EventCallBack cb)
        {
            mOnStateExitQueue.Enqueue(cb);
        }

        Queue<CSCommon.Helper.EventCallBack> mOnStateExitQueue = new Queue<CSCommon.Helper.EventCallBack>();
        public CSCommon.Helper.EventCallBack PopStateExit()
        {
            if (mOnStateExitQueue.Count==0)
                return null;
            return mOnStateExitQueue.Dequeue();
        }

        public virtual bool OnValueChanged(string name, RPC.DataWriter value)
        {
            switch (name)
            {
                case "RoleHP":
                case "RoleMP":
                case "RoleLevel":                    //这些都是玩家要看到的
                    if (!HostMap.IsNullMap)
                    {
                        var pkg = new RPC.PackageWriter();
                        //ExamplePlugins.ZeusGame.H_IGame.smInstance.HIndex(pkg, this.Id).RPC_UpdateRoleValue(pkg, name, value);
                        HostMap.SendPkg2Clients(null, Placement.GetLocation(), pkg);
                    }
                    return true;
            }
            return false;
        }

        public virtual bool OnComValueChanged(string name, RPC.DataWriter value)
        {
            return false;
        }

        public float GetAltitude(float x, float z)
        {
            return this.HostMap.GetAltitude(x, z);
        }
        #endregion

        TaskManager mTaskManager = new TaskManager();
        public TaskManager TaskManager
        {
            get { return mTaskManager; }
        }

        MailManager mMailManager = new MailManager();
        public MailManager MailManager
        {
            get { return mMailManager; }
        }

        #region 背包
        Bag.BagBase mBag = new Bag.BagBase();
        public Bag.BagBase Bag
        {
            get { return mBag; }
        }

        Bag.BagBase mStore = new Bag.BagBase();
        public Bag.BagBase Store
        {
            get { return mStore; }
        }

        

        Bag.EquipBag mEquip = new Bag.EquipBag();
        public Bag.EquipBag Equip
        {
            get { return mEquip; }
        }

        Bag.SkillBag mInitiativeSkillBag = new Bag.SkillBag();
        public Bag.SkillBag InitiativeSkillBag
        {
            get { return mInitiativeSkillBag; }
        }

        Bag.SkillBag mPassiveSkillBag = new Bag.SkillBag();
        public Bag.SkillBag PassiveSkillBag
        {
            get { return mPassiveSkillBag; }
        }

        Bag.ProxySkillBag mProxySkillBag = new Bag.ProxySkillBag();
        public Bag.ProxySkillBag ProxySkillBag
        {
            get { return mProxySkillBag; }
        }

        Bag.BuffBag mBuffBag = new Bag.BuffBag();
        public Bag.BuffBag BuffBag
        {
            get { return mBuffBag; }
        }

        #endregion

        #region 技能系统

        public virtual CSCommon.Data.Skill.SkillData FindSkillData(UInt16 templateId)
        {
            return null;
        }
        public virtual CSCommon.Data.Skill.RuneData FindRuneData(UInt16 templateId)
        {
            
            return null;
        }

        public virtual CSCommon.Data.Skill.BuffData FindBuffData(UInt16 templateId)
        {
            return null;
        }

        #endregion

        #region 物品掉落

        protected virtual UInt16 GetItemDropper()
        {
            return 0;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("根据掉落数据掉落物品")]
        public void DropItems(UInt16 dropId,Role.RoleActor picker)
        {
            if (dropId == UInt16.MaxValue)
            {
                //这里缺省掉落
                dropId = GetItemDropper();
            }

//             var dropper = CSCommon.Data.Item.DropTemplateManager.Instance.GetDropper(dropId);
//             if (dropper != null)
//             {
//                 
//                 var items = ServerCommon.Planes.Bag.Item.DangerouseDropItems(picker,dropper);
// 
//                 foreach (var item in items)
//                 {
//                     var fromPos = Placement.GetLocation();
//                     var dropPos = Placement.GetLocation();
//                     //这里应该在附近随机找能掉落物品的位置
//                     float radius = 2.0f;
//                     dropPos.X += (GetRandomUnit() * 2.0F - 1.0f) * radius;
//                     dropPos.Z += (GetRandomUnit() * 2.0F - 1.0f) * radius;
//                     dropPos.Y = mHostMap.GetAltitude(dropPos.X, dropPos.Z);
// 
//                 }
//             }
        }
        #endregion

        #region AI扩展
        List<RoleActor> mNearRoles = new List<RoleActor> { };
        public List<RoleActor> FindRolesNearBy(float length)
        {
            mNearRoles.Clear();
            var loc = this.Placement.GetLocation();
            UInt32 actorTypes =  (1 << (Int32)CSCommon.Component.eActorGameType.Npc);
            mNearByValue = length;
            this.HostMap.TourRoles(ref loc, length, actorTypes, this.OnVisitRole_GetNearByRole, null);
            return mNearRoles;
        }

        private float mNearByValue;
        private bool OnVisitRole_GetNearByRole(RoleActor role, object arg)
        {
            if (role == this)
                return true;

            float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
            if (mNearByValue > dist)
            {
                mNearRoles.Add(role);
            }
            return true;
        }

        public List<RoleActor> FindFactionRolesNearBy(float length)
        {
            mNearRoles.Clear();
            var loc = this.Placement.GetLocation();
            UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Npc);
            mNearByValue = length;
            this.HostMap.TourRoles(ref loc, length, actorTypes, this.OnVisitRole_GetFactionRole, null);
            return mNearRoles;
        }

        private bool OnVisitRole_GetFactionRole(RoleActor role, object arg)
        {
            if (role == this)
            {
                return true;
            }
            var faction = CSCommon.Helper.FactionManager.Instance.FindFaction(FactionId);
            float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
            if (mNearByValue > dist)
            {
                if (faction != null)
                {
                    if (faction.IsFriend(role.FactionId) || role.FactionId == FactionId)
                        mNearRoles.Add(role);
                }
            }
            return true;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("主动巡逻，主动添加仇恨,主动攻击")]
        public void ActiveFindPlayerAtt()
        {
//             var loc = this.Placement.GetLocation();       
//             UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Player);
//             var arg = new LockOnSelector();
//             this.HostMap.TourRoles(ref loc, this.RoleTemplate.LockOnRadius, actorTypes, this.OnVisitRole_AddHatred, arg);
// 
//             if (arg.LockOnTarget != null)
//             {//选择了这个对象作为打击目标，添加一点仇恨
//                 HatredManager.AddHatred(arg.LockOnTarget, 1);
//             }
//             DoAttackTargetAIBehavior();
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("主动添加仇恨,主动攻击")]
        public void ActiveStayToAtt()
        {
//             var loc = this.Placement.GetLocation();
//             UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Player);
//             var arg = new LockOnSelector();
//             this.HostMap.TourRoles(ref loc, this.RoleTemplate.LockOnRadius, actorTypes, this.OnVisitRole_AddHatred, arg);
//    
//             if (arg.LockOnTarget != null)
//             {//选择了这个对象作为打击目标，添加一点仇恨
//                 float dist = SlimDX.Vector3.Distance(arg.LockOnTarget.Placement.GetLocation(), Placement.GetLocation());    
//                 if (dist <= RoleTemplate.WanderRadius)
//                 {
//                 HatredManager.AddHatred(arg.LockOnTarget, 1);
//                 }
//             }
//             DoAttackCurTargetAIBehavior();
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("主动添加仇恨，有敌人立刻攻击")]
        public void ActiveAddHatredtoIdle()
        {
//             var role = FindTargetRole(RoleTemplate.LockOnRadius);
//             if (role == null)
//                 return;
//             float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
//             if (role != null && dist <= RoleTemplate.WanderRadius)
//             {
//                 HatredManager.AddHatred(role, 1);
//                 this.CurrentState.ToState("Idle", null);
//             }
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("不主动添加仇恨，有敌人立刻攻击")]
        public void ActiveFindTarget()
        {
//             var role = FindClosestRole(RoleTemplate.LockOnRadius);
//             if (role == null)
//                 return;
//             float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
//             if (role != null && dist <= RoleTemplate.WanderRadius)
//             {
//                var val =HatredManager.FindHatred(role);
//                 if (val!= null &&  val.Value !=0)
//                 {
//                     this.CurrentState.ToState("Idle", null);
//                 }     
//             }
        }

        private bool OnVisitRole_AddHatred(RoleActor role, object arg)
        {
            var selector = arg as LockOnSelector;
            if (selector == null)
                return false;
            if (this.CanLockon(role) == false)
                return true;

            float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
            if (selector.Distance > dist)
            {
                selector.LockOnTarget = role;
                selector.Distance = dist;
            }
            return true;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("找离自己最近的role")]
        public RoleActor FindClosestRole(float length)
        {
            var pos = Placement.GetLocation();
            var arg = new LockOnSelector();
            UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Player) | (1 << (Int32)CSCommon.Component.eActorGameType.Npc);
            HostMap.TourRoles(ref pos, length, actorTypes, this.OnVisitRole_SelectClosestDir, arg);
            return arg.LockOnTarget;
        }

        protected bool OnVisitRole_SelectClosestDir(Role.RoleActor role, object arg)
        {
            var selector = arg as LockOnSelector;
            if (role.Id == this.Id)
            {
                return true;
            }
            if (selector == null)
                return false;
        
            float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
            if (selector.Distance > dist)
            {
                selector.LockOnTarget = role;
                selector.Distance = dist;
            }
            return true;
        }

        public RoleActor FindTargetRole(float length)
        {
            var pos = Placement.GetLocation();
            var arg = new LockOnSelector();
            UInt32 actorTypes = (1 << (Int32)CSCommon.Component.eActorGameType.Player) | (1 << (Int32)CSCommon.Component.eActorGameType.Npc);
            HostMap.TourRoles(ref pos, length, actorTypes, this.OnVisitRole_SelectClosestTargetDir, arg);
            return arg.LockOnTarget;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("追踪最近Role")]
        public void TrackNearPole(float length, float acceler,float lerp)
        {
            var tarRole = FindTargetRole(length);
//             if (tarRole.OwnerRole != null)
//             {
//                 OwnerRole.mHatredManager.SetAttackerSingle2Client(tarRole);
//             }

            if (this.CanLockon(tarRole) == false)
                return;
            if (tarRole == null)
                return;
            var walkState = this.AIStates.GetState("Walk");
            if (walkState != null)
            {
                var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
                walkParam.Accelerate = acceler;
                var ssp = new ServerFrame.Support.SimpleSpline();
                var pt = this.Placement.GetLocation();
                var spt = this.Placement.GetLocation() + (walkParam.TargetPosition - this.Placement.GetLocation()) * 0.02F;
                var tpt = this.Placement.GetLocation() +(tarRole.Placement.GetLocation() + (walkParam.TargetPosition - tarRole.Placement.GetLocation()) * 0.6f - this.Placement.GetLocation()) * lerp;
                var ept = tarRole.Placement.GetLocation();
                float dist = SlimDX.Vector3.Distance(Placement.GetLocation(), tarRole.Placement.GetLocation());

                ssp.AddPoint(ref pt);
               // ssp.AddPoint(ref spt);
                ssp.AddPoint(ref tpt);
                ssp.AddPoint(ref ept);
                walkParam.TargetPositions.Clear();
                for (int i = 1; i <13; i++)
                {
                    pt = ssp.Interpolate(((float)i) / 13.0F);
                    walkParam.TargetPositions.Enqueue(pt);
                }
                walkParam.TargetPositions.Enqueue(tarRole.Placement.GetLocation());
                walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
                this.CurrentState.ToState("Walk", walkParam);
            }
        }

        protected bool OnVisitRole_SelectClosestTargetDir(Role.RoleActor role, object arg)
        {
            var selector = arg as LockOnSelector;
            if (selector == null)
                return false;
            if (role.Id == this.Id)
            {
                return true;
            }
            var walkState = this.AIStates.GetState("Walk");
            if (walkState == null)
                return true;

            var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            var staDir = walkParam.TargetPosition - Placement.GetLocation();
            staDir.Y = 0;
            staDir.Normalize();
            var endDir = role.Placement.GetLocation() - Placement.GetLocation();
            endDir.Y = 0;
            endDir.Normalize();

            float angle = SlimDX.Vector3.Dot(staDir, endDir);
            if (angle < 0)
                return true;
            //             endDir.Y = 0;
            //             endDir.Normalize();
            //             float angle = (float)System.Math.Atan2(endDir.Z, endDir.X);
            //             if (System.Math.Abs(angle - Placement.GetDirection()) > System.Math.PI / 2)
            //             {
            //                 return true;
            //             }
            float dist = SlimDX.Vector3.Distance(role.Placement.GetLocation(), Placement.GetLocation());
            if (selector.Distance > dist)
            {
                selector.LockOnTarget = role;
                selector.Distance = dist;
            }
            return true;
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("角色获得了Buff")]
        public void  SetRoleBuff(int buffid,byte level,float rate)
        {
            CSCommon.Data.Skill.RuneTemplate.RuneLevelParam.BuffParam buffparam = new CSCommon.Data.Skill.RuneTemplate.RuneLevelParam.BuffParam();
            buffparam.BuffId = buffid;
            buffparam.BuffLevel = level;
            buffparam.SpellRate = rate;
            BuffBag.CreateBuffAndAutoAdd2Bag(this, buffparam);
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("广播消息")]
        public void BroadcastNews(UInt16 newsid)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            HostMap.SendPkg2Clients(null, Placement.GetLocation(), pkg);
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("请求附近帮助")]
        public void CallHelpNearby(float length)
        {
            //if (length <= 0)
            //    return;
            //FindRolesNearBy(length);

            //if (this.AttackTarget == null)
            //    return;       
            //foreach (RoleActor role in mNearRoles)
            //{
            //    if (role.RoleTemplate.ReplyHelp == false)
            //        continue;
            //    role.HatredManager.AddHatred(this.AttackTarget, 1);
            //    MoveToTargetPos(this.AttackTarget.Placement.GetLocation(),1);
            //    role.IfDaAttackAI = true;
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("请求同阵营或友方阵营的帮助")]
        public void CallFactionHelpNearby(float length)
        {
            //if (length <= 0)
            //    return;
            //FindFactionRolesNearBy(length);

            //if (this.AttackTarget == null)
            //    return;
            //foreach (RoleActor role in mNearRoles)
            //{
            //    if (role.RoleTemplate.ReplyHelp == false)
            //        continue;
            //    role.HatredManager.AddHatred(this.AttackTarget, 1);
            //    MoveToTargetPos(this.AttackTarget.Placement.GetLocation(), 1);
            //    role.IfDaAttackAI = true;
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("逃跑")]
        public void RoleRunAway(float speed, float length)
        {
            var target = SelectAttackTarget();
            if (target == null)
            {
                return;
            }
            else
            {
                if (CurrentState.StateName != "Walk")
                {
                    DoRunAwayAIBehavior(target, speed, length);
                }
            }
        }

        public void DoRunAwayAIBehavior(RoleActor target, float speed, float length)
        {
            //var beAttState = this.AIStates.GetState("BeAttack");
            //if (beAttState != null)
            //{
            //    var walkState = this.AIStates.GetState("Walk");
            //    var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            //    var runto = Placement.GetLocation() - target.Placement.GetLocation();
            //    runto.Y = 0;
            //    runto.Normalize();
            //    walkParam.TargetPosition = Placement.GetLocation() + (runto) * length;
            //    walkParam.MaxCloseDistance = 0.05f;
            //    walkParam.MoveSpeed = speed;
            //    var start = Placement.GetLocation();
            //    var pkg = new RPC.PackageWriter();

            //    H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //    pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //    {
            //        if (bTimeOut)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            //Byte pathFindResult;
            //            //_io.Read(out pathFindResult);
            //            //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //            //{
            //            //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //            //        {
            //            //            int count = 0;
            //            //            _io.Read(out count);
            //            //            for (int i = 0; i < count; i++)
            //            //            {
            //            //                SlimDX.Vector2 pathPt;
            //            //                _io.Read(out pathPt);

            //            //                var pt = new SlimDX.Vector3();
            //            //                pt.X = pathPt.X;
            //            //                pt.Z = pathPt.Y;
            //            //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //            //                walkParam.TargetPositions.Enqueue(pt);
            //            //            }

            //            //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //            //            walkParam.Run = 1;
            //            //            this.IsRun = 1;
            //            //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //            //            this.CurrentState.ToState("Walk", walkParam);
            //            //        }
            //            //        break;
            //            //    default:
            //            //        {
            //            //            this.CurrentState.ToState("Idle", null);
            //            //        }
            //            //        break;
            //            //}
            //        }
            //    };
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("到某个地方去")]
        public void MoveToTargetPos(SlimDX.Vector3 targetPos,float perSpeed)
        {
            //var walkState = this.AIStates.GetState("Walk");
            //if (walkState != null)
            //{         
            //    var walkParam = walkState.Parameter as CSCommon.AISystem.States.IWalkParameter;
            //    walkParam.TargetPosition = targetPos;
            //    walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run) * perSpeed;
            //    walkParam.MaxCloseDistance = 0.05f;
            //    var start = Placement.GetLocation();
            //    var pkg = new RPC.PackageWriter();

            //    H_RPCRoot.smInstance.HGet_PathFindServer(pkg).GlobalMapFindPath(pkg, HostMap.PlanesInstance.PlanesId, HostMap.MapSourceId, this.Id, start, walkParam.TargetPosition);
            //    pkg.WaitDoCommandWithTimeOut(3, IPlanesServer.Instance.PathFindConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            //    {
            //        if (bTimeOut)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            Byte pathFindResult;
            //            _io.Read(out pathFindResult);
            //            //switch ((Navigation.INavigationWrapper.enNavFindPathResult)pathFindResult)
            //            //{
            //            //    case Navigation.INavigationWrapper.enNavFindPathResult.ENFR_Success:
            //            //        {
            //            //            int count = 0;
            //            //            _io.Read(out count);
            //            //            for (int i = 0; i < count; i++)
            //            //            {
            //            //                SlimDX.Vector2 pathPt;
            //            //                _io.Read(out pathPt);

            //            //                var pt = new SlimDX.Vector3();
            //            //                pt.X = pathPt.X;
            //            //                pt.Z = pathPt.Y;
            //            //                pt.Y = HostMap.GetAltitude(pt.X, pt.Z);
            //            //                walkParam.TargetPositions.Enqueue(pt);
            //            //            }

            //            //            walkParam.TargetPosition = walkParam.TargetPositions.Dequeue();
            //            //            walkParam.Run = 1;
            //            //            this.IsRun = 1;
            //            //            walkParam.MoveSpeed = this.GetMoveSpeed(walkParam.Run);
            //            //            this.CurrentState.ToState("Walk", walkParam);
            //            //        }
            //            //        break;
            //            //    default:
            //            //        {
            //            //            this.CurrentState.ToState("Idle", null);
            //            //        }
            //            //        break;
            //            //}
            //        }
            //    };
            //}
        }

        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("呼叫制定的Role")]
        public RoleActor CallForRole(UInt32 singleId)
        {
            RoleActor role = HostMap.GetRole(singleId);
            if (role != null)
            {
                return role;
            }
            return null;
        }

        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("在指定的频道发消息")]
        public virtual void SendMessage(sbyte channel, String msg)
        {
           // RPC_Say(channel, msg, null, null);
        }

        [CSCommon.Event.Attribute.AllowMember(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.AllowMethod(CSCommon.Helper.enCSType.Server)]
        [CSCommon.AISystem.Attribute.ToolTip("计算两点之间的距离")]
        public float GetDistance(SlimDX.Vector3 pos1,SlimDX.Vector3 pos2)
        {   
            return SlimDX.Vector3.Distance(pos1, pos2);
        }
        #endregion
    }
}
