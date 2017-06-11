using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Runtime.InteropServices.OutAttribute;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4 )]
    //[System.ComponentModel.TypeConverter(typeof(SlimDX.Design.Vector3Converter))]
	public struct Vector3 : System.IEquatable<Vector3>
    {
        #region Def Struct
        /// <summary>
        /// Gets or sets the X component of the vector.
        /// </summary>
        /// <value>The X component of the vector.</value>
        public float X;

        /// <summary>
        /// Gets or sets the Y component of the vector.
        /// </summary>
        /// <value>The Y component of the vector.</value>
        public float Y;

        /// <summary>
        /// Gets or sets the Z component of the vector.
        /// </summary>
        /// <value>The Z component of the vector.</value>
        public float Z;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for Vector3 run from 0 to 2, inclusive.");
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
		        case 2:
			        Z = value;
			        break;
		        default:
			        throw new ArgumentOutOfRangeException( "index", "Indices for Vector3 run from 0 to 2, inclusive." );
		        }
            }
        }
        #endregion

        #region Static Member
        public static Vector3 UnitXYZ = new Vector3(1,1,1);
        public static Vector3 Zero = new Vector3(0, 0, 0);
        public static Vector3 UnitX = new Vector3(1, 0, 0);
        public static Vector3 UnitY = new Vector3(0, 1, 0);
        public static Vector3 UnitZ = new Vector3(0, 0, 1);
        #endregion

        #region Constructure
        public Vector3(Vector3 value)
        {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
        }

        public Vector3( float value )
	    {
		    X = value;
		    Y = value;
		    Z = value;
	    }	
        //public Vector3( Vector2 value, float z )
        //{
        //    X = value.X;
        //    Y = value.Y;
        //    Z = z;
        //}
        public Vector3(float x, float y, float z)
	    {
		    X = x;
		    Y = y;
		    Z = z;
	    }
        #endregion

        public float Length()
	    {
		    return (float)( System.Math.Sqrt( (X * X) + (Y * Y) + (Z * Z) ) );
	    }
	
	    public float LengthSquared()
	    {
		    return (X * X) + (Y * Y) + (Z * Z);
	    }

	    public void Normalize()
	    {
		    float length = Length();
		    if( length == 0 )
			    return;
		    float num = 1 / length;
		    X *= num;
		    Y *= num;
		    Z *= num;
	    }
	
	    public static Vector3 Add( Vector3 left, Vector3 right )
	    {
		    return new Vector3( left.X + right.X, left.Y + right.Y, left.Z + right.Z );
	    }

        public static void Add(ref Vector3 left, ref Vector3 right, out Vector3 result)
	    {
		    result = new Vector3( left.X + right.X, left.Y + right.Y, left.Z + right.Z );
	    }

        public static Vector3 Subtract(Vector3 left, Vector3 right)
	    {
		    return new Vector3( left.X - right.X, left.Y - right.Y, left.Z - right.Z );
	    }

        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
	    {
		    result = new Vector3( left.X - right.X, left.Y - right.Y, left.Z - right.Z );
	    }

        public static Vector3 Modulate(Vector3 left, Vector3 right)
	    {
		    return new Vector3( left.X * right.X, left.Y * right.Y, left.Z * right.Z );
	    }

        public static void Modulate(ref Vector3 left, ref Vector3 right, out Vector3 result)
	    {
		    result = new Vector3( left.X * right.X, left.Y * right.Y, left.Z * right.Z );
	    }

        public static Vector3 Multiply(Vector3 value, float scale)
	    {
		    return new Vector3( value.X * scale, value.Y * scale, value.Z * scale );
	    }

        public static void Multiply(ref Vector3 value, float scale, out Vector3 result)
	    {
		    result = new Vector3( value.X * scale, value.Y * scale, value.Z * scale );
	    }

        public static Vector3 Divide(Vector3 value, float scale)
	    {
		    return new Vector3( value.X / scale, value.Y / scale, value.Z / scale );
	    }

        public static void Divide(ref Vector3 value, float scale, out Vector3 result)
	    {
		    result = new Vector3( value.X / scale, value.Y / scale, value.Z / scale );
	    }

        public static Vector3 Negate(Vector3 value)
	    {
		    return new Vector3( -value.X, -value.Y, -value.Z );
	    }

        public static void Negate(ref Vector3 value, out Vector3 result)
	    {
		    result = new Vector3( -value.X, -value.Y, -value.Z );
	    }

        public static Vector3 Barycentric(Vector3 value1, Vector3 value2, Vector3 value3, float amount1, float amount2)
	    {
		    Vector3 vector = new Vector3();
		    vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
		    vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
		    vector.Z = (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z));
		    return vector;
	    }

        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
	    {
		    result = new Vector3((value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X)),
			    (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y)),
			    (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z)) );
	    }

        public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
	    {
		    Vector3 vector = new Vector3();
		    float squared = amount * amount;
		    float cubed = amount * squared;

		    vector.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) + 
			    (((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) + 
			    ((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));

		    vector.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + 
			    (((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) + 
			    ((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));

		    vector.Z = 0.5f * ((((2.0f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + 
			    (((((2.0f * value1.Z) - (5.0f * value2.Z)) + (4.0f * value3.Z)) - value4.Z) * squared)) + 
			    ((((-value1.Z + (3.0f * value2.Z)) - (3.0f * value3.Z)) + value4.Z) * cubed));

		    return vector;
	    }

        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
	    {
		    float squared = amount * amount;
		    float cubed = amount * squared;
		
		    Vector3 r = new Vector3();

		    r.X = 0.5f * ((((2.0f * value2.X) + ((-value1.X + value3.X) * amount)) + 
			    (((((2.0f * value1.X) - (5.0f * value2.X)) + (4.0f * value3.X)) - value4.X) * squared)) + 
			    ((((-value1.X + (3.0f * value2.X)) - (3.0f * value3.X)) + value4.X) * cubed));

		    r.Y = 0.5f * ((((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + 
			    (((((2.0f * value1.Y) - (5.0f * value2.Y)) + (4.0f * value3.Y)) - value4.Y) * squared)) + 
			    ((((-value1.Y + (3.0f * value2.Y)) - (3.0f * value3.Y)) + value4.Y) * cubed));

		    r.Z = 0.5f * ((((2.0f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + 
			    (((((2.0f * value1.Z) - (5.0f * value2.Z)) + (4.0f * value3.Z)) - value4.Z) * squared)) + 
			    ((((-value1.Z + (3.0f * value2.Z)) - (3.0f * value3.Z)) + value4.Z) * cubed));

		    result = r;
	    }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
	    {
		    float x = value.X;
		    x = (x > max.X) ? max.X : x;
		    x = (x < min.X) ? min.X : x;

		    float y = value.Y;
		    y = (y > max.Y) ? max.Y : y;
		    y = (y < min.Y) ? min.Y : y;

		    float z = value.Z;
		    z = (z > max.Z) ? max.Z : z;
		    z = (z < min.Z) ? min.Z : z;

		    return new Vector3( x, y, z );
	    }

        public static void Clamp(ref Vector3 value, ref Vector3 min, ref Vector3 max, out Vector3 result)
	    {
		    float x = value.X;
		    x = (x > max.X) ? max.X : x;
		    x = (x < min.X) ? min.X : x;

		    float y = value.Y;
		    y = (y > max.Y) ? max.Y : y;
		    y = (y < min.Y) ? min.Y : y;

		    float z = value.Z;
		    z = (z > max.Z) ? max.Z : z;
		    z = (z < min.Z) ? min.Z : z;

		    result = new Vector3( x, y, z );
	    }

        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
	    {
		    Vector3 vector = new Vector3();
		    float squared = amount * amount;
		    float cubed = amount * squared;
		    float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
		    float part2 = (-2.0f * cubed) + (3.0f * squared);
		    float part3 = (cubed - (2.0f * squared)) + amount;
		    float part4 = cubed - squared;

		    vector.X = (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4);
		    vector.Y = (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
		    vector.Z = (((value1.Z * part1) + (value2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4);

		    return vector;
	    }

        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
	    {
		    float squared = amount * amount;
		    float cubed = amount * squared;
		    float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
		    float part2 = (-2.0f * cubed) + (3.0f * squared);
		    float part3 = (cubed - (2.0f * squared)) + amount;
		    float part4 = cubed - squared;

		    result.X = (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4);
		    result.Y = (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4);
		    result.Z = (((value1.Z * part1) + (value2.Z * part2)) + (tangent1.Z * part3)) + (tangent2.Z * part4);
	    }

        public static Vector3 Lerp(Vector3 start, Vector3 end, float factor)
	    {
		    Vector3 vector = new Vector3();

		    vector.X = start.X + ((end.X - start.X) * factor);
		    vector.Y = start.Y + ((end.Y - start.Y) * factor);
		    vector.Z = start.Z + ((end.Z - start.Z) * factor);

		    return vector;
	    }

        public static void Lerp(ref Vector3 start, ref Vector3 end, float factor, out Vector3 result)
	    {
		    result.X = start.X + ((end.X - start.X) * factor);
		    result.Y = start.Y + ((end.Y - start.Y) * factor);
		    result.Z = start.Z + ((end.Z - start.Z) * factor);
	    }

        public static Vector3 SmoothStep(Vector3 start, Vector3 end, float amount)
	    {
		    Vector3 vector = new Vector3();

		    amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
		    amount = (amount * amount) * (3.0f - (2.0f * amount));

		    vector.X = start.X + ((end.X - start.X) * amount);
		    vector.Y = start.Y + ((end.Y - start.Y) * amount);
		    vector.Z = start.Z + ((end.Z - start.Z) * amount);

		    return vector;
	    }

        public static void SmoothStep(ref Vector3 start, ref Vector3 end, float amount, out Vector3 result)
	    {
		    amount = (amount > 1.0f) ? 1.0f : ((amount < 0.0f) ? 0.0f : amount);
		    amount = (amount * amount) * (3.0f - (2.0f * amount));

		    result.X = start.X + ((end.X - start.X) * amount);
		    result.Y = start.Y + ((end.Y - start.Y) * amount);
		    result.Z = start.Z + ((end.Z - start.Z) * amount);
	    }

        public static float Distance(Vector3 value1, Vector3 value2)
	    {
		    float x = value1.X - value2.X;
		    float y = value1.Y - value2.Y;
		    float z = value1.Z - value2.Z;

		    return (float)( Math.Sqrt( (x * x) + (y * y) + (z * z) ) );
	    }

        public static float DistanceSquared(Vector3 value1, Vector3 value2)
	    {
		    float x = value1.X - value2.X;
		    float y = value1.Y - value2.Y;
		    float z = value1.Z - value2.Z;

		    return (x * x) + (y * y) + (z * z);
	    }

        public static float Dot(Vector3 left, Vector3 right)
	    {
		    return (left.X * right.X + left.Y * right.Y + left.Z * right.Z);
	    }

        public static Vector3 Cross(Vector3 left, Vector3 right)
	    {
		    Vector3 result = new Vector3();
		    result.X = left.Y * right.Z - left.Z * right.Y;
		    result.Y = left.Z * right.X - left.X * right.Z;
		    result.Z = left.X * right.Y - left.Y * right.X;
		    return result;
	    }

        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
	    {
		    Vector3 r = new Vector3();
		    r.X = left.Y * right.Z - left.Z * right.Y;
		    r.Y = left.Z * right.X - left.X * right.Z;
		    r.Z = left.X * right.Y - left.Y * right.X; 

		    result = r;
	    }

        public static Vector3 Reflect(Vector3 vector, Vector3 normal)
	    {
		    Vector3 result = new Vector3();
		    float dot = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);

		    result.X = vector.X - ((2.0f * dot) * normal.X);
		    result.Y = vector.Y - ((2.0f * dot) * normal.Y);
		    result.Z = vector.Z - ((2.0f * dot) * normal.Z);

		    return result;
	    }

        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
	    {
		    float dot = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);

		    result.X = vector.X - ((2.0f * dot) * normal.X);
		    result.Y = vector.Y - ((2.0f * dot) * normal.Y);
		    result.Z = vector.Z - ((2.0f * dot) * normal.Z);
	    }

        public static Vector3 Normalize(Vector3 vector)
	    {
		    vector.Normalize();
		    return vector;
	    }

        public static void Normalize(ref Vector3 vector, out Vector3 result)
	    {
		    result = new Vector3(vector);
		    result.Normalize();
	    }

        public static Vector4 Transform(Vector3 vector, Matrix transform)
        {
            Vector4 result;

            result.X = (((vector.X * transform.M11) + (vector.Y * transform.M21)) + (vector.Z * transform.M31)) + transform.M41;
            result.Y = (((vector.X * transform.M12) + (vector.Y * transform.M22)) + (vector.Z * transform.M32)) + transform.M42;
            result.Z = (((vector.X * transform.M13) + (vector.Y * transform.M23)) + (vector.Z * transform.M33)) + transform.M43;
            result.W = (((vector.X * transform.M14) + (vector.Y * transform.M24)) + (vector.Z * transform.M34)) + transform.M44;

            return result;
        }

        public static void Transform(ref Vector3 vector, ref Matrix transform, out Vector4 result)
        {
            result = new Vector4();
            result.X = (((vector.X * transform.M11) + (vector.Y * transform.M21)) + (vector.Z * transform.M31)) + transform.M41;
            result.Y = (((vector.X * transform.M12) + (vector.Y * transform.M22)) + (vector.Z * transform.M32)) + transform.M42;
            result.Z = (((vector.X * transform.M13) + (vector.Y * transform.M23)) + (vector.Z * transform.M33)) + transform.M43;
            result.W = (((vector.X * transform.M14) + (vector.Y * transform.M24)) + (vector.Z * transform.M34)) + transform.M44;
        }

        public unsafe static void Transform(Vector3* vectorsIn, int inputStride, Matrix* transformation, Vector4* vectorsOut, int outputStride, int count)
        {
            IDllImportApi.D3DXVec3TransformArray((vectorsOut), (UInt32)outputStride,
                (vectorsIn), (UInt32)inputStride,
                (transformation), (UInt32)count);
        }

        public unsafe static void Transform(Vector3* vectorsIn, Matrix* transformation, Vector4* vectorsOut, int count) 
        { 
            Transform(vectorsIn, (int)sizeof(Vector3), transformation, vectorsOut, (int)sizeof(Vector4), count); 
        }

        public static void Transform(Vector3[] vectorsIn, ref Matrix transformation, Vector4[] vectorsOut, int offset, int count)
        {
            if(vectorsIn.Length != vectorsOut.Length)
                throw new ArgumentException( "Input and output arrays must be the same size.", "vectorsOut" );
            
            unsafe
            {
                fixed (Vector3* pinnedIn = &vectorsIn[offset])
                {
                    fixed (Matrix* pinnedMatrix = &transformation)
                    {
                        fixed (Vector4* pinnedOut = &vectorsOut[offset])
                        {
                            Transform(pinnedIn, pinnedMatrix, pinnedOut, count);
                        }
                    }
                }
            }
        }

        public static void Transform( Vector3[] vectorsIn, ref Matrix transformation, Vector4[] vectorsOut ) 
        { 
            Transform( vectorsIn, ref transformation, vectorsOut, 0, 0 ); 
        }

        public static Vector4[] Transform(Vector3[] vectors, ref Matrix transform)
        {
            int count = vectors.Length;
            Vector4[] results = new Vector4[ count ];

            /*for( int i = 0; i < count; i++ )
            {
                Vector4 r;
                r.X = (((vectors[i].X * transform.M11) + (vectors[i].Y * transform.M21)) + (vectors[i].Z * transform.M31)) + transform.M41;
                r.Y = (((vectors[i].X * transform.M12) + (vectors[i].Y * transform.M22)) + (vectors[i].Z * transform.M32)) + transform.M42;
                r.Z = (((vectors[i].X * transform.M13) + (vectors[i].Y * transform.M23)) + (vectors[i].Z * transform.M33)) + transform.M43;
                r.W = (((vectors[i].X * transform.M14) + (vectors[i].Y * transform.M24)) + (vectors[i].Z * transform.M34)) + transform.M44;
		
                results[i] = r;
            }*/
            Transform( vectors, ref transform, results );
            return results;
        }

        public static Vector4 Transform(Vector3 value, Quaternion rotation)
        {
            Vector4 vector;
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

            vector.X = ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz))) + (value.Z * (xz + wy));
            vector.Y = ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz))) + (value.Z * (yz - wx));
            vector.Z = ((value.X * (xz - wy)) + (value.Y * (yz + wx))) + (value.Z * ((1.0f - xx) - yy));
            vector.W = 1.0f;

            return vector;
        }

        public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector4 result)
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

            result = new Vector4();
            result.X = ((value.X * ((1.0f - yy) - zz)) + (value.Y * (xy - wz))) + (value.Z * (xz + wy));
            result.Y = ((value.X * (xy + wz)) + (value.Y * ((1.0f - xx) - zz))) + (value.Z * (yz - wx));
            result.Z = ((value.X * (xz - wy)) + (value.Y * (yz + wx))) + (value.Z * ((1.0f - xx) - yy));
            result.W = 1.0f;
        }

        public static Vector4[] Transform(Vector3[] vectors, ref Quaternion rotation)
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
                Vector4 r;
                r.X = ((vectors[i].X * ((1.0f - yy) - zz)) + (vectors[i].Y * (xy - wz))) + (vectors[i].Z * (xz + wy));
                r.Y = ((vectors[i].X * (xy + wz)) + (vectors[i].Y * ((1.0f - xx) - zz))) + (vectors[i].Z * (yz - wx));
                r.Z = ((vectors[i].X * (xz - wy)) + (vectors[i].Y * (yz + wx))) + (vectors[i].Z * ((1.0f - xx) - yy));
                r.W = 1.0f;

                results[i] = r;
            }

            return results;
        }

        public static Vector3 TransformCoordinate(Vector3 coord, Matrix transform)
        {
            Vector4 vector;

            vector.X = (((coord.X * transform.M11) + (coord.Y * transform.M21)) + (coord.Z * transform.M31)) + transform.M41;
            vector.Y = (((coord.X * transform.M12) + (coord.Y * transform.M22)) + (coord.Z * transform.M32)) + transform.M42;
            vector.Z = (((coord.X * transform.M13) + (coord.Y * transform.M23)) + (coord.Z * transform.M33)) + transform.M43;
            vector.W = 1 / ((((coord.X * transform.M14) + (coord.Y * transform.M24)) + (coord.Z * transform.M34)) + transform.M44);

            return new Vector3( vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W );
        }

        public static void TransformCoordinate(ref Vector3 coord, ref Matrix transform, out Vector3 result)
        {
            Vector4 vector;

            vector.X = (((coord.X * transform.M11) + (coord.Y * transform.M21)) + (coord.Z * transform.M31)) + transform.M41;
            vector.Y = (((coord.X * transform.M12) + (coord.Y * transform.M22)) + (coord.Z * transform.M32)) + transform.M42;
            vector.Z = (((coord.X * transform.M13) + (coord.Y * transform.M23)) + (coord.Z * transform.M33)) + transform.M43;
            vector.W = 1 / ((((coord.X * transform.M14) + (coord.Y * transform.M24)) + (coord.Z * transform.M34)) + transform.M44);

            result = new Vector3(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
        }

        public static Vector3 TransformCoordinate(Vector3 value, Quaternion rotation)
        {
            var v4 = Transform(value, rotation);
            return new Vector3(v4.X, v4.Y, v4.Z);
        }
	
        public unsafe static void TransformCoordinate( Vector3* coordsIn, int inputStride, Matrix* transformation, Vector3* coordsOut, int outputStride, int count )
        {
            IDllImportApi.D3DXVec3TransformCoordArray( coordsOut , (UInt32)outputStride,
                    coordsIn, (UInt32)inputStride,
                    transformation, (UInt32)count);
        }

        public unsafe static void TransformCoordinate(Vector3* coordinatesIn, Matrix* transformation, Vector3* coordinatesOut, int count) 
        { 
            TransformCoordinate(coordinatesIn, (int)sizeof(Vector3), transformation, coordinatesOut, (int)sizeof(Vector3), count); 
        }

        public static void TransformCoordinate( Vector3[] coordsIn, ref Matrix transformation, Vector3[] coordsOut, int offset, int count )
        {
            if(coordsIn.Length != coordsOut.Length)
                throw new ArgumentException( "Input and output arrays must be the same size.", "coordinatesOut" );
            //Utilities::CheckArrayBounds( coordsIn, offset, count );

            unsafe
            {
                fixed (Vector3* pinnedIn = &coordsIn[offset])
                {
                    fixed (Matrix* pinnedMatrix = &transformation)
                    {
                        fixed (Vector3* pinnedOut = &coordsOut[offset])
                        {
                            TransformCoordinate(pinnedIn, pinnedMatrix, pinnedOut, count);
                        }
                    }
                }
            }
        }

        public static void TransformCoordinate(Vector3[] coordinatesIn, ref Matrix transformation, Vector3[] coordinatesOut) 
        { 
            TransformCoordinate(coordinatesIn, ref transformation, coordinatesOut, 0, 0); 
        }

        public static Vector3[] TransformCoordinate(Vector3[] coords, ref Matrix transform)
        {
            if( coords == null )
                throw new ArgumentNullException( "coordinates" );

            //Vector4 vector = new Vector4();
            int count = coords.Length;
            Vector3[] results = new Vector3[ count ];

            /*for( int i = 0; i < count; i++ )
            {
                vector.X = (((coords[i].X * transform.M11) + (coords[i].Y * transform.M21)) + (coords[i].Z * transform.M31)) + transform.M41;
                vector.Y = (((coords[i].X * transform.M12) + (coords[i].Y * transform.M22)) + (coords[i].Z * transform.M32)) + transform.M42;
                vector.Z = (((coords[i].X * transform.M13) + (coords[i].Y * transform.M23)) + (coords[i].Z * transform.M33)) + transform.M43;
                vector.W = 1 / ((((coords[i].X * transform.M14) + (coords[i].Y * transform.M24)) + (coords[i].Z * transform.M34)) + transform.M44);
                results[i] = Vector3( vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W );
            }*/
            TransformCoordinate( coords, ref transform, results );
            return results;
        }

        public static Vector3 TransformNormal(Vector3 normal, Matrix transform)
        {
            Vector3 vector;

            vector.X = ((normal.X * transform.M11) + (normal.Y * transform.M21)) + (normal.Z * transform.M31);
            vector.Y = ((normal.X * transform.M12) + (normal.Y * transform.M22)) + (normal.Z * transform.M32);
            vector.Z = ((normal.X * transform.M13) + (normal.Y * transform.M23)) + (normal.Z * transform.M33);

            return vector;
        }

        public static void TransformNormal(ref Vector3 normal, ref Matrix transform, out Vector3 result)
        {
            result.X = ((normal.X * transform.M11) + (normal.Y * transform.M21)) + (normal.Z * transform.M31);
            result.Y = ((normal.X * transform.M12) + (normal.Y * transform.M22)) + (normal.Z * transform.M32);
            result.Z = ((normal.X * transform.M13) + (normal.Y * transform.M23)) + (normal.Z * transform.M33);
        }
	
        public unsafe static void TransformNormal( Vector3* normalsIn, int inputStride, Matrix* transformation, Vector3* normalsOut, int outputStride, int count )
        {
            IDllImportApi.D3DXVec3TransformNormalArray( normalsOut , (UInt32)outputStride,
                normalsIn, (UInt32)inputStride,
                transformation, (UInt32)count);
        }

        public unsafe static void TransformNormal(Vector3* normalsIn, Matrix* transformation, Vector3* normalsOut, int count) 
        {
            TransformNormal(normalsIn, (int)sizeof(Vector3), transformation, normalsOut, (int)sizeof(Vector3), count); 
        }

        public static void TransformNormal(Vector3[] normalsIn, ref Matrix transformation, Vector3[] normalsOut) 
        { 
            TransformNormal(normalsIn, ref transformation, normalsOut, 0, 0); 
        }

        public static void TransformNormal(Vector3[] normalsIn, ref Matrix transformation, Vector3[] normalsOut, int offset, int count)
        {
            if(normalsIn.Length != normalsOut.Length)
                throw new ArgumentException( "Input and output arrays must be the same size.", "normalsOut" );
            //Utilities::CheckArrayBounds( normalsOut, offset, count );

            unsafe
            {
                fixed (Vector3* pinnedIn = &normalsIn[offset])
                {
                    fixed (Matrix* pinnedMatrix = &transformation)
                    {
                        fixed (Vector3* pinnedOut = &normalsOut[offset])
                        {
                            TransformNormal(pinnedIn, pinnedMatrix, pinnedOut, count);
                        }
                    }
                }
            }
        }

        public static Vector3[] TransformNormal(Vector3[] normals, ref Matrix transform)
        {
            if( normals == null )
                throw new ArgumentNullException( "normals" );

            int count = normals.Length;
            Vector3[] results = new Vector3[count];

            /*for( int i = 0; i < count; i++ )
            {
                Vector3 r;
                r.X = ((normals[i].X * transform.M11) + (normals[i].Y * transform.M21)) + (normals[i].Z * transform.M31);
                r.Y = ((normals[i].X * transform.M12) + (normals[i].Y * transform.M22)) + (normals[i].Z * transform.M32);
                r.Z = ((normals[i].X * transform.M13) + (normals[i].Y * transform.M23)) + (normals[i].Z * transform.M33);
		
                results[i] = r;
            }*/
            TransformNormal( normals, ref transform, results );
            return results;
        }

        public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection)
        {
            TransformCoordinate( ref vector, ref worldViewProjection, out vector );
            return new Vector3( ( ( 1.0f + vector.X ) * 0.5f * width ) + x, ( ( 1.0f - vector.Y ) * 0.5f * height ) + y, ( vector.Z * ( maxZ - minZ ) ) + minZ );
        }

        public static void Project(ref Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Vector3 result)
        {
            Vector3 v;
            TransformCoordinate( ref vector, ref worldViewProjection, out v );

            result = new Vector3( ( ( 1.0f + v.X ) * 0.5f * width ) + x, ( ( 1.0f - v.Y ) * 0.5f * height ) + y, ( v.Z * ( maxZ - minZ ) ) + minZ );
        }
	
        public static Vector3 Unproject( Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix worldViewProjection )
        {
            Vector3 v = new Vector3();
            Matrix matrix;
            Matrix.Invert( ref worldViewProjection, out matrix );

            v.X = ( ( ( vector.X - x ) / width ) * 2.0f ) - 1.0f;
            v.Y = -( ( ( ( vector.Y - y ) / height ) * 2.0f ) - 1.0f );
            v.Z = ( vector.Z - minZ ) / ( maxZ - minZ );

            TransformCoordinate( ref v, ref matrix, out v );
            return v;
        }
	
        public static void Unproject( ref Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, ref Matrix worldViewProjection, out Vector3 result )
        {
            Vector3 v = new Vector3();
            Matrix matrix;
            Matrix.Invert( ref worldViewProjection, out matrix );

            v.X = ( ( ( vector.X - x ) / width ) * 2.0f ) - 1.0f;
            v.Y = -( ( ( ( vector.Y - y ) / height ) * 2.0f ) - 1.0f );
            v.Z = ( vector.Z - minZ ) / ( maxZ - minZ );

            TransformCoordinate( ref v, ref matrix, out v );
            result = v;
        }
	
        public static Vector3 Minimize( Vector3 left, Vector3 right )
        {
            Vector3 vector;
            vector.X = (left.X < right.X) ? left.X : right.X;
            vector.Y = (left.Y < right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z < right.Z) ? left.Z : right.Z;
            return vector;
        }
	
        public static void Minimize( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = (left.X < right.X) ? left.X : right.X;
            result.Y = (left.Y < right.Y) ? left.Y : right.Y;
            result.Z = (left.Z < right.Z) ? left.Z : right.Z;
        }
	
        public static Vector3 Maximize( Vector3 left, Vector3 right )
        {
            Vector3 vector;
            vector.X = (left.X > right.X) ? left.X : right.X;
            vector.Y = (left.Y > right.Y) ? left.Y : right.Y;
            vector.Z = (left.Z > right.Z) ? left.Z : right.Z;
            return vector;
        }
	
        public static void Maximize( ref Vector3 left, ref Vector3 right, out Vector3 result )
        {
            result.X = (left.X > right.X) ? left.X : right.X;
            result.Y = (left.Y > right.Y) ? left.Y : right.Y;
            result.Z = (left.Z > right.Z) ? left.Z : right.Z;
        }
	
        public static Vector3 operator + ( Vector3 left, Vector3 right )
        {
            return new Vector3( left.X + right.X, left.Y + right.Y, left.Z + right.Z );
        }
	
        public static Vector3 operator - ( Vector3 left, Vector3 right )
        {
            return new Vector3( left.X - right.X, left.Y - right.Y, left.Z - right.Z );
        }
	
        public static Vector3 operator - ( Vector3 value )
        {
            return new Vector3( -value.X, -value.Y, -value.Z );
        }

        public static Vector3 operator * ( Vector3 value, float scale )
        {
            return new Vector3( value.X * scale, value.Y * scale, value.Z * scale );
        }

        public static Vector3 operator * ( float scale, Vector3 vec )
        {
            return vec * scale;
        }
	
        public static Vector3 operator / ( Vector3 value, float scale )
        {
            return new Vector3( value.X / scale, value.Y / scale, value.Z / scale );
        }
	
        public static bool operator == ( Vector3 left, Vector3 right )
        {
            return Equals( left, right );
        }
	
        public static bool operator != ( Vector3 left, Vector3 right )
        {
            return !Equals( left, right );
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2}", 
                X.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                Y.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                Z.ToString(System.Globalization.CultureInfo.CurrentCulture));
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode();
        }

        public override bool Equals( object value )
        {
            if( value == null )
                return false;

            if( value.GetType() != GetType() )
                return false;

            return Equals((Vector3)value);
        }

        public bool Equals(Vector3 value)
        {
            return ( X == value.X && Y == value.Y && Z == value.Z );
        }

        public static bool Equals( ref Vector3 value1, ref Vector3 value2 )
        {
            return ( value1.X == value2.X && value1.Y == value2.Y && value1.Z == value2.Z );
        }
    }
}
