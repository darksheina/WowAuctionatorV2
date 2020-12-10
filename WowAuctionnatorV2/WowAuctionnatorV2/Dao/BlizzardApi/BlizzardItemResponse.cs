using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WowAuctionnatorV2.Dao.BlizzardApi
{
    public class BlizzardItemResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataMember(Name = "level")]
        public int Level { get; set; }

        [DataMember(Name = "required_level")]
        public int Required_Level { get; set; }

        public BlizzardItemQuality Quality { get; set; }

        [DataMember(Name = "item_class")]
        public BlizzardItemClass Item_Class { get; set; }

        [DataMember(Name = "item_subclass")]
        public BlizzardItemSubClass Item_SubClass { get; set; }
    }
}
