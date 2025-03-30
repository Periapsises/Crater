using Core.SemanticAnalyzer.DataTypes;

namespace Core.SemanticAnalyzer;

public abstract class DataType
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new MetaType();
    
    public static readonly DataType NumberType = new NumberType();
    public static readonly DataType StringType = new StringType();
    public static readonly DataType BooleanType = new BooleanType();

    public virtual void Assign(Symbol self, Symbol assignment)
    {
        if (assignment.DataType != this)
        {
            // TODO: Handle wrong type errors
            throw new NotSupportedException();
        }
        
        self.Value = assignment.Value;
    }
}