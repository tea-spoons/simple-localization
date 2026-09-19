
namespace TeaSpoons.SimpleLocalization
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal static class LocalizedStringFormatting
    {
        internal static readonly Regex formatRegex = new Regex(@"\{\{.+?\}\}", RegexOptions.IgnoreCase);

        internal static Dictionary<string, string> GetArgumentDictionary(LocalizedString key)
        {
            Dictionary<string, string> arguments = null;

            var matches = formatRegex.Matches(key.key);
            foreach (Match match in matches)
            {
                arguments ??= new();
                arguments[GetIdentifier(match.Value)] = string.Empty;
            }

            return arguments;
        }

        internal static string Format(string s, Dictionary<string, string> arguments, bool requireAllArguments)
        {
            if (arguments != null)
            {
                if (requireAllArguments)
                {
                    try
                    {
                        s = formatRegex.Replace(s, match => arguments[GetIdentifier(match.Value)]);
                    }
                    catch
                    {
                        throw new System.ArgumentException($"Not all arguments were present to format \"{s}\".");
                    }
                }
                else
                {
                    s = formatRegex.Replace(s, match => arguments.GetValueOrDefault(GetIdentifier(match.Value)));
                }
            }
            return s;
        }

        internal static string Format(string s, object argument)
        {
            return formatRegex.Replace(s, argument.ToString());
        }

        private static string GetIdentifier(string s)
        {
            return s[2..^2];
        }
    }
}
