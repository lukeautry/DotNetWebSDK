define(["require", "exports", "./Generated/Api/PeopleController", "./Generated/Models/Person"], function (require, exports, PeopleController, Person) {
    var Start;
    (function (Start) {
        var peopleController = new PeopleController();
        peopleController.GetPeople().Success(function (people) {
            console.log(people);
        });
        peopleController.DeletePerson(5).Success(function () {
            console.log("Delete success");
        });
        var newPerson = new Person();
        newPerson.Id = 4;
        newPerson.Email = "Test@test.com";
        newPerson.Age = 5;
        peopleController.CreatePerson(newPerson).Success(function (person) {
            console.log("Successfully created: " + person);
        });
        var editedPerson = newPerson;
        editedPerson.Age = 25;
        peopleController.UpdatePerson(editedPerson).Success(function (person) {
            console.log("Successfully created: " + person);
        });
    })(Start || (Start = {}));
    return Start;
});
