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
        // The client
        protected GuildChatClient client;

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

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            server = new GuildChatServer(IPAddress.Loopback, 9000);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.StopServer();
                server = null;
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Peer server = ServerXmlManager.SelectServerFile();

            // Create a new client and attempt a connection to the server
            try
            {
                client = new GuildChatClient(server);
                
                // Checks if the client was added successfully
                if (client.AddClient())
                {
                    client.GetPeers();
                }
                else
                {
                    setStatusText("Adding to peers failed\r\n");
                }
            }
            catch (Exception exc)
            {
                setStatusText("Connection error: " + exc.Message);
            }

        }

        /* Sets up a server file at the specified location */
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the server creation form as a dialog
                CreateServerForm form = new CreateServerForm();
                form.ShowDialog();
            }
            catch (Exception exc)
            {
            }
        }

        /* Sets whether menu items are clickable or not based on the server */
        public void hasServer(bool serverCreated)
        {
            manageToolStripMenuItem.Enabled = serverCreated;
            startToolStripMenuItem.Enabled = serverCreated;
            stopToolStripMenuItem.Enabled = serverCreated;
        }

        /* Adds a line to the status text box */
        public void setStatusText(String msg)
        {
            statusTextBox.Text += msg + "\r\n";
        }
    }
}
