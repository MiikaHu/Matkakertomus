namespace Matkakertomus.Shared
{
    public class ProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? Area { get; set; }
        public byte[]? Image { get; set; }
        public uint TravellerId { get; set; }
    }
}