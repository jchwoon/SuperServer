using Google.Protobuf.Enum;
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
    public class Equipment : Item
    {
        public EquipmentData EquipmentData { get; private set; }
        public Equipment(int ownerDBId, int itemId) : base(ownerDBId, itemId)
        {
            if (ItemData != null)
            {
                EquipmentData = (EquipmentData)ItemData;
            }
        }

        public static readonly Dictionary<EEquipItemType, ESlotType> GetSlotDict = new Dictionary<EEquipItemType, ESlotType>()
        {
            {EEquipItemType.Weapon, ESlotType.Weapon },
            {EEquipItemType.Shield, ESlotType.Shield},
            {EEquipItemType.Helmet, ESlotType.Helmet },
            {EEquipItemType.Armor, ESlotType.Armor },
            {EEquipItemType.Boots, ESlotType.Boots },
            {EEquipItemType.Gloves, ESlotType.Gloves },
            {EEquipItemType.Pendant, ESlotType.Pendant },
        };

        private bool CheckCanEquipItem(Hero hero)
        {
            if (hero == null)
                return false;

            if (EquipmentData.ClassType != hero.HeroInfo.LobbyHeroInfo.ClassType)
                return false;

            if (EquipmentData.RequiredLevel > hero.HeroInfo.LobbyHeroInfo.Level)
                return false;

            return true;
        }

        public void EquipItem(Hero owner)
        {
            if (owner == null)
                return;

            InventoryComponent inventory = owner.Inventory;
            if (inventory == null)
                return;

            bool canEquip = CheckCanEquipItem(owner);
            if (canEquip == false)
                return;

            ESlotType slotType = GetSlotDict[EquipmentData.EquipItemType];

            Equipment equipItem = inventory.GetCurrentEquippedItem(slotType);
            if (equipItem != null)
            {
                equipItem.UnEquipItem(owner);
            }

            //Slot 변경 //ToDo Ring
            inventory.AddEquippedItem(slotType, this);
            SlotType = slotType;

            //ApplyEffect
            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(EquipmentData.EffectId, out effectData) == true)
            {
                EffectDataEx effectEx = new EffectDataEx() { effectData = effectData };
                owner.EffectComponent.ApplyEffect(owner, effectEx);
            }

            //Db반영
            DBCommander.Instance.Push(DBLogic.Instance.UpdateEquipItem, owner, this);
            //패킷 보내기
            SendChangeSlotPacket(owner);
            owner.BroadcastStat();
        }

        public void UnEquipItem(Hero owner)
        {
            if (owner == null)
                return;

            InventoryComponent inventory = owner.Inventory;
            if (inventory == null)
                return;

            ESlotType slotType = ESlotType.Equip;
            ESlotType prevSlot = SlotType;

            inventory.RemoveEquippedItem(prevSlot);
            SlotType = slotType;

            //ReleaseEffect
            EffectData effectData;
            if (DataManager.EffectDict.TryGetValue(EquipmentData.EffectId, out effectData) == true)
                owner.EffectComponent.ReleaseEffect(effectData.TemplateId);
            //DB반영
            DBCommander.Instance.Push(DBLogic.Instance.UpdateEquipItem, owner, this);
            //패킷 보내기
            SendChangeSlotPacket(owner);
            owner.BroadcastStat();
        }
    }
}
