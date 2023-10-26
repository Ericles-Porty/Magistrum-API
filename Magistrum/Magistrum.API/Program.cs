using System.Reflection;
using System.Security.Claims;
using System.Text;
using Magistrum.API.Data;
using Magistrum.API.Repositories;
using Magistrum.API.Repositories.IRepositories;
using Magistrum.API.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Auth
var jwtSecret = new ConfigurationBuilder()
            .SetBasePath(Directory
            .GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build().GetSection("JWT")["Secret"];

var key = Encoding.ASCII.GetBytes(jwtSecret!);

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jb =>
{
    jb.RequireHttpsMetadata = false;
    jb.SaveToken = true;
    jb.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "MagistrumAPI",
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization(a =>
{
    a.AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "director", "teacher", "student"));
    a.AddPolicy("CrudSchedule", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudUser", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudClass", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudSubject", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudTeacher", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudStudent", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudClassroom", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudCourse", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    a.AddPolicy("CrudGrade", policy => policy.RequireClaim(ClaimTypes.Role, "director"));
    // Preencher com as demais políticas
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<MagistrumContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();
#endregion

#region Dependency Injection 
builder.Services.AddTransient<IScheduleService, ScheduleService>();
builder.Services.AddTransient<IScheduleRepository, ScheduleRepository>();
#endregion

#region SwaggerDoc
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Magistrum API",
        Version = "v1",
        Description = "API SGEM - Sistema de Gerenciamento Escolar Magistrum",
        Contact = new OpenApiContact
        {
            Name = "Magistrum",
            Email = "betoericles@gmail.com"
        }

    });

    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        // Scheme = "Bearer",
        BearerFormat = "JWT"
    });


    config.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
#endregion

#region Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    builder.Logging.AddFilter("Default", LogLevel.Information);
});
#endregion

#region Database Connection
var connectionString = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build().GetConnectionString("MagistrumConnectionString");


builder.Services.AddDbContext<MagistrumContext>(options =>
    options.UseNpgsql(connectionString));
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(config => config.SerializeAsV2 = true);
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "Magistrum API v1");
    });
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Starting the app");
app.Run();

