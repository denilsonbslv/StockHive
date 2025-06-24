// using statements necessários que podem estar faltando
using Microsoft.EntityFrameworkCore;
using StockHive.Data;

var builder = WebApplication.CreateBuilder(args);

// ======================= INÍCIO DA CONFIGURAÇÃO DO BANCO DE DADOS =======================

// 1. CORRIGIDO: Ler a connection string e GUARDAR em uma variável.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ADICIONADO: Registrar o AppDbContext nos serviços da aplicação.
//    Isso "ensina" a API a como se conectar ao banco usando a string acima.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ======================= FIM DA CONFIGURAÇÃO DO BANCO DE DADOS =======================


// Add services to the container.
builder.Services.AddControllers();
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