using Google.Protobuf;
using Google.Protobuf.Protocol;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Room
{
    public class GameRoom
    {
        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();
        public int RoomId { get; set; }

        public GameRoom(int roomId)
        {
            RoomId = roomId;
        }

        public void EnterRoom<T>(BaseObject obj) where T : BaseObject
        {
            if (typeof(T) == typeof(Hero))
            {
                Hero hero = (Hero)obj;
                _heroes.Add(hero.ObjectId, hero);

                ResEnterRoomToC resEnterPacket = new ResEnterRoomToC();
                resEnterPacket.MyHero = hero.MyHeroInfo;
                hero.Session.Send(resEnterPacket);

                //신입생에게 기존 유저들을 알림
                {
                    SpawnToC spawnPacket = new SpawnToC();
                    foreach (Hero other in _heroes.Values)
                    {
                        spawnPacket.Hero.Add(other.HeroInfo);
                    }
                    hero.Session.Send(spawnPacket);
                }

                {
                    //신입생을 기존 유저들에게 알림
                    SpawnToC spawnPacket = new SpawnToC();
                    spawnPacket.Hero.Add(hero.HeroInfo);
                    Broadcast(spawnPacket);
                }

            }
        }

        private void Broadcast(IMessage packet)
        {
            foreach (Hero hero in _heroes.Values)
            {
                hero.Session.Send(packet);
            }
        }
    }
}
