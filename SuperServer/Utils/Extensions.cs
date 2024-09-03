using SuperServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    public static class Extensions
    {
        public static bool SaveChangeEx(this GameDBContext db)
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch { return false; }
        }
    }
}
