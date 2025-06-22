using ConfiguracaoPorAmbienteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfiguracaoPorAmbienteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly DatabaseSettings _dbSettings;
        private readonly ExternalServiceSettings _externalSettings;
        private readonly IConfiguration _configuration;

        public ConfigController(
            IWebHostEnvironment environment,
            IOptions<DatabaseSettings> dbSettings,
            IOptions<ExternalServiceSettings> externalSettings,
            IConfiguration configuration)
        {
            _environment = environment;
            _dbSettings = dbSettings.Value;
            _externalSettings = externalSettings.Value;
            _configuration = configuration;
        }

        [HttpGet("ambiente")]
        public IActionResult GetAmbiente()
        {
            return Ok(new
            {
                AmbienteAtual = _environment.EnvironmentName,
                IsDevelopment = _environment.IsDevelopment(),
                IsStaging = _environment.IsStaging(),
                IsProduction = _environment.IsProduction()
            });
        }

        [HttpGet("database")]
        public IActionResult GetDatabaseSettings()
        {
            // Aqui temos acesso às configurações de banco de dados específicas do ambiente atual
            return Ok(_dbSettings);
        }

        [HttpGet("external-service")]
        public IActionResult GetExternalServiceSettings()
        {
            // Aqui temos acesso às configurações de serviço externo específicas do ambiente atual
            return Ok(_externalSettings);
        }

        [HttpGet("raw")]
        public IActionResult GetRawConfig(string section)
        {
            // Método para acessar qualquer seção de configuração diretamente
            var configSection = _configuration.GetSection(section);
            if (!configSection.Exists())
                return NotFound($"Seção de configuração '{section}' não encontrada");

            return Ok(configSection.Get<object>());
        }
    }
}
