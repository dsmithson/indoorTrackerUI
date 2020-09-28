using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IndoorLocationViewer
{
    public class PillarMessage
    {
        [JsonProperty("st")]
        public string StationMac { get; set; }

        [JsonProperty("t")]
        public string T { get; set; }

        [JsonProperty("e")]
        public List<PillarMessageEntry> Entries { get; set; } = new List<PillarMessageEntry>();
    }

    public class PillarMessageEntry
    {
        [JsonProperty("m")]
        public string Mac { get; set; }

        [JsonProperty("r")]
        public string RSSI { get; set; }
    }
}
