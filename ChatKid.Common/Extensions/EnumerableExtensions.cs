using System.Collections.Concurrent;

namespace ChatKid.Common.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// If the enumerable is null, returns an empty enumerable.  Otherwise returns the enumerable.
        /// </summary>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return Enumerable.Empty<T>();
            return source;
        }

        /// <summary>
        /// If the collection is null, returns an empty collection.  Otherwise returns the collection.
        /// </summary>
        public static IReadOnlyCollection<T> EmptyIfNull<T>(this IReadOnlyCollection<T> source)
        {
            if (source == null)
                return Array.Empty<T>();
            return source;
        }

        /// <summary>
        /// If the array is null, returns an empty collection.  Otherwise returns the array.
        /// </summary>
        public static IReadOnlyCollection<T> EmptyIfNull<T>(this T[] source)
        {
            if (source == null)
                return Array.Empty<T>();
            return source;
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T replaceItem, IEnumerable<T> withItems)
        {
            var sourceList = new List<T>(source);
            var indices = Enumerable.Range(0, sourceList.Count)
                .Where(i => sourceList[i].Equals(replaceItem))
                .Reverse();

            foreach (var index in indices)
            {
                sourceList.RemoveAt(index);
                sourceList.InsertRange(index, withItems);
            }
            return sourceList;
        }

        /// <summary>
        /// A safer way to return a nullable object.  If you do DefaultIfEmpty on a enumerable of value types (i.e.
        /// IEnumerable&gt;int&lt;) you will, suprisingly, get back 0 if the enumerable is empty which probably isn't
        /// what you want.  If you did it on a enumerable of reference types (i.e. IEnumerable&gt;string&lt;) you'd 
        /// get null.
        /// 
        /// So, for value types, use this method to get back null in that case.
        /// </summary>
        public static T? FirstOrNullable<T>(this IEnumerable<T> source)
            where T : struct
        {
            return source.EmptyIfNull().Select(x => (T?)x).FirstOrDefault();
        }

        /// <summary>
        /// Creates a set from a enumerable.
        /// </summary>
        public static ISet<T> ToSet<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return new HashSet<T>();
            return new HashSet<T>(source);
        }

        /// <summary>
        /// Creates a set from a enumerable, converting the item into a new item.
        /// </summary>
        public static HashSet<TNewItem> ToSet<TItem, TNewItem>(this IEnumerable<TItem> enumerable, Func<TItem, TNewItem> selector)
        {
            return new HashSet<TNewItem>(enumerable.Select(selector));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, int degreeOfParallelism, Func<T, Task> action)
        {
            return Task.WhenAll(Partitioner.Create(source).GetPartitions(degreeOfParallelism).Select(partition => Task.Run(async () =>
            {
                using (partition)
                    while (partition.MoveNext())
                        await action(partition.Current);
            })));
        }

        /// <summary>
        /// Takes an IEnumerable and returns another IEnumerable that has the same elements but combines each element
        /// with their index in the form of a tuple.
        /// </summary>
        public static IEnumerable<Tuple<int, T>> ZipWithIndexes<T>(this IEnumerable<T> enumerable)
        {
            var positiveIntegers = Enumerable.Range(0, int.MaxValue);
            return positiveIntegers.Zip(enumerable, (i, e) => Tuple.Create(i, e));
        }

        /// <summary>
        /// Partitions an enumerable into two groups based on the passed predicate.  The result is a tuple containing
        /// the list of all elements for which the predicate returned true, and then the list of all elements for which
        /// it returned false.
        /// </summary>
        public static Tuple<IEnumerable<T>, IEnumerable<T>> Partition<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            var trueList = new LinkedList<T>();
            var falseList = new LinkedList<T>();
            foreach (var element in enumerable)
            {
                if (predicate(element))
                    trueList.AddLast(element);
                else
                    falseList.AddLast(element);
            }
            return Tuple.Create((IEnumerable<T>)trueList, (IEnumerable<T>)falseList);
        }

        public static string ToDelimited<T>(this IEnumerable<T> enumerable, string delimiter = ", ")
        {
            return string.Join(delimiter, enumerable.EmptyIfNull());
        }

        public static string ToDelimited<T>(this IEnumerable<T> enumerable, char delimiter)
        {
            return enumerable.ToDelimited(delimiter.ToString());
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> enumerable, int partitionSize)
        {
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return enumerator.TakeFromCurrent(partitionSize);
            }
        }

        public static IEnumerable<T> TakeFromCurrent<T>(this IEnumerator<T> enumerator, int count)
        {
            while (count > 0)
            {
                yield return enumerator.Current;
                if (--count > 0 && !enumerator.MoveNext()) yield break;
            }
        }

        public static IEnumerable<T> SelectAllElements<T>(this IEnumerable<T> collection, Func<T, IEnumerable<T>> predicate)
        {
            var elementToProcess = new Stack<T>(collection);
            while (elementToProcess.Count > 0)
            {
                var e = elementToProcess.Pop();
                var children = predicate(e).EmptyIfNull();
                foreach (var c in children)
                {
                    elementToProcess.Push(c);
                }

                yield return e;
            }
        }
    }
}
