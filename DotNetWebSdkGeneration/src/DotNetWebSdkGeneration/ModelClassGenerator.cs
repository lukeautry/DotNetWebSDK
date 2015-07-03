using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using DotLiquid;
using DotNetWebSdkGeneration.Models;

namespace DotNetWebSdkGeneration
{
    internal sealed class ModelClassGenerator : Generator
    {
        private const string ModelsDirectoryName = "Models";

        internal ModelClassGenerator(ImmutableList<TypeScriptClass> models, string outputPath)
        {
            var template = GetClassTemplate();
            if (models.Any())
            {
                CreateDirectoryIfNecessary(outputPath, ModelsDirectoryName);
            }

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

        private static Template GetClassTemplate()
        {
            return GetTemplate("TypeScriptClass.liq");
        }
    }
}