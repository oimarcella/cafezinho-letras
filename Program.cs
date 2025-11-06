using CafezinhoELivrosApi.Data;
using CafezinhoELivrosApi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CafezinhoELivrosAPI.Data.Seeds;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

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
    options.SwaggerDoc("v1", new OpenApiInfo
     {
         Title = "Api do Cafezinho & Livros",
         Version = "1.0"
     });

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<AppDbContext>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    logger.LogInformation("\n ==== Rodando migrações automáticas");
    // Vai tentar executar as migracoes quando o app iniciar
    db.Database.Migrate();
    logger.LogInformation("Finalizada a execução de migrações ==== \n");
}

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