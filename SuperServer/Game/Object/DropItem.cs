using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using SuperServer.Commander;
using SuperServer.Data;
using SuperServer.Game.Room;
using SuperServer.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Compiler.CodeGeneratorResponse.Types;

namespace SuperServer.Game.Object
{
    public class DropItem : BaseObject
    {
        public ItemData ItemData { get; private set; }
        public Hero Owner { get; private set; }
        public int Count { get; private set; }
        //해당 아이템이 드랍되고 사라지기까지의 시간
        const int LifeTime = 60000;
        IJob _despawnJob;
        public int OwnerDBId { get; private set; }

        public void Init(Hero owner, RewardData reward)
        {
            Owner = owner;
            OwnerDBId = owner.DbHeroId;
            if (DataManager.ItemDict.TryGetValue(reward.ItemId, out ItemData itemData) == true)
                ItemData = itemData;
            ObjectInfo.TemplateId = reward.ItemId;
            Count = reward.Count;
        }

        public void Update()
        {
            StartLifeTime();
        }

        public void OnPickedUp()
        {
            if (Room == null)
                return;

            if (_despawnJob != null)
                _despawnJob.IsCancel = true;

            Room.ExitRoom<DropItem>(this);
        }

        public void Pickup(Hero picker)
        {
            if (Room == null)
                return;

            EPickupFailReason reason = CheckCanPickup(picker);

            PickupDropItemToC pickupItemPacket = new PickupDropItemToC();

            if (reason == EPickupFailReason.None)
            {
                picker.Inventory.AddItem(this);
                OnPickedUp();
            }

            pickupItemPacket.Result = reason;
            picker.Session.Send(pickupItemPacket);
        }

        private EPickupFailReason CheckCanPickup(Hero picker)
        {
            if (picker.DbHeroId != OwnerDBId)
                return EPickupFailReason.NotMine;
            else if (picker.Inventory.CheckFull(ItemData, Count) == true)
                return EPickupFailReason.Full;

            return EPickupFailReason.None;
        }
        private void StartLifeTime()
        {
            if (Room == null)
                return;

            GameRoom room = Room;
            _despawnJob = GameCommander.Instance.PushAfter(LifeTime, room.ExitRoom<DropItem>, this);
        }
    }
}
