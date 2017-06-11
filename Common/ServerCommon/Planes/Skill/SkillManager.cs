using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;

namespace ServerCommon.Planes
{
    public class SkillHitData : RPC.IAutoSaveAndLoad
    {
        public Byte HitType;
        public int Damage;
    }

    public class SkillManager : ServerFrame.HashList<ASkillObject>
    {
        RoleActor mOwner;
        public ASkillObject GetSkill(int skid)
        {
            return this.getElementByKey(skid);
        }
        public void InitBag(RoleActor owner, List<SkillData> skills)
        {
            mOwner = owner;
            this.clear();
            foreach(var i in skills)
            {
                if (false == CreateSkillToBag(i.TemplateId, i.Level))
                {
                    Log.Log.Login.Print("InitBag skill error, owner{0}skillid{1}", owner.Id, i.TemplateId);
                    continue;
                }               
            }
        }

        public bool CreateSkillToBag(int id, byte lv)
        {
            var sk = CreateSkill(mOwner, id, lv);
            if (sk == null)
            {
                Log.Log.Login.Print("CreateSkillToBag sk = null{0}", id);
                return false;
            }
            this.addElement(id, sk);
            return true;
        }

        public List<SkillData> GetBagSaver()
        {
            List<SkillData> skills = new List<SkillData>();
            foreach(var i in this.list)
            {
                SkillData data = new SkillData();
                data.OwnerId = mOwner.Id;
                data.TemplateId = i.ID;
                data.Level = (byte)i.Level;
                skills.Add(data);
            }
            return skills;
        }
        public void Init(RoleActor owner, NPCData rd)
        {
            this.clear();
            //怪物技能默认1级
            foreach (var skid in rd.Template.skill)
            {
                SkillActive sk = CreateSkill(owner, skid, 1) as SkillActive;
                if (sk == null)
                {
                    continue;
                }
                this.addElement(skid, sk);                
            }
            
        }

        public static ASkillObject CreateSkill(RoleActor owner, int skid, byte lv)
        {
            CSTable.ISkillTable sktpl = CSTable.StaticDataManager.SkillActive[skid];
            if (sktpl == null)
            {
                sktpl = CSTable.StaticDataManager.SkillPassive[skid];
                if (sktpl == null)
                {
                    Log.Log.Server.Warning("not find skill id={0}", skid);
                    return null;
                }
            }

            CSCommon.eSkillType stype = (CSCommon.eSkillType)sktpl.type;
            ASkillObject sk = null;
            switch (stype)
            {
                case eSkillType.Normal:
                case eSkillType.Active:
                case eSkillType.Hide:
                    sk = new ServerCommon.Planes.SkillActive();
                    break;
                case eSkillType.Cheats:
                    sk = new ServerCommon.Planes.SkillCheats();
                    break;
                case eSkillType.BodyChannel:
                    sk = new ServerCommon.Planes.SkillBodyChannel();
                    break;
                default:
                    break;
            }

            if (sk == null)
            {
                Log.Log.Fight.Warning("not find skill type ;sktpl.type ={0}", sktpl.type);
                return null;
            }
            sk.Init(skid, lv);
            sk.SetOwner(owner);
            return sk;
        }

    }
}
