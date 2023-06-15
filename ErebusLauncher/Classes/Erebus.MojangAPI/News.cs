using Erebus.MojangAPI.Model;
using Newtonsoft.Json;
using ProjBobcat.Class.Helper;
using ProjBobcat.Class.Model.Mojang;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erebus.MojangAPI
{
    internal class News
    {
        public static async Task<MainNewsManifest?> GetNewsJSON()
        {
            var client = new RestClient("https://launchercontent.mojang.com");
            return await client.GetJsonAsync<MainNewsManifest>("/news.json");
        }
    }
}
