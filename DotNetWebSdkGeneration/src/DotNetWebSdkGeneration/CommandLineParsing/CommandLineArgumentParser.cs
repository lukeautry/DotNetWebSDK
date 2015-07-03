using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetWebSdkGeneration.CommandLineParsing
{
    internal class CommandLineArgumentParser
    {
        private readonly Dictionary<CommandType, string> _commandsToValues;
        private const string CommandPrefix = "--";
        
        internal CommandLineArgumentParser(IEnumerable<string> commandLineArguments)
        {
            _commandsToValues = ParseArguments(commandLineArguments);
        }

        internal string GetOutputPath()
        {
            return GetCommandValue(CommandType.Output);
        }

        internal string GetSourcePath()
        {
            return GetCommandValue(CommandType.Source);
        }

        internal string GetBaseApiUrl()
        {
            try
            {
                return GetCommandValue(CommandType.BaseApiUrl);
            }
            catch
            {
                return "";
            }
        }

        private string GetCommandValue(CommandType commandType)
        {
            try
            {
                return _commandsToValues[commandType];
            }
            catch
            {
                throw new Exception("No value was passed in for " + commandType);
            }
        }

        private static Dictionary<CommandType, string> ParseArguments(IEnumerable<string> commandLineArguments)
        {
            return commandLineArguments.Select(ParseArgument).ToDictionary(commandToValue => commandToValue.Key, commandToValue => commandToValue.Value);
        }

        private static KeyValuePair<CommandType, string> ParseArgument(string argument)
        {
            var argPrefix = argument.Substring(0, 2);
            if (argPrefix != CommandPrefix || !argument.Contains("="))
            {
                // TODO: Could provide a more helpful error here
                throw new Exception("Invalid argument provided: " + argument);
            }

            var trimmedArgument = argument.Substring(CommandPrefix.Length, argument.Length - CommandPrefix.Length);
            var commandType = GetCommandType(trimmedArgument);
            var commandValue = GetCommandValueFromArgument(trimmedArgument);

            return new KeyValuePair<CommandType, string>(commandType, commandValue);
        }

        private static string GetCommandValueFromArgument(string trimmedArgument)
        {
            var indexOfEqualSign = IndexOfEqualSign(trimmedArgument);
            return trimmedArgument.Substring(indexOfEqualSign + 1, trimmedArgument.Length - indexOfEqualSign - 1);
        }

        private static int IndexOfEqualSign(string argument)
        {
            return argument.IndexOf("=", StringComparison.Ordinal);
        }

        private static CommandType GetCommandType(string argument)
        {
            var requestedType = argument.Substring(0, IndexOfEqualSign(argument));

            foreach (var type in Enum.GetValues(typeof(CommandType)))
            {
                if (type.ToString().Equals(requestedType, StringComparison.OrdinalIgnoreCase))
                {
                    return (CommandType)type;
                }
            }

            throw new Exception("Invalid command argument: " + argument);
        }
    }
}