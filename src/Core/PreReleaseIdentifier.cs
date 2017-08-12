using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
	public struct PreReleaseIdentifier
		: IEquatable<PreReleaseIdentifier>, IComparable<PreReleaseIdentifier>
	{
		private const string regexString = @"^(?<main>[0-9,A-Z,a-z]+)(\.(?<additonal>[0-9,A-Z,a-z]+))*$";
		private static readonly Regex identifierStringRegex = new Regex(regexString, 
			RegexOptions.Compiled | RegexOptions.Singleline);
		private readonly string identifierString;
		private readonly string[] identifierParts;
		public PreReleaseIdentifier(string identifierString)
		{
			if (identifierString == null)
			{
				throw new ArgumentNullException(nameof(identifierString));
			}
			var parsed = identifierStringRegex.Match(identifierString);
			if (!parsed.Success)
			{
				throw new ArgumentException($"Invalid prerelease identifier. String '{identifierString}', does not match requirements");
			}
			this.identifierString = identifierString;
			var main = parsed.Groups["main"].Value;
			var additonal = parsed.Groups["additonal"].Captures.Cast<Capture>().Select(c => c.Value).ToArray();
			identifierParts = new string[additonal.Length + 1];
			identifierParts[0] = main;
			Array.Copy(additonal, 0, identifierParts, 1, additonal.Length);
		}

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
			var length = Math.Min(identifierParts.Length,other.identifierParts.Length);
            for(int i = 0; i < length; i++)
			{
				var partDiff = CompareParts(identifierParts[i], other.identifierParts[i]);
				if(partDiff != 0)
				{
					return partDiff;
				}
			}
			return 
				(identifierParts.Length == other.identifierParts.Length)? 0:
				(identifierParts.Length > other.identifierParts.Length)? 1: -1;
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
