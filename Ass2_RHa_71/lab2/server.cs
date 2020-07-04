using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace lab2
{
    class server
    {
        
        private static void Write(BinaryWriter w, string resp)
        {
            w.Write(resp);
            w.Flush();
        }
        private static int _tcpServerPort;
        private static void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, _tcpServerPort);
            listener.Start();
            Console.WriteLine("server: listening on port: {0}", _tcpServerPort);
            using (TcpClient c = listener.AcceptTcpClient())
            {
                Console.WriteLine("server: client connected ...");
                BinaryWriter w = null;
                BinaryReader r = null;
                string resp = "";
                using (NetworkStream n = c.GetStream())
                {
                    var keepServerRunning = true;
                    string msg;
                    while (keepServerRunning)
                    {
                        if (r == null)
                        {
                            r = new BinaryReader(n);
                        }
                        Thread.Sleep(500);
                        if (!n.CanRead)
                        {
                            continue;
                        }
                        msg = r.ReadString();
                        Console.WriteLine("server: received message: {0}", msg);
                        if (msg == "rund")
                        {
                            resp = "rund command was successful";
                        }
                        else if (msg == "exit")
                        {
                            resp = "server shutting down";
                            keepServerRunning = false;
                        }
                        else
                        {
                            resp = msg + " echoing message back!";
                        }
                        if (w == null)
                        {
                            w = new BinaryWriter(n);
                        }
                        Write(w, resp);
                    }
                }
            }
            listener.Stop();
        }
        private static string SendReceive(BinaryWriter w, BinaryReader r, string cmd)
        {
            var threadID = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Client {0}: sending: {1}.", threadID, cmd);
            w.Write(cmd);
            w.Flush();
            var resp = r.ReadString();
            Console.WriteLine("client {0}: rcvd: {1}.", threadID, resp);
            return resp;
        }
    }
}
