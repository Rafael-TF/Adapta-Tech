using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Para evitar, dentro de los Controllers, cuando hacemos consultas de varias tablas (conocidas como join en sql), una referencia infinita entre relaciones
builder.Services.AddControllers(options =>
{
    // Integramos el filtro de excepción para todos los controladores
    //options.Filters.Add<FiltroDeExcepcion>();
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Capturamos del app.settings la cadena de conexión a la base de datos
// Configuration.GetConnectionString va directamente a la propiedad ConnectionStrings y de ahí tomamos el valor de DefaultConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Nuestros servicios resolverán dependencias de otras clases
// Registramos en el sistema de inyección de dependencias de la aplicación el ApplicationDbContext
// Conseguimos una instancia o configuración global de la base de datos para todo el proyecto
builder.Services.AddDbContext<AdaptatechContext>(options =>
{
    options.UseSqlServer(connectionString);
    // Esta opción deshabilita el tracking a nivel de proyecto (NoTracking).
    // Por defecto siempre hace el tracking. Con esta configuración, no.
    // En cada operación de modificación de datos en los controladores, deberemos habilitar el tracking en cada operación
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<OperacionesService>();
builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddHostedService<RegistroOperacionesService>();

var clave = builder.Configuration["ClaveJWT"];
// ----------------- TOKEN --------------------
// Configuramos la seguridad en el proyecto. Manifestamos que se va a implementar la seguridad
// mediante JWT firmados por la firma que está en el app.settings.development.json con el nombre ClaveJWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false, // Esto es porque en el GenerarToken se puede poner el issuer: "Persona que lo envía" (Es opcional ponerlo en el GenerarToken)
                   ValidateAudience = false, // Esto es porque en el GenerarToken se puede poner el audience: "Quienes lo reciben" (Es opcional ponerlo en el GenerarToken)
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(clave))
               });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

