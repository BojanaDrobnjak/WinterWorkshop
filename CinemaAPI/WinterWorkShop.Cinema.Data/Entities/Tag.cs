using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table ("tag")]
    public class Tag
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<MovieTag> MovieTags { get; set; }
    }
}
