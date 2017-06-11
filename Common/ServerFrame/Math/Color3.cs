using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlimDX
{
    [System.Serializable]
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    //[System.ComponentModel.TypeConverter( typeof(SlimDX.Design.Color3Converter))]
	public struct Color3 : System.IEquatable<Color3>
    {
        public float Red;
		public float Green;
		public float Blue;

        public Color3( float red, float green, float blue )
	    {
            Red = red;
            Green = green;
            Blue = blue;
	    }

	    public static bool operator == ( Color3 left, Color3 right )
	    {
		    return Color3.Equals( left, right );
	    }

        public static bool operator !=(Color3 left, Color3 right)
	    {
		    return !Equals( left, right );
	    }

	    public override int GetHashCode()
	    {
		    return Red.GetHashCode() + Green.GetHashCode() + Blue.GetHashCode();
	    }

	    public override bool Equals( object value )
	    {
		    if( value == null )
			    return false;

		    if( value.GetType() != GetType() )
			    return false;

		    return Equals( (Color3)( value ) );
	    }

	    public bool Equals( Color3 value )
	    {
		    return ( Red == value.Red && Green == value.Green && Blue == value.Blue );
	    }

        public static bool Equals(ref Color3 value1, ref Color3 value2)
	    {
		    return ( value1.Red == value2.Red && value1.Green == value2.Green && value1.Blue == value2.Blue );
	    }
    }
}
