using AutoFixture;
using Moq;
using StorageService.Application.Configurations;
using StorageService.Infrastructure.Data;

namespace StorageService.UnitTests
{
    [TestClass]
    public class VisitorStorageTests
    {
        private string path = $"{Directory.GetCurrentDirectory()}/tests/visitors.log";
        private readonly VisitorStorage visitorStorage;
        private readonly Mock<ISettings> settings;
        private readonly Fixture fixture = new Fixture();


        public VisitorStorageTests()
        {
            this.settings = new Mock<ISettings>();
            this.settings
                .SetupGet(s => s.FilePath)
                .Returns(path);

            this.visitorStorage = new VisitorStorage(this.settings.Object);
        }

        [TestCleanup] 
        public void Cleanup()
        {
            if (path != string.Empty && File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public async Task InsertVisitorAsync_WhenFileAndDirNotExist_ShouldBeCreateAndInsert()
        {
            // Arrange
            var visitor = this.fixture.Create<string>();

            // Act
            await this.visitorStorage.InsertVisitorAsync(visitor);

            // Assert
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(File.ReadAllText(path).Contains(visitor));
        }

        [TestMethod]
        public async Task InsertVisitorAsync_WhenDirExist_ShouldBeCreateAndInsert()
        {
            // Arrange
            var visitor = this.fixture.Create<string>();
            var dir = $"{Directory.GetCurrentDirectory()}/tests/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // Act
            await this.visitorStorage.InsertVisitorAsync(visitor);

            // Assert
            Assert.IsTrue(File.ReadAllText(path).Contains(visitor));
        }

        [TestMethod]
        public async Task InsertVisitorAsync_WhenPathIsInvalid_ShouldBeThrowException()
        {
            // Arrange
            var visitor = this.fixture.Create<string>();

            this.settings
                .SetupGet(s => s.FilePath)
                .Returns(string.Empty);

            // Act
            var result = await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => this.visitorStorage.InsertVisitorAsync(visitor));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is Exception);
        }
    }
}
