using KafkaFlow;
using PixelService.Contracts;
using StorageService.Application.Interfaces;

namespace StorageService.Application.Handlers
{
    public class AddVisitorCommandHandler : IMessageHandler<AddVisitorCommand>
    {
        private readonly IVisitorStorage visitorStorage;

        public AddVisitorCommandHandler(IVisitorStorage visitorStorage)
        {
            this.visitorStorage = visitorStorage;
        }

        public async Task Handle(IMessageContext context, AddVisitorCommand message)
        {
            try
            {
                var visitor = $"{message.Date}|{message.Referrer}|{message.UserAgent}|{message.IpAddress}";

                await this.visitorStorage.InsertVisitorAsync(visitor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
