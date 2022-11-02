using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class RoomManager
    {
        public static RoomManager Instance {get;} = new RoomManager();

        object _lock = new object();
        Dictionary<int, GameRoom> _rooms = new Dictionary<int, GameRoom>();
        Dictionary<int, LobbyRoom> _lobbys = new Dictionary<int, LobbyRoom>();

        int _roomId = 1;
        int _waitRoomId = 1;

        public GameRoom Add(int mapId = 1)
        {
            GameRoom gameRoom = new GameRoom();
            //gameRoom.Init(mapId);
            gameRoom.Push(gameRoom.Init, mapId);

            lock(_lock)
            {
                gameRoom.RoomId = _roomId;
                _rooms.Add(_roomId, gameRoom);
                _roomId++;
            }

            return gameRoom;

        }

        public LobbyRoom WaitRoomAdd()
        {
            LobbyRoom gameRoom = new LobbyRoom();
            //gameRoom.Init(mapId);
            
            lock (_lock)
            {
                gameRoom.RoomId = _waitRoomId;
                _lobbys.Add(_waitRoomId, gameRoom);
                _waitRoomId++;
            }

            return gameRoom;

        }

        public bool Remove(int roomId)
        {
            lock (_lock)
            {
                return _rooms.Remove(roomId);
            }


        }

        public GameRoom Find(int roomId)
        {
            lock(_lock)
            {
                GameRoom room = null;
                if (_rooms.TryGetValue(roomId, out room))
                    return room;

                return null;
            }
        }

        public LobbyRoom WaitRoomFind(int roomId)
        {
            lock (_lock)
            {
                LobbyRoom room = null;
                if (_lobbys.TryGetValue(roomId, out room))
                    return room;

                return null;
            }
        }


    }
}
