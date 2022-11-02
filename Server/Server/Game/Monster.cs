using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Monster : GameObject
    {
        public ObjectInfo Info { get; set; }
        public PlayerState State { get; set; }

        public int ObjectId
        {
            get { return Info.ObjectId; }
            set { Info.ObjectId = value; }

        }
        public StatInfo Stat
        {
            get { return Info.StatInfo; }
            set { Info.StatInfo = value; }
        }

        public Monster()
        {
            ObjectType = GameObjectType.Monster;
            Info = new ObjectInfo();
            Info.Player = new PlayerInfo();
            Info.MovementInfo = new MovementInfo();


            {   //작업중
                //살아있는 상태라면 데미지 적용.
                State = PlayerState.Living;


            }

        }


        public override void OnDamaged(GameObject Attacker, int damage)
        {

            Player attacker = Attacker as Player;

            if (State != PlayerState.Living)
                return;


            int totalDamage = damage + attacker.Info.StatInfo.Damage;
            if (totalDamage < 0)
                totalDamage = 0;

            Console.WriteLine($"{attacker.Info.Name}가 {Info.Name}을 공격했습니다 {totalDamage}!");

            Info.StatInfo.Hp = Math.Max(Info.StatInfo.Hp - totalDamage, 0);

            S_ChangeHp changeHp = new S_ChangeHp();
            changeHp.ObjectId = ObjectId;
            changeHp.Hp = Info.StatInfo.Hp;
            Room.BroadCast(changeHp);

            if (Info.StatInfo.Hp <= 0)
                OnDead(Attacker);

        }

        public override void OnDead(GameObject Attacker)
        {

            Player attacker = Attacker as Player;

            if (State != PlayerState.Living)
                return;

            Console.WriteLine($"{attacker.Info.Name}가 {Info.Name}를 죽였습니다!");
            State = PlayerState.Dead;



            S_Die diePacket = new S_Die();
            diePacket.Attacker = attacker.Info;
            diePacket.ObjectId = Info.ObjectId;
            Room.BroadCast(diePacket);


        }
    }
}
