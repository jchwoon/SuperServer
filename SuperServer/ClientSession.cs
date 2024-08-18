using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer
{
    public class ClientSession : PacketSession
    {
        public override void OnConnected()
        {
        }

        public override void OnDisconnected()
        {
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override void OnSend()
        {

        }
    }
}
