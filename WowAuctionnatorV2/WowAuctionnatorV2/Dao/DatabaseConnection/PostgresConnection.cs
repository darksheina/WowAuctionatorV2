using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using WowAuctionnatorV2.Dao.BlizzardApi;
using WowAuctionnatorV2.Dao.DatabaseConnection.Query;
using WowAuctionnatorV2.Models.Item;

namespace WowAuctionnatorV2.Dao.DatabaseConnection
{
    public static class PostgresConnection
    {

        private static readonly string connectionString = "Host=192.168.1.30;Port=15432;Username=dev;Password=dev1234;Database=WowAuctionatorV2;";

        private static void OpenConnection(NpgsqlConnection connexionBDD)
        {
            if (connexionBDD.State == System.Data.ConnectionState.Closed)
            {
                connexionBDD.Open();
            }
        }

        private static void CloseConnection(NpgsqlConnection connexionBDD)
        {
            if (connexionBDD.State == System.Data.ConnectionState.Open)
            {
                connexionBDD.Close();
            }
        }

        public static BlizzardResponse InsertItem(BlizzardItemResponse item)
        {
            NpgsqlConnection connexionBDD = new NpgsqlConnection(connectionString);

            try
            {
                OpenConnection(connexionBDD);

                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO item (name, wow_id, type, sub_type, level, required_level) VALUES(:name, :wowId, :type, :sub_type, :level, :required_level)", connexionBDD);
                cmd.Parameters.Add(new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("wowId", NpgsqlTypes.NpgsqlDbType.Bigint));
                cmd.Parameters.Add(new NpgsqlParameter("type", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("sub_type", NpgsqlTypes.NpgsqlDbType.Text));
                cmd.Parameters.Add(new NpgsqlParameter("level", NpgsqlTypes.NpgsqlDbType.Bigint));
                cmd.Parameters.Add(new NpgsqlParameter("required_level", NpgsqlTypes.NpgsqlDbType.Bigint));
                cmd.Parameters[0].Value = item.Name;
                cmd.Parameters[1].Value = item.Id;
                cmd.Parameters[2].Value = item.Item_Class.Name;
                cmd.Parameters[3].Value = item.Item_SubClass.Name;
                cmd.Parameters[4].Value = item.Level;
                cmd.Parameters[5].Value = item.Required_Level;

                // Execute a query
                int row = cmd.ExecuteNonQuery();
                CloseConnection(connexionBDD);

                return new BlizzardResponse
                {
                    Id = item.Id,
                    IsSuccess = row > 0
                };
            }
            catch (Exception ex)
            {
                CloseConnection(connexionBDD);
                return new BlizzardResponse
                {
                    Id = item.Id,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async static Task<List<ItemBaseViewModel>> GetItem(GetItemQuery query)
        {
            List<ItemBaseViewModel> itemTypeList = null;
            NpgsqlConnection connexionBDD = new NpgsqlConnection(connectionString);

            try
            {
                OpenConnection(connexionBDD);

                string queryCondition = query.SearchAll ? "like \'%" + query.Name + "%\'" : "like \'" + UppercaseFirst(query.Name) + "%\'";

                if (query.Type != string.Empty)
                {
                    queryCondition = string.Concat(queryCondition, " AND type = " + query.Type);
                }

                NpgsqlCommand cmd = new NpgsqlCommand(string.Format("select array_to_json(array_agg(row_to_json(t)))from(SELECT * FROM item WHERE name {0} ORDER BY name limit 50) t;", queryCondition), connexionBDD);

                using (NpgsqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    itemTypeList = dr.IsDBNull(0) ? null : JsonConvert.DeserializeObject<List<ItemBaseViewModel>>(dr.GetFieldValue<string>(0));
                }

                CloseConnection(connexionBDD);
            }
            catch (Exception ex)
            {
                CloseConnection(connexionBDD);
                return null;
            }

            return itemTypeList;
        }

        public static List<ItemTypeViewModel> GetItemTypes()
        {
            List<ItemTypeViewModel> itemTypeList = null;
            NpgsqlConnection connexionBDD = new NpgsqlConnection(connectionString);
            OpenConnection(connexionBDD);

            NpgsqlCommand cmd = new NpgsqlCommand("select array_to_json(array_agg(row_to_json(t)))from(SELECT name FROM item_type) t;", connexionBDD);

            using (NpgsqlDataReader dr = cmd.ExecuteReader())
            {
                dr.Read();
                itemTypeList = JsonConvert.DeserializeObject<List<ItemTypeViewModel>>(dr.GetFieldValue<string>(0));
            }

            CloseConnection(connexionBDD);

            return itemTypeList;
        }

        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static bool InsertSell(SellViewModel sell)
        {
            NpgsqlConnection connexionBDD = new NpgsqlConnection(connectionString);

            try
            {
                OpenConnection(connexionBDD);

                // Define a query
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO sale (quantity, date, price_total_with_tax, price_total_without_tax, unit_price_with_tax, id_item) VALUES (:quantity, :date, :price_total_with_tax, :price_total_without_tax, :unit_price_with_tax, :id_item)", connexionBDD);
                cmd.Parameters.Add(new NpgsqlParameter("quantity", NpgsqlTypes.NpgsqlDbType.Integer));
                cmd.Parameters.Add(new NpgsqlParameter("date", NpgsqlTypes.NpgsqlDbType.Date));
                cmd.Parameters.Add(new NpgsqlParameter("price_total_with_tax", NpgsqlTypes.NpgsqlDbType.Real));
                cmd.Parameters.Add(new NpgsqlParameter("price_total_without_tax", NpgsqlTypes.NpgsqlDbType.Real));
                cmd.Parameters.Add(new NpgsqlParameter("unit_price_with_tax", NpgsqlTypes.NpgsqlDbType.Real));
                cmd.Parameters.Add(new NpgsqlParameter("id_item", NpgsqlTypes.NpgsqlDbType.Bigint));
                cmd.Parameters[0].Value = sell.Quantity;
                cmd.Parameters[1].Value = sell.SellDate;
                cmd.Parameters[2].Value = sell.TotalPriceWithTax;
                cmd.Parameters[3].Value = sell.TotalPriceWithoutTax;
                cmd.Parameters[4].Value = sell.UnitPriceWithTax;
                cmd.Parameters[5].Value = sell.ItemId;

                // Execute a query
                int row = cmd.ExecuteNonQuery();
                CloseConnection(connexionBDD);

                return row > 0;
            }
            catch (Exception ex)
            {
                CloseConnection(connexionBDD);
                return false;
            }
        }
    }
}
