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
    public interface INonemptyList<T> : IImmutableList<T>
    {
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

        public IImmutableList<T> Clear()
        {
            return _list.Clear();
        }

        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return _list.IndexOf(item, index, count, equalityComparer);
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            return _list.LastIndexOf(item, index, count, equalityComparer);
        }

        public IImmutableList<T> Add(T value)
        {
            return _list.Add(value);
        }

        public IImmutableList<T> AddRange(IEnumerable<T> items)
        {
            return _list.AddRange(items);
        }

        public IImmutableList<T> Insert(int index, T element)
        {
            return _list.Insert(index, element);
        }

        public IImmutableList<T> InsertRange(int index, IEnumerable<T> items)
        {
            return _list.InsertRange(index, items);
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

        public IImmutableList<T> SetItem(int index, T value)
        {
            return _list.SetItem(index, value);
        }

        public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer)
        {
            return _list.Replace(oldValue, newValue, equalityComparer);
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
