using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WeaponController : MonoBehaviour
{
    /*
     * 상속구조
     * WeaponController
     *  Gun
     *무기 구현시 Fire랑 OnAttack 부분은 반드시 사용해줄것.
     * Fire은 무기를 사용하는 함수, 
     * OnAttack은 데미지 판정을 주는 함수.
     */


    public BaseController _master; //총을 들고 있는 무기의 주인
    public WeaponData _weaponData;

    public int curBulletCount = 0; //현재 보유한 총알의 수


    public GunState State
    {
        get { return _currentState; }
        set { _currentState = value; }

    }
    public GunState _currentState = GunState.Empty;

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Init()
    {

    }

    public virtual void Fire()
    {
        //호출 되었을 때 무기를 사용하는 함수.
        //무기를 쏠때마다 UI BulletCount에 값을 전달한다.

    }

    public virtual void OnAttack(BaseController target, int damage, BaseController master)
    {
        //데미지를 주는 함수

        if (target == null || master == null || target == master)
            return;

        //무기의 주인이 서버에서 움직이는 캐릭터라면 데미지 판정을 줄 필요가 없다.
        CharacterRemoteControllerVR cr = master as CharacterRemoteControllerVR;
        if (cr != null)
            return;

        target.OnDamage(damage, _master.Info);
    }
}
