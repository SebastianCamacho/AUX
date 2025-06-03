using API.Middleware;
using BLL.Interfaces;
using BLL.Services;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DAL.Repositories_Histories;
using DAL.Interfaces_Histories; 

var builder = WebApplication.CreateBuilder(args);

var DBConnectionString = builder.Configuration.GetConnectionString("DBConnection");
//var DBConnectionString = builder.Configuration.GetConnectionString("GlobalDBConnectionInterna");
var GlobalDbConnectionString = builder.Configuration.GetConnectionString("GlobalDBConnection");

// Add services to the container.
builder.Services.AddDbContext<FUEC_DbContext>(options =>
options.UseMySql(DBConnectionString, ServerVersion.AutoDetect(DBConnectionString)));

builder.Services.AddDbContext<GLOBAL_FUEC_DbContext>(options => 
options.UseMySql(GlobalDbConnectionString, ServerVersion.AutoDetect(GlobalDbConnectionString)));


// Dependency Injection SERVICES
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<INatural_PersonService, Natural_PersonService>();
builder.Services.AddScoped<ILegal_EntityService, Legal_EntityService>();
builder.Services.AddScoped<IThird_PartyService, Third_PartyService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IContracting_PartyService, Contracting_PartyService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IDocument_DriverService, Document_DriverService>();


// Dependency Injection REPOSITORIES
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<INatural_PersonRepository, Natural_PersonRepository>();
builder.Services.AddScoped<ILegal_EntityRepository, Legal_EntityRepository>();
builder.Services.AddScoped<IThird_PartyRepository, Third_PartyRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IContracting_PartyRepository, Contracting_PartyRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverHistoryRepository, DriverHistoryRepository>(); //History ??
builder.Services.AddScoped<IDocumentDriverRepository, DocumentDriverRepository>();
builder.Services.AddScoped<IDocumentDriverHistoryRepository, DocumentDriverHistoryRepository>(); //History ??





//añadir todos los perfiles de mapeo
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();


//Validar configuracion del los mapeos en el arranque
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
