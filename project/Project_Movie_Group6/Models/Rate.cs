using System;
using System.Collections.Generic;

namespace Project_Movie_Group6.Models
{
    public partial class Rate
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public string? Comment { get; set; }
        public double? NumericRating { get; set; }
        public DateTime? Time { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
