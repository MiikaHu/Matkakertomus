using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matkakertomus.Shared
{
    public class TripDto
    {
        public uint TripId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public sbyte? Private { get; set; }

        public uint TravellerId { get; set; }
        public string Title { get; set; } = null!;
    }
}
