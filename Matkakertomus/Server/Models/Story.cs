namespace Matkakertomus.Server.Models;

public partial class Story
{
    public uint StoryId { get; set; }
    public DateTime? Date { get; set; }
    public string? Text { get; set; }

    public uint TripId { get; set; }
    public virtual Trip Trip { get; set; } = null!;

    public uint DestinationId { get; set; }
    public virtual Destination Destination { get; set; } = null!;

    public virtual ICollection<Picture> Pictures { get; } = new List<Picture>();
}
