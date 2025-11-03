using api_inmobiliaria.Interfaces;
using api_inmobiliaria.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = builder.Configuration;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["TokenAuthentication:Issuer"],
            ValidAudience = configuration["TokenAuthentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                configuration["TokenAuthentication:SecretKey"]!)),
        };
    })
;

builder.Services.AddScoped<ITokenService, TokenService>();

if (builder.Configuration.GetValue<bool>("UsarEntityFramework"))
{
    builder.Services.AddScoped<IPropietarioRepository, api_inmobiliaria.Repositories.EntityFramework.PropietarioRepository>();
    builder.Services.AddScoped<IInmuebleRepository, api_inmobiliaria.Repositories.EntityFramework.InmuebleRepository>();
    builder.Services.AddScoped<IInquilinoRepository, api_inmobiliaria.Repositories.EntityFramework.InquilinoRepository>();
    builder.Services.AddScoped<IContratoRepository, api_inmobiliaria.Repositories.EntityFramework.ContratoRepository>();
    builder.Services.AddScoped<IPagoRepository, api_inmobiliaria.Repositories.EntityFramework.PagoRepository>();
    builder.Services.AddScoped<ITipoInmuebleRepository, api_inmobiliaria.Repositories.EntityFramework.TipoInmuebleRepository>();

    builder.Services.AddDbContext<api_inmobiliaria.Repositories.EntityFramework.BDContext>(
        dbContextOptions => dbContextOptions
            .UseMySql(configuration["ConnectionStrings:EFMySql"], new MySqlServerVersion(new Version(80, 0, 43)))
    );
}
else
{
    builder.Services.AddScoped<IPropietarioRepository, api_inmobiliaria.Repositories.MySql.PropietarioRepository>();
    builder.Services.AddScoped<IInmuebleRepository, api_inmobiliaria.Repositories.MySql.InmuebleRepository>();
    builder.Services.AddScoped<IInquilinoRepository, api_inmobiliaria.Repositories.MySql.InquilinoRepository>();
    builder.Services.AddScoped<IContratoRepository, api_inmobiliaria.Repositories.MySql.ContratoRepository>();
    builder.Services.AddScoped<IPagoRepository, api_inmobiliaria.Repositories.MySql.PagoRepository>();
    builder.Services.AddScoped<ITipoInmuebleRepository, api_inmobiliaria.Repositories.MySql.TipoInmuebleRepository>();
}

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
    
// Uso de archivos estáticos (*.html, *.css, *.js, etc.)
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
