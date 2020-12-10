using System;
using System.Collections.Generic;
using WowAuctionnatorV2.Dao.DatabaseConnection;
using WowAuctionnatorV2.Dao.DatabaseConnection.Query;

namespace WowAuctionnatorV2.Models.Item
{
    public class ItemListByNameViewModel
    {
        public List<ItemBaseViewModel> ItemList { get; set; }

        public List<ItemTypeViewModel> ItemTypeList { get; set; }

        public object Initialize(GetItemQuery query)
        {
            ItemList = PostgresConnection.GetItem(query).Result;
            ItemTypeList = PostgresConnection.GetItemTypes();
            return this;
        }
    }
}
