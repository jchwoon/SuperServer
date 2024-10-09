using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public partial class GameRoom
    {
        public void UseSkill(Creature owner, int skillId, int targetId)
        {
            if (owner == null)
                return;

            owner.SkillComponent.UseSKill(skillId, targetId);
        }
    }
}
