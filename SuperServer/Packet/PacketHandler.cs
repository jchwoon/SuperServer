using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Packet
{
    class PacketHandler
    {
        public static void C_TestHandler(IMessage packet)
        {
            C_Test test = packet as C_Test;

            Console.WriteLine(test.Name);
        }
    }
}
