using System;
using System.Collections.Generic;
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
        public List<string> PhoneNumbers { get; set; }

        public Widget Widget { get; set; }
        public List<Widget> WidgetList { get; set; } 
        public Widget[] WidgetArray { get; set; }
        public IEnumerable<Widget> WidgetIEnumerable { get; set; }

        public UnmarkedClass UnmarkedReference { get; set; }
    }
}
