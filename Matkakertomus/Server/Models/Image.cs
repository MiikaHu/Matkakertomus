namespace Matkakertomus.Server.Models;
public class Picture
{
    public uint ImageId { get; set; }
    public byte[] Image { get; set; }

    public uint StoryId { get; set; }
    public virtual Story Story { get; set; } = null!;
}
