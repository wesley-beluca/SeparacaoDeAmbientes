namespace ConfiguracaoPorAmbienteAPI.Models
{
    public class ExternalServiceSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; }
    }
}
