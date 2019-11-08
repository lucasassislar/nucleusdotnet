﻿// Copyright (C) 2016 by David Jeske, Barend Erasmus and donated to the public domain

using SimpleHttpServer;
using SimpleHttpServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleHttpServer
{

    public class HttpServer
    {
        public int Port
        {
            get; private set;
        }

        private TcpListener Listener;
        private HttpProcessor Processor;
        private bool IsActive = true;
        private Thread serverThread;

        public HttpServer(int port, List<Route> routes)
        {
            this.Port = port;
            this.Processor = new HttpProcessor();

            foreach (var route in routes)
            {
                this.Processor.AddRoute(route);
            }
        }

        public void Listen()
        {
            this.Listener = new TcpListener(IPAddress.Any, this.Port);
            this.Listener.Start();

            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private void ServerThread(object state)
        {
            while (this.IsActive)
            {
                try
                {
                    TcpClient s = this.Listener.AcceptTcpClient();
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            this.Processor.HandleClient(s);
                        }
                        catch { }
                    });
                    thread.Start();
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    var startColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Exception " + ex);
                    Console.ForegroundColor = startColor;
                }
            }
        }
    }
}


