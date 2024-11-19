using SuperServer.Data;
using SuperServer.Game.Object;
using SuperServer.Game.Room;
using SuperServer.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SuperServer.Game.Map
{
    public class MapComponent
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinZ { get; set; }
        public int MaxZ { get; set; }
        float[,] _mapCollision;

        public void LoadMap(string mapName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open($"{ConfigManager.Config.dataPath}/Map/{mapName}Data.txt", FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                MinX = reader.ReadInt32();
                MaxX = reader.ReadInt32();
                MinZ = reader.ReadInt32();
                MaxZ = reader.ReadInt32();

                int zCount = MaxZ - MinZ + 1;
                int xCount = MaxX - MinX + 1;
                _mapCollision = new float[zCount, xCount];
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    int x = reader.ReadInt32();
                    int z = reader.ReadInt32();

                    int applyX = x - MinX;
                    int applyZ = z - MinZ;

                    float height = reader.ReadSingle();
                    _mapCollision[applyZ, applyX] = height;
                }
            }
        }
        public bool CanGo(float z, float x)
        {
            //해당 포인트가 (0.9xx, 0.1xxx)라면 (1, 0)을 좌표를 검사
            int roundZ = (int)MathF.Round(z);
            int roundX = (int)MathF.Round(x);
            
            if (roundZ < MinZ || roundZ > MaxZ)
                return false;
            if (roundX < MinX || roundX > MaxX)
                return false;

            int applyZ = roundZ - MinZ;
            int applyX = roundX - MinX;
            if (_mapCollision[applyZ, applyX] != 0)
                return false;

            return true;
        }
        public void ApplyMove(BaseObject obj, Vector3Int destPos)
        {
            obj.PosInfo.PosX = destPos.X;
            obj.PosInfo.PosY = destPos.Y;
            obj.PosInfo.PosZ = destPos.Z;
        }

        public struct PQNode : IComparable<PQNode>
        {
            public float H;
            public float G;
            public Vector3Int Pos;
            public int Depth;
            public int CompareTo(PQNode other)
            {
                if (H + G == other.H + other.G)
                    return 0;
                return (H + G) < (other.H + other.G) ? -1 : 1;
            }
        }

        List<Vector3Int> _dir = new List<Vector3Int>()
        {
            new Vector3Int(0, 0, 1),
            new Vector3Int(1, 0, 1),
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 0, -1),
            new Vector3Int(-1, 0, -1),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(-1, 0, 1),
        };

        List<float> _cost = new List<float>()
        {
            1,
            1.4f,
            1,
            1.4f,
            1,
            1.4f,
            1,
            1.4f,
        };

        public List<Vector3Int> FindPath(Vector3Int startPos, Vector3Int destPos, int maxDepth = 10)
        {
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();
            Dictionary<Vector3Int, float> bestCostPath = new Dictionary<Vector3Int, float>();
            Dictionary<Vector3Int, PQNode> chasePath = new Dictionary<Vector3Int, PQNode>();
            Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();

            float startG = 0.0f;
            float startH = (destPos - startPos).MagnitudeSqr();

            PQNode startNode = new PQNode()
            {
                G = startG,
                H = startH,
                Pos = startPos,
                Depth = 1
            };
            pq.Push(startNode);
            chasePath[startNode.Pos] = startNode;
            bestCostPath[startPos] = startNode.G + startNode.H;

            Vector3Int closestPos = startPos;
            float closestF = startNode.G + startNode.H;

            while (pq.Count > 0)
            {
                PQNode node = pq.Pop();

                //해당 노드의 포스정보와 목적지의 거리가 거의 근접했다면 도착했다고 가정하기
                if ((destPos - node.Pos).MagnitudeSqr() <= 0.1f)
                    break;
                if (node.Depth >= maxDepth)
                    break;
                if (visited.ContainsKey(node.Pos) && visited[node.Pos])
                    continue;
                if (bestCostPath[node.Pos] < node.G + node.H)
                    continue;
                visited[node.Pos] = true;


                for (int i = 0; i < _dir.Count; i++)
                {
                    Vector3Int nextPos = node.Pos + _dir[i];

                    //갈 수 있는지 & 해당 경로(nextPos)가 처음이면 엄청 큰 값과 비교
                    if (CanGo(nextPos.Z, nextPos.X) == false) continue;
                    if (bestCostPath.ContainsKey(nextPos) == false)
                        bestCostPath[nextPos] = int.MaxValue;

                    float g = node.G + _cost[i];
                    float h = (destPos - nextPos).MagnitudeSqr();

                    if (bestCostPath[nextPos] <= g + h)
                        continue;

                    bestCostPath[nextPos] = g + h;
                    PQNode newNode = new PQNode()
                    {
                        G = g,
                        H = h,
                        Pos = nextPos,
                        Depth = node.Depth + 1
                    };
                    pq.Push(newNode);
                    chasePath[nextPos] = node;
                    closestPos = nextPos;
                }
            }
            if (chasePath.ContainsKey(destPos) == false)
                return CalcPathFromChasePath(chasePath, closestPos);
            return CalcPathFromChasePath(chasePath, destPos);
        }


        List<Vector3Int> CalcPathFromChasePath(Dictionary<Vector3Int, PQNode> chasePath, Vector3Int dest)
        {
            List<Vector3Int> path = new List<Vector3Int>();

            if (chasePath.ContainsKey(dest) == false)
                return path;

            Vector3Int now = dest;

            while (chasePath[now].Pos != now)
            {
                path.Add(now);
                now = chasePath[now].Pos;
            }

            path.Add(now);
            path.Reverse();

            return path;
        }

    }
}
