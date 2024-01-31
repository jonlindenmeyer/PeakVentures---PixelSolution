using ProtoBuf;

namespace PixelService.Contracts
{
    // The correct approach for Contracts is generate an nuget package but for this case I will just add a dll reference in the StorageService

    [ProtoContract]
    public class AddVisitorCommand : IRoutable
    {
        [ProtoMember(1)]
        public string Date { get; set; } = string.Empty;

        [ProtoMember(2)]
        public string Referrer { get; set; } = string.Empty;

        [ProtoMember(3)]
        public string UserAgent { get; set; } = string.Empty;

        [ProtoMember(4)]
        public string IpAddress { get; set; } = string.Empty;

        [ProtoIgnore]
        public string RoutingKey => Guid.NewGuid().ToString();
    }
}
