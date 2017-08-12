using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    public static class VersionUtils
    {
        private const string versionSuffixRegexString = 
            @"^(?<main>[0-9,A-Z,a-z]+)(\.(?<additonal>[0-9,A-Z,a-z]+))*$";
        private static readonly Regex versionSuffixRegex = new Regex(versionSuffixRegexString, 
            RegexOptions.Compiled | RegexOptions.Singleline);
        public static bool TryParseVersionSuffix(string input, out string[] parts)
        {
            if (input == null)
			{
				throw new ArgumentNullException(nameof(input));
			}

			var parsed = versionSuffixRegex.Match(input);
			if (!parsed.Success)
			{
                parts = null;
				return false;
			}
            
			var main = parsed.Groups["main"].Value;
			var additonal = parsed.Groups["additonal"].Captures.Cast<Capture>().Select(c => c.Value).ToArray();
			parts = new string[additonal.Length + 1];
			parts[0] = main;
			Array.Copy(additonal, 0, parts, 1, additonal.Length);
            return true;
        }
    }

}