using API.Middleware;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BLL.Mappings; // Add this using directive

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

builder.Services.AddScoped<INatural_PersonRepository, Natural_PersonRepository>();
builder.Services.AddScoped<INatural_PersonService,Natural_PersonService >();

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



var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

try
{
    mapperConfig.AssertConfigurationIsValid();
}
catch (AutoMapperConfigurationException ex)
{
    Console.WriteLine("----------- ERROR DE MAPEADO -----------");
    Console.WriteLine(ex.ToString());
}


app.MapControllers();

app.Run();
