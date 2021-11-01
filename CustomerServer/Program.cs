using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CustomerServer.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, 5000);
            listener.Start();
            
            Console.WriteLine("Server started..");
            
            while (true) {
                TcpClient client = listener.AcceptTcpClient();
            
                Console.WriteLine("Client connected");
                new Thread(() => CustomerController.HandleClientRequest(client)).Start();
            }
        }
    }
}