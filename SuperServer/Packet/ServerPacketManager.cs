
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using ServerCore;

public enum PacketId
{
  ConnectToC = 1,
  ReqHeroListToS = 2,
  ResHeroListToC = 3,
  ReqCreateHeroToS = 4,
  ResCreateHeroToC = 5,
  ReqDeleteHeroToS = 6,
  ResDeleteHeroToC = 7,
  PreEnterRoomToS = 8,
  PreEnterRoomToC = 9,
  ReqEnterRoomToS = 10,
  ResEnterRoomToC = 11,
  ChangeRoomToS = 12,
  ChangeRoomToC = 13,
  SpawnToC = 14,
  CreatePartyToS = 15,
  ReqLeaveGameToS = 16,
  MoveToS = 17,
  MoveToC = 18,
  PingCheckToC = 19,
  PingCheckToS = 20,
  DeSpawnToC = 21,
  ReqUseSkillToS = 22,
  ResUseSkillToC = 23,
  ModifyStatToC = 24,
  ModifyOneStatToC = 25,
  DieToC = 26,
  TeleportToC = 27,
  RewardToC = 28,
  PickupDropItemToS = 29,
  PickupDropItemToC = 30,
  AddItemToC = 31,
  UseItemToS = 32,
  UseItemToC = 33,
  EquipItemToS = 34,
  UnEquipItemToS = 35,
  ChangeSlotTypeToC = 36,
}


class PacketManager
{
    private static PacketManager _instance;

    public static PacketManager Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = new PacketManager();
            }
            return _instance;
        } 
    }

    //들어온 패킷 파싱
    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _parseHandler = new();
    //파싱된 패킷 핸들
    Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new();

    public Action<ushort, IMessage> ClientHandler { get; set; } = null;

    PacketManager()
    {
        PreRaiseHandler();
    }

    private void PreRaiseHandler()
    {
        _parseHandler.Add((ushort)PacketId.ReqHeroListToS, ParsePacket<ReqHeroListToS>);
        _handler.Add((ushort)PacketId.ReqHeroListToS, PacketHandler.ReqHeroListToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqCreateHeroToS, ParsePacket<ReqCreateHeroToS>);
        _handler.Add((ushort)PacketId.ReqCreateHeroToS, PacketHandler.ReqCreateHeroToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqDeleteHeroToS, ParsePacket<ReqDeleteHeroToS>);
        _handler.Add((ushort)PacketId.ReqDeleteHeroToS, PacketHandler.ReqDeleteHeroToSHandler);
        _parseHandler.Add((ushort)PacketId.PreEnterRoomToS, ParsePacket<PreEnterRoomToS>);
        _handler.Add((ushort)PacketId.PreEnterRoomToS, PacketHandler.PreEnterRoomToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqEnterRoomToS, ParsePacket<ReqEnterRoomToS>);
        _handler.Add((ushort)PacketId.ReqEnterRoomToS, PacketHandler.ReqEnterRoomToSHandler);
        _parseHandler.Add((ushort)PacketId.ChangeRoomToS, ParsePacket<ChangeRoomToS>);
        _handler.Add((ushort)PacketId.ChangeRoomToS, PacketHandler.ChangeRoomToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqLeaveGameToS, ParsePacket<ReqLeaveGameToS>);
        _handler.Add((ushort)PacketId.ReqLeaveGameToS, PacketHandler.ReqLeaveGameToSHandler);
        _parseHandler.Add((ushort)PacketId.MoveToS, ParsePacket<MoveToS>);
        _handler.Add((ushort)PacketId.MoveToS, PacketHandler.MoveToSHandler);
        _parseHandler.Add((ushort)PacketId.PingCheckToS, ParsePacket<PingCheckToS>);
        _handler.Add((ushort)PacketId.PingCheckToS, PacketHandler.PingCheckToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqUseSkillToS, ParsePacket<ReqUseSkillToS>);
        _handler.Add((ushort)PacketId.ReqUseSkillToS, PacketHandler.ReqUseSkillToSHandler);
        _parseHandler.Add((ushort)PacketId.PickupDropItemToS, ParsePacket<PickupDropItemToS>);
        _handler.Add((ushort)PacketId.PickupDropItemToS, PacketHandler.PickupDropItemToSHandler);
        _parseHandler.Add((ushort)PacketId.UseItemToS, ParsePacket<UseItemToS>);
        _handler.Add((ushort)PacketId.UseItemToS, PacketHandler.UseItemToSHandler);
        _parseHandler.Add((ushort)PacketId.EquipItemToS, ParsePacket<EquipItemToS>);
        _handler.Add((ushort)PacketId.EquipItemToS, PacketHandler.EquipItemToSHandler);
        _parseHandler.Add((ushort)PacketId.UnEquipItemToS, ParsePacket<UnEquipItemToS>);
        _handler.Add((ushort)PacketId.UnEquipItemToS, PacketHandler.UnEquipItemToSHandler);
        _parseHandler.Add((ushort)PacketId.CreatePartyToS, ParsePacket<CreatePartyToS>);
        _handler.Add((ushort)PacketId.CreatePartyToS, PacketHandler.CreatePartyToSHandler);
    
    }

    public void ReceivePacket(PacketSession session, ArraySegment<byte> segment)
    {
        ushort count = 0;
        ushort packetSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
        count += 2;
        ushort packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
        if (_parseHandler.TryGetValue(packetId, out action) == true)
        {
            action.Invoke(session, segment, packetId);
        }

    }

    private void ParsePacket<T>(PacketSession session, ArraySegment<byte> segment, ushort id) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(segment.Array, segment.Offset + 4, segment.Count - 4);

        Action<PacketSession, IMessage> action = null;
        if (ClientHandler != null)
        {
            ClientHandler.Invoke(id, packet);
        }
        else if (_handler.TryGetValue(id, out action) == true)
        {
            action.Invoke(session, packet);
        }
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;

        if (_handler.TryGetValue(id, out action) == true)
        {
            return action;
        }

        return null;
    }
}
