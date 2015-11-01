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

        public CodeFounder(int numKeywords, string[] keywords,
            string searchedPath, string fileExtensionPattern)
        {
            _numKeywords = numKeywords;
            _keywords = keywords;
            _searchedPath = searchedPath;
            _fileExtensionPattern = fileExtensionPattern;
        }

        public string[] GetMachingFiles()
        {
            var fileList = FindMachingFiles();
            var numMachingFiles = CountMatchingfiles(fileList);
            return SortListOfFiles(numMachingFiles, fileList);
        }

        private string[] SortListOfFiles(int numMachingFiles,
            ConcurrentDictionary<string, int>[] fileList)
        {
            var result = new string[numMachingFiles];
            var nextFile = 0;
            for(var i = _numKeywords - 1; i >= 0; i--)
            {
                var sortedByLineNumbers =
                    SortFilesByLineNumbers(fileList[i]);
                foreach(var file in sortedByLineNumbers)
                {
                    result[nextFile++] = file;
                }
            }
            return result;
        }

        private string[] SortFilesByLineNumbers(
            ConcurrentDictionary<string, int> files)
        {
            var sortedFiles = new string[files.Count];
            var count = 0;
            foreach(var file in files.OrderBy(x=>x.Value))
                sortedFiles[count++] = file.Key;
            return sortedFiles;
        }

        private int CountMatchingfiles(
            ConcurrentDictionary<string, int>[] fileList)
        {
            var numMachingFiles = 0;
            for(var i = 0; i < _numKeywords; i++)
            {
                numMachingFiles += fileList[i].Count;
            }
            return numMachingFiles;
        }

        private ConcurrentDictionary<string, int>[] FindMachingFiles(){
            var fileList = InitializeConcurrentBag(_numKeywords);

            var files = new DirectoryInfo(_searchedPath).GetFiles(
                    _fileExtensionPattern,
                    SearchOption.AllDirectories);

            Parallel.ForEach(files, file =>
            {
                var text = File.ReadAllText(file.FullName);

                var count = -1 +
                            _keywords.Count(
                                keyword =>
                                    text.ToLowerInvariant()
                                        .Contains(
                                            keyword.ToLowerInvariant()));
                if(count > -1)
                {
                    var endOfLineChar = '\n';
                    var numLines = text.Count(x => x == endOfLineChar);
                    fileList[count][file.FullName] = numLines;
                }
            });
            return fileList;
        }

        private static ConcurrentDictionary<string, int>[]
            InitializeConcurrentBag(int numKeywords)
        {
            var fileList =
                new ConcurrentDictionary<string, int>[numKeywords];
            for (var i = 0; i < numKeywords; i++)
                fileList[i] = new ConcurrentDictionary<string, int>();
            return fileList;
        }
    }
}