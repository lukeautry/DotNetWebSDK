using System;
using System.Collections.Generic;
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
            return propertySyntaxes.Select(propertySyntax =>
            {
                var propertySymbol = semanticModel.GetDeclaredSymbol(propertySyntax) as IPropertySymbol;
                if (propertySymbol == null)
                {
                    throw new Exception("Failed to cast symbol to IPropertySymbol.");
                }

                return TypeScriptPropertyBuilder.Build(propertySymbol);
            }).ToImmutableList();
        }

        public static TypeScriptClass ResolveUnknownTypes(TypeScriptClass typeScriptClass, List<TypeScriptClass> otherClasses)
        {
            return new TypeScriptClass
            {
                Name = typeScriptClass.Name,
                Properties =
                    typeScriptClass.Properties.Select(p => TypeScriptPropertyBuilder.ResolveIfNecessary(p, otherClasses)).ToImmutableList()
            };
        }
    }
}
