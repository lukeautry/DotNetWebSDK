using System;
using System.Collections.Generic;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis;

namespace DotNetWebSdkGeneration.ModelBuilding
{
    internal class TypeScriptPropertyBuilder
    {
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

        internal static TypeScriptProperty Build(IPropertySymbol propertySymbol)
        {
            return new TypeScriptProperty
            {
                Name = propertySymbol.Name,
                Type = GetType(propertySymbol.Type)
            };
        }

        private static string GetType(ITypeSymbol typeSymbol)
        {
            string typeName;

            var success = GetKnownTypeName(typeSymbol, out typeName);
            if (success)
            {
                return typeName;
            }

            // This is a type we don't know about

            return AnyType;
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
            // TODO: At some point we need to find a way to get the underlying type, but the API is making it hard
            var name = typeSymbol.ToString();
            if (name == "int?") { return "System.Int32"; }
            if (name == "bool?") { return "System.Boolean"; }
            if (name == "System.DateTime?") { return "System.DateTime"; }

            return typeSymbol.ContainingNamespace + "." + typeSymbol.Name;
        }
    }
}