using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public class myHelper
{

    public static IPAddress ValidateIP(string ipAddress)
    {


        if (ipAddress == "LOOPBACK")
        {
            return IPAddress.Loopback;

        }
        else
        {

            bool isTrue = IPAddress.TryParse(ipAddress, out IPAddress validIP);

            if (isTrue)
                return validIP;
            else
                throw new Exception($"{Assembly.GetCallingAssembly().GetName().Name} - {ipAddress} IPAddress is not valid.");

        }


    }


    public static byte[] StrToByte(string message)
    {

        var _byte = Encoding.ASCII.GetBytes(message.TrimEnd('\0'));

        return _byte;

    }

    public static string ByteToStr(byte[] bytes)
    {

        var str = Encoding.ASCII.GetString(bytes).TrimEnd('\0');

        return str;

    }

}
