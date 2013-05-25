using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace GuildChat
{
    /// <summary>
    /// Responsible for parsing, adding and deleting from the XMl file
    /// </summary>
    public class ServerXmlManager
    {

        /* Picks a server xml file */
        public static Peer SelectServerFile()
        {
            // The server's information is stored in a Peer object
            Peer serverPeer = new Peer();

            // Allow the user to select a file
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (BufferedStream stream = new BufferedStream(dialog.OpenFile()))
                {
                    XmlTextReader reader = new XmlTextReader(stream);

                    // Read the port and ip from the server's xml file
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "ip")
                            {
                                serverPeer.Ip = reader.ReadElementContentAsString();
                            }
                            if (reader.Name == "port")
                            {
                                serverPeer.Port = reader.ReadElementContentAsInt();
                            }
                        }
                    }
                }
            }

            return serverPeer;
        }

    }
}
