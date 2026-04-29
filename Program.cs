using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StockFlow.API.Data;
using StockFlow.API.Interfaces;
using StockFlow.API.Middleware;
using StockFlow.API.Services;
using StockFlow.API.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CONTROLLERS
builder.Services.AddControllers();


// DATABASE (SQL SERVER)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// SERVICES (INJEÇÃO DE DEPENDÊNCIA)
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();

// AUDITORIA (ESSENCIAL)
builder.Services.AddScoped<IAuditService, AuditService>();


// CURRENT USER (PEGAR USUÁRIO LOGADO)
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// FLUENT VALIDATION
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();


// JWT AUTHENTICATION
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


// AUTHORIZATION (POLICIES)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DashboardAccess", policy =>
        policy.RequireRole("Owner", "Admin", "Manager"));

    options.AddPolicy("ReportsAccess", policy =>
        policy.RequireRole("Owner", "Admin", "Manager"));

    options.AddPolicy("StockMovementWrite", policy =>
        policy.RequireRole("Owner", "Admin", "Manager", "Stocker"));

    options.AddPolicy("StockMovementRead", policy =>
        policy.RequireRole("Owner", "Admin", "Manager"));

    options.AddPolicy("SalesAccess", policy =>
        policy.RequireRole("Owner", "Admin", "Cashier"));

    options.AddPolicy("SupplierWrite", policy =>
        policy.RequireRole("Admin", "Manager"));

    options.AddPolicy("SupplierDelete", policy =>
        policy.RequireRole("Admin"));
});


// SWAGGER

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // GRUPOS 
    options.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth", Version = "v1" });
    options.SwaggerDoc("AuditLog", new OpenApiInfo { Title = "AuditLog", Version = "v1" });
    options.SwaggerDoc("Products", new OpenApiInfo { Title = "Products", Version = "v1" });
    options.SwaggerDoc("Categories", new OpenApiInfo { Title = "Categories", Version = "v1" });
    options.SwaggerDoc("Dashboard", new OpenApiInfo { Title = "Dashboard", Version = "v1" });
    options.SwaggerDoc("Reports", new OpenApiInfo { Title = "Reports", Version = "v1" });
    options.SwaggerDoc("StockMovements", new OpenApiInfo { Title = "StockMovements", Version = "v1" });
    options.SwaggerDoc("Suppliers", new OpenApiInfo { Title = "Suppliers", Version = "v1" });
    options.SwaggerDoc("Test", new OpenApiInfo { Title = "Test", Version = "v1" });

    // BOTÃO AUTHORIZE (JWT)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // LIGA CONTROLLER AO GRUPO
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.GroupName == docName;
    });
});


// BUILD APP
var app = builder.Build();


// MIDDLEWARE PIPELINE

// Middleware global de exceção 
app.UseMiddleware<ExceptionMiddleware>();

// HTTPS
app.UseHttpsRedirection();

// AUTENTICAÇÃO 
app.UseAuthentication();
app.UseAuthorization();

// SWAGGER 
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/Auth/swagger.json", "Auth");
    options.SwaggerEndpoint("/swagger/AuditLog/swagger.json", "AuditLog");
    options.SwaggerEndpoint("/swagger/Products/swagger.json", "Products");
    options.SwaggerEndpoint("/swagger/Categories/swagger.json", "Categories");
    options.SwaggerEndpoint("/swagger/Dashboard/swagger.json", "Dashboard");
    options.SwaggerEndpoint("/swagger/Reports/swagger.json", "Reports");
    options.SwaggerEndpoint("/swagger/StockMovements/swagger.json", "StockMovements");
    options.SwaggerEndpoint("/swagger/Suppliers/swagger.json", "Suppliers");
    options.SwaggerEndpoint("/swagger/Test/swagger.json", "Test");

    options.DisplayRequestDuration();
});


// MAP CONTROLLERS
app.MapControllers();

app.Run();