using System;
using DotNetWebSdkGeneration;

namespace Sample.Models
{
    [GeneratedModel]
    public class Person
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }

        public Widget Widget { get; set; }
        public UnmarkedClass UnmarkedReference { get; set; }
    }
}
