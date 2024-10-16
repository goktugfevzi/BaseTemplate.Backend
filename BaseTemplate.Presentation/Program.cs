using Autofac;
using Autofac.Extensions.DependencyInjection;
using BaseTemplate.Business.Container;
using BaseTemplate.Business.ValidationRules.FluentValidation;
using BaseTemplate.Repository.Container;
using BaseTemplate.Presentation.Middlewares;
using BaseTemplate.Presentation.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog.Context;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Autofac.Core;
using Microsoft.Extensions.Options;
using BaseTemplate.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using System;
using Serilog.Sinks.Elasticsearch;
using BaseTemplate.Presentation.Configurations;
using Serilog.Core;
using BaseTemplate.Repository.Elastic.Configuration;
using BaseTemplate.Shared.Abstractions;
using BaseTemplate.Shared.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterBusinessServices();
builder.Services.RegisterRepositoryServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
builder.Host.UseSerilog(SerilogConfiguration.ConfigureLogging(builder.Configuration).CreateLogger());
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

app.UseMiddleware<UserLoggingMiddleware>();

app.UseAuthorization();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.MapHealthChecks("/Nasilsin");


app.Run();
