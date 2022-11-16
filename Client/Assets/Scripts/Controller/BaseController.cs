using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    /*
     * 상속구조
     * BaseController
     *  CharacterMainControllerVR
     *  CharacterRemoteControllerVR
     *  HitBox
     */

    private ObjectInfo _info;

    public ObjectInfo Info
    {
        get { return _info; }
        set { _info = value; }
    }

    public virtual int Hp
    {
        get { return Info.StatInfo.Hp; }
        set { Info.StatInfo.Hp = value; }
    }

    public string Name
    {
        get { return Info.Name; }
    }

    public StatInfo Stat
    {
        get { return _info.StatInfo; }
        set { _info.StatInfo = value; }
    }


    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        if (Info == null)
        {
            Info = new ObjectInfo();
        }

        transform.gameObject.tag = "IDamageable";
        transform.gameObject.layer = LayerMask.NameToLayer("IDamageable");
    }

    public virtual void OnDamage(int damage, ObjectInfo attacker)
    {
        Debug.Log($"{attacker.Name}에게서 {damage} 데미지를 받았다.");

        if (GameMng.I.SingleGame == true)
        {
            //싱글모드일때 데미지 판정
            Hp -= damage;
            Debug.Log($"{attacker.Name}에게서 {damage} 데미지를 받았다.");
            if (Hp <= 0)
                OnDead(attacker);

            return;
        }

        //서버 연결이 되었을 때 데미지 판정
        {

            C_ChangeHp packet = new C_ChangeHp();
            packet.ObjectId = Info.ObjectId;
            packet.Attacker = attacker;
            packet.Damage = damage;
            Managers.Network.Send(packet);

            //데미지 판정은 서버 안에서 이루어져야 한다.
            //그런데 플레이어가 뭘 들고 있는지도 서버에서 다뤄야하나?
        }
    }

    public virtual void OnDead(ObjectInfo attacker)
    {
        Debug.Log($"{transform.name}이 {attacker.Name}한테 죽었습니다.");
    }
}
