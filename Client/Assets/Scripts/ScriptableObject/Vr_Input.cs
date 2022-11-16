using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System;

[CreateAssetMenu]
public class Vr_Input : ScriptableObject
{
    // 겹치는 부분 없애기
    [Header("[ 컨트롤러 ]")]
    public SteamVR_Input_Sources right_hand = SteamVR_Input_Sources.RightHand;
    public SteamVR_Input_Sources left_hand = SteamVR_Input_Sources.LeftHand;

    [Header("[ 조이스틱 ]")]
    [SerializeField] SteamVR_Action_Vector2 move = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Move");
    public Vector2 getMoveAxis { get { return move.GetAxis(left_hand); } }
    public Vector2 getWheelAxis { get { return move.GetAxis(right_hand); } }

    [Header("[ 트리거 ]")]
    // LaserPoint
    [SerializeField] public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.GetBooleanAction("InteractUI");
    public bool getStateFireTrigger { get { return interactWithUI.GetStateDown(right_hand); }}
    public bool getStatePickUpTrigger { get { return interactWithUI.GetStateDown(left_hand); } }

    [Header("[ 스넵턴 조이스틱 ]")]
    public SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    [Header("[ 무기 UI 조이스틱 ]")]
    [SerializeField] SteamVR_Action_Boolean weaponWheel = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("WeaponWheel");
    public bool getStateWeaponWheel { get { return weaponWheel.GetChanged(right_hand); } }

    [SerializeField] SteamVR_Action_Boolean wheelTouch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("WheelTouch");
    public bool getStateWheelTouch { get { return wheelTouch.GetLastState(right_hand); } }

    [Header("[ 버튼 ]")]
    [SerializeField] SteamVR_Action_Boolean jumpBtn = SteamVR_Input.GetBooleanAction("Jump"); 
    public bool getStateJumpBtn { get { return jumpBtn.GetStateDown(right_hand); }}

    [SerializeField] SteamVR_Action_Boolean menuBtn = SteamVR_Input.GetBooleanAction("Menu");
    public bool getMenuBinding { get { return menuBtn.activeBinding; }}
    public bool getStateRightMenuBtn { get { return menuBtn.GetStateDown(right_hand); }}
    public bool getStateLeftMenuBtn { get { return menuBtn.GetStateDown(left_hand); }}
    public bool getStateAnyMenuBtn { get { return menuBtn.GetStateDown(SteamVR_Input_Sources.Any); }}

    public string paramMoveX { get { return "Move X"; } }
    public string paramMoveZ { get { return "Move Z"; } }
    public string paramDistY { get { return "Move Y"; } }
    public string paramGrounded { get { return "Grounded"; } }
    public string paramJump { get { return "Jump"; } }
}