using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generator.Models
{
    public class TypeScriptClass
    {
        public string Name { get; set; }
        public List<TypeScriptProperty> Properties;
    }
}
