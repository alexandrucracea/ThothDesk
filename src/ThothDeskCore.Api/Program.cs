using FluentValidation;
using FluentValidation.AspNetCore;
using ThothDeskCore.Api.Extensions;
using ThothDeskCore.Api.Services;
using ThothDeskCore.Api.Services.Interfaces;
using ThothDeskCore.Api.Validation;
using ThothDeskCore.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("SqlServer");
if (string.IsNullOrWhiteSpace(connStr))
    throw new InvalidOperationException("Connection string 'SqlServer' not found.");

// Add services to the container.
builder.Services
    .AddDataBase(builder.Configuration)
    .AddIdentityAndJwt(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
//builder.Services.AddScoped<IAssignmentService, AssignmentService>();


//TODO automate api versioning
//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.DefaultApiVersion = new(1, 0);
//    options.ReportApiVersions = true;
//});

//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

//var apiPrefix = builder.Configuration["Api:Prefix"] ?? "thothdesk";

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateAssignmentRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks().AddSqlServer(connStr);


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks("/health");

app.MapControllers();

app.Run();
