using AngelOne.SmartApi.Clients.Sockets.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace AngelOne.SmartApi.Clients.Sockets
{
    public class WebSocketV2: IWebSocketV2
    {
        #region WebSocketv2

        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private string _curl;
        private SemaphoreSlim _lockObject = new SemaphoreSlim(1, 1);


        public event OnConnectHandler OnConnect;
        public event OnCloseHandler OnClose;
        public event OnDataHandler OnData;
        public event OnErrorHandler OnError;

        public bool IsSocketOpen()
        {
            if (_clientWebSocket is null)
                return false;

            return _clientWebSocket.State == WebSocketState.Open;
        }

        public void Connect(string Url, Dictionary<string, string> headers = null)
        {
            _curl = Url;
            try
            {
                // Initialize ClientWebSocket instance and connect with Url
                if (headers != null)
                {
                    foreach (string key in headers.Keys)
                    {
                        _clientWebSocket.Options.SetRequestHeader(key, headers[key]);
                    }
                }

                _clientWebSocket.ConnectAsync(new Uri(_curl), CancellationToken.None).Wait();

                OnConnect?.Invoke();
            }
            catch (Exception e)
            {
                OnError?.Invoke("Error while receiving data. Message: " + e.Message);
            }
        }
        public async Task SendAsync(string Message)
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    await _clientWebSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(Message)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while sending data. Message: " + e.Message);
                }
            }
        }
        public async Task ReceiveAsync()
        {
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    var buffer = new byte[1024];
                    WebSocketReceiveResult result;

                    // Use a lock to synchronize access to the WebSocket instance
                    await _lockObject.WaitAsync();
                    try
                    {
                        result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }
                    finally
                    {
                        _lockObject.Release();
                    }

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                        OnClose?.Invoke();
                        break;
                    }
                    else
                    {
                        OnData?.Invoke(buffer, result.EndOfMessage, result.MessageType.ToString());
                    }
                }
                catch (WebSocketException wsEx) when (wsEx.InnerException is IOException || wsEx.InnerException is ObjectDisposedException)
                {
                    // Ignore the exceptions caused by network interruption or disposed WebSocket instance
                }
                catch (WebSocketException wsEx) when (wsEx.Message.Contains("Aborted"))
                {
                    // Ignore the exceptions caused by the WebSocket being in an aborted state
                }
                catch (Exception e) when (e.Message.Contains("Error while receiving data. Message: One or more errors occurred.") == false)
                {
                    OnError?.Invoke("Reconnecting...");
                }
            }
        }
        public void CloseSocket()
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).Wait();
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while closing connection. Message: " + e.Message);
                }
            }
        }
        public void Heartbeat(string Message)
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    SendAsync(Message).Wait();
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while sending heartbeat message. Message: " + e.Message);
                }
            }
        }

        #endregion
    }
}
