using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZERO.Material.Model.Other
{
    public class ActionTree
    {
        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("list")]
        public List<ActionTree> Data { get; set; }
    }
}