using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server.Data;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Server
{
    public class GameRoom : JobSerializer
    {
        
        public int RoomId { get; set; }
        public int TeamdId { get; set; } = 1;
        public int UnitId { get; set; } = 1;

        public Dictionary<int, Player> _players = new Dictionary<int, Player>();
        public Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        public Dictionary<int, Item> _items = new Dictionary<int, Item>();


        public Player _monsterPlayer;
        public Map Map { get; set; }
  
      
        public void Init(int mapId)
        {
            Map = new Map();
            Map.LoadMap(mapId);
        }

        public void Update()
        {
            Flush();
        }

        //TODo
        //중립 몬스터를 생성해보자.

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (gameObject.ObjectType == GameObjectType.Player)
            {
                #region 플레이어 입장
                Player newPlayer = gameObject as Player;

                //게임에 입장했으니 플레이어의 시작 지점을 만들어줘야 한다.
                newPlayer.Info.MovementInfo.PlayerPosInfo = new PositionInfo()
                {
                    PosX = 0,
                    PosY = 0,
                    PosZ = 0,
                };
                newPlayer.Info.MovementInfo.MoveDir = new MoveDirInfo();
                newPlayer.Info.MovementInfo.CameraPosInfo = new PositionInfo();
                newPlayer.Info.MovementInfo.RightHandPosInfo = new PositionInfo();
                newPlayer.Info.MovementInfo.LeftHandPosInfo = new PositionInfo();

                //스탯 추가
                StatInfo stat = null;
                DataManager.StatDict.TryGetValue("Player", out stat);
                newPlayer.Info.StatInfo = stat;
                newPlayer.Info.Name = newPlayer.Info.Player.Name;
                newPlayer.Info.StatInfo.Hp = newPlayer.Info.StatInfo.MaxHp;

                newPlayer.State = PlayerState.Living;

                //선택된 캐릭터은 Client 구역에서 PlayerInfo의 Character 값을 보고 판단.

                //게임룸 안의 플레이어 목록에 추가.
                _players.Add(newPlayer.Info.ObjectId, newPlayer);
                newPlayer.Room = this;

                {
                    //자신에게 보낼 정보
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.Player = newPlayer.Info;
                    newPlayer.Session.Send(enterPacket);


                    //게임 방 안에 있는 플레이어들의 정보 보내기

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player player in _players.Values)
                    {
                        if (player == newPlayer)
                            continue;

                        spawnPacket.Info.Add(player.Info);
                    }
                    newPlayer.Session.Send(spawnPacket);
                }


                //다른 플레이어들에게 보낼 정보.
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Info.Add(newPlayer.Info);
                    foreach (Player player in _players.Values)
                    {
                        if (player == newPlayer)
                            continue;

                        player.Session.Send(spawnPacket);
                    }
                }
                #endregion
            }else if(gameObject.ObjectType == GameObjectType.Monster)
            {

                //몬스터 생성
                Monster monster = gameObject as Monster;
                


            }else if(gameObject.ObjectType == GameObjectType.Item)
            {

                //아이템 생성은 서버에서 일괄적으로 뿌려주는 거기에
                //모든 플레이어들에게 곧바로 보내주면 된다.

                //아이템 생성
                Item item = gameObject as Item;

                //플레이어들에게 보낼 정보.
                {
                    S_ItemSpawn spawnPacket = new S_ItemSpawn();
                    spawnPacket.Infos.Add(item.Info);
                    foreach (Player player in _players.Values)
                    {
                        player.Session.Send(spawnPacket);
                    }
                }


            }
        }

        public void EnterGame(GameObject[] gameObjects)
        {
            if (gameObjects == null)
                return;

            S_ItemSpawn itemSpawnPacket = new S_ItemSpawn();
            S_ObjectSpawn objectSpawnPacket = new S_ObjectSpawn();
            

            foreach (GameObject gameObject in gameObjects)
            {

                if (gameObject.ObjectType == GameObjectType.Monster)
                {

                    //몬스터 생성
                    Monster monster = gameObject as Monster;
                    monster.Room = this;
                    _monsters.Add(monster.ObjectId, monster);
                    objectSpawnPacket.Infos.Add(monster.Info);



                }
                else if (gameObject.ObjectType == GameObjectType.Item)
                {
                    Item item = gameObject as Item;
                    item.Room = this;
                    _items.Add(item.Info.ObjectId, item);
                    itemSpawnPacket.Infos.Add(item.Info);

                }
            }

            foreach (Player player in _players.Values)
            {
                player.Session.Send(itemSpawnPacket);
                player.Session.Send(objectSpawnPacket);
            }

        }

        public void Revive(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (gameObject.ObjectType == GameObjectType.Player)
            {
                #region 플레이어 입장
                Player newPlayer = gameObject as Player;

                //게임에 입장했으니 플레이어의 시작 지점을 만들어줘야 한다.
                newPlayer.Info.MovementInfo.PlayerPosInfo = new PositionInfo()
                {
                    PosX = 0,
                    PosY = 0,
                    PosZ = 0,
                };
                newPlayer.Info.MovementInfo.MoveDir = new MoveDirInfo();
                newPlayer.Info.MovementInfo.CameraPosInfo = new PositionInfo();
                newPlayer.Info.MovementInfo.RightHandPosInfo = new PositionInfo();
                newPlayer.Info.MovementInfo.LeftHandPosInfo = new PositionInfo();

                //스탯 추가
                StatInfo stat = null;
                DataManager.StatDict.TryGetValue("Player", out stat);
                newPlayer.Info.StatInfo = stat;
                newPlayer.Info.Name = newPlayer.Info.Player.Name;
                newPlayer.Info.StatInfo.Hp = newPlayer.Info.StatInfo.MaxHp;

                newPlayer.State = PlayerState.Living;

                //선택된 캐릭터은 Client 구역에서 PlayerInfo의 Character 값을 보고 판단.

                //게임룸 안의 플레이어 목록에 추가.
                _players.Add(newPlayer.Info.ObjectId, newPlayer);
                newPlayer.Room = this;

                {
                    //자신에게 보낼 정보
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.Player = newPlayer.Info;
                    newPlayer.Session.Send(enterPacket);
                }


                //다른 플레이어들에게 보낼 정보.
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Info.Add(newPlayer.Info);
                    foreach (Player player in _players.Values)
                    {
                        if (player == newPlayer)
                            continue;

                        player.Session.Send(spawnPacket);
                    }
                }
                #endregion
            }
        }


        public void SpawnMonster()
        {

          

        }

        public void SpawnItem(Player player)
        {
            S_ItemSpawn itemSpawnPacket = new S_ItemSpawn();
            foreach(Item item in _items.Values)
            {
                itemSpawnPacket.Infos.Add(item.Info);
            }
            player.Session.Send(itemSpawnPacket);

        }

        public void CreateItem()
        {


            GameObject[] items = new GameObject[Map.ItemSpawnPointCount];

            ItemData data = null;

            //아이템 랜덤 생성은 어떻게 해줄까?
            //모든 데이터를 읽는다.
            List<ItemData> allData = new List<ItemData>();
            foreach (ItemData itemdata in DataManager.ItemDict.Values)
            {
                allData.Add(itemdata);
            }
            Random random = new Random();


            //아이템 값을 랜덤으로 넣고, Map의 설정에 따라 위치값 부여.
            for (int i = 0; i < Map.ItemSpawnPointCount; i++)
            {


                Item spawnItem = ObjectManager.Instance.Add<Item>();
                spawnItem.Info.Position = new PositionInfo()
                {
                    PosX = Map.ItemSpawnPoints[i].x,
                    PosY = Map.ItemSpawnPoints[i].y,
                    PosZ = Map.ItemSpawnPoints[i].z,
                };

                int randomCount = random.Next(0, allData.Count);
                data = allData[randomCount];
                spawnItem.Data = data;
                spawnItem.Info.Name = data.name;
                spawnItem.Info.TemplateId = data.id;
                spawnItem.Info.ItemType = data.itemType;
                spawnItem.Room = this;


                items[i] = spawnItem;
            }

            //생성된 아이템을 게임룸의 아이템 리스트에 넣기.
            foreach( Item item in items )
            {
                _items.Add(item.ObjectId, item);
            }
        }

        public void SpawnObject(Player player)
        {
            S_ObjectSpawn objectSpawn = new S_ObjectSpawn();
            foreach (Monster gameObject in _monsters.Values)
            {
                objectSpawn.Infos.Add(gameObject.Info);
            }
            player.Session.Send(objectSpawn);
        }

        public void CreateObject()
        {

            //스폰 패킷 보내고, 클라에서 패킷 핸들러 수정해주기


            GameObject[] objects = new GameObject[Map.ObjectSpawnPointCount];

            StatInfo data = null;
            if (DataManager.StatDict.TryGetValue("HitBox", out data) == false)
            {
                Console.WriteLine("Data값이 없습니다.");
                return;
            }


            //아이템 값을 랜덤으로 넣고, Map의 설정에 따라 위치값 부여.
            for (int i = 0; i < Map.ObjectSpawnPointCount; i++)
            {


                Monster spawnObject = ObjectManager.Instance.Add<Monster>();
                spawnObject.Info.MovementInfo = new MovementInfo()
                {
                    PlayerPosInfo = new PositionInfo()
                    {
                        PosX = Map.ObjectSpawnPoints[i].x,
                        PosY = Map.ObjectSpawnPoints[i].y,
                        PosZ = Map.ObjectSpawnPoints[i].z,
                    }

                };
                spawnObject.Info.StatInfo = data;
                spawnObject.Info.Name = data.Name + "_" + i;
                spawnObject.Room = this;
                objects[i] = spawnObject;
            }

            foreach(Monster monster in objects)
            {
                _monsters.Add(monster.ObjectId, monster);
            }


        }


        public void LeaveGame(int unitId)
        {

            GameObjectType type = ObjectManager.GetObjectTypeById(unitId);

            if (type == GameObjectType.Player)
            {

                Player player = null;
                if (_players.Remove(unitId, out player) == false)
                    return;
                player.Room = null;
                //본인한테 정보 전송
                {
                    S_LeaveGame leavePacket = new S_LeaveGame();
                    player.Session.Send(leavePacket);

                }
            }else if(type == GameObjectType.Monster)
            {
                Monster monster = null;
                if (_monsters.Remove(unitId, out monster) == false)
                    return;
            }
                    
            //타인한테 정보 전송
            {
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.ObjectIds.Add(unitId);
                foreach(Player player in _players.Values)
                {
                    if (player.ObjectId == unitId)
                        continue;

                    player.Session.Send(despawnPacket);
                     
                }   
            }
            
        }


        public void HandleSpawn(Player player, C_Spawn spawnPacket)
        {



        }

        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null)
                return;

            //서버 안의 캐릭터 위치 정보를 변경한다.
            Player handlePlayer = null;
            if (_players.TryGetValue(player.Info.ObjectId, out handlePlayer) == false)
                return;
            handlePlayer.Info.MovementInfo = movePacket.MovementInfo;

            //다른 플레이어들에게 뿌린다.
            S_Move Packet = new S_Move();
            Packet.PlayerId = player.Info.ObjectId;
            Packet.MovementInfo = handlePlayer.Info.MovementInfo;
            BroadCast(Packet);



        }

        public void HandleSkill(Player player, C_Skill skillPacket)
        {
            if (player == null)
                return;

            //서버 안의 유닛 목록에서 상태 수정
             Player serverInfo = null;
            //player 모록에 없으면 종료되었거나 죽은거니, 혹시 스킬 사용 도중에 죽었다면 return. 
            if (_players.TryGetValue(skillPacket.Info.ObjectId, out serverInfo) == false)
                return;


            //패킷 배포
            S_Skill skill = new S_Skill();
            skill.Info = serverInfo.Info;
            skill.SkilIid = skillPacket.Skillid;
            skill.ThrowVelocity = skillPacket.ThrowVelocity;

            foreach (Player p in _players.Values)
            {
                if (p == player)
                    continue;

                p.Session.Send(skill);
            }
        }

        public void HandleHp(Player player, C_ChangeHp changeHpPacket)
        {

            if (player == null)
                return;

            
            //현재까지는 상대방이 Player라는 걸로 가정하고 데미지 판정을 준다.
            if (ObjectManager.GetObjectTypeById(changeHpPacket.ObjectId) == GameObjectType.Player)
            {
                Player serverInfo = null;
                if (_players.TryGetValue(changeHpPacket.ObjectId, out serverInfo) == false)
                    return;

                Player enemyInfo = null;
                if (_players.TryGetValue(changeHpPacket.Attacker.ObjectId, out enemyInfo) == false)
                    return;


                serverInfo.OnDamaged(enemyInfo, changeHpPacket.Damage);
            }
            else if (ObjectManager.GetObjectTypeById(changeHpPacket.ObjectId) == GameObjectType.Monster)
            {
                Monster serverInfo = null;
                if (_monsters.TryGetValue(changeHpPacket.ObjectId, out serverInfo) == false)
                    return;

                Player enemyInfo = null;
                if (_players.TryGetValue(changeHpPacket.Attacker.ObjectId, out enemyInfo) == false)
                    return;


                serverInfo.OnDamaged(enemyInfo, changeHpPacket.Damage);
            }


        }

        public void HandleGetWeapon(Player player, C_GetWeapon GetWeaponPacket)
        {

            if (player == null)
                return;

            S_GetWeapon get = new S_GetWeapon();
            get.Slot = GetWeaponPacket.Slot;
            get.PlayerId = player.ObjectId;
            get.Info = GetWeaponPacket.Info;


            foreach (Player p in _players.Values)
            {
                if (p == player)
                    continue;

                p.Session.Send(get);
            }

        }

        public void HandleChangeWeapon(Player player, C_ChangeWeapon changeWeaponPacket)
        {

            if (player == null)
                return;

            S_ChangeWeapon change = new S_ChangeWeapon();
            change.PlayerId = player.ObjectId;
            change.Slot = changeWeaponPacket.Slot;


            foreach (Player p in _players.Values)
            {
                if (p == player)
                    continue;

                p.Session.Send(change);
            }

        }



        public void ReadObj(ObjectInfo obj)
        {
            Console.WriteLine("unitId :" + obj.ObjectId + " TeamId :" + obj.TeamId);


        }


        public void BroadCast(IMessage Packet)
        {
            foreach (Player p in _players.Values)
            {
                if (p == _monsterPlayer)
                    continue;

                p.Session.Send(Packet);
            }
        }

    }
}
