using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Consumers;
using ProductManager.Application.Services;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Data;
using ProductManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ProductManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();

    config.AddConsumer<ProductCreatedConsumer>();
    config.AddConsumer<ProductUpdatedConsumer>();
    config.AddConsumer<ProductDeletedConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var app = builder.Build();

app.UseCors("AllowMyApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
