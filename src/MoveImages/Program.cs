﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CommandLine;
using MoveImages.Util;

namespace MoveImages
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            if (!Directory.Exists(opts.Source))
                throw new InvalidOperationException($"The source folder '{opts.Source}' does not exists");
            if (!Directory.Exists(opts.Destination))
                throw new InvalidOperationException($"The destination root folder '{opts.Destination}' does not exists");
            // MoveFilesBack(opts);
            // return;

            foreach (var filename in Directory.EnumerateFiles(opts.Source))
            {
                var file = new FileInfo(filename);
                var yearFolder = EnsureFolderExists(opts.Destination, file.LastWriteTime.Year.ToString());
                var dayFolder = EnsureFolderExists(yearFolder, $"{file.LastWriteTime:yyyy-MM-dd}");
                var destinationFileName = Path.Combine(dayFolder, file.Name);
                Console.WriteLine($"Moving {file.FullName} to {destinationFileName}");
                file.MoveTo(destinationFileName);
            }
        }

        private static string EnsureFolderExists(string rootFolder, string subFolder)
        {
            var folder = Path.Combine(rootFolder, subFolder);
            if (!Directory.Exists(folder))
            {
                using (new ChangeConsoleColor(ConsoleColor.Yellow))
                {
                    Console.WriteLine($"Creating folder '{folder}'.");
                    Directory.CreateDirectory(folder);
                }
            }

            return folder;
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            using (new ChangeConsoleColor(ConsoleColor.Red))
            {
                foreach (var err in errs)
                    Console.WriteLine(err.ToString());
            }
        }

        private static void MoveFilesBack(Options opts)
        {
            foreach (var dir in Directory.EnumerateDirectories(@"C:\Users\jense\OneDrive\Família\Imagens\2018\"))
            {
                var dirInfo = new DirectoryInfo(dir);
                if (dirInfo.Name.Length != 10 || !dirInfo.Name.StartsWith("2018-"))
                    continue;
                foreach (var fileName in Directory.EnumerateFiles(dir))
                {
                    var fileInfo = new FileInfo(fileName);
                    var sourceFile = Path.Combine(opts.Source, fileInfo.Name);
                    fileInfo.MoveTo(sourceFile);
                }
            }
        }
    }

    public class Options
    {
        [Option(
            's',
            "source",
            Default=@"C:\Users\jense\OneDrive\Imagens\Imagens da Câmera",
            HelpText = "Source folder to move images from"
        )]
        public string Source { get; set; }

        [Option(
            'd',
            "destination",
            Default = @"C:\Users\jense\OneDrive\Família\Imagens",
            HelpText = "Destination folder to move images to. A folder will be created for each year and year-month-day of the images."
        )]
        public string Destination { get; set; }
    }
}
