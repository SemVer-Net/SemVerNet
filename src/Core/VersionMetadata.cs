using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Helpers;

namespace Core
{
    public struct VersionMetadata : IEquatable<VersionMetadata>
    {
        private readonly string metadataString;

        public VersionMetadata(params string[] metadataParts)
        {
            if (metadataParts == null)
            {
                throw new ArgumentNullException(nameof(metadataParts));
            }
            metadataString = string.Join(".", metadataParts);
            if (metadataParts.Any(part => !part.IsValidSuffixPart()))
            {
                throw new ArgumentException($"Invalid metadataParts '{metadataString}'");
            }
            Parts = metadataParts;
        }

        public VersionMetadata(string metadataString)
        {
            string[] parts;
            if (!VersionHelpers.TryParseVersionSuffix(metadataString, out parts))
            {
                throw new ArgumentException($"Invalid metadata. String '{metadataString}', does not match requirements");
            }
            this.metadataString = metadataString;
            Parts = parts;
        }

        public string[] Parts { get; }

        public static bool operator ==(VersionMetadata operand1, VersionMetadata operand2)
        {
            return operand1.Equals(operand2);
        }

        public static bool operator !=(VersionMetadata operand1, VersionMetadata operand2)
        {
            return !operand1.Equals(operand2);
        }

        public override int GetHashCode()
        {
            return metadataString == null ? 0 : metadataString.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is VersionMetadata ? Equals((VersionMetadata)obj) : false;
        }

        public bool Equals(VersionMetadata other)
        {
            return metadataString == other.metadataString;
        }

        public override string ToString()
        {
            return metadataString;
        }
    }
}