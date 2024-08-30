using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Data
{
    public class HeroStatData
    {
        public int Level;
        public float AtkDamage;
        public float MaxHp;
        public float MaxMp;
        public float Defence;
        public float AtkSpeed;
        public float MoveSpeed;
        public float Exp;
    }

    [Serializable]
    public class HeroStatDataLoader : ILoader<int, HeroStatData>
    {
        public List<HeroStatData> stats = new List<HeroStatData>();

        public Dictionary<int, HeroStatData> MakeDict()
        {
            Dictionary<int, HeroStatData> dict = new Dictionary<int, HeroStatData>();
            foreach (HeroStatData stat in stats)
                dict.Add(stat.Level, stat);

            return dict;
        }
    }
}
