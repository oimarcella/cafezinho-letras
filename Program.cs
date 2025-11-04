using CafezinhoELivrosApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();//Não vou usar da openapi

// CORS liberado
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.WebHost.UseUrls("http://+:5027");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Para o Swagger descobrir os endpoints
builder.Services.AddSwaggerGen();

// Configurando conexao ao banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))
    ));

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
//app.UseAuthentication();   // valida o token/cookie
//app.UseAuthorization(); // checa permissoes

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi(); //Não vou usar da openapi
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Rotas

app.Run();