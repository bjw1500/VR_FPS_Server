using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using static Define;

public class ActionController : MonoBehaviour
{

    //카메라에 부착되는 컨트롤러.
    //현재까지는 아이템 확인하는 기능이 추가됨.
    [SerializeField]
    private float range; //사정거리
    [SerializeField]
    private Vector3 height;

    private bool pickupActivated = false;
    public CharacterMainControllerVR MyPlayer;


    private RaycastHit hitInfo; //충돌체 정보
    [SerializeField]
    private LayerMask layerMask; //아이템 레이어에만 반응하도록 설정


    [SerializeField]
    private UI_HUD _HUD;
    [SerializeField]
    private Text _actionText;     //UI에 띄울 텍스트.

    [SerializeField]
    private WeaponWheel _weaponWheel;

bool test = false;
    private void Start()
    {
        MyPlayer = Managers.Object.MyPlayer;

        range = 3;
        height = new Vector3(0, -0.2f, 0);
    }

    void Update()
    {
        if(GameMng.I.input.weaponWheel.GetChanged(GameMng.I.input.right_hand))
        {
            _weaponWheel.gameObject.SetActive(true);
            test = true;
        }
        else if(test && !GameMng.I.input.wheelTouch.GetLastState(GameMng.I.input.right_hand))
        {
            _weaponWheel.Select();
            _weaponWheel.gameObject.SetActive(false);test = false;
        }

        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (GameMng.I.input.fireTrigger.GetStateDown(GameMng.I.input.left_hand) == true)
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            Item item = hitInfo.transform.GetComponent<Item>();

            //장비를 교환한다.
            if (item != null && item.Type == ItemType.Weapon)
            {
                MyPlayer.GetWeapon(item);
                _weaponWheel.ChangeIcon(MyPlayer._currentWeaponSlot, item);
                InfoDisappear();
            }
            else if (item != null && item.Type == ItemType.Armor)
            {
                //TODO 방어구.

            }
            else if (item != null && item.Type == ItemType.Consumable)
            {
                //소모품을 주웠을 때 인벤토리에 넣어준다.
                //TODO 인벤토리 만들어주기?

            }
        }
    }

    private void CheckItem()
    {
        //카메라가 바라본 방향에 아이템이 있다면 true.
        if (Physics.Raycast(transform.position + height, transform.forward,
            out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        _actionText.gameObject.SetActive(false);
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        _actionText.gameObject.SetActive(true);
        _actionText.text = hitInfo.transform.GetComponent<Item>().Name + " 왼쪽 컨트롤러 트리거로 획득";
    }
}
