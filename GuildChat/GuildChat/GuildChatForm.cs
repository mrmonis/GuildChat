using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using NAudio.Wave;
using System.Net.Sockets;

namespace GuildChat
{
    public partial class GuildChatForm : Form
    {
        protected string address;
        protected int port;

        // Server is only used if hosting
        protected GuildChatServer server;

        // Audio elements (requires NAudio.dll)
        protected WaveIn waveIn;

        public GuildChatForm()
        {
            InitializeComponent();
        }

        /* Attempt to access the file at the given location */
        private void connectButton_Click(object sender, EventArgs e)
        {
           
        }

        /* Start recording when the button is pressed */
        private void startButton_Click(object sender, EventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += waveIn_RecordingStopped;
            waveIn.WaveFormat = new WaveFormat();

            waveIn.StartRecording();
        }

        /* Callback when data is recorded  */
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {

        }

        /* Stop recording when the button is pressed */
        private void stopButton_Click(object sender, EventArgs e)
        {
            waveIn.StopRecording();
        }

        /* Callback when recording is finally done */
        void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveIn.Dispose();
            waveIn = null;
        }

        /* Starts a local server */
        private void startServerButton_Click(object sender, EventArgs e)
        {
            
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            server = new GuildChatServer(IPAddress.Loopback, 9000);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            server.StopServer();
            server = null;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (BufferedStream stream = new BufferedStream(dialog.OpenFile()))
                {
                    XmlTextReader reader = new XmlTextReader(stream);

                    // Read the elements
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "ip")
                            {
                                address = reader.ReadElementContentAsString();
                            }
                            if (reader.Name == "port")
                            {
                                port = reader.ReadElementContentAsInt();
                            }
                        }
                    }
                }
            }

            // Attempt to connect to the server
            try
            {
                TcpClient client = new TcpClient(address, port);

                // Ask to be added to the peer list
                byte[] addRequest = GuildChatServer.Requests.Serialize(GuildChatServer.Requests.CreateAddMsg(address, port));
                NetworkStream stream = client.GetStream();
                stream.Write(addRequest, 0, addRequest.Length);

                // Grab the response
                byte[] buffer = new byte[256];
                int received = 0;

            }
            catch (Exception exc)
            {

            }

        }

    }
}
