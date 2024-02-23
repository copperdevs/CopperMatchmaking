using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CopperMatchmaking.Util
{
    internal class SteamAPIHelper
    {
        public const string STEAM_API_URL = "https://partner.steam-api.com/";
        public const string STEAM_APP_ID = "2149290";
        public const string STEAM_PUBLISHER_WEB_API_KEY = "2131582260089A7AC57871BD68275ED2";

        public static async Task<string> QueryApi(string endPoint, Dictionary<string, object> parameters)
        {
            string url = ConstructApiUrl(endPoint, parameters);

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return "";
                }

                return "";
            }
        }

        static string ConstructApiUrl(string endPoint, Dictionary<string, object> parameters)
        {
            string url = STEAM_API_URL + endPoint + "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            return url;
        }
    }
}
