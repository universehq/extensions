using System.Collections.Generic;

namespace Universe.Extensions.Syntactic;

public static class IntExtensions
{
    public static IEnumerable<int> To(this int start, int end)
    {
        for (int i = start; i <= end; i++)
            yield return i;
    }
}
