
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
  SpawnToC = 12,
  ReqLeaveGameToS = 13,
  MoveToS = 14,
  MoveToC = 15,
  PingCheckToC = 16,
  PingCheckToS = 17,
  DeSpawnToC = 18,
  ReqUseSkillToS = 19,
  ResUseSkillToC = 20,
  ModifyStatToC = 21,
  ModifyOneStatToC = 22,
  DieToC = 23,
  TeleportToC = 24,
  RewardToC = 25,

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
        _parseHandler.Add((ushort)PacketId.ReqLeaveGameToS, ParsePacket<ReqLeaveGameToS>);
        _handler.Add((ushort)PacketId.ReqLeaveGameToS, PacketHandler.ReqLeaveGameToSHandler);
        _parseHandler.Add((ushort)PacketId.MoveToS, ParsePacket<MoveToS>);
        _handler.Add((ushort)PacketId.MoveToS, PacketHandler.MoveToSHandler);
        _parseHandler.Add((ushort)PacketId.PingCheckToS, ParsePacket<PingCheckToS>);
        _handler.Add((ushort)PacketId.PingCheckToS, PacketHandler.PingCheckToSHandler);
        _parseHandler.Add((ushort)PacketId.ReqUseSkillToS, ParsePacket<ReqUseSkillToS>);
        _handler.Add((ushort)PacketId.ReqUseSkillToS, PacketHandler.ReqUseSkillToSHandler);
    
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
