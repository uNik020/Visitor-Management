using Microsoft.EntityFrameworkCore;
using System;
using VisitorManagement.Models;
using VisitorManagement.Controllers;
using VisitorManagement.DTO;
using VisitorManagement.Interfaces;
using VisitorManagement.Services;
using VisitorManagement.HelperClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IHostService, HostService>();
builder.Services.AddTransient<IVisitorService, VisitorService>();
builder.Services.AddTransient<IVisitService, VisitService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IDepartmentService, DepartmentService>();

builder.Services.AddDbContext<VisitorManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.MapControllers();

app.Run();
