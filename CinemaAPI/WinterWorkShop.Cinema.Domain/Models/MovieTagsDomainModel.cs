using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieTagsDomainModel
    {
        public int TagId { get; set; }
        public Guid MovieId { get; set; }
        public string TagName { get; set; }
        public string MovieTitle { get; set; }
    }
}
