using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SemVer.Net.Core.Helpers;

namespace SemVer.Net.Core
{
	public struct PreReleaseIdentifier
		: IEquatable<PreReleaseIdentifier>, IComparable<PreReleaseIdentifier>
	{
		private readonly string identifierString;
        
        public PreReleaseIdentifier(params string[] identifierParts)
        {
            if (identifierParts == null)
            {
                throw new ArgumentNullException(nameof(identifierParts));
            }
            identifierString = string.Join(".", identifierParts);
            if (identifierParts.Any(part => !part.IsValidSuffixPart()))
            {
                throw new ArgumentException($"Invalid metadataParts '{identifierString}'");
            }
            Parts = identifierParts;
        }

        public PreReleaseIdentifier(string identifierString)
		{
			string[] parts;
			if (!VersionHelpers.TryParseVersionSuffix(identifierString, out parts))
			{
				throw new ArgumentException($"Invalid prerelease identifier. String '{identifierString}', does not match requirements");
			}
            this.identifierString = identifierString;
			Parts = parts;
		}

		public string[] Parts { get; }

		public static bool operator == (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return operand1.Equals(operand2);
		}

		public static bool operator != (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return !operand1.Equals(operand2);
		}

		public static bool operator >  (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return operand1.CompareTo(operand2) == 1;
		}

		public static bool operator <  (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return operand1.CompareTo(operand2) == -1;
		}

		public static bool operator >=  (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return operand1.CompareTo(operand2) >= 0;
		}

		public static bool operator <=  (PreReleaseIdentifier operand1, PreReleaseIdentifier operand2)
		{
			return operand1.CompareTo(operand2) <= 0;
        }

        public static implicit operator string(PreReleaseIdentifier version)
        {
            return version.ToString();
        }

        public static implicit operator PreReleaseIdentifier(string versionString)
        {
            return new PreReleaseIdentifier(versionString);
        }

        public override string ToString()
		{
			return identifierString;
		}

		public override int GetHashCode()
		{
			return identifierString == null? 0: identifierString.GetHashCode();
		}

        public override bool Equals(object obj)
		{
			return obj is PreReleaseIdentifier ? Equals((PreReleaseIdentifier)obj) : false;
		}

        public bool Equals(PreReleaseIdentifier other)
        {
            return identifierString == other.identifierString;
        }

        public int CompareTo(PreReleaseIdentifier other)
        {
			var length = Math.Min(Parts.Length,other.Parts.Length);
            for(int i = 0; i < length; i++)
			{
				var partDiff = CompareParts(Parts[i], other.Parts[i]);
				if(partDiff != 0)
				{
					return partDiff;
				}
			}
			return 
				(Parts.Length == other.Parts.Length)? 0:
				(Parts.Length > other.Parts.Length)? 1: -1;
        }

		private int CompareParts(string thisPart, string otherPart)
		{
			int thisNumPart;
			bool isThisPartNumeric = int.TryParse(thisPart, out thisNumPart);
			int otherNumPart;
			bool isOtherPartNumeric = int.TryParse(otherPart, out otherNumPart);
			
			if(isThisPartNumeric)
			{
				return isOtherPartNumeric?
					thisNumPart.CompareTo(otherNumPart): -1;
			}
			return isOtherPartNumeric? 1: thisPart.CompareTo(otherPart);
		}
    }
}
