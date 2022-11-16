using Google.Protobuf.Protocol;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class GameObject
    {

        public GameObjectType ObjectType;
        public virtual int ObjectId { get; set; }
 
        public GameRoom Room { get; set; }
       
        public GameObject()
        {
 
        }

        public virtual void Update()
        {


        }

        public virtual void OnDamaged(GameObject Attacker, int Damage)
        {
            






        }
        
        public virtual void OnDead(GameObject Attacker)
        {



            

        }



    }
}
