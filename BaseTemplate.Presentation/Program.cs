using Autofac;
using Autofac.Extensions.DependencyInjection;
using BaseTemplate.Business.Container;
using BaseTemplate.Presentation.Configurations;
using BaseTemplate.Presentation.Filters;
using BaseTemplate.Presentation.Middlewares;
using BaseTemplate.Presentation.Modules;
using BaseTemplate.Repository.Container;
using BaseTemplate.Repository.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(
    o =>
    {
        o.Filters.Add<ValidationFilter>();
    })
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
builder.Host.UseSerilog(SerilogConfiguration.ConfigureLogging(builder.Configuration).CreateLogger());

builder.Services.RegisterBusinessServices();
builder.Services.RegisterRepositoryServices(builder.Configuration);
builder.Services.AddRateLimiter();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddSwaggerSettings();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ExampleContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {  // Loglama veya hata yönetimi
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Update-Database yapamadım.");
    }
}
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials());

app.UseHttpsRedirection();

app.UseCustomException();

app.UseRateLimiter();

app.UseMiddleware<UserInfoMiddleware>();

app.UseAuthorization();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.MapHealthChecks("/Nasilsin");


app.Run();
