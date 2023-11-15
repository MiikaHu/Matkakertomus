using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matkakertomus.Shared
{
    public class StoryDto
    {
        public uint StoryId { get; set; }
        public DateTime? Date { get; set; }
        public string? Text { get; set; }

        public uint TripId { get; set; }
        //public virtual Trip Trip { get; set; } = null!;

        public uint DestinationId { get; set; }
        //public virtual Destination Destination { get; set; } = null!;

        //public virtual ICollection<Picture> Pictures { get; } = new List<Picture>();
    }
}
