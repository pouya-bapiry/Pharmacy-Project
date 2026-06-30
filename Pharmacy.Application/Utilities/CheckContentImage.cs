using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;


namespace Pharmacy.Application.Utilities
{
    public static class CheckContentImage
    {
        private static readonly string[] AllowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".bmp",
            ".gif"
        };

        public static bool IsImage(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // بررسی پسوند
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                return false;

            try
            {
                // بررسی واقعی بودن فایل
                using var stream = file.OpenReadStream();

                var info = Image.Identify(stream);

                return info != null;
            }
            catch
            {
                return false;
            }
        }
    }
}