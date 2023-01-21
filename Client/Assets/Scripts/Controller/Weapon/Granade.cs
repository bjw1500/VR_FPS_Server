using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : WeaponController
{
    Animator anim;
    Custom_Interactable interactable;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("PullPin", false);
        interactable = GetComponent<Custom_Interactable>();
    }

    private void OnEnable()
    {
        anim.SetBool("PullPin", false);
    }

    void Update()
    {
        //if (Input.GetButton("Granade") && isPullPin)
        //{
        //    anim.SetTrigger("Explosion");
        //    isPullPin = false;
        //    anim.SetBool("PullPin", false);
        //}

        //if (Input.GetButton("Granade") && !anim.GetBool("PullPin")) {
        //    anim.SetBool("PullPin", true);
        //    Invoke("SetPullPin", 1);
        //}
    }

    public override void Fire()
    {
        base.Fire();
        if (!anim.GetBool("PullPin"))
        {
            //수류탄 핀을 뽑는다.
            anim.SetBool("PullPin", true);
            Invoke("SetPullPin", 3f);

            //Vr 안의 물건 던지기 기능 활성화하기.
            //먼저 플레이어와 수류탄 간의 자식 관계를 끊어준다.
            //던질 때 Master의 컨트롤러랑 Pick Up 연결해주기

            PickUp pickpUp = _master.Com.RightController.GetComponent<PickUp>();
            pickpUp.Pickup(interactable);
            transform.parent = null;
            
        }

    }

    void SetPullPin() {
            Debug.Log($"{_master.Info.Name}의 수류탄이 터졌습니다.");
            anim.SetTrigger("Explosion");
    }
}
