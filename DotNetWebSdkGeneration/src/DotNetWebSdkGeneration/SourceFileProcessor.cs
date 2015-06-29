using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetWebSdkGeneration
{
    internal class SourceFileProcessor
    {
        internal readonly bool IsTaggedForGeneration;
        internal readonly string ClassName;

        private readonly MetadataReference _mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
        private readonly SyntaxTree _syntaxTree;
        private readonly SemanticModel _semanticModel;

        internal SourceFileProcessor(string sourceFilePath)
        {
            _syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));
            var compilation = CSharpCompilation.Create("Comp")
                .AddSyntaxTrees(_syntaxTree)
                .AddReferences(_mscorlib);

            var classSyntax = _syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classSyntax == null) { return; }

            _semanticModel = compilation.GetSemanticModel(_syntaxTree, false);
            var classSymbol = _semanticModel.GetDeclaredSymbol(classSyntax);

            IsTaggedForGeneration = classSymbol.GetAttributes().Any(a => a.ToString() == typeof(GeneratedModel).Name);
            if (IsTaggedForGeneration)
            {
                ClassName = classSymbol.Name;
            }
        }

        internal ImmutableList<TypeScriptProperty> GetProperties(IEnumerable<string> classNames)
        {
            var propertySyntaxes = _syntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>();
            return propertySyntaxes.Select(propertySyntax =>
            {
                var propertySymbol = _semanticModel.GetDeclaredSymbol(propertySyntax);
                return new TypeScriptProperty(propertySymbol.Name, propertySymbol.Type, classNames);
            }).ToImmutableList();
        }
    }
}
