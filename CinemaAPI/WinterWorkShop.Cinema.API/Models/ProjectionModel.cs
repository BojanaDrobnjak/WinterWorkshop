using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class ProjectionModel
    {
        [Required]
        public Guid MovieId { get; set; }
        [Required]
        public int AuditoriumId { get; set; }
        [Required]
        public DateTime ProjectionTime { get; set; }

    }
}
