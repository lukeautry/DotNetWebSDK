import Controller = require("./Controller");
import ApiPromise = require("./ApiPromise");
{% for reference in References -%}
import {{reference}} = require("../Models/{{reference}}");
{% endfor -%}

class {{Name}} extends Controller {
{% for method in Methods -%}

	public {{method.Name}}({{method.FormattedArguments}}): ApiPromise<{{method.ReturnType}}> {
		return this.CallApi<{{method.ReturnType}}>({
			Verb: "{{ method.Verb -}}",
			Endpoint: {{ method.FormattedEndpoint -}}{% if method.FormattedDate != "null" -%},
			Data: {{ method.FormattedData -}}
			{% endif %}
		});
	}
{% endfor -%}
}

export = {{Name}};