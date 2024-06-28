using System.Text;
using App.Services.AuthApi.Data;
using App.Services.AuthApi.Entites;
using App.Services.AuthApi.ServiceContracts;
using App.Services.AuthApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

//Configuring the Identity Service
builder.Services.AddIdentity<ApplicationUser,ApplicationRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
.AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();

//Adding Authentication Service
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Secret"))),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("ApiSettings").GetSection("Jwt").GetValue<string>("Audience")
    };
});

//Adding Authorization Service
builder.Services.AddAuthorization();

builder.Services.AddControllers();

//Open Api Configuration

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option => {
    option.AddSecurityDefinition(name:"Bearer",securityScheme: new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"

    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(policy =>{
    policy.AddDefaultPolicy(options => {
        options.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
