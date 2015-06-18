using System;
using System.Collections.Generic;
using DotNetWebSdkGeneration.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetWebSdkGeneration.ModelBuilding
{
    internal class TypeScriptPropertyBuilder
    {
        private const string NumberType = "number";
        private const string BoolType = "boolean";
        private const string StringType = "string";
        private const string DateType = "Date";

        private static readonly Dictionary<TypeCode, string> TypeCodesToTypeScriptPropertyName = new Dictionary<TypeCode, string>
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

        internal static TypeScriptProperty Build(PropertyDeclarationSyntax propertySyntax, SemanticModel semanticModel)
        {
            var propertySymbol = semanticModel.GetDeclaredSymbol(propertySyntax) as IPropertySymbol;
            if (propertySymbol == null)
            {
                throw new Exception("Failed to cast property to IPropertySymbol.");
            }

            return new TypeScriptProperty
            {
                Name = propertySymbol.Name,
                Type = GetType(propertySymbol)
            };
        }

        private static string GetType(IPropertySymbol propertySymbol)
        {
            var isPrimitive = propertySymbol.Type.IsValueType || propertySymbol.Type.ContainingNamespace.ToString().Contains("System");
            if (isPrimitive)
            {
                var typeFullName = propertySymbol.Type.ContainingNamespace + "." + propertySymbol.Type.Name;

                var type = Type.GetType(typeFullName);
                if (type != null)
                {
                    string typeName;
                    var success = TypeCodesToTypeScriptPropertyName.TryGetValue(Type.GetTypeCode(type), out typeName);
                    if (success)
                    {
                        return typeName;
                    }
                }
            }

            return StringType;
        }
    }
}