using System;
using System.IO;
using static MoveProject.IO.Path;

namespace MoveProject
{
    internal sealed class MovingProject
    {
        /// <summary>
        /// Creates and initializes a new instance of the <see cref="MovingProject"/> class.
        /// </summary>
        /// <param name="projectPath">The path to the project.</param>
        /// <exception cref="ArgumentException">If the input path is not valid.</exception>
        /// <exception cref="FileNotFoundException">If the project file was not found.</exception>
        public MovingProject(string projectPath, string newPath)
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                throw new ArgumentException("The input path is not valid.", nameof(projectPath));
            }

            var oldFullPath = Path.GetFullPath(projectPath);

            if (!File.Exists(oldFullPath))
            {
                throw new FileNotFoundException("The project file was not found.", projectPath);
            }

            OldProjectNameAndExtension = Path.GetFileName(oldFullPath);
            OldDirectoryPath = Path.GetDirectoryName(oldFullPath);

            if (string.IsNullOrEmpty(newPath))
            {
                throw new ArgumentException("The new path is not valid.", nameof(newPath));
            }

            var newFullPath = Path.GetFullPath(newPath);

            if (File.Exists(newFullPath))
            {
                throw new FileNotFoundException("A project of the same name already exists.", newPath);
            }

            NewProjectNameAndExtension = Path.GetFileName(newFullPath);
            NewDirectoryPath = Path.GetDirectoryName(newFullPath);
        }

        public string OldProjectNameAndExtension { get; }

        public string OldDirectoryPath { get; }

        public string NewProjectNameAndExtension { get; }

        public string NewDirectoryPath { get; }

        public bool HasMoved { get; private set; }

        public string NewFullPath => Path.Combine(NewDirectoryPath, NewProjectNameAndExtension);

        public string OldFullPath => Path.Combine(OldDirectoryPath, OldProjectNameAndExtension);

        public string OldPathToNewPath => GetRelativePath(OldDirectoryPath, NewDirectoryPath);

        public void Move()
        {
            if (HasMoved)
            {
                throw new NotSupportedException("The project was already moved.");
            }

            var oldProject = Path.Combine(OldDirectoryPath, OldProjectNameAndExtension);
            var newProjectInOldDirectory = Path.Combine(OldDirectoryPath, NewProjectNameAndExtension);
            File.Move(oldProject, newProjectInOldDirectory);

            if (OldDirectoryPath != NewDirectoryPath)
            {
                MoveProject.IO.Directory.Move(OldDirectoryPath, NewDirectoryPath);
            }

            // TODO: Update ProjectReferences.

            HasMoved = true;
        }
    }
}
