namespace ParfumeExpressApi.Interfaces
{
    public interface IImageRepository
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task<string> UpdateImageAsync(string oldImagePath, IFormFile newFile);
        Task<bool> DeleteImageAsync(string imagePath);
    }
}
