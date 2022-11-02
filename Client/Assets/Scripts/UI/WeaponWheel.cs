using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheel : MonoBehaviour
{
    [SerializeField] private Vector2 joystickPos;
    [SerializeField] private CharacterMainControllerVR controllerVR;
    [SerializeField] float currentAngle;
    [SerializeField] int selection;

    [SerializeField] private GameObject parent;
    [SerializeField] private List<UnityEngine.UI.Image> wheelImage = new List<UnityEngine.UI.Image>();
    // [SerializeField] private Sprite[] sprits = new Sprite[2];

    //코드 추가
    [SerializeField] private Image[] icons = new Image[4];
    [SerializeField] private TextMeshProUGUI[] ammoDisplays = new TextMeshProUGUI[4];
    [SerializeField] private GameObject joyStickDot;

    void Start()
    {
        controllerVR = Managers.Object.MyPlayer;
        for (int i = 0; i < icons.Length; i++)
        {
            wheelImage.Add(parent.transform.GetChild(i).GetComponent<UnityEngine.UI.Image>());
            icons[i] = parent.transform.GetChild(i).GetChild(0).GetComponent<UnityEngine.UI.Image>();
            ammoDisplays[i] = parent.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        joyStickDot = parent.transform.Find("JoyStickDot").gameObject;
    }

    void Update()
    {
        CalcAngle();
        // Select();
    }

    void CalcAngle()
    {
        if (GameMng.I.VR_Type == Define.VRType.Oculus)
            joystickPos = GameMng.I.input.move.GetAxis(GameMng.I.input.right_hand);
        else
            joystickPos = GameMng.I.input.touchPosition.GetAxis(GameMng.I.input.right_hand);

        /*
         * 40도 일때 x y의 위치.
         * x = cos angle * r
         * y = sin angle * r
         */

        currentAngle = Mathf.Atan2(joystickPos.y, joystickPos.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 315) % 365;

        selection = (int)currentAngle / 90;

        //조이스틱 위치 구하기
        //Vector3 currentPosition = new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0);
        //currentPosition = currentPosition * 280;
        //joyStickDot.transform.localPosition = currentPosition;
        joyStickDot.transform.localPosition = joystickPos * 280;
    }

    public void ChangeIcon(int slot, Item item)
    {
        //icon 변경
        icons[slot].sprite = item.IconImage;
    }

    public void Remove(int slot)
    {
        icons[slot].sprite = null;
    }

    public void Select()
    {
        // if (controllerVR.Com.weaponSlot[selection] == null)
        //     return;

        switch (selection)
        {
            case 0:
                // wheelImage[selection].sprite = sprits[1];
                controllerVR.ChangeWeapon(selection);
                break;
            case 1:
                //  wheelImage[selection].sprite = sprits[1];
                controllerVR.ChangeWeapon(selection);
                break;
            case 2:
                //  wheelImage[selection].sprite = sprits[1];
                controllerVR.ChangeWeapon(selection);
                break;
            case 3:
                //  wheelImage[selection].sprite = sprits[1];
                controllerVR.ChangeWeapon(selection);
                break;
                // Clear(selection);
        }
    }

    // void Clear(int selected)
    // {
    //     for (int i = 0; i < wheelImage.Count; i++)
    //     {
    //         if (i != selected)
    //             wheelImage[i].sprite = sprits[0];
    //     }
    // }
}
