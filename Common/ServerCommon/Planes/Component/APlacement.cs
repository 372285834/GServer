using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public abstract class APlacement : AComponent
    {
        public delegate void Delegate_OnLocationChanged(ref SlimDX.Vector3 loc);
        public event Delegate_OnLocationChanged OnLocationChanged;

        public APlacement() { }
        public void Cleanup() { }

        public abstract AActorBase GetActor();

        public abstract bool GetLocation(out SlimDX.Vector3 loc);

        public abstract SlimDX.Vector3 GetLocation();
        public abstract float GetDirection();

        public abstract bool SetLocation(ref SlimDX.Vector3 loc);
        public abstract void SetDirection(float angle);
       

        public void _OnPlacementChanged(ref SlimDX.Vector3 loc)
        {
            if (OnLocationChanged != null)
                OnLocationChanged(ref loc);
        }

        public virtual bool Move(ref SlimDX.Vector3 pos) { return true; }

        //public virtual bool Move(ref SlimDX.Vector3 dir, float delta) { return true; }

        //public virtual bool AutoMove(ref SlimDX.Vector3 dir, float delta) { return true; }

    }

    public class IStandPlacement : APlacement
    {
        protected AActorBase mHostActor;
        public SlimDX.Vector3 mLocation;
        public float mDirection;

        public IStandPlacement(AActorBase host)
        {
            mHostActor = host;
            mLocation = SlimDX.Vector3.Zero;
            mDirection = 0;
        }

        public SlimDX.Vector3 Location
        {
            get { return mLocation; }
        }

        public float Direction
        {
            get { return mDirection; }
        }

        public override AActorBase GetActor()
        {
            return mHostActor;
        }

        public override bool GetLocation(out SlimDX.Vector3 loc)
        {
            loc = mLocation;
            return true;
        }

        public override float GetDirection()
        {
            return mDirection;
        }

        public override SlimDX.Vector3 GetLocation()
        {
            return mLocation;
        }

        public override bool SetLocation(ref SlimDX.Vector3 loc)
        {
            mLocation = loc;

            _OnPlacementChanged(ref loc);

            return true;
        }
        public override void SetDirection(float angle)
        {
            OnUpdateRoationY(angle);
        }
               
        protected virtual void OnUpdateRoationY(float angle)
        {
            mDirection = angle;
        }
        
    };
    
}
