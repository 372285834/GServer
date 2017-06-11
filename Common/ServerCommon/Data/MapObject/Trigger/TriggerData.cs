//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.Trigger
//{
//    public enum TriggerDataType
//    {
//        Common,
//        PostProcess,
//    }

//    ///[EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Trigger")]
//    public delegate void FOnEnter(CSCommon.Data.MapObject.Trigger.TriggerProcessData processData, TriggerData trigger);
//    //[EditorCommon.Assist.DelegateMethodEditor_AllowedDelegate("Trigger")]
//    public delegate void FOnLeave(CSCommon.Data.MapObject.Trigger.TriggerProcessData processData, TriggerData trigger);

//    public class TriggerData : CSCommon.Data.NPCData
//    {
//        public delegate void Delegate_OnJump2Map(int planes,int map);
//        public Delegate_OnJump2Map OnJumpMap;
//        //Guid mId = Guid.Empty;
//        //[ServerFrame.Support.AutoSaveLoadAttribute]
//        //[ReadOnly(true)]
//        //public Guid Id
//        //{
//        //    get { return mId; }
//        //    set { mId = value; }
//        //}

//        //string mName = "";
//        //[ServerFrame.Support.AutoCopyAttribute]
//        //[ServerFrame.Support.AutoSaveLoadAttribute]
//        //public string Name
//        //{
//        //    get { return mName; }
//        //    set
//        //    {
//        //        mName = value;

//        //        if (OnPropertyUpdate != null)
//        //            OnPropertyUpdate(this);
//        //    }
//        //}

//        SlimDX.Matrix mTransMatrix = SlimDX.Matrix.Identity;
//        [ServerFrame.Support.AutoCopyAttribute]
//        [ServerFrame.Support.AutoSaveLoadAttribute]
//        [Browsable(false)]
//        public SlimDX.Matrix TransMatrix
//        {
//            get { return mTransMatrix; }
//            set
//            {
//                mTransMatrix = value;

//                if (OnPropertyUpdate != null)
//                    OnPropertyUpdate(this);

//                OnPropertyChanged("TransMatrix");
//            }
//        }

//        bool mEnable = true;
//        [ServerFrame.Support.AutoCopyAttribute]
//        [ServerFrame.Support.AutoSaveLoadAttribute]
//        public bool Enable
//        {
//            get { return mEnable; }
//            set
//            {
//                mEnable = value;

//                if (OnPropertyUpdate != null)
//                    OnPropertyUpdate(this);

//                OnPropertyChanged("Enable");
//            }
//        }

//        bool mNeedServer = true;
//        [ServerFrame.Support.AutoCopyAttribute]
//        [ServerFrame.Support.AutoSaveLoadAttribute]
//        public bool NeedServer
//        {
//            get { return mNeedServer; }
//            set
//            {
//                mNeedServer = value;

//                if (OnPropertyUpdate != null)
//                    OnPropertyUpdate(this);

//                OnPropertyChanged("NeedServer");
//            }
//        }

//        // delta为检测范围调整阈值，服务器验证时使用
//        public bool IsPositionIn(SlimDX.Vector3 pos, float delta = 1.0f)
//        {
//            var invMat = SlimDX.Matrix.Invert(ref mTransMatrix);
//            var triggerSpacePos = SlimDX.Vector3.TransformCoordinate(pos, invMat);
//            if ((Math.Abs(triggerSpacePos.X) < (0.5f * delta)) &&
//                (Math.Abs(triggerSpacePos.Y) < (0.5f * delta)) &&
//                (Math.Abs(triggerSpacePos.Z) < (0.5f * delta)))
//                return true;

//            return false;
//        }

//        //public virtual void Tick(long elapsedMillisecond)
//        //{

//        //}

//    }
//}
