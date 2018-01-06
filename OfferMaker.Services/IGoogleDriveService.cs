namespace OfferMaker.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IGoogleDriveService
    {
        Task<string> UploadFileAsync(IFormFile file);

        Task<byte[]> DownloadFileAsync(string fileId);
    }
}
