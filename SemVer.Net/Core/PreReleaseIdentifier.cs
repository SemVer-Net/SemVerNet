using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{

	public struct PreReleaseIdentifier
	{
		private const string regexString = @"^(?<main>[0-9,A-Z,a-z])+(?<additonal>\.[0-9,A-Z,a-z]+)*$";
		private static readonly Regex identifierStringRegex = new Regex(regexString, RegexOptions.Compiled);
		private readonly string identifierString;
		public PreReleaseIdentifier(string identifierString)
		{
			if (identifierString == null)
			{
				throw new ArgumentNullException(nameof(identifierString));
			}
			if (!identifierStringRegex.IsMatch(identifierString))
			{
				throw new ArgumentException($"Invalid prerelease identifier. String '{identifierString}', does not match requirements");
			}
			this.identifierString = identifierString;
		}
	}
}
