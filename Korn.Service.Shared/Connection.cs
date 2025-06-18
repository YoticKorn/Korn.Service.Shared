using Korn.Pipes;
using System;

namespace Korn.Service
{
    public abstract class Connection : IDisposable
    {
        public Connection(ConnectionID identifier) => Indentifier = identifier;

        public ConnectionID Indentifier { get; protected set; }
        public InputPipe InputPipe { get; protected set; }
        public OutputPipe OutputPipe { get; protected set; }
        public bool IsConnected { get; protected set; }
        public DateTime LastConnectTime { get; protected set; }
        public bool WasConnected { get; protected set; }

        protected void OnPipeConnected()
        {
            IsConnected = InputPipe != null && OutputPipe != null && InputPipe.IsConnected && OutputPipe.IsConnected;
            
            if (IsConnected)
            {
                LastConnectTime = DateTime.Now;
                WasConnected = true;
            }
        }

        protected void OnPipeDisconnected() => IsConnected = false;

        public void Send(byte[] bytes) => InputPipe.Send(bytes);

        public void Dispose()
        {
            InputPipe.Dispose();
            OutputPipe.Dispose();
        }
    }
}