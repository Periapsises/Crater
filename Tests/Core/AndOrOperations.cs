using Core.SemanticAnalyzer;
using FluentAssertions;

namespace Tests.Core;

public class AndOperations
{
    [Fact] // a and x -> { x }
    public void ContainsOnlyTheRightSymbolIfLeftIsTruthy()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);

        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which the left symbol is truthy contains only the left symbol");
        possibleSymbols[0].Should().Be(b, "because the left symbol is truthy");
    }

    [Fact] // true and x -> { x }
    public void ContainsOnlyFalseIfTheLeftSymbolIsTrue()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which the left symbol is true contains only the right symbol");
        possibleSymbols[0].Should().Be(b, "because a is true");
    }
    
    [Fact] // false and x -> { false }
    public void ContainsOnlyFalseIfTheLeftSymbolIsFalse()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which the left symbol is falsy contains only that symbol");
        possibleSymbols[0].Should().Be(a, "because it is falsy");
    }
    
    [Fact] // nil and x -> { nil, x }
    public void ContainsOnlyNilIfTheLeftSymbolIsNull()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.NullValue, type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which the left symbol is null contains only that symbol");
        possibleSymbols[0].Should().Be(a, "because it is null");
    }

    [Fact] // bool and x -> { false, x }
    public void ContainsFalseAndRightSymbolIfLeftIsUnknownBoolean()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);

        possibleSymbols.Should().HaveCount(2, "because left and right symbols should be present if the left is an unknown boolean");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because left was an unknown boolean");
        possibleSymbols[0].Value.GetBoolean().Should().BeFalse("because only a falsy value can be passed from the left symbol");
        possibleSymbols.Should().Contain(b, "because 'a' is an unknown boolean");
    }

    [Fact] // a? and x -> { nil, x }
    public void ContainsNilAndRightSymbolIfLeftIsUnknownNullable()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        
        possibleSymbols.Should().HaveCount(2, "because left and right symbols should be present if the left is an unknown nullable");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Null, "because only a falsy value can be passed from the left symbol");
        possibleSymbols.Should().Contain(b, "because 'a' is an unknown nullable");
    }

    [Fact] // bool? and x -> { false, nil, x }
    public void ContainsBothFalseAndNilIfLeftIsANullableUnknownBoolean()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);

        possibleSymbols.Should()
            .HaveCount(3, "because we expect both false and nil from the left side, as well as the right side");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because we expect false from an unknown boolean");
        possibleSymbols[0].Value.GetBoolean().Should().BeFalse("because we expect false from an unknown boolean");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Null, "because we expect nil from a nullable");
        possibleSymbols.Should().Contain(b, "because 'a' can be truthy");
    }

    [Fact] // nil? and x -> { nil }
    public void ContainsNilIfLeftIsANullableAndAKnownNull()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.NullValue, type, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b]);

        possibleSymbols.Should().ContainSingle("because we expect only nil from a known nullable");
        possibleSymbols.Should().Contain(a, "because it is a known null");
    }
}

public class OrOperations
{
    [Fact] // a or x -> { a }
    public void ContainsOnlyTheLeftSymbolIfItIsTruthy()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b]);

        possibleSymbols.Should()
            .ContainSingle("because an 'or' operation in which the left symbol is truthy contains only that symbol");
        possibleSymbols[0].Should().Be(a, "because it is truthy");
    }

    [Fact] // false or x -> { x }
    public void ContainsOnlyTheRightSymbolIfTheLeftSymbolIsFalsy()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'or' operation in which the left symbol is false contains only the right symbol");
        possibleSymbols[0].Should().Be(b, "because a is false");
    }
    
    [Fact] // nil or x -> { x }
    public void ContainsOnlyTheRightSymbolIfTheLeftSymbolIsNull()
    {
        var type = new DataType();
        
        var a = new Symbol(Value.NullValue, type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'or' operation in which the left symbol is nil contains only the right symbol");
        possibleSymbols[0].Should().Be(b, "because a is nil");
    }
    
    [Fact] // a? or x -> { a!, x }
    public void ContainsBothSymbolsIfTheLeftSymbolIsNullable()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b]);
        
        possibleSymbols.Should().HaveCount(2, "because left and right symbols should be present if the left is an unknown nullable");
        possibleSymbols[0].Nullable.Should().BeFalse("because null will not be passed by an 'or' operation");
        possibleSymbols.Should().Contain(b, "because a is nullable");
    }

    [Fact] // bool? or x -> { true, x }
    public void ContainsTrueAndTheRightSymbolIfTheRightIsANullableBoolean()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b]);

        possibleSymbols.Should()
            .HaveCount(2, "because we expect true from the left symbol, as well as the right symbol");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because we expect true from the right symbol");
        possibleSymbols[0].Value.GetBoolean().Should().BeTrue("because we expect true from the right symbol");
        possibleSymbols.Should().Contain(b, "because the left symbol can be false");
    }
}

// And or operations need special attention because of Lua's ternary operation "_ and _ or _".
public class AndOrOperations
{
    [Fact] // a and b or c -> { b }
    public void ContainsOnlyTheLeftMostSymbolIfItIsTruthy()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);

        possibleSymbolsFromOr.Should()
            .ContainSingle("because an 'or' operation in which the left symbol is truthy contains only that symbol");
        possibleSymbolsFromOr[0].Should().Be(b, "because the left symbol is truthy");
    }

    [Fact] // bool and b or c -> { b, c }
    public void FiltersTheASymbolIfItIsAnUnknownBoolean()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);

        possibleSymbolsFromOr.Should().HaveCount(2, "because only b and c are expected in the result");
        possibleSymbolsFromOr.Should().NotContain(a, "because an unknown bool should be filtered out in an 'and or' operation");
        possibleSymbolsFromOr.Should().Contain(b, "because b is a possibility in the 'and' operation");
        possibleSymbolsFromOr.Should().Contain(c, "because c is a possibility in the 'or' operation");
    }
    
    [Fact] // a? and b or c -> { b, c }
    public void FiltersTheASymbolIfItIsNullable()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);

        possibleSymbolsFromOr.Should().HaveCount(2, "because only b and c are expected in the result");
        possibleSymbolsFromOr.Should().NotContain(a, "because an unknown bool should be filtered out in an 'and or' operation");
        possibleSymbolsFromOr.Should().Contain(b, "because b is a possibility in the 'and' operation");
        possibleSymbolsFromOr.Should().Contain(c, "because c is a possibility in the 'or' operation");
    }

    [Fact] // bool? and b or c -> { b, c }
    public void FiltersTheASymbolIfItIsANullableBoolean()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);
        
        possibleSymbolsFromOr.Should().HaveCount(2, "because only b and c are expected in the result");
        possibleSymbolsFromOr.Should().NotContain(a, "because an unknown bool should be filtered out in an 'and or' operation");
        possibleSymbolsFromOr.Should().Contain(b, "because b is a possibility in the 'and' operation");
        possibleSymbolsFromOr.Should().Contain(c, "because c is a possibility in the 'or' operation");
    }
}