using DefaultCorsPolicyNugetPackage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using TELERADIOLOGY.Application;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Infrastructure;
using TELERADIOLOGY.Infrastructure.Context;
using TELERADIOLOGY.Infrastructure.Converters;
using TELERADIOLOGY.Infrastructure.Services.NetMail;
using TELERADIOLOGY.Infrastructure.Sms;
using TELERADIOLOGY.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("PostgreSql")
                       ?? "Host=localhost;Port=5432;Database=ata;Username=postgres;Password=1";

var columnWriters = new Dictionary<string, ColumnWriterBase>
{
    { "message",   new MessageTemplateColumnWriter() },
    { "level",     new LevelColumnWriter(true, NpgsqlDbType.Varchar, 50) },
    { "timestamp", new TimestampColumnWriter() },
    { "exception", new ExceptionColumnWriter() },
    { "properties",new PropertiesColumnWriter() }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "logs",
        columnOptions: columnWriters,
        needAutoCreateTable: true,
        schemaName: "public")
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDefaultCors();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<ISmsSender, SmsService>();
builder.Services.AddDbContext<PostgreSqlDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "tr" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});


builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));  
builder.Services.AddTransient<EmailSender, EmailSender>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new SmartEnumJsonConverterFactory());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPasswordHasher<Login>, PasswordHasher<Login>>();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    
    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});

builder.Services.AddRateLimiter(x =>
x.AddFixedWindowLimiter("fixed", cfg =>
{
    cfg.QueueLimit = 100;
    cfg.Window = TimeSpan.FromSeconds(1);
    cfg.PermitLimit = 100;
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);
app.UseCors();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed");

app.UseMiddleware<CultureMiddleware>();
ExtensionsMiddleware.CreateFirstUser(app);

app.Run();
