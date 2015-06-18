using System.Collections.Immutable;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptClass
    {
        public string Name { get; set; }
        public ImmutableList<TypeScriptProperty> Properties { get; set; }
    }
}
