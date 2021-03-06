﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoopSimulation
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var element in collection)
			{
				action(element);
			}
			return collection;
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
		{
			var idx = 0;
			foreach (var element in collection)
			{
				action(element, idx++);
			}
			return collection;
		}

		public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
		{
			if (list == null) throw new ArgumentNullException(nameof(list));
			if (items == null) throw new ArgumentNullException(nameof(items));

			if (list is List<T> asList)
			{
				asList.AddRange(items);
			}
			else
			{
				foreach (var item in items)
				{
					list.Add(item);
				}
			}
		}

		public static void RemoveAll<T>(this IList<T> iList, IEnumerable<T> itemsToRemove)
		{
			var set = new HashSet<T>(itemsToRemove);

			var list = iList as List<T>;
			if (list == null)
			{
				int i = 0;
				while (i < iList.Count)
				{
					if (set.Contains(iList[i]))
					{
						iList.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				list.RemoveAll(set.Contains);
			}
		}
	}
}
