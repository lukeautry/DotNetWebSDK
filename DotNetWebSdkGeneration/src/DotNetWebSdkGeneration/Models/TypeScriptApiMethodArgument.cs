namespace DotNetWebSdkGeneration.Models
{
    internal class TypeScriptApiMethodArgument
    {
        internal readonly string Type;
        internal readonly string Name;

        internal TypeScriptApiMethodArgument(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}