using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using SuperServer.Commander;
using SuperServer.Game.Party;
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

        GameCommander.Instance.Push(room.HandleUseSkill, hero, skillPacket.SkillInfo, skillPacket.SkillPivot);
    }

    public static void PickupDropItemToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        PickupDropItemToS pickupItemPacket = (PickupDropItemToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.PickupDropItem, hero, pickupItemPacket.ObjectId);
    }

    public static void UseItemToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        UseItemToS useItemPacket = (UseItemToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.HandleUseItem, hero, useItemPacket.ItemdDbId);
    }

    public static void EquipItemToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        EquipItemToS equipItemPacket = (EquipItemToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.HandleEquipItem, hero, equipItemPacket.ItemDbId);
    }

    public static void UnEquipItemToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        UnEquipItemToS unEquipItemPacket = (UnEquipItemToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.HandleUnEquipItem, hero, unEquipItemPacket.ItemDbId);
    }
    
    public static void ChangeRoomToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        ChangeRoomToS changeRoomPacket = (ChangeRoomToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;
        GameCommander.Instance.Push(room.ChangeRoom, changeRoomPacket.RoomId, hero);
    }

    public static void CreatePartyToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        CreatePartyToS createPartyPacket = (CreatePartyToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;
        
        GameCommander.Instance.Push(PartyManager.Instance.CreateParty, hero);
    }

    public static void ReqLevelUpSkillToSHandler(PacketSession session, IMessage packet)
    {
        ClientSession cSession = (ClientSession)session;
        ReqLevelUpSkillToS levelUpSkillPacket = (ReqLevelUpSkillToS)packet;

        Hero hero = cSession.PlayingHero;
        if (hero == null)
            return;

        GameRoom room = hero.Room;
        if (room == null)
            return;

        GameCommander.Instance.Push(room.HandleLevelUpSkill, hero, levelUpSkillPacket.SkillId);
    }
}