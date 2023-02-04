using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject superGranage;

    private void OnEnable()
    {
        Destroy(superGranage, 0.1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "IDamageable")
        {
            BaseController target = other.GetComponent<BaseController>();
            if (target == null)
            {
                Debug.Log("잘못된 타겟입니다.");
                return;
            }
            
            WeaponController wc = superGranage.GetComponent<Granade>();
            Debug.Log($"{target.Name} hit");
            superGranage.GetComponent<Granade>().OnAttack(target, wc._weaponData.damage, wc._master);  //수류탄에 입력된 데미지를 받음. 받은 데미지 값을 부딪힌 플레이어에게 적용
        }
    }
}
