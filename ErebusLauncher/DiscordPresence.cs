using DiscordRPC;
using DiscordRPC.Logging;
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

        public DiscordRpcClient RunConnection(string current)
        {
            client = new DiscordRPC.DiscordRpcClient(SystemConfig.APPID);

            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            // This does not show anything
            //AllocConsole();

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
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
