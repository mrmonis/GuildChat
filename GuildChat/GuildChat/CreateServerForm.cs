using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GuildChat
{
    public partial class CreateServerForm : Form
    {
        protected GuildChatForm form;

        public CreateServerForm()
        {
            InitializeComponent();
        }

        public CreateServerForm(GuildChatForm chatForm) : this()
        {
            form = chatForm;
        }

        /* Close the form */
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /* Let the user select a location */
        private void browseLocationButton_Click(object sender, EventArgs e)
        {
             FolderBrowserDialog dialog = new FolderBrowserDialog();
             if (dialog.ShowDialog() == DialogResult.OK)
             {
                 // Set the path
                 locationTextBox.Text = dialog.SelectedPath;
             }
        }

        /* Attempt to create a new server file at the selected location */
        private void createButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Write the XML file
                XmlTextWriter writer = new XmlTextWriter(locationTextBox.Text + "\\server.xml", null);
                writer.WriteStartDocument();
                writer.WriteComment("Server with information");
                writer.WriteStartElement("server");
                // Write the ip
                writer.WriteStartElement("ip");
                writer.WriteString("127.0.0.1");
                writer.WriteEndElement();
                // Write the port
                writer.WriteStartElement("port");
                writer.WriteString(portNumberUpDown.Value.ToString());
                writer.WriteEndElement();
                // Close everything
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

                form.setStatusText("Server created on port " + portNumberUpDown.Value.ToString());
                form.hasServer(true);
                form.Invalidate();
            }
            catch (Exception exc)
            {
                form.setStatusText("Form creation failed: " + exc.Message);
            }
            finally
            {
                // No matter what, close the form
                Close();
            }
        }
    }
}
