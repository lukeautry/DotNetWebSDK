using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using DotLiquid;
using DotNetWebSdkGeneration.Attributes;
using DotNetWebSdkGeneration.CommandLineParsing;
using DotNetWebSdkGeneration.Models;

namespace DotNetWebSdkGeneration.FileGeneration
{
    internal static class ModelGenerator
    {
        private const string ModelsDirectoryName = "Models";

        internal static IReadOnlyList<TypeScriptClass> Generate(CommandLineArgumentParser arguments)
        {
            var sourceFileProcessors = SourceFileProcessor.GetSourceFileProcessors(arguments.GetSourcePath(), typeof(GeneratedModel));

            var models = GetModels(sourceFileProcessors);
            if (models.Any())
            {
                FileGenerationHelper.CreateDirectoryIfNecessary(arguments.GetOutputPath(), ModelsDirectoryName);
            }

            RenderClassFiles(arguments.GetOutputPath(), models);

            return models;
        }

        private static IReadOnlyList<TypeScriptClass> GetModels(ImmutableList<SourceFileProcessor> processors)
        {
            var models = new List<TypeScriptClass>();

            var classNames = processors.Select(p => p.ClassName).ToImmutableList();
            var classNamesToProperties = processors.ToDictionary(processor => processor.ClassName, processor => processor.GetProperties(classNames));

            foreach (var classToProperty in classNamesToProperties)
            {
                var references = classToProperty.Value.Where(p => classNames.Contains(p.Type) && p.Type != classToProperty.Key).Select(p => p.Type).Distinct().ToImmutableList();
                models.Add(new TypeScriptClass(classToProperty.Key, classToProperty.Value, references));
            }

            return models.ToImmutableList();
        }

        private static void RenderClassFiles(string outputPath, IEnumerable<TypeScriptClass> models)
        {
            var template = FileGenerationHelper.GetTemplate("TypeScriptClass.liq");

            foreach (var typeScriptClass in models)
            {
                var renderedContent = template.Render(Hash.FromAnonymousObject(new
                {
                    typeScriptClass.Name,
                    Properties = typeScriptClass.Properties.ToList(),
                    References = typeScriptClass.References.ToList()
                }));

                File.WriteAllText(Path.Combine(outputPath, ModelsDirectoryName, typeScriptClass.Name + ".ts"), renderedContent);
            }
        }
    }
}