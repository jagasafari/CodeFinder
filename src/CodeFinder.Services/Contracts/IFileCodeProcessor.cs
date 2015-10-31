namespace CodeFinder.Services.Contracts
{
    public interface IFileCodeProcessor
    {
        string[] Process(string path);
    }
}