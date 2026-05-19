using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using SIS.Application.Services.Interfaces;
using SIS.Domain.Common.Interfaces;
using SIS.Infrastructure;
using SIS.Infrastructure.Persistence.Contexts;
using SIS.Infrastructure.Repositories;
using SIS.Infrastructure.Seeds;
using SIS.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<SIS.Application.Validators.StudentCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SIS.Application.Validators.StudentUpdateDtoValidator>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Identity DbContext using seperated database
//
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(SIS.Application.Mapping.StudentProfile).Assembly));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// You don't necessarily need to register IStudentRepository if you access it via UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register MediatR (Points to the Application layer)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(SIS.Application.DTOs.StudentListDto).Assembly));

// F. Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// 1. Add the built-in OpenAPI service
// 2. OpenAPI with Security Requirements
builder.Services.AddOpenApi(options =>
{
    _ = options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Initialize Components
        document.Components ??= new OpenApiComponents();

        // 2. Initialize the Dictionary using the INTERFACE type (IOpenApiSecurityScheme)
        // This fixes error CS0019
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        const string schemeId = "Bearer";

        // Use the interface type for the variable to ensure compatibility
        IOpenApiSecurityScheme jwtScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token (e.g., 12345abcdef)."
        };

        // 3. Add the scheme
        if (!document.Components.SecuritySchemes.ContainsKey(schemeId))
        {
            document.Components.SecuritySchemes.Add(schemeId, jwtScheme);
        }

        // 4. Initialize Security list if null
        document.Security ??= new List<OpenApiSecurityRequirement>();

        // Initialize SecurityRequirement using the interface list if needed
        var securityRequirement = new OpenApiSecurityRequirement();

        // In v3.x, use the specialized Reference class
        var schemeReference = new OpenApiSecuritySchemeReference(schemeId, document);

        securityRequirement.Add(schemeReference, new List<string>());

        document.Security.Add(securityRequirement);

        return Task.CompletedTask;
    });
});

// 2. Map the endpoint in the middleware section
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    // Generates the JSON document at /openapi/v1.json
    app.MapOpenApi();

    // Adds a beautiful UI at /scalar/v1
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeed.SeedRolesAndAdmin(userManager, roleManager);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
