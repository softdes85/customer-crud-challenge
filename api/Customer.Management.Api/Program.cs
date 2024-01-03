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
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;
using Customers.Management.Api.ExceptionHanders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// loging
// Configure Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
Log.Logger = logger;

builder.Host.UseSerilog();



builder.Host.UseSerilog();


// add cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Angular app's origin
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});


// regisgter db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");

builder.Services.AddDbContext<CustomerDBContext>(options =>
{
    switch (databaseProvider)
    {
        case "MySql":
            //options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            break;
        case "InMemoryDb":
            options.UseInMemoryDatabase("InMemoryDb");
            break;
    }
});

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

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors("AllowSpecificOrigin"); // Apply CORS policy

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
