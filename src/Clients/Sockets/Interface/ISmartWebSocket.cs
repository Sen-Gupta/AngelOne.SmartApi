using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Sockets.Interface
{
    public interface ISmartWebSocket
    {
        event Connected OnConnect;
        event Closed OnClose;
        event DataReceived OnDataReceived;
        event Error OnError;
        Task ConnectAsync(string Url, Dictionary<string, string> headers = null!);
        Task SendAsync(string Message);
        Task ReceiveAsync();
        bool IsSocketOpen();
        Task CloseSocket();
        Task Heartbeat(string Message);
    }

    public delegate void Connected();
    public delegate void Closed();
    public delegate void Error(string Message);
    public delegate void DataReceived(byte[] Data, bool EndOfMessage, string MessageType);
}
