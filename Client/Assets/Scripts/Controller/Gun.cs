using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : WeaponController

    //오디오 및 UI는 기존의 있던 것을 유지해서 변동 없음. 나중에 수정 예정
    //총 발사에 대한 기본적인 알고리즘은 기존과 동일
    //전체적인 변수의 이름 변경 및 추가, 삭제 사항 있음. 변수 변경 외의 메소드 별 수정 사항은 메소드 이름 위 주석에서 표시
{
 
    public GameObject bullet; //총알 오브젝트
    public Transform bulletPos; //총알이 나갈 위치
    public Transform gunPos; //총의 위치
    public GameObject gunParticle; //총 발사 및 탄피 효과

    //나중에 지우거나 수정
    //public Transform _fireStarting;          //총구
    //public ParticleSystem _shellEjectEffect; //탄피 효과
    //public ParticleSystem _muzzleFlashEffect; //총구 화염

    //Audio
    public AudioSource _audioSource;        //총 소리
    public AudioClip _shotClip;             //총성
    public AudioClip _reloadClip;           //재장전 소리

    //public LineRenderer _bulletLineRenderer;
    public GameObject _impactPrefab;        //피탄 효과
    public Text _ammoText;                  //남은 탄환수

    //Stat 나중에 데이터 시트로 정리할 예정.
    public int ammoBulletCount = 13; //탄창 당 총알의 개수
    public int ammoCount = 10; //보유한 탄창의 수
    public float fireRate = 0.3f; //발사 간격
    public int _damage = 25; //데미지
    public float reloadTime = 2.0f; //재장전 시간
    public float _fireDistance = 100f;

    //
    private float _lastFireTime; //총을 마지막으로 발사한 시점.
    private int curBulletCount = 0; //현재 보유한 총알의 수

    //State
    public enum GunState { Ready, Empty, Reloading }
    public GunState State
    {
        get { return _currentState; }
        set { _currentState = value; }

    }
    GunState _currentState = GunState.Empty;

    

    Animator anim;


    //수정 사항: animator 변수의 이름을 anim으로 변경, public이 아닌 GetComponent로 변경, bulletLineRenderer의 사용을 취소함
    public override void Init()
    {
        anim = GetComponent<Animator>();
        _currentState = GunState.Empty;
        _lastFireTime = 0;
        //bullet = Managers.Resource.Load<GameObject>("Prefabs/Weapon/Bullet");
        //_bulletLineRenderer.positionCount = 2;
        //_bulletLineRenderer.enabled = false;

        UpdateUI();

    }

    public override void Fire()
    {

        Debug.Log("Fire!");
        //총이 준비된 상태인가? 마지막 발사 시점에서 연사 간격을 넘어갔는가?
        if(_currentState == GunState.Ready && Time.time >= _lastFireTime + fireRate)
        {
            _lastFireTime = Time.time;
            Shot();
            UpdateUI();
        }else if(_currentState == GunState.Empty)
        {
            Reload();

        }


    }


    //수정 사항: 총알 생성 코드의 추가
    private void Shot()
    {

        //총알 생성 및 addforce를 이용하여 발사
        Bullet shootingBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation).GetComponent<Bullet>();
        Rigidbody bulletRigid = shootingBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = (bulletPos.position - gunPos.position) * shootingBullet.speed;
        shootingBullet._gun = this;
        shootingBullet.dmg = _damage;

        //충돌 판정은 Bullet으로 옮김.
        //발사 -> 총의 데이터를 지닌 채 총알이 날아감 -> 충돌 판정 후 쏜 총의 데미지만큼 상대에게 데미지를 준다.



        //발사 이팩트
        StartCoroutine(ShotEffect(bulletPos.transform.position));

        //탄환 갯수 감소
        curBulletCount--;

        if(curBulletCount <= 0)
        {
            _currentState = GunState.Empty;
        }


    }

    //수정 사항: _bulletLineRenderer의 사용을 취소하고, particle 효과를 담당할 변수를 변경, waitForSeconds를 fireRate로 변경, animator의 parameter 변경
    //발사 이펙트를 재상하고 총알 궤적을 잠시 그리고 끄는 함수
    IEnumerator ShotEffect(Vector3 hitPosition)
    {
        anim.SetBool("Shoot", true);

        //_bulletLineRenderer.enabled = true;
        //_bulletLineRenderer.SetPosition(0, _fireStarting.transform.position);
        //_bulletLineRenderer.SetPosition(1, hitPosition);

        //_muzzleFlashEffect.Play();
        //_shellEjectEffect.Play();
        gunParticle.GetComponent<ParticleSystem>().Play();

        if(_audioSource.clip != _shotClip)
        {
            _audioSource.clip = _shotClip;
        }

        _audioSource.Play();

        yield return new WaitForSeconds(fireRate);
        
        anim.SetBool("Shoot", false);
        //_bulletLineRenderer.enabled = false;

    }




    // 총의 탄약 UI에 남은 탄약수를 갱신해서 띄워줌.
    private void UpdateUI()
    {
        if(_currentState == GunState.Empty)
        {
            _ammoText.text = "Empty";

        }else if(_currentState == GunState.Reloading)
        {

            _ammoText.text = "Reloading";
        }else 
        {
            _ammoText.text = curBulletCount.ToString();
        }

    }

    public void Reload()
    {
        if (_currentState == GunState.Reloading  || ammoCount == 0)
            return;

        StartCoroutine(ReloadRoutine());

    }

    //수정 사항: Animator의 파라미터 이름 변경 및 재장전할 때 탄창 수가 줄어드는 코드 추가
    //실제 재장전 처리가 진행되는 곳.
    IEnumerator ReloadRoutine()
    {
        anim.SetTrigger("DoReload");
        _currentState = GunState.Reloading;
        _audioSource.clip = _reloadClip;
        _audioSource.Play();

        UpdateUI();

        yield return new WaitForSeconds(reloadTime);

        curBulletCount = ammoBulletCount;
        ammoCount--;
        _currentState = GunState.Ready;
        UpdateUI();

    }



}
