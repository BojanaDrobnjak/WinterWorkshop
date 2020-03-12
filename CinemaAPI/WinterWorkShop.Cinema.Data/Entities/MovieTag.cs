using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table("MovieTag")]
    public class MovieTag
    {
        [Column("movieId")]
        public Guid MovieId { get; set; }

        [Column("tagId")]

        public int Tagid { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
