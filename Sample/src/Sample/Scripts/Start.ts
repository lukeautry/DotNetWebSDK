import PeopleController = require("./Generated/Api/PeopleController");
import Person = require("./Generated/Models/Person");

module Start {
    var peopleController = new PeopleController();

    peopleController.GetPeople().Success(people => {
        console.log(people);
    });

    peopleController.DeletePerson(5).Success(() => {
        console.log("Delete success");
    });

    var newPerson = new Person();
    newPerson.Id = 4;
    newPerson.Email = "Test@test.com";
    newPerson.Age = 5;

    peopleController.CreatePerson(newPerson).Success(person => {
        console.log("Successfully created: " + person);
    });

    var editedPerson = newPerson;
    editedPerson.Age = 25;

    peopleController.UpdatePerson(editedPerson).Success(person => {
        console.log("Successfully created: " + person);
    });
}

export = Start;