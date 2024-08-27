using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using SuperServer.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PacketHandler
{
    public static void ReqHeroListToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession) session;
        ReqHeroListToS reqHeroListPacket = new ReqHeroListToS();

        cSession.HandleReqHeroList(reqHeroListPacket);
    }

    public static void ReqCreateHeroToSHandler(PacketSession session, IMessage packet)
    {

    }
}