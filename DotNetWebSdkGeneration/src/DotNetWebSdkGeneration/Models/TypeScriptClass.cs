using System.Collections.Generic;

namespace DotNetWebSdkGeneration.Models
{
    public class TypeScriptClass
    {
        public string Name { get; set; }
        public List<TypeScriptProperty> Properties { get; set; }
    }
}
