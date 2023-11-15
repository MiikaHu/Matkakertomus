using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Models;

[Index(nameof(Email), IsUnique = true)]
public class Traveller
{
	public uint TravellerId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Username { get; set; }
	public string? Area { get; set; }
	public string? Description { get; set; }
	public byte[]? Image { get; set; }
	public string Email { get; set; }

	public byte[] PasswordHash { get; set; }
	public byte[] PasswordSalt { get; set; }

	public virtual IEnumerable<Trip> Trips { get; } = new List<Trip>();
}
