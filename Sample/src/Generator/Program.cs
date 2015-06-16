using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Generator.Models;
using RazorEngine;
using RazorEngine.Templating;

namespace Generator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectDirectory = @"Z:\Code\sdkgen\Sample\src\Sample";
            var outputDirectory = @"Z:\Code\sdkgen\Sample\src\Sample\Scripts";
            var allCsFilePaths = Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories);

            var classes = new List<TypeScriptClass>();

            foreach(var path in allCsFilePaths)
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

            var classTemplate = File.ReadAllText(@"Templates/TypeScriptClass.cshtml");

            foreach (var typeScriptClass in classes)
            {
                var result = Engine.Razor.RunCompile(classTemplate, "key", typeof(TypeScriptClass), typeScriptClass);
                File.WriteAllText(Path.Combine(outputDirectory, typeScriptClass.Name + ".ts"), result);
            }
        }
    }
}
