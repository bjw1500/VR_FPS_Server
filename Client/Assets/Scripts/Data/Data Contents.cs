using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

[Serializable]
public class StatData : ILoader<string, StatInfo>
{
    public List<StatInfo> stats = new List<StatInfo>();

    public Dictionary<string, StatInfo> MakeDict()
    {
        Dictionary<string, StatInfo> dict = new Dictionary<string, StatInfo>();
        foreach (StatInfo stat in stats)
            dict.Add(stat.Name, stat);
        return dict;
    }
}


//TODO 나중에 플레이어나 몬스터 스킬 같은게 생길지 모르니.. 혹시 모를 예비용.

[Serializable]
public class Skill
{
    public int id;
    public string name;
    public float cooldown;
    public int damage;
}

public class SkillData : ILoader<int, Skill>
{
    public List<Skill> Skills = new List<Skill>();

    public Dictionary<int, Skill> MakeDict()
    {
        Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
        foreach (Skill skill in Skills)
            dict.Add(skill.id, skill);
        return dict;
    }
}


//TODO 아이템Info 만들기

[Serializable]
public class ItemData
{
    public int id;          //데이터 시트 아이디.
    public string name;    //아이템 이름
    public string prefabPath;   //리소스 폴더 안의 프리팹 주소.
    public string imagePath;   //리소스 폴더 안의 이미지 주소
    public ItemType itemType;
}


[Serializable]
public class WeaponData : ItemData
{
    public int damage;
    public int ammoBulletCount = 13; //탄창 당 총알의 개수
    public int ammoCount = 10; //보유한 탄창의 수
    public float fireRate = 0.3f; //발사 간격
    public float reloadTime = 2.0f; //재장전 시간
    public float fireDistance = 100f;

    public WeaponType weaponType;   //TODO 권총. 기관총. 저격총 분류하기.
    public bool IsThrowable;
}

[Serializable]
public class ArmorData : ItemData
{
    public int defence;
    public ArmorType armorType; 

}

[Serializable]
public class ConsumableData : ItemData
{
    //TODO 소모품 구현은 소모품 만들고서 좀 더 구체화 해주자.
    public ConsumableType consumableType;
    public int maxCount;
}

[Serializable]
public class ItemLoader : ILoader<string, ItemData>
{
    public List<WeaponData> weapons = new List<WeaponData>();
    public List<ArmorData> armors = new List<ArmorData>();
    public List<ConsumableData> consumables = new List<ConsumableData>();

    public Dictionary<string, ItemData> MakeDict()
    {
        Dictionary<string, ItemData> dict = new Dictionary<string, ItemData>();
        foreach (ItemData item in weapons)
        {
            item.itemType = ItemType.Weapon;
            dict.Add(item.name, item);
        }
        foreach (ItemData item in armors)
        {
            item.itemType = ItemType.Armor;
            dict.Add(item.name, item);
        }
        foreach (ItemData item in consumables)
        {
            item.itemType = ItemType.Consumable;
            dict.Add(item.name, item);
        }
        return dict;
    }
}

