using Antlr4.Runtime;

namespace Core.SemanticAnalyzer;

public static class Context
{
    public static ParserRuleContext? CurrentContext { get; private set; }
    private static readonly Stack<ParserRuleContext?> _contextStack = [];

    public static void PushContext(ParserRuleContext context)
    {
        _contextStack.Push(context);
        CurrentContext = context;
    }

    public static void PopContext()
    {
        if (_contextStack.Count == 0)
            throw new InvalidOperationException("Context stack is empty");
        
        CurrentContext = _contextStack.Pop();
    }

    public static T Get<T>() where T : ParserRuleContext
    {
        if (CurrentContext == null)
            throw new InvalidOperationException("Current context is null");
        
        if (CurrentContext is T currentContext)
            return currentContext;
        
        throw new InvalidCastException($"Current context is of type {CurrentContext.GetType().Name}, expexted {typeof(T).Name}");
    }
}

public sealed class ScopedContext : IDisposable
{
    public ScopedContext(ParserRuleContext context) => Context.PushContext(context);
    public void Dispose() => Context.PopContext();
}
