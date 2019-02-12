/*
 * 
 */

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lombiq.TrainingDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.FileStorage;
using OrchardCore.Media;

namespace Lombiq.TrainingDemo.Controllers
{
    public class FileManagementController : Controller
    {
        private const string TestFileRelativePath = "TrainingDemo/TestFile1.txt";
        private const string UploadedFileFolderRelativePath = "TrainingDemo/Uploaded";


        private readonly IMediaFileStore _mediaFileStore;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;
        private readonly ICustomFileStore _customFileStore;


        public FileManagementController(
            IMediaFileStore mediaFileStore,
            INotifier notifier,
            IHtmlLocalizer<FileManagementController> htmlLocalizer,
            ICustomFileStore customFileStore)
        {
            _mediaFileStore = mediaFileStore;
            _notifier = notifier;
            _customFileStore = customFileStore;
            H = htmlLocalizer;
        }


        public async Task<string> CreateFileInMediaFolder()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hi there!")))
            {
                await _mediaFileStore.CreateFileFromStream(TestFileRelativePath, stream, true);
            }

            var fileInfo = await _mediaFileStore.GetFileInfoAsync(TestFileRelativePath);

            var publicUrl = _mediaFileStore.MapPathToPublicUrl(TestFileRelativePath);

            return $"Successfully created file! File size: {fileInfo.Length} bytes. Public URL: {publicUrl}";
        }

        public async Task<string> ReadFileFromMediaFolder()
        {
            if (await _mediaFileStore.GetFileInfoAsync(TestFileRelativePath) == null)
            {
                return "Create the file first!";
            }

            using (var stream = await _mediaFileStore.GetFileStreamAsync(TestFileRelativePath))
            using (var streamReader = new StreamReader(stream))
            {
                var content = streamReader.ReadToEnd();

                return $"File content: {content}";
            }
        }

        public ActionResult UploadFileToMedia() => View();

        [HttpPost, ActionName(nameof(UploadFileToMedia))]
        public async Task<ActionResult> UploadFileToMediaPost(IFormFile file)
        {
            var mediaFilePath = _mediaFileStore.Combine(UploadedFileFolderRelativePath, file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await _mediaFileStore.CreateFileFromStream(mediaFilePath, stream);
            }

            _notifier.Information(H["Successfully uploaded file!"]);

            return RedirectToAction(nameof(UploadFileToMedia));
        }

        public async Task<string> CreateFileInCustomFolder()
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("Hi there in the custom file storage!")))
            {
                await _customFileStore.CreateFileFromStream(TestFileRelativePath, stream, true);
            }

            var fileInfo = await _customFileStore.GetFileInfoAsync(TestFileRelativePath);

            return $"Successfully created file in the custom file storage! File size: {fileInfo.Length} bytes.";
        }
    }
}