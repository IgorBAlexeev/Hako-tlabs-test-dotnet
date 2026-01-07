using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Text;
using TestApp.ToDoList.Module;
using TestApp.ToDoList.Repository;
using TestApp.ToDoList.Store;
using TestApp.ToDoList.Tracker;

namespace TestApp.Server
{
  public class Startup
  {
    IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
      this.configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      // Add DB
      services.AddDbContext<ToDoListDbContext>();

      // Add controllers
      services.AddControllers(options => {
        options.CacheProfiles.Add("NoCache",
        new CacheProfile() { NoStore = true });
        options.CacheProfiles.Add("Any-120",
        new CacheProfile()
        {
          Location = ResponseCacheLocation.Any,
          Duration = 120
        });
      });

      // Configure app services
      services.AddScoped<IToDoListTracker, ToDoListTracker>();
      services.AddScoped<IToDoItemsRepository, ToDoItemsRepository>();
      services.AddScoped<ToDoListEntityModel>();
      var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
      services.AddCors(options =>
      {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
              builder.WithOrigins("http://localhost:5173") // Allow requests from your frontend's exact origin
                 .AllowAnyHeader()
                 .AllowAnyMethod();
            });
      });
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
      };
    });
      services.AddAuthorization();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen(options =>
      {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
          In = ParameterLocation.Header,
          Description = "Please enter token",
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
              }
            },
            Array.Empty<string>()
          }
        });
      });
      services.AddMemoryCache();
      services.AddExceptionHandler<GlobalExceptionHandler>();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svcProv)
    {
      // Enable Swagger in all environments
      app.UseSwagger();
      app.UseSwaggerUI();

      var appLifetime = svcProv.GetRequiredService<IHostApplicationLifetime>();
      appLifetime.ApplicationStarted.Register(onApplicationStarted);

      app.UseRouting();
      app.UseCors("_myAllowSpecificOrigins");
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseExceptionHandler(_ => { });
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      }
      );
    }
    void onApplicationStarted()
    {
      // Do nothing
    }
  }
}