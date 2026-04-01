using Newtonsoft.Json;

namespace StellarSdk.Model
{
    public partial class Flags
    {
        [JsonProperty("auth_required")]
        public bool AuthRequired { get; set; }

        [JsonProperty("auth_revocable")]
        public bool AuthRevocable { get; set; }
    }
}
