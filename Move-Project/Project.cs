using System.IO;
using System.Threading.Tasks;
using static MoveProject.IO.Path;
using static System.IO.File;

namespace MoveProject
{
    internal sealed class Project
    {
        public Project(string path)
        {
            FullPath = Path.GetFullPath(path);
        }

        public string FullPath { get; }

        public async Task UpdateAsync(MovingProject movingProject)
        {
            var fromThisProjectToTheOldMovingOne = GetRelativePath(FullPath, movingProject.OldFullPath);
            var fromThisProjectToTheOldMovingOne2 = fromThisProjectToTheOldMovingOne.Replace('\\', '/');

            string projectContent;
            using (var file = OpenText(FullPath))
            {
                projectContent = await file.ReadToEndAsync();
            }

            if (!projectContent.Contains(fromThisProjectToTheOldMovingOne) &&
                !projectContent.Contains(fromThisProjectToTheOldMovingOne2))
            {
                return;
            }

            var fromThisProjectToTheNewMovingOne = GetRelativePath(FullPath, movingProject.NewFullPath);
            var fromThisProjectToTheNewMovingOne2 = fromThisProjectToTheNewMovingOne.Replace('\\', '/');

            projectContent = projectContent.Replace(fromThisProjectToTheOldMovingOne, fromThisProjectToTheNewMovingOne);
            projectContent = projectContent.Replace(fromThisProjectToTheOldMovingOne2, fromThisProjectToTheNewMovingOne2);

            using (var file = CreateText(FullPath))
            {
                await file.WriteAsync(projectContent);
            }
        }
    }
}
