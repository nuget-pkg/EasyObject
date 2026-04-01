using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public class LinksData
    {
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("templated")]
        public bool Templated { get; set; }
    }
}
