using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 4 )]
	public struct Matrix3x2 : System.IEquatable<Matrix3x2>
	{
        public float M11;
		public float M12;
		public float M21;
		public float M22;
		public float M31;
		public float M32;

        public static Matrix3x2 Identity
	    {
            get
            {
		        Matrix3x2 result = new Matrix3x2();
		        result.M11 = 1.0f;
		        result.M22 = 1.0f;

		        return result;
            }
	    }

        //Matrix3x2 Invert( Matrix3x2 mat )
        //{
        //    Matrix3x2 result = mat;
        //    (D2D1.Matrix3x2F.ReinterpretBaseType((D2D1_MATRIX_3X2_F*)&result)).Invert();
        //    return result;
        //}
			
        //void Invert( ref Matrix3x2 mat, out Matrix3x2 result )
        //{
        //    result = mat;

        //    pin_ptr<Matrix3x2> pinResult = &result;

        //    (D2D1.Matrix3x2F.ReinterpretBaseType((D2D1_MATRIX_3X2_F*)pinResult)).Invert();
        //}

        public static Matrix3x2 operator *(Matrix3x2 left, Matrix3x2 right)
	    {
		    Matrix3x2 r;

		    r.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            r.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            r.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            r.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);
            r.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + right.M31;
            r.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + right.M32;

		    return r;
	    }

        public static Matrix3x2 Multiply(Matrix3x2 left, Matrix3x2 right)
	    {
		    Matrix3x2 r;

		    r.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            r.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            r.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            r.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);
            r.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + right.M31;
            r.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + right.M32;

		    return r;
	    }

        public static void Multiply(ref Matrix3x2 left, ref Matrix3x2 right, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = (left.M11 * right.M11) + (left.M12 * right.M21);
            r.M12 = (left.M11 * right.M12) + (left.M12 * right.M22);
            r.M21 = (left.M21 * right.M11) + (left.M22 * right.M21);
            r.M22 = (left.M21 * right.M12) + (left.M22 * right.M22);
            r.M31 = (left.M31 * right.M11) + (left.M32 * right.M21) + right.M31;
            r.M32 = (left.M31 * right.M12) + (left.M32 * right.M22) + right.M32;

		    result = r;
	    }

        public static Matrix3x2 Rotation(float angle, System.Drawing.PointF centerPoint)
	    {
		    return Translation( -centerPoint.X, -centerPoint.Y ) *
			       Rotation( angle ) *
			       Translation( centerPoint.X, centerPoint.Y );
	    }

        public static void Rotation(float angle, ref System.Drawing.PointF centerPoint, out Matrix3x2 result)
	    {
		    result = Translation( -centerPoint.X, -centerPoint.Y ) *
				     Rotation( angle ) *
				     Translation( centerPoint.X, centerPoint.Y );
	    }

        public static void Rotation(float angle, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    double radians = (Math.PI * angle) / 180.0;

		    float cos = (float)( Math.Cos( radians ) );
		    float sin = (float)( Math.Sin( radians ) );

		    r.M11 = cos;
		    r.M12 = sin;
		    r.M21 = -sin;
		    r.M22 = cos;
		    r.M31 = 0.0f;
		    r.M32 = 0.0f;

		    result = r;
	    }

        public static Matrix3x2 Rotation(float angle)
	    {
		    Matrix3x2 result;

		    double radians = (Math.PI * angle) / 180.0;

		    float cos = (float)( Math.Cos( radians ) );
		    float sin = (float)( Math.Sin( radians ) );

		    result.M11 = cos;
		    result.M12 = sin;
		    result.M21 = -sin;
		    result.M22 = cos;
		    result.M31 = 0.0f;
		    result.M32 = 0.0f;

		    return result;
	    }

        public static Matrix3x2 Scale(System.Drawing.SizeF size)
	    {
		    Matrix3x2 r;

		    r.M11 = size.Width;	r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = size.Height;
		    r.M31 = 0.0f;		r.M32 = 0.0f;

		    return r;
	    }

        public static void Scale(ref System.Drawing.SizeF size, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = size.Width;	r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = size.Height;
		    r.M31 = 0.0f;		r.M32 = 0.0f;

		    result = r;
	    }

        public static Matrix3x2 Scale(float x, float y)
	    {
		    Matrix3x2 r;

		    r.M11 = x;			r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = y;
		    r.M31 = 0.0f;		r.M32 = 0.0f;

		    return r;
	    }

        public static void Scale(float x, float y, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = x;			r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = y;
		    r.M31 = 0.0f;		r.M32 = 0.0f;

		    result = r;
	    }

        public static Matrix3x2 Scale(System.Drawing.SizeF size, System.Drawing.PointF centerPoint)
	    {
		    Matrix3x2 r;

		    r.M11 = size.Width;	r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = size.Height;

		    r.M31 = centerPoint.X - (size.Width * centerPoint.X);
		    r.M32 = centerPoint.Y - (size.Height * centerPoint.Y);

		    return r;
	    }

        public static void Scale(ref System.Drawing.SizeF size, ref System.Drawing.PointF centerPoint, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = size.Width;	r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = size.Height;

		    r.M31 = centerPoint.X - (size.Width * centerPoint.X);
		    r.M32 = centerPoint.Y - (size.Height * centerPoint.Y);

		    result = r;
	    }

        public static Matrix3x2 Scale(float x, float y, System.Drawing.PointF centerPoint)
	    {
		    Matrix3x2 r;

		    r.M11 = x;		r.M12 = 0.0f;
		    r.M21 = 0.0f;	r.M22 = y;

		    r.M31 = centerPoint.X - (x * centerPoint.X);
		    r.M32 = centerPoint.Y - (y * centerPoint.Y);

		    return r;
	    }

        public static void Scale(float x, float y, ref System.Drawing.PointF centerPoint, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = x;		r.M12 = 0.0f;
		    r.M21 = 0.0f;	r.M22 = y;

		    r.M31 = centerPoint.X - (x * centerPoint.X);
		    r.M32 = centerPoint.Y - (y * centerPoint.Y);

		    result = r;
	    }

        //public static Matrix3x2 Skew(float angleX, float angleY, System.Drawing.PointF centerPoint)
        //{
        //    D2D1_POINT_2F centerPoint_n = D2D1.Point2F( centerPoint.X, centerPoint.Y );

        //    Matrix3x2 r;

        //    D2D1.Matrix3x2F mat = D2D1.Matrix3x2F.Skew( angleX, angleY, centerPoint_n );

        //    r.M11 = mat._11; r.M12 = mat._12;
        //    r.M21 = mat._21; r.M22 = mat._22;
        //    r.M31 = mat._31; r.M32 = mat._32;

        //    return r;
        //}

        //public static void Skew(float angleX, float angleY, ref System.Drawing.PointF centerPoint, out Matrix3x2 result)
        //{
        //    D2D1_POINT_2F centerPoint_n = D2D1.Point2F( centerPoint.X, centerPoint.Y );

        //    Matrix3x2 r;

        //    D2D1.Matrix3x2F mat = D2D1.Matrix3x2F.Skew( angleX, angleY, centerPoint_n );

        //    r.M11 = mat._11; r.M12 = mat._12;
        //    r.M21 = mat._21; r.M22 = mat._22;
        //    r.M31 = mat._31; r.M32 = mat._32;

        //    result = r;
        //}

        public static Matrix3x2 Translation(System.Drawing.PointF point)
	    {
		    Matrix3x2 r;

		    r.M11 = 1.0f;		r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = 1.0f;
		    r.M31 = point.X;	r.M32 = point.Y;

		    return r;
	    }

        public static void Translation(ref System.Drawing.PointF point, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = 1.0f;		r.M12 = 0.0f;
		    r.M21 = 0.0f;		r.M22 = 1.0f;
		    r.M31 = point.X;	r.M32 = point.Y;

		    result = r;
	    }

        public static Matrix3x2 Translation(float x, float y)
	    {
		    Matrix3x2 r;

		    r.M11 = 1.0f;	r.M12 = 0.0f;
		    r.M21 = 0.0f;	r.M22 = 1.0f;
		    r.M31 = x;		r.M32 = y;

		    return r;
	    }

        public static void Translation(float x, float y, out Matrix3x2 result)
	    {
		    Matrix3x2 r;

		    r.M11 = 1.0f;	r.M12 = 0.0f;
		    r.M21 = 0.0f;	r.M22 = 1.0f;
		    r.M31 = x;		r.M32 = y;

		    result = r;
	    }

        public static System.Drawing.PointF TransformPoint(Matrix3x2 mat, System.Drawing.PointF point)
	    {
		    System.Drawing.PointF r = new System.Drawing.PointF();

		    r.X = (point.X * mat.M11) + (point.Y * mat.M21) + mat.M31;
		    r.Y = (point.X * mat.M12) + (point.Y * mat.M22) + mat.M32;

		    return r;
	    }

        public static void TransformPoint(ref Matrix3x2 mat, ref System.Drawing.PointF point, out System.Drawing.PointF result)
	    {
            System.Drawing.PointF r = new System.Drawing.PointF();

		    r.X = (point.X * mat.M11) + (point.Y * mat.M21) + mat.M31;
		    r.Y = (point.X * mat.M12) + (point.Y * mat.M22) + mat.M32;

		    result = r;
	    }

        public bool IsIdentity
	    {
            get
            {
                return (M11 == 1.0f && M12 == 0.0f &&
                          M21 == 0.0f && M22 == 1.0f &&
                          M31 == 0.0f && M32 == 0.0f);
            }
	    }

        public float Determinant()
	    {
		    return (M11 * M22) - (M12 * M21);
	    }

        //bool Invert()
        //{
        //    pin_ptr<Matrix3x2> pinnedThis = this;

        //    return (D2D1.Matrix3x2F.ReinterpretBaseType((D2D1_MATRIX_3X2_F*)pinnedThis)).Invert();
        //}

        public static bool operator ==(Matrix3x2 left, Matrix3x2 right)
	    {
		    return Equals( left, right );
	    }

        public static bool operator !=(Matrix3x2 left, Matrix3x2 right)
	    {
		    return !Equals( left, right );
	    }

        public override System.String ToString()
	    {
		    return String.Format( CultureInfo.CurrentCulture, "[[M11:{0} M12:{1}] [M21:{2} M22:{3}] [M31:{4} M32:{5}]]",
			    M11.ToString(CultureInfo.CurrentCulture), M12.ToString(CultureInfo.CurrentCulture),
			    M21.ToString(CultureInfo.CurrentCulture), M22.ToString(CultureInfo.CurrentCulture),
			    M31.ToString(CultureInfo.CurrentCulture), M32.ToString(CultureInfo.CurrentCulture));
	    }

        public override int GetHashCode()
	    {
		    return M11.GetHashCode() + M12.GetHashCode() +
			       M21.GetHashCode() + M22.GetHashCode() + 
			       M31.GetHashCode() + M32.GetHashCode();
	    }

        public override bool Equals(System.Object obj)
	    {
		    if( obj == null )
			    return false;

		    if( obj.GetType() != GetType() )
			    return false;

		    return Equals( (Matrix3x2)( obj ) );
	    }

	    public bool Equals( Matrix3x2 other )
	    {
		    return ( M11 == other.M11 && M12 == other.M12 &&
				     M21 == other.M21 && M22 == other.M22 &&
				     M31 == other.M31 && M32 == other.M32 );
	    }

        public static bool Equals(ref Matrix3x2 value1, ref Matrix3x2 value2)
	    {
		    return ( value1.M11 == value2.M11 && value1.M12 == value2.M12 &&
				     value1.M21 == value2.M21 && value1.M22 == value2.M22 &&
				     value1.M31 == value2.M31 && value1.M32 == value2.M32 );
	    }
    }
}
