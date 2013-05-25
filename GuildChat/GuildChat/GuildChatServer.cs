using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace GuildChat
{
    /// <summary>
    /// The server class, responsible for presenting list of currently connected users
    /// 
    /// Server information is stored in an xml file which should be saved in a publicly acessible area (Dropbox, google drive etc).
    /// 
    /// Peer information is also stored in an xml file. This file is returned to users to grant them access to peers. 
    /// Also, peers can report when other peers appear to be offline and they are removed from the current peers list
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
                // Get ip and port for connection
                string clientIP = data[1];
                string clientPort = data[2];

                // Try and open the document, creating it if it doesn't exist
                if (!File.Exists("peers.xml"))
                {
                    CreatePeerXML();
                }

                XDocument document = XDocument.Load(@"peers.xml");
                // Add a new peer
                try
                {
                    // The default first port
                    int assignedPort = 10000;
                    XElement existingPeer = document.Element("peers").Elements("peer").FirstOrDefault(e => ((string)e.Element("ip") == clientIP));
                    if (existingPeer == null)
                    {
                        // Not in list, assign new port number
                        XElement lastPort = document.Element("peers").Elements("peer").LastOrDefault(e => ((string)e.Element("port") != null));

                        // If the list isn't empty, assign to a port 2 greater than the last element
                        if (lastPort != null)
                        {
                            assignedPort = int.Parse(lastPort.Value) + 2;
                        }
                        // Peers.xml totally empty, add a new peer element
                        else 
                        {
                             // Add to peers.xml
                            XElement newPeer = new XElement("peer",
                                                new XElement("ip", clientIP),
                                                new XElement("port", assignedPort));
                            document.Element("peers").Add(newPeer);
                            document.Save(@"peers.xml");
                        }
                    }
                    else
                    {
                        // In list, get old number
                        assignedPort = int.Parse(existingPeer.Element("port").Value);
                    }

                   
                    // Everything is good, send OK message
                    SendResponse(client, Requests.CreateOKAddMsg(assignedPort));
                }
                catch (Exception exc)
                {
                    // Something bad happened, let the client know
                    SendResponse(client, Requests.CreateNOMsg());
                }

            }

            // Send the file to the peer
        }

        /* Creates the peer XML file */
        private void CreatePeerXML()
        {
            XmlTextWriter writer = new XmlTextWriter("peers.xml", null);
            // Write the intro
            writer.WriteStartDocument();
            writer.WriteComment("Current peer list");
            writer.WriteStartElement("peers");

            // Close up
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /* Retrieve the current peer list */
        private void GetPeers(TcpClient client)
        {
            lock (readlock)
            {
                SendResponse(client, Requests.CreateOKGetMsg((new FileInfo("peers.xml")).Length));

                // Read the peers and send them back one at a time
                try
                {
                    client.Client.SendFile("peers.xml");
                }
                catch (Exception exc)
                {
                    // Something bad happened, let the client know
                    SendResponse(client, Requests.CreateNOMsg());
                }
            }
        }

        /* Remove a peer from the current peer lise */
        private void RemovePeer(TcpClient client, String[] data)
        {

        }

        /* Send a response to the client */
        private void SendResponse(TcpClient client, String response)
        {
            byte[] responseBuffer = Requests.Serialize(response);
            client.GetStream().Write(responseBuffer, 0, responseBuffer.Length);
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

            // Returns the OK message for adding
            public static String CreateOKAddMsg(int port)
            {
                return "OK|" + port;
            }

            // Returns the OK message for getting
            public static String CreateOKGetMsg(long filesize)
            {
                return "OK|" + filesize;
            }

            // Returns the NO message
            public static String CreateNOMsg()
            {
                return "NO";
            }
        }
    }
}
