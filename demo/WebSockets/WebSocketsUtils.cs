using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace demo.WebSockets
{
    public class WebSocketsUtils
    {
        public const int TIMESTAMP_INTERVAL_SEC = 3;
        public const int BROADCAST_TRANSMIT_INTERVAL_MS = 100;
        public const int CLOSE_SOCKET_TIMEOUT_MS = 2500;

        // should use a logger but hey, it's a demo and it's free
        public static void ReportException(Exception ex, [CallerMemberName]string location = "(Caller name not set)")
        {
            Console.WriteLine($"\n{location}:\n  Exception {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"  Inner Exception {ex.InnerException.GetType().Name}: {ex.InnerException.Message}");
        }
    }
}
