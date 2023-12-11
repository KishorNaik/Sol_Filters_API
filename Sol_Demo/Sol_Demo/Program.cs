using Microsoft.EntityFrameworkCore;
using Sol_Demo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(option =>
{
    option.RegisterServicesFromAssemblyContaining(typeof(Program));
});

builder.Services.AddDbContext<AdventureWorks2012Context>(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("DB");

    config.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();