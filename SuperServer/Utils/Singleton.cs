using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    public class Singleton<T>  where T : new()
    {
        static T _instance = new T();

        public static T Instance { get { return _instance; } }
    }
}
