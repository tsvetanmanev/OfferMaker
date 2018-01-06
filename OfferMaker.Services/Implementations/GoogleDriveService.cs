namespace OfferMaker.Services.Implementations
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Download;
    using Google.Apis.Drive.v3;
    using Google.Apis.Services;
    using Google.Apis.Util.Store;
    using System.IO;
    using System.Threading;
    using System;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    public class GoogleDriveService : IGoogleDriveService
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "OfferMaker";

        private string folderId;
        private DriveService driveService;

        public GoogleDriveService()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            this.driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Get the App Folder to Store OfferMaker Files
            var searchRequest = driveService.Files.List();
            searchRequest.Q = $"mimeType='application/vnd.google-apps.folder' and name='{ApplicationName}'";
            searchRequest.Fields = "files(id)";
            var result = searchRequest.Execute();
            folderId = result.Files.First().Id;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
                Parents = new List<string>
                        {
                            folderId
                        }
            };
            FilesResource.CreateMediaUpload request;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                request = this.driveService.Files.Create(
                    fileMetadata, stream, "application/pdf");
                request.Fields = "id";
                request.Upload();
            }

            var uploadedFile = request.ResponseBody;
            return uploadedFile.Id;
        }

        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            var request = this.driveService.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            await request.DownloadAsync(stream);

            var resultFile = stream.ToArray();

            return resultFile;
        }
    }
}
