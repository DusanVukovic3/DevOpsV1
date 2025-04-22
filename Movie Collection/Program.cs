using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movie_Collection.Authentication.Repository;
using Movie_Collection.Authentication.Service;
using Movie_Collection.Mapper;
using Movie_Collection.Movies.Repository;
using Movie_Collection.Movies.Service;
using Movie_Collection.Settings;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MovieCollectionDb") ?? throw new InvalidOperationException("Connection string 'MovieCollectionDb' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authorization using Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>(); //swashbuckle
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();  //Dependency injection for when we re using interface
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", b =>
    {
        var env = builder.Environment.EnvironmentName;

        if (builder.Environment.IsDevelopment())
        {
            // Allow Angular running locally
            b.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        }
        else
        {
            // Production environment - allow Netlify + custom domain
            b.WithOrigins(
                 "https://krecimstanove.com")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())    // Na ovaj nacin, moze da radi i lokalno i na VPS
{
    app.Run(); 
}
else
{
    app.Run("http://0.0.0.0:5000"); 
}

