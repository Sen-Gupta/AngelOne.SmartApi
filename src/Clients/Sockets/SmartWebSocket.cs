using AngelOne.SmartApi.Clients.Sockets.Interface;

using System.Net.WebSockets;
using System.Text;

namespace AngelOne.SmartApi.Clients.Sockets
{
    public class SmartWebSocket: ISmartWebSocket
    {
        #region WebSocketv2

        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private SemaphoreSlim _lockObject = new SemaphoreSlim(1, 1);


        public event Connected OnConnect;
        public event Closed OnClose;
        public event DataReceived OnDataReceived;
        public event Error OnError;

        public bool IsSocketOpen()
        {
            if (_clientWebSocket is null)
                return false;

            return _clientWebSocket.State == WebSocketState.Open;
        }

        public async Task ConnectAsync(string Url, Dictionary<string, string> headers = null)
        {
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

                await _clientWebSocket.ConnectAsync(new Uri(Url), CancellationToken.None);

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
                        OnDataReceived?.Invoke(buffer, result.EndOfMessage, result.MessageType.ToString());
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
        public async Task CloseSocket()
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    
                    await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                catch (Exception e)
                {
                    OnError?.Invoke("Error while closing connection. Message: " + e.Message);
                }
            }
        }
        public async Task Heartbeat(string Message)
        {
            if (_clientWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    await SendAsync(Message);
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
