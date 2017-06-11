//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace CSCommon.Data.MapObject.Trigger
//{
//    public class TriggerActorInit : CSCommon.Component.IActorInitBase
//    {
//        CSCommon.Data.Trigger.TriggerData mTriggerData;
//        public CSCommon.Data.Trigger.TriggerData TriggerData
//        {
//            get { return mTriggerData; }
//            set { mTriggerData = value; }
//        }

//        public TriggerActorInit()
//        {
//            GameType = CSCommon.Component.eActorGameType.Trigger;
//        }

//        public virtual Data.Trigger.TriggerData CreateTriggerData()
//        {
//            return new Data.Trigger.TriggerData();
//        }

//        public override void CopyFrom(ServerFrame.Support.IXndSaveLoadProxy srcData)
//        {
//            base.CopyFrom(srcData);

//            var srcTriggerInit = srcData as TriggerActorInit;

//            TriggerData = CreateTriggerData();
//            TriggerData.RoleId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Trigger);
//            TriggerData.Copy(srcTriggerInit.TriggerData);
//        }

//        public override bool Read(ServerFrame.Support.IXndAttrib xndAtt)
//        {
//            if (!base.Read(xndAtt))
//                return false;

//            SByte hasTriggerData;
//            xndAtt.Read(out hasTriggerData);

//            if (hasTriggerData > 0)
//            {
//                if (TriggerData == null)
//                    TriggerData = CreateTriggerData();
//                TriggerData.ReadXND(xndAtt);
//            }

//            return true;
//        }

//        public override bool Write(ServerFrame.Support.IXndAttrib xndAtt)
//        {
//            if (!base.Write(xndAtt))
//                return false;

//            SByte hasTriggerData = (SByte)((TriggerData != null) ? 1 : -1);
//            xndAtt.Write(hasTriggerData);

//            if (TriggerData != null)
//            {
//                TriggerData.WriteXND(xndAtt);
//            }

//            return true;
//        }
//    }
//}
