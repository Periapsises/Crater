using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer;

public class Scope(Scope? parent = null)
{
    private readonly Dictionary<string, Symbol> _symbols = [];

    public void Declare(string name, Symbol symbol)
    {
        _symbols[name] = symbol;
    }

    public bool HasSymbol(string name)
    {
        return _symbols.ContainsKey(name);
    }

    public bool TryGetSymbol(string name, [NotNullWhen(true)] out Symbol? symbol)
    {
        if (_symbols.TryGetValue(name, out symbol))
            return true;

        if (parent != null)
            return parent.TryGetSymbol(name, out symbol);

        return false;
    }
}