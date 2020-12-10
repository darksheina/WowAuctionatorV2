using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WowAuctionnatorV2.Dao.BlizzardApi;
using WowAuctionnatorV2.Dao.DatabaseConnection;
using WowAuctionnatorV2.Dao.DatabaseConnection.Query;
using WowAuctionnatorV2.Models.Item;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WowAuctionnatorV2.Controllers
{
    public class ItemController : Controller
    {
        // GET: /<controller>/
        public IActionResult GetItem(int Id)
        {
            return View();
        }

        public IActionResult GetItemListByName(string name, string type, bool searchAll)
        {
            GetItemQuery query = new GetItemQuery
            {
                Name = name.Replace("\'", "\'\'"),
                Type = type ?? string.Empty,
                SearchAll = searchAll
            };
            
            return View(new ItemListByNameViewModel().Initialize(query));
        }

        public IActionResult GetItemIconUrl(int id)
        {
            string iconUrl = BlizzardApi.GetItemIcon(id).Result;
            return Json(iconUrl);
        }

        public IActionResult Insert(SellViewModel sell)
        {
            bool isSucess = PostgresConnection.InsertSell(sell);
            return RedirectPermanent("/Form/Form");
        }

        public IActionResult FetchItemFromBlizzard(int index, int number)
        {
            List<BlizzardResponse> iconUrl = BlizzardApi.GetItemList(index, number).Result;
            return Ok(iconUrl);
        }
    }
}
