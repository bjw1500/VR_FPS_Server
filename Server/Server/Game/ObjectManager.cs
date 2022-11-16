using Google.Protobuf.Protocol;
using Server.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class ObjectManager
    {
        public static ObjectManager Instance { get; } = new ObjectManager();

        object _lock = new object();
        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        int _counter = 0;
        // [UNUSED(1)][TYPE(7)][ID(24)]


        public T Add<T>() where T : GameObject, new()
        {
            T gameObject = new T();

            lock (_lock)
            {
                
                if (gameObject.ObjectType == GameObjectType.Player)
                {
                    Player player = gameObject as Player;
                    player.ObjectId = GenerateId(gameObject.ObjectType);
                    player.Info.ObjectType = GameObjectType.Player;
                    
                    _players.Add(_counter, gameObject as Player);
                }
                else if(gameObject.ObjectType == GameObjectType.Item)
                {
                    Item item = gameObject as Item;
                    item.Info.ObjectId = GenerateId(gameObject.ObjectType);
              
                    _players.Add(_counter, gameObject as Player);

                }else if(gameObject.ObjectType == GameObjectType.Monster)
                {
                    Monster monster = gameObject as Monster;
                    monster.Info.ObjectId = GenerateId(gameObject.ObjectType);
                    monster.Info.ObjectType = GameObjectType.Monster;


                }



            }

            return gameObject;

        }

        int GenerateId(GameObjectType type)
        {
            lock (_lock)
            {
                return ((int)type << 24) | (_counter++);
            }
        }

        public static GameObjectType GetObjectTypeById(int id)
        {
            int type = (id >> 24) & 0x7F;
            return (GameObjectType)type;
        }


        public bool Remove(int playerId)
        {
            lock (_lock)
            {
                return _players.Remove(playerId);
            }


        }

        public Player Find(int playerId)
        {
            lock (_lock)
            {
                Player player = null;
                if (_players.TryGetValue(playerId, out player))
                    return player;

                return null;

            }
        }


    }
}
