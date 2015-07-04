var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "./Controller"], function (require, exports, Controller) {
    var FooController = (function (_super) {
        __extends(FooController, _super);
        function FooController() {
            _super.apply(this, arguments);
        }
        FooController.prototype.GetFoos = function () {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "Foo",
                Data: null
            });
        };
        FooController.prototype.GetFoo = function (fooId) {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "Foo/" + widgetId + "?fooId=" + fooId,
                Data: null
            });
        };
        FooController.prototype.CreateFoo = function (foo) {
            return this.CallApi({
                Verb: "POST",
                Endpoint: "Foo",
                Data: JSON.stringify(foo)
            });
        };
        FooController.prototype.DeleteFoo = function (fooId) {
            return this.CallApi({
                Verb: "DELETE",
                Endpoint: "Foo/" + widgetId + "?fooId=" + fooId,
                Data: null
            });
        };
        FooController.prototype.UpdateFoo = function (foo) {
            return this.CallApi({
                Verb: "PATCH",
                Endpoint: "Foo",
                Data: JSON.stringify(foo)
            });
        };
        return FooController;
    })(Controller);
    return FooController;
});
