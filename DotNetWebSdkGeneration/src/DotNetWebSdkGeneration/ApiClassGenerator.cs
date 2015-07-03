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
    internal sealed class ApiClassGenerator : Generator
    {
        private const string ApiDirectoryName = "Api";

        internal ApiClassGenerator(ImmutableList<TypeScriptClass> models, CommandLineArgumentParser arguments)
        {
            var outputPath = arguments.GetOutputPath();
            CreateDirectoryIfNecessary(outputPath, ApiDirectoryName);
            
            var sourceFileProcessors = SourceFileProcessor.GetSourceFileProcessors(arguments.GetSourcePath(), typeof(GeneratedController));

            var controllers = new List<TypeScriptApiController>();
            foreach (var processor in sourceFileProcessors)
            {
                var controllerName = processor.ClassName;
                var controllerMethods = processor.GetMethods(models.Select(m => m.Name).ToImmutableList());

                var references = new List<string>();
                var knownClassNames = models.Select(m => m.Name).ToList();

                foreach (var method in controllerMethods)
                {
                    if (knownClassNames.Contains(method.ReturnType))
                    {
                        references.Add(method.ReturnType);
                        continue;
                    }

                    foreach (var argument in method.Arguments.Where(argument => knownClassNames.Contains(argument.Type)))
                    {
                        references.Add(argument.Type);
                        break;
                    }
                }

                var distinctReferences = references.Distinct(StringComparer.OrdinalIgnoreCase).ToImmutableList();

                var controller = new TypeScriptApiController(controllerName, controllerMethods, distinctReferences);
                controllers.Add(controller);
            }

            if (controllers.Any())
            {
                RenderControllers(controllers.ToImmutableList(), outputPath);
                CopyStaticTypeScriptFiles(Path.Combine(outputPath, ApiDirectoryName));
                RenderQueryTemplate(outputPath, arguments.GetBaseApiUrl());
            }
        }

        private void RenderControllers(ImmutableList<TypeScriptApiController> controllers, string outputPath)
        {
            var template = GetTemplate("Api.DynamicController.liq");

            foreach (var controller in controllers)
            {
                var renderedContent = template.Render(Hash.FromAnonymousObject(new
                {
                    controller.Name, Methods = controller.Methods.ToList(), References = controller.References.ToList()
                }));

                File.WriteAllText(Path.Combine(outputPath, ApiDirectoryName, controller.Name + ".ts"), renderedContent);
            }
        }

        private static void RenderQueryTemplate(string outputPath, string baseApiUrl)
        {
            var template = GetTemplate("Api.Controller.ts");

            var renderedContent = template.Render(Hash.FromAnonymousObject(new
            {
                BaseApiUrl = baseApiUrl
            }));

            File.WriteAllText(Path.Combine(outputPath, ApiDirectoryName, "Controller.ts"), renderedContent);
        }

        private static void CopyStaticTypeScriptFiles(string outputPath)
        {
            var staticTypeScriptPaths = new Dictionary<string, string>
            {
                { "ApiPromise.ts", "DotNetWebSdkGeneration.Templates.Api.ApiPromise.ts" },
                { "QueryOptions.ts", "DotNetWebSdkGeneration.Templates.Api.QueryOptions.ts" }
            };

            var assembly = Assembly.GetExecutingAssembly();
            foreach (var filePath in staticTypeScriptPaths)
            {
                var fileContent = GetResourceContent(assembly, filePath.Value);
                File.WriteAllText(Path.Combine(outputPath, filePath.Key), fileContent);
            }
        }
    }
}
