using System;
using System.Runtime.Serialization;

namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public class BlizzardItemSubClass
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
