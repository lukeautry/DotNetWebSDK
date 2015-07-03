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
    internal sealed class SourceFileProcessor
    {
        internal readonly bool IsTaggedForGeneration;
        internal readonly string ClassName;

        private readonly MetadataReference _mscorlib = MetadataReference.CreateFromAssembly(typeof(object).Assembly);
        private readonly MetadataReference _mvcAssembly = MetadataReference.CreateFromAssembly(typeof(Microsoft.AspNet.Mvc.AcceptVerbsAttribute).Assembly);
        private readonly SyntaxTree _syntaxTree;
        private readonly SemanticModel _semanticModel;

        internal SourceFileProcessor(string sourceFilePath, Type markedAttributeType)
        {
            _syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(sourceFilePath));
            var compilation = CSharpCompilation.Create("Comp")
                .AddSyntaxTrees(_syntaxTree)
                .AddReferences(_mscorlib, _mvcAssembly);

            var classSyntax = _syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classSyntax == null) { return; }

            _semanticModel = compilation.GetSemanticModel(_syntaxTree, false);
            var classSymbol = _semanticModel.GetDeclaredSymbol(classSyntax);

            IsTaggedForGeneration = classSymbol.GetAttributes().Any(a => a.ToString() == markedAttributeType.Name);
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

        internal static ImmutableList<SourceFileProcessor> GetSourceFileProcessors(string sourcePath, Type type)
        {
            var sourceFilePaths = Directory.GetFiles(sourcePath, "*.cs", SearchOption.AllDirectories);
            return sourceFilePaths.Select(path => new SourceFileProcessor(path, type)).Where(processor => processor.IsTaggedForGeneration).ToImmutableList();
        }

        internal ImmutableList<TypeScriptApiMethod> GetMethods(ImmutableList<string> knownClassNames)
        {
            var methods = new List<TypeScriptApiMethod>();

            var methodSyntaxes = _syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var syntax in methodSyntaxes)
            {
                var methodSymbol = _semanticModel.GetDeclaredSymbol(syntax);
                var methodName = methodSymbol.Name;
                var methodVerb = GetMethodVerb(methodSymbol);
                var methodReturnType = GetReturnType(methodSymbol, knownClassNames);
                var methodArguments = GetArguments(methodSymbol, knownClassNames);
                var methodUrl = GetUrl(methodSymbol);

                methods.Add(new TypeScriptApiMethod(methodName, methodUrl, methodVerb, methodArguments, methodReturnType));
            }

            return methods.ToImmutableList();
        }

        private static string GetUrl(IMethodSymbol methodSymbol)
        {
            try
            {
                return GetAttributeArgument(methodSymbol, "Microsoft.AspNet.Mvc.RouteAttribute");
            }
            catch
            {
                throw new Exception("No method route url found (make sure there's a Route attribute assigned for all controller methods)");
            }
        }

        private static string GetAttributeArgument(IMethodSymbol methodSymbol, string typeName)
        {
            var attribute = methodSymbol.GetAttributes().FirstOrDefault(a => a.ToString().Contains(typeName));
            if (attribute != null)
            {
                return attribute.ConstructorArguments.First().Value.ToString();
            }

            throw new Exception("Unable to retrieve/parse attribute parameters.");
        }

        private static ImmutableList<TypeScriptApiMethodArgument> GetArguments(IMethodSymbol methodSymbol, ImmutableList<string> knownClassNames)
        {
            var arguments = new List<TypeScriptApiMethodArgument>();

            var parameters = methodSymbol.Parameters;
            foreach (var parameter in parameters)
            {
                var parameterType = TypeResolver.GetType(parameter.Type, knownClassNames);
                var parameterName = parameter.Name;

                arguments.Add(new TypeScriptApiMethodArgument(parameterType, parameterName));
            }

            return arguments.ToImmutableList();
        }

        private static string GetReturnType(IMethodSymbol methodSymbol, ImmutableList<string> knownClassNames)
        {
            return TypeResolver.GetType(methodSymbol.ReturnType, knownClassNames);
        }

        private static string GetMethodVerb(IMethodSymbol methodSymbol)
        {
            try
            {
                return GetAttributeArgument(methodSymbol, "Microsoft.AspNet.Mvc.AcceptVerbsAttribute");
            }
            catch
            {
                return "GET";
            }
        }
    }
}
