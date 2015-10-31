namespace CodeFinder.Controllers
{
    using CodeFinder.Services.Contracts;
    using CodeFinder.ViewModels;
    using Microsoft.AspNet.Mvc;

    public class SearchController : Controller
    {
        private readonly ICodeFounderFactory _codeFounderFactory;

        public SearchController(ICodeFounderFactory codeFounderFactory)
        {
            _codeFounderFactory = codeFounderFactory;
        }

        [HttpGet]
        public IActionResult FileCode(string path)
        {
            return Json(System.IO.File.ReadAllLines(path));
        }

        [HttpPost]
        public IActionResult MatchingFiles(SearchedCodeViewModel model)
        {
            var codeFounder = _codeFounderFactory.Create(model);
            var machingFiles = codeFounder.GetMachingFiles();

            var machingFilesViewModel = new MachingFilesViewModel
            {
                NextFile = 0,
                MachingFiles = machingFiles,
                FirstFileContent =
                    machingFiles.Length > 0
                        ? System.IO.File.ReadAllLines(machingFiles[0])
                        : null
            };

            return View(machingFilesViewModel);
        }
    }
}