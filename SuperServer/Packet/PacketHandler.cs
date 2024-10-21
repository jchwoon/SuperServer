using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using SuperServer.Commander;
using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Logic;
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
        ReqHeroListToS reqHeroListPacket = (ReqHeroListToS)packet;

        cSession.HandleReqHeroList(reqHeroListPacket);
    }

    public static void ReqCreateHeroToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;

        ReqCreateHeroToS reqHeroListPacket = (ReqCreateHeroToS)packet;

        cSession.HandleReqCreateHero(reqHeroListPacket);
    }

    public static void ReqDeleteHeroToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;

        ReqDeleteHeroToS reqDeleteHeroPacket = (ReqDeleteHeroToS)packet;

        cSession.HandleReqDeleteHero(reqDeleteHeroPacket);
    }
    public static void PreEnterRoomToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;

        PreEnterRoomToS preEnterPacket = (PreEnterRoomToS)packet;

        cSession.HandlePreEnterRoom(preEnterPacket);
    }

    public static void ReqEnterRoomToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;

        ReqEnterRoomToS reqEnterRoomPacket = (ReqEnterRoomToS)packet;

        cSession.HandleReqEnterRoom(reqEnterRoomPacket);
    }

    public static void ReqLeaveGameToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;

        ReqLeaveGameToS reqLeaveGamePacket = (ReqLeaveGameToS)packet;

        cSession.HandleLeaveGame(reqLeaveGamePacket);
    }

    public static void MoveToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        MoveToS movePacket = (MoveToS)packet;

        if (cSession.PlayingHero != null && cSession.PlayingHero.Room != null)
        {
            Hero hero = cSession.PlayingHero;
            GameRoom room = cSession.PlayingHero.Room;
            GameCommander.Instance.Push(room.HandleMove, hero, movePacket);
        }
    }

    public static void PingCheckToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        PingCheckToS pingPacket = (PingCheckToS)packet;

        cSession.HandlePing();
    }

    public static void ReqUseSkillToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        ReqUseSkillToS skillPacket = (ReqUseSkillToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.UseSkill, hero, skillPacket.SkillId, skillPacket.TargetId);
    }
}