﻿using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Skill
{
    public class NormalSkill : BaseSkill
    {
        public NormalSkill(Creature owner, SkillData skillData, int skillId) : base(owner, skillData, skillId)
        {
        }

        protected override bool CheckCoolTime()
        {
            long elapsedTime = GetElapsedTimeAfterLastUseSkill();
            if (elapsedTime >= SkillData.AnimTime * Owner.StatComponent.StatInfo.AtkSpeed *  1000)
                return true;

            return false;
        }
    }
}