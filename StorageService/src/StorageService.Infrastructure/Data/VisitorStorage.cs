using StorageService.Application.Configurations;
using StorageService.Application.Interfaces;

namespace StorageService.Infrastructure.Data
{
    public class VisitorStorage : IVisitorStorage
    {
        private readonly ISettings settings;
        public VisitorStorage(ISettings settings)
        {
            this.settings = settings;
        }

        public async Task InsertVisitorAsync(string visitor)
        {
            try
            {
                var path = this.settings.FilePath;

                if (!File.Exists(path))
                {
                    var splited = path.Split("/");
                    var dir = path.Split(splited[splited.Length-1]);
                    if (!Directory.Exists(dir[0]))
                    {
                        Directory.CreateDirectory(dir[0]);
                    }
                }

                using (var writer = new StreamWriter(path, append: true))
                {
                   await writer.WriteLineAsync(visitor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
