using DiscordRPC;
using DiscordRPC.Logging;
using Obsidi.Jupiter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErebusLauncher
{
    class DiscordPresence
    {
        private DiscordRpcClient client;

        private Boolean HasFailed;

        public DiscordRpcClient RunConnection(string current, MainWindow main)
        {

            client = new DiscordRPC.DiscordRpcClient(SystemConfig.APPID);

            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            // This does not show anything
            //AllocConsole();

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
                main.logger.StackLog($"presence ready on {e.User.Username}");
            };

            client.OnConnectionFailed += (sender, e) =>
            {
                Console.WriteLine("Discord is potentialy not running... continuing without it");
                if (!this.HasFailed)
                {
                    main.logger.StackLog("Cannot connect to discord, is the application open?");
                    main.logger.StackLog("continuing without rich presence");
                    main.logger.StackLog($"Discord Stack:\n>> {e} <<");
                    main.logger.StackLine();
                    this.HasFailed = true;
                }
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
                main.logger.StackLog($"Presence update, {e.Presence}");
            };

            //Connect to the RPC
            client.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client.SetPresence(new RichPresence()
            {
                Details = current,
                State = "In the launcher"
            });

            return client;
        }
    }
}
