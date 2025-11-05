using CafezinhoELivrosApi.Data;
using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CafezinhoELivrosAPI.Data.Seeds;


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
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Configurando conexao ao banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

builder.Services.AddIdentity<User, Role>()
       .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

//builder.Services.AddHostedService<SeedHostedService>(); // Tentativa de executar seeds de roles e admin user ao iniciar código

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();   // valida o token/cookie
app.UseAuthorization(); // checa permissoes

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi(); //Não vou usar da openapi
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();