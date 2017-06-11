using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.PlaneConverter) )]
	public struct Plane : System.IEquatable<Plane>
    {
        public Vector3 Normal;
        public float D;

        #region Equal Override
        public override string ToString()
	    {
		    return string.Format( CultureInfo.CurrentCulture, "Normal:{0} D:{1}", Normal.ToString(), D.ToString(CultureInfo.CurrentCulture) );
	    }

	    public override int GetHashCode()
	    {
		    return Normal.GetHashCode() + D.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Plane)( value ) );
	    }

	    public bool Equals( Plane value )
	    {
		    return ( Normal == value.Normal && D == value.D );
	    }

	    public static bool Equals( ref Plane value1, ref Plane value2 )
	    {
		    return ( value1.Normal == value2.Normal && value1.D == value2.D );
	    }
        #endregion

        public Plane(Plane value)
        {
            Normal = value.Normal;
            D = value.D;
        }

        public Plane( float a, float b, float c, float d )
	    {
		    Normal = new Vector3( a, b, c );
		    D = d;
	    }
	
	    public Plane( Vector3 normal, float d )
	    {
		    Normal = normal;
		    D = d;
	    }

	    public Plane( Vector3 point, Vector3 normal )
	    {
		    Normal = normal;
		    D = -Vector3.Dot( normal, point );
	    }
	
	    public Plane( Vector3 point1, Vector3 point2, Vector3 point3 )
	    {
		    float x1 = point2.X - point1.X;
		    float y1 = point2.Y - point1.Y;
		    float z1 = point2.Z - point1.Z;
		    float x2 = point3.X - point1.X;
		    float y2 = point3.Y - point1.Y;
		    float z2 = point3.Z - point1.Z;
		    float yz = (y1 * z2) - (z1 * y2);
		    float xz = (z1 * x2) - (x1 * z2);
		    float xy = (x1 * y2) - (y1 * x2);
		    float invPyth = 1.0f / (float)( Math.Sqrt((yz * yz) + (xz * xz) + (xy * xy)) );

		    Normal.X = yz * invPyth;
		    Normal.Y = xz * invPyth;
		    Normal.Z = xy * invPyth;
		    D = -((Normal.X * point1.X) + (Normal.Y * point1.Y) + (Normal.Z * point1.Z));
	    }
	
	    public Plane( Vector4 value )
	    {
		    Normal = new Vector3( value.X, value.Y, value.Z );
		    D = value.W;
	    }
	
	    public static float Dot( Plane plane, Vector4 point )
	    {
		    return (plane.Normal.X * point.X) + (plane.Normal.Y * point.Y) + (plane.Normal.Z * point.Z) + (plane.D * point.W);
	    }
	
	    public static float DotCoordinate( Plane plane, Vector3 point )
	    {
		    return (plane.Normal.X * point.X) + (plane.Normal.Y * point.Y) + (plane.Normal.Z * point.Z) + plane.D;
	    }
	
	    public static float DotNormal( Plane plane, Vector3 point )
	    {
		    return (plane.Normal.X * point.X) + (plane.Normal.Y * point.Y) + (plane.Normal.Z * point.Z);
	    }
	
	    public void Normalize()
	    {
		    float magnitude = 1.0f / (float)( Math.Sqrt( (Normal.X * Normal.X) + (Normal.Y * Normal.Y) + (Normal.Z * Normal.Z) ) );

		    Normal.X *= magnitude;
		    Normal.Y *= magnitude;
		    Normal.Z *= magnitude;
		    D *= magnitude;
	    }

	    public static Plane Normalize( Plane plane )
	    {
		    float magnitude = 1.0f / (float)( Math.Sqrt( (plane.Normal.X * plane.Normal.X) + (plane.Normal.Y * plane.Normal.Y) + (plane.Normal.Z * plane.Normal.Z) ) );

		    return new Plane( plane.Normal.X * magnitude, plane.Normal.Y * magnitude, plane.Normal.Z * magnitude, plane.D * magnitude );
	    }

	    public static void Normalize( ref Plane plane, out Plane result )
	    {
		    float magnitude = 1.0f / (float)( Math.Sqrt( (plane.Normal.X * plane.Normal.X) + (plane.Normal.Y * plane.Normal.Y) + (plane.Normal.Z * plane.Normal.Z) ) );

		    result = new Plane( plane.Normal.X * magnitude, plane.Normal.Y * magnitude, plane.Normal.Z * magnitude, plane.D * magnitude );
	    }
	
	    public static Plane Transform( Plane plane, Matrix transformation )
	    {
		    Plane result;
		    float x = plane.Normal.X;
		    float y = plane.Normal.Y;
		    float z = plane.Normal.Z;
		    float d = plane.D;

		    transformation.Invert();
		    result.Normal.X = (((x * transformation.M11) + (y * transformation.M12)) + (z * transformation.M13)) + (d * transformation.M14);
		    result.Normal.Y = (((x * transformation.M21) + (y * transformation.M22)) + (z * transformation.M23)) + (d * transformation.M24);
		    result.Normal.Z = (((x * transformation.M31) + (y * transformation.M32)) + (z * transformation.M33)) + (d * transformation.M34);
		    result.D		= (((x * transformation.M41) + (y * transformation.M42)) + (z * transformation.M43)) + (d * transformation.M44);

		    return result;
	    }

	    public static void Transform( ref Plane plane, ref Matrix temp, out Plane result )
	    {
		    float x = plane.Normal.X;
		    float y = plane.Normal.Y;
		    float z = plane.Normal.Z;
		    float d = plane.D;

		    Matrix transformation = Matrix.Invert( ref temp );

		    Plane r;
		    r.Normal.X = (((x * transformation.M11) + (y * transformation.M12)) + (z * transformation.M13)) + (d * transformation.M14);
		    r.Normal.Y = (((x * transformation.M21) + (y * transformation.M22)) + (z * transformation.M23)) + (d * transformation.M24);
		    r.Normal.Z = (((x * transformation.M31) + (y * transformation.M32)) + (z * transformation.M33)) + (d * transformation.M34);
		    r.D		= (((x * transformation.M41) + (y * transformation.M42)) + (z * transformation.M43)) + (d * transformation.M44);
	
		    result = r;
	    }

	    public static Plane[] Transform( Plane[] planes, ref Matrix temp )
	    {
		    if( planes == null )
			    throw new ArgumentNullException( "planes" );

		    int count = planes.Length;
		    Plane[] results = new Plane[ count ];
		    Matrix transformation = Matrix.Invert( ref temp );

		    for( int i = 0; i < count; i++ )
		    {
			    float x = planes[i].Normal.X;
			    float y = planes[i].Normal.Y;
			    float z = planes[i].Normal.Z;
			    float d = planes[i].D;

			    Plane r;
			    r.Normal.X = (((x * transformation.M11) + (y * transformation.M12)) + (z * transformation.M13)) + (d * transformation.M14);
			    r.Normal.Y = (((x * transformation.M21) + (y * transformation.M22)) + (z * transformation.M23)) + (d * transformation.M24);
			    r.Normal.Z = (((x * transformation.M31) + (y * transformation.M32)) + (z * transformation.M33)) + (d * transformation.M34);
			    r.D		= (((x * transformation.M41) + (y * transformation.M42)) + (z * transformation.M43)) + (d * transformation.M44);
		
			    results[i] = r;
		    }

		    return results;
	    }

	    public static Plane Transform( Plane plane, Quaternion rotation )
	    {
		    Plane result;
		    float x2 = rotation.X + rotation.X;
		    float y2 = rotation.Y + rotation.Y;
		    float z2 = rotation.Z + rotation.Z;
		    float wx = rotation.W * x2;
		    float wy = rotation.W * y2;
		    float wz = rotation.W * z2;
		    float xx = rotation.X * x2;
		    float xy = rotation.X * y2;
		    float xz = rotation.X * z2;
		    float yy = rotation.Y * y2;
		    float yz = rotation.Y * z2;
		    float zz = rotation.Z * z2;

		    float x = plane.Normal.X;
		    float y = plane.Normal.Y;
		    float z = plane.Normal.Z;

		    result.Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
		    result.Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
		    result.Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
		    result.D = plane.D;
		    return result;
	    }
	
	    public static void Transform( ref Plane plane, ref Quaternion rotation, out Plane result )
	    {
		    float x2 = rotation.X + rotation.X;
		    float y2 = rotation.Y + rotation.Y;
		    float z2 = rotation.Z + rotation.Z;
		    float wx = rotation.W * x2;
		    float wy = rotation.W * y2;
		    float wz = rotation.W * z2;
		    float xx = rotation.X * x2;
		    float xy = rotation.X * y2;
		    float xz = rotation.X * z2;
		    float yy = rotation.Y * y2;
		    float yz = rotation.Y * z2;
		    float zz = rotation.Z * z2;

		    float x = plane.Normal.X;
		    float y = plane.Normal.Y;
		    float z = plane.Normal.Z;

		    Plane r = new Plane();
		    r.Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
		    r.Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
		    r.Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
		    r.D = plane.D;

		    result = r;
	    }

	    public static Plane[] Transform( Plane[] planes, ref Quaternion rotation )
	    {
		    if( planes == null )
			    throw new ArgumentNullException( "planes" );

		    int count = planes.Length;
		    Plane[] results = new Plane[ count ];

		    float x2 = rotation.X + rotation.X;
		    float y2 = rotation.Y + rotation.Y;
		    float z2 = rotation.Z + rotation.Z;
		    float wx = rotation.W * x2;
		    float wy = rotation.W * y2;
		    float wz = rotation.W * z2;
		    float xx = rotation.X * x2;
		    float xy = rotation.X * y2;
		    float xz = rotation.X * z2;
		    float yy = rotation.Y * y2;
		    float yz = rotation.Y * z2;
		    float zz = rotation.Z * z2;

		    for( int i = 0; i < count; i++ )
		    {
			    float x = planes[i].Normal.X;
			    float y = planes[i].Normal.Y;
			    float z = planes[i].Normal.Z;

			    Plane r;
			    r.Normal.X = ((x * ((1.0f - yy) - zz)) + (y * (xy - wz))) + (z * (xz + wy));
			    r.Normal.Y = ((x * (xy + wz)) + (y * ((1.0f - xx) - zz))) + (z * (yz - wx));
			    r.Normal.Z = ((x * (xz - wy)) + (y * (yz + wx))) + (z * ((1.0f - xx) - yy));
			    r.D = planes[i].D;

			    results[i] = r;
		    }

		    return results;
	    }

        public static bool Intersects(ref Plane plane, ref Vector3 start, ref Vector3 end, out Vector3 intersectPoint)
	    {
            unsafe
            {
                fixed (Plane* pPlane = &plane)
                {
                    fixed (Vector3* pStart = &start)
                    {
                        fixed (Vector3* pEnd = &end)
                        {
                            fixed (Vector3* pIntersectPoint = &intersectPoint)
                            {
                                if (0 == (int)IDllImportApi.D3DXPlaneIntersectLine(pIntersectPoint, pPlane, pStart, pEnd))
                                {
                                    return false;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
	    }

        public static PlaneIntersectionType Intersects(Plane plane, BoundingBox box)
        {
            Vector3 min;
            Vector3 max;
            max.X = (plane.Normal.X >= 0.0f) ? box.Minimum.X : box.Maximum.X;
            max.Y = (plane.Normal.Y >= 0.0f) ? box.Minimum.Y : box.Maximum.Y;
            max.Z = (plane.Normal.Z >= 0.0f) ? box.Minimum.Z : box.Maximum.Z;
            min.X = (plane.Normal.X >= 0.0f) ? box.Maximum.X : box.Minimum.X;
            min.Y = (plane.Normal.Y >= 0.0f) ? box.Maximum.Y : box.Minimum.Y;
            min.Z = (plane.Normal.Z >= 0.0f) ? box.Maximum.Z : box.Minimum.Z;

            float dot = (plane.Normal.X * max.X) + (plane.Normal.Y * max.Y) + (plane.Normal.Z * max.Z);

            if (dot + plane.D > 0.0f)
                return PlaneIntersectionType.Front;

            dot = (plane.Normal.X * min.X) + (plane.Normal.Y * min.Y) + (plane.Normal.Z * min.Z);

            if (dot + plane.D < 0.0f)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

        public static PlaneIntersectionType Intersects(Plane plane, BoundingSphere sphere)
        {
            float dot = (sphere.Center.X * plane.Normal.X) + (sphere.Center.Y * plane.Normal.Y) + (sphere.Center.Z * plane.Normal.Z) + plane.D;

            if (dot > sphere.Radius)
                return PlaneIntersectionType.Front;

            if (dot < -sphere.Radius)
                return PlaneIntersectionType.Back;

            return PlaneIntersectionType.Intersecting;
        }

	    public static Plane Multiply( Plane plane, float scale )
	    {
		    Plane result = new Plane();
            unsafe
            {
                IDllImportApi.D3DXPlaneScale((Plane*)&result, (Plane*)&plane, scale);
            }
            
		    return result;
	    }

	    public static void Multiply( ref Plane plane, float scale, out Plane result )
	    {
            unsafe
            {
                fixed (Plane* pResult = &result)
                {
                    fixed (Plane* pPlane = &plane)
                    {
                        IDllImportApi.D3DXPlaneScale(pResult, pPlane, scale);
                    }
                }
            }
	    }

	
	    public static Plane operator * ( Plane plane, float scale )
	    {
		    Plane result;
            unsafe
            {
                IDllImportApi.D3DXPlaneScale((Plane*)(&result), (Plane*)(&plane), scale);
            }
		    return result;
	    }

	    public static Plane operator * ( float scale, Plane plane )
	    {
		    return plane * scale;
	    }

	    public static bool operator == ( Plane left, Plane right )
	    {
		    return Equals( left, right );
	    }

	    public static bool operator != ( Plane left, Plane right )
	    {
		    return !Equals( left, right );
	    }
    }
}
