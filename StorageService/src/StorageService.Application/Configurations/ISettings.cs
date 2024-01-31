namespace StorageService.Application.Configurations
{
    public interface ISettings
    {
        public string FilePath { get; set; }

        public KafkaSettings Kafka { get; set; }
    }
}
