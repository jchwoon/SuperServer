using Google.Protobuf.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.DB
{
    [Table("Hero")]
    [Index(nameof(HeroName), IsUnique = true)]
    public class Hero
    {
        public int HeroId { get; set; }
        public int AccountId { get; set; }
        public string HeroName { get; set; }
        public EHeroClassType Class { get; set; }
        public DateTime CreateAt { get; set; }
        public int Level { get; set; }
        public Stats HeroStat { get; set; }
    }

    ///////////////

    [Owned]
    public class Stats
    {
        public float MaxHp { get; set; }
        public float MaxMp { get; set; }
        public float HP { get; set; }
        public float MP { get; set; }
        public float AttackDamage { get; set; }
        public float Defense { get; set; }
        public float MoveSpeed { get; set; }
        public float AtkSpeed { get; set; }
        public float Exp { get; set; }
    }
}
