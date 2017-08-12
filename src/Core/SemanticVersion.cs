using System;

namespace Core
{
	public struct SemanticVersion 
		: IEquatable<SemanticVersion>, IComparable<SemanticVersion>
	{
		public SemanticVersion(int major, int minor, int patch, PreReleaseIdentifier? preRelease = null)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
			if(preRelease.HasValue && preRelease.Value == default(PreReleaseIdentifier))
			{
				throw new ArgumentException("Pre release identifier cannot be empty");
			}

			PreRelease = preRelease;
		}

		public int Major { get; }

		public int Minor { get; }

		public int Patch { get; }

		public PreReleaseIdentifier? PreRelease { get; }


		public static bool operator == (SemanticVersion operand1, SemanticVersion operand2)
		{
			return operand1.Equals(operand2);
		}

		public static bool operator != (SemanticVersion operand1, SemanticVersion operand2)
		{
			return !operand1.Equals(operand2);
		}

		public static bool operator >  (SemanticVersion operand1, SemanticVersion operand2)
		{
			return operand1.CompareTo(operand2) == 1;
		}

		public static bool operator <  (SemanticVersion operand1, SemanticVersion operand2)
		{
			return operand1.CompareTo(operand2) == -1;
		}

		public static bool operator >=  (SemanticVersion operand1, SemanticVersion operand2)
		{
			return operand1.CompareTo(operand2) >= 0;
		}

		public static bool operator <=  (SemanticVersion operand1, SemanticVersion operand2)
		{
			return operand1.CompareTo(operand2) <= 0;
		}

		public override string ToString()
		{ 
			return PreRelease.HasValue? 
				$"{Major}.{Minor}.{Patch}-{PreRelease.ToString()}":
				$"{Major}.{Minor}.{Patch}";
		}

		public override int GetHashCode()
		{
			return 
				Major +
				Minor +
				Patch +
				(PreRelease.HasValue? PreRelease.GetHashCode(): 0);
		}

        public override bool Equals(object obj)
		{
			return obj is SemanticVersion ? Equals((SemanticVersion)obj) : false;
		}

		public bool Equals(SemanticVersion other)
		{
            return this.Major == other.Major
				&& this.Minor == other.Minor
				&& this.Patch == other.Patch
				&& (this.PreRelease.HasValue ?
					other.PreRelease.HasValue && this.PreRelease.Value.Equals(other.PreRelease.Value) :
					!other.PreRelease.HasValue);
		}
        public int CompareTo(SemanticVersion other)
        {
            return 
				this.Major > other.Major? 1:
				this.Major < other.Major? -1:
				this.Minor > other.Minor? 1:
				this.Minor < other.Minor? -1:
				this.Patch > other.Patch? 1:
				this.Patch < other.Patch? -1:
				(this.PreRelease.HasValue && !other.PreRelease.HasValue)? -1:
				(!this.PreRelease.HasValue && other.PreRelease.HasValue)? 1:
				(!this.PreRelease.HasValue && !other.PreRelease.HasValue)? 0:
				this.PreRelease.Value.CompareTo(other.PreRelease.Value);
        }
	}
}
