using Application;
using Infrastructure;
using Presentation.Extensions;
using Presentation.Utils;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddOpenApiWithAuth();
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

LogDetails.LogEnvironmentDetails(app.Logger, builder.Configuration);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Social Campus API")
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
            .WithPreferredScheme("Bearer");
    });

    if (builder.Configuration["DOTNET_RUNNING_IN_CONTAINER"] == "true")
    {
        app.ApplyMigrations();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

RouteGroupBuilder baseGroup = app
    .MapGroup("api");

app.MapEndpoints(baseGroup);

app.UseCors("ClientCors");

await app.RunAsync();
