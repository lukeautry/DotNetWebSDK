using System;
using System.Collections.Generic;
using DotNetWebSdkGeneration.Attributes;

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
        public List<string> PhoneNumbers { get; set; }

        public Widget PrimaryWidget { get; set; }
        public List<Widget> AllWidgets { get; set; } 

        public UnmarkedClass UnmarkedReference { get; set; }
    }
}
