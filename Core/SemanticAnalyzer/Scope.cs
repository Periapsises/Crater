using Core.SyntaxTreeConverter;
using Core.Utils;

namespace Core.SemanticAnalyzer;

public class Scope(Scope? parent = null)
{
    private readonly Dictionary<string, Symbol> _symbols = [];

    public void Declare(string name, Symbol symbol)
    {
        if (_symbols.ContainsKey(name))
        {
            // TODO: Actual warning for variable re-definition
            DebugMessage.Write("TODO: Actual warning for variable re-definition");
        }
        
        _symbols[name] = symbol;
    }

    private Symbol? Get(string name)
    {
        if (_symbols.TryGetValue(name, out var symbol))
            return symbol;
        
        if (parent != null)
            return parent.Get(name);
        
        return null;
    }

    public Symbol? Find(DataTypeReference searchReference)
    {
        return Get(searchReference.Name);
    }
}