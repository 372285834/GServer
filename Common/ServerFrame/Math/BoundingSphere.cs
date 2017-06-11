using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.BoundingSphereConverter) )]
	public struct BoundingSphere : System.IEquatable<BoundingSphere>
	{
        public Vector3 Center;
		public float Radius;

        public BoundingSphere( Vector3 center, float radius )
	    {
		    Center = center;
		    Radius = radius;
	    }

        public static ContainmentType Contains(BoundingSphere sphere, BoundingBox box)
	    {
		    Vector3 vector;

		    if( !BoundingBox.Intersects( box, sphere ) )
			    return ContainmentType.Disjoint;

		    float radius = sphere.Radius * sphere.Radius;
		    vector.X = sphere.Center.X - box.Minimum.X;
		    vector.Y = sphere.Center.Y - box.Maximum.Y;
		    vector.Z = sphere.Center.Z - box.Maximum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Maximum.X;
		    vector.Y = sphere.Center.Y - box.Maximum.Y;
		    vector.Z = sphere.Center.Z - box.Maximum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Maximum.X;
		    vector.Y = sphere.Center.Y - box.Minimum.Y;
		    vector.Z = sphere.Center.Z - box.Maximum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Minimum.X;
		    vector.Y = sphere.Center.Y - box.Minimum.Y;
		    vector.Z = sphere.Center.Z - box.Maximum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Minimum.X;
		    vector.Y = sphere.Center.Y - box.Maximum.Y;
		    vector.Z = sphere.Center.Z - box.Minimum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Maximum.X;
		    vector.Y = sphere.Center.Y - box.Maximum.Y;
		    vector.Z = sphere.Center.Z - box.Minimum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Maximum.X;
		    vector.Y = sphere.Center.Y - box.Minimum.Y;
		    vector.Z = sphere.Center.Z - box.Minimum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    vector.X = sphere.Center.X - box.Minimum.X;
		    vector.Y = sphere.Center.Y - box.Minimum.Y;
		    vector.Z = sphere.Center.Z - box.Minimum.Z;

		    if( vector.LengthSquared() > radius )
			    return ContainmentType.Intersects;

		    return ContainmentType.Contains;
	    }

        public static ContainmentType Contains(BoundingSphere sphere1, BoundingSphere sphere2)
	    {
		    float distance;
		    float x = sphere1.Center.X - sphere2.Center.X;
		    float y = sphere1.Center.Y - sphere2.Center.Y;
		    float z = sphere1.Center.Z - sphere2.Center.Z;

		    distance = (float)( Math.Sqrt( (x * x) + (y * y) + (z * z) ) );
		    float radius = sphere1.Radius;
		    float radius2 = sphere2.Radius;

		    if( radius + radius2 < distance )
			    return ContainmentType.Disjoint;

		    if( radius - radius2 < distance )
			    return ContainmentType.Intersects;

		    return ContainmentType.Contains;
	    }

        public static ContainmentType Contains(BoundingSphere sphere, Vector3 vector)
	    {
		    float x = vector.X - sphere.Center.X;
		    float y = vector.Y - sphere.Center.Y;
		    float z = vector.Z - sphere.Center.Z;

		    float distance = (x * x) + (y * y) + (z * z);

		    if( distance >= (sphere.Radius * sphere.Radius) )
			    return ContainmentType.Disjoint;

		    return ContainmentType.Contains;
	    }

        public static BoundingSphere FromBox(BoundingBox box)
	    {
		    BoundingSphere sphere = new BoundingSphere();
		    Vector3.Lerp( ref box.Minimum, ref box.Maximum, 0.5f, out sphere.Center );

		    float x = box.Minimum.X - box.Maximum.X;
		    float y = box.Minimum.Y - box.Maximum.Y;
		    float z = box.Minimum.Z - box.Maximum.Z;

		    float distance = (float)( Math.Sqrt( (x * x) + (y * y) + (z * z) ) );

		    sphere.Radius = distance * 0.5f;

		    return sphere;
	    }

        public static BoundingSphere FromPoints(Vector3[] points)
	    {
            BoundingSphere sphere = new BoundingSphere();
            unsafe
            {
		        fixed(Vector3* pinnedPoints = &points[0])
                {
                    int hr = IDllImportApi.D3DXComputeBoundingSphere(pinnedPoints, (UInt32)points.Length, sizeof(float) * 3, &sphere.Center, &sphere.Radius);
		            if( hr!=0 )
			            return new BoundingSphere();
                }
            }
		    return sphere;
	    }

        public static BoundingSphere Merge(BoundingSphere sphere1, BoundingSphere sphere2)
	    {
		    BoundingSphere sphere;
		    Vector3 difference = sphere2.Center - sphere1.Center;

		    float length = difference.Length();
		    float radius = sphere1.Radius;
		    float radius2 = sphere2.Radius;

		    if( radius + radius2 >= length)
		    {
			    if( radius - radius2 >= length )
				    return sphere1;

			    if( radius2 - radius >= length )
				    return sphere2;
		    }

		    Vector3 vector = difference * ( 1.0f / length );
		    float min = Math.Min( -radius, length - radius2 );
		    float max = ( Math.Max( radius, length + radius2 ) - min ) * 0.5f;

		    sphere.Center = sphere1.Center + vector * ( max + min );
		    sphere.Radius = max;

		    return sphere;
	    }

        public static bool Intersects(BoundingSphere sphere, BoundingBox box)
	    {
		    return BoundingBox.Intersects( box, sphere );
	    }

        public static bool Intersects(BoundingSphere sphere1, BoundingSphere sphere2)
	    {
		    float distance;
		    distance = Vector3.DistanceSquared( sphere1.Center, sphere2.Center );
		    float radius = sphere1.Radius;
		    float radius2 = sphere2.Radius;

		    if( (radius * radius) + (2.0f * radius * radius2) + (radius2 * radius2) <= distance )
			    return false;

		    return true;
	    }

        //bool Intersects( BoundingSphere sphere, Ray ray, out float distance )
        //{
        //    return Ray.Intersects( ray, sphere, distance );
        //}

        public static PlaneIntersectionType Intersects(BoundingSphere sphere, Plane plane)
	    {
		    return Plane.Intersects( plane, sphere );
	    }

        public static bool operator ==(BoundingSphere left, BoundingSphere right)
	    {
		    return Equals( left, right );
	    }

        public static bool operator !=(BoundingSphere left, BoundingSphere right)
	    {
		    return !Equals( left, right );
	    }

        public override String ToString()
	    {
		    return String.Format( CultureInfo.CurrentCulture, "Center:{0} Radius:{1}", Center.ToString(), Radius.ToString(CultureInfo.CurrentCulture) );
	    }

        public override int GetHashCode()
	    {
		    return Center.GetHashCode() + Radius.GetHashCode();
	    }

        public override bool Equals(Object value)
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (BoundingSphere)( value ) );
	    }

        public bool Equals(BoundingSphere value)
	    {
		    return ( Center == value.Center && Radius == value.Radius );
	    }

        public static bool Equals(ref BoundingSphere value1, ref BoundingSphere value2)
	    {
		    return ( value1.Center == value2.Center && value1.Radius == value2.Radius );
	    }
    }
}
