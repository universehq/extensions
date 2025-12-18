using System.Diagnostics.CodeAnalysis;

namespace Universe.Extensions.Syntactic.Patterns;

[Experimental("Mutator")]
public delegate void Mutator<T>(T value);

[Experimental("RefMutator")]
public delegate void RefMutator<T>(ref T value);
