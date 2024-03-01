using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class TypeRoom
    {
        public int TypeId { get; set; }
        public int HotelId { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
