using AngelOne.SmartApi.Clients.Managers;
using AngelOne.SmartApi.Clients.Models;
using AngelOne.SmartApi.Clients.Models.Ticks;
using AngelOne.SmartApi.Clients.Requests;
using AngelOne.SmartApi.Clients.Settings;
using AngelOne.SmartApi.Clients.Sockets.Interface;

using Microsoft.Extensions.Configuration;

using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace AngelOne.SmartApi.Clients.Sockets
{
    public class TickerService
    {
        #region Private Variables
        System.Timers.Timer timer;
        private bool isTimerRunning = false;
        public Timer pingTimer;
        
        bool RESUBSCRIBE_FLAG = true;

        #endregion

        #region Events
        public delegate void OnConnected();
        public delegate void OnClosed();
        public delegate void OnTickReceived(Tick TickData);
        public delegate void OnTickLtpReceived(TickLtp TickData);
        public delegate void OnTickQuoteReceived(TickQuote TickData);
        public delegate void OnTickPongReceived(TickPong TickData);
        public delegate void OnErrored(string Message);
        
        public event OnConnected OnConnect;
        public event OnClosed OnClose;
        public event OnTickReceived OnTick;
        public event OnTickLtpReceived OnTickLtp;
        public event OnTickQuoteReceived OnTickQuote;
        public event OnTickPongReceived OnTickPong;
        public event OnErrored OnError;

        #endregion

        private readonly IConfiguration _configuration;
        private readonly SmartApiSettings _smartApiSettings;
        private readonly TokenManager _tokenManager;
        private readonly ISmartWebSocket _webSocketV2;
        private readonly AngelOneTokenClient _angelOneTokenClient;
        public TickerService(
            ISmartWebSocket webSocketV2,
            IConfiguration configuration,
            AngelOneTokenClient angelOneTokenClient,
            TokenManager tokenManager
            )
        {
            _angelOneTokenClient = angelOneTokenClient;
            _webSocketV2 = webSocketV2;
            _configuration = configuration;
            _tokenManager = tokenManager;
            _smartApiSettings = _configuration.GetSection("SmartApi").Get<SmartApiSettings>();
            _webSocketV2.OnConnect += ConnectedHandler;
            _webSocketV2.OnClose += CloseHandler;
            _webSocketV2.OnError += ErrorHandler;
            _webSocketV2.OnDataReceived += OnDataReceivedHandler;
        }

        #region Subscriptions
        public void Subscribe(SubscribeRequest subscribeRequest)
        {
            //Set Action as Subscribe if not set
            subscribeRequest.Action = Constants.Sockets.Actions.SUBSCRIBE_ACTION;
            try
            {
                //Todo: if mode == 4, then we can only have max 50 subscriptions
                var subscribeRequestJson = JsonSerializer.Serialize(subscribeRequest);
                SendAsync(subscribeRequestJson);
                RESUBSCRIBE_FLAG = true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred during subscribe: {e.Message}");
                throw;
            }
        }

        public void UnSubscribe(SubscribeRequest unSubscribeRequest)
        {
            //Set Action as UnSubscribe if not set
            unSubscribeRequest.Action = Constants.Sockets.Actions.UNSUBSCRIBE_ACTION;
            try
            {
                //Todo: if mode == 4, then we can only have max 50 subscriptions
                var subscribeRequestJson = JsonSerializer.Serialize(unSubscribeRequest);
                SendAsync(subscribeRequestJson);
                RESUBSCRIBE_FLAG = true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred during subscribe: {e.Message}");
                throw;
            }
        }

        public void ReSubscribe(SubscribeRequest reSubscribeRequest)
        {
            //Set Action as UnSubscribe if not set
            reSubscribeRequest.Action = Constants.Sockets.Actions.SUBSCRIBE_ACTION;
            try
            {
                //Todo: if mode == 4, then we can only have max 50 subscriptions
                var subscribeRequestJson = JsonSerializer.Serialize(reSubscribeRequest);
                SendAsync(subscribeRequestJson);
                RESUBSCRIBE_FLAG = true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error occurred during subscribe: {e.Message}");
                throw;
            }
        }



        #endregion

        #region Operations
        /// <summary>
        /// Start a WebSocket connection
        /// </summary>
        public async Task Connect()
        {
            Console.WriteLine("Connecting to WebSocket...");

            //Lets ensure the Token first

            bool IsValidSession =await _angelOneTokenClient.EnsureSession();
            if(!IsValidSession)
            {
                Console.WriteLine("Failed creating sockect connection. Session is invalid.");
                return;
            }
            var token = _tokenManager.GetAPIToken();
            if (!IsConnected)
            {
                await _webSocketV2.ConnectAsync(Constants.Sockets.TickerSocketUrl,
                         new Dictionary<string, string>()
                         {
                             [Constants.Sockets.Headers.AUTHORIZATION] = token.JwtToken,
                             [Constants.Sockets.Headers.APIKEY] = _smartApiSettings.GetAPIKey(),
                             [Constants.Sockets.Headers.CLIENTCODE] = _smartApiSettings.Credentials.ClientCode,
                             [Constants.Sockets.Headers.FEEDTOKEN] = token.FeedToken
                         }); 
            }
        }

        /// <summary>
        /// Send Request message to WebSocket
        /// </summary>
        public void SendAsync(string msg)
        {
            if (IsConnected)
            {
                _webSocketV2.SendAsync(msg);
            }
        }

        /// <summary>
        /// Receive response result from WebSocket
        /// </summary>
        public void ReceiveAsync()
        {
            if (IsConnected)
            {
                try
                {
                    _webSocketV2.ReceiveAsync();
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;
                    // Invoke the OnError event and pass the error message
                    OnError?.Invoke(errorMessage);
                }
            }
        }

        #endregion

        #region Events
        private void ConnectedHandler()
        {
            OnConnect?.Invoke();
            // Start the heartbeat method once connected
            Heartbeat();
        }
        private void CloseHandler()
        {
            if (isTimerRunning)
            {
                timer.Stop();
                isTimerRunning = false;
            }

            OnClose?.Invoke();
        }
        private void ErrorHandler(string Message)
        {
            // pipe the error message from ticker to the events
            OnError?.Invoke(Message);
        }

        /// <summary>
        /// Reads buffer data from raw binary data
        /// </summary>
        private void OnDataReceivedHandler(byte[] Data, bool EndOfMessage, string MessageType)
        {
            if (MessageType == Convert.ToString(WebSocketMessageType.Binary))
            {
                var sub_mod = Data.Skip(0).Take(1).ToArray();
                if (sub_mod[0] == Constants.TickerModes.Codes[Constants.TickerModes.LTP])
                {
                    TickLtp tickltp = new TickLtp();
                    tickltp = ReadLTP(Data);
                    OnTickLtp(tickltp);
                }
                else if (sub_mod[0] == Constants.TickerModes.Codes[Constants.TickerModes.QUOTE])
                {
                    TickQuote tickquote = new TickQuote();
                    tickquote = ReadQuote(Data);
                    OnTickQuote(tickquote);
                }
                else if (sub_mod[0] == Constants.TickerModes.Codes[Constants.TickerModes.FULL])
                {
                    Tick tick = new Tick();
                    tick = ReadFull(Data);
                    if(OnTick != null)
                    {
                        OnTick(tick);
                    }
                }
            }
            else if (MessageType == Convert.ToString(WebSocketMessageType.Text))
            {
                TickPong tickpong = new TickPong();
                tickpong = ReadPong(Data);
                if(OnTickPong != null)
                {
                    OnTickPong(tickpong);
                }
            }
            else
            {
                CloseSocket();
            }
        }
        #endregion

        #region Read Quotes
        /// <summary>
        /// 
        /// Reads an ltp mode tick from raw binary data
        /// </summary>
        private TickLtp ReadLTP(byte[] response)
        {
            TickLtp tickltp = new TickLtp();
            tickltp.Mode = Constants.TickerModes.LTP;
            int SubscriptionMode = response[0];
            tickltp.SubscriptionMode = Convert.ToUInt16(SubscriptionMode);
            int ExchangeType = response[1];
            tickltp.ExchangeType = Convert.ToUInt16(ExchangeType);
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tickltp.Token = parts[0];
            tickltp.SequenceNumber = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            //DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var epocSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tickltp.ExchangeTimestamp = epocSeconds;
            var ltp = BitConverter.ToInt32(response.Skip(43).Take(8).ToArray(), 0);
            tickltp.LastTradedPrice = ltp * 0.01;
            return tickltp;
        }

        /// <summary>
        /// Reads a quote mode tick from raw binary data
        /// </summary>
        private TickQuote ReadQuote(byte[] response)
        {
            TickQuote tickquote = new TickQuote();
            tickquote.Mode = Constants.TickerModes.QUOTE;
            int SubscriptionMode = response[0];
            tickquote.SubscriptionMode = Convert.ToUInt16(SubscriptionMode);
            int exchangeType = response[1];
            tickquote.ExchangeType = response[1];
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tickquote.Token = parts[0];
            tickquote.SequenceNumber = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            var exchangeTimeStampInMilliSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tickquote.ExchangeTimestamp = exchangeTimeStampInMilliSeconds;
            var ltp = BitConverter.ToInt64(response.Skip(43).Take(8).ToArray(), 0);
            tickquote.LastTradedPrice = ltp;
            tickquote.LastTradedQuantity = BitConverter.ToInt64(response.Skip(51).Take(8).ToArray(), 0);
            var averageTradedPrice = BitConverter.ToInt64(response.Skip(59).Take(8).ToArray(), 0);
            tickquote.AvgTradedPrice = averageTradedPrice;
            tickquote.VolTraded = BitConverter.ToInt64(response.Skip(67).Take(8).ToArray(), 0);
            tickquote.TotalBuyQuantity = BitConverter.ToDouble(response.Skip(75).Take(8).ToArray(), 0);
            tickquote.TotalSellQuantity = BitConverter.ToDouble(response.Skip(83).Take(8).ToArray(), 0);
            var openPriceOfTheDay = BitConverter.ToInt64(response.Skip(91).Take(8).ToArray(), 0);
            tickquote.OpenPrice = openPriceOfTheDay;
            var highPriceOfTheDay = BitConverter.ToInt64(response.Skip(99).Take(8).ToArray(), 0);
            tickquote.HighPrice = highPriceOfTheDay;
            var lowPriceOfTheDay = BitConverter.ToInt64(response.Skip(107).Take(8).ToArray(), 0);
            tickquote.LowPrice = lowPriceOfTheDay;
            var closePrice = BitConverter.ToInt64(response.Skip(115).Take(8).ToArray(), 0);
            tickquote.ClosePrice = closePrice;
            return tickquote;

        }

        /// <summary>
        /// Reads a snapquote mode tick from raw binary data
        /// </summary>
        private Tick ReadFull(byte[] response)
        {
            Tick tick = new Tick();
            tick.Mode = Constants.TickerModes.FULL;
            int SubscriptionMode = response[0];
            tick.SubscriptionMode = Convert.ToUInt16(SubscriptionMode);
            tick.ExchangeType = response[1];
            var token = Encoding.UTF8.GetString(response.Skip(2).Take(25).ToArray());
            string[] parts = token.Split('\u0000');
            tick.Token = parts[0];
            tick.SequenceNumber = BitConverter.ToInt64(response.Skip(27).Take(8).ToArray(), 0);
            var exchangeTimeStampInMilliSeconds = BitConverter.ToInt64(response.Skip(35).Take(8).ToArray(), 0);
            tick.ExchangeTimestamp = exchangeTimeStampInMilliSeconds;
            var ltp = BitConverter.ToInt64(response.Skip(43).Take(8).ToArray(), 0);
            tick.LastTradedPrice = ltp;
            tick.LastTradedQuantity = BitConverter.ToInt64(response.Skip(51).Take(8).ToArray(), 0);
            var averageTradedPrice = BitConverter.ToInt64(response.Skip(59).Take(8).ToArray(), 0);
            tick.AvgTradedPrice = averageTradedPrice;
            tick.VolTraded = BitConverter.ToInt64(response.Skip(67).Take(8).ToArray(), 0);
            tick.TotalBuyQuantity = BitConverter.ToDouble(response.Skip(75).Take(8).ToArray(), 0);
            tick.TotalSellQuantity = BitConverter.ToDouble(response.Skip(83).Take(8).ToArray(), 0);
            var openPriceOfTheDay = BitConverter.ToInt64(response.Skip(91).Take(8).ToArray(), 0);
            tick.OpenPrice = openPriceOfTheDay;
            var highPriceOfTheDay = BitConverter.ToInt64(response.Skip(99).Take(8).ToArray(), 0);
            tick.HighPrice = highPriceOfTheDay;
            var lowPriceOfTheDay = BitConverter.ToInt64(response.Skip(107).Take(8).ToArray(), 0);
            tick.LowPrice = lowPriceOfTheDay;
            var closePrice = BitConverter.ToInt64(response.Skip(115).Take(8).ToArray(), 0);
            tick.ClosePrice = closePrice;
            var epoch1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastTradedTimestampInSeconds = BitConverter.ToInt64(response.Skip(123).Take(8).ToArray(), 0);
            tick.LastTradedTimestamp = lastTradedTimestampInSeconds;
            tick.OpenInterest = BitConverter.ToInt64(response.Skip(131).Take(8).ToArray(), 0);
            byte[] best5Bytes = response.Skip(147).Take(200).ToArray();
            tick.Best5 = new BestFiveItem[10];
            for (int i = 0; i < 10; i++)
            {
                var bestData = best5Bytes.Skip(i * 20).Take(20).ToArray();
                tick.Best5[i].BuySellFlag = BitConverter.ToInt16(bestData.Skip(0).Take(2).ToArray(), 0);

                tick.Best5[i].Quantity = BitConverter.ToInt64(bestData.Skip(2).Take(8).ToArray(), 0);

                var price = BitConverter.ToInt64(bestData.Skip(10).Take(8).ToArray(), 0);
                tick.Best5[i].Price = price;

                tick.Best5[i].Orders = BitConverter.ToInt16(bestData.Skip(18).Take(2).ToArray(), 0);
            }

            var upperCircuitLimit = BitConverter.ToInt64(response.Skip(347).Take(8).ToArray(), 0);
            tick.UpperCircuit = upperCircuitLimit;
            var lowerCircuitLimit = BitConverter.ToInt64(response.Skip(355).Take(8).ToArray(), 0);
            tick.LowerCircuit = lowerCircuitLimit;
            var fiftyTwoWeekHighPrice = BitConverter.ToInt64(response.Skip(363).Take(8).ToArray(), 0);
            tick.High52Week = fiftyTwoWeekHighPrice;
            var fiftyTwoWeekLowPrice = BitConverter.ToInt64(response.Skip(371).Take(8).ToArray(), 0);
            tick.Low52Week = fiftyTwoWeekLowPrice;
            return tick;
        }

        /// <summary>
        /// Read a pong from websokect
        /// </summary>
        private TickPong ReadPong(byte[] response)
        {
            TickPong tickp = new TickPong();
            var result_msg = Encoding.UTF8.GetString(response.Skip(0).Take(4).ToArray());
            tickp.PingResult = result_msg;
            return tickp;
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Tells whether ticker is connected to server not.
        /// </summary>
        public bool IsConnected
        {
            get { return _webSocketV2.IsSocketOpen(); }
        }

        /// <summary>
        /// Close a WebSocket connection
        /// </summary>
        private void CloseSocket()
        {
            this.RESUBSCRIBE_FLAG = false;
            _webSocketV2.CloseSocket();

        }
        private void Heartbeat()
        {
            // Create a timer with the specified interval
            timer = new System.Timers.Timer(Constants.Sockets.HeartBeatInterval * 1000);

            timer.Elapsed += (sender, e) =>
            {
                Console.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")} sending ping.");
                if (IsConnected)
                {
                    _webSocketV2.SendAsync(Constants.Sockets.PING);
                }

                if (IsConnected)
                {
                    _webSocketV2.ReceiveAsync();
                }
            };

            // Start the timer
            timer.Start();
        }

        /// <summary>
        /// Send Request Message to Websocket
        /// </summary>
        public async void SetMode(string requestMessage)
        {
            if (requestMessage == null) return;

            if (IsConnected)
            {
                await _webSocketV2.SendAsync(requestMessage);
            }
            if (IsConnected)
            {
                await _webSocketV2.ReceiveAsync();
            }
        }
        public void InitiatePingTimer()
        {
            // Start the ping timer
            pingTimer = new Timer(HandlePingTimeout!, null, TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);
        }

        public void ResetPingTimer()
        {
            // Cancel the previous timer if it exists
            pingTimer?.Dispose();

            // Create a new timer to check for ping within 20 seconds
            pingTimer = new Timer(HandlePingTimeout!, null, TimeSpan.FromSeconds(20), Timeout.InfiniteTimeSpan);
        }

        public void HandlePingTimeout(object state)
        {
            // Ping timeout occurred, handle it accordingly
            Console.WriteLine("Ping timeout. Reconnecting...");

            //Thread.Sleep(10000);
            // Perform necessary actions to handle the timeout and attempt to reconnect or handle the situation accordingly
            // For example, you can call the OnError method to initiate the reconnection process
            OnError("Ping timeout");
            ResetPingTimer();
        }

        #endregion
    }
}
