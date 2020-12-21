using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using WowAuctionnatorV2.Models.Item;
using WowAuctionnatorV2.Dao.DatabaseConnection;
using System;

namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public static class BlizzardApi
    {
        private static string BlizzardToken = "USzVW1b8FNrf4ZB25toEoT1rtdcJAUFbD8";

        private static string QueryGetIcon = "https://us.api.blizzard.com/data/wow/media/item/{0}?namespace=static-us&locale=en_US&access_token=" + BlizzardToken;

        private static string SnifQuery = "https://eu.api.blizzard.com/data/wow/item/{0}?namespace=static-eu&locale=fr_FR&access_token=" + BlizzardToken;

        public static async Task<List<BlizzardResponse>> GetItemList(int index, int number)
        {
            List<BlizzardResponse> result = new List<BlizzardResponse>();

            for (int i = 0; i < number; i++)
            {
                try
                {
                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = await client.GetAsync(string.Format(SnifQuery, index));

                    if (response.IsSuccessStatusCode)
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
                catch (Exception ex)
                {
                    result.Add(new BlizzardResponse
                    {
                        Id = index,
                        IsSuccess = false,
                        Message = ex.Message + " Retrying"
                    });
                }
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
