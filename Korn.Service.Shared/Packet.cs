namespace Korn.Service
{
    public abstract class Packet { }

    public class ClientPacket : Packet { } // packet from client    

    public class ClientPacketWithCallback<T> : ClientPacket where T : ServerCallbackPacket { }
    
    public class ClientCallbackPacket : ServerPacket { }

    public class ServerPacket : Packet { } // packet from server

    public class ServerPacketWithCallback<T> : ServerPacket where T : ClientCallbackPacket { }

    public class ServerCallbackPacket : ClientPacket { }
}