using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public abstract class AComponent 
    {
        public virtual int GetComponentId()
        {
            return this.GetType().FullName.GetHashCode();
        }
        public AComponent GetComponent(int Id)
        {
            return null;
        }
        public AComponent GetComponent(System.Type type)
        {
            return null;
        }
        public bool AddComponent(System.Type type, AComponent comp)
        {
            return true;
        }
        public void RemoveComponent(System.Type type)
        {
            return;
        }

        public virtual void Tick(AActorBase host, long elapsedMillisecond) { }
    }
}
