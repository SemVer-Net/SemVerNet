using System;
using System.Collections.Generic;
using Core;
using Xunit;

namespace Tests
{
	public class SemanticVersionTests
	{
		[Theory()]
		[MemberData(nameof(PreVersionIdentifierValidationTests))]
		public void PreVersionIdentifierValidations(string identifierString)
		{
			Assert.ThrowsAny<ArgumentException>(() => new PreReleaseIdentifier(identifierString));
		}
		
		public static IEnumerable<object[]> PreVersionIdentifierValidationTests
		{
			get
			{
				yield return new object[] { "1..2" };
				yield return new object[] { "s$ome" };
				yield return new object[] { "Pre Version" };
				yield return new object[] { " " };
				yield return new object[] { "" };
				yield return new object[] { null };
			}
		}

		[Theory]
		[MemberData(nameof(EqualityTests))]
		public void VersionEquality(SemanticVersion version1, SemanticVersion version2)
		{
			Assert.Equal(version1, version2);
		}

		public static IEnumerable<object[]> EqualityTests
		{
			get
			{
				yield return new object[] {
					new SemanticVersion(), new SemanticVersion(0, 0, 0, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, null), new SemanticVersion(1, 2, 3, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier()),
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier()) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some")),
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some")) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some.1")),
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some.1")) };
			}
		}

		[Theory]
		[MemberData(nameof(InEqualityTests))]
		public void VersioInEquality(SemanticVersion version1, SemanticVersion version2)
		{
			Assert.NotEqual(version1, version2);
		}

		public static IEnumerable<object[]> InEqualityTests
		{
			get
			{
				yield return new object[] {
					new SemanticVersion(1, 2, 3, null),
					new SemanticVersion(2, 2, 3, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, null),
					new SemanticVersion(1, 1, 3, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, null),
					new SemanticVersion(1, 2, 4, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier()),
					new SemanticVersion(1, 2, 3, null) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some")),
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("other")) };
				yield return new object[] {
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some.1")),
					new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("other.2")) };
			}
		}
	}
}
