using System;
using System.Collections.Generic;

namespace Travel.Models
{
    public partial class ImgTour
    {
        public int TourId { get; set; }
        public int ImgTourId { get; set; }
        public string Description { get; set; }

        public virtual Tour Tour { get; set; }
    }
}
