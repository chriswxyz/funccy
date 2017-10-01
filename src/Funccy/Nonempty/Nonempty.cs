using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Funccy
{
    /// <summary>
    /// An immutable list that always has at least one element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INonemptyList<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>
    {
        /// <summary>
        /// Gets the value at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        T this[int index] { get; }

        /// <summary>
        /// Finds the index of a value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer);

        /// <summary>
        /// Finds the last index of a value.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer);

        /// <summary>
        /// Adds a value to the list.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        INonemptyList<T> Add(T value);

        /// <summary>
        /// Adds values to the list.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        INonemptyList<T> AddRange(IEnumerable<T> items);

        /// <summary>
        /// Inserts a value at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        INonemptyList<T> Insert(int index, T element);

        /// <summary>
        /// Inserts values at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        INonemptyList<T> InsertRange(int index, IEnumerable<T> items);

        /// <summary>
        /// Replaces the value at index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        INonemptyList<T> SetItem(int index, T value);

        /// <summary>
        /// Replaces the first matching value in the list.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        INonemptyList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer);

        /// <summary>
        /// Removes the first instance of the value.
        /// Removal of items is not guaranteed to result in another nonempty list.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        IImmutableList<T> Remove(T value, IEqualityComparer<T> equalityComparer);

        /// <summary>
        /// Removes all instances matching the predicate.
        /// Removal of items is not guaranteed to result in another nonempty list.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        IImmutableList<T> RemoveAll(Predicate<T> match);

        /// <summary>
        /// Removes all instances matching the given items.
        /// Removal of items is not guaranteed to result in another nonempty list.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="equalityComparer"></param>
        /// <returns></returns>
        IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer);

        /// <summary>
        /// Removes count values starting at index.
        /// Removal of items is not guaranteed to result in another nonempty list.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IImmutableList<T> RemoveRange(int index, int count);

        /// <summary>
        /// Removes the value at index.
        /// Removal of items is not guaranteed to result in another nonempty list.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IImmutableList<T> RemoveAt(int index);
    }

    /// <summary>
    /// An immutable list that always has at least one element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class NonemptyList<T> : INonemptyList<T>
    {
        private readonly IImmutableList<T> _list = ImmutableList<T>.Empty;

        public int Count => _list.Count;

        public T this[int index] => _list[index];

        private NonemptyList() { }

        public NonemptyList(T first)
        {
            _list = _list.Add(first);
        }

        public NonemptyList(T first, IEnumerable<T> rest)
        {
            _list = _list
                .Add(first)
                .AddRange(rest);
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return _list.IndexOf(item, index, count, equalityComparer);
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return _list.LastIndexOf(item, index, count, equalityComparer);
        }

        public INonemptyList<T> Add(T value)
        {
            return _list.Add(value)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public INonemptyList<T> AddRange(IEnumerable<T> items)
        {
            return _list.AddRange(items)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public INonemptyList<T> Insert(int index, T element)
        {
            return _list.Insert(index, element)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public INonemptyList<T> InsertRange(int index, IEnumerable<T> items)
        {
            return _list.InsertRange(index, items)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public IImmutableList<T> Remove(T value, IEqualityComparer<T> equalityComparer)
        {
            return _list.Remove(value, equalityComparer);
        }

        public IImmutableList<T> RemoveAll(Predicate<T> match)
        {
            return _list.RemoveAll(match);
        }

        public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            return _list.RemoveRange(items, equalityComparer);
        }

        public IImmutableList<T> RemoveRange(int index, int count)
        {
            return _list.RemoveRange(index, count);
        }

        public IImmutableList<T> RemoveAt(int index)
        {
            return _list.RemoveAt(index);
        }

        public INonemptyList<T> SetItem(int index, T value)
        {
            return _list.SetItem(index, value)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public INonemptyList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
        {
            return _list.Replace(oldValue, newValue, equalityComparer)
                .AsNonemptyList()
                .Extract(() => throw new InvalidOperationException());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
