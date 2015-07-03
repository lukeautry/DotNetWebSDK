using System.Collections.Immutable;

namespace DotNetWebSdkGeneration.Models
{
    internal class TypeScriptApiController
    {
        internal readonly string Name;
        internal readonly ImmutableList<TypeScriptApiMethod> Methods;
        internal readonly ImmutableList<string> References;

        internal TypeScriptApiController(string name, ImmutableList<TypeScriptApiMethod> methods, ImmutableList<string> references)
        {
            Name = name;
            Methods = methods;
            References = references;
        }
    }
}
