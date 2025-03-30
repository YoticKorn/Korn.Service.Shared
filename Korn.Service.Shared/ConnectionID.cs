using Korn.Pipes;
using System;

namespace Korn.Service
{
    public class ConnectionID
    {
        static Random random = new Random();

        public ConnectionID(ServerConfiguration configuration) : this(configuration, random.NextUInt64()) { } 
        public ConnectionID(ServerConfiguration configuration, ulong id) => (Configuration, Indentitifer) = (configuration, id);

        public readonly ServerConfiguration Configuration;
        public readonly ulong Indentitifer;

        public PipeConfiguration InServerConfiguration => new PipeConfiguration($"{Configuration.Name}-connection-{Indentitifer}-server");
        public PipeConfiguration InClientConfiguration => new PipeConfiguration($"{Configuration.Name}-connection-{Indentitifer}-client");
    }
}