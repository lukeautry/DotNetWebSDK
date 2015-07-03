import ApiPromise = require("./ApiPromise");

class Query {
    private BaseApiUrl = "";

    protected CallApi<T>(options: QueryOptions): ApiPromise<T> {
        var promise = new ApiPromise<T>();
        var xhr = XMLHttpRequest || ActiveXObject;
        var endpoint = this.BaseApiUrl + options.Endpoint;

        var request = new xhr('MSXML2.XMLHTTP.3.0');
        request.open(options.Verb, options.Endpoint, true);
        request.setRequestHeader('Content-type', 'application/json');
        request.onreadystatechange = () => {
            if (request.readyState === 4) {
                if (request.status >= 200 && request.status < 300) {
                    var response = this.ParseResponse(request);
                    promise.ResolveSuccess(response);
                } else {
                    var response = this.ParseResponse(request);
                    promise.Error(response);
                }
            }
        };

        request.send(options.Data);

        return promise;
    }

    private ParseResponse(request: any): any {
        try {
            return JSON.parse(request.responseText);
        } catch (e) {
            return request.responseText;
        }
    }
}

export = Query;