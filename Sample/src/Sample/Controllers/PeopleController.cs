using Microsoft.AspNet.Mvc;
using Sample.Models;
using System.Collections.Generic;
using DotNetWebSdkGeneration.Attributes;
using System.Linq;

namespace Sample.Controllers
{
    [GeneratedController]
    public class PeopleController : Controller
    {
        [Route("People")]
        [AcceptVerbs("GET")]
        public IEnumerable<Person> GetPeople()
        {
            var people = new List<Person>()
            {
                new Person { Id = 1, Email = "test-user-1", Age = 20 },
                new Person { Id = 2, Email = "test-user-2", Age = 21 },
                new Person { Id = 3, Email = "test-user-3", Age = 22 }
            };

            return people;
        }

        [Route("People/{personId}")]
        [AcceptVerbs("GET")]
        public Person GetPerson(int personId, string anotherProperty)
        {
            return GetPeople().Single(p => p.Id == personId);
        }

        [Route("People")]
        [AcceptVerbs("POST")]
        public Person CreatePerson([FromBody]Person person)
        {
            // Create person

            return person;
        }

        [Route("People")]
        [AcceptVerbs("PATCH")]
        public Person UpdatePerson([FromBody]Person person)
        {
            // Update person

            return person;
        }

        [Route("People/{personId}")]
        [AcceptVerbs("DELETE")]
        public void DeletePerson(int personId)
        {
            // Delete widget
        }
    }
}
