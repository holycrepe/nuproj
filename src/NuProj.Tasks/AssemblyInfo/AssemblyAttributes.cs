namespace NuProj.Tasks.AssemblyInfo
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class AssemblyAttributes
    {
        public static Regex GetPattern(string attribute = "[a-z]+")
            =>
                new Regex(
                    @"(?<!^.*//.*)\[ *assembly *: (?<Name>" + attribute + @")(?:Attribute)? *\( *"" *(?<Value>.+) *"" *\) *\]",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);

        public static Regex All = GetPattern();

        public static Regex Versions = GetPattern(@"Assembly(?:File)?Version");

        public static Regex Version = GetPattern(@"AssemblyVersion");

        public static Regex FileVersion = GetPattern(@"AssemblyFileVersion");
        public static Dictionary<string, string> ParseAllAttributes(string contents)
        {
            var values = new Dictionary<string, string>();
            var match = All.Match(contents);
            while (match.Success)
            {
                var value = match.Groups["Value"].Value;
                if (!string.IsNullOrWhiteSpace(value))
                    values[match.Groups["Name"].Value] = value;
                match = match.NextMatch();
            }
            return values;
        }
    }
}