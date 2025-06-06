using System.Reflection;
using Application;
using Infrastructure;
using Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddClientCors(
    "ClientCors",
    [
        "http://localhost:3000",
        "https://localhost:3000"
    ]
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Social Campus API")
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
            .AddPreferredSecuritySchemes("Bearer");
    });

    await app.EnsureDatabaseCreatedAsync();
}

app.UseHttpsRedirection();

app.UseCors("ClientCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints("api");

await app.RunAsync();