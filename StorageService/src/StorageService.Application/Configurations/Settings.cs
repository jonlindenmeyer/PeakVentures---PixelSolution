namespace StorageService.Application.Configurations
{
    public class Settings : ISettings
    {
        public string FilePath { get; set; } = string.Empty;

        public KafkaSettings Kafka { get; set; }
    }
}
