using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SocketExtensions
{
    public static void Warning(this SocketInfo socketInfo, string message)
    {

        socketInfo.totalWarning++;
        var _bytes = myHelper.StrToByte(message);
        socketInfo.socket.Send(_bytes);
    }

    public static void Kick(this SocketInfo socketInfo, string kickMessage)
    {
        var _bytes = myHelper.StrToByte(kickMessage);
        socketInfo.socket.Send(_bytes);

        socketInfo.socket.Disconnect(true);

    }



}
