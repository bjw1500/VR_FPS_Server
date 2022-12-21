using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : WeaponController
{
    public int damage;
    Animator anim;
    bool isPullPin;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("PullPin", false);
        isPullPin = false;
    }

    private void OnEnable()
    {
        anim.SetBool("PullPin", false);
        isPullPin = false;
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

            //Vr 안의 물건 던지기 기능 활성화하기.


            Invoke("SetPullPin", 1);
            //이제 1초후에 폭발. 그러면 던져야지. 
            //그런데 어떻게 던져야 할까?
        }

    }

    void SetPullPin() {
        isPullPin = true;
        if (isPullPin)
        {
            anim.SetTrigger("Explosion");
            isPullPin = false;
            anim.SetBool("PullPin", false);
        }
    }
}
