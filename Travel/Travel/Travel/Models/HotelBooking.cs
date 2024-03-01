using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class HotelBooking
    {
        public int HotelId { get; set; }
        public int TourId { get; set; }
        public DateTime Date { get; set; }
        public int Hbid { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
