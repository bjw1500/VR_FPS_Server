using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunX : MonoBehaviour
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
        Fire();
        Reload();
    }

    void TimeCount() {
 
        fireRateTime += Time.deltaTime;
        if (isReload) {
            reloadRateTime += Time.deltaTime;
            if (reloadRateTime >= reloadRate) {
                isReload = false;
                reloadRateTime = 0;
            }
        }

    }

    void Fire() {
        
        if (Input.GetButton("Fire") && fireRateTime >= fireRate && !isReload && bulletcurCount > 0)
        {
            anim.SetBool("Shoot", true);
            gunParticle.SetActive(true);

            Bullet shootingBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
            Rigidbody bulletRigid = shootingBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = (bulletPos.position - gunPos.position) * shootingBullet.speed;
            fireRateTime = 0;
            bulletcurCount--;
        }

        if (Input.GetButtonUp("Fire") || bulletcurCount <= 0)
        {
            gunParticle.SetActive(false);
            anim.SetBool("Shoot", false);
        }

    }

    void Reload() {
        if (Input.GetButtonDown("Reload") && !isReload && bulletCharge > 0)
        {
            isReload = true;
            anim.SetTrigger("DoReload");
            bulletCharge--;
            bulletcurCount = bulletCount;
        }
    }
}
