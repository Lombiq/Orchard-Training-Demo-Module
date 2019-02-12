using OrchardCore.FileStorage;
using OrchardCore.FileStorage.FileSystem;

namespace Lombiq.TrainingDemo.Services
{
    public interface ICustomFileStore : IFileStore
    {
    }

    public class CustomFileStore : FileSystemStore, ICustomFileStore
    {
        public CustomFileStore(string fileSystemPath)
            : base(fileSystemPath)
        {
        }
    }
}