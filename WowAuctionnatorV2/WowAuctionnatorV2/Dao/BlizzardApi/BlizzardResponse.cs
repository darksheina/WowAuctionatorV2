using System;
namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public class BlizzardResponse
    {
        public int Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
