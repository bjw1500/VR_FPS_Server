using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public WeaponController _gun;       //총알을 쏜 총
    public GameObject _impactPrefab;    //피탄 효과
    public int dmg;
    public float speed;
    public float existTime;             //총알 생존 시간.


   

    private void Awake()
    {
        existTime = 10.0f;
        Destroy(gameObject, existTime);
    }


    private void OnCollisionEnter(Collision collision)
    {

        //OnCollision을 사용하기 위해서는 양쪽 모두 IsTrigger가 꺼져 있어야 한다.
        //Trigger 대신 Collision을 사용하는 이유는 총알 접촉시 피탄 지점을 얻기 위해서.

        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "IDamageable")
        {


            BaseController target = collision.transform.GetComponent<BaseController>();
            ObjectInfo testinfo = target.Info;
            if (target != null && _gun._master != null)
            {
                ObjectInfo testinfo2 = target.Info;
                _gun.OnAttack(target, dmg, _gun._master);
                //데미지 판정 효과는 어떻게 할 것인가?
            }


            //피탄효과 생성

            ContactPoint contact = collision.GetContact(0);
            GameObject decal = Instantiate(_impactPrefab, contact.point, Quaternion.LookRotation(contact.normal));
            decal.transform.SetParent(collision.transform);

            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
