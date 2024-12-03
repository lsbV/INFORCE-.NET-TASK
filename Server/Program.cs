using System.Security.Claims;
using System.Text;
using Database;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShortenerComponent.Options;
using UserComponent;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<ShortenerServiceOptions>(builder.Configuration.GetSection("ShortenerServiceOptions"));
builder.Services.AddSingleton(new ShortenerServiceOptions()
{
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
    DefaultExpirationTime = TimeSpan.FromDays(7),
    HashLength = 6,
    MaxAttempts = 5
});

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(ShortenUrlCommand).Assembly);
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!));
var credentials = new SigningCredentials(key, SecurityAlgorithms.Sha256);
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITokenService>(new TokenService(credentials));

builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly, typeof(LoginRequestValidator).Assembly]);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.MigrateAsync();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();




await app.RunAsync();