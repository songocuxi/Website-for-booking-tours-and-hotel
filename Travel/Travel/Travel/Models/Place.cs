using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Place
    {
        public Place()
        {
            Tours = new HashSet<Tour>();
        }

        public int PlaceId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public int? LocationId { get; set; }

        public virtual Location1 Location { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
