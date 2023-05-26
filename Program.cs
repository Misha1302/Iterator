using System.Collections;
using System.Text;

SimpleList<int> ints = new();
for (int i = 0; i < 3; i++)
    ints.Add(Random.Shared.Next(0, 10));

Console.WriteLine(ints.ToString());


public class SimpleList<T> : IReadOnlyList<T>
{
    private const float Multiplier = 1.75f;
    private const int ItemsDefaultCapacity = 4;
    private T[] _items;


    public int Count { get; private set; }
    public T this[int index] => _items[index];
    public int Capacity { get; private set; } = ItemsDefaultCapacity;


    public SimpleList()
    {
        _items = new T[ItemsDefaultCapacity];
    }

    public void Add(T item)
    {
        _items[Count++] = item;
        ResizeIfNeed();
    }

    private void ResizeIfNeed()
    {
        if (Count >= Capacity)
        {
            int newCapacity = (int)(Capacity * Multiplier);
            Capacity = newCapacity;
            Array.Resize(ref _items, newCapacity);
        }
    }

    public override string ToString()
    {
        return this.ToStringExt();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new SimpleIEnumerator(_items);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new SimpleIEnumerator(_items);
    }

    private class SimpleIEnumerator : IEnumerator<T>
    {
        private readonly T[] _items;
        private int _index = -1;

        public SimpleIEnumerator(T[] items)
        {
            _items = items;
        }

        public T Current => _items[_index];

        object? IEnumerator.Current => Current;

        public void Dispose()
        {
            // Nothing to dispose of
            // GC will do all the work for us
        }

        public bool MoveNext()
        {
            _index++;
            return (_index + 1) < _items.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}

public static class Extensions
{
    public static string ToStringExt<T>(this SimpleList<T> values)
    {
        StringBuilder sb = new StringBuilder();

        foreach (T i in values)
            sb.Append(i + ", ");

        sb.Remove(sb.Length - 2, 2);
        return sb.ToString();
    }
}