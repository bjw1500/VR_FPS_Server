using Google.Protobuf.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CharacterRemoteControllerVR : BaseController
{

    [Serializable]
    public class CharacterState
    {
        public bool isCurrentFp;
        public bool isMoving;
        public bool isRunning;
        public bool isGrounded;
        public bool isCursorActive;
    }

    public CharacterState State => _state;

    [Space, SerializeField] private CharacterState _state = new CharacterState();


    [SerializeField]
    private float _distFromGround;
    // Animation Params
    private float _moveX;
    private float _moveZ;

    [SerializeField]
    private Vector3 _moveDir;


    /// <summary> 월드 이동 벡터 </summary>
    [SerializeField]
    private Vector3 _worldMoveDir = Vector3.zero;
    public Vector3 WorldMoveDir
    {
        get { return _worldMoveDir; }
        set
        {
            _worldMoveDir = value;
        }
    }

    //무기 관련 필드
    public const int _weaponSlotSize = 4;
    [SerializeField]
    public int _currentWeaponSlot = 0;

    public override void Init()
    {
        base.Init();
        InitComponent();
    }

    void Update()
    {
        UpdatePosition();
        RemoteMove();
        UpdateAnimationParams();
    }

    public void InitComponent()
    {
        Com.myGun = GetComponentInChildren<WeaponController>();
        Com.cam = GetComponentInChildren<Camera>();
        Com.anim = GetComponentInChildren<Animator>();

        Com.CameraRig = Util.FindChild(transform.gameObject, "CameraRig");
        Com.RightController = Util.FindChild(Com.CameraRig, "RightController");
        Com.LeftController = Util.FindChild(Com.CameraRig, "LeftController");

        Com.weaponSlot = new GameObject[_weaponSlotSize];

        TryGetComponent(out Com.movement3D);
        if (Com.movement3D == null)
            Com.movement3D = gameObject.AddComponent<PhysicsBasedMovement>();
    }

    public void UpdatePosition()
    {
        #region 위치 업데이트

        _moveDir = new Vector3(Info.MovementInfo.MoveDir.PosX,
        Info.MovementInfo.MoveDir.PosY, Info.MovementInfo.MoveDir.PosZ);

        //이게 최선인가? 좀 더 개선해볼것.
        gameObject.transform.position = new Vector3(Info.MovementInfo.PlayerPosInfo.PosX, Info.MovementInfo.PlayerPosInfo.PosY, Info.MovementInfo.PlayerPosInfo.PosZ);
        gameObject.transform.eulerAngles = new Vector3(Info.MovementInfo.PlayerPosInfo.RotateX, Info.MovementInfo.PlayerPosInfo.RotateY, Info.MovementInfo.PlayerPosInfo.RotateZ);

        Com.cam.transform.position = new Vector3(Info.MovementInfo.CameraPosInfo.PosX, Info.MovementInfo.CameraPosInfo.PosY, Info.MovementInfo.CameraPosInfo.PosZ);
        Com.cam.transform.eulerAngles = new Vector3(Info.MovementInfo.CameraPosInfo.RotateX, Info.MovementInfo.CameraPosInfo.RotateY, Info.MovementInfo.CameraPosInfo.RotateZ);

        Com.RightController.transform.position = new Vector3(Info.MovementInfo.RightHandPosInfo.PosX, Info.MovementInfo.RightHandPosInfo.PosY, Info.MovementInfo.RightHandPosInfo.PosZ);
        Com.RightController.transform.eulerAngles = new Vector3(Info.MovementInfo.RightHandPosInfo.RotateX, Info.MovementInfo.RightHandPosInfo.RotateY, Info.MovementInfo.RightHandPosInfo.RotateZ);

        Com.LeftController.transform.position = new Vector3(Info.MovementInfo.LeftHandPosInfo.PosX, Info.MovementInfo.LeftHandPosInfo.PosY, Info.MovementInfo.LeftHandPosInfo.PosZ);
        Com.LeftController.transform.eulerAngles = new Vector3(Info.MovementInfo.LeftHandPosInfo.RotateX, Info.MovementInfo.LeftHandPosInfo.RotateY, Info.MovementInfo.LeftHandPosInfo.RotateZ);

        #endregion

        State.isMoving = Info.MovementInfo.Moving;
        State.isRunning = Info.MovementInfo.Running;
        State.isGrounded = Info.MovementInfo.Ground;
    }

    private void UpdateAnimationParams()
    {
        float x, z;
        x = _moveDir.x;
        z = _moveDir.z;

        if (State.isRunning)
        {
            x *= 2f;
            z *= 2f;
        }

        // 보간
        const float LerpSpeed = 0.05f;
        _moveX = Mathf.Lerp(_moveX, x, LerpSpeed);
        _moveZ = Mathf.Lerp(_moveZ, z, LerpSpeed);

        Com.anim.SetFloat(GameMng.I.input.paramMoveX, _moveX);
        Com.anim.SetFloat(GameMng.I.input.paramMoveZ, _moveZ);
        Com.anim.SetFloat(GameMng.I.input.paramDistY, _distFromGround);
        Com.anim.SetBool(GameMng.I.input.paramGrounded, State.isGrounded);
    }

    /***********************************************************************
    *                               Movement Methods
    ***********************************************************************/

    private void RemoteMove()
    {
        WorldMoveDir = Com.cam.transform.TransformDirection(_moveDir);
        Com.movement3D.SetMovement(WorldMoveDir, State.isRunning);
    }

    public void UseSkill(int skillId)
    {
        switch (skillId)
        {
            //나중에 데이터 시트로 정리하기.
            case 1:
                Com.myGun.Fire();
                break;

            case 2:
                Jump();
                break;

            default:
                break;
        }
    }

    private void CheckGroundDistance()
    {
        _distFromGround = Com.movement3D.GetDistanceFromGround();
        State.isGrounded = Com.movement3D.IsGrounded();
    }


    private void Jump()
    {
        bool jumpSucceeded = Com.movement3D.SetJump();

        if (jumpSucceeded)
        {
            // 애니메이션 점프 트리거
            Com.anim.SetTrigger(GameMng.I.input.paramJump);

            Debug.Log("JUMP");
        }
    }

    public void KnockBack(in Vector3 force, float time)
    {
        Com.movement3D.KnockBack(force, time);
    }

    public void GetWeapon(Item item)
    {
        Weapon weapon = item as Weapon;
        Debug.Log($"{Info.Name}이 {item.Name}를 획득 했습니다.");


        Rigidbody body = item.transform.gameObject.GetComponent<Rigidbody>();
        Collider col = item.transform.GetComponent<Collider>();

        //수류탄을 던지기 위해서는 RigidBody가 활성화 되어 있어야 한다.

        body.useGravity = false;
        col.enabled = false;

        //슬롯을 채워주는 동시에 현재 플레이어의 무기를 바꿔준다.
        for (int i = 0; i < _weaponSlotSize; i++)
        {
            if (Com.weaponSlot[i] == null)
            {
                //무기 슬롯이 비어있다면, 채워주고 
                Com.weaponSlot[i] = item.transform.gameObject;

                //현재 사용중인 무기를 비활성화 해준다음, 주운 무기로 바꿔준다.
                if (Com.myGun != null)
                {
                    Com.myGun.gameObject.SetActive(false);
                }

                _currentWeaponSlot = i;
                item.transform.SetParent(Com.RightController.transform, false);
                item.Info.Master = Info;

                Com.myGun = item.transform.GetComponent<WeaponController>();
                Com.myGun._master = this;
                break;

            }
            else if (i == _weaponSlotSize - 1)
            {
                //무기 슬롯이 비어 있지 않다면,
                //현재 장착 중인 무기를 버리고 교환한다.
                Com.weaponSlot[_currentWeaponSlot] = null;
                Managers.Resource.Destroy(Com.myGun.gameObject);

                _currentWeaponSlot = i;
                item.transform.SetParent(Com.RightController.transform, false);
                item.Info.Master = Info;

                Com.weaponSlot[_currentWeaponSlot] = item.transform.gameObject;
                Com.myGun = item.transform.GetComponent<WeaponController>();
                Com.myGun._master = this;
                break;
            }
        }

        //무기를 주웠을 때 위치 초기화 해주기.
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public void ChangeWeapon(int slot)
    {
        //현재 무기 숨기기
        WeaponController currentWeapon = Com.weaponSlot[_currentWeaponSlot].GetComponent<WeaponController>();
        currentWeapon.gameObject.SetActive(false);

        //무기 변경
        _currentWeaponSlot = slot;
        WeaponController wc = Com.weaponSlot[slot].GetComponent<WeaponController>();
        wc.gameObject.SetActive(true);
        wc._master = this;
        Com.myGun = wc;

        //서버 부분
        if (GameMng.I.SingleGame == true)
            return;
    }

    public override void OnDamage(int damage, ObjectInfo attacker)
    {
        base.OnDamage(damage, attacker);
    }

    public override void OnDead(ObjectInfo attacker)
    {
        base.OnDead(attacker);

        Debug.Log($"{attacker.Name}에 의해 {Info.Name}이 파괴됩니다.");
        Managers.Object.Remove(Info.ObjectId);
    }
}
