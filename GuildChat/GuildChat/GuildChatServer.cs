using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Net;

namespace GuildChat
{
    /// <summary>
    /// The server class, responsible for presenting list of currently connected users
    /// </summary>
    public class GuildChatServer
    {
        protected int port;
        protected IPAddress ip;
        protected Thread serverThread;
        protected TcpListener listener;
        private readonly object readlock = new object();

        public GuildChatServer(IPAddress ip, int port)
        {
            // Start a thread that listens for requests
            this.ip = ip;
            this.port = port;

            ThreadStart start = new ThreadStart(StartServer);
            serverThread = new Thread(start);
            serverThread.Start();
        }

        /* Starts the server thread, opening the listening port */
        public void StartServer()
        {
            // Start listening to connections
            listener = new TcpListener(ip, port);
            listener.Start();

            // Loop forever
            while (true)
            {
                // Need to know when server was interrupted
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    // Start a new thread that allows peers to manage data
                    ParameterizedThreadStart paramStart = new ParameterizedThreadStart(ManageClient);
                    Thread t = new Thread(paramStart);
                    t.Start(client);
                }
                catch (SocketException se)
                {
                    
                }
            }
        }

        /* Stops the server */
        public void StopServer()
        {
            try
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
            catch (Exception exc)
            {

            }
            finally
            {
                serverThread = null;
            }
        }

        /* Manage a client */
        private void ManageClient(Object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream stream = client.GetStream();
            Byte[] buffer = new Byte[256];
            String request;
            int received = 0;


            while ((received = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                // Grab the request string
                request = System.Text.Encoding.ASCII.GetString(buffer, 0, received);
                String[] split = request.Split('|');
                switch (split[0])
                {
                    case "ADD":
                        AddPeer(client, split);
                        break;
                    case "GET":
                        GetPeers(client);
                        break;
                    case "REMOVE":

                        break;
                }
            }
        }

        /* Add a peer to the current peer list */
        private void AddPeer(TcpClient client, String[] data)
        {
            // Lock so only one peer is added at a time
            lock (readlock)
            {
                
            }

            // Send the file to the peer
        }

        /* Retrieve the current peer list */
        private void GetPeers(TcpClient client)
        {
            lock (readlock)
            {
                client.Client.SendFile("peers.xml");
            }
        }

        /* Remove a peer from the current peer lise */
        private void RemovePeer(TcpClient client, String[] data)
        {

        }

        /// <summary>
        /// All requests to the server go through this interface. Ensures changes in format do not affect client or server code
        /// </summary>
        public class Requests
        {
            /* Serialzes a request message */
            public static byte[] Serialize(String msg)
            {
                return System.Text.Encoding.ASCII.GetBytes(msg);
            }

            /* Deserializes a request message */
            public static String Deserialize(byte[] data)
            {
                return System.Text.Encoding.ASCII.GetString(data);
            }

            // Returns the add message
            public static String CreateAddMsg(String ip, int port)
            {
                return "ADD|" + ip + "|" + port;
            }

            // Returns the get message
            public static String CreateGetMsg()
            {
                return "GET";
            }

            // Returns the remove message
            public static String CreateRemoveMsg(String ip, int port)
            {
                return "REMOVE|" + ip + "|" + port;
            }
        }
    }
}
