using BlogApi.DataAcess;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using BlogApi.Models.Dtos;
using BlogApi.Validations;
using BlogApi.Repository.Interfaces;
using BlogApi.Repository.Repositories;
using BlogApi.Services.Interfaces;
using BlogApi.Services.Managers;
using BlogApi.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using BlogApi.Auth;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ContextApi>(options =>
    options.UseMySQL(connectionString));

// Añadimos dependencia Jwt
builder.Services.AddSingleton<JwtHelper>();

// Agregar configuración de JWT desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? throw new ArgumentNullException("JWT Secret Key not found.");

// Configurar autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // ✅ Ahora valida el emisor
            ValidateAudience = true, // ✅ Ahora valida la audiencia
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"], // ✅ Se obtiene desde configuración
            ValidAudience = jwtSettings["Audience"], // ✅ Se obtiene desde configuración
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build();
});

// Habilitar CORS si es necesario
// Habilitar CORS correctamente
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") // Especifica el origen de tu frontend
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()); // Permitir credenciales
});



// Configurar la implementacion de FluentValidation
builder.Services.AddScoped<IValidator<PostDto>, PostValidatorDto>();


// Registrar Entidades con su implementacion
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserManager>();

// Registrar AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappersProfile>());



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migración automática de la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ContextApi>();

    // Aplicar migraciones pendientes
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowFrontend"); 

app.UseAuthentication(); // 🔑 Importante para validar JWT
app.UseAuthorization();


app.MapControllers();

app.Run();
