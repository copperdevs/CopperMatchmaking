using CopperMatchmaking.Data;
using CopperMatchmaking.Util;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CopperMatchmaking.Server
{
    /// <summary>
    /// Base built in server handler
    /// </summary>
    public class BasicServerHandler : IServerHandler
    {
        /// <summary>
        /// Verify if a client should be allowed to connect
        /// </summary>
        /// <param name="client">Target client to connect</param>
        /// <returns>True if client is allowed to connect</returns>
        public bool VerifyPlayer(ConnectedClient client)
        {
            bool isAppOwned = PlayerOwnsApp(client);

            Console.WriteLine("Player Owned App: " + isAppOwned);

            if (!isAppOwned) return false;


            return true;
        }

        private bool PlayerOwnsApp(ConnectedClient client)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "key", SteamAPIHelper.STEAM_PUBLISHER_WEB_API_KEY },
                    { "steamid", client.PlayerId.ToString() },
                    { "appid", SteamAPIHelper.STEAM_APP_ID }
                };

                string response = SteamAPIHelper.QueryApi("ISteamUser/CheckAppOwnership/v2/", parameters).GetAwaiter().GetResult();

                var jsonObject = JsonDocument.Parse(response).RootElement;
                var ownsApp = jsonObject.GetProperty("appownership").GetProperty("ownsapp").GetBoolean();

                return ownsApp;
            }
            catch (Exception ex)
            {
                //Returns false if steam if is not valid, will be better once we authenticate user first
                Console.WriteLine($"Exception during Steam API verification: {ex.Message}");
                return false;
            }
        }

    }
}