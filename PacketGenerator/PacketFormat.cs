﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    public class PacketFormat
    {
        // {0} 패킷 IDs
        // {1} 사전 등록 handler
        public static string packetManager = @"
using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;

public enum PacketId
{{
{0}
}}


class PacketManager
{{
    private static PacketManager _instance;

    public static PacketManager Instance 
    {{ 
        get 
        {{
            if (_instance == null)
            {{
                _instance = new PacketManager();
            }}
            return _instance;
        }} 
    }}

    //들어온 패킷 파싱
    Dictionary<ushort, Action<ArraySegment<byte>, ushort>> _parseHandler = new();
    //파싱된 패킷 핸들
    Dictionary<ushort, Action<IMessage>> _handler = new();

    PacketManager()
    {{
        PreRaiseHandler();
    }}

    private void PreRaiseHandler()
    {{
        {1}
    }}

    public void ReceivePacket(ArraySegment<byte> segment)
    {{
        ushort count = 0;
        ushort packetSize = BitConverter.ToUInt16(segment.Array, segment.Offset);
        count += 2;
        ushort packetId = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
        count += 2;

        Action<ArraySegment<byte>, ushort> action = null;
        if (_parseHandler.TryGetValue(packetId, out action) == true)
        {{
            action.Invoke(segment, packetId);
        }}

    }}

    private void ParsePacket<T>(ArraySegment<byte> segment, ushort id) where T : IMessage, new()
    {{
        T packet = new T();
        packet.MergeFrom(segment.Array, segment.Offset + 4, segment.Count - 4);

        Action<IMessage> action = null;
        if (_handler.TryGetValue(id, out action) == true)
        {{
            action.Invoke(packet);
        }}
    }}
}}
";

        // {0} Packet Name
        // {1} Packet Num
        public static string managerPacketIds = @"  {0} = {1},
";
        // {0} Packet Name
        public static string managerPreHandlers = @"_parseHandler.Add((ushort)PacketId.{0}, ParsePacket<{0}>);
        _handler.Add((ushort)PacketId.{0}, PacketHandler.{0}Handler);
";
    }
}