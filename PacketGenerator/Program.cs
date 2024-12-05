using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace PacketGenerator
{
    public class Program
    {
        static string packetEnums = "";

        static string clientPacketPreHandlers = "";
        static string serverPacketPreHandlers = "";

        static int protocolCount = 1;

        static void Main(string[] args)
        {
            // 먼저 enum과 handler를 만들어줌
            string rootDirPath = "../../../../";
            string protoPath = rootDirPath + "SuperServer/Packet/ProtoBuff/Schema/";

            foreach (string line in File.ReadAllLines(protoPath + "Protocol.proto"))
            {
                string[] names = line.Split(" ");
                if (names.Length == 0)
                    continue;

                if (names[0].StartsWith("message") == false)
                    continue;

                //PacketName
                ParsePacket(names[1]);
            }


            string clientManagerText = string.Format(PacketFormat.packetManager, packetEnums, clientPacketPreHandlers);
            File.WriteAllText(protoPath + "ClientPacketManager.cs", clientManagerText);

            string serverManagerText = string.Format(PacketFormat.packetManager, packetEnums, serverPacketPreHandlers);
            File.WriteAllText(protoPath + "ServerPacketManager.cs", serverManagerText);
        }

        static void ParsePacket(string name)
        {
            if (name.Last() == 'C')
            {
                clientPacketPreHandlers += string.Format(PacketFormat.managerPreHandlers, name, name);
                packetEnums += string.Format(PacketFormat.managerPacketIds, name, protocolCount);
                protocolCount++;
            }
            else if (name.Last() == 'S')
            {
                serverPacketPreHandlers += string.Format(PacketFormat.managerPreHandlers, name, name);
                packetEnums += string.Format(PacketFormat.managerPacketIds, name, protocolCount);

                protocolCount++;
            }
        }
    }
}