namespace Korn.Service
{
    public abstract class Packet { }

    public class ClientPacket : Packet { } // packet from client

    public class ClientCallbackablePacket : ClientPacket
    {
        public int CallbackID;
    }

    public class ClientCallbackablePacket<T> : ClientCallbackablePacket where T : ServerCallbackPacket { }

    public class ClientCallbackPacket : ClientPacket
    {
        public int CallbackID;

        public ClientCallbackPacket ApplyCallback(ServerCallbackablePacket packet)
        {
            CallbackID = packet.CallbackID;
            return this;
        }
    }

    public class ServerPacket : Packet { } // packet from server

    public abstract class ServerCallbackablePacket : ServerPacket
    {
        public int CallbackID;
    }

    public class ServerCallbackablePacket<T> : ServerCallbackablePacket where T : ClientCallbackPacket { }

    public class ServerCallbackPacket : ServerPacket
    {
        public int CallbackID;

        public ServerCallbackPacket ApplyCallback(ClientCallbackablePacket packet)
        {
            CallbackID = packet.CallbackID;
            return this;
        }
    }
}