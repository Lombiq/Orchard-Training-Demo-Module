/*
 * This simple controller serves as a demonstration for file management inside Orchard.
 */


using Orchard;
using Orchard.Exceptions;
using Orchard.FileSystems.Media;
using Orchard.Localization;
using System;
using System.IO;
using System.Web.Mvc;

namespace OrchardHUN.TrainingDemo.Controllers
{
    public class FileManagementController : Controller
    {
        /*
         * There are several services in Ochard that provide an abstraction to file system access. You should generally 
         * use these instead of directly accessing the file system through the .NET libraries as because of the variety
         * of hosting services direct file access is not always possible (e.g. media files can be outsourced to an 
         * external storage provider like an Amazon cloud service or an Azure Blob Storage: you can't access files stored 
         * there directly); these services however give you a consistent API, regardless of the implementation.
         * 
         * Orchard file system access services:
         * *    IStorageProvider: we'll use this here as this is the most important probably. With this service you can 
         *      access files stored in the Media folder of the tenant (each tenant has its own folder in the Media 
         *      directory). The Media directory is accessible to anyone!
         *      Also keep in mind that the Media folder's content is accessible from the dashboard through the Media module
         *      (http://docs.orchardproject.net/Documentation/Adding-and-managing-media-content) so your users are able 
         *      to directly access them.
         *      However a hidden, "technical" media folder is under consideration.
         * *    IMediaService: a higher level service to access the Media folder as well, but it also contains additional 
         *      functionality to lower-level file access e.g. for handling uploaded files. It also honours the settings 
         *      configured under Media settings on the dashboard.
         * *    IVirtualPathProvider: this is the service for accessing files outside the Media folder, like any file in 
         *      modules' directories.
         *      Use this only if you want to have the lowest level file access that most likely corresponds to the local 
         *      file system (in contrary to the two other services that, as mentioned, can point to external file systems).
         */
        private readonly IStorageProvider _storageProvider;

        private const string DemoFolderPath = "DemoFolder/";
        private const string DemoFile1Path = DemoFolderPath + "MyFile.txt";
        private const string DemoFile2Path = DemoFolderPath + "MyFile2.txt";

        public Localizer T { get; set; }


        public FileManagementController(IStorageProvider storageProvider) => _storageProvider = storageProvider;


        // Creating/writing some files. Don't forget to check them out after running this action in the Media folder!
        // You can access this action under ~/OrchardHUN.TrainingDemo/FileManagement/Create
        public string Create()
        {
            // Brace yourself, simple examples are coming!

            try
            {
                // The paths used as arguments here are relative to the tenant's media folder: this translates to
                // ~/Media/TenantName/DemoFolder/MyFile.txt where TenantName is the tenant's technical name ("Default" 
                // if for the first tenant).
                IStorageFile file;
                if (!_storageProvider.FileExists(DemoFile1Path))
                {
                    file = _storageProvider.CreateFile(DemoFile1Path);
                }
                else
                {
                    file = _storageProvider.GetFile(DemoFile1Path);
                }
                // Notice that the DemoFolder folder is implicitly created if it doesn't exist but we could explicitly
                // create folder with _storageProvider.CreateFolder("FolderPath");

                // Simple interface for writing...
                using (var stream = file.OpenWrite())
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write("Hello there!");
                }

                // Equivalent shorthand for creating a file. SaveStream() will throw an exception if the file exists.
                using (var stream = new MemoryStream())
                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write("Hello there!");
                    _storageProvider.SaveStream(DemoFile2Path, stream);
                    // SaveStream has a counterpart, TrySaveStream that returns a bool indicating whether the operation
                    // was successful or not, it doesn't throw an exception if the file exists. However internally it
                    // also catches exceptions...
                }

                // ...and equally for reading.
                // OpenRead() throws an exception if the file doesn't exist.
                using (var stream = file.OpenRead())
                using (var streamReader = new StreamReader(stream))
                {
                    var content = streamReader.ReadToEnd();
                    // If you don't have to choose a specific exception class for throwing and exception, you should use
                    // OrchardException, for its message is localizable.
                    if (!content.Equals("Hello there!")) throw new OrchardException(T("Well, this is awkward."));
                }

                // Renaming is nothing special.
                _storageProvider.RenameFile(DemoFile2Path, DemoFolderPath + "HiddenSecretDontOpen.txt");

                // Fetching the public URL of the file. We can use this to access the file from the outside world, e.g.
                // from a link.
                return _storageProvider.GetPublicUrl(DemoFile1Path);
            }
            // Sometimes we can't know what type of exception a service can throw so the best we can do is catch
            // Exception and then check whether it's fatal. For clarification see:
            // http://english.orchardproject.hu/blog/orchard-gems-exception-fatality-check
            catch (Exception ex) when (!ex.IsFatal())
            {
                return "Something went terribly wrong: " + ex.Message;
            }
        }

        // Use this action to remove everything created with Create() so you can run the latter again.
        public string CleanUp()
        {
            // Deleting the folder also deletes every file inside it, surprisingly :-).
            if (_storageProvider.FolderExists(DemoFolderPath))
            {
                _storageProvider.DeleteFolder(DemoFolderPath);
            }

            // But we could also delete individual files of course.
            //_storageProvider.DeleteFile(DemoFile1Path);

            return "Everything OK";
        }
    }

    // NEXT STATION: Services/BackgroundTask.cs
}