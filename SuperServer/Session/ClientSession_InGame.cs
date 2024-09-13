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

namespace SuperServer.Session
{
    public partial class ClientSession : PacketSession
    {
        private MoveToC _movePacket = new MoveToC() { PosInfo = new PosInfo() };
        readonly float _threshold = 2.0f;
        bool _isExpected = false;

        public void HandleMove(MoveToS packet)
        {
            Vector3 expectPos = GetExpectPos(packet.PosInfo);

            //예상한 좌표를 보냈었다면 진짜 위치 정보와 비교 과정을 거치고 그렇지 않으면 예측을한다
            if (_isExpected == false)
                ExpectSend(expectPos, packet.PosInfo.Speed);
            else
                CompareExpectToRealAndSend(expectPos, packet.PosInfo);
        }
        public Vector3 GetExpectPos(PosInfo info)
        {
            float dist = info.Speed * ((Ping / 1000) + 0.5f);

            Vector3 dir = GetLookDir(info.RotY);

            float expectX = info.PosX + (dir.X * dist);
            float expectZ = info.PosZ + (dir.Z * dist);

            return new Vector3(expectX, 0, expectZ);
        }

        //단위 원 생각
        public Vector3 GetLookDir(float rotY)
        {
            float radians = rotY * (float)(Math.PI / 180.0);

            float directionX = (float)Math.Sin(radians);
            float directionZ = (float)Math.Cos(radians);

            Vector3 direction = new Vector3(directionX, 0, directionZ);

            return direction.Normalize();
        }

        private void CompareExpectToRealAndSend(Vector3 expect, PosInfo real)
        {
            Vector3 realVec = new Vector3(real.PosX, real.PosY, real.PosZ);
            float dist = (realVec - expect).Magnitude();

            if (dist > _threshold)
                RealSend(real);
            else
                ExpectSend(expect, real.Speed);
        }

        private void ExpectSend(Vector3 expect, float speed)
        {
            Console.WriteLine("Expect");
            _movePacket.ObjectId = PlayingHero.ObjectId;
            _movePacket.PosInfo.PosX = expect.X;
            _movePacket.PosInfo.PosY = expect.Y;
            _movePacket.PosInfo.PosZ = expect.Z;
            _movePacket.PosInfo.Speed = speed;
            MoveSend();
            _isExpected = true;
        }

        private void RealSend(PosInfo real)
        {
            Console.WriteLine("real");
            _movePacket.ObjectId = PlayingHero.ObjectId;
            _movePacket.PosInfo.PosX = real.PosX;
            _movePacket.PosInfo.PosY = real.PosY;
            _movePacket.PosInfo.PosZ = real.PosZ;
            _movePacket.PosInfo.Speed = real.Speed;
            MoveSend();
            _isExpected = false;
        }

        private void MoveSend()
        {
            GameCommander.Instance.Push(() =>
            {
                PlayingHero.Room.Broadcast(_movePacket, PlayingHero);
            });
        }
    }
}
