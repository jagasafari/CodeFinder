namespace CodeFinder.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CodeToFindViewModel
    {
        [Required]
        public string SearchedPath { get; set; }
        [Required]
        public string FileExtensionPattern { get; set; }
        [Required]
        public string  Keywords { get; set; }
    }
}