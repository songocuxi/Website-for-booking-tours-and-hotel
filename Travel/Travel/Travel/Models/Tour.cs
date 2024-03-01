using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Tour
    {
        public Tour()
        {
            Bookings = new HashSet<Booking>();
            HotelBookings = new HashSet<HotelBooking>();
            ImgTours = new HashSet<ImgTour>();
        }

        public int TourId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }
        public int PlaceId { get; set; }

        public virtual Place Place { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<HotelBooking> HotelBookings { get; set; }
        public virtual ICollection<ImgTour> ImgTours { get; set; }
    }
}
