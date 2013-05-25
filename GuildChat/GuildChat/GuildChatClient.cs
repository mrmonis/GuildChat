using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Xml.Linq;
using NAudio.Wave;
using System.Net;
using System.Threading;

namespace GuildChat
{
    /// <summary>
    /// The client must connect to the server as well as other peers in the list
    /// </summary>
    public class GuildChatClient
    {
        // Info about the server, the client and all other peers respectively
        public Peer server { get; set; }
        protected Peer self;
        protected List<Peer> peers;

        // The socket used by this client to both receive and send data
        protected UdpClient clientSocket;

        // Another thread is used to send received data to the speakers
        protected Thread listenThread;

        // Audio elements (requires NAudio.dll)
        protected WaveIn waveIn;

        public GuildChatClient(Peer server)
        {
            this.server = server;
        }

        // Attempt to add this peer to the list
        public bool AddClient()
        {
            // Open a new connection to the server and add to the list
            using (TcpClient client = new TcpClient(server.Ip, server.Port))
            {

                // Ask to be added to the peer list
                byte[] addRequest = GuildChatServer.Requests.Serialize(GuildChatServer.Requests.CreateAddMsg(server.Ip, server.Port));
                NetworkStream stream = client.GetStream();
                stream.Write(addRequest, 0, addRequest.Length);

                // Parse the response
                string response;
                byte[] buffer = new byte[1024];

                int read = stream.Read(buffer, 0, buffer.Length);
                response = GuildChatServer.Requests.Deserialize(buffer);

                // If something was returned and the response was good return true 
                if (response != null)
                {
                    String[] split = response.Split('|');
                    if (split[0].Equals("OK"))
                    {
                        // Get the ip and port returned by the server
                        self = new Peer(split[1], int.Parse(split[2]));

                        return true;
                    }
                }

                // In any other case, return false
                return false;
            }
        }

        // Attempts to retrieve peer info from the server
        public bool GetPeers()
        {
            // Open a new connection to the server and ask for the peer list
            using (TcpClient client = new TcpClient(server.Ip, server.Port))
            {
                byte[] getRequest = GuildChatServer.Requests.Serialize(GuildChatServer.Requests.CreateGetMsg());
                NetworkStream stream = client.GetStream();
                stream.Write(getRequest, 0, getRequest.Length);

                // Parse the response
                string response;
                byte[] buffer = new byte[1024];

                long read = stream.Read(buffer, 0, buffer.Length);
                response = GuildChatServer.Requests.Deserialize(buffer);

                // If something was returned, get the file
                if (response != null)
                {
                    String[] split = response.Split('|');
                    if (split[0].Equals("OK"))
                    {
                        // Get the size of the peer list
                        long size = long.Parse(split[1]);
                        
                        // Keep track of bytes read and output to memory stream
                        read = 0;
                        buffer = new byte[size];

                        do
                        {
                            read += stream.Read(buffer, (int)read, buffer.Length - (int)read);
                        } while (read < size);

                        // When done, write to stream
                        MemoryStream output = new MemoryStream(buffer);

                        // Get the peers
                        peers = new List<Peer>();
                        XDocument document = XDocument.Load(output);
                        IEnumerable<XElement> peerList = document.Element("peers").Elements("peer");

                        // Grab the data from each element
                        foreach (XElement element in peerList) 
                        {
                            Peer p = new Peer(element.Element("ip").Value, int.Parse(element.Element("port").Value));
                            
                            // Make sure the client doesn't add itelf to the list
                            if (!p.Ip.Equals(self.Ip))
                            {
                                peers.Add(p);
                            }
                        }

                        // Set up the UDP port for sending and receiving
                        clientSocket = new UdpClient();

                        // Start the listening thread
                        listenThread = new Thread(new ThreadStart(ListenToBroadcast));
                        listenThread.Start();

                        return true;
                    }
                }
                return false;
            }
        }

        /* Passes any received data to the speakers */
        protected void ListenToBroadcast()
        {
            UdpClient listenSocket = new UdpClient(self.Port);
            WaveOut waveOut = new WaveOut();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(self.Ip), self.Port);
            BufferedWaveProvider provider = new BufferedWaveProvider(new WaveFormat());

            while (true)
            {
                byte[] data = listenSocket.Receive(ref ip);
                provider.AddSamples(data, 0, data.Length);
                if (provider.BufferedDuration > new TimeSpan(100))
                {
                    waveOut.Play();
                }
            }
        }

        /* Opens up a datagram socket for transmitting and receiving sound */
        public void StartBroadcast()
        {
            // Create a new WaveIn and set callbacks
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += waveIn_RecordingStopped;
            waveIn.WaveFormat = new WaveFormat();

            waveIn.StartRecording();
        }

        /* Callback when data is recorded  */
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // Send the data to the peers
            foreach (Peer p in peers)
            {
                clientSocket.Send(e.Buffer, e.BytesRecorded,p.Ip, p.Port);
            }
        }

        /* Stop broadcasting data */
        public void StopBroadcast()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }
        }


        /* Callback when recording is done */
        private void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveIn.Dispose();
            waveIn = null;
        }
    }
}
