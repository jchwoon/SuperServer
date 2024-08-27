using Google.Protobuf;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Protocol;
using SuperServer.Commander;

namespace SuperServer.Session
{
    public partial class ClientSession : PacketSession
    {
        public void Send(IMessage packet)
        {
            Send(MakePacketToBuffer(packet));
        }
        private ArraySegment<byte> MakePacketToBuffer(IMessage packet)
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
            GameCommander.Instance.Push(() =>
            {
                ConnectToC connectPacket = new ConnectToC();
                Send(connectPacket);
            });
        }

        public override void OnDisconnected()
        {
        }

        public override void OnRecvPacket(ArraySegment<byte> segment)
        {
            PacketManager.Instance.ReceivePacket(this, segment);
        }

        public override void OnSend()
        {
            Console.WriteLine("Sended");
        }
    }
}
