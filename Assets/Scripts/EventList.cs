using System;
using System.Collections;
using System.Collections.Generic;

public class EventList<T> : IList<T>
{
    List<T> _list = new();

    public T this[int index] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public int Count => _list.Count;

    public bool IsReadOnly => throw null;

    public event Action<T> OnItemAdded;
    public event Action<T> OnItemRemoved;
    public event Action OnListCleared;

    public EventList(List<T> list = null)
    {
        if (_list == null) list = new List<T>();

        _list = list;
    }

    public static explicit operator EventList<T>(List<T> v)
    {
        return new EventList<T>(v);
    }

    public void Add(T item)
    {
        _list.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public void Clear()
    {
        _list.Clear();
        OnListCleared?.Invoke();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        OnItemAdded?.Invoke(item);
    }

    public bool Remove(T item)
    {
        bool removed = _list.Remove(item);
        if (removed) OnItemRemoved?.Invoke(item);
        return removed;
    }

    public void RemoveAt(int index)
    {
        if (index < _list.Count) OnItemRemoved?.Invoke(_list[index]);
        _list.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}
