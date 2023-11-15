namespace Matkakertomus.Shared
{
	public class DestinationDto
	{
		public uint DestinationId { get; set; }
		public string? DestinationName { get; set; }
		public string? Country { get; set; }
		public string? Area { get; set; }
		public string? Description { get; set; }
		public byte[]? Image { get; set; }
	}
}
