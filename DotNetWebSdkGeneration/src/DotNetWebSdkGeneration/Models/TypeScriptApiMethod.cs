using System.Collections.Immutable;
using System.Linq;
using System.Text;
using DotLiquid;

namespace DotNetWebSdkGeneration.Models
{
    internal sealed class TypeScriptApiMethod : ILiquidizable
    {
        internal readonly string Name;
        internal readonly string Url;
        internal readonly string Verb;
        internal readonly IImmutableList<TypeScriptApiMethodArgument> Arguments;
        internal readonly string ReturnType;
        internal readonly string FormattedArguments;
        internal readonly string FormattedEndpoint;
        internal readonly string FormattedData;
        private const string NullStringValue = "null";

        internal TypeScriptApiMethod(string name, string url, string verb, IImmutableList<TypeScriptApiMethodArgument> arguments, string returnType)
        {
            Name = name;
            Url = url;
            Verb = verb.ToUpper();
            Arguments = arguments;
            ReturnType = returnType;

            FormattedArguments = GetFormattedArguments();
            FormattedEndpoint = GetFormattedEndpoint();
            FormattedData = GetFormattedData();
        }

        public object ToLiquid()
        {
            return new
            {
                Name,
                Url,
                Verb,
                Arguments = Arguments.ToList(),
                ReturnType,
                FormattedArguments,
                FormattedEndpoint,
                FormattedData
            };
        }

        private string GetFormattedData()
        {
            if (Verb == "GET" || Verb == "DELETE")
            {
                return NullStringValue;
            }

            var postableArgument = Arguments.FirstOrDefault(a => !IsValidTypeForQueryString(a.Type));
            if (postableArgument == null)
            {
                return NullStringValue;
            }

            return $"JSON.stringify({postableArgument.Name})";
        }

        private string GetFormattedEndpoint()
        {
            var endpoint = GetEndpointWithRouteParameters();

            var queryStringableArguments = Arguments.Where(a => !endpoint.Contains(a.Name) && IsValidTypeForQueryString(a.Type)).ToImmutableList();
            if (queryStringableArguments.Any())
            {
                var sb = new StringBuilder(endpoint).Append(" + \"?");

                foreach (var argument in queryStringableArguments)
                {
                    sb.AppendFormat("{0}=\" + {1}", argument.Name, argument.Name);
                }

                sb.Append("\"");
            }

            return endpoint;
        }

        private bool IsValidTypeForQueryString(string type)
        {
            return type == Constants.NumberType || type == Constants.StringType;
        }

        private string GetEndpointWithRouteParameters()
        {
            if (!Url.Contains("{") || !Url.Contains("}"))
            {
                return $"\"{Url}\"";
            }

            var sb = new StringBuilder("\"");
            for (var i = 0; i < Url.Length; i++)
            {
                var character = Url[i];
                if (character == '{')
                {
                    // start of param matching, close previous quotation
                    sb.Append("\" + ");
                    continue;
                }

                if (character == '}')
                {
                    if (!IsLastIndex(i))
                    {
                        sb.Append(" + \"");
                    }
                    continue;
                }

                sb.Append(character);

                if (IsLastIndex(i))
                {
                    sb.Append("\"");
                }
            }

            return sb.ToString();
        }

        private bool IsLastIndex(int i)
        {
            return i == Url.Length - 1;
        }

        private string GetFormattedArguments()
        {
            if (!Arguments.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            var last = Arguments.Last();
            foreach (var argument in Arguments)
            {
                sb.AppendFormat("{0}: {1}", argument.Name, argument.Type);

                if (!argument.Equals(last))
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
    }
}
 