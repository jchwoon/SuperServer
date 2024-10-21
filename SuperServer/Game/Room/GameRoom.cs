using Google.Protobuf;
using Google.Protobuf.Protocol;
using SuperServer.Game.Object;
using SuperServer.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SuperServer.Data;
using Google.Protobuf.Enum;
using SuperServer.Utils;
using System.Runtime.ConstrainedExecution;

namespace SuperServer.Game.Room
{
    public partial class GameRoom
    {
        Dictionary<int, Hero> _heroes = new Dictionary<int, Hero>();
        Dictionary<int, Monster> _monsters = new Dictionary<int, Monster>();
        public MapComponent Map { get; set; } = new MapComponent();
        public SpawningPool SpawningPool { get; private set; } = new SpawningPool();
        public RoomData RoomData { get; private set; }
        public int RoomId { get; set; }
        //거리 25
        public const float SqrInterestRange = 625;

        public GameRoom(int roomId)
        {
            RoomId = roomId;
        }

        public void Init()
        {
            RoomData data;
            if (DataManager.RoomDict.TryGetValue(RoomId, out data) == true)
            {
                Map.LoadMap(data.Name);
                RoomData = data;
            }

            SpawningPool.Init(this);
        }

        public void EnterRoom<T>(BaseObject obj) where T : BaseObject
        {
            if (obj == null)
                return;
            EObjectType type = obj.ObjectType;
            obj.Room = this;
            //히어로
            if (type == EObjectType.Hero)
            {
                Hero hero = (Hero)obj;
                _heroes.Add(hero.ObjectId, hero);

                ResEnterRoomToC resEnterPacket = new ResEnterRoomToC();
                resEnterPacket.MyHero = hero.MyHeroInfo;
                hero.Session.Send(resEnterPacket);
                hero.InterestRegion?.Update();
            }
            // 몬스터
            else if (type == EObjectType.Monster)
            {
                Monster monster = (Monster)obj;
                _monsters.Add(monster.ObjectId, monster);
                monster.Update();
            }
        }

        public void ExitRoom<T>(BaseObject obj) where T : BaseObject 
        {
            if (obj == null)
                return; 

            if (typeof(T) == typeof(Hero))
            {
                Hero hero = (Hero)obj;
                _heroes.Remove(hero.ObjectId);
                hero.InterestRegion.Clear();
                hero.Room = null;
            }
            else if (typeof(T) == typeof(Monster))
            {
                Monster monster = (Monster)obj;
                _monsters.Remove(monster.ObjectId);
                monster.Room = null;
            }

            DeSpawnToC deSpawnPacket = new DeSpawnToC();
            deSpawnPacket.ObjectIds.Add(obj.ObjectId);
            Broadcast(deSpawnPacket, obj.Position);
        }

        public void ReSpawn(Creature obj)
        {
            EObjectType type = obj.ObjectType;
            if (type == EObjectType.Hero)
            {
                obj.PosInfo.PosX = RoomData.StartPosX;
                obj.PosInfo.PosY = RoomData.StartPosY;
                obj.PosInfo.PosZ = RoomData.StartPosZ;

                TeleportToC telpoPacket = new TeleportToC();
                telpoPacket.PosInfo = obj.PosInfo;
                telpoPacket.ObjectId = obj.ObjectId;
                telpoPacket.TelpoType = ETeleportType.Respawn;

                Broadcast(telpoPacket, obj.Position);
            }
            else if (type == EObjectType.Monster)
            {
                Monster monster = obj as Monster;
                SpawningPool.ReSpawn(monster, monster.PoolData);
            }

            obj.ReSpawn();
        }

        //모두 보내기
        public void Broadcast(IMessage packet, Vector3 pos)
        {
            foreach (Hero hero in _heroes.Values)
            {
                float dist = (pos - hero.Position).MagnitudeSqr();
                if (dist < SqrInterestRange)
                    hero.Session.Send(packet);
            }
        }
        //단일 대상 제외
        public void Broadcast(IMessage packet, Hero excludeHero, Vector3 pos)
        {
            foreach (Hero hero in _heroes.Values)
            {
                if (hero.DbHeroId == excludeHero.DbHeroId)
                    continue;
                float dist = (pos - hero.Position).MagnitudeSqr();
                if (dist < SqrInterestRange)
                    hero.Session.Send(packet);
            }
        }

        public Creature FindCreatureById(int id)
        {
            Creature creature;
            creature = FindHeroById(id);
            if (creature != null)
                return creature;

            creature = FindMonsterById(id);
            if (creature != null) 
                return creature;

            return null;
        }

        public Hero FindHeroById(int id)
        {
            Hero hero;
            if (_heroes.TryGetValue(id, out hero) == false)
                return null;
            return hero;
        }
        public Monster FindMonsterById(int id)
        {
            Monster monster;
            if (_monsters.TryGetValue(id, out monster) == false)
                return null;
            return monster;
        }

        public List<Creature> GetCreatures()
        {
            List<Creature> objects = new List<Creature>();

            foreach(Hero hero in _heroes.Values)
            {
                objects.Add(hero);
            }

            foreach(Monster monster in _monsters.Values)
            {
                objects.Add(monster);
            }

            return objects;
        }
    }
}
