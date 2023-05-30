using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public WeaponController _gun;       //총알을 쏜 총
    public GameObject _impactPrefab;    //피탄 효과
    public int dmg;
    // public float speed;
    private const float existTime = 10.0f;             //총알 생존 시간.
    [SerializeField] Rigidbody rigid;

    IObjectPool<Bullet> pool;

    public void setPool(IObjectPool<Bullet> pool)
    {
        this.pool = pool;
    }

    public void Initialize(WeaponController gun, int dmg, Vector3 bulletPos, Quaternion rotation, Vector3 gunPos, float speed = 110.0f)
    {
        this.dmg = dmg;
        this.transform.position = bulletPos;
        this.transform.rotation = rotation;
        _gun = gun;
        rigid.velocity = (bulletPos - gunPos) * speed;
        Invoke("DestroyBullet", existTime);
    }

    void DestroyBullet()
    {
        pool.Release(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //OnCollision을 사용하기 위해서는 양쪽 모두 IsTrigger가 꺼져 있어야 한다.
        //Trigger 대신 Collision을 사용하는 이유는 총알 접촉시 피탄 지점을 얻기 위해서.

        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "IDamageable")
        {
            BaseController target = collision.transform.GetComponent<BaseController>();
            if (target != null && _gun._master != null)
            {
                _gun.OnAttack(target, dmg, _gun._master);
                //데미지 판정 효과는 어떻게 할 것인가?
            }
            //피탄효과 생성

            ContactPoint contact = collision.GetContact(0);
            GameObject decal = Instantiate(_impactPrefab, contact.point, Quaternion.LookRotation(contact.normal));
            decal.transform.SetParent(collision.transform);
        }

        pool.Release(this);
    }

    private void OnDisable()
    {
        rigid.velocity = Vector3.zero;
    }
}