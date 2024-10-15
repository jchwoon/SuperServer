using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.DB;
using SuperServer.Game.StateMachine;
using SuperServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Object
{
    public class Monster : Creature
    {
        public int MonsterId { get; private set; }
        public MonsterMachine Machine { get; set; }
        public MonsterData MonsterData { get; set; }
        public AggroComponent AggroComponent { get; set; }


        public void Init(int monsterId, PoolData poolData)
        {
            MonsterData monsterData;
            if (DataManager.MonsterDict.TryGetValue(monsterId, out monsterData) == false)
                return;
            AggroComponent = new AggroComponent();
            MonsterData = monsterData;
            PoolData = poolData;
            MonsterId = monsterId;
            ObjectInfo.TemplateId = monsterId;
            StatComponent.InitSetStat(monsterData);
            Machine = new MonsterMachine(this);
            Machine.ChangeState(Machine.IdleState);
        }

        public void Update()
        {
            Machine.Update();
        }

        public override void OnDamage(Creature attacker, float damage)
        {
            base.OnDamage(attacker,damage);
            Machine.OnDamage(attacker);
            AggroComponent.OnDamage(attacker.ObjectId, damage);
        }
    }
}
