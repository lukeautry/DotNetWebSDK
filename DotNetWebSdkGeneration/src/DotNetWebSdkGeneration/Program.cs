using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotLiquid;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetWebSdkGeneration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourceDirectory = @"Z:\Code\sdkgen\Sample\src\Sample";
            var outputDirectory = @"Z:\Code\sdkgen\Sample\src\Sample\Scripts";

            var sourceFilePaths = Directory.GetFiles(sourceDirectory, "*.cs", SearchOption.AllDirectories);

            var classes = new List<TypeScriptClass>();

            foreach(var path in sourceFilePaths)
            {
                var fileContent = File.ReadAllText(path);
                var tree = CSharpSyntaxTree.ParseText(fileContent);

                var compiled = CSharpCompilation.Create("MyCompilation", syntaxTrees: new[] { tree });
                var model = compiled.GetSemanticModel(tree, false);

                var classSyntax = tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classSyntax == null)
                {
                    continue;
                }

                var classSymbol = model.GetDeclaredSymbol(classSyntax);

                var isTaggedForGeneration = classSymbol.GetAttributes().Any(a => a.ToString() == typeof(GeneratedModel).Name);
                if (!isTaggedForGeneration)
                {
                    continue;
                }

                var typescriptClass = new TypeScriptClass();
                typescriptClass.Name = classSymbol.Name;
                typescriptClass.Properties = new List<TypeScriptProperty>();

                var propertySyntaxes = tree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>();
                foreach (var propertySyntax in propertySyntaxes)
                {
                    var propertySymbol = model.GetDeclaredSymbol(propertySyntax);
                    var typeScriptProperty = new TypeScriptProperty();
                    typeScriptProperty.Name = propertySymbol.Name;
                    typeScriptProperty.Type = "string";

                    typescriptClass.Properties.Add(typeScriptProperty);
                }

                classes.Add(typescriptClass);
            }

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

            foreach (var typeScriptClass in classes)
            {
                var result = template.Render(Hash.FromAnonymousObject(typeScriptClass));

                File.WriteAllText(Path.Combine(outputDirectory, typeScriptClass.Name + ".ts"), result);
            }
        }
    }
}
