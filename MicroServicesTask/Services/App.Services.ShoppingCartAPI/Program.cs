using System.Text;
using App.Services.ShoppingCartApi;
using App.Services.ShoppingCartApi.Data;
using App.Services.ShoppingCartApi.ServiceContracts;
using App.Services.ShoppingCartApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Adding DbContext Service
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();


//Adding Automapper service to the service container
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//ProxyClients configuration for ProductClient and CouponClient

builder.Services.AddHttpClient("ProductClient", client =>{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ServiceUrls").GetValue<string>("ProductAPI"));
});

builder.Services.AddHttpClient("CouponClient", client =>{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ServiceUrls").GetValue<string>("CouponAPI"));
});

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

