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

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Social Campus API")
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
            .AddPreferredSecuritySchemes("BearerAuth")
            .AddHttpAuthentication("BearerAuth", auth => { auth.Token = "eyJhbGciOiJ..."; });
    });
}

app.UseHttpsRedirection();

app.UseCors("ClientCors");

app.UseAuthentication();
app.UseAuthorization();

var baseGroup = app
    .MapGroup("api");

app.MapEndpoints(baseGroup);

await app.RunAsync();