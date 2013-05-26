using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildChatServer
{
    /// <summary>
    /// Holds all the servers properties
    /// </summary>
    public class ServerData
    {
        public string IP {get; set;}
        public int Port {get;  set;}

        public ServerData(string ip, int port)
        {
            IP = ip;
            Port = port;
        }

        public ServerData()
        {
        }
    }
}
