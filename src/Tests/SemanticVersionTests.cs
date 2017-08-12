using System;
using System.Collections.Generic;
using Core;
using Xunit;

namespace Tests
{
	public class SemanticVersionTests
	{
		[Fact]
		public void IdentifierCanNotBeEmpty()
		{
			Assert.Throws<ArgumentException>(()=> 
				new SemanticVersion(1, 2, 3, new PreReleaseIdentifier()));
		}

        [Theory]
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
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some")),
                    new SemanticVersion(1, 2, 3, null) };
                yield return new object[] {
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some")),
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("other")) };
                yield return new object[] {
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("some.1")),
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("other.2")) };
            }
        }

        [Theory]
        [MemberData(nameof(VersionCompareTests))]
        public void VersionCompare(SemanticVersion version1, SemanticVersion version2, int expectedCompareDistance)
        {
            Assert.Equal(expectedCompareDistance, version1.CompareTo(version2));
        }


        public static IEnumerable<object[]> VersionCompareTests
        {
            get
            {
                yield return new object[] {
                    new SemanticVersion(1, 10, 10, null),
                    new SemanticVersion(2, 0, 1, null), -1 };
                yield return new object[] {
                    new SemanticVersion(2, 2, 3, null),
                    new SemanticVersion(1, 10, 10, null), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 10, null),
                    new SemanticVersion(1, 2, 1, null), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 2, 3, null),
                    new SemanticVersion(1, 1, 10, null), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 2, 3, null),
                    new SemanticVersion(1, 2, 3, null), 0 };
                yield return new object[] {
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 2, 3, new PreReleaseIdentifier("beta")), 0 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 1, 2, new PreReleaseIdentifier("alpha")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("alpha")),
                    new SemanticVersion(1, 1, 2, new PreReleaseIdentifier("beta")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("alpha")),
                    new SemanticVersion(1, 1, 1, null), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, null),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("alpha")), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("alpha")), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("1")), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.1")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.1")), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.10")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.z")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.1")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("alpha.2.1")), 1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.1")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.2")), -1 };
                yield return new object[] {
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.a")),
                    new SemanticVersion(1, 1, 1, new PreReleaseIdentifier("beta.2.2")), 1 };
            }
        }
    }
}
