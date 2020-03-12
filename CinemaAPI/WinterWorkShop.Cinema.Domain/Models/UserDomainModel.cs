using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class UserDomainModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsSuperUser { get; set; }

        public bool IsUser { get; set; }

        public int BonusPoints { get; set; }
    }
}
