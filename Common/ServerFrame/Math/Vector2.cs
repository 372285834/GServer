using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4 )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.Vector2Converter) )]
	public struct Vector2 : System.IEquatable<Vector2>
    {
        public float X;
		public float Y;

        public Vector2( float value )
	    {
		    X = value;
		    Y = value;
	    }

        public Vector2(float x, float y)
	    {
		    X = x;
		    Y = y;
	    }

        public float this[int index]
        {
            get
	        {
		        switch( index )
		        {
		        case 0:
			        return X;
		        case 1:
			        return Y;
		        default:
			        throw new ArgumentOutOfRangeException( "index", "Indices for Vector2 run from 0 to 1, inclusive." );
		        }
	        }
            set
	        {
		        switch( index )
		        {
		        case 0:
			        X = value;
			        break;
		        case 1:
			        Y = value;
			        break;
		        default:
			        throw new ArgumentOutOfRangeException( "index", "Indices for Vector2 run from 0 to 1, inclusive." );
		        }
	        }
        }

        public static Vector2 Zero { get { return new Vector2(0, 0); } }
	    public static Vector2 UnitX { get { return new Vector2(1, 0); } }
        public static Vector2 UnitY { get{ return new Vector2(0, 1); } }
        public static int SizeInBytes { get{ return System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2)); } }

        #region Equal Override
        public override string ToString()
	    {
		    return String.Format( CultureInfo.CurrentCulture, "X:{0} Y:{1}", X.ToString(CultureInfo.CurrentCulture), Y.ToString(CultureInfo.CurrentCulture) );
	    }

	    public override int GetHashCode()
	    {
		    return X.GetHashCode() + Y.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Vector2)( value ) );
	    }

	    public bool Equals( Vector2 value )
	    {
		    return ( X == value.X && Y == value.Y );
	    }

	    public static bool Equals( ref Vector2 value1, ref Vector2 value2 )
	    {
		    return ( value1.X == value2.X && value1.Y == value2.Y );
	    }
        #endregion

        public float Length()
	    {
		    return (float)( Math.Sqrt( (X * X) + (Y * Y) ) );
	    }
	
	    public float LengthSquared()
	    {
		    return (X * X) + (Y * Y);
	    }
	
	    public void Normalize()
	    {
		    float length = Length();
		    if( length == 0 )
			    return;
		    float num = 1 / length;
		    X *= num;
		    Y *= num;
	    }

	    public static Vector2 Add( Vector2 left, Vector2 right )
	    {
		    return new Vector2( left.X + right.X, left.Y + right.Y );
	    }
	
	    public static void Add( ref Vector2 left, ref Vector2 right, out Vector2 result )
	    {
            result = new Vector2(left.X + right.X, left.Y + right.Y);
	    }
	
	    public static Vector2 Subtract( Vector2 left, Vector2 right )
	    {
            return new Vector2(left.X - right.X, left.Y - right.Y);
	    }
	
	    public static void Subtract( ref Vector2 left, ref Vector2 right, out Vector2 result )
	    {
            result = new Vector2(left.X - right.X, left.Y - right.Y);
	    }
	
	    public static Vector2 Modulate( Vector2 left, Vector2 right )
	    {
            return new Vector2(left.X * right.X, left.Y * right.Y);
	    }
	
	    public static void Modulate( ref Vector2 left, ref Vector2 right, out Vector2 result )
	    {
            result = new Vector2(left.X * right.X, left.Y * right.Y);
	    }
	
	    public static Vector2 Multiply( Vector2 value, float scale )
	    {
            return new Vector2(value.X * scale, value.Y * scale);
	    }
	
	    public static void Multiply( ref Vector2 value, float scale, out Vector2 result )
	    {
            result = new Vector2(value.X * scale, value.Y * scale);
	    }
	
	    public static Vector2 Divide( Vector2 value, float scale )
	    {
            return new Vector2(value.X / scale, value.Y / scale);
	    }

	    public static void Divide( ref Vector2 value, float scale, out Vector2 result )
	    {
            result = new Vector2(value.X / scale, value.Y / scale);
	    }

	    public static Vector2 Negate( Vector2 value )
	    {
            return new Vector2(-value.X, -value.Y);
	    }
	
	    public static void Negate( ref Vector2 value, out Vector2 result )
	    {
            result = new Vector2(-value.X, -value.Y);
	    }
	
	    public static Vector2 Barycentric( Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2 )
	    {
		    Vector2 vector = new Vector2();
		    vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
		    vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
		    return vector;
	    }
	
	    public static void Barycentric( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result )
	    {
		    result = new Vector2((value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X)),
			    (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y)) );
	    }
	
	    public static Vector2 CatmullRom( Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount )
	    {
            Vector2 vector = new Vector2();
		    float squared = amount * amount;
		    float cubed = amount * squared;

		    vector.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) + 
			    (((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) + 
			    ((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));

		    vector.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + 
			    (((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) + 
			    ((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));

		    return vector;
	    }
	
	    public static void CatmullRom( ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result )
	    {
		    float squared = amount * amount;
		    float cubed = amount * squared;
            Vector2 r = new Vector2();

		    r.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) + 
			    (((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) + 
			    ((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));

		    r.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + 
			    (((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) + 
			    ((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));

		    result = r;
	    }
	
	    public static Vector2 Clamp( Vector2 value, Vector2 min, Vector2 max )
	    {
		    float x = value.X;
		    x = (x > max.X) ? max.X : x;
		    x = (x < min.X) ? min.X : x;

		    float y = value.Y;
		    y = (y > max.Y) ? max.Y : y;
		    y = (y < min.Y) ? min.Y : y;

		    return new Vector2( x, y );
	    }
	
	    public static void Clamp( ref Vector2 value, ref Vector2 min, ref Vector2 max, out Vector2 result )
	    {
		    float x = value.X;
		    x = (x > max.X) ? max.X : x;
		    x = (x < min.X) ? min.X : x;

		    float y = value.Y;
		    y = (y > max.Y) ? max.Y : y;
		    y = (y < min.Y) ? min.Y : y;

		    result = new Vector2( x, y );
	    }
	
	    public static Vector2 Hermite( Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount )
	    {
            Vector2 vector = new Vector2();
		    float squared = amount * amount;
		    float cubed = amount * squared;
		    float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
		    float part2 = (-2.0f * cubed) + (3.0f * squared);
		    float part3 = (cubed - (2.0f * squared)) + amount;
		    float part4 = cubed - squared;

		    vector.X = (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4);
		    vector.Y = (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4);

		    return vector;
	    }	
	
	    public static void Hermite( ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result )
	    {
		    float squared = amount * amount;
		    float cubed = amount * squared;
		    float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
		    float part2 = (-2.0f * cubed) + (3.0f * squared);
		    float part3 = (cubed - (2.0f * squared)) + amount;
		    float part4 = cubed - squared;

            Vector2 r = new Vector2();
		    r.X = (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4);
		    r.Y = (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4);

		    result = r;
	    }
	
	    public static Vector2 Lerp( Vector2 start, Vector2 end, float factor )
	    {
            Vector2 vector = new Vector2();

		    vector.X = start.X + ((end.X - start.X) * factor);
		    vector.Y = start.Y + ((end.Y - start.Y) * factor);

		    return vector;
	    }
	
	    public static void Lerp( ref Vector2 start, ref Vector2 end, float factor, out Vector2 result )
	    {
		    Vector2 r;
		    r.X = start.X + ((end.X - start.X) * factor);
		    r.Y = start.Y + ((end.Y - start.Y) * factor);

		    result = r;
	    }
	
	    public static Vector2 SmoothStep( Vector2 start, Vector2 end, float amount )
	    {
            Vector2 vector = new Vector2();

		    amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
		    amount = (amount * amount) * (3.0f - (2.0f * amount));

		    vector.X = start.X + ((end.X - start.X) * amount);
		    vector.Y = start.Y + ((end.Y - start.Y) * amount);

		    return vector;
	    }
	
	    public static void SmoothStep( ref Vector2 start, ref Vector2 end, float amount, out Vector2 result )
	    {
		    amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
		    amount = (amount * amount) * (3.0f - (2.0f * amount));

            Vector2 r = new Vector2();
		    r.X = start.X + ((end.X - start.X) * amount);
		    r.Y = start.Y + ((end.Y - start.Y) * amount);

		    result = r;
	    }
	
	    public static float Distance( Vector2 value1, Vector2 value2 )
	    {
		    float x = value1.X - value2.X;
		    float y = value1.Y - value2.Y;

		    return (float)( Math.Sqrt( (x * x) + (y * y) ) );
	    }

	    public static float DistanceSquared( Vector2 value1, Vector2 value2 )
	    {
		    float x = value1.X - value2.X;
		    float y = value1.Y - value2.Y;

		    return (x * x) + (y * y);
	    }
	
	    public static float Dot( Vector2 left, Vector2 right )
	    {
		    return (left.X * right.X + left.Y * right.Y);
	    }
	
	    public static Vector2 Normalize( Vector2 vector )
	    {
		    vector.Normalize();
		    return vector;
	    }
	
	    public static void Normalize( ref Vector2 vector, out Vector2 result )
	    {
		    result = Normalize(vector);
	    }
	
	    public static Vector4 Transform( Vector2 vector, Matrix transform )
	    {
            Vector4 result = new Vector4();

		    result.X = (vector.X * transform.M11) + (vector.Y * transform.M21) + transform.M41;
		    result.Y = (vector.X * transform.M12) + (vector.Y * transform.M22) + transform.M42;
		    result.Z = (vector.X * transform.M13) + (vector.Y * transform.M23) + transform.M43;
		    result.W = (vector.X * transform.M14) + (vector.Y * transform.M24) + transform.M44;

		    return result;
	    }
	
	    public static void Transform( ref Vector2 vector, ref Matrix transform, out Vector4 result )
	    {
            Vector4 r = new Vector4();
		    r.X = (vector.X * transform.M11) + (vector.Y * transform.M21) + transform.M41;
		    r.Y = (vector.X * transform.M12) + (vector.Y * transform.M22) + transform.M42;
		    r.Z = (vector.X * transform.M13) + (vector.Y * transform.M23) + transform.M43;
		    r.W = (vector.X * transform.M14) + (vector.Y * transform.M24) + transform.M44;

		    result = r;
	    }
	
	    public static Vector4[] Transform( Vector2[] vectors, ref Matrix transform )
	    {
		    if( vectors == null )
			    throw new ArgumentNullException( "vectors" );

		    int count = vectors.Length;
		    Vector4[] results = new Vector4[ count ];

		    for( int i = 0; i < count; i++ )
		    {
                Vector4 r = new Vector4();
			    r.X = (vectors[i].X * transform.M11) + (vectors[i].Y * transform.M21) + transform.M41;
			    r.Y = (vectors[i].X * transform.M12) + (vectors[i].Y * transform.M22) + transform.M42;
			    r.Z = (vectors[i].X * transform.M13) + (vectors[i].Y * transform.M23) + transform.M43;
			    r.W = (vectors[i].X * transform.M14) + (vectors[i].Y * transform.M24) + transform.M44;

			    results[i] = r;
		    }

		    return results;
	    }
	
	    public static Vector4 Transform( Vector2 value, Quaternion rotation )
	    {
            Vector4 vector = new Vector4();
		    float x = rotation.X + rotation.X;
		    float y = rotation.Y + rotation.Y;
		    float z = rotation.Z + rotation.Z;
		    float wx = rotation.W * x;
		    float wy = rotation.W * y;
		    float wz = rotation.W * z;
		    float xx = rotation.X * x;
		    float xy = rotation.X * y;
		    float xz = rotation.X * z;
		    float yy = rotation.Y * y;
		    float yz = rotation.Y * z;
		    float zz = rotation.Z * z;

		    vector.X = ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz)));
		    vector.Y = ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz)));
		    vector.Z = ((value.X * (xz - wy)) + (value.Y * (yz + wx)));
		    vector.W = 1.0f;

		    return vector;
	    }
	
	    public static void Transform( ref Vector2 value, ref Quaternion rotation, out Vector4 result )
	    {
		    float x = rotation.X + rotation.X;
		    float y = rotation.Y + rotation.Y;
		    float z = rotation.Z + rotation.Z;
		    float wx = rotation.W * x;
		    float wy = rotation.W * y;
		    float wz = rotation.W * z;
		    float xx = rotation.X * x;
		    float xy = rotation.X * y;
		    float xz = rotation.X * z;
		    float yy = rotation.Y * y;
		    float yz = rotation.Y * z;
		    float zz = rotation.Z * z;

            Vector4 r = new Vector4();
		    r.X = ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz)));
		    r.Y = ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz)));
		    r.Z = ((value.X * (xz - wy)) + (value.Y * (yz + wx)));
		    r.W = 1.0f;

		    result = r;
	    }

	    public static Vector4[] Transform( Vector2[] vectors, ref Quaternion rotation )
	    {
		    if( vectors == null )
			    throw new ArgumentNullException( "vectors" );

		    int count = vectors.Length;
		    Vector4[] results = new Vector4[ count ];

		    float x = rotation.X + rotation.X;
		    float y = rotation.Y + rotation.Y;
		    float z = rotation.Z + rotation.Z;
		    float wx = rotation.W * x;
		    float wy = rotation.W * y;
		    float wz = rotation.W * z;
		    float xx = rotation.X * x;
		    float xy = rotation.X * y;
		    float xz = rotation.X * z;
		    float yy = rotation.Y * y;
		    float yz = rotation.Y * z;
		    float zz = rotation.Z * z;

		    for( int i = 0; i < count; i++ )
		    {
                Vector4 r = new Vector4();
			    r.X = ((vectors[i].X * ((1.0f - yy) - zz)) + (vectors[i].Y * (xy - wz)));
			    r.Y = ((vectors[i].X * (xy + wz)) + (vectors[i].Y * ((1.0f - xx) - zz)));
			    r.Z = ((vectors[i].X * (xz - wy)) + (vectors[i].Y * (yz + wx)));
			    r.W = 1.0f;

			    results[i] = r;
		    }

		    return results;
	    }
	
	    public static Vector2 TransformCoordinate( Vector2 coord, Matrix transform )
	    {
            Vector4 vector = new Vector4();

		    vector.X = (coord.X * transform.M11) + (coord.Y * transform.M21) + transform.M41;
		    vector.Y = (coord.X * transform.M12) + (coord.Y * transform.M22) + transform.M42;
		    vector.Z = (coord.X * transform.M13) + (coord.Y * transform.M23) + transform.M43;
		    vector.W = 1 / ((coord.X * transform.M14) + (coord.Y * transform.M24) + transform.M44);

		    return new Vector2( vector.X * vector.W, vector.Y * vector.W );
	    }
	
	    public static void TransformCoordinate( ref Vector2 coord, ref Matrix transform, out Vector2 result )
	    {
            Vector4 vector = new Vector4();

		    vector.X = (coord.X * transform.M11) + (coord.Y * transform.M21) + transform.M41;
		    vector.Y = (coord.X * transform.M12) + (coord.Y * transform.M22) + transform.M42;
		    vector.Z = (coord.X * transform.M13) + (coord.Y * transform.M23) + transform.M43;
		    vector.W = 1 / ((coord.X * transform.M14) + (coord.Y * transform.M24) + transform.M44);

		    result = new Vector2( vector.X * vector.W, vector.Y * vector.W );
	    }
	
	    public static Vector2[] TransformCoordinate( Vector2[] coords, ref Matrix transform )
	    {
		    if( coords == null )
			    throw new ArgumentNullException( "coordinates" );

            Vector4 vector = new Vector4();
		    int count = coords.Length;
		    Vector2[] results = new Vector2[ count ];

		    for( int i = 0; i < count; i++ )
		    {
			    vector.X = (coords[i].X * transform.M11) + (coords[i].Y * transform.M21) + transform.M41;
			    vector.Y = (coords[i].X * transform.M12) + (coords[i].Y * transform.M22) + transform.M42;
			    vector.Z = (coords[i].X * transform.M13) + (coords[i].Y * transform.M23) + transform.M43;
			    vector.W = 1 / ((coords[i].X * transform.M14) + (coords[i].Y * transform.M24) + transform.M44);
			    results[i] = new Vector2( vector.X * vector.W, vector.Y * vector.W );
		    }

		    return results;
	    }

	    public static Vector2 TransformNormal( Vector2 normal, Matrix transform )
	    {
            Vector2 vector = new Vector2();

		    vector.X = (normal.X * transform.M11) + (normal.Y * transform.M21);
		    vector.Y = (normal.X * transform.M12) + (normal.Y * transform.M22);

		    return vector;
	    }
	
	    public static void TransformNormal( ref Vector2 normal, ref Matrix transform, out Vector2 result )
	    {
            Vector2 r = new Vector2();
		    r.X = (normal.X * transform.M11) + (normal.Y * transform.M21);
		    r.Y = (normal.X * transform.M12) + (normal.Y * transform.M22);

		    result = r;
	    }
	
	    public static Vector2[] TransformNormal( Vector2[] normals, ref Matrix transform )
	    {
		    if( normals == null )
			    throw new ArgumentNullException( "normals" );

		    int count = normals.Length;
		    Vector2[] results = new Vector2[ count ];

		    for( int i = 0; i < count; i++ )
		    {
                Vector2 r = new Vector2();
			    r.X = (normals[i].X * transform.M11) + (normals[i].Y * transform.M21);
			    r.Y = (normals[i].X * transform.M12) + (normals[i].Y * transform.M22);

			    results[i] = r;
		    }

		    return results;
	    }
	
	    public static Vector2 Minimize( Vector2 left, Vector2 right )
	    {
            Vector2 vector = new Vector2();
		    vector.X = (left.X < right.X) ? left.X : right.X;
		    vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
		    return vector;
	    }
	
	    public static void Minimize( ref Vector2 left, ref Vector2 right, out Vector2 result )
	    {
            Vector2 r = new Vector2();
		    r.X = (left.X < right.X) ? left.X : right.X;
		    r.Y = (left.Y < right.Y) ? left.Y : right.Y;

		    result = r;
	    }
	
	    public static Vector2 Maximize( Vector2 left, Vector2 right )
	    {
            Vector2 vector = new Vector2();
		    vector.X = (left.X > right.X) ? left.X : right.X;
		    vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
		    return vector;
	    }

	    public static void Maximize( ref Vector2 left, ref Vector2 right, out Vector2 result )
	    {
            Vector2 r = new Vector2();
		    r.X = (left.X > right.X) ? left.X : right.X;
		    r.Y = (left.Y > right.Y) ? left.Y : right.Y;

		    result = r;
	    }
	
	    public static Vector2 operator + ( Vector2 left, Vector2 right )
	    {
		    return new Vector2( left.X + right.X, left.Y + right.Y );
	    }

	    public static Vector2 operator - ( Vector2 left, Vector2 right )
	    {
		    return new Vector2( left.X - right.X, left.Y - right.Y );
	    }
	
	    public static Vector2 operator - ( Vector2 value )
	    {
            return new Vector2(-value.X, -value.Y);
	    }
	
	    public static Vector2 operator * ( Vector2 value, float scale )
	    {
            return new Vector2(value.X * scale, value.Y * scale);
	    }
	
	    public static Vector2 operator * ( float scale, Vector2 vec )
	    {
		    return vec * scale;
	    }
	
	    public static Vector2 operator / ( Vector2 value, float scale )
	    {
            return new Vector2(value.X / scale, value.Y / scale);
	    }
	
	    public static bool operator == ( Vector2 left, Vector2 right )
	    {
		    return Equals( left, right );
	    }
	
	    public static bool operator != ( Vector2 left, Vector2 right )
	    {
		    return !Equals( left, right );
	    }
    }
}
