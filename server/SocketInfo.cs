using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class SocketInfo
{

    public readonly Socket socket;

    public SocketInfo(ref Socket socket)
    {
        this.socket = socket;

    }

    public string Id { get { return socket.Handle.ToString(); } }
    public DateTime? LastMessageDate { get; set; } = null;
    public int totalWarning = 1;



}
