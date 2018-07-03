using System.IO;

namespace MoveProject.IO
{
    public static class Directory
    {
        public static void Move(string fromDirectory, string toDirectory)
        {
            if (!System.IO.Directory.Exists(toDirectory))
            {
                System.IO.Directory.CreateDirectory(toDirectory);
            }

            foreach (var subDirectory in System.IO.Directory.GetDirectories(fromDirectory))
            {
                var subDirectoryName = new DirectoryInfo(subDirectory).Name;

                Move(subDirectory, System.IO.Path.Combine(toDirectory, subDirectoryName));
            }

            foreach (var file in System.IO.Directory.GetFiles(fromDirectory))
            {
                var fileName = System.IO.Path.GetFileName(file);

                File.Move(file, System.IO.Path.Combine(toDirectory, fileName));
            }

            System.IO.Directory.Delete(fromDirectory, true);
        }
    }
}
