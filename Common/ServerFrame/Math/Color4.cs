using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.Color4Converter) )]
	public struct Color4
    {
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;
        
	    public Color4( float alpha, float red, float green, float blue )
	    {
		    Alpha = alpha;
		    Red = red;
		    Green = green;
		    Blue = blue;
	    }

	    public Color4( float red, float green, float blue )
	    {
		    Alpha = 1.0f;
		    Red = red;
		    Green = green;
		    Blue = blue;
	    }

        public Color4(System.Drawing.Color color)
	    {
		    Alpha = color.A / 255.0f;
		    Red = color.R / 255.0f;
		    Green = color.G / 255.0f;
		    Blue = color.B / 255.0f;
	    }

	    public Color4( Color3 color )
	    {
		    Alpha = 1.0f;
		    Red = color.Red;
		    Green = color.Green;
		    Blue = color.Blue;
	    }

	    public Color4( Vector3 color )
	    {
		    Alpha = 1.0f;
		    Red = color.X;
		    Green = color.Y;
		    Blue = color.Z;
	    }

	    public Color4( Vector4 color )
	    {
		    Alpha = color.W;
		    Red = color.X;
		    Green = color.Y;
		    Blue = color.Z;
	    }

	    public Color4( int argb )
	    {
		    Alpha = ( ( argb >> 24 ) & 255 ) / 255.0f;
		    Red = ( ( argb >> 16 ) & 255 ) / 255.0f;
		    Green = ( ( argb >> 8 ) & 255 ) / 255.0f;
		    Blue = ( argb & 255 ) / 255.0f;
	    }

	    public Color ToColor()
	    {
		    return Color.FromArgb( (int)(Alpha * 255), (int)(Red * 255), (int)(Green * 255), (int)(Blue * 255) );
	    }

	    public Color3 ToColor3()
	    {
		    return new Color3( Red, Green, Blue );
	    }

	    public int ToArgb()
	    {
		    UInt32 a, r, g, b;

		    a = (UInt32)(Alpha * 255.0f);
		    r = (UInt32)(Red * 255.0f);
		    g = (UInt32)(Green * 255.0f);
		    b = (UInt32)(Blue * 255.0f);

		    UInt32 value = b;
		    value += g << 8;
		    value += r << 16;
		    value += a << 24;

		    return (int)( value );
	    }

	    public Vector3 ToVector3()
	    {
            return new Vector3(Red, Green, Blue);
	    }

	    public Vector4 ToVector4()
	    {
            return new Vector4(Red, Green, Blue, Alpha);
	    }

	    public static Color4 Add( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha + color2.Alpha, color1.Red + color2.Red, color1.Green + color2.Green, color1.Blue + color2.Blue);
	    }

	    public static void Add( ref Color4 color1, ref Color4 color2, out Color4 result )
	    {
            result = new Color4(color1.Alpha + color2.Alpha, color1.Red + color2.Red, color1.Green + color2.Green, color1.Blue + color2.Blue);
	    }

	    public static Color4 Subtract( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha - color2.Alpha, color1.Red - color2.Red, color1.Green - color2.Green, color1.Blue - color2.Blue);
	    }

	    public static void Subtract( ref Color4 color1, ref Color4 color2, out Color4 result )
	    {
            result = new Color4(color1.Alpha - color2.Alpha, color1.Red - color2.Red, color1.Green - color2.Green, color1.Blue - color2.Blue);
	    }

	    public static Color4 Modulate( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha * color2.Alpha, color1.Red * color2.Red, color1.Green * color2.Green, color1.Blue * color2.Blue);
	    }

	    public static void Modulate( ref Color4 color1, ref Color4 color2, out Color4 result )
	    {
            result = new Color4(color1.Alpha * color2.Alpha, color1.Red * color2.Red, color1.Green * color2.Green, color1.Blue * color2.Blue);
	    }

	    public static Color4 Lerp( Color4 color1, Color4 color2, float amount )
	    {
		    float a = color1.Alpha + amount * ( color2.Alpha - color1.Alpha );
		    float r = color1.Red + amount * ( color2.Red - color1.Red );
		    float g = color1.Green + amount * ( color2.Green - color1.Green );
		    float b = color1.Blue + amount * ( color2.Blue - color1.Blue );

            return new Color4(a, r, g, b);
	    }

	    public static void Lerp( ref Color4 color1, ref Color4 color2, float amount, out Color4 result )
	    {
		    float a = color1.Alpha + amount * ( color2.Alpha - color1.Alpha );
		    float r = color1.Red + amount * ( color2.Red - color1.Red );
		    float g = color1.Green + amount * ( color2.Green - color1.Green );
		    float b = color1.Blue + amount * ( color2.Blue - color1.Blue );

            result = new Color4(a, r, g, b);
	    }

	    public static Color4 Negate( Color4 color )
	    {
            return new Color4(1.0f - color.Alpha, 1.0f - color.Red, 1.0f - color.Green, 1.0f - color.Blue);
	    }

	    public static void Negate( ref Color4 color, out Color4 result )
	    {
            result = new Color4(1.0f - color.Alpha, 1.0f - color.Red, 1.0f - color.Green, 1.0f - color.Blue);
	    }

	    public static Color4 AdjustContrast( Color4 color, float contrast )
	    {
		    float r = 0.5f + contrast * ( color.Red - 0.5f );
		    float g = 0.5f + contrast * ( color.Green - 0.5f );
		    float b = 0.5f + contrast * ( color.Blue - 0.5f );

            return new Color4(color.Alpha, r, g, b);
	    }

	    public static void AdjustContrast( ref Color4 color, float contrast, out Color4 result )
	    {
		    float r = 0.5f + contrast * ( color.Red - 0.5f );
		    float g = 0.5f + contrast * ( color.Green - 0.5f );
		    float b = 0.5f + contrast * ( color.Blue - 0.5f );

            result = new Color4(color.Alpha, r, g, b);
	    }

	    public static Color4 AdjustSaturation( Color4 color, float saturation )
	    {
		    float grey = color.Red * 0.2125f + color.Green * 0.7154f + color.Blue * 0.0721f;
		    float r = grey + saturation * ( color.Red - grey );
		    float g = grey + saturation * ( color.Green - grey );
		    float b = grey + saturation * ( color.Blue - grey );

            return new Color4(color.Alpha, r, g, b);
	    }

	    public static void AdjustSaturation( ref Color4 color, float saturation, out Color4 result )
	    {
		    float grey = color.Red * 0.2125f + color.Green * 0.7154f + color.Blue * 0.0721f;
		    float r = grey + saturation * ( color.Red - grey );
		    float g = grey + saturation * ( color.Green - grey );
		    float b = grey + saturation * ( color.Blue - grey );

            result = new Color4(color.Alpha, r, g, b);
	    }

	    public static Color4 Scale( Color4 color, float scale )
	    {
            return new Color4(color.Alpha, color.Red * scale, color.Green * scale, color.Blue * scale);
	    }

	    public static void Scale( ref Color4 color, float scale, out Color4 result )
	    {
            result = new Color4(color.Alpha, color.Red * scale, color.Green * scale, color.Blue * scale);
	    }

	    public static Color4 operator + ( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha + color2.Alpha, color1.Red + color2.Red, color1.Green + color2.Green, color1.Blue + color2.Blue);
	    }

	    public static Color4 operator - ( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha - color2.Alpha, color1.Red - color2.Red, color1.Green - color2.Green, color1.Blue - color2.Blue);
	    }

	    public static Color4 operator - ( Color4 color )
	    {
            return new Color4(1.0f - color.Alpha, 1.0f - color.Red, 1.0f - color.Green, 1.0f - color.Blue);
	    }

	    public static Color4 operator * ( Color4 color1, Color4 color2 )
	    {
            return new Color4(color1.Alpha * color2.Alpha, color1.Red * color2.Red, color1.Green * color2.Green, color1.Blue * color2.Blue);
	    }

        public static Color4 operator *(Color4 color, float scale)
	    {
            return new Color4(color.Alpha, color.Red * scale, color.Green * scale, color.Blue * scale);
	    }

	    public static Color4 operator * ( float scale, Color4 value )
	    {
		    return value * scale;
	    }

	    public static bool operator == ( Color4 left, Color4 right )
	    {
		    return Equals( left, right );
	    }

	    public static bool operator != ( Color4 left, Color4 right )
	    {
		    return !Equals( left, right );
	    }

	    public static implicit operator int( Color4 value )
	    {
		    return value.ToArgb();
	    }

	    public static implicit operator Color3( Color4 value )
	    {
		    return value.ToColor3();
	    }

	    public static implicit operator System.Drawing.Color( Color4 value )
	    {
		    return value.ToColor();
	    }

	    public static implicit operator Vector3( Color4 value )
	    {
		    return value.ToVector3();
	    }

	    public static implicit operator Vector4( Color4 value )
	    {
		    return value.ToVector4();
	    }

	    public static implicit operator Color4( int value )
	    {
            return new Color4(value);
	    }

	    public static implicit operator Color4( Color3 value )
	    {
            return new Color4(value);
	    }

	    public static implicit operator Color4( System.Drawing.Color value )
	    {
            return new Color4(value);
	    }

	    public static implicit operator Color4( Vector3 value )
	    {
            return new Color4(value);
	    }

        public static implicit operator Color4(Vector4 value)
	    {
            return new Color4(value);
	    }

	    public override string ToString()
	    {
		    return string.Format( CultureInfo.CurrentCulture, "A:{0} R:{1} G:{2} B:{3}", 
			    Alpha.ToString(CultureInfo.CurrentCulture), Red.ToString(CultureInfo.CurrentCulture), 
			    Green.ToString(CultureInfo.CurrentCulture), Blue.ToString(CultureInfo.CurrentCulture) );
	    }

	    public override int GetHashCode()
	    {
		    return Alpha.GetHashCode() + Red.GetHashCode() + Green.GetHashCode() + Blue.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Color4)( value ) );
	    }

	    public bool Equals( Color4 value )
	    {
		    return ( Alpha == value.Alpha && Red == value.Red && Green == value.Green && Blue == value.Blue );
	    }

	    public static bool Equals( ref Color4 value1, ref Color4 value2 )
	    {
		    return ( value1.Alpha == value2.Alpha && value1.Red == value2.Red && value1.Green == value2.Green && value1.Blue == value2.Blue );
	    }
    }
}
