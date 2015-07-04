using DotNetWebSdkGeneration.Attributes;
using System;

namespace Sample.Models
{
    [GeneratedModel]
    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
