using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Application.Services;
using ComponentSelector.Infrastructure.Data;
using ComponentSelector.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddScoped<IComponentsRepository, ComponentsRepository>();
builder.Services.AddScoped<IComponentsService, ComponentsService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthorization();

app.MapControllers();

app.Run();
