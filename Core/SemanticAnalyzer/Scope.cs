using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer;

public class Scope(Scope? parent = null)
{
    private readonly Dictionary<string, Symbol> _symbols = [];

    public void Declare(string name, Symbol symbol)
    {
        _symbols[name] = symbol;
    }

    public bool HasVariable(string name)
    {
        return _symbols.ContainsKey(name);
    }
    
    private Symbol? Get(string name)
    {
        if (_symbols.TryGetValue(name, out var symbol))
            return symbol;
        
        if (parent != null)
            return parent.Get(name);
        
        return null;
    }

    public Symbol? Find(VariableReference searchReference)
    {
        return Get(searchReference.Name);
    }
}