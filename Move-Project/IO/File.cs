using System.IO;

namespace MoveProject.IO
{
    public static class File
    {
        public static void Move(string fromFile, string toFile)
        {
            if (System.IO.File.Exists(toFile))
            {
                System.IO.File.Delete(toFile);
            }

            try
            {
                System.IO.File.Move(fromFile, toFile);
            }
            catch (IOException)
            {
                System.Console.WriteLine(fromFile);
                System.Console.WriteLine(toFile);

                throw;
            }
        }
    }
}
