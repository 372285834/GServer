using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Component
{
    //public class IGravityComp : IComponent
    //{
    //    protected static float	mWorldGravity = -9.8f;
    //    protected static float	mEpsilon = 0.05f;
    //    protected float			mGravity;
    //    protected SlimDX.Vector3	mVelocity;
    //    protected bool			mSuspend;
    //    protected bool			mIsFloating;
        
    //    public bool IsFloating
    //    {
    //        get { return mIsFloating; }
    //    }
    //    public static float WorldGravity
    //    {
    //        get { return mWorldGravity; }
    //        set { mWorldGravity = value; }
    //    }
    //    public float Gravity
    //    {
    //        get { return mGravity; }
    //        set { mGravity = value; }
    //    }
    //    public bool Suspend
    //    {
    //        get { return mSuspend; }
    //        set { mSuspend = value; }
    //    }
    //    public SlimDX.Vector3 Velocity
    //    {
    //        get { return mVelocity; }
    //        set { mVelocity = value; }
    //    }

    //    public override void Tick(IActorBase host, long elapsedMillisecond)
    //    {
    //        if( mSuspend )
    //            return;
    //        IPlacement placement = host.Placement;
    //        if(placement==null)
    //            return;

    //        float elapse = (float)elapsedMillisecond/1000.0F;
    //        if(elapse==0)
    //            return;

    //        SlimDX.Vector3 startTest = host.Placement.GetLocation();
    //        SlimDX.Vector3 location = startTest;
    //        if( mVelocity.Y!=0.0F )
    //        {
    //            if( host.Placement != null )
    //            {
    //                if (mVelocity.X != 0 || mVelocity.Z != 0)
    //                {
    //                    SlimDX.Vector3 VelocityXZ = new SlimDX.Vector3(mVelocity.X, 0, mVelocity.Z);
    //                    float velocityXZ = VelocityXZ.Length();
    //                    VelocityXZ.Normalize();
    //                    float moveXZ = velocityXZ * elapse;
    //                    //host.Placement.Move(ref VelocityXZ, moveXZ);
    //                    location += VelocityXZ * moveXZ;
    //                }
    //            }
    //        }
		
    //        float gravity=mGravity;
    //        if (gravity == 0)
    //            gravity = mWorldGravity;
    //        float FallDistance = mVelocity.Y*elapse+0.5f*gravity*elapse*elapse;
    //        mVelocity.Y = mVelocity.Y + gravity*elapse;
    //        //location.Y -= (halfHeight+mEpsilon);

    //        startTest.Y += 2.0F;
    //        SlimDX.Vector3 endTest = new SlimDX.Vector3(location.X, location.Y + FallDistance - 0.3F, location.Z);
    //        IHitResult result;
    //        //result.mHitFlags |= (DWORD)enHitFlag::HitMeshTriangle;
    //        if (host.WorldLineCheck(ref startTest, ref endTest, out result))
    //        {//已经落到碰撞体上了
    //            location.Y = result.mHitPosition.Y+mEpsilon;
    //            mVelocity.Y = 0.0f;
    //            mVelocity.X = 0.0f;
    //            mVelocity.Z = 0.0f;
    //            mIsFloating = false;
    //        }
    //        else
    //        {
    //            //location.Y = location.Y + FallDistance;
    //            location.Y = location.Y + FallDistance;
                
    //            mIsFloating = true;
    //        }
            
    //        //下面是临时代码测试用
    //        //{
    //        //    var prevLoc = placement.GetLocation();
    //        //    if (host.WorldLineCheck(ref prevLoc, ref location, out result))
    //        //    {
    //        //        System.Diagnostics.Debugger.Break();
    //        //    }
    //        //}
    //        var prevPos = placement.GetLocation();
    //        var moveDir = location - prevPos;
    //        if (moveDir.Y * gravity <= 0)
    //            return;

    //        moveDir.Normalize();
    //        float dist = SlimDX.Vector3.Distance(location, prevPos);
    //        placement.Move(ref moveDir,dist*0.95F);
    //        //placement.SetLocation(ref location);
    //    }
    //}
}
