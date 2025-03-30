using Korn.Pipes;
using System;

namespace Korn.Service
{
    public class ServerConfiguration
    {
        public ServerConfiguration(string name, PacketList clientPackets, PacketList serverPackets)
        {
            Name = name;
            ClientPackets = clientPackets;
            ServerPackets = serverPackets;

            PipeConfiguration = new PipeConfiguration(name);

            VerifyPackets();
        }

        public readonly string Name;
        public readonly PipeConfiguration PipeConfiguration;
        public readonly PacketList ClientPackets;
        public readonly PacketList ServerPackets;

        public PipeConfiguration ConnectConfiguration => new PipeConfiguration($"{Name}-connect");

        void VerifyPackets()
        {
            VerifyClientPackets();
            VerifyServerPackets();

            void VerifyClientPackets()
            {
                foreach (var type in ClientPackets.Types)
                    if (!typeof(ClientPacket).IsAssignableFrom(type))
                        throw new InvalidOperationException($"Client side packet type error during ServerConfiguration initialization");
            }

            void VerifyServerPackets()
            {
                foreach (var type in ServerPackets.Types)
                    if (!typeof(ServerPacket).IsAssignableFrom(type))
                        throw new InvalidOperationException($"Server side packet type error during ServerConfiguration initialization");
            }
        }

        public Type GetClientPacketTypeByID(uint id)
        {
            if (id >= ClientPackets.Count)
                throw new InvalidOperationException("Attempting to get a client packet with a non-existent id");

            return ClientPackets[(int)id];
        }

        public Type GetServerPacketTypeByID(uint id)
        {
            if (id >= ServerPackets.Count)
                throw new InvalidOperationException("Attempting to get a server packet with a non-existent id");

            return ServerPackets[(int)id];
        }

        public uint GetClientPacketID(Type type)
        {
            var index = ClientPackets.Types.IndexOf(type);
            if (index == -1)
                throw new InvalidOperationException("Attempting to get a client packet id with a non-existent packet type");

            return (uint)index;
        }

        public uint GetClientPacketID(Packet/*ClientPacket*/ packet) => GetClientPacketID(packet.GetType());

        public uint GetServerPacketID(Type type)
        {
            var index = ServerPackets.Types.IndexOf(type);
            if (index == -1)
                throw new InvalidOperationException("Attempting to get a server packet id with a non-existent packet type");

            return (uint)index;
        }

        public uint GetServerPacketID(Packet/*ServerPacket*/ packet) => GetServerPacketID(packet.GetType());
    }
}