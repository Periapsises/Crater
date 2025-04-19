namespace Core.SemanticAnalyzer;

public class Function(List<TypeUsage> parameterTypes, List<TypeUsage> returnTypes)
{
    public readonly List<TypeUsage> ParameterTypes = parameterTypes;
    public readonly List<TypeUsage> ReturnTypes = returnTypes;
}