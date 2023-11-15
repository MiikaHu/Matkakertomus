namespace Matkakertomus.Server.Models;

public partial class Trip
{
    public uint TripId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public sbyte? Private { get; set; }

    public string Title { get; set; } = null!;

    public uint TravellerId { get; set; }
    public virtual Traveller Traveller { get; set; } = null!;

    public virtual ICollection<Story> Stories { get; } = new List<Story>();
}
