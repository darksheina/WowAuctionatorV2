using System;
namespace WowAuctionnatorV2.Dao.DatabaseConnection.Query
{
    public class GetItemQuery
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool SearchAll { get; set; }
    }
}
