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
        public Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public List<bool> _checkSlot = new List<bool>();
        //플레이어의 슬롯 카운트를 센다?

        public void CheckSlotCount(Player player)
        {
            //처음에는 1로 시작한다.
            //1~4까지. 슬롯을 확인했을 때 비어 있으면 슬롯을 부여한다?
            foreach(bool emptySlot in _checkSlot)
            {
                if(emptySlot == false)
                {
                    //플레이어의 슬롯 넣어주기.




                }
            }
        }

        public void EnterLobbyRoom(Player player)
        {

            lock (_lock)
            {
                //방 안에 들어오면 로비룸의 플레이어 목록에 추가
                _players.Add(player.Info.ObjectId, player);
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

        internal void EnterLoobyScene(Player player)
        {
            //OK 패킷을 보내자..
            player.LobbyRoom = this;
            S_EnterLobbyOk okPacket = new S_EnterLobbyOk();
            player.Session.Send(okPacket);
        }

        public void StartGame(C_StartGame startPacket)
        {
            lock (_lock)
            {
                if (_startFlag == false)
                    return;

                _startFlag = true;


                //플레이어들을 게임 룸에 입장시킨다.
                GameRoom gameRoom = RoomManager.Instance.Find(1);

                foreach (Player player in _players.Values)
                {

                    S_StartGame start = new S_StartGame();

                    player.Info.TeamId = startPacket.Slot % 2;
                    player.Info.Player.Kill = 0;
                    player.Info.Player.Death = 0;
                    player.Session.Send(start);
                    player.LobbyRoom = null;
                }



                foreach (Player player in _players.Values)
                {
                    //EnterGame에서 플레이어 스폰 패킷을 뿌린다.
                    gameRoom.Push(gameRoom.EnterGame, player);
                    
                }

                gameRoom.Push(gameRoom.SpawnItem);
                gameRoom.Push(gameRoom.SpawnObject);

                //Lobby에서 전부 퇴장시킨다.
                _players.Clear();

            }
            
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
