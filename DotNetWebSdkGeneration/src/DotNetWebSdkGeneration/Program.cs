using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using DotLiquid;
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

            var sourceFileProcessors = GetSourceFileProcessors(sourcePath);
            var models = GetModels(sourceFileProcessors);

            var template = GetClassTemplate();
            foreach (var typeScriptClass in models)
            {
                var renderedContent = template.Render(Hash.FromAnonymousObject(new
                {
                    typeScriptClass.Name,
                    Properties = typeScriptClass.Properties.ToList(),
                    References = typeScriptClass.References.ToList()
                }));

                File.WriteAllText(Path.Combine(outputPath, typeScriptClass.Name + ".ts"), renderedContent);
            }
        }

        private static IEnumerable<TypeScriptClass> GetModels(ImmutableList<SourceFileProcessor> processors)
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

        private static ImmutableList<SourceFileProcessor> GetSourceFileProcessors(string sourcePath)
        {
            var sourceFilePaths = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);
            return sourceFilePaths.Select(path => new SourceFileProcessor(path)).Where(processor => processor.IsTaggedForGeneration).ToImmutableList();
        }

        private static Template GetClassTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "DotNetWebSdkGeneration.Templates.TypeScriptClass.liq";
            string classTemplate;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                classTemplate = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(classTemplate))
            {
                throw new Exception("Couldn't find " + resourceName);
            }

            var template = Template.Parse(classTemplate);
            return template;
        }
    }
}
