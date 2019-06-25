using System;
using CopyDirectory.Lib;

namespace CopyDirectory.UI
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
