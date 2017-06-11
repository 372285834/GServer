using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFrame;

namespace ServerCommon.Planes
{
    public class RolePlacement : IStandPlacement
    {
        public RolePlacement(AActorBase host) 
                    : base(host)
        {
            
        }
        public override bool SetLocation(ref SlimDX.Vector3 loc)
        {
            base.SetLocation(ref loc);

            OnUpdatePosition(ref mLocation);
            return true;
        }

        public override bool Move(ref SlimDX.Vector3 pos)
        {
            var actor = GetActor() as RoleActor;
            if (actor == null || actor.HostMap.IsNullMap)
                return false;

            if (actor.FightState != CSCommon.eRoleFightState.Normal
                && actor.FightState != CSCommon.eRoleFightState.Slient)
                return false;

            //SlimDX.Vector3 targetPoint = mLocation + dir * delta;


            //targetPoint.Y = actor.HostMap.GetAltitude(targetPoint.X, targetPoint.Z);

            SetLocation(ref pos);

            return true;
        }

        //public override bool AutoMove(ref SlimDX.Vector3 dir, float delta)
        //{
        //    dir.Normalize();
        //    return Move(ref dir, delta);
        //}

        bool mIsUpdate2Client = true;
        public bool IsUpdate2Client
        {
            get { return mIsUpdate2Client; }
            set { mIsUpdate2Client = value; }
        }
        SlimDX.Vector3 mPrevPos;
        Int64 mPrevUpdateTime;
        private void OnUpdatePosition(ref SlimDX.Vector3 loc)
        {
            RoleActor role = mHostActor as RoleActor;
            if (role != null && !role.IsDie)
            {
                role.SetPosition(ref loc);

                if (mIsUpdate2Client == false)
                    return;
                Int64 time = IServer.timeGetTime();
                if (time - mPrevUpdateTime > 3000)//3秒钟必然同步一次
                {
                    mPrevUpdateTime = time;
                    if (!role.HostMap.IsNullMap)
                    {
                        role.HostMap.RolePositionChanged(role, ref loc);
                        mPrevPos = loc;
                    }
                }
                else
                {
                    float dist = Util.DistanceH(loc, mPrevPos);
                    if (dist > GameSet.Instance.PosSyncDistRange)
                    {
                        mPrevPos = loc;

                        if (!role.HostMap.IsNullMap)
                            role.HostMap.RolePositionChanged(role, ref loc);
                    }
                }
            }
        }

        float mPrevAngle;
        protected override void OnUpdateRoationY(float angle)
        {
            base.OnUpdateRoationY(angle);
            RoleActor role = mHostActor as RoleActor; 
            if (role != null)
            {
                role.SetDir(angle);
            }

            if (mIsUpdate2Client == false)
                return;

            Int64 time = IServer.timeGetTime();
            if (time - mPrevUpdateTime > 3000)//3秒钟必然同步一次
            {
                mPrevUpdateTime = time;
                if (!role.HostMap.IsNullMap)
                    role.HostMap.RoleDirectionChanged(role, angle);
            }
            else
            {
                float dist = System.Math.Abs(mPrevAngle-angle);
                if (dist > System.Math.PI*5/180)
                {
                    mPrevAngle = angle;

                    if (!role.HostMap.IsNullMap)
                        role.HostMap.RoleDirectionChanged(role, angle);
                }
            }
        }
    }
}
