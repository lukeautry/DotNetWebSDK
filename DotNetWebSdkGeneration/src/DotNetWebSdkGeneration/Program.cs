using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using DotNetWebSdkGeneration.CommandLineParsing;
using DotNetWebSdkGeneration.Models;

namespace DotNetWebSdkGeneration
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var arguments = new CommandLineArgumentParser(args);
            var outputPath = arguments.GetOutputPath();
            var sourcePath = arguments.GetSourcePath();


            var sourceFileProcessors = SourceFileProcessor.GetSourceFileProcessors(sourcePath, typeof(GeneratedModel));
            var models = GetModels(sourceFileProcessors);

            EmptyDirectory(new DirectoryInfo(outputPath));

            new ModelClassGenerator(models, outputPath);
            new ApiClassGenerator(models, arguments);
        }

        private static ImmutableList<TypeScriptClass> GetModels(ImmutableList<SourceFileProcessor> processors)
        {
            var models = new List<TypeScriptClass>();

            var classNames = processors.Select(p => p.ClassName).ToImmutableList();
            var classNamesToProperties = processors.ToDictionary(processor => processor.ClassName, processor => processor.GetProperties(classNames));

            foreach (var classToProperty in classNamesToProperties)
            {
                var references = classNames.Where(className => classNames.Contains(className) && className != classToProperty.Key).ToImmutableList();
                models.Add(new TypeScriptClass(classToProperty.Key, classToProperty.Value, references));
            }

            return models.ToImmutableList();
        }

        public static void EmptyDirectory(DirectoryInfo directory)
        {
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
