using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class Location1
    {
        public Location1()
        {
            Places = new HashSet<Place>();
        }

        public int LocationId { get; set; }
        public int Levels { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }

        public virtual ICollection<Place> Places { get; set; }
    }
}
