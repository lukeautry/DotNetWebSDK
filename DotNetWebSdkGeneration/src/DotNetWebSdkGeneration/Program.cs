using System.IO;
using DotNetWebSdkGeneration.CommandLineParsing;
using DotNetWebSdkGeneration.FileGeneration;

namespace DotNetWebSdkGeneration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new CommandLineArgumentParser(args);
            CleanOutputDirectory(arguments);
            
            var models = ModelGenerator.Generate(arguments);
            ControllerGenerator.Generate(models, arguments);
        }

        internal static void CleanOutputDirectory(CommandLineArgumentParser arguments)
        {
            var directory = new DirectoryInfo(arguments.GetOutputPath());
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }
    }
}
