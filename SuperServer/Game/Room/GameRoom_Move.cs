using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using SuperServer.Commander;
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
        readonly float _threshold = 2.0f;
        public void HandleMove(Hero hero, MoveToS packet)
        {
            if (hero == null)
                return;

            bool canGoCurrent = Map.CanGo(packet.PosInfo.PosZ, packet.PosInfo.PosX);

            if (canGoCurrent == false)
                return;

            //예상위치에 대한 판별
            Vector3 expectPos = GetExpectPos(packet.PosInfo, hero);
            bool canGoExpected = Map.CanGo(expectPos.Z, expectPos.X);

            if (canGoExpected == false)
                return;

            hero.PosInfo.MergeFrom(packet.PosInfo);
            CompareExpectToRealAndSend(hero, expectPos, packet.PosInfo);
        }
        private Vector3 GetExpectPos(PosInfo info, Hero hero)
        {
            
            float dist = hero.StatComponent.StatInfo.MoveSpeed * ((hero.Session.Ping / 1000) + 0.2f);

            Vector3 dir = GetLookDir(info.RotY);

            float expectX = info.PosX + (dir.X * dist);
            float expectZ = info.PosZ + (dir.Z * dist);

            return new Vector3(expectX, 0, expectZ);
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

        private void CompareExpectToRealAndSend(Hero hero, Vector3 expect, PosInfo real)
        {
            Vector3 realVec = new Vector3(real.PosX, real.PosY, real.PosZ);
            float dist = (realVec - expect).Magnitude();

            if (dist > _threshold)
            {
                Vector3 destPos = new Vector3(real.PosX, real.PosY, real.PosZ);
                hero.BroadcastMove(destPos);
            }
            else
                hero.BroadcastMove(expect);
        }
    }
}
