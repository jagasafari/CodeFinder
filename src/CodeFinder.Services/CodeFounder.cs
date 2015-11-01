namespace CodeFinder.Services
{
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;

    public class CodeFounder : ICodeFounder
    {
        private readonly string _fileExtensionPattern;
        private readonly string[] _keywords;
        private readonly int _numKeywords;
        private readonly string _searchedPath;
        private readonly ConcurrentDictionary<string, int>[] _groupedFiles;

        public CodeFounder(int numKeywords, string[] keywords,
            string searchedPath, string fileExtensionPattern)
        {
            _numKeywords = numKeywords;
            _keywords = keywords;
            _searchedPath = searchedPath;
            _fileExtensionPattern = fileExtensionPattern;
            _groupedFiles = InitializeConcurrentBag(_numKeywords);
        }

        public string[] GetMachingFiles()
        {
            GroupMatchingFiles();
            return FlattenFileGroups();
        }

        private string[] FlattenFileGroups()
        {
            var numMachingFiles =
                _groupedFiles.Sum(fileGroup => fileGroup.Count);
            var result = new string[numMachingFiles];
            var nextFile = 0;
            for(var i = _numKeywords - 1; i >= 0; i--)
            {
                var sortedByLineNumbers =
                    SortFilesByLineNumbers(_groupedFiles[i]);
                foreach(var file in sortedByLineNumbers)
                    result[nextFile++] = file;
            }
            return result;
        }

        private string[] SortFilesByLineNumbers(
            ConcurrentDictionary<string, int> files)
        {
            var sortedFiles = new string[files.Count];
            var count = 0;
            foreach(var file in files.OrderBy(x => x.Value))
                sortedFiles[count++] = file.Key;
            return sortedFiles;
        }

        private void GroupMatchingFiles()
        {
            var files = new DirectoryInfo(_searchedPath).GetFiles(
                _fileExtensionPattern,
                SearchOption.AllDirectories);

            Parallel.ForEach(files, InterviewFile);
        }

        private void InterviewFile(FileInfo file)
        {
            var text = File.ReadAllText(file.FullName);

            var count = -1 +
                        _keywords.Count(keyword =>
                                text.ToLowerInvariant()
                                    .Contains(
                                        keyword.ToLowerInvariant()));
            if(count <= -1) return;
            var endOfLineChar = '\n';
            var numLines = text.Count(x => x == endOfLineChar);
            _groupedFiles[count][file.FullName] = numLines;
        }

        private static ConcurrentDictionary<string, int>[]
            InitializeConcurrentBag(int numKeywords)
        {
            var groupedFiles =
                new ConcurrentDictionary<string, int>[numKeywords];
            for(var i = 0; i < numKeywords; i++)
                groupedFiles[i] = new ConcurrentDictionary<string, int>();
            return groupedFiles;
        }
    }
}