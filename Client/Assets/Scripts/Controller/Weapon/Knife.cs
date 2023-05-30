using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : WeaponController
{
    BoxCollider boxC;
    public bool isSwing;
    public Transform bloodPos;
    public GameObject blood;

    private void Awake()
    {
        boxC = GetComponent<BoxCollider>();
    }

    private void Update()
    {

    }

    public override void OnAttack(BaseController target, int damage, BaseController master)
    {
        base.OnAttack(target, damage, master);
    }

    public override void Fire()
    {
        base.Fire();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IDamageable") {

            BaseController target = other.transform.GetComponent<BaseController>();
            if (target != null && _master != null && target != _master)
            {
                OnAttack(target, _weaponData.damage, _master);
                GameObject shotBlood = Instantiate(blood, bloodPos.position, bloodPos.rotation);
                Destroy(shotBlood, 2f);
                //데미지 판정 효과는 어떻게 할 것인가?
            }
        }
    }

}
