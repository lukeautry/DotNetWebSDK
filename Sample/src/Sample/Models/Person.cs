using DotNetWebSdkGeneration;

namespace Sample.Models
{
    [GeneratedModel]
    public class Person
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public Widget Widget { get; set; }
    }
}
