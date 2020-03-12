using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateReservationModel
    {
        [Required]
        public Guid ProjectionId { get; set; }


        [Required]
        public List<Guid> SeatIds { get; set; }


        [Required]
        public Guid UserId { get; set; }

    }
}