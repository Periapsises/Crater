using Core.SemanticAnalyzer.DataTypes;

namespace Core.SemanticAnalyzer;

public abstract class DataType
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new MetaType();
    
    public static readonly DataType NumberType = new NumberType();
    public static readonly DataType StringType = new StringType();
    public static readonly DataType BooleanType = new BooleanType();

    public abstract string GetName();
    
    public virtual void Assign(Symbol self, Symbol assignment)
    {
        if (assignment.DataType != this)
        {
            DiagnosticsReporter.CurrentDiagnostics?.PushError($"Cannot convert from type '{assignment.DataType.GetName()}' to type '{self.DataType.GetName()}'");
            return;
        }
        
        self.Value = assignment.Value;
    }
}