using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public class myClient
    {

        private IPAddress _ipAddress;
        private int _port;
        private Socket _client;
        private IPEndPoint _ipEndPoint;



        public myClient(string ipAddress = "LOOPBACK", int port = 6666)
        {

            _ipAddress = myHelper.ValidateIP(ipAddress);

            _port = (port > 0 ? port : 6666);


            Connection(_ipAddress, port);
        }

        public bool isConnected()
        {
            return _client.Connected;
        }
        public bool CloseConnection()
        {
            try
            {

                _client.Close();


                return true;

            }
            catch (Exception ex)
            {

                return false;
            }
        }



        public bool isReceiveMessage(int secondDelay = 10)
        {
            var _beginDate = DateTime.Now.AddSeconds(secondDelay);
            byte[] buffer = new byte[byte.MaxValue];
            bool isReceive = false;


            CancellationToken _token = new CancellationToken();
            CancellationToken _token2 = new CancellationToken();

            bool isLoop = true;



            Task.Factory.StartNew(() =>
            {

                _token.Register(Thread.CurrentThread.Abort);


                while (isLoop)
                {
                    int receiveDataLen = _client.Receive(buffer);
                    if (receiveDataLen > 0)
                    {
                        isReceive = true;
                        break;
                    }
                }




            }, _token);




            Task.Factory.StartNew(() =>
            {
                _token2.Register(Thread.CurrentThread.Abort);

                while (true)
                {

                    if (_beginDate <= DateTime.Now)
                    {
                        isLoop = false;
                        _token.ThrowIfCancellationRequested();
                        _token2.ThrowIfCancellationRequested();
                        break;

                    }



                }



            }).Wait();





            return isReceive;
        }

        private bool Connection(IPAddress serverAddress, int serverPort)
        {

            try
            {

                _ipEndPoint = new IPEndPoint(serverAddress, serverPort);

                _client = new Socket(_ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                _client.BeginConnect(_ipEndPoint, new AsyncCallback(Connect), _client);



                Recive(_client);


                return true;

            }
            catch (Exception ex)
            {

                myMonitor.WriteConsole(ex.Message, myMonitor.messageTypeEnum.Error);

                return false;
            }
        }


        private void Connect(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);



                myMonitor.WriteConsole($"Connection Established: {client.RemoteEndPoint}", myMonitor.messageTypeEnum.Status);

            }
            catch (Exception ex)
            {
                myMonitor.WriteConsole(ex.Message, myMonitor.messageTypeEnum.Error);

            }

        }

        public void SendMessage(string msg)
        {

            var _msgbytes = myHelper.StrToByte(msg);

            _client.BeginSend(_msgbytes, 0, _msgbytes.Length, 0, new AsyncCallback(SendCallback), _client);

            myMonitor.WriteConsole($"Sended: {msg}", myMonitor.messageTypeEnum.Client);

        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            int sendByte = client.EndSend(ar);



        }

        private void Recive(Socket client)
        {
            try
            {
                SocketState state = new SocketState();

                state.socket = client;

                client.BeginReceive(state.buffer, 0, SocketState.BUFFER_SIZE, 0, new AsyncCallback(ReciveCallback), state);


            }
            catch (Exception ex)
            {

                myMonitor.WriteConsole(ex.Message, myMonitor.messageTypeEnum.Error);
            }
        }
        private void ReciveCallback(IAsyncResult ar)
        {
            try
            {

                SocketState state = (SocketState)ar.AsyncState;

                Socket client = state.socket;

                int reciveByte = client.EndReceive(ar);

                if (reciveByte > 0)
                {
                    var msg = myHelper.ByteToStr(state.buffer);

                    myMonitor.WriteConsole($"Recive Message: {msg}", myMonitor.messageTypeEnum.Server);

                    client.BeginReceive(state.buffer, 0, SocketState.BUFFER_SIZE, 0, new AsyncCallback(ReciveCallback), state);

                }


            }
            catch (Exception ex)
            {

                myMonitor.WriteConsole(ex.Message, myMonitor.messageTypeEnum.Error);

            }
        }



    }
}
