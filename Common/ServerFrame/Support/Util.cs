using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using SlimDX;

namespace ServerFrame
{

    public class Util
    {
        static UInt16 index = 0;
        static UInt16 serverId = 0; //一个进程一个id，默认用planesid，若一个进程有多个plane,会随机选择一个plane作为id
        readonly static object _sync = new object();
        static System.DateTime saveTime = new System.DateTime(2014, 1, 1);
        public static void InitServerID(UInt16 id) { serverId = id; Log.Log.Server.Info("ServerID = {0}", id); }

        //id是64位的（type：5位；serverID：11位；时间戳：32位；序列号：16位）
        public static UInt64 GenerateObjID(GameObjectType type)
	    {
            lock (_sync)
            {
                var span = System.DateTime.Now - saveTime;
                UInt32 ts = (UInt32)span.TotalSeconds;
                UInt64 objId = (UInt64)(((UInt64)(type) << 59) | ((UInt64)(serverId) << 48) | ((UInt64)(ts) << 16) | ((UInt64)(++index)));
                return objId;
            }
	    }

        public static UInt32 Obj_ID_SEQ(UInt64 id){return (UInt32)((id) & 0xFFFF);}
        public static UInt32 Obj_ID_TIMESTAMP(UInt64 id){return (UInt32)(((id) >> 16) & 0xFFFFFFFF);}
        public static UInt32 Obj_ID_SERVER(UInt64 id){return (UInt32)(((id) >> 48) & 0x3FF);}
        public static UInt32 Obj_ID_TYPE(UInt64 id){return (UInt32)(((id) >> 59) & 0x3F);}



        static readonly ThreadLocal<Random> appRandom = new ThreadLocal<Random>(() => new Random());
        public static System.Random Rand
        {
            get
            {
                return appRandom.Value;
            }
        }

        public static float RandInRadius(float center, float radius)
        {
            float min = center - radius;
            float max = center + radius;
            int res = Util.Rand.Next((int)(min * 10), (int)(max * 10));
            return (float)res / 10;
        }

        public static Vector3 RandInRadius(Vector3 center, float radius)
        {
            float range = Util.Rand.Next(0, (int)(radius * 100));
            float dir = Util.Rand.Next(0, 360);
            Vector3 res = Vector3.Zero;
            res.X = (float)(range / 100 * Math.Sin(dir * Math.PI / 180));
            res.Z = (float)(range / 100 * Math.Cos(dir * Math.PI / 180));
            return center + res;
        }

        public static float Angle(Vector3 left, Vector3 right)
        {
            if (left == Vector3.Zero || right == Vector3.Zero) return 0;

            left.Normalize();
            right.Normalize();
            float angle = (float)(Math.Acos(Vector3.Dot(left, right)) / Math.PI * 180);
            return angle;
        }

        public static float MyAngle(Vector3 cur, Vector3 tar)
	    {
            double tmp = Math.Atan2((cur.Z - tar.Z), (cur.X - tar.X));
		    tmp = tmp/Math.PI * 180;
		    return (float)tmp;
        }

        public static float AngleTo360(float angle)
        {
            angle = angle > 0 ? angle - 360 : angle;
            angle -= 90;
            angle = -angle;
            angle = angle > 360 ? angle - 360 : angle;
            return angle;
        }

        public static Vector3 DirToVector(float dir)
        {
            Vector3 vec = new Vector3(1, 0, 1);
            double rad = dir / 180 * Math.PI;
            vec.X *= (float)Math.Sin(rad);
            vec.Z *= (float)Math.Cos(rad);
            return vec;
        }

        public static int AngleToDir(float angle)
        {
            angle = AngleTo360(angle);
            int dir = (int)Math.Round((float)angle / 45) + 1;
            dir = dir < 9 ? dir : 1;
            return dir;
        }

        public static float DistanceH(Vector3 a, Vector3 b)
        {
            Vector3 tmp = a;
            tmp.Y = b.Y;
            return Vector3.Distance(tmp, b);
        }

        public static Dictionary<int, int> ParsingStr(string str)
        {
            Dictionary<int, int> items = new Dictionary<int, int>();
            if (string.IsNullOrEmpty(str))
            {
                return items;
            }
            string[] array = str.Split('|');
            for (int i = 0; i < array.Count(); i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                    continue;
                string[] info = array[i].Split(',');
                if (info.Count() != 2)
                    continue;
                if (string.IsNullOrEmpty(info[0]) || string.IsNullOrEmpty(info[1]))
                    continue;
                int id = Convert.ToInt32(info[0]);
                int count = Convert.ToInt32(info[1]);
                items[id] = count;
            }
            return items;
        }

        public static string ParsingIntIntDict(Dictionary<int, int> dict)
        {
            string result = "";
            foreach(var i in dict)
            {
                result += i.Key.ToString() + "," + i.Value.ToString() + "|";
            }
            return result;
        }

        [DllImport("PathFinder_d.dll",CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool InitSceneData(IntPtr pnt);

        [DllImport("PathFinder_d.dll",CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PassableForMod(int mapId, float dx, float dy);

        //public static bool PassableForMod(int mapId, Vector3 desPos)
        //{
        //    return PassableForMod(mapId, desPos.X, desPos.Z);
        //}

        public static bool PassableForMod(Navigator nav, Vector3 desPos)
        {
            if (null == nav)
                return false;

            return nav.PassableForMod(desPos);
        }

        public static bool FindRandomPoint(Navigator nav, Vector3 start, Vector3 center, float radius, ref Vector3 point)
        {
            if (null == nav)
                return false;

            return nav.FindRandomPoint(start, center, radius, ref point);
        }

        [DllImport("PathFinder_d.dll", EntryPoint = "FindPath", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FindPath(int mapId, float nx, float ny, float dx, float dy, ref int len);

        [DllImport("PathFinder_d.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetCollisionLayerData(int mapId, ref int width,ref int height,ref float inv,ref int offsetx,ref int offsetz);

        public static byte[] GetCollision(int mapId, out int w, out int h, out float inv,out int offsetx,out int offsetz)
        {//byte中 1 表示可行走区域，0,表示不可行走区域
            w = 0;
            h = 0;
            inv = 0;
            offsetx=0;
            offsetz=0;
            var ptr = GetCollisionLayerData(mapId, ref w, ref h, ref inv,ref offsetx,ref offsetz);
            if (ptr == IntPtr.Zero)
                return null;

            var ret = new byte[w * h];
            Marshal.Copy(ptr, ret, 0, w * h);
            return ret;
        }

        struct WorldPoint
        {
            public float x;
            public float y;
        };

        //public static bool FindPath(int mapId, Vector3 nowPos, Vector3 desPos, ref List<Vector3> nodes)
        //{
        //    int len = 0;
        //    IntPtr pBuff = FindPath(mapId, nowPos.X, nowPos.Z, desPos.X, desPos.Z, ref len);
        //    if (pBuff == (IntPtr)0 || len == 0) return false;

        //    for (int i = 0; i < len; i++)
        //    {
        //        IntPtr pPointor = new IntPtr(pBuff.ToInt64() + Marshal.SizeOf(typeof(WorldPoint)) * i);
        //        WorldPoint node = (WorldPoint)Marshal.PtrToStructure(pPointor, typeof(WorldPoint));
        //        if (i > 0)
        //            nodes.Add(new Vector3(node.x, desPos.Y, node.y));
        //    }
        //    if (nodes.Count() > 1)
        //        return true;

        //    return true;
        //}
    }
}
