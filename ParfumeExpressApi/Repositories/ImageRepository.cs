using ParfumeExpressApi.Interfaces;

namespace ParfumeExpressApi.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _env;

        public ImageRepository(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath)) return false;

                var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/{fileName}"; // Return relative path
        }

        public async Task<string> UpdateImageAsync(string oldImagePath, IFormFile newFile)
        {
            // Delete the old image if it exists
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                var oldImageFullPath = Path.Combine(_env.WebRootPath, oldImagePath.TrimStart('/'));
                if (File.Exists(oldImageFullPath))
                {
                    File.Delete(oldImageFullPath);
                }
            }

            // Save the new image
            return await SaveImageAsync(newFile);
        }
    }
}
