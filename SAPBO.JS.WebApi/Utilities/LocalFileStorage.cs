namespace SAPBO.JS.WebApi.Utilities
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> SaveFile(string path, IFormFile file, string newFileName)
        {
            var fullPath = Path.Combine(path, newFileName + Path.GetExtension(file.FileName));
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(fullPath, content);
            }
            return fullPath;
        }

        public string GetWebPath(string container, string fileName)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, container, fileName);
        }

        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
