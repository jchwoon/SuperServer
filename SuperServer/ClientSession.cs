﻿using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Protocol;

namespace SuperServer
{
    public class ClientSession : PacketSession
    {
        public ArraySegment<byte> Send(IMessage packet)
        {
            PacketId packetId = (PacketId)Enum.Parse(typeof(PacketId), packet.Descriptor.Name);
            ushort size = (ushort)packet.CalculateSize();
            ArraySegment<byte> sendBuffer = new ArraySegment<byte>(new byte[size + 4], 0, size + 4);
            //길이가 2인 바이트 배열의 0번째부터 sizeof(ushort)까지 sendBuffer에 복사
            ushort count = 0;
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer.Array, sendBuffer.Offset, sizeof(ushort));
            count += 2;
            Array.Copy(BitConverter.GetBytes((ushort)packetId), 0, sendBuffer.Array, sendBuffer.Offset + count, sizeof(ushort));
            count += 2;
            Array.Copy(packet.ToByteArray(), 0, sendBuffer.Array, sendBuffer.Offset + count, size);
            return sendBuffer;
        }
        public override void OnConnected()
        {
            S_Test test = new S_Test();
            test.Name = "Welcome To Super World";
            Send(Send(test));
        }

        public override void OnDisconnected()
        {
        }

        public override void OnRecvPacket(ArraySegment<byte> segment)
        {
            PacketManager.Instance.ReceivePacket(segment);
        }

        public override void OnSend()
        {
            Console.WriteLine("Sended");
        }
    }
}