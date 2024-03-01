using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            HotelBookings = new HashSet<HotelBooking>();
            ImgHotels = new HashSet<ImgHotel>();
            TypeRooms = new HashSet<TypeRoom>();
        }

        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<HotelBooking> HotelBookings { get; set; }
        public virtual ICollection<ImgHotel> ImgHotels { get; set; }
        public virtual ICollection<TypeRoom> TypeRooms { get; set; }
    }
}
