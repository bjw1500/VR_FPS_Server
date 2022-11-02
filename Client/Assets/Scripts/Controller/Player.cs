using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public SteamVR_Input_Sources right_hand = SteamVR_Input_Sources.RightHand;
    public SteamVR_Input_Sources left_hand = SteamVR_Input_Sources.LeftHand;

    [SerializeField] private SteamVR_Action_Vector2 move = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Move");
    [SerializeField] private SteamVR_Action_Vector2 touchPosition = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchPosition");

    public SteamVR_Action_Boolean trigger = SteamVR_Actions.default_InteractUI;

    [SerializeField] Vector3 vec = Vector3.zero;
    [SerializeField] Camera cam = null;
    [SerializeField] VrState State = VrState.Oculus;
    [SerializeField] public Gun myGun;

    enum VrState
    {
        Oculus,
        Vive,
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController();
    }

    void PlayerController()
    {
        //if (Managers.GameManager.I.isVR)
        //{

        //    if (trigger.GetStateDown(right_hand) == true)
        //    {
        //        myGun.Fire();
        //    }


        //    오큘러스 or Vive
        //    if (State == VrState.Oculus)
        //    {
        //        Debug.Log(touchPosition.GetAxis(left_hand));
        //        PlayerMove(move.GetAxis(left_hand).x, move.GetAxis(left_hand).y);

        //    }
        //    else if (State == VrState.Vive)
        //    {
        //        Debug.Log(touchPosition.GetAxis(left_hand));
        //        PlayerMove(touchPosition.GetAxis(left_hand).x, touchPosition.GetAxis(left_hand).y);
        //    }

        //}
        //else
        //{
        //    PlayerMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //}
    }

    void PlayerMove(float moveX, float moveZ)
    {
        vec = Vector3.right * moveX + Vector3.forward * moveZ;

        vec = cam.transform.TransformDirection(vec); // 카메라가 보고 있는 방향으로 앞 방향 변경
        vec.Normalize(); // 균일한 이동 위해서 정규화

        transform.position += vec * 5.0f * Time.deltaTime;
    }
}