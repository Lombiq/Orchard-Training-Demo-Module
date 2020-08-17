using OrchardCore.FileStorage;
using OrchardCore.FileStorage.FileSystem;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.TrainingDemo.Services
{
    // You have multiple ways to use the FileSystemStore service. The one demonstrated here is to simply inherit our
    // service from the FileSystemStore. And also inherit our service from IFileStore. The other way is to "decorate"
    // the IFileStore which means that it will be an injected service inside our implementation. Find MediaFileStore.cs
    // to see how that way is done.
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "It's a simple sample.")]
    public interface ICustomFileStore : IFileStore
    {
        // You can add additional methods if you want.
    }


    public class CustomFileStore : FileSystemStore, ICustomFileStore
    {
        // Since FileSystemStore requires a base path we also need to have it. If you have a very specific absolute
        // path then you don't need it to be injected but for demonstration purposes we'll inject it from Startup.cs
        // because it will be in the tenant's folder.
        public CustomFileStore(string fileSystemPath)
            : base(fileSystemPath)
        {
        }
    }
}

// NEXT STATION: Startup.cs and find the File System comment line in the ConfigureServices method.
