using Microsoft.Extensions.CommandLineUtils;
using System;

namespace MoveProject
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication();

            var projectArg = app.Argument("project", "The path to the project to rename including the extension.");
            projectArg.MultipleValues = false;

            var newNameArg = app.Argument("newName", "The new name (and path) of the project including the extension.");
            newNameArg.MultipleValues = false;

            app.HelpOption("-h | --help");

            app.OnExecute(async () =>
            {
                try
                {
                    var workingDirectory = new WorkingDirectory();

                    var movingProject = new MovingProject(projectArg.Value, newNameArg.Value);
                    movingProject.Move();

                    foreach (var solution in workingDirectory.EnumerateSolutions())
                    {
                        await solution.UpdateAsync(movingProject);
                    }

                    foreach (var project in workingDirectory.EnumerateProjects())
                    {
                        await project.UpdateAsync(movingProject);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return -1;
                }

                return 0;
            });

            if (args.Length < 2)
            {
                app.ShowHelp();
                return -1;
            }

            return app.Execute(args);
        }
    }
}
