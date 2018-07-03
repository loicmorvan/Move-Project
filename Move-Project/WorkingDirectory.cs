using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoveProject
{
    internal sealed class WorkingDirectory
    {
        public WorkingDirectory(string directoryPath = ".")
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new ArgumentException("message", nameof(directoryPath));
            }

            FullPath = Path.GetFullPath(directoryPath);
        }

        public string FullPath { get; }

        public IEnumerable<Solution> EnumerateSolutions()
        {
            foreach (var solutionFile in EnumerateFiles(FullPath, ".sln"))
            {
                yield return new Solution(solutionFile);
            }
        }

        public IEnumerable<Project> EnumerateProjects()
        {
            foreach (var projectFile in EnumerateFiles(FullPath, ".csproj", ".vcxproj"))
            {
                yield return new Project(projectFile);
            }
        }

        private static IEnumerable<string> EnumerateFiles(string currentDirectory, params string[] extensions)
        {
            foreach (var file in Directory.GetFiles(currentDirectory))
            {
                var fileExtension = Path.GetExtension(file);
                if (extensions.Contains(fileExtension))
                {
                    yield return file;
                }
            }

            foreach (var directory in Directory.GetDirectories(currentDirectory))
            {
                foreach (var file in EnumerateFiles(directory, extensions))
                {
                    yield return file;
                }
            }
        }
    }
}
