using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OutlineBot
{
    public class Configuration
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("outlineManagementUrl")]
        public string OutlineManagementUrl { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; }

        [JsonProperty("debugGuilds")]
        public ulong[] DebugGuilds { get; set; }

        [JsonProperty("dataLimit")]
        public long DataLimit { get; set; }
    }
}
