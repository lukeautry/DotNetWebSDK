using System;
using System.IO;
using System.Reflection;
using DotLiquid;
using DotNetWebSdkGeneration.CommandLineParsing;
using DotNetWebSdkGeneration.ModelBuilding;

namespace DotNetWebSdkGeneration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var arguments = new CommandLineArgumentParser(args);
            var outputPath = arguments.GetOutputPath();
            var sourcePath = arguments.GetSourcePath();
            var template = GetClassTemplate();

            var models = ViewModelBuilder.Build(sourcePath);
            foreach (var typeScriptClass in models)
            {
                var result = template.Render(Hash.FromAnonymousObject(typeScriptClass));
                File.WriteAllText(Path.Combine(outputPath, typeScriptClass.Name + ".ts"), result);
            }
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
