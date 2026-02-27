using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Carbon.Css;

/// <summary>
/// A small buffer optimized for 1–4 items with zero heap allocation.
/// Overflows to a lazily allocated list for larger collections.
/// </summary>
internal struct SmallList<T>
{
    private T? _item0;
    private T? _item1;
    private T? _item2;
    private T? _item3;
    private List<T>? _overflow;
    private int _count;

    public readonly int Count => _count;

    public void Add(T item)
    {
        switch (_count)
        {
            case 0: _item0 = item; break;
            case 1: _item1 = item; break;
            case 2: _item2 = item; break;
            case 3: _item3 = item; break;
            default:
                _overflow ??= new List<T>(4);
                _overflow.Add(item);
                break;
        }

        _count++;
    }

    public readonly T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if ((uint)index >= (uint)_count)
            {
                ThrowOutOfRange();
            }

            return index switch {
                0 => _item0!,
                1 => _item1!,
                2 => _item2!,
                3 => _item3!,
                _ => _overflow![index - 4]
            };
        }
    }

    [DoesNotReturn]
    private static void ThrowOutOfRange() =>
       throw new ArgumentOutOfRangeException("index");

    /// <summary>
    /// Copies all items into a new array.
    /// </summary>
    public readonly T[] ToArray()
    {
        var array = new T[_count];

        for (int i = 0; i < _count; i++)
        {
            array[i] = this[i];
        }

        return array;
    }

    internal void Clear()
    {
        _count = 0;
    }
}
