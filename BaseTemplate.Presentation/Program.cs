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
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));
builder.Services.AddScoped<IUserClaimService, UserClaimService>();



builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddFixedWindowLimiter(
        "Fixed",
        c =>
        {
            c.Window = TimeSpan.FromSeconds(5);
            c.PermitLimit = 1;
            c.QueueLimit = 5;
            c.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        });
});
builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("Elasticsearch"));
var serilogConfig = new SerilogConfiguration(builder.Configuration);
Logger log = serilogConfig.ConfigureLogging().CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParamters) => expires != null ? expires > DateTime.UtcNow : false,

        NameClaimType = ClaimTypes.Name
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BaseTemplate API", Version = "v1", Description = "BaseTemplate APIs" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter only token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer"
                }
            },
            Array.Empty<string>()
        }

    });
});
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

app.UseAuthorization();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    var userId = context.User.Claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();
    LogContext.PushProperty("UserName", userName);
    if (!string.IsNullOrEmpty(userId))
        LogContext.PushProperty("UserId", userId);
    await next();
});

app.UseSerilogRequestLogging();

app.MapControllers();

app.MapHealthChecks("/Nasilsin");


app.Run();
