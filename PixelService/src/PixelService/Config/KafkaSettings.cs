namespace PixelService.Config
{
    public class KafkaSettings
    {
        public IEnumerable<string> Brokers { get; set; } = new List<string>();

        public string DefaultTopic { get; set; } = string.Empty;

        public string ProducerName { get; set; } = string.Empty;
    }
}
