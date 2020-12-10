using System;
namespace WowAuctionnatorV2.Models.Item
{
    public class SellViewModel
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime SellDate { get; set; }
        public float UnitPriceWithTax { get; set; }
        public float TotalPriceWithTax { get; set; }
        public float TotalPriceWithoutTax { get; set; }
    }
}
