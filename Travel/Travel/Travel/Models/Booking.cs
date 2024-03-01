using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Booking
    {
        public int TourId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool Pay { get; set; }
        public int BookingId { get; set; }

        public virtual Account Customer { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
