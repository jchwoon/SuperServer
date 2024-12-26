using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.StateMachine;
using SuperServer.DB;
using SuperServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperServer.Game.Room;
using SuperServer.Utils;
using Google.Protobuf.Protocol;
using SuperServer.Logic;

namespace SuperServer.Game.Object
{
    public class Monster : Creature
    {
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
            int retDamage = GetCalculatedDamage(damage);
            AggroComponent.OnDamage(attacker.ObjectId, retDamage);
            if (retDamage > 0)
                Machine.OnDamage();
        }

        public override void OnDie(Creature killer)
        {
            if (CurrentState == ECreatureState.Die)
                return;
            SelectRecipientAndGiveReward();
            base.OnDie(killer);

            Machine.OnDie();

            GameCommander.Instance.PushAfter(1000, Room.ExitRoom<Monster>, this);
            GameCommander.Instance.PushAfter(3000, Room.ReSpawn, this);
        }

        public override void ReSpawn()
        {
            base.ReSpawn();
            AggroComponent.Clear();
            Machine.Clear();
            Machine.ChangeState(Machine.IdleState);
        }

        public void Reset()
        {
            StatComponent.SetStat(EStatType.Hp, StatComponent.GetStat(EStatType.MaxHp));
            BroadcastStat();
        }

        private void InitSkill()
        {
            Dictionary<int, int> skills = new Dictionary<int, int>();

            foreach (int id in MonsterData.SkillIds)
            {
                skills.Add(id, 1);
            }
            SkillComponent.RegisterSkill(skills);
        }

        private void SelectRecipientAndGiveReward()
        {
            Hero hero = SelectRecipient();

            if (hero == null) 
                return;

            RewardTableData tableData;
            if (DataManager.RewardTableDict.TryGetValue(ObjectInfo.TemplateId, out tableData) == false)
                return;

            hero.GiveExpAndGold(tableData);
            //인벤이 꽉 차 있으면 DropItem 아니면 AddInven
            AddInvenItemAndDropItem(hero, tableData);
        }

        private Hero SelectRecipient()
        {
            int topId = AggroComponent.GetTopDamageAttackerId();
            Hero hero = Room.FindHeroById(topId);

            return hero;
        }

        private void AddInvenItemAndDropItem(Hero recipient, RewardTableData tableData)
        {
            foreach (RewardInfo info in tableData.RewardInfos)
            {
                double randValue = _rand.NextDouble();
                if (randValue < info.Probability)
                {
                    if (DataManager.RewardDict.TryGetValue(info.RewardId, out RewardData rewardData) == false)
                        return;

                    if (DataManager.ItemDict.TryGetValue(rewardData.ItemId, out ItemData itemData) == false)
                        return;

                    DropItem dropItem = ObjectManager.Instance.Spawn<DropItem>();
                    dropItem.Init(recipient, rewardData);
                    if (recipient.Inventory.CheckFull(itemData, rewardData.Count))
                    {
                        Vector3 offset = GetRandomOffset(1);
                        dropItem.PosInfo.MergeFrom(PosInfo);
                        dropItem.PosInfo.PosX += offset.X;
                        dropItem.PosInfo.PosY += offset.Y;
                        dropItem.PosInfo.PosZ += offset.Z;
                        GameRoom room = Room;
                        GameCommander.Instance.Push(room.EnterRoom<DropItem>, dropItem, false);
                    }
                    else
                    {
                        recipient.Inventory.AddItem(dropItem);
                    }
                }
            }
        }

        private Vector3 GetRandomOffset(float radius)
        {
            float angle = (float)(_rand.NextDouble() * Math.PI * 2);
            float dist = (float)(_rand.NextDouble() * radius);

            return new Vector3(dist * (float)Math.Cos(angle), 0, dist * (float)Math.Sin(angle));
        }
    }
}
