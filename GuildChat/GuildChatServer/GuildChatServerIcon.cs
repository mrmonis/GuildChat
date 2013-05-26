using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuildChatServer.Properties;
using System.Net;

namespace GuildChatServer
{
    class GuildChatServerIcon : IDisposable
    {
        NotifyIcon icon;

        // The server being run
        GuildChatServer server;

        /* Constructor */
        public GuildChatServerIcon()
        {
            // Instantiate icon
            icon = new NotifyIcon();
        }

        /* Displays the icon on the system tray */
        public void Display()
        {
            // Attach all the handlers
            icon.MouseClick += new MouseEventHandler(icon_MouseClick);
            icon.Icon = Resources.GuildChatIcon;
            icon.Text = "Guild Chat Server";
            icon.Visible = true;

            // Create the context menu
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator seperator = new ToolStripSeparator();

            // Item for starting server
            item = new ToolStripMenuItem("Start");
            item.Click += start_Click;
            menu.Items.Add(item);

            // Item for stopping server
            item = new ToolStripMenuItem("Stop");
            item.Click += stop_Click;
            menu.Items.Add(item);

            // Item for restarting the server
            item = new ToolStripMenuItem("Restart");
            item.Click += restart_Click;
            menu.Items.Add(item);

            // Seperator
            menu.Items.Add(seperator);

            // Item for closing the server
            item = new ToolStripMenuItem("Exit");
            item.Click += exit_Click;
            menu.Items.Add(item);

            // Add the context menu to the icon
            icon.ContextMenuStrip = menu;

        }

        /* Close the application on click */
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /* Restarts the server */
        private void restart_Click(object sender, EventArgs e)
        {
            stop_Click(sender, e);
            start_Click(sender, e);
        }

        /* Stops the server if its running */
        private void stop_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.StopServer();
                server = null;
            }
        }

        /* Starts the server if it isn't running */
        private void start_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                // Create a new server, starting it in the process
                server = new GuildChatServer();
            }
        }

        /* Remove the icon from the tray immediately */
        public void Dispose()
        {
            // Remove the icon immediatly
            icon.Dispose();
        }

        public void icon_MouseClick(object sender, MouseEventArgs e)
        {
            /* Open the menu regardless of button */
            if (e.Button == MouseButtons.Left)
            {
                
            }
        }
    }
}
