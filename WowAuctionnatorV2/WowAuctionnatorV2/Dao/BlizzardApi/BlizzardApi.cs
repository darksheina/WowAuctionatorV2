using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using WowAuctionnatorV2.Models.Item;
using WowAuctionnatorV2.Dao.DatabaseConnection;

namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public static class BlizzardApi
    {
        private static string QuerySearchItem = "https://eu.api.blizzard.com/data/wow/search/item?namespace=static-eu&locale=fr_FR&name.fr_FR={0}&orderby=id&_page=1&access_token=USXYYQMEiSVJK5UzulThJWjGYKvXyqO1Rk&_pageSize=50";

        private static string QueryGetIcon = "https://us.api.blizzard.com/data/wow/media/item/{0}?namespace=static-us&locale=en_US&access_token=USXYYQMEiSVJK5UzulThJWjGYKvXyqO1Rk";

        private static string SnifQuery = "https://eu.api.blizzard.com/data/wow/item/{0}?namespace=static-eu&locale=fr_FR&access_token=USXYYQMEiSVJK5UzulThJWjGYKvXyqO1Rk";

        public static async Task<List<BlizzardResponse>> GetItemList(int index, int number)
        {
            List<BlizzardResponse> result = new List<BlizzardResponse>();
            for(int i =0; i < number; i++)
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(string.Format(SnifQuery, index));

                if(response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    BlizzardItemResponse item = JsonConvert.DeserializeObject<BlizzardItemResponse>(jsonString);

                    result.Add(PostgresConnection.InsertItem(item));
                }
                else
                {
                    result.Add(new BlizzardResponse
                    {
                        Id = index,
                        IsSuccess = false,
                        Message = response.ReasonPhrase
                    });
                }

                index++;
            }
            

            return result;
        }

        public static async Task<string> GetItemIcon(int id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(string.Format(QueryGetIcon, id));
            response.EnsureSuccessStatusCode();
            string jsonString = await response.Content.ReadAsStringAsync();
            BlizzardIconResponse blizzardIconResponse = JsonConvert.DeserializeObject<BlizzardIconResponse>(jsonString);

            return blizzardIconResponse.Assets.FirstOrDefault()?.Value;
        }
    }
}
