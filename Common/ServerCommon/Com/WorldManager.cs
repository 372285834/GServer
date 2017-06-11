using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public enum eDefendLevel
    {
        Boss,       //主将
        Jianjun,    //监军
        Sima,       //司马
        Langjiang1, //郎将
        Langjiang2,
        Canjiang1,  //参军
        Canjiang2,
        MAX_COUNT,
    }

    //据点
    public struct HoldPoint
    {
        public uint ArmsCount;
        public CSCommon.eCamp Camp;        
    }

    //城市
    public class City
    {
        public static readonly int AdvaceArmsCount = 10;

        public ushort Mapid;
        public CSCommon.eCamp Camp;
        public List<HoldPoint> HoldPoints = new List<HoldPoint>();
        public ulong[] Defender = new ulong[(int)eDefendLevel.MAX_COUNT]; //官职拥有人，为0表示没有人驻守
        public uint[] AdvanceArms = new uint[AdvaceArmsCount];             //禁卫军


        public void Init(CityData data)
        {
            Mapid = data.Mapid;
            Camp = (CSCommon.eCamp)data.Camp;
            DeserializeArms(data.Arms);
        }

        public CityData GetSaveData()
        {
            CityData ret = new CityData();
            ret.Mapid = Mapid;
            ret.Camp = (byte)Camp;
            ret.Arms = SerializeArms();
            return ret;
        }

        public byte[] SerializeArms()
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            dw.Write((byte)HoldPoints.Count);
            foreach (var i in HoldPoints)
            {
                dw.Write(i.ArmsCount);
                dw.Write((byte)i.Camp);
            }
            dw.Write((byte)Defender.Length);
            foreach (var i in Defender)
            {
                dw.Write(i);
            }

            dw.Write((byte)AdvanceArms.Length);
            foreach (var i in AdvanceArms)
            {
                dw.Write(i);
            }

            if (dw.Length >= 255)
            {
                Log.Log.Common.Error("City 序列化数据最大不能超过255!");
            }

            return dw.Trim();
        }

        public void DeserializeArms(byte[] buffer)
        {
            HoldPoints.Clear();            

            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            var count = dr.ReadByte();

            for (int i=0;i<count;i++)
            {
                var hold = new HoldPoint();
                HoldPoints.Add(hold);
                hold.ArmsCount = dr.ReadUInt32();
                hold.Camp = (CSCommon.eCamp)dr.ReadByte();
            }
            count = dr.ReadByte();
            for (int i = 0; i < count; i++)
            {
                var val = dr.ReadUInt64();
                if (i<Defender.Length)
                {
                    Defender[i] = val;
                }
            }
            count = dr.ReadByte();
            for (int i = 0; i < count; i++)
            {
                var val = dr.ReadUInt32();
                if (i < AdvanceArms.Length)
                {
                    AdvanceArms[i] = val;
                }
            }
        }

    }

    //国家
    public class Country
    {
        City mCapital;
        List<City> AllCitys = new List<City>();
        CSCommon.eCamp Camp;
        public void AddCity(CityData data)
        {
            City ct = new City();
            ct.Init(data);
            AllCitys.Add(ct);
        }
    }

    //一个服一个world，即一个planedata
    public class World
    {
        int Level;
        int PlaneId;
        Dictionary<CSCommon.eCamp, Country> AllCountrys = new Dictionary<CSCommon.eCamp, Country>();

        public void Init(int planeid, List<CityData> citys)
        {
            AllCountrys.Clear();

            PlaneId = planeid;
            AllCountrys[CSCommon.eCamp.Song] = new Country();
            AllCountrys[CSCommon.eCamp.Jing] = new Country();


            foreach (var city in citys)
            {
                Country ct;
                if (AllCountrys.TryGetValue((CSCommon.eCamp)city.Camp, out ct))
                {
                    ct.AddCity(city);
                }
                else
                {
                    Log.Log.Server.Warning("有城市没有国家!!camp={0}", city.Camp);
                }

            }
        }


    }


    [RPC.RPCClassAttribute(typeof(WorldManager))]
    public class WorldManager : RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        static WorldManager smInstance = new WorldManager();
        static public WorldManager Instance
        {
            get {return smInstance;}
        }

        Dictionary<ushort, World> mWorlds = new Dictionary<ushort, World>();
        public Dictionary<ushort, World> Worlds
        {
            get { return mWorlds; }
        }


        public void Init(PlanesData[] planedatas)
        {
            foreach (var p in planedatas)
            {
                string condition = "PlaneId=" + p.PlanesId;
                ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CityData(),"");
                System.Data.DataTable tab = UserRoleManager.Instance.DBConnect._ExecuteSelect(dbOp, "CityData");
                if (tab != null)
                {
                    List<CityData> citys = new List<CityData>();
                    foreach (System.Data.DataRow r in tab.Rows)
                    {
                        CSCommon.Data.CityData city = new CSCommon.Data.CityData();
                        if (false == ServerFrame.DB.DBConnect.FillObject(city, r))
                            continue;
                        citys.Add(city);
                    }

                    var world = new World();
                    world.Init(p.PlanesId, citys);
                    mWorlds[p.PlanesId] = world;
                }
                else
                {
                    Log.Log.Mail.Print("WorldManager init PlanesData failed :{0}", condition);
                }
            }
        }

        public void DB_Save()
        {
            foreach (var w in mWorlds.Values)
            {

            }
        }
        
    }
}
