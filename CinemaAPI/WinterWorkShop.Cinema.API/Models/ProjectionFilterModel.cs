using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class ProjectionFilterModel
    {
        public int CinemaId { get; set; }
        public int AuditoriumId { get; set; }

        public Guid MovieId { get; set; }

        public DateTime DateTime { get; set; }
        public string BannerUrl { get; set; }
    }
}
