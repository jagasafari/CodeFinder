﻿namespace CodeFinder.Controllers
{
    using System.Linq;
    using Microsoft.AspNet.Mvc;
    using Services.Contracts;
    using ViewModels;

    public class SearchController : Controller
    {
        private readonly ICodeFinderFactory _codeFinderFactory;
        private readonly IFileCodeProcessor _codeProcessor;

        public SearchController(ICodeFinderFactory codeFinderFactory,
            IFileCodeProcessor codeProcessor)
        {
            _codeFinderFactory = codeFinderFactory;
            _codeProcessor = codeProcessor;
        }

        [HttpGet]
        public IActionResult MatchingFiles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MatchingFiles(
            [FromForm] CodeToFindViewModel codeToFind)
        {
            if(! ModelState.IsValid)
                return View(codeToFind);

            return RedirectToAction("Code", codeToFind);
        }

        public IActionResult Code(CodeToFindViewModel codeToFind)
        {
            var codeFounder = _codeFinderFactory.Create(codeToFind);
            var machingFiles = codeFounder.GetMachingFiles();

            if(! machingFiles.Any())
            {
                return View(new MachingFilesViewModel
                {
                    CodeToFind=codeToFind,
                    NumberOfMatchingFiles = 0,
                    NotFound = true
                });
            }

            var nextFile = 0;
            var bestMatchShortestFileContent =
                _codeProcessor.Process(machingFiles[nextFile++]);

            var matchingFiles = new MachingFilesViewModel
            {
                CodeToFind = codeToFind,
                FirstFileContent = bestMatchShortestFileContent,
                NextFile = nextFile,
                NumberOfMatchingFiles =machingFiles.Length,
                MachingFiles = machingFiles,
            };

            return View(matchingFiles);
        }

        [HttpGet]
        public IActionResult FileCode(string path)
        {
            return Json(_codeProcessor.Process(path));
        }
    }
}