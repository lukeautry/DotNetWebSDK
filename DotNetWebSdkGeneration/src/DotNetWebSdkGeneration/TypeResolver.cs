using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace DotNetWebSdkGeneration
{
    public static class TypeResolver
    {
        private static readonly Dictionary<TypeCode, string> PrimitiveCodesToPropertyNames = new Dictionary<TypeCode, string>
        {
            {TypeCode.Int16, Constants.NumberType },
            {TypeCode.Int32, Constants.NumberType },
            {TypeCode.Int64, Constants.NumberType },
            {TypeCode.UInt16, Constants.NumberType },
            {TypeCode.UInt32, Constants.NumberType },
            {TypeCode.UInt64, Constants.NumberType },
            {TypeCode.Byte, Constants.NumberType },
            {TypeCode.Double, Constants.NumberType },
            {TypeCode.Decimal, Constants.NumberType },
            {TypeCode.SByte, Constants.NumberType },
            {TypeCode.Single, Constants.NumberType },
            {TypeCode.Boolean, Constants.BoolType },
            {TypeCode.Empty, Constants.StringType },
            {TypeCode.String, Constants.StringType },
            {TypeCode.Char, Constants.StringType },
            {TypeCode.DBNull, Constants.StringType },
            {TypeCode.DateTime, Constants.DateType }
        };

        internal static string GetType(ITypeSymbol typeSymbol, IEnumerable<string> knownClassNames)
        {
            string typeName;

            var success = GetKnownTypeName(typeSymbol, out typeName);
            if (success) { return typeName; }

            if (IsCollection(typeSymbol))
            {
                return GetCollectionTypeName(typeSymbol, knownClassNames);
            }

            if (typeSymbol.Name == "Task")
            {
                var namedTypeSymbol = typeSymbol as INamedTypeSymbol;

                var typeArgument = namedTypeSymbol?.TypeArguments.FirstOrDefault();
                if (typeArgument != null)
                {
                    return GetType(typeArgument, knownClassNames);
                }

                return Constants.VoidType;
            }

            if (typeSymbol.Name.Equals("void", StringComparison.OrdinalIgnoreCase))
            {
                return Constants.VoidType;
            }

            return knownClassNames.Contains(typeSymbol.Name) ? typeSymbol.Name : Constants.AnyType;
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
                collectionTypeName = name.EndsWith("[]") ? name.Substring(0, name.Length - 2) : Constants.AnyType;
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

            var type = Type.GetType(extractedTypeName);
            if (type != null)
            {
                return PrimitiveCodesToPropertyNames.TryGetValue(Type.GetTypeCode(type), out typeName);
            }

            typeName = string.Empty;
            return false;
        }

        private static string ExtractTypeNameFromSymbol(ITypeSymbol typeSymbol)
        {
            var name = typeSymbol.ToString();
            if (name == "int?") { return "System.Int32"; }
            if (name == "bool?") { return "System.Boolean"; }
            if (name == "System.DateTime?") { return "System.DateTime"; }

            return typeSymbol.ContainingNamespace + "." + typeSymbol.Name;
        }
    }
}
