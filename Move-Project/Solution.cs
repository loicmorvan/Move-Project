using System;
using System.Threading.Tasks;

using static MoveProject.IO.Path;
using static System.IO.File;
using static System.IO.Path;

namespace MoveProject
{
    internal sealed class Solution
    {
        private readonly string _solutionPath;

        public Solution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                throw new ArgumentException("The solution path is not valid.", nameof(solutionPath));
            }

            _solutionPath = GetFullPath(solutionPath);
        }

        public async Task UpdateAsync(MovingProject project)
        {
            var oldProjectRelativePath = '"' + GetRelativePath(_solutionPath, project.OldFullPath) + '"';
            var newProjectRelativePath = '"' + GetRelativePath(_solutionPath, project.NewFullPath) + '"';

            string solutionContent;

            using (var file = OpenText(_solutionPath))
            {
                solutionContent = await file.ReadToEndAsync();
            }

            if (!solutionContent.Contains(oldProjectRelativePath))
            {
                return;
            }

            solutionContent = solutionContent.Replace(oldProjectRelativePath, newProjectRelativePath);

            using (var file = CreateText(_solutionPath))
            {
                await file.WriteAsync(solutionContent);
            }
        }
    }
}
