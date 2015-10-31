namespace CodeFinder.Services
{
    using Contracts;

    public class FileCodeProcessor:IFileCodeProcessor
    {
        public string[] Process(string path)
        {
            var lines = System.IO.File.ReadAllLines(path);
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] =
                    lines[i].Replace("script", "scriptTagRemoved");
            }
            return lines;
        }
    }
}