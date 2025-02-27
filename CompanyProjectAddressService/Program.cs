using CompanyProjectAddressService.Infrastructure.DbContext;
using CompanyProjectAddressService.Infrastructure.InitialDbContent;
using CompanyProjectAddressService.Infrastructure.PartOfAddress;
using EntityFrameworkCore.UnitOfWork.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#if DEBUG
var conString = new NpgsqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("ConnectionStringToPostgreSQL"))
{
    Host = builder.Configuration["DbHost"],
    Username = builder.Configuration["DbUserName"],
    Password = builder.Configuration["DbPassword"]
}.ConnectionString;
builder.Services.AddDbContext<CompanyProjectAddressDbContext>(options =>
    options.UseNpgsql(conString, sqlOptions =>
        sqlOptions.MigrationsAssembly(typeof(CompanyProjectAddressDbContext).Assembly.GetName().Name)));
#else
builder.Services.AddDbContext<CompanyProjectAddressDbContext>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings:ConnectionStringToPostgreSQL"], sqlOptions =>
        sqlOptions.MigrationsAssembly(typeof(CompanyProjectAddressDbContext).Assembly.GetName().Name)));
#endif
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbContext, CompanyProjectAddressDbContext>();
builder.Services.AddScoped<IInitialDbContent, InitialDbContent>();
builder.Services.AddScoped<IPartOfAddress, PartOfAddress>();
builder.Services.AddUnitOfWork();
builder.Services.AddUnitOfWork<CompanyProjectAddressDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.MapControllers();

app.Run();
