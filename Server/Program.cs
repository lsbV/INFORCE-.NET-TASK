using System.Security.Claims;
using System.Text;
using Database;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Behavior;
using ShortenerComponent.Options;
using UserComponent;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ShortenerServiceOptions>(builder.Configuration.GetSection("ShortenerServiceOptions"));
builder.Services.Configure<ConcatBaseUrlBehaviorOptions>(builder.Configuration.GetSection("ConcatBaseUrlBehaviorOptions"));


builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(ShortenUrlCommand).Assembly);
});
builder.Services.AddScoped(typeof(IPipelineBehavior<VisitRequest, Url>), typeof(DistributedCacheBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<GetUrlRequest, Url>), typeof(ConcatBaseUrlBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<GetUrlRequest, Url>), typeof(DistributedCacheBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<GetUrlsRequest, List<Url>>), typeof(ConcatBaseUrlBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<GetUrlRequest, Url>), typeof(CheckAuthorityBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<DeleteUrlCommand, Url>), typeof(CheckAuthorityBehavior));
builder.Services.AddScoped(typeof(IPipelineBehavior<GetUrlRequest, Url>), typeof(AddCreatorEmailBehavior));



builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
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
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
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
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = key,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITokenService>(new TokenService(credentials));

builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly, typeof(LoginRequestValidator).Assembly]);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors();


using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
if (!await dbContext.Database.CanConnectAsync())
{
    await dbContext.Database.MigrateAsync();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();




await app.RunAsync();