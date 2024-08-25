using AgentRest.Data;
using AgentRest.Servise;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAgentServis, AgentServis>(); // to add servis
builder.Services.AddScoped<ITargetServis, TargetServis>(); // to add servis
builder.Services.AddScoped<IMissionServis, MissionServis>(); // to add servis
 
//builder.Services.AddSingleton<Authentication>(); // AddSingleton -------------------


// for the Connection String
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
