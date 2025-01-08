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
    public class DBHero
    {
        public int DBHeroId { get; set; }
        public int AccountId { get; set; }
        public string HeroName { get; set; }
        public EHeroClassType Class { get; set; }
        public DateTime CreateAt { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }
        public long Exp { get; set; }
        public Stats HeroStat { get; set; }
        public int RoomId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float RotY { get; set; }
        public ICollection<DBItem> Items { get; set; } = new List<DBItem>();
        public int EquipmentSlotCount { get; set; }
        public int ConsumableSlotCount { get; set; }
        public int ETCSlotCount { get; set; }
        public Dictionary<int, int> Skills { get; set; } = new Dictionary<int, int>();
        public int ActiveSkillPoint { get; set; }
        public int PassiveSkillPoint { get; set; }
    }

    [Table("Item")]
    public class DBItem
    {
        //데이터 베이스에서 자체 Id생성 금지 Application에서 관리하도록
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DBItemId { get; set; }
        public int ItemTemplateId { get; set; }
        public int Count { get; set; }
        public ESlotType SlotType { get; set; }
        public int OwnerDbId { get; set; }
        public DBHero OwnerDb { get; set; }
    }

    ///////////////

    [Owned]
    public class Stats
    {
        public int MaxHp { get; set; }
        public int MaxMp { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int AtkDamage { get; set; }
        public int Defence { get; set; }
        public float MoveSpeed { get; set; }
    }
}
