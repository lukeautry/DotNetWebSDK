var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "./Controller"], function (require, exports, Controller) {
    var PeopleController = (function (_super) {
        __extends(PeopleController, _super);
        function PeopleController() {
            _super.apply(this, arguments);
        }
        PeopleController.prototype.GetPeople = function () {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "People",
                Data: null
            });
        };
        PeopleController.prototype.GetPerson = function (personId, anotherProperty) {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "People/" + personId,
                Data: null
            });
        };
        PeopleController.prototype.CreatePerson = function (person) {
            return this.CallApi({
                Verb: "POST",
                Endpoint: "People",
                Data: JSON.stringify(person)
            });
        };
        PeopleController.prototype.UpdatePerson = function (person) {
            return this.CallApi({
                Verb: "PATCH",
                Endpoint: "People",
                Data: JSON.stringify(person)
            });
        };
        PeopleController.prototype.DeletePerson = function (personId) {
            return this.CallApi({
                Verb: "DELETE",
                Endpoint: "People/" + personId,
                Data: null
            });
        };
        return PeopleController;
    })(Controller);
    return PeopleController;
});
