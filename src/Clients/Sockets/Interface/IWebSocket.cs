using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Sockets.Interface
{
    public interface IWebSocketV2
    {
        event OnConnectHandler OnConnect;
        event OnCloseHandler OnClose;
        event OnDataHandler OnData;
        event OnErrorHandler OnError;
        void Connect(string Url, Dictionary<string, string> headers = null);
        Task SendAsync(string Message);
        Task ReceiveAsync();
        bool IsSocketOpen();
        void CloseSocket();
        void Heartbeat(string Message);
    }

    public delegate void OnConnectHandler();
    public delegate void OnCloseHandler();
    public delegate void OnErrorHandler(string Message);
    public delegate void OnDataHandler(byte[] Data, bool EndOfMessage, string MessageType);
}
