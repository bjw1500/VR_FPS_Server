using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
    bool isReload;
    Animator anim;

    // 총 소리 추가
    public AudioClip fireSfx;
    //AudioSource 컴포넌트 저장
    private AudioSource source = null;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }


    //Stat


    private void Awake()
    {
        anim = GetComponent<Animator>();
        fireRateTime = _weaponData.fireRate;
        reloadRateTime = 0;
        curBulletCount = 0;
        _currentState = GunState.Empty;
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
                _currentState = GunState.Ready;
                isReload = false;
                reloadRateTime = 0;
            }
        }

    }

    public override void Fire() {
        
        if (fireRateTime >= _weaponData.fireRate && !isReload && curBulletCount > 0)
        {
            anim.SetBool("Shoot", true);
            gunParticle.SetActive(true);

            //총 소리 발생
            source.PlayOneShot(fireSfx, 0.9f);

            GameMng.I.bulletPool.pool.Get().Initialize(this, _weaponData.damage, bulletPos.position, bulletPos.rotation, gunPos.position);

            // Bullet shootingBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
            // Rigidbody bulletRigid = shootingBullet.GetComponent<Rigidbody>();
            // bulletRigid.velocity = (bulletPos.position - gunPos.position) * shootingBullet.speed;
            // shootingBullet._gun = this;
            // shootingBullet.dmg = _weaponData.damage;
            fireRateTime = 0;
            curBulletCount--;
        }

        if (curBulletCount <= 0)
        {
            _currentState = GunState.Empty;
            gunParticle.SetActive(false);
            anim.SetBool("Shoot", false);
            Reload();
        }

    }

    void Reload() {
        if (!isReload && _weaponData.ammoCount > 0)
        {
            _currentState = GunState.Reloading;
            isReload = true;
            anim.SetTrigger("DoReload");
            _weaponData.ammoCount--;
            curBulletCount = _weaponData.ammoBulletCount;
        }
    }
}
