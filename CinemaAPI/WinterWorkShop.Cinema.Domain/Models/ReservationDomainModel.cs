using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class ReservationDomainModel
    {
        public Guid ProjectionId { get; set; }

        public Guid SeatId { get; set; }

        public Guid UserId { get; set; }

    }
}
