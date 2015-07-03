using System;
using System.IO;
using System.Reflection;
using DotLiquid;

namespace DotNetWebSdkGeneration
{
    internal abstract class Generator
    {
        protected static Template GetTemplate(string periodSeparatedPath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DotNetWebSdkGeneration.Templates." + periodSeparatedPath;
            var classTemplate = GetResourceContent(assembly, resourceName);

            var template = Template.Parse(classTemplate);
            return template;
        }

        protected static string GetResourceContent(Assembly assembly, string resourceName)
        {
            string resourceContent;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                resourceContent = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(resourceContent))
            {
                throw new Exception("Couldn't find " + resourceName);
            }

            return resourceContent;
        }

        protected static void CreateDirectoryIfNecessary(string outputPath, string directoryName)
        {
            var fullPath = Path.Combine(outputPath, directoryName);
            var directoryExists = Directory.Exists(fullPath);
            if (!directoryExists)
            {
                Directory.CreateDirectory(fullPath);
            }
        }
    }
}