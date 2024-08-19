using System.Text.RegularExpressions;

namespace EssentialLayers.Helpers.Extension
{
	public static class StringExtension
	{
		public static bool IsAny(this string self, params string[] values)
		{
			return values.Any(item => item == self);
		}

		public static bool IsAny(this string self, IEnumerable<string> values)
		{
			return values.Any(item => item == self);
		}

		public static bool NotAny(this string self, params string[] values)
		{
			return values.All(item => item != self);
		}

		public static bool NotAny(this string self, IEnumerable<string> values)
		{
			return values.All(item => item != self);
		}

		public static bool IsEmpty(this string self)
		{
			return string.IsNullOrEmpty(self);
		}

		public static bool NotEmpty(this string self)
		{
			return !string.IsNullOrEmpty(self);
		}

		public static bool Match(this string self, string pattern)
		{
			return new Regex(pattern).IsMatch(self);
		}

		public static bool NoMatch(this string self, string pattern)
		{
			return !new Regex(pattern).IsMatch(self);
		}

		public static bool NotEquals(this string self, string another)
		{
			if (self != null)
			{
				return !self.Equals(another);
			}

			return false;
		}

		public static string RegexReplace(this string self, string pattern, string replacement)
		{
			return new Regex(pattern).Replace(self, replacement);
		}

		public static string Capitalize(this string? self, string separator = " ")
		{
			string[] result = [];

			if (self != null)
			{
				string[] words = self.Split(separator);

				foreach (string word in words)
				{
					result.Append(char.ToUpper(word[0]) + word[1..].ToLower());
				}
			}

			return string.Join(separator, result);
		}

		public static bool ContainsAny(this string self, params string[] values)
		{
			return values.Any(value => value.Contains(self));
		}

		public static bool HasDigits(this string self)
		{
			if (self.IsEmpty()) return false;

			return self.Any(char.IsDigit);
		}

		public static bool HasLetters(this string self)
		{
			if (self.IsEmpty()) return false;

			return self.Any(char.IsLetter);
		}

		public static bool MinLength(this string self, int length)
		{
			return self.Length >= length;
		}

		public static bool Length(this string self, int length)
		{
			return self.Length == length;
		}

		public static bool MaxLength(this string self, int length)
		{
			return self.Length <= length;
		}

		public static string FirstUpperWord(this string self)
		{
			string first = string.Empty;

			foreach (char s in self)
			{
				if (s.ToString().Match("^[A-Z]*$")) break;

				first += s;
			}

			return first;
		}

		public static string WrapText(this string self, int n, bool wrap = true)
		{
			string value = self;

			if (wrap && self.NotNull() && self.Length > n) value = self[..n] + "...";

			return value;
		}

		public static string LeftSpaces(this string self, int length)
		{
			if (self.IsEmpty()) return string.Empty.PadLeft(length);

			return self.Length <= length ? self.PadLeft(length) : self[..length];
		}

		public static string RightSpaces(this string self, int length)
		{
			if (self.IsEmpty()) return string.Empty.PadRight(length);

			return self.Length <= length ? self.PadLeft(length) : self[..length];
		}
	}
}