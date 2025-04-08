using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer;

public class Environment
{
    public static Environment? Instance { get; private set; }

    private readonly Scope _globalScope = new();
    private Scope? _localScope;

    private readonly Stack<Scope> _previousScopes = [];
    
    private readonly Dictionary<string, Scope> _moduleScopes = [];

    private Environment() { }

    public static void SetupEnvironment()
    {
        Instance = new Environment();
    }

    public static void EnterModuleScope(string moduleName)
    {
        if (Instance is null) throw new NullReferenceException();
        
        if (Instance._moduleScopes.TryGetValue(moduleName, out var scope))
        {
            Instance._localScope = scope;
            return;
        }

        Instance._localScope = new Scope(Instance._globalScope);
        Instance._moduleScopes[moduleName] = Instance._localScope;
    }

    public static void ExitModuleScope()
    {
        if (Instance is null) throw new NullReferenceException();
        
        Instance._localScope = null;
    }

    public static Scope GetLocalScope()
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();

        return Instance._localScope;
    }

    public static Scope GetGlobalScope()
    {
        if (Instance is null) throw new NullReferenceException();

        return Instance._globalScope;
    }

    public static Scope CreateSubScope()
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();

        var scope = new Scope(Instance._localScope);
        return scope;
    }

    public static void EnterScope(Scope scope)
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();
        
        Instance._previousScopes.Push(Instance._localScope);
        Instance._localScope = scope;
    }

    public static void ExitScope()
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();

        if (Instance._previousScopes.TryPop(out var scope))
        {
            Instance._localScope = scope;
            return;
        }
        
        throw new NullReferenceException();
    }

    public static void DeclareLocal(string name, Symbol symbol)
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();

        Instance._localScope.Declare(name, symbol);
    }

    public static void DeclareGlobal(string name, Symbol symbol)
    {
        if (Instance is null) throw new NullReferenceException();
        
        Instance._globalScope.Declare(name, symbol);
    }

    public static Symbol? GetVariable(VariableReference reference)
    {
        if (Instance is null) throw new NullReferenceException();
        if (Instance._localScope is null) throw new NullReferenceException();
        
        return Instance._localScope.Find(reference);
    }
}