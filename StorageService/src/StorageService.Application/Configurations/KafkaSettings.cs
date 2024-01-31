namespace StorageService.Application.Configurations
{
    public class KafkaSettings
    {
        public IEnumerable<string> Brokers { get; set; } = new List<string>();

        public string Topic { get; set; } = string.Empty;

        public string ConsumerGroup { get; set; } = string.Empty;
    }
}
