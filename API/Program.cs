using API.Data;
using API.Tools;
using BLL.DTO.Mails;
using BLL.DTO.Payment;
using BLL.Service.BrainTree;
using BLL.Service.Copies;
using BLL.Service.Games;
using BLL.Service.Genres;
using BLL.Service.Mails;
using BLL.Service.Orders;
using BLL.Tools;
using DAL.Context;
using DAL.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
if (builder.Environment.IsProduction())
{
    builder.Logging.AddAzureWebAppDiagnostics();
}

var defaultConnection = builder.Configuration.GetConnectionString("Default");
var sqlConnection = new SqlConnection(defaultConnection ?? string.Empty);

builder.Services.AddDbContext<GameContext>(options =>
	options.UseSqlServer(defaultConnection, providerOptions =>
	{
		providerOptions.EnableRetryOnFailure();
	})
	.UseLazyLoadingProxies());

builder.Services.AddIdentity<User, IdentityRole<int>>()
	.AddEntityFrameworkStores<GameContext>()
	.AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.RequireUniqueEmail = true;

	options.SignIn.RequireConfirmedEmail = true;
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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Constants.ADMINISTRATOR, authBuilder =>
		{
			authBuilder.RequireRole(Constants.ADMINISTRATOR);
		})
    .AddPolicy(Constants.MANAGER, authBuilder =>
		{
			authBuilder.RequireRole(Constants.ADMINISTRATOR, Constants.MANAGER);
		});

builder.Services.AddCors(options =>
{
	options.AddPolicy("Policy", builder =>
	{
		builder.AllowAnyMethod();
		builder.AllowAnyHeader();
		builder.WithOrigins("https://localhost:44358", "http://localhost:52324", "https://localhost:7219", "http://localhost:5019");
	});
});
MailSettings emailConfig = new();
var mailSettingsSection = builder.Configuration.GetSection("MailSettings");
mailSettingsSection.Bind(emailConfig);
builder.Services.AddSingleton(emailConfig);

BraintreeSettings braintreeConfig = new();
var braintreeConfigSection = builder.Configuration.GetSection("BraintreeGateway");
braintreeConfigSection.Bind(braintreeConfig);
builder.Services.AddSingleton(braintreeConfig);

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ISubscriptionService, MailService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICopyService, CopyService>();
builder.Services.AddTransient<IBraintreeService, BraintreeService>();

var app = builder.Build();
if (app.Logger.IsEnabled(LogLevel.Debug))
{
	app.Logger.LogDebug(mailSettingsSection.ToString());
	app.Logger.LogDebug(braintreeConfigSection.ToString());
}
if (!app.Environment.IsDevelopment())
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}
else
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
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