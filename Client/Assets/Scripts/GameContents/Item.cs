using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Item : MonoBehaviour
{

    //TODO
    //서버에서 관리할 object ID. 데이터 시트에서 분리할 itemName.
    //리모트 캐릭터가 아이템 사용 혹은 습득시 ObjectId로 파악하고,
    //아이템 정보는 데이터 시트를 활용하여 itemName으로 초기화한다.

    //Info에는 서버에서 관리해줘야할 ID 정보 등이 들어 있고,
    //ItemData에는 게임 안의 주된 스탯들이 담겨 있다.

    public ItemInfo Info { get; set; }
    public string itemName;

    public Sprite IconImage
    {
        get;
        set;
    }

   
    public ItemData ItemData
    {
		get;set;
    }

    public string Name { get { return ItemData.name; } }
    public ItemType Type { get { return ItemData.itemType; } }

	void Start()
    {
        Init();
    }


    public virtual void Init()
    {
        //Layer를 Item으로 해줘야 아이템 집기가 가능.

        transform.gameObject.tag = "Item";
        transform.gameObject.layer = LayerMask.NameToLayer("Item");


        if (GameMng.I.SingleGame == true)
        {
            ItemData itemData = null;
            if (Managers.Data.ItemDict.TryGetValue(itemName, out itemData) == false)
            {
                Debug.Log($"{itemName} 데이터 시트 오류. Prefab에서 이름을 정확히 입력해주세요.");
                return;
            }

            ItemInfo info = new ItemInfo();
            ItemData = itemData;
            IconImage = Managers.Resource.Load<Sprite>(ItemData.imagePath);
        }


        //TODO 서버 작업용 코드 들어갈 예정.
        //Map 제작툴을 만들어서 아이템 관리까지 해줘야 할까?
        //아이템 관리 하기 위해 ObjectManager에 등록해주기.
        //Managers.Object.AddItem(this);

        

    }
}
