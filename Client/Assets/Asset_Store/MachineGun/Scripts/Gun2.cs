using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun2 : WeaponController
{
    public GameObject bullet;
    public Transform bulletPos;
    public Transform gunPos;
    public GameObject gunParticle;
    public float fireRate;
    public float reloadRate;
    public int bulletCount;
    public int bulletCharge;

    float fireRateTime;
    float reloadRateTime;
    int bulletcurCount;
    bool isReload;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireRateTime = fireRate;
        reloadRateTime = 0;
        bulletcurCount = bulletCount;
    }

    void Update()
    {
        TimeCount();
    }

    void TimeCount() {
 
        fireRateTime += Time.deltaTime;
        if (isReload) {
            reloadRateTime += Time.deltaTime;
            Debug.Log("Time: " + reloadRateTime);
            if (reloadRateTime >= reloadRate) {
                isReload = false;
                reloadRateTime = 0;
            }
        }

    }

    public override void Fire() {
        
        if (fireRateTime >= fireRate && !isReload && bulletcurCount > 0)
        {
            anim.SetBool("Shoot", true);
            gunParticle.SetActive(true);

            Bullet shootingBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
            Rigidbody bulletRigid = shootingBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = (bulletPos.position - gunPos.position) * shootingBullet.speed;
            fireRateTime = 0;
            bulletcurCount--;
        }

        if (bulletcurCount <= 0)
        {
            gunParticle.SetActive(false);
            anim.SetBool("Shoot", false);
            Reload();
        }

    }

    void Reload() {
        if (!isReload && bulletCharge > 0)
        {
            isReload = true;
            anim.SetTrigger("DoReload");
            bulletCharge--;
            bulletcurCount = bulletCount;
        }
    }
}
