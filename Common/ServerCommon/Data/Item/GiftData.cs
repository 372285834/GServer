using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    [ServerFrame.DB.DBBindTable("GiftData")]
    public class GiftData : RPC.IAutoSaveAndLoad
    {
        ulong mOwnerId;
        [ServerFrame.DB.DBBindField("OwnerId")]
        [RPC.AutoSaveLoad]
        public ulong OwnerId
        {
            get { return mOwnerId; }
            set { mOwnerId = value; }
        }

        int mGift1 = 0;
        [ServerFrame.DB.DBBindField("Gift1")]
        [RPC.AutoSaveLoad(true)]
        public int Gift1
        {
            get { return mGift1; }
            set { mGift1 = value; }
        }

        int mGift2 = 0;
        [ServerFrame.DB.DBBindField("Gift2")]
        [RPC.AutoSaveLoad(true)]
        public int Gift2
        {
            get { return mGift2; }
            set { mGift2 = value; }
        }

        int mGift3 = 0;
        [ServerFrame.DB.DBBindField("Gift3")]
        [RPC.AutoSaveLoad(true)]
        public int Gift3
        {
            get { return mGift3; }
            set { mGift3 = value; }
        }

        int mGift4 = 0;
        [ServerFrame.DB.DBBindField("Gift4")]
        [RPC.AutoSaveLoad(true)]
        public int Gift4
        {
            get { return mGift4; }
            set { mGift4 = value; }
        }

        int mGift5 = 0;
        [ServerFrame.DB.DBBindField("Gift5")]
        [RPC.AutoSaveLoad(true)]
        public int Gift5
        {
            get { return mGift5; }
            set { mGift5 = value; }
        }


        public void AddGiftCount(int index, int count)
        {
            switch (index)
            {
                case 0:
                    mGift1 += count;
                    break;
                case 1:
                    mGift2 += count;
                    break;
                case 2:
                    mGift3 += count;
                    break;
                case 3:
                    mGift4 += count;
                    break;
                case 4:
                    mGift5 += count;
                    break;

            }
        }





    }
}
