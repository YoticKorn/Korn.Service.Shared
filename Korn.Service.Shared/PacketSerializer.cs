using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Korn.Service
{
    public static class PacketSerializer
    {
        static JsonSerializer jsonSerializer = new JsonSerializer();

        public static bool DeserializeClientPacket(ServerConfiguration configuration, byte[] bytes, out ClientPacket clientPacket, out uint packetID)
        {
            var result = Deserialize(configuration.GetClientPacketTypeByID, bytes, out var packet, out packetID);
            clientPacket = packet as ClientPacket;
            return result;
        }

        public static bool DeserializeServerPacket(ServerConfiguration configuration, byte[] bytes, out ServerPacket serverPacket, out uint packetID)
        {
            var result = Deserialize(configuration.GetServerPacketTypeByID, bytes, out var packet, out packetID);
            serverPacket = packet as ServerPacket;
            return result;
        }

        static bool Deserialize(Func<uint, Type> packetGetter, byte[] bytes, out Packet packet, out uint packetID)
        {
            if (bytes.Length < 4)
                throw new Exception("umm, why bytes array length is less than 4?");

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    packetID = binaryReader.ReadUInt32();
                    var packetType = packetGetter(packetID);

                    using (var bsonReader = new BsonReader(binaryReader))
                    {
                        packet = jsonSerializer.Deserialize(bsonReader, packetType) as Packet;
                        if (packet == null)
                            return false;

                        return true;
                    }
                }
            }
        }

        public static byte[] SerializeClientPacket(ServerConfiguration configuration, ClientPacket packet) => Serialize(configuration.GetClientPacketID, packet);

        public static byte[] SerializeServerPacket(ServerConfiguration configuration, ServerPacket packet) => Serialize(configuration.GetServerPacketID, packet);

        static byte[] Serialize(Func<Packet, uint> idGetter, Packet packet)
        {            
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    var packetID = idGetter(packet);
                    binaryWriter.Write(packetID);

                    using (var bsonWriter = new BsonWriter(binaryWriter))
                    {
                        jsonSerializer.Serialize(bsonWriter, packet);
                        var bytes = memoryStream.ToArray();
                        return bytes;
                    }
                }
            }
        }
    }
}