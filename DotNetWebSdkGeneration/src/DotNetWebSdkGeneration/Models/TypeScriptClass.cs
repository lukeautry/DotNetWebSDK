using System.Collections.Immutable;
using DotLiquid;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptClass : ILiquidizable
    {
        public string Name { get; set; }
        public ImmutableList<TypeScriptProperty> Properties { get; set; }
        public ImmutableList<TypeScriptClass> References { get; set; }

        public object ToLiquid()
        {
            return new
            {
                Name,
                Properties
            };
        }
    }
}
