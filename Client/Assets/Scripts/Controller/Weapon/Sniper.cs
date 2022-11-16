using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : WeaponController
{
    public GameObject bullet;
    public Transform bulletPos;
    public Transform gunPos;
    public GameObject gunParticle;

    //무기 스탯

    //public int damage;
    //public int ammoBulletCount;
    //public int ammouCount;
    //public float fireRate;
    //public float reloadTime;
    //public float fireDistance;

    float fireRateTime;
    float reloadRateTime;
    public int bulletcurCount;
    bool isReload;
    Animator anim;

    //Stat


    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireRateTime = _weaponData.fireRate;
        reloadRateTime = 0;
        bulletcurCount = _weaponData.ammoBulletCount;
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
            if (reloadRateTime >= _weaponData.reloadTime) {
                isReload = false;
                reloadRateTime = 0;
            }
        }

    }

    public override void Fire() {
        
        if (fireRateTime >= _weaponData.fireRate && !isReload && bulletcurCount > 0)
        {
            anim.SetBool("Shoot", true);
            gunParticle.SetActive(true);

            Bullet shootingBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
            Rigidbody bulletRigid = shootingBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = (bulletPos.position - gunPos.position) * shootingBullet.speed;
            shootingBullet._gun = this;
            shootingBullet.dmg = _weaponData.damage;
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
        if (!isReload && _weaponData.ammoCount > 0)
        {
            isReload = true;
            anim.SetTrigger("DoReload");
            _weaponData.ammoCount--;
            bulletcurCount = _weaponData.ammoBulletCount;
        }
    }
}
