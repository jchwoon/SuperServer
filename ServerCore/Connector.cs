using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        Socket _socket;
        Func<Session> _session;

        public void Connect(IPEndPoint endPoint, Func<Session> session)
        {
            _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _session = session;

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(CompleteConnect);
            StartConnect(args);
        }

        private void StartConnect(SocketAsyncEventArgs args)
        {
            bool willRaiseEvent = false;
            willRaiseEvent = _socket.ConnectAsync(args);

            if (willRaiseEvent == false)
            {
                CompleteConnect(null, args);
            }
        }

        private void CompleteConnect(object? sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = _session.Invoke();
                session.Start(_socket);
                session.OnConnected();
            }
            else
            {
                Console.WriteLine("Connect Failed");
                StartConnect(args);
            }
        }
    }
}
