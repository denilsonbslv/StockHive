using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockHive.Configure;
using StockHive.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// --- Configura��o do Banco de Dados ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- Adiciona os Controllers e o AutoMapper ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Esta configuração crucial diz ao serializador para não quebrar
    // quando encontrar um loop de referência (Pai -> Filho -> Pai).
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddAutoMapper(typeof(Program));

// --- Configura��o do Versionamento (Correto como estava) ---
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ======================= CONFIGURA��O DO SWAGGER CORRIGIDA =======================

// 1. Registra nossa classe auxiliar na Inje��o de Depend�ncia
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// 2. Adiciona o SwaggerGen. Ele vai usar nossa classe auxiliar automaticamente.
builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments for Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // A configura��o de seguran�a para JWT (Bearer) que voc� j� tinha. Perfeito!
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor, insira o token JWT com Bearer no campo",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // ======================= CONFIGURA��O DO SWAGGER UI CORRIGIDA =======================
    // Precisamos dizer ao Swagger UI para criar um endpoint para cada vers�o da API

    // Pegamos o provedor de descri��o de vers�o da API que foi registrado
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        // Cria um endpoint na UI para cada vers�o descoberta
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();