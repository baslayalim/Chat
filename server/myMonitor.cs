using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class myMonitor
{
    public enum messageTypeEnum
    {
        Error,
        Client,
        Server,
        Status
    }

    public static async Task WriteConsole(string message, messageTypeEnum msgType)
    {
        var _dt = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();

        if (msgType == messageTypeEnum.Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_dt + " Error: " + message);
            Console.ResetColor();

        }
        else if (msgType == messageTypeEnum.Client)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_dt + " Client: " + message);
            Console.ResetColor();

        }
        else if (msgType == messageTypeEnum.Server)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(_dt + " Server: " + message);
            Console.ResetColor();

        }
        else if (msgType == messageTypeEnum.Status)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(_dt + " Status: " + message);
            Console.ResetColor();

        }

    }


}
