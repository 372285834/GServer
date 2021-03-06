/*
 * 本代码由工具自动生成，请勿手工修改
 * */
/// <summary>
/// 模版数据;..\..\..\..\Client\Assets\StreamingAssets\TableData/ItemPackage.txt
/// </summary>
using System;

namespace CSTable
{
	/// <summary>
	/// 构造函数;
	/// </summary>
	public class ItemPackageData:IItemBase{
		public ItemPackageData()		
		{				
		}

		public ItemPackageData(string _value)		
		{				
			string[] array = _value.Split(new char[] { StaticDataConstant.GAP_TAB });
			id = StaticDataManager.stringToInt(array[0]);
				ItemName = array[1];
				ItemType = StaticDataManager.stringToInt(array[2]);
				ItemQuality = StaticDataManager.stringToInt(array[3]);
				ItemDesc = array[4];
				ItemIconName = array[5];
				ItemMaxStackNum = StaticDataManager.stringToInt(array[6]);
				ItemMaxShelflife = StaticDataManager.stringToInt(array[7]);
				ItemBindType = StaticDataManager.stringToInt(array[8]);
				ItemUseCamp = StaticDataManager.stringToInt(array[9]);
				ItemSex = StaticDataManager.stringToInt(array[10]);
				ItemProfession = StaticDataManager.stringToInt(array[11]);
				ItemUseRoleLv = StaticDataManager.stringToUshort(array[12]);
				ItemUseRoleOfficeLv = StaticDataManager.stringToByte(array[13]);
				ItemUseVipLv = StaticDataManager.stringToByte(array[14]);
				CanManualUse = StaticDataManager.stringToByte(array[15]);
				ItemUseId = StaticDataManager.stringToInt(array[16]);
				ItemIsSell = StaticDataManager.stringToByte(array[17]);
				SellCurrenceType = StaticDataManager.stringToInt(array[18]);
				SellingPrice = StaticDataManager.stringToInt(array[19]);
				PackageType = StaticDataManager.stringToByte(array[20]);
				DropId = StaticDataManager.stringToInt(array[21]);
				NeedItemId = StaticDataManager.stringToInt(array[22]);
				NeedCurrenceType = StaticDataManager.stringToByte(array[23]);
				NeedCount = StaticDataManager.stringToInt(array[24]);
				UseMaxNum = StaticDataManager.stringToByte(array[25]);
				CdDelay = StaticDataManager.stringToFloat(array[26]);
					
		}
				
		//物品Id;		
		public int id
		{			
			set;			
			get;		
		}		
		//物品名字;		
		public string ItemName
		{			
			set;			
			get;		
		}		
		//物品类型;		
		public int ItemType
		{			
			set;			
			get;		
		}		
		//物品品质;		
		public int ItemQuality
		{			
			set;			
			get;		
		}		
		//物品描述;		
		public string ItemDesc
		{			
			set;			
			get;		
		}		
		//物品图标;		
		public string ItemIconName
		{			
			set;			
			get;		
		}		
		//物品最大堆叠数量;		
		public int ItemMaxStackNum
		{			
			set;			
			get;		
		}		
		//物品最大保质期;		
		public int ItemMaxShelflife
		{			
			set;			
			get;		
		}		
		//物品绑定;		
		public int ItemBindType
		{			
			set;			
			get;		
		}		
		//阵营限制;		
		public int ItemUseCamp
		{			
			set;			
			get;		
		}		
		//性别限制;		
		public int ItemSex
		{			
			set;			
			get;		
		}		
		//职业限制;		
		public int ItemProfession
		{			
			set;			
			get;		
		}		
		//角色等级限制;		
		public ushort ItemUseRoleLv
		{			
			set;			
			get;		
		}		
		//角色官职等级限制;		
		public byte ItemUseRoleOfficeLv
		{			
			set;			
			get;		
		}		
		//vip等级限制;		
		public byte ItemUseVipLv
		{			
			set;			
			get;		
		}		
		//物品是否可手动使用;		
		public byte CanManualUse
		{			
			set;			
			get;		
		}		
		//物品使用对应模板;		
		public int ItemUseId
		{			
			set;			
			get;		
		}		
		//是否可出售;		
		public byte ItemIsSell
		{			
			set;			
			get;		
		}		
		//贩卖货币类型;		
		public int SellCurrenceType
		{			
			set;			
			get;		
		}		
		//出售价格;		
		public int SellingPrice
		{			
			set;			
			get;		
		}		
		//礼包兑换类型;		
		public byte PackageType
		{			
			set;			
			get;		
		}		
		//礼包掉落Id;		
		public int DropId
		{			
			set;			
			get;		
		}		
		//需求物品Id;		
		public int NeedItemId
		{			
			set;			
			get;		
		}		
		//需求货币类型;		
		public byte NeedCurrenceType
		{			
			set;			
			get;		
		}		
		//需求数量;		
		public int NeedCount
		{			
			set;			
			get;		
		}		
		//使用最大次数;		
		public byte UseMaxNum
		{			
			set;			
			get;		
		}		
		//使用CD;		
		public float CdDelay
		{			
			set;			
			get;		
		}

	}
}