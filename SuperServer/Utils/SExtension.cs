using SuperServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    public static class SExtension
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
