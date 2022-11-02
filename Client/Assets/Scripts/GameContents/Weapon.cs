using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Weapon : Item
{
    int Damage { get; set; }
	WeaponType WeaponType { get; set; } 

    //아이템 스탯
    //아직은 데이터 시트에 추가 안했지만, 적당히 무기가 구현되고 스탯 통일이 되면 추가할 예정.
    //int ammoBulletCount = 13; //탄창 당 총알의 개수
    //int ammoCount = 10; //보유한 탄창의 수
    //float fireRate = 0.3f; //발사 간격
    //int _damage = 25; //데미지
    //float reloadTime = 2.0f; //재장전 시간
    //float _fireDistance = 100f;

	public override void Init()
	{
		base.Init();

		if (ItemData.itemType != ItemType.Weapon)
		{
			return;
		}

		WeaponData weaponData = (WeaponData)ItemData;
		Damage = weaponData.damage;
		WeaponType = weaponData.weaponType;
		
	}

}


