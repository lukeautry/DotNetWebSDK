import Controller = require("./Controller");
import ApiPromise = require("./ApiPromise");
import Person = require("../Models/Person");

class PeopleController extends Controller {

	public GetPeople(): ApiPromise<Array<Person>> {
		return this.CallApi<Array<Person>>({
			Verb: "GET",
			Endpoint: "People",
			Data: null			
		});
	}

	public GetPerson(personId: number, anotherProperty: string): ApiPromise<Person> {
		return this.CallApi<Person>({
			Verb: "GET",
			Endpoint: "People/" + personId,
			Data: null			
		});
	}

	public CreatePerson(person: Person): ApiPromise<Person> {
		return this.CallApi<Person>({
			Verb: "POST",
			Endpoint: "People",
			Data: JSON.stringify(person)			
		});
	}
}

export = PeopleController;