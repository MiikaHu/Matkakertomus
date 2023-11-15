using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Matkakertomus.Server.Services;

public class AuthService : IAuthService
{
	private readonly IConfiguration _configuration;
	private readonly TravelContext _context;

	public AuthService(IConfiguration configuration, TravelContext context)
	{
		_configuration = configuration;
		_context = context;
	}

	public async Task<bool> Register(UserRegisterDto request)
	{
		CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

		var user = new Traveller
		{
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			Email = request.Email,
			Username = request.Username,
			FirstName = request.FirstName,
			LastName = request.LastName,
		};

		_context.Travellers.Add(user);

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException e) when (e.InnerException is SqliteException)
		{
			return false;
		}

		return true;
	}

	public async Task<string?> Login(UserLoginDto request)
	{
		var user = await _context.Travellers.FirstOrDefaultAsync(t => t.Email == request.Email);

		if (user is null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
		{
			return null;
		}

		string token = CreateToken(user);
		return token;
	}

	private string CreateToken(Traveller user)
	{
		List<Claim> claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Username),
			new Claim("Id", user.TravellerId.ToString())
            //new Claim(ClaimTypes.Role, "Admin"),
        };

		var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
			_configuration.GetSection("AppSettings:Token").Value));

		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.Now.AddDays(100),
			signingCredentials: creds);

		var jwt = new JwtSecurityTokenHandler().WriteToken(token);

		return jwt;
	}

	private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512();
		passwordSalt = hmac.Key;
		passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
	}

	private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
	{
		using var hmac = new HMACSHA512(passwordSalt);
		var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		return computedHash.SequenceEqual(passwordHash);
	}
}


public interface IAuthService
{
	Task<bool> Register(UserRegisterDto request);
	Task<string?> Login(UserLoginDto request);
}
