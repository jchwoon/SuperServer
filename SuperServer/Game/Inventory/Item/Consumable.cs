using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Skill.Effect;
using SuperServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Inventory
{
    public class Consumable : Item
    {
        public ConsumableData ConsumableData { get; private set; }
        public Consumable(int ownerDBId, int itemId) : base(ownerDBId, itemId)
        {
            if (ItemData != null)
            {
                ConsumableData = (ConsumableData)ItemData;
            }
        }

        public override bool CheckCanUseItemAndUse(InventoryComponent inventory)
        {
            if (inventory == null)
                return false;

            if (Count <= 0)
                return false;

            if (inventory.CheckCanUseItem(ConsumableData.ConsumableType, ConsumableData.CoolTime) == false)
                return false;

            UseItem(inventory);

            return true;
        }

        private void UseItem(InventoryComponent inventory, int useCount = 1)
        {
            //1. 쿨타임 갱신 2.패킷 보내기 3.수량 차감 
            //4.Item효과 적용 5.Db적용
            inventory.UpdateCoolTick(ConsumableData.ConsumableType);
            inventory.SendUseItemPacket(this);
            inventory.AddCount(this.ItemDbId, -useCount);
            ApplyEffect(inventory.Owner);
            if (Count <= 0)
                inventory.RemoveItem(ItemDbId);
            DBCommander.Instance.Push(DBLogic.Instance.UseConsumeItem, inventory, this);
        }

        private void ApplyEffect(Hero owner)
        {
            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(ConsumableData.EffectId, out effectData) == false)
                return;

            EffectDataEx effectDataEx = new EffectDataEx()
            {
                effectData = effectData,
            };
            owner.EffectComponent.ApplyEffect(owner, effectDataEx);
        }
    }
}
