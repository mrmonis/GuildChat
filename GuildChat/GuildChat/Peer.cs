using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildChat
{
    /// <summary>
    /// A PODA for peer information
    /// </summary>
    public class Peer
    {
        public Peer()
        {
        }

        public Peer(String ip, int port)
        {
            Ip = ip;
            Port = port;
        }
        
        public String Ip { get; set; }
        public int Port { get; set; }
    }
}
