using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Erebus.MojangAPI.Model;

public class Manifest
{
    [JsonPropertyName("entries")] public int Entries { get; set; }
}

public class NewsManifest
{
    [JsonPropertyName("title")] public string Title { get; set; }
    [JsonPropertyName("tag")] public string Tag { get; set; }
    [JsonPropertyName("category")] public string Category { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("readMoreLink")] public string ReadMoreLink { get; set; }
}

public class MainNewsManifest
{
    [JsonPropertyName("entries")] public NewsManifest[] Entries { get; set; }
}