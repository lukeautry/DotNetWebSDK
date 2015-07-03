var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "./Controller"], function (require, exports, Controller) {
    var WidgetsController = (function (_super) {
        __extends(WidgetsController, _super);
        function WidgetsController() {
            _super.apply(this, arguments);
        }
        WidgetsController.prototype.GetWidgets = function () {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "Widgets",
                Data: null
            });
        };
        WidgetsController.prototype.GetWidget = function (widgetId) {
            return this.CallApi({
                Verb: "GET",
                Endpoint: "Widgets/" + widgetId,
                Data: null
            });
        };
        WidgetsController.prototype.CreateWidget = function (widget) {
            return this.CallApi({
                Verb: "POST",
                Endpoint: "Widgets",
                Data: JSON.stringify(widget)
            });
        };
        WidgetsController.prototype.DeleteWidget = function (widgetId) {
            return this.CallApi({
                Verb: "DELETE",
                Endpoint: "Widgets/" + widgetId,
                Data: null
            });
        };
        WidgetsController.prototype.UpdateWidget = function (widget) {
            return this.CallApi({
                Verb: "PATCH",
                Endpoint: "Widgets",
                Data: JSON.stringify(widget)
            });
        };
        return WidgetsController;
    })(Controller);
    return WidgetsController;
});
