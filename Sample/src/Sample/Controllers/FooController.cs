using Microsoft.AspNet.Mvc;
using Sample.Models;
using System.Collections.Generic;
using DotNetWebSdkGeneration.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Controllers
{
    [GeneratedController]
    public class FooController : Controller
    {
        [Route("Foo")]
        [AcceptVerbs("GET")]
        public async Task<IEnumerable<Foo>> GetFoos()
        {
            var foos = new List<Foo>()
            {
                new Foo { Id = 1, Name = "test-foo-1" },
                new Foo { Id = 2, Name = "test-foo-2" },
                new Foo { Id = 3, Name = "test-foo-3" }
            };

            return foos;
        }

        [Route("Foo/{fooId}")]
        [AcceptVerbs("GET")]
        public async Task<Foo> GetFoo(int fooId)
        {
            var foos = await GetFoos();
            return foos.Single(f => f.Id == fooId);
        }

        [Route("Foo")]
        [AcceptVerbs("POST")]
        public async Task<Foo> CreateFoo([FromBody]Foo foo)
        {
            // Create foo
            return foo;
        }

        [Route("Foo/{fooId}")]
        [AcceptVerbs("DELETE")]
        public async Task DeleteFoo(int fooId)
        {
            // Delete foo
        }

        [Route("Foo")]
        [AcceptVerbs("PATCH")]
        public async Task<Foo> UpdateFoo([FromBody]Foo foo)
        {
            // Update foo
            return foo;
        }
    }
}
