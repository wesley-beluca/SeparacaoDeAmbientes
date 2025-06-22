namespace ConfiguracaoPorAmbienteAPI.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int MaxRetryCount { get; set; }
        public int CommandTimeout { get; set; }
    }
}
