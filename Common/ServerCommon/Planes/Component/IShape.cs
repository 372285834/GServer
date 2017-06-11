using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public struct IHitResult
	{
		public SlimDX.Vector3		mHitPosition;
		public SlimDX.Vector3		mHitNormal;
		public float				mHitLength;
		public UInt32				mHitFlags;

        public bool HasFlag(UInt32 flag)
		{
			return ((mHitFlags & flag) == flag);
		}
	};

    public class IShape : AComponent
    {
        public virtual bool LineCheck( ref SlimDX.Vector3 start, ref SlimDX.Vector3 length, ref SlimDX.Matrix matrix, out IHitResult result )
        {
            result = new IHitResult();
            return false; 
        }
    }

    public class IShapeCylinder : IShape
	{
	    protected float		mHalfHeight = 2.0f;
	    protected float		mRadius = 0.25f;
        public IShapeCylinder()
        {

        }

        public override bool LineCheck(ref SlimDX.Vector3 start, ref SlimDX.Vector3 length, ref SlimDX.Matrix matrix, out IHitResult result)
        {
            result = new IHitResult();
            return false;
        }

        public float HalfHeight
        {
            get { return mHalfHeight; }
            set { mHalfHeight = value; }
        }
        public float Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }
	}
}
