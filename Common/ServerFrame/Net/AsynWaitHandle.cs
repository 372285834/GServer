using System;
using System.Collections.Generic;

using System.Text;

namespace RPC
{
    public delegate void FFarCallFinished(PackageProxy ret,bool bTimeOut);
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class RPCWaitHandle
    {
        public FFarCallFinished OnFarCallFinished = null;
        System.UInt16 mCallID;
        public System.UInt16 CallID
        {
            get { return mCallID; }
            set { mCallID = value; }
        }
        public float mTimeOut = -1.0F;
        System.String mSrcFile;
        public string SrcFile
        {
            get { return mSrcFile; }
            set { mSrcFile = value; }
        }
        public int mSrcLine = 0;

        public void Raise_OnFarCallFinished(PackageProxy ret,bool bTimeout)
        {
            if (OnFarCallFinished != null)
            {
                OnFarCallFinished(ret,bTimeout);
            }
            else
            {
                //System::Diagnostics::Debugger::Break();
                Log.Log.Common.Print("OnFarCallFinished==null");
            }
        }
    }
}
