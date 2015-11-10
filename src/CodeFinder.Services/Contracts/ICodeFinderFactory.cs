namespace CodeFinder.Services.Contracts
{
    using ViewModels;

    public interface ICodeFinderFactory 
    {
        ICodeFinder Create(CodeToFindViewModel model);
    }
}