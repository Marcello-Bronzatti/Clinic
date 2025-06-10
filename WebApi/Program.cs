using Microsoft.Data.Sqlite;
using System.Data;
using Infrastructure.Data; 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqliteConnection(builder.Configuration.GetConnectionString("DefaultConnection")));


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "docs", "script.sql");

DatabaseInitializer.InitializeDatabase(connectionString, scriptPath);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
