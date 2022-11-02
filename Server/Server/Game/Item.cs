using Google.Protobuf.Protocol;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class Item : GameObject
    {

        public ItemData Data { get; set; }
        public ItemInfo Info { get; set; }

        public int TemplateId
        {
            get { return Data.id; }
        }

        public string Name
        {
            get { return Data.name; }
        }

        public ItemType Type
        {
            get { return Data.itemType; }
        }

        public Item()
        {
            ObjectType = GameObjectType.Item;
            Info = new ItemInfo();
         
        }
    }
}
