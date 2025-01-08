using Google.Protobuf.Enum;
using Google.Protobuf.Struct;
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
        public void HandleUseSkill(Creature owner, SkillInfo skillInfo, PosInfo skillPivot)
        {
            if (owner == null)
                return;

            owner.SkillComponent.UseSKill(skillInfo, skillPivot);
        }

        public void HandleLevelUpSkill(Hero hero, int skillId)
        {
            if (hero == null)
                return;

            hero.SkillComponent.LevelUpSkill(skillId);
        }

        public void HandleInitSkillPoint(Hero hero, ESkillType skillType)
        {
            if (hero == null)
                return;

            hero.SkillComponent.CheckAndResetSkillPoint(skillType);
        }
    }
}
