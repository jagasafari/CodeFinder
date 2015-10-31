namespace CodeFinder.Controllers
{
    using Microsoft.AspNet.Mvc;
    using Services.Contracts;
    using ViewModels;

    public class SearchController : Controller
    {
        private readonly ICodeFounderFactory _codeFounderFactory;
        private readonly IFileCodeProcessor _codeProcessor;

        public SearchController(ICodeFounderFactory codeFounderFactory,
            IFileCodeProcessor codeProcessor)
        {
            _codeFounderFactory = codeFounderFactory;
            _codeProcessor = codeProcessor;
        }

        [HttpGet]
        public IActionResult FileCode(string path)
        {
            return Json(_codeProcessor.Process(path));
        }

        [HttpPost]
        public IActionResult MatchingFiles(SearchedCodeViewModel model)
        {
            var codeFounder = _codeFounderFactory.Create(model);
            var machingFiles = codeFounder.GetMachingFiles();
            var machingFilesViewModel = new MachingFilesViewModel
            {
                Keywords = model.Keywords.Split(','),
                NextFile = 1,
                MachingFiles = machingFiles,
                FirstFileContent =
                    machingFiles.Length > 0
                        ? _codeProcessor.Process(machingFiles[0])
                        : null
            };

            return View(machingFilesViewModel);
        }
    }
}