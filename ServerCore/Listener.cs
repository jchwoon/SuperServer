using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    //https://learn.microsoft.com/ko-kr/dotnet/api/system.net.sockets.socketasynceventargs?view=net-8.0
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _session;

        public void Open(IPEndPoint endPoint, Func<Session> session, int backlog = 100)
        {
            //SocketType.Stream
            //데이터 중복이나 경계 유지 없이 신뢰성 있는 양방향 연결 기반의 바이트 스트림을 지원합니다.
            //이 종류의 Socket은 단일 피어와 통신하며 이 소켓을 사용할 경우 !!!!!통신을 시작하기 전에 원격 호스트에 연결!!!!해야 합니다
            _session += session;
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _listenSocket.Bind(endPoint);

            _listenSocket.Listen(backlog);

            SocketAsyncEventArgs acceptEventArg = new SocketAsyncEventArgs();
            acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(CompletedAccept);
            StartAccept(acceptEventArg);
        }


        public void StartAccept(SocketAsyncEventArgs args)
        {
            // loop while the method completes synchronously
            args.AcceptSocket = null;
            bool willRaiseEvent = _listenSocket.AcceptAsync(args);
            if (willRaiseEvent == false)
            {
                CompletedAccept(null, args);
            }
        }

        void CompletedAccept(object sender, SocketAsyncEventArgs args)
        {
            // Accept the next connection request
            if (args.SocketError == SocketError.Success)
            {
                Session session = _session.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected();
            }

            StartAccept(args);
        }
    }
}