using MeuBolso.IdentifyServer.Api;
using MeuBolso.IdentifyServer.Api.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IdentityServerDbContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser<Guid>>();

await app.RunAsync();
