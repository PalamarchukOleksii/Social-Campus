using System.Reflection;
using System.Text;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation;
using Presentation.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation(builder.Configuration);

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