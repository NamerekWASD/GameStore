using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Microsoft.IdentityModel.Tokens;
using ASP.NET.react.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddDbContext<ApplicationContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
				providerOptions => { providerOptions.EnableRetryOnFailure(); }).UseLazyLoadingProxies());

builder.Services.AddIdentity<User, IdentityRole<int>>()
	.AddEntityFrameworkStores<ApplicationContext>()
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
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

	options.LoginPath = "/user-account/login";
	options.AccessDeniedPath = "/user-account/accessDenied";
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
	}).AddGoogle("google", opt =>
	{
		var googleAuth = builder.Configuration.GetSection("Authentication:Google");
		opt.ClientId = googleAuth["ClientId"];
		opt.ClientSecret = googleAuth["ClientSecret"];
		opt.SignInScheme = IdentityConstants.ExternalScheme;
	}); ;

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

// Add services to the container.

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

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
	pattern: "api/{controller=Home}/{action=Index}/{id?}");

app.Run();
