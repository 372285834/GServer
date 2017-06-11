﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4 )]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.MatrixConverter) )]
	public struct Matrix : System.IEquatable<Matrix>
    {
        #region Member
        public float M11;

        public float M12;

        public float M13;

        public float M14;

        public float M21;

        public float M22;

        public float M23;

        public float M24;

        public float M31;

        public float M32;

        public float M33;

        public float M34;

        public float M41;

        public float M42;

        public float M43;

        public float M44;
        #endregion

        public float this[int row, int column]
        {
            get
	        {
		        if( row < 0 || row > 3 )
			        throw new ArgumentOutOfRangeException( "row", "Rows and columns for matrices run from 0 to 3, inclusive." );

		        if( column < 0 || column > 3 )
			        throw new ArgumentOutOfRangeException( "column", "Rows and columns for matrices run from 0 to 3, inclusive." );

		        int index = row * 4 + column;
		        switch( index )
		        {
		        case 0:  return M11;
		        case 1:  return M12;
		        case 2:  return M13;
		        case 3:  return M14;
		        case 4:  return M21;
		        case 5:  return M22;
		        case 6:  return M23;
		        case 7:  return M24;
		        case 8:  return M31;
		        case 9:  return M32;
		        case 10: return M33;
		        case 11: return M34;
		        case 12: return M41;
		        case 13: return M42;
		        case 14: return M43;
		        case 15: return M44;
		        }

		        return 0.0f;
	        }
	
	        set
	        {
		        if( row < 0 || row > 3 )
			        throw new ArgumentOutOfRangeException( "row", "Rows and columns for matrices run from 0 to 3, inclusive." );

		        if( column < 0 || column > 3 )
			        throw new ArgumentOutOfRangeException( "column", "Rows and columns for matrices run from 0 to 3, inclusive." );

		        int index = row * 4 + column;
		        switch( index )
		        {
		        case 0:  M11 = value; break;
		        case 1:  M12 = value; break;
		        case 2:  M13 = value; break;
		        case 3:  M14 = value; break;
		        case 4:  M21 = value; break;
		        case 5:  M22 = value; break;
		        case 6:  M23 = value; break;
		        case 7:  M24 = value; break;
		        case 8:  M31 = value; break;
		        case 9:  M32 = value; break;
		        case 10: M33 = value; break;
		        case 11: M34 = value; break;
		        case 12: M41 = value; break;
		        case 13: M42 = value; break;
		        case 14: M43 = value; break;
		        case 15: M44 = value; break;
		        }
	        }
        }

        public static Matrix mIdentity = InitStaticMatrix();
        public static Matrix InitStaticMatrix()
        {
            Matrix matrix = new Matrix();
            matrix.M11 = 1.0f;
            matrix.M22 = 1.0f;
            matrix.M33 = 1.0f;
            matrix.M44 = 1.0f;
            return matrix;
        }

        public static Matrix Identity
        {
            get
            {
                return mIdentity;
            }
        }

        #region Equal Overrride
        public override string ToString()
	    {
		    return string.Format( System.Globalization.CultureInfo.CurrentCulture, "[[M11:{0} M12:{1} M13:{2} M14:{3}] [M21:{4} M22:{5} M23:{6} M24:{7}] [M31:{8} M32:{9} M33:{10} M34:{11}] [M41:{12} M42:{13} M43:{14} M44:{15}]]",
			    M11.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M12.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M13.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M14.ToString(System.Globalization.CultureInfo.CurrentCulture),
			    M21.ToString(System.Globalization.CultureInfo.CurrentCulture),
                M22.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M23.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M24.ToString(System.Globalization.CultureInfo.CurrentCulture),
			    M31.ToString(System.Globalization.CultureInfo.CurrentCulture),
                M32.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M33.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M34.ToString(System.Globalization.CultureInfo.CurrentCulture),
			    M41.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M42.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M43.ToString(System.Globalization.CultureInfo.CurrentCulture), 
                M44.ToString(System.Globalization.CultureInfo.CurrentCulture) );
	    }

	    public override int GetHashCode()
	    {
		    return M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
			       M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
			       M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
			       M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Matrix)( value ) );
	    }

	    public bool Equals( Matrix value )
	    {
		    return ( M11 == value.M11 && M12 == value.M12 && M13 == value.M13 && M14 == value.M14 &&
				     M21 == value.M21 && M22 == value.M22 && M23 == value.M23 && M24 == value.M24 &&
				     M31 == value.M31 && M32 == value.M32 && M33 == value.M33 && M34 == value.M34 &&
				     M41 == value.M41 && M42 == value.M42 && M43 == value.M43 && M44 == value.M44 );
	    }

	    public static bool Equals( ref Matrix value1, ref Matrix value2 )
	    {
		    return ( value1.M11 == value2.M11 && value1.M12 == value2.M12 && value1.M13 == value2.M13 && value1.M14 == value2.M14 &&
				     value1.M21 == value2.M21 && value1.M22 == value2.M22 && value1.M23 == value2.M23 && value1.M24 == value2.M24 &&
				     value1.M31 == value2.M31 && value1.M32 == value2.M32 && value1.M33 == value2.M33 && value1.M34 == value2.M34 &&
				     value1.M41 == value2.M41 && value1.M42 == value2.M42 && value1.M43 == value2.M43 && value1.M44 == value2.M44 );
        }
        #endregion

        public float[] ToArray()
	    {
		    float[] result = new float[ 16 ];
		    result[0] = M11;
		    result[1] = M12;
		    result[2] = M13;
		    result[3] = M14;
		    result[4] = M21;
		    result[5] = M22;
		    result[6] = M23;
		    result[7] = M24;
		    result[8] = M31;
		    result[9] = M32;
		    result[10] = M33;
		    result[11] = M34;
		    result[12] = M41;
		    result[13] = M42;
		    result[14] = M43;
		    result[15] = M44;

		    return result;
	    }

	    public void Invert()
	    {
            unsafe
            {
                fixed (Matrix* pinnedThis = &this)
                {
                    IDllImportApi.D3DXMatrixInverse(pinnedThis, (float*)0, pinnedThis);
                }
            }
	    }

	    public bool Decompose( out Vector3 scale, out Quaternion rotation, out Vector3 translation )
	    {
		    unsafe
            {
                fixed (Vector3* plocalScale = &scale)
                {
                    fixed (Quaternion* plocalRot = &rotation)
                    {
                        fixed (Vector3* plocalTrans = &translation)
                        {
                            fixed (Matrix* pinnedThis = &this)
                            {
                                int hr = IDllImportApi.D3DXMatrixDecompose(plocalScale, plocalRot, plocalTrans, pinnedThis);
                                return hr == 0;
                            }
                        }
                    }
                }
            }
	    }

        public float Determinant()
	    {
		    float temp1 = (M33 * M44) - (M34 * M43);
		    float temp2 = (M32 * M44) - (M34 * M42);
		    float temp3 = (M32 * M43) - (M33 * M42);
		    float temp4 = (M31 * M44) - (M34 * M41);
		    float temp5 = (M31 * M43) - (M33 * M41);
		    float temp6 = (M31 * M42) - (M32 * M41);
		
		    return ((((M11 * (((M22 * temp1) - (M23 * temp2)) + (M24 * temp3))) - (M12 * (((M21 * temp1) - 
			    (M23 * temp4)) + (M24 * temp5)))) + (M13 * (((M21 * temp2) - (M22 * temp4)) + (M24 * temp6)))) - 
			    (M14 * (((M21 * temp3) - (M22 * temp5)) + (M23 * temp6))));
	    }

        public static Matrix Add(Matrix left, Matrix right)
	    {
		    Matrix result;
		    result.M11 = left.M11 + right.M11;
		    result.M12 = left.M12 + right.M12;
		    result.M13 = left.M13 + right.M13;
		    result.M14 = left.M14 + right.M14;
		    result.M21 = left.M21 + right.M21;
		    result.M22 = left.M22 + right.M22;
		    result.M23 = left.M23 + right.M23;
		    result.M24 = left.M24 + right.M24;
		    result.M31 = left.M31 + right.M31;
		    result.M32 = left.M32 + right.M32;
		    result.M33 = left.M33 + right.M33;
		    result.M34 = left.M34 + right.M34;
		    result.M41 = left.M41 + right.M41;
		    result.M42 = left.M42 + right.M42;
		    result.M43 = left.M43 + right.M43;
		    result.M44 = left.M44 + right.M44;
		    return result;
	    }

        public static void Add(ref Matrix left, ref Matrix right, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = left.M11 + right.M11;
		    r.M12 = left.M12 + right.M12;
		    r.M13 = left.M13 + right.M13;
		    r.M14 = left.M14 + right.M14;
		    r.M21 = left.M21 + right.M21;
		    r.M22 = left.M22 + right.M22;
		    r.M23 = left.M23 + right.M23;
		    r.M24 = left.M24 + right.M24;
		    r.M31 = left.M31 + right.M31;
		    r.M32 = left.M32 + right.M32;
		    r.M33 = left.M33 + right.M33;
		    r.M34 = left.M34 + right.M34;
		    r.M41 = left.M41 + right.M41;
		    r.M42 = left.M42 + right.M42;
		    r.M43 = left.M43 + right.M43;
		    r.M44 = left.M44 + right.M44;

		    result = r;
	    }

        public static Matrix Subtract(Matrix left, Matrix right)
	    {
		    Matrix result;
		    result.M11 = left.M11 - right.M11;
		    result.M12 = left.M12 - right.M12;
		    result.M13 = left.M13 - right.M13;
		    result.M14 = left.M14 - right.M14;
		    result.M21 = left.M21 - right.M21;
		    result.M22 = left.M22 - right.M22;
		    result.M23 = left.M23 - right.M23;
		    result.M24 = left.M24 - right.M24;
		    result.M31 = left.M31 - right.M31;
		    result.M32 = left.M32 - right.M32;
		    result.M33 = left.M33 - right.M33;
		    result.M34 = left.M34 - right.M34;
		    result.M41 = left.M41 - right.M41;
		    result.M42 = left.M42 - right.M42;
		    result.M43 = left.M43 - right.M43;
		    result.M44 = left.M44 - right.M44;
		    return result;
	    }

        public static void Subtract(ref Matrix left, ref Matrix right, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = left.M11 - right.M11;
		    r.M12 = left.M12 - right.M12;
		    r.M13 = left.M13 - right.M13;
		    r.M14 = left.M14 - right.M14;
		    r.M21 = left.M21 - right.M21;
		    r.M22 = left.M22 - right.M22;
		    r.M23 = left.M23 - right.M23;
		    r.M24 = left.M24 - right.M24;
		    r.M31 = left.M31 - right.M31;
		    r.M32 = left.M32 - right.M32;
		    r.M33 = left.M33 - right.M33;
		    r.M34 = left.M34 - right.M34;
		    r.M41 = left.M41 - right.M41;
		    r.M42 = left.M42 - right.M42;
		    r.M43 = left.M43 - right.M43;
		    r.M44 = left.M44 - right.M44;

		    result = r;
	    }

        public static Matrix Negate(Matrix matrix)
	    {
		    Matrix result;
		    result.M11 = -matrix.M11;
		    result.M12 = -matrix.M12;
		    result.M13 = -matrix.M13;
		    result.M14 = -matrix.M14;
		    result.M21 = -matrix.M21;
		    result.M22 = -matrix.M22;
		    result.M23 = -matrix.M23;
		    result.M24 = -matrix.M24;
		    result.M31 = -matrix.M31;
		    result.M32 = -matrix.M32;
		    result.M33 = -matrix.M33;
		    result.M34 = -matrix.M34;
		    result.M41 = -matrix.M41;
		    result.M42 = -matrix.M42;
		    result.M43 = -matrix.M43;
		    result.M44 = -matrix.M44;
		    return result;
	    }

        public static void Negate(ref Matrix matrix, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = -matrix.M11;
		    r.M12 = -matrix.M12;
		    r.M13 = -matrix.M13;
		    r.M14 = -matrix.M14;
		    r.M21 = -matrix.M21;
		    r.M22 = -matrix.M22;
		    r.M23 = -matrix.M23;
		    r.M24 = -matrix.M24;
		    r.M31 = -matrix.M31;
		    r.M32 = -matrix.M32;
		    r.M33 = -matrix.M33;
		    r.M34 = -matrix.M34;
		    r.M41 = -matrix.M41;
		    r.M42 = -matrix.M42;
		    r.M43 = -matrix.M43;
		    r.M44 = -matrix.M44;

		    result = r;
	    }

        public static Matrix Multiply(Matrix left, Matrix right)
	    {
		    Matrix result;
		    result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
		    result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
		    result.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
		    result.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
		    result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
		    result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
		    result.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
		    result.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
		    result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
		    result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
		    result.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
		    result.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
		    result.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
		    result.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
		    result.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
		    result.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
		    return result;
	    }

        public static void Multiply(ref Matrix left, ref Matrix right, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
		    r.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
		    r.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
		    r.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
		    r.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
		    r.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
		    r.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
		    r.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
		    r.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
		    r.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
		    r.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
		    r.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
		    r.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
		    r.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
		    r.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
		    r.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
	
		    result = r;
	    }

	    public unsafe static void Multiply( Matrix* left, Matrix* right, Matrix* result, int count )
	    {
		    for( int i = 0; i < count; ++i )
		    {
			    IDllImportApi.D3DXMatrixMultiply( &left[i] ,
				    &right[i] ,
				    &result[i] );
		    }
	    }

        public static void Multiply(Matrix[] left, Matrix[] right, Matrix[] result, int offset, int count)
	    {
		    if( left.Length != right.Length )
			    throw new ArgumentException( "Left and right arrays must be the same size.", "right" );
		    if( right.Length != result.Length )
			    throw new ArgumentException( "Result array must be the same size as input arrays.", "result" );
		    //Utilities.CheckArrayBounds( left, offset, count );

            unsafe
            {
                fixed (Matrix* pinnedLeft = &left[offset])
                {
                    fixed (Matrix* pinnedRight = &right[offset])
                    {
                        fixed (Matrix* pinnedResult = &result[offset])
                        {
                            Multiply(pinnedLeft, pinnedRight, pinnedResult, count);
                        }
                    }
                }
            }
	    }

	    public static void Multiply( Matrix[] left, Matrix right, Matrix[] result, int offset, int count )
	    {
		    if( left.Length != result.Length )
			    throw new ArgumentException( "Result array must be the same size as the input array.", "result" );
		    //Utilities.CheckArrayBounds( left, offset, count );

            unsafe
            {
                fixed(Matrix* pinnedLeft = &left[offset])
                {
		            fixed(Matrix* pinnedResult = &result[offset])
                    {
                        for( int i = 0; i < count; ++i )
		                {
			                IDllImportApi.D3DXMatrixMultiply( &pinnedLeft[i] ,
				                &right ,
				                &pinnedResult[i] );
		                }
                    }
                }
            }
	    }

        public static Matrix Multiply(Matrix left, float right)
	    {
		    Matrix result;
		    result.M11 = left.M11 * right;
		    result.M12 = left.M12 * right;
		    result.M13 = left.M13 * right;
		    result.M14 = left.M14 * right;
		    result.M21 = left.M21 * right;
		    result.M22 = left.M22 * right;
		    result.M23 = left.M23 * right;
		    result.M24 = left.M24 * right;
		    result.M31 = left.M31 * right;
		    result.M32 = left.M32 * right;
		    result.M33 = left.M33 * right;
		    result.M34 = left.M34 * right;
		    result.M41 = left.M41 * right;
		    result.M42 = left.M42 * right;
		    result.M43 = left.M43 * right;
		    result.M44 = left.M44 * right;
		    return result;
	    }

        public static void Multiply(ref Matrix left, float right, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = left.M11 * right;
		    r.M12 = left.M12 * right;
		    r.M13 = left.M13 * right;
		    r.M14 = left.M14 * right;
		    r.M21 = left.M21 * right;
		    r.M22 = left.M22 * right;
		    r.M23 = left.M23 * right;
		    r.M24 = left.M24 * right;
		    r.M31 = left.M31 * right;
		    r.M32 = left.M32 * right;
		    r.M33 = left.M33 * right;
		    r.M34 = left.M34 * right;
		    r.M41 = left.M41 * right;
		    r.M42 = left.M42 * right;
		    r.M43 = left.M43 * right;
		    r.M44 = left.M44 * right;

		    result = r;
	    }

        public static Matrix Divide(Matrix left, Matrix right)
	    {
		    Matrix result;
		    result.M11 = left.M11 / right.M11;
		    result.M12 = left.M12 / right.M12;
		    result.M13 = left.M13 / right.M13;
		    result.M14 = left.M14 / right.M14;
		    result.M21 = left.M21 / right.M21;
		    result.M22 = left.M22 / right.M22;
		    result.M23 = left.M23 / right.M23;
		    result.M24 = left.M24 / right.M24;
		    result.M31 = left.M31 / right.M31;
		    result.M32 = left.M32 / right.M32;
		    result.M33 = left.M33 / right.M33;
		    result.M34 = left.M34 / right.M34;
		    result.M41 = left.M41 / right.M41;
		    result.M42 = left.M42 / right.M42;
		    result.M43 = left.M43 / right.M43;
		    result.M44 = left.M44 / right.M44;
		    return result;
	    }

        public static void Divide(ref Matrix left, ref Matrix right, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = left.M11 / right.M11;
		    r.M12 = left.M12 / right.M12;
		    r.M13 = left.M13 / right.M13;
		    r.M14 = left.M14 / right.M14;
		    r.M21 = left.M21 / right.M21;
		    r.M22 = left.M22 / right.M22;
		    r.M23 = left.M23 / right.M23;
		    r.M24 = left.M24 / right.M24;
		    r.M31 = left.M31 / right.M31;
		    r.M32 = left.M32 / right.M32;
		    r.M33 = left.M33 / right.M33;
		    r.M34 = left.M34 / right.M34;
		    r.M41 = left.M41 / right.M41;
		    r.M42 = left.M42 / right.M42;
		    r.M43 = left.M43 / right.M43;
		    r.M44 = left.M44 / right.M44;

		    result = r;
	    }

        public static Matrix Divide(Matrix left, float right)
	    {
		    Matrix result;
		    float inv = 1.0f / right;

		    result.M11 = left.M11 * inv;
		    result.M12 = left.M12 * inv;
		    result.M13 = left.M13 * inv;
		    result.M14 = left.M14 * inv;
		    result.M21 = left.M21 * inv;
		    result.M22 = left.M22 * inv;
		    result.M23 = left.M23 * inv;
		    result.M24 = left.M24 * inv;
		    result.M31 = left.M31 * inv;
		    result.M32 = left.M32 * inv;
		    result.M33 = left.M33 * inv;
		    result.M34 = left.M34 * inv;
		    result.M41 = left.M41 * inv;
		    result.M42 = left.M42 * inv;
		    result.M43 = left.M43 * inv;
		    result.M44 = left.M44 * inv;
		    return result;
	    }

        public static void Divide(ref Matrix left, float right, out Matrix result)
	    {
		    float inv = 1.0f / right;

		    Matrix r;
		    r.M11 = left.M11 * inv;
		    r.M12 = left.M12 * inv;
		    r.M13 = left.M13 * inv;
		    r.M14 = left.M14 * inv;
		    r.M21 = left.M21 * inv;
		    r.M22 = left.M22 * inv;
		    r.M23 = left.M23 * inv;
		    r.M24 = left.M24 * inv;
		    r.M31 = left.M31 * inv;
		    r.M32 = left.M32 * inv;
		    r.M33 = left.M33 * inv;
		    r.M34 = left.M34 * inv;
		    r.M41 = left.M41 * inv;
		    r.M42 = left.M42 * inv;
		    r.M43 = left.M43 * inv;
		    r.M44 = left.M44 * inv;

		    result = r;
	    }

        public static Matrix Lerp(Matrix value1, Matrix value2, float amount)
	    {
		    Matrix result;
		    result.M11 = value1.M11 + ((value2.M11 - value1.M11) * amount);
		    result.M12 = value1.M12 + ((value2.M12 - value1.M12) * amount);
		    result.M13 = value1.M13 + ((value2.M13 - value1.M13) * amount);
		    result.M14 = value1.M14 + ((value2.M14 - value1.M14) * amount);
		    result.M21 = value1.M21 + ((value2.M21 - value1.M21) * amount);
		    result.M22 = value1.M22 + ((value2.M22 - value1.M22) * amount);
		    result.M23 = value1.M23 + ((value2.M23 - value1.M23) * amount);
		    result.M24 = value1.M24 + ((value2.M24 - value1.M24) * amount);
		    result.M31 = value1.M31 + ((value2.M31 - value1.M31) * amount);
		    result.M32 = value1.M32 + ((value2.M32 - value1.M32) * amount);
		    result.M33 = value1.M33 + ((value2.M33 - value1.M33) * amount);
		    result.M34 = value1.M34 + ((value2.M34 - value1.M34) * amount);
		    result.M41 = value1.M41 + ((value2.M41 - value1.M41) * amount);
		    result.M42 = value1.M42 + ((value2.M42 - value1.M42) * amount);
		    result.M43 = value1.M43 + ((value2.M43 - value1.M43) * amount);
		    result.M44 = value1.M44 + ((value2.M44 - value1.M44) * amount);
		    return result;
	    }

        public static void Lerp(ref Matrix value1, ref Matrix value2, float amount, out Matrix result)
	    {
		    Matrix r;
		    r.M11 = value1.M11 + ((value2.M11 - value1.M11) * amount);
		    r.M12 = value1.M12 + ((value2.M12 - value1.M12) * amount);
		    r.M13 = value1.M13 + ((value2.M13 - value1.M13) * amount);
		    r.M14 = value1.M14 + ((value2.M14 - value1.M14) * amount);
		    r.M21 = value1.M21 + ((value2.M21 - value1.M21) * amount);
		    r.M22 = value1.M22 + ((value2.M22 - value1.M22) * amount);
		    r.M23 = value1.M23 + ((value2.M23 - value1.M23) * amount);
		    r.M24 = value1.M24 + ((value2.M24 - value1.M24) * amount);
		    r.M31 = value1.M31 + ((value2.M31 - value1.M31) * amount);
		    r.M32 = value1.M32 + ((value2.M32 - value1.M32) * amount);
		    r.M33 = value1.M33 + ((value2.M33 - value1.M33) * amount);
		    r.M34 = value1.M34 + ((value2.M34 - value1.M34) * amount);
		    r.M41 = value1.M41 + ((value2.M41 - value1.M41) * amount);
		    r.M42 = value1.M42 + ((value2.M42 - value1.M42) * amount);
		    r.M43 = value1.M43 + ((value2.M43 - value1.M43) * amount);
		    r.M44 = value1.M44 + ((value2.M44 - value1.M44) * amount);

		    result = r;
	    }

	    public static Matrix Billboard( Vector3 objectPosition, Vector3 cameraPosition, Vector3 cameraUpVector, Vector3 cameraForwardVector )
	    {
		    Matrix result = new Matrix();
		    Vector3 difference = objectPosition - cameraPosition;
		    Vector3 crossed = new Vector3();
		    Vector3 final = new Vector3();

		    float lengthSq = difference.LengthSquared();
		    if (lengthSq < 0.0001f)
			    difference = -cameraForwardVector;
		    else
			    difference *= (float)( 1.0f / Math.Sqrt( lengthSq ) );

            Vector3.Cross(ref cameraUpVector, ref difference, out crossed);
		    crossed.Normalize();
            Vector3.Cross(ref difference, ref crossed, out final);

		    result.M11 = crossed.X;
		    result.M12 = crossed.Y;
		    result.M13 = crossed.Z;
		    result.M14 = 0.0f;
		    result.M21 = final.X;
		    result.M22 = final.Y;
		    result.M23 = final.Z;
		    result.M24 = 0.0f;
		    result.M31 = difference.X;
		    result.M32 = difference.Y;
		    result.M33 = difference.Z;
		    result.M34 = 0.0f;
		    result.M41 = objectPosition.X;
		    result.M42 = objectPosition.Y;
		    result.M43 = objectPosition.Z;
		    result.M44 = 1.0f;

		    return result;
	    }

	    public static void Billboard( ref Vector3 objectPosition, ref Vector3 cameraPosition, ref Vector3 cameraUpVector, ref Vector3 cameraForwardVector, out Matrix result )
	    {
		    Vector3 difference = objectPosition - cameraPosition;
		    Vector3 crossed = new Vector3();
		    Vector3 final = new Vector3();

		    float lengthSq = difference.LengthSquared();
		    if (lengthSq < 0.0001f)
			    difference = -cameraForwardVector;
		    else
			    difference *= (float)( 1.0f / Math.Sqrt( lengthSq ) );

            Vector3.Cross(ref cameraUpVector, ref difference, out crossed);
		    crossed.Normalize();
            Vector3.Cross(ref difference, ref crossed, out final);

		    result.M11 = crossed.X;
		    result.M12 = crossed.Y;
		    result.M13 = crossed.Z;
		    result.M14 = 0.0f;
		    result.M21 = final.X;
		    result.M22 = final.Y;
		    result.M23 = final.Z;
		    result.M24 = 0.0f;
		    result.M31 = difference.X;
		    result.M32 = difference.Y;
		    result.M33 = difference.Z;
		    result.M34 = 0.0f;
		    result.M41 = objectPosition.X;
		    result.M42 = objectPosition.Y;
		    result.M43 = objectPosition.Z;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationX(float angle)
	    {
		    Matrix result;
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = 1.0f;
		    result.M12 = 0.0f;
		    result.M13 = 0.0f;
		    result.M14 = 0.0f;
		    result.M21 = 0.0f;
		    result.M22 = cos;
		    result.M23 = sin;
		    result.M24 = 0.0f;
		    result.M31 = 0.0f;
		    result.M32 = -sin;
		    result.M33 = cos;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;

		    //D3DXMatrixRotationX((D3DXMATRIX*)&result, angle);

		    return result;
	    }

        public static void RotationX(float angle, out Matrix result)
	    {
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = 1.0f;
		    result.M12 = 0.0f;
		    result.M13 = 0.0f;
		    result.M14 = 0.0f;
		    result.M21 = 0.0f;
		    result.M22 = cos;
		    result.M23 = sin;
		    result.M24 = 0.0f;
		    result.M31 = 0.0f;
		    result.M32 = -sin;
		    result.M33 = cos;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationY(float angle)
	    {
		    Matrix result;
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = cos;
		    result.M12 = 0.0f;
		    result.M13 = -sin;
		    result.M14 = 0.0f;
		    result.M21 = 0.0f;
		    result.M22 = 1.0f;
		    result.M23 = 0.0f;
		    result.M24 = 0.0f;
		    result.M31 = sin;
		    result.M32 = 0.0f;
		    result.M33 = cos;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;

		    //D3DXMatrixRotationY((D3DXMATRIX*)&result, angle);

		    return result;
	    }

        public static void RotationY(float angle, out Matrix result)
	    {
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = cos;
		    result.M12 = 0.0f;
		    result.M13 = -sin;
		    result.M14 = 0.0f;
		    result.M21 = 0.0f;
		    result.M22 = 1.0f;
		    result.M23 = 0.0f;
		    result.M24 = 0.0f;
		    result.M31 = sin;
		    result.M32 = 0.0f;
		    result.M33 = cos;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationZ(float angle)
	    {
		    Matrix result;
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = cos;
		    result.M12 = sin;
		    result.M13 = 0.0f;
		    result.M14 = 0.0f;
		    result.M21 = -sin;
		    result.M22 = cos;
		    result.M23 = 0.0f;
		    result.M24 = 0.0f;
		    result.M31 = 0.0f;
		    result.M32 = 0.0f;
		    result.M33 = 1.0f;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;

		    //D3DXMatrixRotationZ((D3DXMATRIX*)&result, angle);

		    return result;
	    }

        public static void RotationZ(float angle, out Matrix result)
	    {
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );

		    result.M11 = cos;
		    result.M12 = sin;
		    result.M13 = 0.0f;
		    result.M14 = 0.0f;
		    result.M21 = -sin;
		    result.M22 = cos;
		    result.M23 = 0.0f;
		    result.M24 = 0.0f;
		    result.M31 = 0.0f;
		    result.M32 = 0.0f;
		    result.M33 = 1.0f;
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationQuaternion(Quaternion quaternion)
	    {
		    Matrix result;

		    float xx = quaternion.X * quaternion.X;
		    float yy = quaternion.Y * quaternion.Y;
		    float zz = quaternion.Z * quaternion.Z;
		    float xy = quaternion.X * quaternion.Y;
		    float zw = quaternion.Z * quaternion.W;
		    float zx = quaternion.Z * quaternion.X;
		    float yw = quaternion.Y * quaternion.W;
		    float yz = quaternion.Y * quaternion.Z;
		    float xw = quaternion.X * quaternion.W;
		    result.M11 = 1.0f - (2.0f * (yy + zz));
		    result.M12 = 2.0f * (xy + zw);
		    result.M13 = 2.0f * (zx - yw);
		    result.M14 = 0.0f;
		    result.M21 = 2.0f * (xy - zw);
		    result.M22 = 1.0f - (2.0f * (zz + xx));
		    result.M23 = 2.0f * (yz + xw);
		    result.M24 = 0.0f;
		    result.M31 = 2.0f * (zx + yw);
		    result.M32 = 2.0f * (yz - xw);
		    result.M33 = 1.0f - (2.0f * (yy + xx));
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;

		    //D3DXMatrixRotationQuaternion((D3DXMATRIX*)&result, (D3DXQUATERNION*)&quaternion);

		    return result;
	    }

        public static void RotationQuaternion(ref Quaternion rotation, out Matrix result)
	    {
		    float xx = rotation.X * rotation.X;
		    float yy = rotation.Y * rotation.Y;
		    float zz = rotation.Z * rotation.Z;
		    float xy = rotation.X * rotation.Y;
		    float zw = rotation.Z * rotation.W;
		    float zx = rotation.Z * rotation.X;
		    float yw = rotation.Y * rotation.W;
		    float yz = rotation.Y * rotation.Z;
		    float xw = rotation.X * rotation.W;
		    result.M11 = 1.0f - (2.0f * (yy + zz));
		    result.M12 = 2.0f * (xy + zw);
		    result.M13 = 2.0f * (zx - yw);
		    result.M14 = 0.0f;
		    result.M21 = 2.0f * (xy - zw);
		    result.M22 = 1.0f - (2.0f * (zz + xx));
		    result.M23 = 2.0f * (yz + xw);
		    result.M24 = 0.0f;
		    result.M31 = 2.0f * (zx + yw);
		    result.M32 = 2.0f * (yz - xw);
		    result.M33 = 1.0f - (2.0f * (yy + xx));
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationAxis(Vector3 axis, float angle)
	    {
		    if( axis.LengthSquared() != 1.0f )
			    axis.Normalize();

		    Matrix result;
		    float x = axis.X;
		    float y = axis.Y;
		    float z = axis.Z;
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );
		    float xx = x * x;
		    float yy = y * y;
		    float zz = z * z;
		    float xy = x * y;
		    float xz = x * z;
		    float yz = y * z;

		    result.M11 = xx + (cos * (1.0f - xx));
		    result.M12 = (xy - (cos * xy)) + (sin * z);
		    result.M13 = (xz - (cos * xz)) - (sin * y);
		    result.M14 = 0.0f;
		    result.M21 = (xy - (cos * xy)) - (sin * z);
		    result.M22 = yy + (cos * (1.0f - yy));
		    result.M23 = (yz - (cos * yz)) + (sin * x);
		    result.M24 = 0.0f;
		    result.M31 = (xz - (cos * xz)) + (sin * y);
		    result.M32 = (yz - (cos * yz)) - (sin * x);
		    result.M33 = zz + (cos * (1.0f - zz));
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;

		    return result;
	    }

        public static void RotationAxis(ref Vector3 axis, float angle, out Matrix result)
	    {
		    if( axis.LengthSquared() != 1.0f )
			    axis.Normalize();

		    float x = axis.X;
		    float y = axis.Y;
		    float z = axis.Z;
		    float cos = (float)( Math.Cos( (double)( angle ) ) );
		    float sin = (float)( Math.Sin( (double)( angle ) ) );
		    float xx = x * x;
		    float yy = y * y;
		    float zz = z * z;
		    float xy = x * y;
		    float xz = x * z;
		    float yz = y * z;

		    result.M11 = xx + (cos * (1.0f - xx));
		    result.M12 = (xy - (cos * xy)) + (sin * z);
		    result.M13 = (xz - (cos * xz)) - (sin * y);
		    result.M14 = 0.0f;
		    result.M21 = (xy - (cos * xy)) - (sin * z);
		    result.M22 = yy + (cos * (1.0f - yy));
		    result.M23 = (yz - (cos * yz)) + (sin * x);
		    result.M24 = 0.0f;
		    result.M31 = (xz - (cos * xz)) + (sin * y);
		    result.M32 = (yz - (cos * yz)) - (sin * x);
		    result.M33 = zz + (cos * (1.0f - zz));
		    result.M34 = 0.0f;
		    result.M41 = 0.0f;
		    result.M42 = 0.0f;
		    result.M43 = 0.0f;
		    result.M44 = 1.0f;
	    }

        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Matrix result = new Matrix();
            Quaternion quaternion = new Quaternion();
            Quaternion.RotationYawPitchRoll(yaw, pitch, roll, out quaternion);
            RotationQuaternion(ref quaternion, out result);
            return result;
        }

        public static void RotationYawPitchRoll(float yaw, float pitch, float roll, out Matrix result)
        {
            Quaternion quaternion = new Quaternion();
            Quaternion.RotationYawPitchRoll(yaw, pitch, roll, out quaternion);
            RotationQuaternion(ref quaternion, out result);
        }

        public static Matrix Translation(float x, float y, float z)
        {
            Matrix result;
            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1.0f;
            return result;
        }

        public static void Translation(float x, float y, float z, out Matrix result)
        {
            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = x;
            result.M42 = y;
            result.M43 = z;
            result.M44 = 1.0f;
        }

        public static Matrix Translation(Vector3 translation)
        {
            Matrix result;
            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = translation.X;
            result.M42 = translation.Y;
            result.M43 = translation.Z;
            result.M44 = 1.0f;
            return result;
        }

        public static void Translation(ref Vector3 translation, out Matrix result)
        {
            result.M11 = 1.0f;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = 1.0f;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = 1.0f;
            result.M34 = 0.0f;
            result.M41 = translation.X;
            result.M42 = translation.Y;
            result.M43 = translation.Z;
            result.M44 = 1.0f;
        }

        public static Matrix Scaling(float x, float y, float z)
        {
            Matrix result;
            result.M11 = x;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = y;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = z;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
            return result;
        }

        public static void Scaling(float x, float y, float z, out Matrix result)
        {
            result.M11 = x;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = y;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = z;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        public static Matrix Scaling(Vector3 scaling)
        {
            Matrix result;
            result.M11 = scaling.X;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = scaling.Y;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = scaling.Z;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
            return result;
        }

        public static void Scaling(ref Vector3 scaling, out Matrix result)
        {
            result.M11 = scaling.X;
            result.M12 = 0.0f;
            result.M13 = 0.0f;
            result.M14 = 0.0f;
            result.M21 = 0.0f;
            result.M22 = scaling.Y;
            result.M23 = 0.0f;
            result.M24 = 0.0f;
            result.M31 = 0.0f;
            result.M32 = 0.0f;
            result.M33 = scaling.Z;
            result.M34 = 0.0f;
            result.M41 = 0.0f;
            result.M42 = 0.0f;
            result.M43 = 0.0f;
            result.M44 = 1.0f;
        }

        public static Matrix AffineTransformation(float scaling, Vector3 rotationCenter, Quaternion rotation, Vector3 translation)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixAffineTransformation((Matrix*)&result, scaling, (Vector3*)&rotationCenter, (Quaternion*)&rotation, (Vector3*)&translation);
            }
            return result;
        }

        public static void AffineTransformation(float scaling, ref Vector3 rotationCenter, ref Quaternion rotation, ref Vector3 translation, out Matrix result)
        {
            unsafe
            {
                fixed (Vector3* pinRotationCenter = &rotationCenter)
                {
                    fixed(Quaternion* pinRotation = &rotation)
                    {
                        fixed(Vector3* pinTranslation = &translation)
                        {
                            fixed (Matrix* pinResult = &result)
                            {
                                IDllImportApi.D3DXMatrixAffineTransformation(pinResult, scaling, pinRotationCenter, pinRotation, pinTranslation);
                            }
                        }
                    }
                }
            }
        }

        public static Matrix AffineTransformation2D(float scaling, Vector2 rotationCenter, float rotation, Vector2 translation)
        {
            Matrix result;
            unsafe
            {
                IDllImportApi.D3DXMatrixAffineTransformation2D((Matrix*)&result, scaling, (Vector2*)&rotationCenter, rotation, (Vector2*)&translation);
            }
            return result;
        }

        public static void AffineTransformation2D(float scaling, ref Vector2 rotationCenter, float rotation, ref Vector2 translation, out Matrix result)
        {
            unsafe
            {
                fixed (Vector2* pinRotationCenter = &rotationCenter)
                {
                    fixed(Vector2* pinTranslation = &translation)
                    {
                        fixed (Matrix* pinResult = &result)
                        {
                            IDllImportApi.D3DXMatrixAffineTransformation2D(pinResult, scaling, pinRotationCenter, rotation, pinTranslation);
                        }
                    }
                }
            }
        }

        public static Matrix Transformation(Vector3 scalingCenter, Quaternion scalingRotation, Vector3 scaling, Vector3 rotationCenter, Quaternion rotation, Vector3 translation)
        {
            Matrix result;
            unsafe
            {
                IDllImportApi.D3DXMatrixTransformation((Matrix*)&result, (Vector3*)&scalingCenter, (Quaternion*)&scalingRotation, (Vector3*)&scaling, (Vector3*)&rotationCenter, (Quaternion*)&rotation, (Vector3*)&translation);
            }
            return result;
        }

        public static void Transformation(ref Vector3 scalingCenter, ref Quaternion scalingRotation, ref Vector3 scaling, ref Vector3 rotationCenter, ref Quaternion rotation, ref Vector3 translation, out Matrix result)
        {
            unsafe
            {
                fixed(Vector3* pinScalingCenter = &scalingCenter)
                {
                    fixed(Quaternion* pinScalingRotation = &scalingRotation)
                    {
                        fixed(Vector3* pinScaling = &scaling)
                        {
                            fixed(Vector3* pinRotationCenter = &rotationCenter)
                            {
                                fixed(Quaternion* pinRotation = &rotation)
                                {
                                    fixed(Vector3* pinTranslation = &translation)
                                    {
                                        fixed(Matrix* pinResult = &result)
                                        {
                                            IDllImportApi.D3DXMatrixTransformation(pinResult, pinScalingCenter, pinScalingRotation,pinScaling, pinRotationCenter, pinRotation, pinTranslation);
                                        }
                                    }            
                                }            
                            }            
                        }            
                    }
                }
            }
            
            
        }

        public static Matrix Transformation2D(Vector2 scalingCenter, float scalingRotation, Vector2 scaling, Vector2 rotationCenter, float rotation, Vector2 translation)
        {
            Matrix result;
            unsafe
            {
                IDllImportApi.D3DXMatrixTransformation2D((Matrix*)&result, (Vector2*)&scalingCenter, scalingRotation, (Vector2*)&scaling, (Vector2*)&rotationCenter, rotation, (Vector2*)&translation);
            }
            
            return result;
        }

        public static void Transformation2D(ref Vector2 scalingCenter, float scalingRotation, ref Vector2 scaling, ref Vector2 rotationCenter, float rotation, ref Vector2 translation, out Matrix result)
        {
            unsafe
            {
                fixed(Vector2* pinScalingCenter = &scalingCenter)
                {
                    fixed(Vector2* pinScaling = &scaling)
                    {
                        fixed(Vector2* pinRotationCenter = &rotationCenter)
                        {
                            fixed(Vector2* pinTranslation = &translation)
                            {
                                fixed (Matrix* pinResult = &result)
                                {
                                    IDllImportApi.D3DXMatrixTransformation2D(pinResult, pinScalingCenter, scalingRotation,
                                        pinScaling, pinRotationCenter, rotation, pinTranslation);                        
                                }
                            }
                        }
                    }
                }
            }
            
            
        }

        public static Matrix LookAtLH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixLookAtLH((Matrix*)&result, (Vector3*)&eye, (Vector3*)&target, (Vector3*)&up);
            }
            return result;
        }

        public static void LookAtLH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
        {
            unsafe
            {
                fixed(Vector3* pinEye = &eye)
                {
                    fixed(Vector3* pinTarget = &target)
                    {
                        fixed(Vector3* pinUp = &up)
                        {
                            fixed(Matrix* pinResult = &result)
                            {
                                IDllImportApi.D3DXMatrixLookAtLH(pinResult, pinEye, pinTarget, pinUp);
                            }
                        }
                    }
                }
            }
        }

        public static Matrix LookAtRH(Vector3 eye, Vector3 target, Vector3 up)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixLookAtRH((Matrix*)&result, (Vector3*)&eye, (Vector3*)&target, (Vector3*)&up);
            }
            return result;
        }

        public static void LookAtRH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
        {
            unsafe
            {
                fixed (Vector3* pinEye = &eye)
                {
                    fixed (Vector3* pinTarget = &target)
                    {
                        fixed (Vector3* pinUp = &up)
                        {
                            fixed (Matrix* pinResult = &result)
                            {
                                IDllImportApi.D3DXMatrixLookAtRH(pinResult, pinEye, pinTarget, pinUp);
                            }
                        }
                    }
                }
            }
        }

        public static Matrix OrthoLH(float width, float height, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixOrthoLH((Matrix*)&result, width, height, znear, zfar);
            }
            
            return result;
        }

        public static void OrthoLH(float width, float height, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixOrthoLH(pinResult, width, height, znear, zfar);
                }
            }
        }

        public static Matrix OrthoRH(float width, float height, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixOrthoRH((Matrix*)&result, width, height, znear, zfar);
            }

            return result;
        }

        public static void OrthoRH(float width, float height, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixOrthoRH(pinResult, width, height, znear, zfar);
                }
            }
        }

        public static Matrix OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixOrthoOffCenterLH((Matrix*)&result, left, right, bottom, top, znear, zfar);
            }            
            return result;
        }

        public static void OrthoOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixOrthoOffCenterLH(pinResult, left, right, bottom, top, znear, zfar);
                }
            }            
        }

        public static Matrix OrthoOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixOrthoOffCenterRH((Matrix*)&result, left, right, bottom, top, znear, zfar);
            }
            return result;
        }

        public static void OrthoOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixOrthoOffCenterRH(pinResult, left, right, bottom, top, znear, zfar);
                }
            } 
        }

        public static Matrix PerspectiveLH(float width, float height, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveLH((Matrix*)&result, width, height, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveLH(float width, float height, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveLH(pinResult, width, height, znear, zfar);
                }
            } 
        }

        public static Matrix PerspectiveRH(float width, float height, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveRH((Matrix*)&result, width, height, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveRH(float width, float height, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveRH(pinResult, width, height, znear, zfar);
                }
            }
        }

        public static Matrix PerspectiveFovLH(float fov, float aspect, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveFovLH((Matrix*)&result, fov, aspect, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveFovLH(float fov, float aspect, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveFovLH(pinResult, fov, aspect, znear, zfar);
                }
            }
        }

        public static Matrix PerspectiveFovRH(float fov, float aspect, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveFovRH((Matrix*)&result, fov, aspect, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveFovRH(float fov, float aspect, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveFovRH(pinResult, fov, aspect, znear, zfar);
                }
            }
        }

        public static Matrix PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveOffCenterLH(&result, left, right, bottom, top, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveOffCenterLH(pinResult, left, right, bottom, top, znear, zfar);
                }
            }
        }

        public static Matrix PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            Matrix result = new Matrix();
            unsafe
            {
                IDllImportApi.D3DXMatrixPerspectiveOffCenterRH(&result, left, right, bottom, top, znear, zfar);
            }
            return result;
        }

        public static void PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pinResult = &result)
                {
                    IDllImportApi.D3DXMatrixPerspectiveOffCenterRH(pinResult, left, right, bottom, top, znear, zfar);
                }
            }
        }

        public static Matrix Reflection(Plane plane)
        {
            Matrix result;
            plane.Normalize();
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float x2 = -2.0f * x;
            float y2 = -2.0f * y;
            float z2 = -2.0f * z;
            result.M11 = (x2 * x) + 1.0f;
            result.M12 = y2 * x;
            result.M13 = z2 * x;
            result.M14 = 0.0f;
            result.M21 = x2 * y;
            result.M22 = (y2 * y) + 1.0f;
            result.M23 = z2 * y;
            result.M24 = 0.0f;
            result.M31 = x2 * z;
            result.M32 = y2 * z;
            result.M33 = (z2 * z) + 1.0f;
            result.M34 = 0.0f;
            result.M41 = x2 * plane.D;
            result.M42 = y2 * plane.D;
            result.M43 = z2 * plane.D;
            result.M44 = 1.0f;
            return result;
        }

        public static void Reflection(ref Plane plane, out Matrix result)
        {
            plane.Normalize();
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float x2 = -2.0f * x;
            float y2 = -2.0f * y;
            float z2 = -2.0f * z;
            result.M11 = (x2 * x) + 1.0f;
            result.M12 = y2 * x;
            result.M13 = z2 * x;
            result.M14 = 0.0f;
            result.M21 = x2 * y;
            result.M22 = (y2 * y) + 1.0f;
            result.M23 = z2 * y;
            result.M24 = 0.0f;
            result.M31 = x2 * z;
            result.M32 = y2 * z;
            result.M33 = (z2 * z) + 1.0f;
            result.M34 = 0.0f;
            result.M41 = x2 * plane.D;
            result.M42 = y2 * plane.D;
            result.M43 = z2 * plane.D;
            result.M44 = 1.0f;
        }

        public static Matrix Shadow(Vector4 light, Plane plane)
        {
            Matrix result;
            plane.Normalize();
            float dot = ((plane.Normal.X * light.X) + (plane.Normal.Y * light.Y)) + (plane.Normal.Z * light.Z);
            float x = -plane.Normal.X;
            float y = -plane.Normal.Y;
            float z = -plane.Normal.Z;
            float d = -plane.D;
            result.M11 = (x * light.X) + dot;
            result.M21 = y * light.X;
            result.M31 = z * light.X;
            result.M41 = d * light.X;
            result.M12 = x * light.Y;
            result.M22 = (y * light.Y) + dot;
            result.M32 = z * light.Y;
            result.M42 = d * light.Y;
            result.M13 = x * light.Z;
            result.M23 = y * light.Z;
            result.M33 = (z * light.Z) + dot;
            result.M43 = d * light.Z;
            result.M14 = 0.0f;
            result.M24 = 0.0f;
            result.M34 = 0.0f;
            result.M44 = dot;
            return result;
        }

        public static void Shadow(ref Vector4 light, ref Plane plane, out Matrix result)
        {
            plane.Normalize();
            float dot = ((plane.Normal.X * light.X) + (plane.Normal.Y * light.Y)) + (plane.Normal.Z * light.Z);
            float x = -plane.Normal.X;
            float y = -plane.Normal.Y;
            float z = -plane.Normal.Z;
            float d = -plane.D;
            result.M11 = (x * light.X) + dot;
            result.M21 = y * light.X;
            result.M31 = z * light.X;
            result.M41 = d * light.X;
            result.M12 = x * light.Y;
            result.M22 = (y * light.Y) + dot;
            result.M32 = z * light.Y;
            result.M42 = d * light.Y;
            result.M13 = x * light.Z;
            result.M23 = y * light.Z;
            result.M33 = (z * light.Z) + dot;
            result.M43 = d * light.Z;
            result.M14 = 0.0f;
            result.M24 = 0.0f;
            result.M34 = 0.0f;
            result.M44 = dot;
        }

        public static Matrix Invert(ref Matrix mat)
        {
            Matrix result = new Matrix();
            Invert(ref mat, out result);

            return result;
        }

        public static void Invert(ref Matrix mat, out Matrix result)
        {
            unsafe
            {
                fixed (Matrix* pResult = &result)
                {
                    fixed (Matrix* pMat = &mat)
                    {
                        IDllImportApi.D3DXMatrixInverse(pResult, (float*)0, pMat);
                    }
                }
            }
        }

        public static Matrix Transpose(ref Matrix mat)
        {
            Matrix result;
            result.M11 = mat.M11;
            result.M12 = mat.M21;
            result.M13 = mat.M31;
            result.M14 = mat.M41;
            result.M21 = mat.M12;
            result.M22 = mat.M22;
            result.M23 = mat.M32;
            result.M24 = mat.M42;
            result.M31 = mat.M13;
            result.M32 = mat.M23;
            result.M33 = mat.M33;
            result.M34 = mat.M43;
            result.M41 = mat.M14;
            result.M42 = mat.M24;
            result.M43 = mat.M34;
            result.M44 = mat.M44;
            return result;
        }

        public static void Transpose(ref Matrix mat, out Matrix result)
        {
            Matrix r;
            r.M11 = mat.M11;
            r.M12 = mat.M21;
            r.M13 = mat.M31;
            r.M14 = mat.M41;
            r.M21 = mat.M12;
            r.M22 = mat.M22;
            r.M23 = mat.M32;
            r.M24 = mat.M42;
            r.M31 = mat.M13;
            r.M32 = mat.M23;
            r.M33 = mat.M33;
            r.M34 = mat.M43;
            r.M41 = mat.M14;
            r.M42 = mat.M24;
            r.M43 = mat.M34;
            r.M44 = mat.M44;

            result = r;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            Matrix result;
            result.M11 = (left.M11 * right.M11) + (left.M12 * right.M21) + (left.M13 * right.M31) + (left.M14 * right.M41);
            result.M12 = (left.M11 * right.M12) + (left.M12 * right.M22) + (left.M13 * right.M32) + (left.M14 * right.M42);
            result.M13 = (left.M11 * right.M13) + (left.M12 * right.M23) + (left.M13 * right.M33) + (left.M14 * right.M43);
            result.M14 = (left.M11 * right.M14) + (left.M12 * right.M24) + (left.M13 * right.M34) + (left.M14 * right.M44);
            result.M21 = (left.M21 * right.M11) + (left.M22 * right.M21) + (left.M23 * right.M31) + (left.M24 * right.M41);
            result.M22 = (left.M21 * right.M12) + (left.M22 * right.M22) + (left.M23 * right.M32) + (left.M24 * right.M42);
            result.M23 = (left.M21 * right.M13) + (left.M22 * right.M23) + (left.M23 * right.M33) + (left.M24 * right.M43);
            result.M24 = (left.M21 * right.M14) + (left.M22 * right.M24) + (left.M23 * right.M34) + (left.M24 * right.M44);
            result.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + (left.M33 * right.M31) + (left.M34 * right.M41);
            result.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + (left.M33 * right.M32) + (left.M34 * right.M42);
            result.M33 = (left.M31 * right.M13) + (left.M32 * right.M23) + (left.M33 * right.M33) + (left.M34 * right.M43);
            result.M34 = (left.M31 * right.M14) + (left.M32 * right.M24) + (left.M33 * right.M34) + (left.M34 * right.M44);
            result.M41 = (left.M41 * right.M11) + (left.M42 * right.M21) + (left.M43 * right.M31) + (left.M44 * right.M41);
            result.M42 = (left.M41 * right.M12) + (left.M42 * right.M22) + (left.M43 * right.M32) + (left.M44 * right.M42);
            result.M43 = (left.M41 * right.M13) + (left.M42 * right.M23) + (left.M43 * right.M33) + (left.M44 * right.M43);
            result.M44 = (left.M41 * right.M14) + (left.M42 * right.M24) + (left.M43 * right.M34) + (left.M44 * right.M44);
            return result;
        }

        public static Matrix operator *(Matrix left, float right)
        {
            Matrix result;
            result.M11 = left.M11 * right;
            result.M12 = left.M12 * right;
            result.M13 = left.M13 * right;
            result.M14 = left.M14 * right;
            result.M21 = left.M21 * right;
            result.M22 = left.M22 * right;
            result.M23 = left.M23 * right;
            result.M24 = left.M24 * right;
            result.M31 = left.M31 * right;
            result.M32 = left.M32 * right;
            result.M33 = left.M33 * right;
            result.M34 = left.M34 * right;
            result.M41 = left.M41 * right;
            result.M42 = left.M42 * right;
            result.M43 = left.M43 * right;
            result.M44 = left.M44 * right;
            return result;
        }

        public static Matrix operator *(float right, Matrix left)
        {
            return left * right;
        }

        public static Matrix operator /(Matrix left, Matrix right)
        {
            Matrix result;
            result.M11 = left.M11 / right.M11;
            result.M12 = left.M12 / right.M12;
            result.M13 = left.M13 / right.M13;
            result.M14 = left.M14 / right.M14;
            result.M21 = left.M21 / right.M21;
            result.M22 = left.M22 / right.M22;
            result.M23 = left.M23 / right.M23;
            result.M24 = left.M24 / right.M24;
            result.M31 = left.M31 / right.M31;
            result.M32 = left.M32 / right.M32;
            result.M33 = left.M33 / right.M33;
            result.M34 = left.M34 / right.M34;
            result.M41 = left.M41 / right.M41;
            result.M42 = left.M42 / right.M42;
            result.M43 = left.M43 / right.M43;
            result.M44 = left.M44 / right.M44;
            return result;
        }

        public static Matrix operator /(Matrix left, float right)
        {
            Matrix result;
            result.M11 = left.M11 / right;
            result.M12 = left.M12 / right;
            result.M13 = left.M13 / right;
            result.M14 = left.M14 / right;
            result.M21 = left.M21 / right;
            result.M22 = left.M22 / right;
            result.M23 = left.M23 / right;
            result.M24 = left.M24 / right;
            result.M31 = left.M31 / right;
            result.M32 = left.M32 / right;
            result.M33 = left.M33 / right;
            result.M34 = left.M34 / right;
            result.M41 = left.M41 / right;
            result.M42 = left.M42 / right;
            result.M43 = left.M43 / right;
            result.M44 = left.M44 / right;
            return result;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            Matrix result;
            result.M11 = left.M11 + right.M11;
            result.M12 = left.M12 + right.M12;
            result.M13 = left.M13 + right.M13;
            result.M14 = left.M14 + right.M14;
            result.M21 = left.M21 + right.M21;
            result.M22 = left.M22 + right.M22;
            result.M23 = left.M23 + right.M23;
            result.M24 = left.M24 + right.M24;
            result.M31 = left.M31 + right.M31;
            result.M32 = left.M32 + right.M32;
            result.M33 = left.M33 + right.M33;
            result.M34 = left.M34 + right.M34;
            result.M41 = left.M41 + right.M41;
            result.M42 = left.M42 + right.M42;
            result.M43 = left.M43 + right.M43;
            result.M44 = left.M44 + right.M44;
            return result;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            Matrix result;
            result.M11 = left.M11 - right.M11;
            result.M12 = left.M12 - right.M12;
            result.M13 = left.M13 - right.M13;
            result.M14 = left.M14 - right.M14;
            result.M21 = left.M21 - right.M21;
            result.M22 = left.M22 - right.M22;
            result.M23 = left.M23 - right.M23;
            result.M24 = left.M24 - right.M24;
            result.M31 = left.M31 - right.M31;
            result.M32 = left.M32 - right.M32;
            result.M33 = left.M33 - right.M33;
            result.M34 = left.M34 - right.M34;
            result.M41 = left.M41 - right.M41;
            result.M42 = left.M42 - right.M42;
            result.M43 = left.M43 - right.M43;
            result.M44 = left.M44 - right.M44;
            return result;
        }

        public static Matrix operator -(Matrix matrix)
        {
            Matrix result;
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;
            return result;
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !Matrix.Equals(left, right);
        }
    }
}
