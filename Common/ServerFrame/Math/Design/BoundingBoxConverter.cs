using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace SlimDX.Design
{
    public class BoundingBoxConverter : System.ComponentModel.ExpandableObjectConverter
	{
		private PropertyDescriptorCollection m_Properties;

        public BoundingBoxConverter()
	    {
		    Type type = typeof(BoundingBox);
            PropertyDescriptor[] propArray = new PropertyDescriptor[]
		    {
			    new FieldPropertyDescriptor(type.GetField("Minimum")),
			    new FieldPropertyDescriptor(type.GetField("Maximum")),
		    };

		    m_Properties = new PropertyDescriptorCollection(propArray);
	    }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	    {
		    if( destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor) )
			    return true;
		    else
			    return base.CanConvertTo(context, destinationType);
	    }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	    {
		    if( sourceType == typeof(string) )
			    return true;
		    else
			    return base.CanConvertFrom(context, sourceType);
	    }

        public override Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
	    {
		    if( destinationType == null )
			    throw new ArgumentNullException( "destinationType" );

		    if( culture == null )
			    culture = CultureInfo.CurrentCulture;

		    BoundingBox box = (BoundingBox)( value );

		    if( destinationType == typeof(string) && box != null )
		    {
			    String separator = culture.TextInfo.ListSeparator + " ";
			    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Vector3));
			    string[] stringArray = new string[ 2 ];

			    stringArray[0] = converter.ConvertToString( context, culture, box.Maximum );
			    stringArray[1] = converter.ConvertToString( context, culture, box.Minimum );

			    return String.Join( separator, stringArray );
		    }
		    else if( destinationType == typeof(InstanceDescriptor) && box != null )
		    {
			    ConstructorInfo info = (typeof(BoundingBox)).GetConstructor( new Type[] { typeof(Vector3), typeof(Vector3) } );
			    if( info != null )
				    return new InstanceDescriptor( info, new Object[] { box.Maximum, box.Minimum } );
		    }

		    return base.ConvertTo(context, culture, value, destinationType);
	    }

        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
	    {
		    if( culture == null )
			    culture = CultureInfo.CurrentCulture;

		    String Str = (string)( value );

		    if( Str != null )
		    {
			    Str = Str.Trim();
			    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Vector3));
			    string[] stringArray = Str.Split( culture.TextInfo.ListSeparator[0] );

			    if( stringArray.Length != 2 )
				    throw new ArgumentException("Invalid box format.");

			    Vector3 Maximum = (Vector3)( converter.ConvertFromString( context, culture, stringArray[0] ) );
			    Vector3 Minimum = (Vector3)( converter.ConvertFromString( context, culture, stringArray[1] ) );

			    return new BoundingBox(Maximum, Minimum);
		    }

		    return base.ConvertFrom(context, culture, value);
	    }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
	    {
		    //SLIMDX_UNREFERENCED_PARAMETER(context);
		    return true;
	    }

        public override Object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
	    {
		    //SLIMDX_UNREFERENCED_PARAMETER(context);

		    if( propertyValues == null )
			    throw new ArgumentNullException( "propertyValues" );

		    return new BoundingBox( (Vector3)( propertyValues["Maximum"] ), (Vector3)( propertyValues["Minimum"] ) );
	    }

        public override bool GetPropertiesSupported(ITypeDescriptorContext tdc)
	    {
		    return true;
	    }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext tdc, Object obj, Attribute[] attrs)
	    {
		    return m_Properties;
	    }
    }
}
