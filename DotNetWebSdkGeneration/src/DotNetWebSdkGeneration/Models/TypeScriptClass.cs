using System.Collections.Immutable;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptClass
    {
        public readonly string Name;
        public readonly ImmutableList<TypeScriptProperty> Properties;
        public readonly ImmutableList<string> References;

        public TypeScriptClass(string name, ImmutableList<TypeScriptProperty> properties, ImmutableList<string> references)
        {
            Name = name;
            Properties = properties;
            References = references;
        }
    }
}
