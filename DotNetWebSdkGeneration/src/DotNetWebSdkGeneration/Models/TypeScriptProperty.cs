using System.Collections.Generic;
using DotLiquid;
using Microsoft.CodeAnalysis;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptProperty : ILiquidizable
    {
        public readonly string Name;
        public readonly string Type;

        public TypeScriptProperty(string name, ITypeSymbol type, IEnumerable<string> knownClassNames)
        {
            Name = name;
            Type = TypeResolver.GetType(type, knownClassNames);
        }

        public object ToLiquid()
        {
            return new { Name, Type };
        }
    }
}