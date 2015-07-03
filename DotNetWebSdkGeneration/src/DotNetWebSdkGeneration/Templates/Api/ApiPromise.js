define(["require", "exports"], function (require, exports) {
    var ApiPromise = (function () {
        function ApiPromise() {
            this.SuccessCallbacks = new Array();
            this.ErrorCallbacks = new Array();
        }
        ApiPromise.prototype.Success = function (callback) {
            this.SuccessCallbacks.push(callback);
            return this;
        };
        ApiPromise.prototype.Error = function (callback) {
            this.ErrorCallbacks.push(callback);
            return this;
        };
        ApiPromise.prototype.ResolveSuccess = function (value) {
            this.SuccessCallbacks.forEach(function (callback) { return callback(value); });
        };
        ApiPromise.prototype.ResolveError = function (message) {
            this.ErrorCallbacks.forEach(function (callback) { return callback(message); });
        };
        return ApiPromise;
    })();
    return ApiPromise;
});
