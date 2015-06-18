using System.Collections.Immutable;
using System.Linq;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetWebSdkGeneration.ModelBuilding
{
    internal class TypeScriptClassBuilder
    {
        internal static TypeScriptClass Build(string name, SyntaxTree syntaxTree, SemanticModel semanticModel)
        {
            return new TypeScriptClass
            {
                Name = name,
                Properties = GetProperties(syntaxTree, semanticModel)
            };
        }

        private static ImmutableList<TypeScriptProperty> GetProperties(SyntaxTree syntaxTree, SemanticModel semanticModel)
        {
            var propertySyntaxes = syntaxTree.GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>();
            return propertySyntaxes.Select(propertySyntax => TypeScriptPropertyBuilder.Build(propertySyntax, semanticModel)).ToImmutableList();
        }
    }
}
