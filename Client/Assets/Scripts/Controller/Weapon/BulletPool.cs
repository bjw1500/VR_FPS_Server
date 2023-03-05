using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] Bullet bullet;

    public IObjectPool<Bullet> pool;
    // Start is called before the first frame update
    void Start()
    {
         pool = new ObjectPool<Bullet>(OnInit, OnObject, OnRelease, OnDestroyBullet, maxSize: 100);
    }

    public Bullet OnInit()
    {
        Bullet obj = Instantiate(bullet);
        obj.transform.parent = this.transform;
        obj.setPool(pool);
        return obj;
    }

    public void OnObject(Bullet obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void OnRelease(Bullet obj)
    {
        obj.gameObject.SetActive(false);
    }

    void OnDestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);
    }
}
