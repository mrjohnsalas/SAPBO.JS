namespace SAPBO.JS.WebApi.Utilities
{
    public interface IFileStorage
    {
        Task<string> SaveFile(string path, IFormFile file, string newFileName);

        string GetWebPath(string container, string fileName);

        string GetCurrentDirectory();
    }
}
