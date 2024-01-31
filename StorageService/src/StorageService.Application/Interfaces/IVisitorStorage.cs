namespace StorageService.Application.Interfaces
{
    public interface IVisitorStorage
    {
        public Task InsertVisitorAsync(string visitor);
    }
}
