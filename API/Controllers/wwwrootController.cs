using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WWWrootController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WWWrootController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetVideos()
        {
            try
            {
                string videosPath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "game videos", "static");

                // Отримання всіх файлів в директорії "videosPath" з розширенням ".mp4"
                string[] videoFiles = Directory.GetFiles(videosPath, "*.mp4", SearchOption.AllDirectories);

                // Конвертування абсолютних шляхів файлів в URL-адреси
                var videoUrls = videoFiles.Select(GetUrlFromPath);

                return Ok(videoUrls);
            }
            catch (Exception ex)
            {
                // Обробка винятку та повернення відповідної помилки
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GetUrlFromPath(string filePath)
        {
            // Відносний шлях від кореня веб-додатка
            string relativePath = filePath.Replace(_webHostEnvironment.WebRootPath, "").Replace("\\", "/").TrimStart('/');

            // Формування URL-адреси
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            string videoUrl = $"{baseUrl}/{relativePath}";

            return videoUrl;
        }
    }
}
