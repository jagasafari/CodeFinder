namespace CodeFinder.ViewModels
{
    public class MachingFilesViewModel
    {
        public CodeToFindViewModel CodeToFind { get; set; }    
        public string[] MachingFiles { get; set; }
        public string[] FirstFileContent { get; set; }
        public int NextFile { get; set; }
        public bool NotFound { get; set; }
        public int NumberOfMatchingFiles { get; set; }
    }
}