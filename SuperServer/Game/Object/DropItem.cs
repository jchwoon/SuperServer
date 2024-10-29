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
        //해당 아이템이 드랍되고 사라지기까지의 시간
        const int LifeTime = 60000;
        IJob _despawnJob;

        public void Init(Hero owner, int itemId)
        {
            Owner = owner;
            if (DataManager.ItemDict.TryGetValue(itemId, out ItemData itemData) == true)
                ItemData = itemData;
            ObjectInfo.TemplateId = itemId;
        }

        public void Update()
        {
            StartLifeTime();
        }
        //Temp 나중에 주울려는 사람이 owner와 일치하는지 check먼저
        public void OnPickedUp()
        {
            if (Room == null)
                return;

            if (_despawnJob != null)
                _despawnJob.IsCancel = true;

            GameRoom room = Room;
            _despawnJob = GameCommander.Instance.PushAfter(LifeTime, room.ExitRoom<DropItem>, this);
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
