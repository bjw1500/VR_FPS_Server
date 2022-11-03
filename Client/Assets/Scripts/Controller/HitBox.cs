using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : BaseController
{
    public override void Init()
    {

        base.Init();

        if (Info.StatInfo == null)
        {
            StatInfo stat = null;
            Managers.Data.StatDict.TryGetValue("HitBox", out stat);
            if (stat == null)
            {
                Debug.Log("스탯 정보가 없습니다.");
                return;
            }

            Info.StatInfo = stat;
        }
    }

    public override void OnDamage(int damage, ObjectInfo attacker)
    {
        base.OnDamage(damage, attacker);
        Debug.Log("오버라이드된 OnDamage");
    }
}