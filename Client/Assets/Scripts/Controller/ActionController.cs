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
    [SerializeField] private float range; //사정거리
    [SerializeField] private Vector3 height;

    private bool pickupActivated = false;
    public CharacterMainControllerVR MyPlayer;

    private RaycastHit hitInfo; //충돌체 정보
    [SerializeField] private LayerMask layerMask; //아이템 레이어에만 반응하도록 설정

    [SerializeField] private UI_HUD _HUD;
    [SerializeField] private Text _actionText;     //UI에 띄울 텍스트.

    [SerializeField] GameObject rightLaserPointer;
    [SerializeField] GameObject leftLaserPointer;

    [SerializeField] Transform rightPose;
    [SerializeField] Transform leftPose;

    [SerializeField] private GameObject _menu;
    [SerializeField] private WeaponWheel _weaponWheel;

    [SerializeField] bool activeUI = false;
    bool wheelUI = false;
    private void Start()
    {
        MyPlayer = Managers.Object.MyPlayer;

        range = 3;
        height = new Vector3(0, -0.2f, 0);

        rightLaserPointer.SetActive(false);
        leftLaserPointer.SetActive(false);
    }

    void Update()
    {
        CheckItem();
        InputController();
    }

    private void InputController()
    {
        // if (GameMng.I.input.getStatePickUpTrigger)        // 총 줍기
        // master
        if (GameMng.I.input.getStateFireTrigger)        // 총 줍기
        {
            CheckItem();
            CanPickUp();
        }

        if (!activeUI && !wheelUI)
        {
            SnapTurn();
        }

        if (GameMng.I.input.getStateWeaponWheel)    // 무기 선택창
        {
            _weaponWheel.gameObject.SetActive(true);
            wheelUI = true;
        }
        else if (wheelUI && !GameMng.I.input.getStateWheelTouch)      // 무기 선택창 끄기
        {
            _weaponWheel.Select();
            _weaponWheel.gameObject.SetActive(false);
            wheelUI = false;
        }

        if (!activeUI && GameMng.I.input.getMenuBinding)    // 매뉴 창
        {
            if (GameMng.I.input.getStateRightMenuBtn)
            {
                leftLaserPointer.SetActive(true);
                _menu.transform.parent = rightPose;
                SetMenuParent();
            }
            else if (GameMng.I.input.getStateLeftMenuBtn)
            {
                rightLaserPointer.SetActive(true);
                _menu.transform.parent = leftPose;
                SetMenuParent();
            }
        }
        else if (activeUI && GameMng.I.input.getStateAnyMenuBtn)      // 매뉴 끄기
        {
            _menu.transform.parent = this.transform;
            _menu.gameObject.SetActive(false);
            activeUI = false;
            rightLaserPointer.SetActive(false);
            leftLaserPointer.SetActive(false);
        }
    }

    void SetMenuParent()
    {
        _menu.transform.localPosition = new Vector3(0f, 0f, 0.15f);
        _menu.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        _menu.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);

        _menu.gameObject.SetActive(true);
        activeUI = true;
    }

    // @brief 오른손 컨트롤러 카메라 회전
    private void SnapTurn()
    {
        if (GameMng.I.snapturn.getCanRotate && GameMng.I.input.snapLeftAction != null && GameMng.I.input.snapRightAction != null
        && GameMng.I.input.snapLeftAction.activeBinding && GameMng.I.input.snapRightAction.activeBinding)
        {
            //only allow snap turning after a quarter second after the last teleport
            if (Time.time < (CustomSnapTurn.teleportLastActiveTime + GameMng.I.snapturn.canTurnEverySeconds))
                return;

            bool rightHandTurnLeft = GameMng.I.input.snapLeftAction.GetStateDown(SteamVR_Input_Sources.RightHand);

            bool rightHandTurnRight = GameMng.I.input.snapRightAction.GetStateDown(SteamVR_Input_Sources.RightHand);

            if (rightHandTurnLeft)
            {
                GameMng.I.snapturn.RotatePlayer(-GameMng.I.snapturn.snapAngle);
            }
            else if (rightHandTurnRight)
            {
                GameMng.I.snapturn.RotatePlayer(GameMng.I.snapturn.snapAngle);
            }
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
