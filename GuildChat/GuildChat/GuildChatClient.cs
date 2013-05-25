using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace GuildChat
{
    /// <summary>
    /// The client must connect to the server as well as other peers in the list
    /// </summary>
    public class GuildChatClient
    {
        public Peer server { get; set; }
        protected Peer[] peers;

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

                        // When done, write to a file
                        File.WriteAllBytes("peerlist.xml", buffer);
                    }
                }
                return false;
            }
        }
    }
}
