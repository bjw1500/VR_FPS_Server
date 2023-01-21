using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Weapon : Item
{
	//아이템 스탯
	public WeaponData WeaponData { get; set; }

	public override void Init()
	{
		base.Init();

		if (ItemData.itemType != ItemType.Weapon)
		{
			return;
		}

		//WeaponController랑 연동해주기.
		WeaponData = (WeaponData)ItemData;

		WeaponController wc = transform.gameObject.GetComponent<WeaponController>();
		wc._weaponData = WeaponData;

	}

}


