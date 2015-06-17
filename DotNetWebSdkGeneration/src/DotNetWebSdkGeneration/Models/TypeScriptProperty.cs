using DotLiquid;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptProperty : ILiquidizable
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public object ToLiquid()
        {
            return new
            {
                Name,
                Type
            };
        }
    }
}