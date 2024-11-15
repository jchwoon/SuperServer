using Google.Protobuf.Protocol;
using ServerCore;
using SuperServer.Commander;
using Google.Protobuf.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperServer.Utils;
using System.Net.Sockets;
using System.Diagnostics;
using SuperServer.Logic;
using SuperServer.Game.Room;

namespace SuperServer.Session
{
    public partial class ClientSession : PacketSession
    {
        public void HandleLeaveGame(ReqLeaveGameToS packet)
        {
            if (PlayingHero == null)
                return;
            if (PlayingHero.Room == null)
                return;

            Hero myHero = PlayingHero;
            GameRoom room = myHero.Room;
            GameCommander.Instance.Push(room.ExitRoom<Hero>, myHero);
            DBCommander.Instance.Push(DBLogic.Instance.SaveHero, myHero, room);
        }
    }
}
