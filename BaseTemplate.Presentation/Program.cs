using Autofac;
using Autofac.Extensions.DependencyInjection;
using BaseTemplate.Business.Container;
using BaseTemplate.Dal.Container;
using BaseTemplate.Presentation.Configurations;
using BaseTemplate.Presentation.Middlewares;
using BaseTemplate.Presentation.Modules;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

//PROGRAM YAZARAK YAZILAN TUM FLUENT VALIDATIONLARI VE MAPPER PROFILE LERI EKLEMIS OLDUK
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddAutoMapper(typeof(Program));
//PROGRAM YAZARAK YAZILAN TUM FLUENT VALIDATIONLARI VE MAPPER PROFILE LERI EKLEMIS OLDUK

builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});
builder.Services.RegisterRepositoryServices(builder.Configuration);
builder.Services.RegisterBusinessServices();
builder.Services.AddMemoryCache();



//sorulacak
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

//sorulacak
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

builder.Services.AddHttpLogging(l =>
{
    l.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    l.RequestHeaders.Add("sec-ch-ua");
    l.MediaTypeOptions.AddText("application/javascript");
    l.RequestBodyLogLimit = 4096;
    l.ResponseBodyLogLimit = 4096;
});
Logger log = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour)
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("Sql"), sinkOptions: SerilogConfiguration.SinkOptions, columnOptions: SerilogConfiguration.ColumnOptions)
    //.WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "Logs", needAutoCreateTable: true)
    .Enrich.FromLogContext()
    .MinimumLevel.Warning()
    .CreateLogger();

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

builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.MapControllers();

app.MapHealthChecks("/Nasilsin");


app.Run();
