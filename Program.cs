using ConfiguracaoPorAmbienteAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// O .NET Core carrega automaticamente:
// 1. appsettings.json
// 2. appsettings.{Environment}.json (baseado na variável ASPNETCORE_ENVIRONMENT)
// Onde {Environment} pode ser Development, Staging, Production, etc.

// Configurando serviços para injeção de dependência
// Vinculando as seções do arquivo de configuração às classes de configuração
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.Configure<ExternalServiceSettings>(
    builder.Configuration.GetSection(nameof(ExternalServiceSettings)));

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Exibindo informações do ambiente atual
app.MapGet("/ambiente", (IWebHostEnvironment env) => 
{
    return Results.Ok(new
    {
        AmbienteAtual = env.EnvironmentName,
        IsDevelopment = env.IsDevelopment(),
        IsStaging = env.IsStaging(),
        IsProduction = env.IsProduction()
    });
});

// Endpoint para mostrar as configurações de banco de dados do ambiente atual
app.MapGet("/config/database", (IOptions<DatabaseSettings> dbSettings) =>
{
    return Results.Ok(dbSettings.Value);
});

// Endpoint para mostrar as configurações de serviços externos do ambiente atual
app.MapGet("/config/external-service", (IOptions<ExternalServiceSettings> svcSettings) =>
{
    return Results.Ok(svcSettings.Value);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Configurações específicas para ambiente de desenvolvimento
    app.Logger.LogInformation("Aplicação rodando em ambiente de DESENVOLVIMENTO");
}
else if (app.Environment.IsStaging())
{
    // Configurações específicas para ambiente de qualidade/staging
    app.Logger.LogInformation("Aplicação rodando em ambiente de QUALIDADE");
}
else
{
    // Configurações específicas para ambiente de produção
    app.Logger.LogInformation("Aplicação rodando em ambiente de PRODUÇÃO");
}

app.UseHttpsRedirection();

app.Run();
