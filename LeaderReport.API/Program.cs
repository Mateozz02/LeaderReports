using LeaderReport.Application.Interfaces;
using LeaderReport.Application.Services;
using LeaderReport.Data;
using LeaderReport.Domain.Interfaces;
using LeaderReport.Infrastructure.Repositories;
using LeaderReport.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "LeaderReport API", Version = "v1" });

    c.OperationFilter<SwaggerFileOperationFilter>();
});

builder.Services.AddDbContext<LeaderReportContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileProcessService, FileProcessService>();
builder.Services.AddScoped<MarcaService>();


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