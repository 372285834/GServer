using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerFrame.Config
{
    public class StringTemplate
    {
        string mContentText = "";
        [ServerFrame.Config.DataValueAttribute("ContentText")]
        public string ContentText
        {
            get { return mContentText; }
            set { mContentText = value; }
        }
    }
}
