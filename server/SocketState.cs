using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class SocketState
{
    public Socket socket = null;
    public const int BUFFER_SIZE = 1024;
    public byte[] buffer = new byte[BUFFER_SIZE];
    public StringBuilder sb = new StringBuilder();

    public void Clear()
    {
        buffer = new byte[BUFFER_SIZE];
    }
}

