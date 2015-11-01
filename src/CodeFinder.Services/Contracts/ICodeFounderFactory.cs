namespace CodeFinder.Services.Contracts
{
    using ViewModels;

    public interface ICodeFounderFactory 
    {
        ICodeFounder Create(CodeToFindViewModel model);
    }
}