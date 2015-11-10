namespace CodeFinder.Services
{
    using Contracts;
    using ViewModels;

    public class CodeFinderFactory:ICodeFinderFactory
    {
        public ICodeFinder Create(CodeToFindViewModel model)
        {
            var keywords = model.Keywords.Split(',');
            var searchedPath = model.SearchedPath;
            var fileExtensionPattern = model.FileExtensionPattern;
            return new CodeFinder( keywords, searchedPath, fileExtensionPattern);
        }
    }
}