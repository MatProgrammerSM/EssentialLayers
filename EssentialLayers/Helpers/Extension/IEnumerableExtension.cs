﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EssentialLayers.Helpers.Extension
{
	public static class IEnumerableExtension
	{
		public static bool NotAny(
			this object self, IEnumerable<object> values
		)
		{
			return values.Any(x => x.ToString().Equals(self.ToString())).False();
		}

		public static bool IsEmpty<T>(
			this IEnumerable<T> list
		)
		{
			if (list == null)
			{
				return true;
			}

			return list.Any().False();
		}

		public static bool SingleOne<T>(
			this IEnumerable<T> list
		)
		{
			if (list == null)
			{
				return false;
			}

			return list.Count() == 1;
		}

		public static bool NotEmpty<T>(
			this IEnumerable<T> list
		)
		{
			if (list == null)
			{
				return false;
			}

			return list.Any();
		}

		public static bool AddIf<T>(
			this IList<T> lst, T data, Func<T, bool> check
		)
		{
			if (lst.All(check).False()) return false;

			lst.Add(data);

			return true;
		}

		public static bool AreEquals<T, K>(
			this IEnumerable<T> self, IEnumerable<K> other
		)
		{
			string serializedSelf = self.Serialize();
			string serializedOther = other.Serialize();

			return serializedSelf.Equals(serializedOther);
		}

		public static bool NotAny<T>(
			this IEnumerable<T> items, Func<T, bool> predicate
		)
		{
			return items.Any(predicate).False();
		}

		public static string ToStringJoin<T>(
			this IEnumerable<T> self
		)
		{
			return string.Join(", ", self);
		}

		public static string Join<T>(
			this IEnumerable<T> self, string separator = ", "
		)
		{
			return string.Join(separator, self);
		}

		public static Dictionary<string, TValue> ToDictionary<TValue, T>(this T self)
		{
			if (self != null)
			{
				Dictionary<string, TValue> dictionary = [];

				foreach (PropertyInfo property in self.GetType().GetProperties())
				{
					TValue value = (TValue)property.GetValue(self, null);

					string key = property.Name;

					dictionary.Add(key, value);
				}

				return dictionary;
			}

			return [];
		}
	}
}