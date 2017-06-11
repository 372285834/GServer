using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.QuaternionConverter) )]
	public struct Quaternion : System.IEquatable<Quaternion>
	{
        #region Member
        public float X;

		public float Y;

		public float Z;

		public float W;
        #endregion

        #region Equal override
        public override string ToString()
	    {
		    return String.Format( CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(CultureInfo.CurrentCulture), 
			    Y.ToString(CultureInfo.CurrentCulture), Z.ToString(CultureInfo.CurrentCulture),
			    W.ToString(CultureInfo.CurrentCulture) );
	    }

	    public override int GetHashCode()
	    {
		    return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Quaternion)( value ) );
	    }

	    public bool Equals( Quaternion value )
	    {
		    return ( X == value.X && Y == value.Y && Z == value.Z && W == value.W );
	    }

	    public static bool Equals( ref Quaternion value1, ref Quaternion value2 )
	    {
		    return ( value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z && value1.W == value2.W );
	    }
        #endregion

        public Quaternion(float x, float y, float z, float w)
	    {
		    X = x;
		    Y = y;
		    Z = z;
		    W = w;
	    }

	    public Quaternion( Vector3 value, float w )
	    {
		    X = value.X;
		    Y = value.Y;
		    Z = value.Z;
		    W = w;
	    }

        public static Quaternion Identity
	    {
            get
            {
		        Quaternion result;
		        result.X = 0.0f;
		        result.Y = 0.0f;
		        result.Z = 0.0f;
		        result.W = 1.0f;
		        return result;
            }
	    }

	    public bool IsIdentity
	    {
            get
            {
		        if( X != 0.0f || Y != 0.0f || Z != 0.0f )
			        return false;

		        return (W == 1.0f);
            }
	    }

	    public Vector3 Axis
	    {
            get
            {
                unsafe
                {
		            fixed(Quaternion* pinThis = &this)
                    {
		                float angle;
		                Vector3 axis = new Vector3();

		                IDllImportApi.D3DXQuaternionToAxisAngle( pinThis, (Vector3*) &axis, &angle );
		                return axis;
                    }
                }
            }
	    }

	    public float Angle
	    {
            get
            {
                unsafe
                {
		            fixed(Quaternion* pinThis = &this)
                    {
		                float angle;
		                Vector3 axis = new Vector3();

		                IDllImportApi.D3DXQuaternionToAxisAngle( pinThis, (Vector3*) &axis, &angle );
		                return angle;
                    }
                }
            }
	    }

	    public float GetAngleWithAxis(Vector3 axis)
	    {
            unsafe
            {
                fixed (Quaternion* pinThis = &this)
                {
                    float angle;

                    IDllImportApi.D3DXQuaternionToAxisAngle(pinThis, (Vector3*)&axis, &angle);
                    return angle;
                }
            }
	    }

	    //#define CLAMP(x , min , max) ((x) > (max) ? (max) : ((x) < (min) ? (min) : x))
	    public void GetYawPitchRoll(out float Yaw, out float Pitch, out float Roll)
	    {
		    //double d0 = X * X + Y * Y - Z * Z - W * W;
		    //double d1 = 2 * (Y * Z + X * W);
		    //double d2 = X * X - Y * Y - Z * Z + W * W;
		    //Yaw = (float)(Math.Atan(d1 / d0));
		    //Pitch = (float)(Math.Asin(-2 * (Y * W - X * Z)));
		    //Roll = (float)(Math.Atan( 2 * (Z * W + X * Y) / d2));

		    //if(d2 < 0)
		    //{
		    //	if(Roll < 0)
		    //		Roll += Math.PI;
		    //	else
		    //		Roll -= Math.PI;
		    //}

		    //if(d0 < 0)
		    //{
		    //	if(d1 > 0)
		    //		Yaw += Math.PI;
		    //	else
		    //		Yaw -= Math.PI;
		    //}

		    Yaw = (float)(Math.Atan2(2 * (W * Y + Z * X), 1 - 2 * (X * X + Y * Y)));
            float value = 2 * (W * X - Y * Z);
            value = ((value) > (1.0f) ? (1.0f) : ((value) < (-1.0f) ? (-1.0f) : value));
		    Pitch = (float)(Math.Asin(value));
		    Roll = (float)(Math.Atan2(2 * (W * Z + X * Y), 1 - 2 * (Z * Z + X * X)));
	    }

	    public float Length()
	    {
		    return (float)( Math.Sqrt( (X * X) + (Y * Y) + (Z * Z) + (W * W) ) );
	    }
	
	    public float LengthSquared()
	    {
		    return (X * X) + (Y * Y) + (Z * Z) + (W * W);
	    }

	    public void Normalize()
	    {
		    float length = 1.0f / Length();
		    X *= length;
		    Y *= length;
		    Z *= length;
		    W *= length;
	    }

	    public void Conjugate()
	    {
		    X = -X;
		    Y = -Y;
		    Z = -Z;
	    }

	    public void Invert()
	    {
		    float lengthSq = 1.0f / ( (X * X) + (Y * Y) + (Z * Z) + (W * W) );
		    X = -X * lengthSq;
		    Y = -Y * lengthSq;
		    Z = -Z * lengthSq;
		    W = W * lengthSq;
	    }

	    public static Quaternion Add( Quaternion left, Quaternion right )
	    {
		    Quaternion result;
		    result.X = left.X + right.X;
		    result.Y = left.Y + right.Y;
		    result.Z = left.Z + right.Z;
		    result.W = left.W + right.W;
		    return result;
	    }

	    public static void Add( ref Quaternion left, ref Quaternion right, out Quaternion result )
	    {
		    Quaternion r;
		    r.X = left.X + right.X;
		    r.Y = left.Y + right.Y;
		    r.Z = left.Z + right.Z;
		    r.W = left.W + right.W;

		    result = r;
	    }

	    public static Quaternion Barycentric( Quaternion q1, Quaternion q2, Quaternion q3, float f, float g )
	    {
		    Quaternion result = new Quaternion();

            unsafe
            {
                IDllImportApi.D3DXQuaternionBaryCentric((Quaternion*)&result, (Quaternion*)&q1,
                (Quaternion*)&q2, (Quaternion*)&q3, f, g);
            }

		    return result;
	    }

	    public static void Barycentric( ref Quaternion q1, ref Quaternion q2, ref Quaternion q3, float f, float g, out Quaternion result )
	    {
            unsafe
            {
                fixed (Quaternion* pinResult = &result)
                {
                    fixed (Quaternion* pin1 = &q1)
                    {
                        fixed (Quaternion* pin2 = &q2)
                        {
                            fixed (Quaternion* pin3 = &q3)
                            {
                                IDllImportApi.D3DXQuaternionBaryCentric(pinResult, pin1, pin2, pin3, f, g);
                            }
                        }
                    }
                }
            }
	    }

	    public static Quaternion Conjugate( Quaternion quat )
	    {
		    Quaternion result;
		    result.X = -quat.X;
		    result.Y = -quat.Y;
		    result.Z = -quat.Z;
		    result.W = quat.W;
		    return result;
	    }

	    public static void Conjugate( ref Quaternion quat, out Quaternion result )
	    {
		    result.X = -quat.X;
		    result.Y = -quat.Y;
		    result.Z = -quat.Z;
		    result.W = quat.W;
	    }

	    public static Quaternion Divide( Quaternion left, Quaternion right )
	    {
		    Quaternion result;
		    result.X = left.X / right.X;
		    result.Y = left.Y / right.Y;
		    result.Z = left.Z / right.Z;
		    result.W = left.W / right.W;
		    return result;
	    }

	    public static void Divide( ref Quaternion left, ref Quaternion right, out Quaternion result )
	    {
		    result.X = left.X / right.X;
		    result.Y = left.Y / right.Y;
		    result.Z = left.Z / right.Z;
		    result.W = left.W / right.W;
	    }
	
	    public static float Dot( Quaternion left, Quaternion right )
	    {
		    return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W); 
	    }

	    public static Quaternion Exponential( Quaternion quat )
	    {
		    Quaternion result = new Quaternion();
            unsafe
            {
                IDllImportApi.D3DXQuaternionExp((Quaternion*)&result, (Quaternion*)&quat);
            }
		    return result;
	    }

	    public static void Exponential( ref Quaternion quat, out Quaternion result )
	    {
            unsafe
            {
                fixed (Quaternion* pinQuat = &quat)
                {
                    fixed (Quaternion* pinResult = &result)
                    {
                        IDllImportApi.D3DXQuaternionExp(pinResult, pinQuat);
                    }
                }
            }
	    }

	    public static Quaternion Invert( Quaternion quaternion )
	    {
		    Quaternion result;
		    float lengthSq = 1.0f / ( (quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W) );

		    result.X = -quaternion.X * lengthSq;
		    result.Y = -quaternion.Y * lengthSq;
		    result.Z = -quaternion.Z * lengthSq;
		    result.W = quaternion.W * lengthSq;

		    return result;
	    }

	    public static void Invert( ref Quaternion quaternion, out Quaternion result )
	    {
		    float lengthSq = 1.0f / ( (quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y) + (quaternion.Z * quaternion.Z) + (quaternion.W * quaternion.W) );

		    result.X = -quaternion.X * lengthSq;
		    result.Y = -quaternion.Y * lengthSq;
		    result.Z = -quaternion.Z * lengthSq;
		    result.W = quaternion.W * lengthSq;
	    }

	    public static Quaternion Lerp( Quaternion left, Quaternion right, float amount )
	    {
		    Quaternion result;
		    float inverse = 1.0f - amount;
		    float dot = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W); 

		    if( dot >= 0.0f )
		    {
			    result.X = (inverse * left.X) + (amount * right.X);
			    result.Y = (inverse * left.Y) + (amount * right.Y);
			    result.Z = (inverse * left.Z) + (amount * right.Z);
			    result.W = (inverse * left.W) + (amount * right.W);
		    }
		    else
		    {
			    result.X = (inverse * left.X) - (amount * right.X);
			    result.Y = (inverse * left.Y) - (amount * right.Y);
			    result.Z = (inverse * left.Z) - (amount * right.Z);
			    result.W = (inverse * left.W) - (amount * right.W);
		    }

		    float invLength = 1.0f / result.Length();

		    result.X *= invLength;
		    result.Y *= invLength;
		    result.Z *= invLength;
		    result.W *= invLength;

		    return result;
	    }

	    public static void Lerp( ref Quaternion left, ref Quaternion right, float amount, out Quaternion result )
	    {
		    float inverse = 1.0f - amount;
		    float dot = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W); 

		    if( dot >= 0.0f )
		    {
			    result.X = (inverse * left.X) + (amount * right.X);
			    result.Y = (inverse * left.Y) + (amount * right.Y);
			    result.Z = (inverse * left.Z) + (amount * right.Z);
			    result.W = (inverse * left.W) + (amount * right.W);
		    }
		    else
		    {
			    result.X = (inverse * left.X) - (amount * right.X);
			    result.Y = (inverse * left.Y) - (amount * right.Y);
			    result.Z = (inverse * left.Z) - (amount * right.Z);
			    result.W = (inverse * left.W) - (amount * right.W);
		    }

		    float invLength = 1.0f / result.Length();

		    result.X *= invLength;
		    result.Y *= invLength;
		    result.Z *= invLength;
		    result.W *= invLength;
	    }

	    public static Quaternion Logarithm( Quaternion quat )
	    {
		    Quaternion result;
            unsafe
            {
                IDllImportApi.D3DXQuaternionLn((Quaternion*)&result, (Quaternion*)&quat);
            }
		    return result;
	    }

	    public static void Logarithm( ref Quaternion quat, out Quaternion result )
	    {
            unsafe
            {
                fixed (Quaternion* pinQuat = &quat)
                {
                    fixed (Quaternion* pinResult = &result)
                    {
                        IDllImportApi.D3DXQuaternionLn((Quaternion*)pinResult, (Quaternion*)pinQuat);
                    }
                }
            }
	    }

	    public static Quaternion Multiply( Quaternion left, Quaternion right )
	    {
		    Quaternion quaternion;
		    float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;

            quaternion.X = (rx * lw + lx * rw + ry * lz) - (rz * ly);
            quaternion.Y = (ry * lw + ly * rw + rz * lx) - (rx * lz);
            quaternion.Z = (rz * lw + lz * rw + rx * ly) - (ry * lx);
            quaternion.W = (rw * lw) - (rx * lx + ry * ly + rz * lz);

		    return quaternion;
	    }

	    public static void Multiply( ref Quaternion left, ref Quaternion right, out Quaternion result )
	    {
		    float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;

            result.X = (rx * lw + lx * rw + ry * lz) - (rz * ly);
            result.Y = (ry * lw + ly * rw + rz * lx) - (rx * lz);
            result.Z = (rz * lw + lz * rw + rx * ly) - (ry * lx);
            result.W = (rw * lw) - (rx * lx + ry * ly + rz * lz);
	    }

	    public static Quaternion Multiply( Quaternion quaternion, float scale )
	    {
		    Quaternion result;
		    result.X = quaternion.X * scale;
		    result.Y = quaternion.Y * scale;
		    result.Z = quaternion.Z * scale;
		    result.W = quaternion.W * scale;
		    return result;
	    }

	    public static void Multiply( ref Quaternion quaternion, float scale, out Quaternion result )
	    {
		    result.X = quaternion.X * scale;
		    result.Y = quaternion.Y * scale;
		    result.Z = quaternion.Z * scale;
		    result.W = quaternion.W * scale;
	    }

	    public static Quaternion Negate( Quaternion quat )
	    {
		    Quaternion result;
		    result.X = -quat.X;
		    result.Y = -quat.Y;
		    result.Z = -quat.Z;
		    result.W = -quat.W;
		    return result;
	    }

	    public static void Negate( ref Quaternion quat, out Quaternion result )
	    {
		    result.X = -quat.X;
		    result.Y = -quat.Y;
		    result.Z = -quat.Z;
		    result.W = -quat.W;
	    }

	    public static Quaternion Normalize( Quaternion quat )
	    {
		    quat.Normalize();
		    return quat;
	    }

	    public static void Normalize( ref Quaternion quat, out Quaternion result )
	    {
		    float length = 1.0f / quat.Length();
		    result.X = quat.X * length;
		    result.Y = quat.Y * length;
		    result.Z = quat.Z * length;
		    result.W = quat.W * length;
	    }

	    public static Quaternion RotationAxis( Vector3 axis, float angle )
	    {
		    Quaternion result;

		    Vector3.Normalize( ref axis, out axis );

		    float half = angle * 0.5f;
		    float sin = (float)( Math.Sin( (double)( half ) ) );
		    float cos = (float)( Math.Cos( (double)( half ) ) );

		    result.X = axis.X * sin;
		    result.Y = axis.Y * sin;
		    result.Z = axis.Z * sin;
		    result.W = cos;

		    return result;
	    }

	    public static void RotationAxis( ref Vector3 axis, float angle, out Quaternion result )
	    {
		    Vector3.Normalize( ref axis, out axis );

		    float half = angle * 0.5f;
		    float sin = (float)( Math.Sin( (double)( half ) ) );
		    float cos = (float)( Math.Cos( (double)( half ) ) );

		    result.X = axis.X * sin;
		    result.Y = axis.Y * sin;
		    result.Z = axis.Z * sin;
		    result.W = cos;
	    }
	
	    public static Quaternion RotationMatrix( Matrix matrix )
	    {
		    Quaternion result;
		    //float scale = matrix.M11 + matrix.M22 + matrix.M33;

		    //if( scale > 0.0f )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(scale + 1.0f) ) );

		    //	result.W = sqrt * 0.5f;
		    //	sqrt = 0.5f / sqrt;

		    //	result.X = (matrix.M23 - matrix.M32) * sqrt;
		    //	result.Y = (matrix.M31 - matrix.M13) * sqrt;
		    //	result.Z = (matrix.M12 - matrix.M21) * sqrt;

		    //	return result;
		    //}

		    //if( (matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33) )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M11 - matrix.M22 - matrix.M33) ) );
		    //	float half = 0.5f / sqrt;

		    //	result.X = 0.5f * sqrt;
		    //	result.Y = (matrix.M12 + matrix.M21) * half;
		    //	result.Z = (matrix.M13 + matrix.M31) * half;
		    //	result.W = (matrix.M23 - matrix.M32) * half;

		    //	return result;
		    //}

		    //if( matrix.M22 > matrix.M33 )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M22 - matrix.M11 - matrix.M33) ) );
		    //	float half = 0.5f / sqrt;

		    //	result.X = (matrix.M21 + matrix.M12) * half;
		    //	result.Y = 0.5f * sqrt;
		    //	result.Z = (matrix.M32 + matrix.M23) * half;
		    //	result.W = (matrix.M31 - matrix.M13) * half;

		    //	return result;
		    //}

		    //float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M33 - matrix.M11 - matrix.M22) ) );
		    //float half = 0.5f / sqrt;

		    //result.X = (matrix.M31 + matrix.M13) * half;
		    //result.Y = (matrix.M32 + matrix.M23) * half;
		    //result.Z = 0.5f * sqrt;
		    //result.W = (matrix.M12 - matrix.M21) * half;

		    //Quaternion result2;
            unsafe
            {
                IDllImportApi.D3DXQuaternionRotationMatrix((Quaternion*)&result, (Matrix*)&matrix);
            }

		    return result;
	    }

	    public static void RotationMatrix( ref Matrix matrix, out Quaternion result )
	    {
		    //float scale = matrix.M11 + matrix.M22 + matrix.M33;

		    //if( scale > 0.0f )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(scale + 1.0f) ) );

		    //	result.W = sqrt * 0.5f;
		    //	sqrt = 0.5f / sqrt;

		    //	result.X = (matrix.M23 - matrix.M32) * sqrt;
		    //	result.Y = (matrix.M31 - matrix.M13) * sqrt;
		    //	result.Z = (matrix.M12 - matrix.M21) * sqrt;
		    //	return;
		    //}

		    //if( (matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33) )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M11 - matrix.M22 - matrix.M33) ) );
		    //	float half = 0.5f / sqrt;

		    //	result.X = 0.5f * sqrt;
		    //	result.Y = (matrix.M12 + matrix.M21) * half;
		    //	result.Z = (matrix.M13 + matrix.M31) * half;
		    //	result.W = (matrix.M23 - matrix.M32) * half;
		    //	return;
		    //}

		    //if( matrix.M22 > matrix.M33 )
		    //{
		    //	float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M22 - matrix.M11 - matrix.M33) ) );
		    //	float half = 0.5f / sqrt;

		    //	result.X = (matrix.M21 + matrix.M12) * half;
		    //	result.Y = 0.5f * sqrt;
		    //	result.Z = (matrix.M32 + matrix.M23) * half;
		    //	result.W = (matrix.M31 - matrix.M13) * half;
		    //	return;
		    //}

		    //float sqrt = (float)( Math.Sqrt( (double)(1.0f + matrix.M33 - matrix.M11 - matrix.M22) ) );
		    //float half = 0.5f / sqrt;

		    //result.X = (matrix.M31 + matrix.M13) * half;
		    //result.Y = (matrix.M32 + matrix.M23) * half;
		    //result.Z = 0.5f * sqrt;
		    //result.W = (matrix.M12 - matrix.M21) * half;

            unsafe
            {
                fixed (Quaternion* pin_result = &result)
                {
                    fixed (Matrix* pin_matrix = &matrix)
                    {
                        IDllImportApi.D3DXQuaternionRotationMatrix(pin_result, pin_matrix);
                    }
                }
            }
	    }

	    public static Quaternion RotationYawPitchRoll( float yaw, float pitch, float roll )
	    {
		    Quaternion result;

		    float halfRoll = roll * 0.5f;
		    float sinRoll = (float)( Math.Sin( (double)( halfRoll ) ) );
		    float cosRoll = (float)( Math.Cos( (double)( halfRoll ) ) );
		    float halfPitch = pitch * 0.5f;
		    float sinPitch = (float)( Math.Sin( (double)( halfPitch ) ) );
		    float cosPitch = (float)( Math.Cos( (double)( halfPitch ) ) );
		    float halfYaw = yaw * 0.5f;
		    float sinYaw = (float)( Math.Sin( (double)( halfYaw ) ) );
		    float cosYaw = (float)( Math.Cos( (double)( halfYaw ) ) );

		    result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
		    result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
		    result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
		    result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);

		    return result;
	    }

	    public static void RotationYawPitchRoll( float yaw, float pitch, float roll, out Quaternion result )
	    {
		    float halfRoll = roll * 0.5f;
		    float sinRoll = (float)( Math.Sin( (double)( halfRoll ) ) );
		    float cosRoll = (float)( Math.Cos( (double)( halfRoll ) ) );
		    float halfPitch = pitch * 0.5f;
		    float sinPitch = (float)( Math.Sin( (double)( halfPitch ) ) );
		    float cosPitch = (float)( Math.Cos( (double)( halfPitch ) ) );
		    float halfYaw = yaw * 0.5f;
		    float sinYaw = (float)( Math.Sin( (double)( halfYaw ) ) );
		    float cosYaw = (float)( Math.Cos( (double)( halfYaw ) ) );

		    result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
		    result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
		    result.Z = (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll);
		    result.W = (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll);
	    }

	    public static Quaternion Slerp( Quaternion q1, Quaternion q2, float t )
	    {
		    Quaternion result;

		    float opposite;
		    float inverse;
		    float dot = (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
		    bool flag = false;

		    if( dot < 0.0f )
		    {
			    flag = true;
			    dot = -dot;
		    }

		    if( dot > 0.999999f )
		    {
			    inverse = 1.0f - t;
			    opposite = flag ? -t : t;
		    }
		    else
		    {
			    float acos = (float)( Math.Acos( (double)( dot ) ) );
			    float invSin = (float)( ( 1.0f / Math.Sin( (double)( acos ) ) ) );

			    inverse = ( (float)( Math.Sin( (double)( (1.0f - t) * acos ) ) ) ) * invSin;
			    opposite = flag ? ( ( (float)( -Math.Sin( (double)( t * acos ) ) ) ) * invSin ) : ( ( (float)( Math.Sin( (double)( t * acos ) ) ) ) * invSin );
		    }

		    result.X = (inverse * q1.X) + (opposite * q2.X);
		    result.Y = (inverse * q1.Y) + (opposite * q2.Y);
		    result.Z = (inverse * q1.Z) + (opposite * q2.Z);
		    result.W = (inverse * q1.W) + (opposite * q2.W);

		    return result;
	    }

	    public static void Slerp( ref Quaternion q1, ref Quaternion q2, float t, out Quaternion result )
	    {
		    float opposite;
		    float inverse;
		    float dot = (q1.X * q2.X) + (q1.Y * q2.Y) + (q1.Z * q2.Z) + (q1.W * q2.W);
		    bool flag = false;

		    if( dot < 0.0f )
		    {
			    flag = true;
			    dot = -dot;
		    }

		    if( dot > 0.999999f )
		    {
			    inverse = 1.0f - t;
			    opposite = flag ? -t : t;
		    }
		    else
		    {
			    float acos = (float)( Math.Acos( (double)( dot ) ) );
			    float invSin = (float)( ( 1.0f / Math.Sin( (double)( acos ) ) ) );

			    inverse = ( (float)( Math.Sin( (double)( (1.0f - t) * acos ) ) ) ) * invSin;
			    opposite = flag ? ( ( (float)( -Math.Sin( (double)( t * acos ) ) ) ) * invSin ) : ( ( (float)( Math.Sin( (double)( t * acos ) ) ) ) * invSin );
		    }

		    result.X = (inverse * q1.X) + (opposite * q2.X);
		    result.Y = (inverse * q1.Y) + (opposite * q2.Y);
		    result.Z = (inverse * q1.Z) + (opposite * q2.Z);
		    result.W = (inverse * q1.W) + (opposite * q2.W);
	    }

	    public static Quaternion Squad( Quaternion q1, Quaternion a, Quaternion b, Quaternion c, float t )
	    {
		    Quaternion result;

            unsafe
            {
                IDllImportApi.D3DXQuaternionSquad((Quaternion*)&result, (Quaternion*)&q1, (Quaternion*)&a,
                    (Quaternion*)&b, (Quaternion*)&c, t);
            }

		    return result;
	    }

	    public static void Squad( ref Quaternion q1, ref Quaternion a, ref Quaternion b, ref Quaternion c, float t, out Quaternion result )
	    {
            unsafe
            {
		        fixed(Quaternion* pin1 = &q1)
                {
		            fixed(Quaternion* pinA = &a)
                    {
		                fixed(Quaternion* pinB = &b)
                        {
		                    fixed(Quaternion* pinC = &c)
                            {
                                fixed (Quaternion* pinResult = &result)
                                {
                                    IDllImportApi.D3DXQuaternionSquad(pinResult, pin1, pinA,pinB, pinC, t);
                                }
                            }
                        }
                    }
                }
            }
	    }

	    public static Quaternion[] SquadSetup( Quaternion source1, Quaternion source2, Quaternion source3, Quaternion source4 )
	    {
		    Quaternion result1 = new Quaternion();
		    Quaternion result2 = new Quaternion();
		    Quaternion result3 = new Quaternion();
            Quaternion[] results = new Quaternion[3];

            unsafe
            {
                IDllImportApi.D3DXQuaternionSquadSetup((Quaternion*)&result1, (Quaternion*)&result2, (Quaternion*)&result3,
                       (Quaternion*)&source1, (Quaternion*)&source2, (Quaternion*)&source3, (Quaternion*)&source4);
            }

		    results[0] = result1;
		    results[1] = result2;
		    results[2] = result3;
		    return results;
	    }

	    public static Quaternion Subtract( Quaternion left, Quaternion right )
	    {
		    Quaternion result;
		    result.X = left.X - right.X;
		    result.Y = left.Y - right.Y;
		    result.Z = left.Z - right.Z;
		    result.W = left.W - right.W;
		    return result;
	    }

	    public static void Subtract( ref Quaternion left, ref Quaternion right, out Quaternion result )
	    {
		    result.X = left.X - right.X;
		    result.Y = left.Y - right.Y;
		    result.Z = left.Z - right.Z;
		    result.W = left.W - right.W;
	    }

	    public static Quaternion operator * (Quaternion left, Quaternion right)
	    {
		    Quaternion quaternion;
		    float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;

            quaternion.X = (rx * lw + lx * rw + ry * lz) - (rz * ly);
            quaternion.Y = (ry * lw + ly * rw + rz * lx) - (rx * lz);
            quaternion.Z = (rz * lw + lz * rw + rx * ly) - (ry * lx);
            quaternion.W = (rw * lw) - (rx * lx + ry * ly + rz * lz);

		    return quaternion;
	    }

	    public static Quaternion operator * (Quaternion quaternion, float scale)
	    {
		    Quaternion result;
		    result.X = quaternion.X * scale;
		    result.Y = quaternion.Y * scale;
		    result.Z = quaternion.Z * scale;
		    result.W = quaternion.W * scale;
		    return result;
	    }

	    public static Quaternion operator * (float scale, Quaternion quaternion)
	    {
		    Quaternion result;
		    result.X = quaternion.X * scale;
		    result.Y = quaternion.Y * scale;
		    result.Z = quaternion.Z * scale;
		    result.W = quaternion.W * scale;
		    return result;
	    }

	    public static Quaternion operator / (Quaternion lhs, float rhs)
	    {
		    Quaternion result;
		    result.X = lhs.X / rhs;
		    result.Y = lhs.Y / rhs;
		    result.Z = lhs.Z / rhs;
		    result.W = lhs.W / rhs;
		    return result;
	    }

	    public static Quaternion operator + (Quaternion lhs, Quaternion rhs)
	    {
		    Quaternion result;
		    result.X = lhs.X + rhs.X;
		    result.Y = lhs.Y + rhs.Y;
		    result.Z = lhs.Z + rhs.Z;
		    result.W = lhs.W + rhs.W;
		    return result;
	    }

	    public static Quaternion operator - (Quaternion lhs, Quaternion rhs)
	    {
		    Quaternion result;
		    result.X = lhs.X - rhs.X;
		    result.Y = lhs.Y - rhs.Y;
		    result.Z = lhs.Z - rhs.Z;
		    result.W = lhs.W - rhs.W;
		    return result;
	    }

	    public static Quaternion operator - (Quaternion quaternion)
	    {
		    Quaternion result;
		    result.X = -quaternion.X;
		    result.Y = -quaternion.Y;
		    result.Z = -quaternion.Z;
		    result.W = -quaternion.W;
		    return result;
	    }

	    public static bool operator == ( Quaternion left, Quaternion right )
	    {
		    return Equals( left, right );
	    }

	    public static bool operator != ( Quaternion left, Quaternion right )
	    {
		    return !Equals( left, right );
	    }
    }
}
