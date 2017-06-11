using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using ServerFrame;

namespace ServerCommon.Planes
{

    public interface IStateHost
    {
        bool OnValueChanged(string name, RPC.DataWriter value);
        //void SwitchStateTo(eFsmState state,object param);
    }

    public class MoveNodes<T>
    {
        public LinkedListNode<T> currNode = null;
        public LinkedList<T> targetPosNode = new LinkedList<T>();

	    public void CleanUp()
	    {
            currNode = null;
		    targetPosNode.Clear();
	    }

	    public bool IsEmpty()
	    {
		    return (targetPosNode.Count() == 0 || null == currNode);
	    }

        public bool IsFirst()
        {
            return (targetPosNode.Count() == 0 || currNode == targetPosNode.First);
        }

	    public void AddTargetPos(T targetPos)
	    {
		    targetPosNode.AddLast(targetPos);
	    }

	    public bool GetCurTargetPos(ref T worldPos)
	    {
            if (null == currNode)
			    return false;

		    worldPos = currNode.Value;
		    return true;
	    }

        public bool GetNextTargetPos(ref T worldPos)
        {
            if (null == currNode)
                return false;
            if (null == currNode.Next)
                return false;

            worldPos = currNode.Next.Value;
            return true;
        }

	    public bool GetFinalTarPos(ref T worldPos)
	    {
		    if (targetPosNode.Count() == 0)
			    return false;

		    worldPos =  targetPosNode.Last.Value;
		    return true;
	    }

        public bool NextTargetPos()
        {
            if (currNode == targetPosNode.Last)
                return false;

            currNode = currNode.Next;
            return true;
        }
    };

    /// <summary>
    /// 在地图中的角色对象
    /// </summary>
    public partial class RoleActor : AActorBase, IStateHost
    {
        public RoleActor()
        {
            //mHatredManager = new Fight.Hatred(this);
            mFightState = eRoleFightState.PKProtect;
        }

        public virtual eElemType ElemType
        {
            get { return eElemType.Gold; }
        }


        public virtual string RoleName
        {
            get { return "无名氏"; }
        }

        public virtual eCamp Camp
        {
            get { return eCamp.None; }
        }

        int mRoleLevel;
        public virtual int RoleLevel
        {
            get { return mRoleLevel; }
            set { mRoleLevel = value; }
        }

        public override bool Initialize(IActorInitBase initBase)
        {
            mBuffMgr.SetOwner(this);
            mPlayerStatus.CleanUp();
            if (!base.Initialize(initBase))
                return false;

            return true;
        }

        public void SetPosition(ref Vector3 pos)
        {
            OnPlacementUpdatePosition(ref pos);
        }

        public void SetPosition(float x, float y, float z)
        {
            Vector3 pos = new Vector3(x, y, z);
            SetPosition(ref pos);
        }
  
        protected virtual void OnPlacementUpdatePosition(ref Vector3 pos)
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

        public void SetDir(float dir)
        {
            OnPlacementUpdateDirectionY(dir);
        }

        protected virtual void OnPlacementUpdateDirectionY(float angle)
        {

        }


        public virtual bool OnValueChanged(string name, RPC.DataWriter value)
        {
            switch (name)
            {
                case "RoleHp":
                case "RoleMaxHp":
                case "RoleLevel":                    //这些都是玩家要看到的
                    if (!HostMap.IsNullMap)
                    {
                        RoleActor ignore = name == "RoleHp" && null != CurrImpact && CurrImpact is AttackImpact ? CurrImpact.Owner : null;
                        var pkg = new RPC.PackageWriter();
                        Wuxia.H_RpcRoot.smInstance.HIndex(pkg,this.Id).RPC_UpdateRoleValue(pkg, name, value);
                        HostMap.SendPkg2Clients(ignore, GetPosition(), pkg);
                    }
                    return true;
            }
            return false;
        }

        protected MoveNodes<Vector3> mMoveNodes = new MoveNodes<Vector3>();
        protected uint mSynPositionTime;
        protected float mDeadBodyTime;

        public Vector3 GetPosition()
        {
            return Placement.GetLocation();
        }

        public float GetDirection()
        {
            return Placement.GetDirection();
        }

        protected bool MoveLogic(uint lapseTime)
        {
            if (!mPlayerStatus.IsCanMove())
                return false;

            Vector3 curPos = GetPosition();
	        Vector3 targetPos = Vector3.Zero;
	        if (!mMoveNodes.GetCurTargetPos(ref targetPos))
		        return false;

            mSynPositionTime += lapseTime;

            if (mSynPositionTime < GameSet.Instance.SyncPosTime)
                return true;

	        float speed = FinalRoleValue.Speed;
	        if (speed <= 0.1)
	        {
		        speed = 5.0f;
	        }

	        // 这一帧可移动的路径长度
	        float moveDist = (speed * mSynPositionTime) / 1000.0f;
            if (moveDist < GameSet.Instance.PosSyncDistRange)
                return true;

            mSynPositionTime = 0;
            // 当前位置与当前的目标路径点的水平距离
	        float distToTarget = Util.DistanceH(curPos, targetPos);
            while (true)
            {
                if (distToTarget > GameSet.Instance.PosSyncDistRange)
                    break;

                if (!mMoveNodes.NextTargetPos())
                    break;

                Vector3 tmp = targetPos;
                if (!mMoveNodes.GetCurTargetPos(ref targetPos))
                    return false;

                distToTarget = Util.DistanceH(curPos, targetPos);
            }
            Vector3 posMustTo = targetPos;
            if (moveDist < distToTarget && Math.Abs(moveDist - distToTarget) > GameSet.Instance.PosSyncDistRange)
            {
                posMustTo.X = curPos.X +
                    (moveDist * (targetPos.X - curPos.X)) / distToTarget;
                posMustTo.Z = curPos.Z +
                    (moveDist * (targetPos.Z - curPos.Z)) / distToTarget;
            }
            else if (curPos == targetPos)
            {
                if (!mMoveNodes.NextTargetPos())
                {
                    mMoveNodes.CleanUp();
                    return true;
                }
            }
	   
            DirectMoveTo(ref posMustTo, true);

            return (!mMoveNodes.IsEmpty());
        }

        public bool DirectMoveTo(ref Vector3 targetPos, bool isBroadCast)
        {
            this.Placement.Move(ref targetPos);
	
	        if (isBroadCast)
	        {
                //广播位置
	        }

	        return true;
        }

        public bool MoveTo(Vector3 targetPos)
        {
            LinkedList<Vector3> nodes;
            if (null == mHostMap.Navigator) return false;

	        //if (Util.FindPath(HostMap.MapSourceId, GetPosition(), targetPos, ref nodes))
            if (mHostMap.Navigator.FindPath(GetPosition(), targetPos, out nodes))
	        {
                SetMoveNodes(nodes);
		        mSynPositionTime = 0;
		        return true;
	        }

	        return false;
        }

        public void SetMoveNodes(LinkedList<Vector3> nodes)
        {
            if (nodes.Count() > 0)
            {
                if (Util.DistanceH(nodes.First.Value, nodes.Last.Value) < (nodes.Count() - 1) * 0.1f)
                    return;
            }
	        mMoveNodes.CleanUp();
		    mMoveNodes.targetPosNode = nodes;
            mMoveNodes.currNode = mMoveNodes.targetPosNode.First;
        }
	    
        public bool IsMoving
        {
            get { return !mMoveNodes.IsEmpty(); }
        }

	    protected void CleanMoveNodes()
        {
            mMoveNodes.CleanUp();
        }

        Int64 mPrevUpdateTime;
        //两次调用Tick间的时间差(单位：毫秒，整数)
	    public uint LogicTime
        { 
            get { return (uint)(mNowTime - mLastTime); }
        }
        protected long mLastTime;
        protected long mNowTime;
        protected long mCreateTime;

        private void UpdateTime(long nowTime)
        {
            if (mNowTime == 0)
            {
                mLastTime = nowTime;
                mCreateTime = nowTime;
            }
            else
            {
                mLastTime = mNowTime;
            }
            mNowTime = nowTime;
        }

        public override void Tick(long elapsedMillisecond)
        {
            Int64 time = IServer.timeGetTime();
            UpdateTime(time);
            if (time - mPrevUpdateTime > 3000)//3秒钟必然同步一次
            {
                if (!this.IsDie)
                {
                    mPrevUpdateTime = time;
                    var loc = GetPosition();
                    Placement.SetLocation(ref loc);
                }
            }
            mBuffMgr.Tick(elapsedMillisecond);
            if (time - mCreateTime > 1000)
            {
                if (mFightState == eRoleFightState.PKProtect)
                    mFightState = eRoleFightState.Normal;
            }
        }

        #region 物品掉落

        protected virtual UInt16 GetItemDropper()
        {
            return 0;
        }

        public void DropItems(UInt16 dropId,RoleActor picker)
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
//                     var fromPos = GetPosition();
//                     var dropPos = GetPosition();
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

        RoleActor mFatherRole = null;
        public virtual RoleActor FatherRole
        {
            get { return mFatherRole; }
            set { mFatherRole = value; }
        }
        public List<RoleActor> mSonRole = new List<RoleActor>();

        string mFallowedRole;
        public virtual string FallowedRole
        {
            get { return mFallowedRole; }
            set { mFallowedRole = value; }
        }

        public virtual RoleActor FindTargetTemplateRole(float length)
        {
            return null;
        }

        public virtual RoleActor GetTarget(ulong targetId)
        {
            if (null == mHostMap) return null;

            return mHostMap.GetRole(targetId);
        }

    }
}
