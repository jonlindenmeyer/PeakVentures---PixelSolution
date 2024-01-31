using AutoFixture;
using KafkaFlow;
using Moq;
using PixelService.Contracts;
using StorageService.Application.Handlers;
using StorageService.Application.Interfaces;

namespace StorageService.UnitTests
{
    [TestClass]
    public class AddVisitorCommandHandlerTests
    {
        private readonly Mock<IVisitorStorage> visitorStorage;
        private readonly AddVisitorCommandHandler handler;
        private readonly Mock<IMessageContext> messageContext;
        private readonly Fixture fixture = new Fixture();

        public AddVisitorCommandHandlerTests()
        {
            this.visitorStorage = new Mock<IVisitorStorage>();
            this.messageContext = new Mock<IMessageContext>();
            this.handler = new AddVisitorCommandHandler(this.visitorStorage.Object);
        }

        [TestMethod]
        public async Task AddVisitorCommandHandler_ShouldBeHandle()
        {
            // Arrange
            var message = new AddVisitorCommand()
            {
                Date = DateTime.Now.ToString(),
                Referrer = this.fixture.Create<string>(),
                IpAddress = this.fixture.Create<string>(),
                UserAgent = this.fixture.Create<string>()
            };

            this.visitorStorage
                .Setup(x => x.InsertVisitorAsync($"{message.Date}|{message.Referrer}|{message.UserAgent}|{message.IpAddress}"))
                .Verifiable();

            // Act
            await this.handler.Handle(this.messageContext.Object, message);

            // Assert
            this.visitorStorage.Verify();
            this.visitorStorage.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task AddVisitorCommandHandler_WhenStorageThrowsException_ShouldBeConsoleLog()
        {
            // Arrange
            var message = new AddVisitorCommand()
            {
                Date = DateTime.Now.ToString(),
                Referrer = this.fixture.Create<string>(),
                IpAddress = this.fixture.Create<string>(),
                UserAgent = this.fixture.Create<string>()
            };

            this.visitorStorage
                .Setup(x => x.InsertVisitorAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception())
                .Verifiable();

            // Act
            await this.handler.Handle(this.messageContext.Object, message);

            // Assert
            this.visitorStorage.Verify();
            this.visitorStorage.VerifyNoOtherCalls();
        }
    }
}