using API.Middleware;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

var DBConnectionString = builder.Configuration.GetConnectionString("DBConnection");
var GlobalDbConnectionString = builder.Configuration.GetConnectionString("GlobalDBConnection");

// Add services to the container.
builder.Services.AddDbContext<FUEC_DbContext>(options =>
    options.UseMySql(DBConnectionString, ServerVersion.AutoDetect(DBConnectionString)));

builder.Services.AddDbContext<GLOBAL_FUEC_DbContext>(options => options.UseMySql(GlobalDbConnectionString, ServerVersion.AutoDetect(GlobalDbConnectionString)));

// Dependency Injection Addresses
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());





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

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
