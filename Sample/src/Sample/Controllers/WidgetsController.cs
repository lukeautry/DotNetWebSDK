using Microsoft.AspNet.Mvc;
using Sample.Models;
using System.Collections.Generic;
using DotNetWebSdkGeneration;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Controllers
{
    [GeneratedController]
    public class WidgetsController : Controller
    {
        [Route("Widgets")]
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<Widget>> GetWidgets()
        {
            var people = new List<Widget>()
            {
                new Widget { Id = 1, Name = "test-widget-1" },
                new Widget { Id = 2, Name = "test-widget-2" },
                new Widget { Id = 3, Name = "test-widget-3" }
            };

            return people;
        }

        [Route("Widgets/{widgetId}")]
        [AcceptVerbs("GET")]
        public async Task<Widget> GetWidget(int widgetId)
        {
            var widgets = await GetWidgets();
            return widgets.Single(w => w.Id == widgetId);
        }

        [Route("Widgets")]
        [AcceptVerbs("POST")]
        public async Task<Widget> CreateWidget([FromBody]Widget widget)
        {
            // Create widget
            return widget;
        }

        [Route("Widgets/{widgetId}")]
        [AcceptVerbs("DELETE")]
        public async Task DeleteWidget(int widgetId)
        {
            // Delete widget
        }

        [Route("Widgets")]
        [AcceptVerbs("PATCH")]
        public async Task<Widget> UpdateWidget([FromBody]Widget widget)
        {
            // Update widget
            return widget;
        }
    }
}
