define(["require", "exports", "./ApiPromise"], function (require, exports, ApiPromise) {
    var Query = (function () {
        function Query() {
            this.BaseApiUrl = "{{BaseApiUrl}}";
        }
        Query.prototype.CallApi = function (options) {
            var _this = this;
            var promise = new ApiPromise();
            var xhr = XMLHttpRequest || ActiveXObject;
            var request = new xhr('MSXML2.XMLHTTP.3.0');
            request.open(options.Verb, options.Endpoint, true);
            request.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');
            request.onreadystatechange = function () {
                if (request.readyState === 4) {
                    if (request.status >= 200 && request.status < 300) {
                        var response = _this.ParseResponse(request);
                        promise.ResolveSuccess(response);
                    }
                    else {
                        var response = _this.ParseResponse(request);
                        promise.Error(response);
                    }
                }
            };
            request.send(options.Data);
            return promise;
        };
        Query.prototype.ParseResponse = function (request) {
            try {
                return JSON.parse(request.responseText);
            }
            catch (e) {
                return request.responseText;
            }
        };
        return Query;
    })();
    return Query;
});
