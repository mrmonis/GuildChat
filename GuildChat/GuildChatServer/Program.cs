using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuildChatServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the tray icon
            using (GuildChatServerIcon icon = new GuildChatServerIcon())
            {
                // Display the icon
                icon.Display();

                // Run the application
                Application.Run();
            }

           
        }
    }
}
