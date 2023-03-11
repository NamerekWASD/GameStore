using API.Data;
using API.Tools;
using BLL.Interface;
using BLL.Service;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddDbContext<GameContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
				providerOptions => { providerOptions.EnableRetryOnFailure(); }).UseLazyLoadingProxies());

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IMailService, BLL.Service.MailService>();

builder.Services.AddIdentity<User, IdentityRole<int>>()
	.AddEntityFrameworkStores<GameContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;

	options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromDays(5);

	options.LoginPath = "/api/account/login";
	options.AccessDeniedPath = "/api/account/accessDenied";
	options.SlidingExpiration = true;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = AuthOptions.ISSUER,
			ValidateAudience = true,
			ValidAudience = AuthOptions.AUDIENCE,
			ValidateLifetime = true,
			IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
			ValidateIssuerSigningKey = true,
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Administrator",
		authBuilder =>
		{
			authBuilder.RequireRole("Administrator");
		});
	options.AddPolicy("Manager",
		authBuilder =>
		{
			authBuilder.RequireRole("Administrator, Manager");
		});
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("Policy", Builder =>
	{
		Builder.AllowAnyMethod();
		Builder.AllowAnyHeader();
		Builder.WithOrigins("https://localhost:44458", "http://localhost:52324", "https://localhost:7219", "http://localhost:5019");
	});
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}
app.UseCors("Policy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();
// app.UseRequestLocalization();
// app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
// app.UseSession();
// app.UseResponseCompression();
// app.UseResponseCaching();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();