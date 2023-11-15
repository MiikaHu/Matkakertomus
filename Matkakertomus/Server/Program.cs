using Matkakertomus.Server.Models;
using Matkakertomus.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
				.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TravelContext>(options =>
	options.UseSqlite("Data Source=database.db"));

builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITravellerService, TravellerService>();
builder.Services.AddScoped<ITripService, TripService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	// Configure the HTTP request pipeline.
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


public partial class Program { }