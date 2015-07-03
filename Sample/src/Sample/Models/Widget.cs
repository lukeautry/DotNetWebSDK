using DotNetWebSdkGeneration.Attributes;

namespace Sample.Models
{
    [GeneratedModel]
    public class Widget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Widget Subwidget { get; set; }
    }
}
