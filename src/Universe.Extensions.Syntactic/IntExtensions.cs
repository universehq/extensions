using System.Collections.Generic;

namespace Universe.Extensions.Syntactic;

public static class IntExtensions
{
    public static IEnumerable<byte> To(this byte start, byte end)
    {
        for (byte i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<sbyte> To(this sbyte start, sbyte end)
    {
        for (sbyte i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<short> To(this short start, short end)
    {
        for (short i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<ushort> To(this ushort start, ushort end)
    {
        for (ushort i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<char> To(this char start, char end)
    {
        for (char i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<int> To(this int start, int end)
    {
        for (int i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<uint> To(this uint start, uint end)
    {
        for (uint i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<long> To(this long start, long end)
    {
        for (long i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<ulong> To(this ulong start, ulong end)
    {
        for (ulong i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<nint> To(this nint start, nint end)
    {
        for (nint i = start; i <= end; i++)
            yield return i;
    }

    public static IEnumerable<nuint> To(this nuint start, nuint end)
    {
        for (nuint i = start; i <= end; i++)
            yield return i;
    }
}
