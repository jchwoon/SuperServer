using SuperServer.Data;
using SuperServer.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Game.Map
{
    public class MapComponent
    {
        Dictionary<(int, int), float> _mapCollision = new Dictionary<(int, int), float>();
        public void LoadMap(string mapName)
        {
            string[] lines = File.ReadAllLines($"{ConfigManager.Config.dataPath}/Map/{mapName}Data.txt");

            foreach (string line in lines)
            {
                string[] pos = line.Split(',');

                int posX = int.Parse(pos[0]);
                int posZ = int.Parse(pos[1]);
                float posY = float.Parse(pos[2]);

                _mapCollision.Add((posX, posZ), posY);
            }
        }

        public bool CanGo(float x, float z)
        {
            float value = 0.0f;
            //해당 포인트가 (0.9xx, 0.1xxx)라면 (1, 0)을 좌표를 검사
            int roundX = (int)MathF.Round(x);
            int roundZ = (int)MathF.Round(z);
            if (_mapCollision.TryGetValue((roundX, roundZ), out value) == false)
                return false;

            if (value == -1)
                return false;

            return true;
        }
    }
}
