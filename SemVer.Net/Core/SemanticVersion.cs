using System;

namespace Core
{
	public struct SemanticVersion //: IEquatable<SemanticVersion>
	{
		public SemanticVersion(int major, int minor, int patch, PreReleaseIdentifier? preRelease = null)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
			PreRelease = preRelease;
		}

		public int Major { get; }

		public int Minor { get; }

		public int Patch { get; }

		public PreReleaseIdentifier? PreRelease { get; }

		//public override bool Equals(object obj)
		//{
		//	return obj is SemanticVersion ? Equals((SemanticVersion)obj) : false;
		//}

		//public bool Equals(SemanticVersion other)
		//{
		//	return this.Major == other.Major
		//		&& this.Minor == other.Minor
		//		&& this.Patch == other.Patch
		//		&& this.PreRelease.HasValue ?
		//			other.PreRelease.HasValue && this.PreRelease.Value.Equals(other.PreRelease.Value) :
		//			!other.PreRelease.HasValue;
		//}
	}
}
