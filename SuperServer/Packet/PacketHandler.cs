using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PacketHandler
{
    public static void LoginToSHandler(IMessage packet)
    {
        LoginToS loginPacket = packet as LoginToS;

    }
}