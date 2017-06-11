using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public class Social
    {
        public Social(CSCommon.Data.SocialData data)
        {
            mSocialData = data;
        }

        CSCommon.Data.SocialData mSocialData = null;
        public CSCommon.Data.SocialData SocialData
        {
            get { return mSocialData; }
        }

        public void SetSocial(CSCommon.eSocialType type, bool want)
        {
            if (want)
            {
                mSocialData.Socail |= (uint)type; //相应位得1
            }
            else
            {
                mSocialData.Socail &= (0xffffff ^ (uint)type);     //相应位取0
            }


        }

        public bool IsSocial(CSCommon.eSocialType type)
        {
            if ((mSocialData.Socail & (uint)type) > 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateIntimacy()
        {
            System.DateTime last = mSocialData.LastAddIntimacyTime;

            System.DateTime templast = new System.DateTime(last.Year, last.Month, last.Day);

            System.DateTime now = System.DateTime.Now;

            System.DateTime tempnow = new System.DateTime(now.Year, now.Month, now.Day);

            if ((tempnow - templast).Days == 1)
            {
                mSocialData.YesterdayIntimacy = mSocialData.TodayIntimacy;
                mSocialData.TodayIntimacy = 0;
            }
            else if ((tempnow - templast).Days > 1)
            {
                mSocialData.YesterdayIntimacy = 0;
                mSocialData.TodayIntimacy = 0;
            }

        }


    }


    public class SocialManager
    {
        protected UserRole mHostRole;
        Dictionary<ulong, Social> mSocialDict = new Dictionary<ulong, Social>();

        public void InitSocialList(UserRole owner)
        {
            mHostRole = owner;
            foreach (var i in mHostRole.Socials)
            {
                Social s = new Social(i);
                s.UpdateIntimacy();
                mSocialDict[i.OtherId] = s;
            }
        }

        public List<CSCommon.Data.SocialData> SaveSocialList()
        {
            List<CSCommon.Data.SocialData> list = new List<CSCommon.Data.SocialData>();
            foreach (var i in mSocialDict.Values)
            {
                list.Add(i.SocialData);
            }
            return list;
        }

        public List<CSCommon.Data.SocialData> GetSocialList(CSCommon.eSocialType type)
        {
            List<CSCommon.Data.SocialData> list = new List<CSCommon.Data.SocialData>();
            foreach (var i in mSocialDict.Values)
            {
                if (IsCoupleType(type))
                {
                    for (CSCommon.eSocialType stype = CSCommon.eSocialType.ManWife; stype <= CSCommon.eSocialType.Lily; )
                    {
                        if (i.IsSocial(stype))
                        {
                            list.Add(i.SocialData);
                            break;
                        }
                        stype = (CSCommon.eSocialType)(((uint)stype) << 1);
                    }
                }
                else
                {
                    if (i.IsSocial(type))
                    {
                        list.Add(i.SocialData);
                    }
                }
            }
            return list;
        }

        public CSCommon.Data.SocialData AddSocial(ulong otherId, CSCommon.eSocialType type)
        {
            CSCommon.Data.SocialData sd = null;
            if (mSocialDict.ContainsKey(otherId))
            {
                Social s = mSocialDict[otherId];
                if (s.IsSocial(type))
                {
                    return null;
                }
                else
                {
                    s.SetSocial(type, true);
                    sd = s.SocialData;
                }
            }
            else
            {
                sd = new CSCommon.Data.SocialData();
                sd.OwnerId = mHostRole.RoleData.RoleId;
                sd.OtherId = otherId;
                Social s = new Social(sd);
                s.SetSocial(type, true);
                mSocialDict[sd.OtherId] = s;
            }

            return sd;
        }

        public void RemoveCouple(ulong otherId)
        {
            if (mSocialDict.ContainsKey(otherId))
            {
                Social s = mSocialDict[otherId];
                for (CSCommon.eSocialType stype = CSCommon.eSocialType.ManWife; stype <= CSCommon.eSocialType.Lily; )
                {
                    if (s.IsSocial(stype))
                    {
                        s.SetSocial(stype, false);
                    }
                    stype = (CSCommon.eSocialType)(((uint)stype) << 1);
                }

            }
        }

        public bool RemoveSocial(ulong otherId, CSCommon.eSocialType type)
        {
            if (mSocialDict.ContainsKey(otherId))
            {
                Social s = mSocialDict[otherId];
                if (s.IsSocial(type) == false)
                {
                    return false;
                }
                else
                {
                    s.SetSocial(type, false);
                }
            }

            return true;
        }

        public bool IsCouple(ulong otherId)
        {
            if (mSocialDict.ContainsKey(otherId) == false)
            {
                return false;
            }

            for (CSCommon.eSocialType type = CSCommon.eSocialType.ManWife; type <= CSCommon.eSocialType.Lily; )
            {
                if (mSocialDict[otherId].IsSocial(type))
                {
                    return true;
                }
                type = (CSCommon.eSocialType)(((uint)type) << 1);
            }

            return false;
        }
        public bool IsCoupleType(CSCommon.eSocialType stype)
        {
            for (CSCommon.eSocialType type = CSCommon.eSocialType.ManWife; type <= CSCommon.eSocialType.Lily; )
            {
                if (stype == type)
                {
                    return true;
                }
                type = (CSCommon.eSocialType)(((uint)type) << 1);
            }
            return false;
        }

        public bool IsSocial(ulong otherId, CSCommon.eSocialType type)
        {
            if (mSocialDict.ContainsKey(otherId) == false)
            {
                return false;
            }
            if (IsCoupleType(type))
            {
                if (IsCouple(otherId))
                {
                    return true;
                }
            }
            else if (mSocialDict[otherId].IsSocial(type))
            {
                return true;
            }
            return false;
        }


//         public sbyte VisitFriend(ulong otherId)
//         {
//             if (IsSocial(otherId, CSCommon.eSocialTyoe.Friend) == false)
//             {
//                 return (sbyte)(-1);
//             }
// 
//             System.DateTime lastvisittime = mSocialDict[otherId].SocialData.LastVisitTime;
//             System.DateTime now = System.DateTime.Now;
//             if ((now.Year == lastvisittime.Year) && (now.Month == lastvisittime.Month) && (now.Day == lastvisittime.Day))
//             {
//                 return (sbyte)(-2);
//             }
//             int addvalue = CSCommon.SocialCommon.Instance.AddIntimacyByVisit;
//             mSocialDict[otherId].SocialData.LastVisitTime = now;
// 
//             AddIntimacy(otherId, addvalue);
//             return 1;
//         }

        public void AddIntimacy(ulong otherId, int addvalue)
        {
            mSocialDict[otherId].SocialData.TodayIntimacy += addvalue;
            mSocialDict[otherId].SocialData.SocailIntimacy += addvalue;
            mSocialDict[otherId].SocialData.LastAddIntimacyTime = System.DateTime.Now;
        }

    }


}
