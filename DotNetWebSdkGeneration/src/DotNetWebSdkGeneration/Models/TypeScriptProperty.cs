using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using Microsoft.CodeAnalysis;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptProperty : ILiquidizable
    {
        public readonly string Name;
        public readonly string Type;

        private const string NumberType = "number";
        private const string BoolType = "boolean";
        private const string StringType = "string";
        private const string DateType = "Date";
        private const string AnyType = "any";
        
        private static readonly Dictionary<TypeCode, string> PrimitiveCodesToPropertyNames = new Dictionary<TypeCode, string>
        {
            {TypeCode.Int16, NumberType },
            {TypeCode.Int32, NumberType },
            {TypeCode.Int64, NumberType },
            {TypeCode.UInt16, NumberType },
            {TypeCode.UInt32, NumberType },
            {TypeCode.UInt64, NumberType },
            {TypeCode.Byte, NumberType },
            {TypeCode.Double, NumberType },
            {TypeCode.Decimal, NumberType },
            {TypeCode.SByte, NumberType },
            {TypeCode.Single, NumberType },
            {TypeCode.Boolean, BoolType },
            {TypeCode.Empty, StringType },
            {TypeCode.String, StringType },
            {TypeCode.Char, StringType },
            {TypeCode.DBNull, StringType },
            {TypeCode.DateTime, DateType },
        };

        public TypeScriptProperty(string name, ITypeSymbol type, IEnumerable<string> knownClassNames)
        {
            Name = name;
            Type = GetType(type, knownClassNames);
        }

        private static string GetType(ITypeSymbol typeSymbol, IEnumerable<string> knownClassNames)
        {
            string typeName;

            var success = GetKnownTypeName(typeSymbol, out typeName);
            if (success) { return typeName; }
            
            if (IsCollection(typeSymbol))
            {
                return GetCollectionTypeName(typeSymbol, knownClassNames);
            }

            return knownClassNames.Contains(typeSymbol.Name) ? typeSymbol.Name : AnyType;
        }

        private static bool IsCollection(ITypeSymbol typeSymbol)
        {
            return typeSymbol.AllInterfaces.Any(i => i.Name == "ICollection" || i.Name == "IEnumerable");
        }

        private static string GetCollectionTypeName(ITypeSymbol typeSymbol, IEnumerable<string> knownClassNames)
        {
            var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
            string collectionTypeName;

            var typeArgument = namedTypeSymbol?.TypeArguments.FirstOrDefault();
            if (typeArgument == null)
            {
                // Arrays don't have type arguments, but we can pull it out manually
                var name = typeSymbol.ToString();
                collectionTypeName = name.EndsWith("[]") ? name.Substring(0, name.Length - 2) : AnyType;
            }
            else
            {
                collectionTypeName = GetType(typeArgument, knownClassNames);
            }

            return $"Array<{collectionTypeName}>";
        }

        private static bool GetKnownTypeName(ITypeSymbol typeSymbol, out string typeName)
        {
            var extractedTypeName = ExtractTypeNameFromSymbol(typeSymbol);

            var type = System.Type.GetType(extractedTypeName);
            if (type != null)
            {
                return PrimitiveCodesToPropertyNames.TryGetValue(System.Type.GetTypeCode(type), out typeName);
            }

            typeName = string.Empty;
            return false;
        }

        private static string ExtractTypeNameFromSymbol(ITypeSymbol typeSymbol)
        {
            // TODO: At some point we need to find a way to get the underlying type, but the API is making it hard
            var name = typeSymbol.ToString();
            if (name == "int?") { return "System.Int32"; }
            if (name == "bool?") { return "System.Boolean"; }
            if (name == "System.DateTime?") { return "System.DateTime"; }

            return typeSymbol.ContainingNamespace + "." + typeSymbol.Name;
        }

        public object ToLiquid()
        {
            return new { Name, Type };
        }
    }
}