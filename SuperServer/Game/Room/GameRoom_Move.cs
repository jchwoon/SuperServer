using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Commander;
using SuperServer.Game.Object;
using SuperServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public partial class GameRoom
    {
        private MoveToC _movePacket = new MoveToC() { PosInfo = new PosInfo() };
        readonly float _threshold = 2.0f;
        bool _isExpected = false;
        public void HandleMove(Hero hero, MoveToS packet)
        {
            if (hero == null)
                return;

            bool canGoCurrent = Map.CanGo(packet.PosInfo.PosX, packet.PosInfo.PosZ);

            if (canGoCurrent == false)
            {
                return;
            }

            //예상위치에 대한 판별
            Vector3 expectPos = GetExpectPos(packet.PosInfo, hero);
            bool canGoExpected = Map.CanGo(expectPos.X, expectPos.Z);

            if (canGoExpected == false)
            {
                return;
            }
 
            hero.HeroInfo.ObjectInfo.PosInfo = packet.PosInfo;
            _movePacket.ObjectId = hero.ObjectId;
            //예상한 좌표를 보냈었다면 진짜 위치 정보와 비교 과정을 거치고 그렇지 않으면 예측을한다
            if (_isExpected == false)
                ExpectSend(expectPos, packet.PosInfo.Speed);
            else
                CompareExpectToRealAndSend(expectPos, packet.PosInfo);
        }
        private Vector3 GetExpectPos(PosInfo info, Hero hero)
        {
            float dist = info.Speed * ((hero.Session.Ping / 1000) + 0.5f);

            Vector3 dir = GetLookDir(info.RotY);

            float expectX = info.PosX + (dir.X * dist);
            float expectZ = info.PosZ + (dir.Z * dist);

            return new Vector3((int)expectX, 0, (int)expectZ);
        }

        //단위 원 생각
        private Vector3 GetLookDir(float rotY)
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
            _movePacket.PosInfo.PosX = expect.X;
            _movePacket.PosInfo.PosY = expect.Y;
            _movePacket.PosInfo.PosZ = expect.Z;
            _movePacket.PosInfo.Speed = speed;
            MoveSend();
            _isExpected = true;
        }

        private void RealSend(PosInfo real)
        {
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
                Broadcast(_movePacket);
            });
        }
    }
}
