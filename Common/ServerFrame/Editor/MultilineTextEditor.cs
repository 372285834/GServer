using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;

namespace ServerFrame.Editor
{
    public class MultilineTextEditor : System.Drawing.Design.UITypeEditor
    {
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService svc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (svc != null)
                {
                    if (value is string)
                    {
                        //RichTextBox box = new RichTextBox();
                        TextBox box = new TextBox();
                        box.AcceptsReturn = true;
                        box.Multiline = true;
                        box.Height = 120;
                        box.BorderStyle = BorderStyle.None;
                        box.Text = value as string;
                        svc.DropDownControl(box);

                        return box.Text;
                    }
                }
            }
            catch (System.Exception)
            {
                
            }
            return value;
        }
    }
}
