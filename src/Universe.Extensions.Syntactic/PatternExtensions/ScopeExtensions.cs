using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using  Universe.Extensions.Syntactic.Patterns;

namespace  Universe.Extensions.Syntactic.PatternExtensions;

public static class ScopeExtensions
{
    extension<T>(T self)
        where T : class
    {
        [Experimental("ScopeFunction")]
        public T Apply(Mutator<T> mutator)
        {
            mutator(self);
            return self;
        }
    }

    extension<T>(T self)
        where T : struct
    {
        [Experimental("ScopeFunction")]
        public ref T Apply(RefMutator<T> mutator)
        {
            mutator(ref self);
            return ref Unsafe.AsRef(in self);
        }
    }
}
