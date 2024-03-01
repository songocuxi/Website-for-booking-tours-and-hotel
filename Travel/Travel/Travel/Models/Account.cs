using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Account
    {
        public Account()
        {
            Bookings = new HashSet<Booking>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
