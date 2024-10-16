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
using System.Net.NetworkInformation;
using SuperServer.Game.Object;

namespace SuperServer.Session
{
    public partial class ClientSession : PacketSession
    {
        public int AccountId { get; set; }
        public int SessionId { get; set; }
        public long Ping { get; set; }
        long _sendTime;

        public void PingCheck()
        {
            //한번이라도 주고받았다면
            if (Ping > 0)
            {
                long elapsed = (System.Environment.TickCount64 - (Ping + _sendTime));
                Console.WriteLine(elapsed);
                if (elapsed > 10 * 1000)
                {
                    Console.WriteLine("Ping Disconnect");
                    CloseClientSocket();
                    return;
                }
            }


            PingCheckToC pingPacket = new PingCheckToC();
            _sendTime = System.Environment.TickCount64;
            Send(pingPacket);

            GameCommander.Instance.PushAfter(3000, PingCheck);
        }

        public void HandlePing()
        {
            Ping = System.Environment.TickCount64 - _sendTime;
        }

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
            ConnectToC connectPacket = new ConnectToC();
            Send(connectPacket);

            GameCommander.Instance.PushAfter(3000, PingCheck);

        }

        public override void OnDisconnected()
        {
            GameCommander.Instance.Push(() =>
            {
                if (PlayingHero == null)
                    return;

                if (PlayingHero.Room == null)
                    return;

                GameCommander.Instance.Push(PlayingHero.Room.ExitRoom<Hero>, PlayingHero);
            });
            SessionManager.Instance.Remove(this);
            Console.WriteLine("Disconnected");
        }

        public override void OnRecvPacket(ArraySegment<byte> segment)
        {
            PacketManager.Instance.ReceivePacket(this, segment);
        }

        public override void OnSend()
        {
            //Console.WriteLine("Sended");
        }
    }
}
