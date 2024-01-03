using Customers.Management.Api.Seed;
using Customers.Management.Repository;
using Customers.Management.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using Customers.Management.Repository.Repositories;
using FluentValidation.AspNetCore;
using Customers.Management.Api.Validators;
using Customers.Management.Service.Interfaces;
using Customers.Management.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// regisgter db
builder.Services.AddDbContext<CustomerDBContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

// automapper
builder.Services.AddAutoMapper(typeof(Program));

// Dependency Injection
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ICustomersService, CustomersService>();

// Validators
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>());

var app = builder.Build();

// seeding data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDBContext>();
    dbContext.SeedData();
}

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
