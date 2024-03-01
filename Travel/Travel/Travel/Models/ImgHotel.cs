using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class ImgHotel
    {
        public int HotelId { get; set; }
        public string Description { get; set; }
        public int ImgHotelId { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
