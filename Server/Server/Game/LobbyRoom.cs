using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server
{
    public class LobbyRoom
    {
        object _lock = new object();
        bool _startFlag = true;
        public int RoomId { get; set; }
        public int GameRoomId { get; set; }
        public Dictionary<int, Player> _players = new Dictionary<int, Player>();
        


        public void EnterLobbyRoom(Player player)
        {

            lock (_lock)
            {
                //방 안에 들어오면 로비룸의 플레이어 목록에 추가
                _players.Add(player.Info.ObjectId, player);
                player.Info.TeamId = _players.Count;
                player.LobbyRoom = this;

                foreach (Player enterPlayer in _players.Values)
                {
                    //방안에 들어와있는 플레이어의 정보를 받는다.
                    S_EnterWaitingRoom enterPacket = new S_EnterWaitingRoom();
                    enterPacket.Info = enterPlayer.Info;

                    if(player == enterPlayer)
                        enterPacket.MyPlayer = true;

                    player.Session.Send(enterPacket);
                }


                //방안에 있던 플레이어들에게 패킷을 뿌린다.
                foreach (Player enterPlayer in _players.Values)
                {

                    //보내진 패킷은 기존에 있던 플레이어들의 목록에 추가.

                    S_EnterWaitingRoom enterPacket = new S_EnterWaitingRoom();
                    enterPacket.Info = player.Info;
                    if (enterPlayer == player)
                    {
                        continue;
                    }
        
                    enterPlayer.Session.Send(enterPacket);
                }
                //TODO UNITY 대기방 만들기

            }


        }

        public void EnterLobbyScene(Player player)
        {
            //OK 패킷을 보내자..
            player.LobbyRoom = this;
            Console.WriteLine($"{player.Info.Player.Name}이 로비에 입장했습니다.");
            S_EnterLobbyOk okPacket = new S_EnterLobbyOk();
            player.Session.Send(okPacket);
        }

        public void SelectCharacter(C_SelectCharacter packet)
        {
            Player player = null;
            _players.TryGetValue(packet.PlayerId, out player);
            if (player == null)
            {
                Console.WriteLine("Error SelecterCharacter. Player Try Get Value.");

            }

            Console.WriteLine($"{player.Info.Player.Name}이 {packet.CharacterNumber} 선택했습니다.");

            player.Info.Player.ChracterId = packet.CharacterNumber;

            S_SelectCharacter select = new S_SelectCharacter();
            select.PlayerId = player.ObjectId;
            select.PlayerInfo = player.Info.Player;
            BroadCast(select);
        }



        public void StartGame(C_StartGame startPacket)
        {
            lock (_lock)
            {
                if (_startFlag == false)
                    return;

                _startFlag = true;

                //TODO
                //나중에 플레이어들이 로비창에서 캐릭터 선택할 수 있도록 해주기.

                //TODO
                //Client에서 플레이어 캐릭터 생성할 프리팹 번호로 나누어주기.


                //플레이어들이 고른 맵을 생성한 후, 입장시킨다.
                GameRoom gameRoom = RoomManager.Instance.Add(startPacket.MapId);
                GameRoomId = gameRoom.RoomId;
                Program.TickRoom(gameRoom, 50);
                gameRoom.Push(gameRoom.CreateItem);
                gameRoom.Push(gameRoom.CreateObject);

                foreach(PlayerInfo info in startPacket.Players)
                {

                    Player player = null;
                    _players.TryGetValue(info.ObjectId, out player);
                    if(player == null)
                    {
                        Console.WriteLine("Error StartGame - player null");
                        break;
                    }
                    player.Info.TeamId = player.Info.TeamId % 2;
                    player.Info.Player.Kill = 0;
                    player.Info.Player.Death = 0;
                    player.Info.Player.ChracterId = info.ChracterId;


                }

                S_StartGame start = new S_StartGame();
                start.MapId = startPacket.MapId;
                BroadCast(start);
            }
        }

        public void LoadingFinish(int playerId)
        {

            GameRoom gameRoom = RoomManager.Instance.Find(GameRoomId);
            foreach (Player player in _players.Values)
            {
                //EnterGame에서 플레이어 스폰 패킷을 뿌린다.

                if (player.ObjectId != playerId)
                    continue;

                gameRoom.Push(gameRoom.EnterGame, player);
                gameRoom.Push(gameRoom.SpawnItem, player);
                gameRoom.Push(gameRoom.SpawnObject, player);
                //player.LobbyRoom = null;
                Console.WriteLine($"{player.Info.Player.Name}의 로딩이 끝났습니다.");

            }

            //이렇게 하면 플레이어가 로딩을 끝날 때마다 템이 생겨난다.
            //이걸 어떻게 해결하지?
            //1.맵에서 미리 아이템을 생성한다.
            //2.플레이어 개인은 로딩이 끝나면 그런 맵의 정보를 받는다.

            //gameRoom.Push(gameRoom.SpawnItem);
            //gameRoom.Push(gameRoom.SpawnObject);
            

            //Lobby에서 전부 퇴장시킨다.
           // _players.Remove(playerId);
        }

        public void LeaveGame(int playerId)
        {

            Player player = null;
            if (_players.Remove(playerId, out player) == false)
                return;

            player.LobbyRoom = null;

            //타인한테 정보 전송
            {
                S_LeaveWaitingRoom leave = new S_LeaveWaitingRoom();
                leave.Info = player.Info;
                BroadCast(leave);
            }

        }
        public void BroadCast(IMessage Packet)
        {


            foreach (Player p in _players.Values)
            {

                p.Session.Send(Packet);
            }


        }


    }
}
