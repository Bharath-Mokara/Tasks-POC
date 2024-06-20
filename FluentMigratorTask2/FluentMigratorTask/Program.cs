using System.Reflection;
using FluentMigrator.Runner;
using FluentMigratorTask.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


//configure azure blob storage service
//Create a container for this and update the db schema using fluent migrator.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations().For.EmbeddedResources())
                .AddLogging(lb => lb.AddFluentMigratorConsole().SetMinimumLevel(LogLevel.Debug));  // Added detailed logging
    })
    .Build();

using(var scope = host.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.MapControllers();


app.Run();

