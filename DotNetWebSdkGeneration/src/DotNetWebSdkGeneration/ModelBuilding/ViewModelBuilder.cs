using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetWebSdkGeneration.ModelBuilding
{
    internal class ViewModelBuilder
    {
        public static List<TypeScriptClass> Build(string sourcePath)
        {
            var sourceFilePaths = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);
            var mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);

            var classes = new List<TypeScriptClass>();
            foreach (var path in sourceFilePaths)
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(path));
                var compilation = CSharpCompilation.Create("Comp")
                    .AddSyntaxTrees(syntaxTree)
                    .AddReferences(mscorlib);

                var classSyntax = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classSyntax == null) { continue; }

                var semanticModel = compilation.GetSemanticModel(syntaxTree, false);
                var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);

                // TODO: Not really a sure fire way to make sure the attribute matches, find something better
                var isTaggedForGeneration = classSymbol.GetAttributes().Any(a => a.ToString() == typeof(GeneratedModel).Name);
                if (isTaggedForGeneration)
                {
                    var typescriptClass = TypeScriptClassBuilder.Build(classSymbol.Name, syntaxTree, semanticModel);
                    classes.Add(typescriptClass);
                }
            }

            classes = classes.Select(c => TypeScriptClassBuilder.ResolveUnknownTypes(c, classes)).ToList();

            return classes;
        }
    }
}