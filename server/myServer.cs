using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class myServer
{

    private Socket _server;
    private IPEndPoint localEndpoint;
    private int _port = 6666;
    private int _backlogCounter = 10;


    public static List<SocketInfo> clientList = new List<SocketInfo>();
    public static ManualResetEvent allDone = new ManualResetEvent(false);
    private string _ServerLabel => $"Welcome to Baslayalim Services ({localEndpoint.Address}:{localEndpoint.Port})";


    #region Warning/Kick Settings

    private int _maxWarning = 3;
    private string _WarningMessage => $"You can send up to {_maxWarning} messages in one second. (Try {{warningNumber}}/{_maxWarning})";
    private string _BanMessage => $"You have been kicked out of the system for sending more than {_maxWarning} messages per second";

    #endregion

    public myServer(int port = 6666, int backlogCount = 10)
    {


        #region Detected LocalIPAddress 

        //Windows reserved port range 1~1024 
        _port = (port > 1024 ? port : 6666);

        var localIPAdress = IPAddress.Loopback;
        localEndpoint = new IPEndPoint(localIPAdress, _port);

        #endregion

        try
        {
            _server = new Socket(localEndpoint.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
            _server.Bind(localEndpoint);
            _server.Listen(_backlogCounter);

            myMonitor.WriteConsole(_ServerLabel, myMonitor.messageTypeEnum.Server);

            var _th = new Thread(() => {

                while (true)
                {
                    allDone.Reset();

                    _server.BeginAccept(new AsyncCallback(HandleSocket), _server);

                    allDone.WaitOne();

                }

            });

            _th.Start();

        }
        catch (Exception ex)
        {

            //throw new Exception(ex.Message);
        }

    }


    public bool IsActive()
    {
        try
        {

            using (System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient())
            {
                client.Connect(localEndpoint);

                client.Close();
            }

            return true;

        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private void HandleSocket(IAsyncResult ar)
    {
        allDone.Set();

        Socket _so = (Socket)ar.AsyncState;
        var client = (Socket)_so.EndAccept(ar);

        clientList.Add(new SocketInfo(ref client));

        #region welcome message


        var _byteServerLabel = myHelper.StrToByte(_ServerLabel);
        client.Send(_byteServerLabel);

        myMonitor.WriteConsole($"Connected To {client.RemoteEndPoint}/ID:{client.Handle.ToString()}", myMonitor.messageTypeEnum.Status);

        #endregion


        SocketState state = new SocketState();

        state.socket = client;
        client.BeginReceive(state.buffer, 0, SocketState.BUFFER_SIZE, 0, Recive, state);

    }

    private void Recive(IAsyncResult ar)
    {
        SocketState _ss = (SocketState)ar.AsyncState;
        Socket client = _ss.socket;


        //int read = client.EndReceive(ar);

        SocketError errorCode;
        int read = client.EndReceive(ar, out errorCode);
        if (errorCode != SocketError.Success)
        {
            read = 0;
        }


        if (read > 1)
        {

            var _socketInfo = clientList.Find(x => x.Id == client.Handle.ToString());



            if (_socketInfo != null)
            {


                if (_socketInfo.LastMessageDate != null)
                {
                    var _waitCalc = _socketInfo.LastMessageDate.Value;

                    if (_waitCalc.Ticks >= DateTime.Now.AddMilliseconds(-900).Ticks)
                    {

                        if (_socketInfo.totalWarning >= _maxWarning)
                        {
                            _socketInfo.Kick(_BanMessage);
                            clientList.Remove(_socketInfo);

                            return;
                        }



                        var _repWar = _WarningMessage.Replace("{warningNumber}", _socketInfo.totalWarning.ToString());
                        _socketInfo.Warning(_repWar);

                    }
                    else
                    {

                        _socketInfo.LastMessageDate = DateTime.Now;
                    }



                }
                else
                {

                    _socketInfo.LastMessageDate = DateTime.Now;
                }

            }

            string txt = myHelper.ByteToStr(_ss.buffer);

            var _clientLabel = $"(Client: {client.RemoteEndPoint}/ID:{client.Handle.ToString()}";

            myMonitor.WriteConsole($"{_clientLabel} => {txt}", myMonitor.messageTypeEnum.Client);

            _ss.Clear();

            client.BeginReceive(_ss.buffer, 0, SocketState.BUFFER_SIZE, 0, new AsyncCallback(Recive), _ss);

        }



    }


}

