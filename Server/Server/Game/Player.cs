using Google.Protobuf.Protocol;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class PlayerSlot
    {
        public int _slot;
        public PlayerInfo _playerInfo;


        //플레이어 입장.
        //슬롯 변경
        //만약 플레이어가 나가면, 슬롯 비우고 플레이어 위치 변경?


    }


    public class Player : GameObject
    {

        public ClientSession Session { get; set; }
        public LobbyRoom LobbyRoom { get; set; }

        public Dictionary<int, Item> OwnItem = new Dictionary<int, Item>();

        public ObjectInfo Info { get; set; }

        public PlayerState State { get; set; }

        public StatInfo Stat
        {
            get { return Info.StatInfo; }
            set { Info.StatInfo = value; }
        }

        public Player()
        {
            ObjectType = GameObjectType.Player;
            Info = new ObjectInfo();
            Info.Player = new PlayerInfo();
            Info.Player.Death = 0;
            Info.Player.Kill = 0;
            Info.Player.ChracterId = 0;
            Info.MovementInfo = new MovementInfo();


            {   //작업중
                //살아있는 상태라면 데미지 적용.
                State = PlayerState.Living;


            }

        }


        public override void OnDamaged(GameObject Attacker, int damage)
        {

            Player attacker = null;
            Room._players.TryGetValue(Attacker.ObjectId, out attacker);

            if (State != PlayerState.Living)
                return;
            //if (attacker == this && (attacker.Info.TeamId == Info.TeamId))
            //{
            //    //자기 자신에 대한 공격은 무효화 해준다.
            //    Console.WriteLine($"{attacker.Info.Name}가 자기 자신을 공격했습니다!");
            //    return;
            //}


            int totalDamage = damage + attacker.Info.StatInfo.Damage;
            if (totalDamage < 0)
                totalDamage = 0;
            attacker.Info.Player.TotalDamage += totalDamage;

            Console.WriteLine($"{attacker.Info.Name}가 {totalDamage}만큼 {Info.Name}을 공격했습니다 {totalDamage}!");

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

            Player attacker = null;

            Room._players.TryGetValue(Attacker.ObjectId , out attacker);

            if (State != PlayerState.Living)
                return;

            Console.WriteLine($"{attacker.Info.Name}가 {Info.Name}를 죽였습니다!");
            State = PlayerState.Dead;

            //PlayerList 정보 수정.
            attacker.Info.Player.Kill++;
            Info.Player.Death++;



            Console.WriteLine($"현재 {attacker.Info.Player.Name}이 {attacker.Info.Player.Kill}킬 기록 중입니다.");
             
            S_Die diePacket = new S_Die();
            diePacket.Attacker = attacker.Info;
            diePacket.ObjectId = Info.ObjectId;
            Room.BroadCast(diePacket);


            //죽고 나서 다시 스폰?
            //템도 모두 지워준다?
            Room._players.Remove(Info.ObjectId);
            Room.Push(Room.Revive, this);


            //플레이어 킬로그 업데이트
            S_UpdatePlayerInfo playerinfo = new S_UpdatePlayerInfo();
            playerinfo.Infos.Add(attacker.Info);
            playerinfo.Infos.Add(Info);

            Room.BroadCast(playerinfo);

        }

    }
}
