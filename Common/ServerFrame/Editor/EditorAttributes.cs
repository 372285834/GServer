using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Helper
{
    public enum enCSType
    {
        Common,
        Server,
        Client,
        All,
    }
}

namespace EditorCommon.Assist
{
    public sealed class Editor_AISetAttribute : Attribute { }

    public sealed class Editor_MeshInitAttribute : Attribute { }
    public sealed class Editor_MeshTemplateAttribute : Attribute { }
    public sealed class Editor_ActionInitAttribute : Attribute { }
    public sealed class Editor_EffectTemplateAttribute : Attribute { }

    public sealed class Editor_MaterialCollectionAttribute : Attribute { }
    public sealed class Editor_Material_ShaderValueAttribute : Attribute { }

    public sealed class Editor_TexturePathValueAttribute : Attribute { }
    public sealed class Editor_MaterialParamValueAttribute : Attribute { }

    public sealed class Editor_MultipleOfTwoAttribute : Attribute { }

    public sealed class Editor_ColorPicker : Attribute { }

    public sealed class Editor_HexAttribute : Attribute { }

    public sealed class Editor_VectorEditor : Attribute { }

    public sealed class Editor_ActorLayerSetter : Attribute { }

    public sealed class Editor_Angle360Setter : Attribute { }
    public sealed class Editor_Angle180Setter : Attribute { }

    public sealed class Editor_MeshSocketSetter : Attribute { }
    public sealed class Editor_ScalarVariableSetter : Attribute { }
    public sealed class Editor_HotKeySetter : Attribute { }
    public sealed class Editor_UIControlTemplateSetter : Attribute { }

    #region UIEditor

    public sealed class UIEditor_ControlAttribute : Attribute
    {
        System.String mName;
        public System.String Name
        {
            get { return mName; }
        }

        public UIEditor_ControlAttribute(System.String name)
        {
            mName = name;
        }
    }

    // 可以模板化的控件
    public sealed class UIEditor_ControlTemplateAbleAttribute : Attribute
    {
        System.String mName;
        public System.String Name
        {
            get { return mName; }
        }

        public UIEditor_ControlTemplateAbleAttribute(System.String name)
        {
            mName = name;
        }
    }

    public sealed class UIEditor_UVAnimSetterAttribute : Attribute { }

    // Binding
    public sealed class UIEditor_BindingEventAttribute : Attribute { }
    public sealed class UIEditor_BindingMethodAttribute : Attribute { }
    public sealed class UIEditor_BindingPropertyAttribute : Attribute
    {
        Type[] mAvailableTypes;
        public Type[] AvailableTypes
        {
            get { return mAvailableTypes; }
        }

        public UIEditor_BindingPropertyAttribute(Type[] availableTypes = null)
        {
            mAvailableTypes = availableTypes;
        }
    }

    public sealed class UIEditor_WhenWinBaseParentIsTypeShow : Attribute
    {
        Type[] mParentTypes;
        public Type[] ParentTypes
        {
            get { return mParentTypes; }
        }

        public UIEditor_WhenWinBaseParentIsTypeShow(Type[] parentTypes)
        {
            mParentTypes = parentTypes;
        }
    }

    public sealed class UIEditor_PropertysWithAutoSet : Attribute { }

    public sealed class UIEditor_CommandEventAttribute : Attribute { }
    public sealed class UIEditor_CommandMethodAttribute : Attribute { }

    //public sealed class UIEditor_DefaultValue : Attribute
    //{
    //    Type mValueType = null;
    //    public Type ValueType
    //    {
    //        get { return mValueType; }
    //    }
    //    object mValue = null;
    //    public object Value
    //    {
    //        get { return mValue; }
    //    }

    //    public UIEditor_DefaultValue(object value)
    //    {
    //        if (value != null)
    //        {
    //            mValueType = value.GetType();
    //            mValue = value;
    //        }
    //    }

    //    public static bool IsEqualDefaultValue(object instance, string propertyName, object value)
    //    {
    //        var property = TypeDescriptor.GetProperties(instance)[propertyName];
    //        if(property == null)
    //            return false;

    //        AttributeCollection attributes = property.Attributes;
    //        UIEditor_DefaultValue defAtt = attributes[typeof(UIEditor_DefaultValue)] as UIEditor_DefaultValue;
    //        if (defAtt != null)
    //        {
    //            if (property.PropertyType != value.GetType())
    //                return false;

    //            return object.Equals(value, defAtt.Value);
    //        }

    //        return false;
    //    }
    //}

    public sealed class UIEditor_Scale9InfoEditor : Attribute { }
    public sealed class UIEditor_UIControlTypesSelecterEditor : Attribute { }

    public sealed class UIEditor_FontParamCollectionAttribute : Attribute { }

    public sealed class UIEditor_OpenFileEditorAttribute : Attribute
    {
        public List<string> ExtNames = new List<string>();
        public UIEditor_OpenFileEditorAttribute(string ext)
        {
            var splits = ext.Split(',');
            foreach (var split in splits)
            {
                ExtNames.Add(split);
            }
        }
    }
    public sealed class UIEditor_DefaultFontPathAttribute : Attribute { }

    public sealed class UIEditor_ValueWithRange : Attribute
    {
        public double maxValue = 100;
        public double minValue = 0;
        public UIEditor_ValueWithRange(double min, double max)
        {
            minValue = min;
            maxValue = max;
        }
    }

    public sealed class UIEditor_DocumentEditorAttribute : Attribute { }

    public sealed class UIEditor_DocumentTextEditorAttribute : Attribute { }
    public sealed class UIEditor_GridDefinitionEditorAttribute : Attribute
    {
        public enum GridDefinitionType
        {
            Column,
            Row,
        }
        public GridDefinitionType DefinitionType = GridDefinitionType.Column;

        public UIEditor_GridDefinitionEditorAttribute(GridDefinitionType type)
        {
            DefinitionType = type;
        }
    }

    #endregion

    #region AIEditor

    public sealed class AIEditor_AIUseAblePropertyAttribute : Attribute { }
    public sealed class AIEditor_AIUseAbleMethodAttribute : Attribute { }

    #endregion

#region DelegateMethodEditor

    public sealed class DelegateMethodEditor_AllowedDelegate : Attribute
    {
        // delegate类型
        public string TypeStr;

        public DelegateMethodEditor_AllowedDelegate(string typeStr)
        {
            TypeStr = typeStr;
        }
    }

    public sealed class DelegateMethodEditor_DelegateType : Attribute
    {
        public Type DelegateType;

        public DelegateMethodEditor_DelegateType(Type dType)
        {
            DelegateType = dType;
        }
    }

#endregion

}
