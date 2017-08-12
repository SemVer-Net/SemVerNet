using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class VersionHelpers
    {

        private const string suffixPartRegexString = "[0-9,A-Z,a-z]+";
        private static readonly string versionSuffixRegexString =
            $@"(?<main>{suffixPartRegexString})(\.(?<additonal>{suffixPartRegexString}))*";

        private static readonly string versioRegexString =
            $@"(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<patch>[0-9]+)(-(?<preRelease>[0-9,A-Z,a-z,\.]+))?(\+(?<metadata>[0-9,A-Z,a-z,\.]+))?";

        private static readonly Regex versioRegex = new Regex($"^{versioRegexString}$",
             RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex suffixPartRegex = new Regex($"^{suffixPartRegexString}$",
             RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex versionSuffixRegex = new Regex($"^{versionSuffixRegexString}$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        public static bool TryParseVersion(string input,
            out int major, out int minor, out int patch,
            out PreReleaseIdentifier? preRelease, out VersionMetadata? metadata)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var match = versioRegex.Match(input);
            if (!match.Success)
            {
                major = minor = patch = 0;
                preRelease = null;
                metadata = null;
                return false;
            }
            major = int.Parse(match.Groups["major"].Value);
            minor = int.Parse(match.Groups["minor"].Value);
            patch = int.Parse(match.Groups["patch"].Value);

            var metadataMatch = match.Groups["metadata"];
            metadata = metadataMatch.Success ?
                new VersionMetadata(metadataMatch.Value) :
                (VersionMetadata?)null;

            var preReleaseMatch = match.Groups["preRelease"];
            preRelease = preReleaseMatch.Success ?
                new PreReleaseIdentifier(preReleaseMatch.Value) :
                (PreReleaseIdentifier?)null;
            return true;
        }

        public static bool IsValidSuffixPart(this string part)
        {
            if (part == null)
            {
                throw new ArgumentNullException(nameof(part));
            }
            return suffixPartRegex.IsMatch(part);
        }

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