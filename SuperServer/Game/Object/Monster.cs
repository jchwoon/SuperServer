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
            AggroComponent = new AggroComponent(this);
            MonsterData = monsterData;
            PoolData = poolData;
            MonsterId = monsterId;
            ObjectInfo.TemplateId = monsterId;
            StatComponent.InitSetStat(monsterData);
            InitSkill();
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
            AggroComponent.OnDamage(attacker.ObjectId, damage);
            Machine.OnDamage();
        }

        public override void OnDie(Creature killer)
        {
            if (CurrentState == ECreatureState.Die)
                return;

            base.OnDie(killer);
            Machine.OnDie();
            GameCommander.Instance.PushAfter(1000, Room.ExitRoom<Monster>, this);
            GameCommander.Instance.PushAfter(3000, Room.ReSpawn, this);
        }

        public override void ReSpawn()
        {
            base.ReSpawn();
            Machine.ChangeState(Machine.IdleState);
            AggroComponent.Clear();
        }

        public void Reset()
        {
            StatComponent.SetStat(EStatType.Hp, StatComponent.GetStat(EStatType.MaxHp));
            BroadcastStat();
        }

        private void InitSkill()
        {
            SkillComponent.RegisterSkill(MonsterData.SkillIds);
        }
    }
}
