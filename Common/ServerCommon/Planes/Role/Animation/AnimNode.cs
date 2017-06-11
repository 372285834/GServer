using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes.Role.Animation
{
    public class AnimNode : CSCommon.Animation.AnimationTree
    {
        ActionNode mAction;
        public CSCommon.Animation.BaseAction Action
        {
            get { return mAction; }
            set { mAction = value as ActionNode; }
        }

        Int64 mCurNotifyTime;
        public Int64 CurNotifyTime
        {
            get { return mCurNotifyTime; }
            set { mCurNotifyTime = value; }
        }

        List<CSCommon.Animation.AnimationTree> mAnimations = new List<CSCommon.Animation.AnimationTree>();
        public List<CSCommon.Animation.AnimationTree> GetAnimations()
        {
            return mAnimations;
        }
        //void SetAnimations(List<AnimationTree> anims);        
        public void AddNode(CSCommon.Animation.AnimationTree node)
        {
            mAnimations.Add(node);
        }

        public virtual bool IsActionFinished()
        {
            if (mAnimations.Count == 0)
                return false;

            foreach(var i in mAnimations)
            {
                if (i.IsActionFinished() == false)
                    return false;
            }

            return true;// return this->GetATFinished();
        }

        bool mbLoop = false;
        public void SetLoop(bool bLoop)
        {
            mbLoop = bLoop;
        }

        public bool GetLoop()
        {
            return mbLoop;
        }

        CSCommon.Animation.Delegate_OnAnimTreeFinish mDelegateOnAnimTreeFinish;
        public CSCommon.Animation.Delegate_OnAnimTreeFinish DelegateOnAnimTreeFinish
        {
            get { return mDelegateOnAnimTreeFinish; }
            set { mDelegateOnAnimTreeFinish = value; }
        }

        CSCommon.Animation.Delegate_OnActionFinish mDelegateOnActionFinish;
        public CSCommon.Animation.Delegate_OnActionFinish DelegateOnActionFinish 
        {
            get { return mDelegateOnActionFinish; }
            set { mDelegateOnActionFinish = value; }
        }
    }
}
