using Newtonsoft.Json;

namespace WowAuctionnatorV2.Models.Item
{
    public class ItemBaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("wow_id")]
        public int WowId { get; set; }
    }
}
