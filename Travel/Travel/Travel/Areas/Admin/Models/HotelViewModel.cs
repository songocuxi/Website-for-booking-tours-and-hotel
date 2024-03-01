using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Travel.Models;

namespace Travel.Areas.Admin.Models
{
    public class HotelViewModel
    { 
        public Hotel lsHotel {get; set;}
        public List<ImgHotel> lsImgHotel { get; set; }
        public List<TypeRoom> lsTypeRoom { get; set; }

    }
}
