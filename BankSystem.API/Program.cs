using BankSystem.App.Interfaces;
using BankSystem.App.Services;
using BankSystem.App.Validators;
using FluentValidation.AspNetCore;
using BankSystem.Data.Storages;
using BankSystem.Data;
using Microsoft.EntityFrameworkCore;
using BankSystem.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankSystemDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientStorage, ClientStorage>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IStorage<Employee>, EmployeeStorage>();

builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();

builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<ClientDtoValidator>();
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
