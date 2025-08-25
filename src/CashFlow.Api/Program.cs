using CashFlow.Store.Api.Endpoints;
using CashFlow.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

app.MapAllEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CashFlow.Infrastructure.Common.Persistence.CashFlowDbContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();

await app.RunAsync();

