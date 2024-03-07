using adaptatechwebapibackend.Middlewares;
using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Filters;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Para evitar, dentro de los Controllers, cuando hacemos consultas de varias tablas (conocidas como join en sql), una referencia infinita entre relaciones
builder.Services.AddControllers(options =>
{
    // Integramos el filtro de excepción para todos los controladores
    options.Filters.Add(typeof(FiltroDeExcepcion));
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);




// Capturamos del app.settings la cadena de conexión a la base de datos
// Configuration.GetConnectionString va directamente a la propiedad ConnectionStrings y de ahí tomamos el valor de DefaultConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");




// Nuestros servicios resolverán dependencias de otras clases
// Registramos en el sistema de inyección de dependencias de la aplicación el ApplicationDbContext
// Conseguimos una instancia o configuración global de la base de datos para todo el proyecto




builder.Services.AddDbContext<AdaptatechContext>(options =>
{
    options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
    // Esta opción deshabilita el tracking a nivel de proyecto (NoTracking).
    // Por defecto siempre hace el tracking. Con esta configuración, no.
    // En cada operación de modificación de datos en los controladores, deberemos habilitar el tracking en cada operación
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);




builder.Services.AddHttpContextAccessor();



builder.Services.AddTransient<GestorArchivosLocal>();
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





// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        // builder.WithOrigins("https://www.almacenjuanluisusuario.com").WithMethods("GET").AllowAnyHeader();
        // builder.WithOrigins("https://www.almacenjuanluisadmin.com").AllowAnyMethod().AllowAnyHeader();
        // builder.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();
        // builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

        /**
         * 
         * Politica CORS acepta todas las peticiones de cualquier IP, par cualquier método, con cualquier cabecera.
         * 
         **/

        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();


    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
});

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI();
    }


app.UseFileServer(); // Para que me aparezcan las imagenes en la pantalla web

app.UseHttpsRedirection();

app.UseMiddleware<AccesoIPMiddleware>();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

