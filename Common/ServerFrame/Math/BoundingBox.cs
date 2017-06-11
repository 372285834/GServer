using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.BoundingBoxConverter) )]
	public struct BoundingBox : System.IEquatable<BoundingBox>
    {
        public Vector3 Maximum;
        public Vector3 Minimum;

        public BoundingBox( Vector3 minimum, Vector3 maximum )
	    {
		    Minimum = minimum;
		    Maximum = maximum;
	    }

        public unsafe void InitEmptyBox(float* minimum, float* maximum)
        {
            unsafe
            {
                Minimum.X = minimum[0];
                Minimum.Y = minimum[1];
                Minimum.Z = minimum[2];

                Maximum.X = maximum[0];
                Maximum.Y = maximum[1];
                Maximum.Z = maximum[2];
            }
        }

        public void InitEmptyBox()
        {
            Minimum = new Vector3(1, 1, 1);
            Maximum = new Vector3(-1, -1, -1);
        }

        public Vector3[] GetCorners()
	    {
		    Vector3[] results = new Vector3[8];
		    results[0] = new Vector3(Minimum.X, Maximum.Y, Maximum.Z);
		    results[1] = new Vector3(Maximum.X, Maximum.Y, Maximum.Z);
		    results[2] = new Vector3(Maximum.X, Minimum.Y, Maximum.Z);
		    results[3] = new Vector3(Minimum.X, Minimum.Y, Maximum.Z);
		    results[4] = new Vector3(Minimum.X, Maximum.Y, Minimum.Z);
		    results[5] = new Vector3(Maximum.X, Maximum.Y, Minimum.Z);
		    results[6] = new Vector3(Maximum.X, Minimum.Y, Minimum.Z);
		    results[7] = new Vector3(Minimum.X, Minimum.Y, Minimum.Z);
		    return results;
	    }

        public Vector3 GetCenter()
	    {
		    return (Maximum + Minimum) * 0.5f;
	    }

        public static ContainmentType Contains(BoundingBox box1, BoundingBox box2)
	    {
		    if( box1.Maximum.X < box2.Minimum.X || box1.Minimum.X > box2.Maximum.X )
			    return ContainmentType.Disjoint;

		    if( box1.Maximum.Y < box2.Minimum.Y || box1.Minimum.Y > box2.Maximum.Y )
			    return ContainmentType.Disjoint;

		    if( box1.Maximum.Z < box2.Minimum.Z || box1.Minimum.Z > box2.Maximum.Z )
			    return ContainmentType.Disjoint;

		    if( box1.Minimum.X <= box2.Minimum.X && box2.Maximum.X <= box1.Maximum.X && box1.Minimum.Y <= box2.Minimum.Y && 
			    box2.Maximum.Y <= box1.Maximum.Y && box1.Minimum.Z <= box2.Minimum.Z && box2.Maximum.Z <= box1.Maximum.Z )
			    return ContainmentType.Contains;

		    return ContainmentType.Intersects;
	    }

        //ContainmentType Contains( BoundingBox box, BoundingSphere sphere )
        //{
        //    float dist;
        //    Vector3 clamped;

        //    Vector3.Clamp( sphere.Center, box.Minimum, box.Maximum, clamped );

        //    float x = sphere.Center.X - clamped.X;
        //    float y = sphere.Center.Y - clamped.Y;
        //    float z = sphere.Center.Z - clamped.Z;

        //    dist = (x * x) + (y * y) + (z * z);
        //    float radius = sphere.Radius;

        //    if( dist > (radius * radius) )
        //        return ContainmentType.Disjoint;

        //    if( box.Minimum.X + radius <= sphere.Center.X && sphere.Center.X <= box.Maximum.X - radius && 
        //        box.Maximum.X - box.Minimum.X > radius && box.Minimum.Y + radius <= sphere.Center.Y && 
        //        sphere.Center.Y <= box.Maximum.Y - radius && box.Maximum.Y - box.Minimum.Y > radius && 
        //        box.Minimum.Z + radius <= sphere.Center.Z && sphere.Center.Z <= box.Maximum.Z - radius &&
        //        box.Maximum.X - box.Minimum.X > radius )
        //        return ContainmentType.Contains;

        //    return ContainmentType.Intersects;
        //}

        public static ContainmentType Contains(BoundingBox box, Vector3 vector)
	    {
		    if( box.Minimum.X <= vector.X && vector.X <= box.Maximum.X && box.Minimum.Y <= vector.Y && 
			    vector.Y <= box.Maximum.Y && box.Minimum.Z <= vector.Z && vector.Z <= box.Maximum.Z )
			    return ContainmentType.Contains;

		    return ContainmentType.Disjoint;
	    }

        public static BoundingBox FromPoints(Vector3[] points)
	    {
		    if( points == null || points.Length <= 0 )
			    throw new ArgumentNullException( "points" );

		    Vector3 min = new Vector3( float.MaxValue );
		    Vector3 max = new Vector3( float.MinValue );

		    foreach( var i in points )
		    {
                Vector3 vector = i;
			    Vector3.Minimize( ref min, ref vector, out min );
			    Vector3.Maximize( ref max, ref vector, out max );
		    }

		    return new BoundingBox( min, max );
	    }

        //BoundingBox FromPoints( DataStream points, int count, int stride )
        //{
        //    BoundingBox box;

        //    HRESULT hr = D3DXComputeBoundingBox( reinterpret_cast<D3DXVECTOR3*>( points.PositionPointer ), count, stride, 
        //        reinterpret_cast<D3DXVECTOR3*>( &box.Minimum ), reinterpret_cast<D3DXVECTOR3*>( &box.Maximum ) );

        //    if( RECORD_SDX( hr ).IsFailure )
        //        return BoundingBox();

        //    return box;
        //}

        //BoundingBox FromSphere( BoundingSphere sphere )
        //{
        //    BoundingBox box;
        //    box.Minimum = new Vector3( sphere.Center.X - sphere.Radius, sphere.Center.Y - sphere.Radius, sphere.Center.Z - sphere.Radius );
        //    box.Maximum = new Vector3( sphere.Center.X + sphere.Radius, sphere.Center.Y + sphere.Radius, sphere.Center.Z + sphere.Radius );
        //    return box;
        //}

        public static BoundingBox Merge(BoundingBox box1, BoundingBox box2)
	    {
		    BoundingBox box = new BoundingBox();
		    Vector3.Minimize( ref box1.Minimum, ref box2.Minimum, out box.Minimum );
		    Vector3.Maximize( ref box1.Maximum, ref box2.Maximum, out box.Maximum );
		    return box;
	    }

        public static bool Intersects(BoundingBox box1, BoundingBox box2)
	    {
		    if ( box1.Maximum.X < box2.Minimum.X || box1.Minimum.X > box2.Maximum.X )
			    return false;

		    if ( box1.Maximum.Y < box2.Minimum.Y || box1.Minimum.Y > box2.Maximum.Y )
			    return false;

		    return ( box1.Maximum.Z >= box2.Minimum.Z && box1.Minimum.Z <= box2.Maximum.Z );
	    }

        public static bool Intersects(BoundingBox box, BoundingSphere sphere)
        {
            float dist;
            Vector3 clamped = new Vector3();

            Vector3.Clamp(ref sphere.Center, ref box.Minimum, ref box.Maximum, out clamped);

            float x = sphere.Center.X - clamped.X;
            float y = sphere.Center.Y - clamped.Y;
            float z = sphere.Center.Z - clamped.Z;

            dist = (x * x) + (y * y) + (z * z);

            return (dist <= (sphere.Radius * sphere.Radius));
        }

        //bool Intersects( BoundingBox box, Ray ray, out float distance )
        //{
        //    return Ray.Intersects( ray, box, distance );
        //}

        public static PlaneIntersectionType Intersects(BoundingBox box, Plane plane)
	    {
		    return Plane.Intersects( plane, box);
	    }

        public static bool operator ==(BoundingBox left, BoundingBox right)
	    {
		    return Equals( left, right );
	    }

        public static bool operator !=(BoundingBox left, BoundingBox right)
	    {
		    return !Equals( left, right );
	    }

        public override String ToString()
	    {
		    return String.Format( CultureInfo.CurrentCulture, "Minimum:{0} Maximum:{1}", Minimum.ToString(), Maximum.ToString() );
	    }

        public override int GetHashCode()
	    {
		    return Minimum.GetHashCode() + Maximum.GetHashCode();
	    }

        public override bool Equals(Object value)
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (BoundingBox)( value ) );
	    }

        public bool Equals(BoundingBox value)
	    {
		    return ( Minimum == value.Minimum && Maximum == value.Maximum );
	    }

        public static bool Equals(ref BoundingBox value1, ref BoundingBox value2)
	    {
		    return ( value1.Minimum == value2.Minimum && value1.Maximum == value2.Maximum );
	    }
    }
}
