using System.Collections.Generic;
using System.Linq;

namespace HolidayShowdown.Models;
public class RankedList<T> where T : IComparable<T>
{
    private readonly List<T> _items = new();

    public IReadOnlyList<T> Items => _items;

    public void Add(T item) => _items.Add(item);

    public void Clear() => _items.Clear();

    public IReadOnlyList<T> Ranked => _items.OrderByDescending(i => i).ToList();

    public int Count => _items.Count;
}