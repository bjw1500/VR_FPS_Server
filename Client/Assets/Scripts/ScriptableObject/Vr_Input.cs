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

    [Header("[ 이동 조이스틱 ]")]
    public SteamVR_Action_Vector2 move = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Move");
    public SteamVR_Action_Vector2 touchPosition = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchPosition");

    [Header("[ 트리거 ]")]
    public SteamVR_Action_Boolean fireTrigger = SteamVR_Actions.default_InteractUI;
    public SteamVR_Action_Boolean jumpTrigger = SteamVR_Actions.default_GrabGrip;

    // LaserPoint
    public SteamVR_Action_Boolean interactWithUI = SteamVR_Input.GetBooleanAction("InteractUI");

    [Header("[ 스넵턴 조이스틱 ]")]
    public SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");


    [Header("[ 무기 UI 조이스틱 ]")]
    public SteamVR_Action_Boolean weaponWheel = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("WeaponWheel");
    public SteamVR_Action_Boolean wheelTouch = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("WheelTouch");



    public string paramMoveX { get { return "Move X"; } }
    public string paramMoveZ { get { return "Move Z"; } }
    public string paramDistY { get { return "Move Y"; } }
    public string paramGrounded { get { return "Grounded"; } }
    public string paramJump { get { return "Jump"; } }
}