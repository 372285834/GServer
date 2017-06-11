using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Role.Animation
{
    //public class ActionSource
    //{
    //    Int64 mDuration = 0;
    //    public Int64 Duration
    //    {
    //        get { return mDuration; }
    //    }
    //    List<CSCommon.Animation.ActionNotifier> mNotifierList = new List<CSCommon.Animation.ActionNotifier>();
    //    public List<CSCommon.Animation.ActionNotifier> NotifierList
    //    {
    //        get { return mNotifierList; }
    //    }

    //    public bool LoadActionNotifier(string fileName)
    //    {
    //        mNotifierList.Clear();

    //        fileName += CSUtility.Support.IFileConfig.Instance.ActionNotifyExtension;

    //        var holder = CSUtility.Support.IXndHolder.LoadXND(fileName, false);
    //        if (holder == null)
    //            return false;

    //        var attr = holder.Node.FindAttrib("Header");
    //        if (attr != null)
    //        {
    //            attr.BeginRead();

    //            Guid meshId;
    //            attr.Read(out meshId);
    //            attr.Read(out mDuration);

    //            attr.EndRead();
    //        }

    //        var notifyNode = holder.Node.FindNode("Notifys");
    //        if (notifyNode != null)
    //        {
    //            var notifyAtts = notifyNode.GetAttribs();
    //            foreach (var ntfAtt in notifyAtts)
    //            {
    //                var notifyType = ntfAtt.GetName();
    //                var splits = notifyType.Split('|');
    //                if (splits.Length != 2)
    //                    continue;

    //                var assembly = System.Reflection.Assembly.Load(splits[0]);
    //                if (assembly == null)
    //                    continue;

    //                var notify = assembly.CreateInstance(splits[1]) as CSCommon.Animation.ActionNotifier;
    //                if (notify == null)
    //                    continue;

    //                ntfAtt.BeginRead();
    //                ntfAtt.Read(notify);
    //                ntfAtt.EndRead();

    //                notify.ActionDurationMilliSecond = mDuration;

    //                mNotifierList.Add(notify);
    //            }
    //        }

    //        return true;
    //    }
    //}

    public class ActionNode : AnimNode , CSCommon.Animation.BaseAction
    {
        public ActionNode()
        {
            base.Action = this;
        }
        string mActionName;
        public string ActionName 
        {
            get { return mActionName; }
            set
            {
                mActionName = value;
                //真的加载动作文件
                //LoadActionNotifier(mActionName);
                mActionSource = CSCommon.Animation.ActionNodeManager.Instance.GetActionSource(value, false);
                
            }
        }
        
        CSCommon.Animation.AxisRootmotionType mXRootmotionType; 
        public CSCommon.Animation.AxisRootmotionType XRootmotionType 
        {
            get { return mXRootmotionType; }
            set { mXRootmotionType = value; } 
        }
        CSCommon.Animation.AxisRootmotionType mYRootmotionType;
        public CSCommon.Animation.AxisRootmotionType YRootmotionType 
        {
            get { return mYRootmotionType; }
            set { mYRootmotionType = value; }
        }
        CSCommon.Animation.AxisRootmotionType mZRootmotionType;
        public CSCommon.Animation.AxisRootmotionType ZRootmotionType 
        {
            get { return mZRootmotionType; }
            set { mZRootmotionType = value; }
        }
        CSCommon.Animation.EActionPlayerMode mPlayerMode;
        public CSCommon.Animation.EActionPlayerMode PlayerMode 
        {
            get { return mPlayerMode; }
            set { mPlayerMode = value; }
        }

        public Int64 Duration 
        {
            get 
            {
                if (mActionSource == null)
                    return 0;
                return mActionSource.Duration; 
            }
        }
        double mPlayRate = 1D;
        public double PlayRate 
        {
            get { return mPlayRate; }
            set { mPlayRate = value; }
        }

        CSCommon.Animation.ActionSource mActionSource = null;

        public CSCommon.Animation.ActionNotifier GetNotifier(string name)
        {
            if (mActionSource == null)
                return null;

            return mActionSource.GetFirstNotifier(name);
            //foreach (var notifier in mActionSource.NotifierList)
            //{
            //    if (notifier.NotifyName == name)
            //        return notifier;
            //}

            //return null;
        }
        public CSCommon.Animation.ActionNotifier GetNotifier(UInt32 index)
        {
            if (mActionSource == null)
                return null;

            return mActionSource.GetNotifier((int)index);
            //if (index < mActionSource.NotifierList.Count)
            //    return mActionSource.NotifierList[(int)index];

            //return null;
        }
        public List<CSCommon.Animation.ActionNotifier> GetNotifiers(System.Type type)
        {
            if (mActionSource == null)
                return new List<CSCommon.Animation.ActionNotifier>();

            return mActionSource.GetNotifier(type);
        }

        public override bool IsActionFinished()
        {
            if (mActionSource == null)
                return true;

            return CurNotifyTime >= mActionSource.Duration;
        }
    }
}
