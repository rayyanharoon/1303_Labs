using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace lab3
{
    class client : server
    {
        static void Main(string[] args)
        {

        }
        public static void TestWebClientWithExceptionHandling()
        {
            WebClient wc = new WebClient();
            wc.Proxy = null;

            try
            {
                wc.DownloadString(new Uri("https://www.msn123789.com"));
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    Console.WriteLine("bad domain name provided: {0}", we.Message);
                }
                else
                {
                    Console.WriteLine("webexception: {0}", we.Message);
                    throw;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }
            finally
            {
                Console.WriteLine(" finally executed.");
            }
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
        private static void RunClient(int port)
        {
            Console.WriteLine("client connecting to server on port: {0}", port);
            using (TcpClient client = new TcpClient("class.cnlntsrvr.com", port))
            {
                BinaryWriter w = null;
                BinaryReader r = null;
                using (NetworkStream n = client.GetStream())
                {
                    while (true)
                    {
                        if (w == null)
                        {
                            w = new BinaryWriter(n);
                        }
                        if (r == null)
                        {
                            r = new BinaryReader(n);
                        }
                        string resp = SendReceive(w, r, "hello");
                        resp = SendReceive(w, r, "rund");
                        Thread.Sleep(2000);
                    }
                }
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
}
