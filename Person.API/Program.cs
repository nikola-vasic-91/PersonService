using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PersonService.API.Helpers;
using PersonService.Domain.DtoModels;
using PersonService.Domain.Interfaces;
using PersonService.Domain.Models;
using PersonService.Domain.Modules;
using PersonService.Domain.Validators;
using PersonService.Infrastructure.Contexts;
using PersonService.Infrastructure.Repositories;
using Serilog;
using System.Data.SqlClient;
using System.Reflection;
using static PersonService.Service.Modules.ServiceModule;
using static PersonService.API.Constants.OtherConstants;
using PersonService.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sg =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        sg.IncludeXmlComments(xmlPath);
        sg.OperationFilter<SwaggerXCorrelationIdFilter>();
    }
);

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(nameof(DatabaseOptions)));
builder.Services.Configure<ServiceUrls>(builder.Configuration.GetSection(nameof(ServiceUrls)));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
builder.Services.AddScoped(typeof(IDataRepository<Person>), typeof(PersonRepository));
builder.Services.AddScoped<IValidator<PersonDto>, PersonValidator>();
builder.Services.AddDbContext<PersonDbContext>((serviceProvider, options) =>
               options.UseSqlServer(
                    new SqlConnection(serviceProvider.GetService<IOptions<DatabaseOptions>>()?.Value?.ConnectionString)));

builder.Services.RegisterMediatR();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyName,
                      policy =>
                      {
                          policy.WithOrigins(builder.Services.BuildServiceProvider().GetService<IOptions<ServiceUrls>>()?.Value?.PersonPortalUrl)
                          .WithHeaders(CorrelationIdHeaderName, ContentTypeHeaderName);
                      });
});

builder.Services.AddMvc(options =>
{
    options.Filters.Add<OperationCancelledExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(CorsPolicyName);

app.MapControllers();

app.Run();