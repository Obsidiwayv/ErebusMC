using ProjBobcat.Class.Helper;
using ProjBobcat.Class.Model.Mojang;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Erebus.MojangAPI
{
    internal class Versions
    {
        // https://github.com/Corona-Studio/BobcatExamples/blob/main/BobcatExamples.WPF/GameBasis/Core.cs#L35
        public static async Task<VersionManifest?> GetVersionJSON()
        {
            const string vmUrl = "http://launchermeta.mojang.com/mc/game/version_manifest.json";
            var contentRes = await HttpHelper.Get(vmUrl);
            var content = await contentRes.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<VersionManifest>(content);

            return model;
        }
    }
}
