using System;
using System.Runtime.Serialization;

namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public class BlizzardItemClass
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
