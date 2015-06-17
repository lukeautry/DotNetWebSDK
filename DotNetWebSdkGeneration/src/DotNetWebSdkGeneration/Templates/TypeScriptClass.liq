class {{Name}} {
{% for property in Properties -%}
	{{property.Name}}: {{property.Type}};
{% endfor -%}
}