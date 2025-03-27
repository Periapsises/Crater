using Core.SemanticAnalyzer;
using FluentAssertions;

namespace Tests;

public class AndOperationsMultiPossibilities
{
    [Fact] // { a1, a2 } and x -> { x }
    public void ContainsOnlyTheRightSymbolIfLeftAreAllTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);

        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which all left symbol are truthy contains only the left symbol");
        possibleSymbols[0].Should().Be(b, "because the left symbols are all truthy");
    }
    
    [Fact] // { true, true } and x -> { x }
    public void ContainsOnlyFalseIfTheLeftSymbolsAreAllTrue()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        
        possibleSymbols.Should()
            .ContainSingle("because an 'and' operation in which all left symbols are true contains only the right symbol");
        possibleSymbols[0].Should().Be(b, "because the left symbols are all true");
    }

    [Fact] // { false, false } and x -> { false, false }
    public void ContainsOnlyFalseIfTheRightSymbolsAreFalse()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        
        possibleSymbols.Should().HaveCount(2, "because we expect both falsy left symbols");
        possibleSymbols.Should().Contain(a1, "because it is false");
        possibleSymbols.Should().Contain(a2, "because it is false");
    }

    [Fact] // { bool, bool } and x -> { false, false, x }
    public void ContainsFalseAndRightSymbolsIfLeftAreAllUnknownBooleans()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(3, "because left and right symbols should be present if the left are all unknown booleans");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because it was an unknown boolean");
        possibleSymbols[0].Value.GetBoolean().Should().BeFalse("because only a falsy value can be passed from the left symbol");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Boolean, "because it was an unknown boolean");
        possibleSymbols[1].Value.GetBoolean().Should().BeFalse("because only a falsy value can be passed from the left symbol");
        possibleSymbols.Should().Contain(b, "because left are all unknown booleans");
    }

    [Fact] // { a1?, a2? } and x -> { nil, nil, x }
    public void ContainsNilAndRightSymbolIfLeftAreAllUnknownNullables()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        
        possibleSymbols.Should().HaveCount(3, "because left and right symbols should be present if the left are all unknown nullables");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Null, "because only a falsy value can be passed from the left symbol");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Null, "because only a falsy value can be passed from the left symbol");
        possibleSymbols.Should().Contain(b, "because left are all unknown nullables");
    }

    [Fact] // { false, true } and x -> { false, x }
    public void ContainsFalseAndRightSymbolIfLeftCanBeTruthyOrFalsy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(2, "because we expect the false left symbol and the right symbol");
        possibleSymbols.Should().Contain(a1, "because it is false");
        possibleSymbols.Should().Contain(b, "because left has a truthy possibility");
    }

    [Fact] // { a1?, a2 } and x -> { nil, x }
    public void ContainsNilAndRightSymbolIfLeftCanBeTruthyOrNull()
    {
        var type = new DataType();

        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(2, "because we expect the nil left symbol and the right symbol");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Null, "because the left side can be null");
        possibleSymbols.Should().Contain(b, "because the left side can be truthy");
    }

    [Fact] // { a1?, bool } and x -> { nil, false, x }
    public void ContainsTheFalsyFromLeftAndRightIfLeftCanAlsoBeTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);

        possibleSymbols.Should()
            .HaveCount(3, "because we expect false and nil from the left side and the right symbol because left and also be truthy");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Null, "because a1 can be null");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Boolean, "because a2 can be false");
        possibleSymbols[1].Value.GetBoolean().Should().BeFalse("because a2 can be false");
        possibleSymbols.Should().Contain(b, "because both left symbols can be truthy");
    }

    [Fact] // bool and { b1, b2 } -> { true, b1, b2 }
    public void ContainsAllSymbolsFromRightOperandIfLeftCanBeTruthy()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b1 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a], [b1, b2]);
        
        possibleSymbols.Should().HaveCount(3, "because the right symbol can be false and we expect all symbols from the right");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because the right side can be true");
        possibleSymbols[0].Value.GetBoolean().Should().BeFalse("because the right side can be false");
        possibleSymbols.Should().Contain(b1, "because the right side can be true");
        possibleSymbols.Should().Contain(b2, "because the right side can be true");
    }

    [Fact] // { bool?, a2 } and b -> { false, nil, b }
    public void ContainsFalseAndNilFromNullableBooleanAndRightSymbolIfLeftCanBeTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        
        possibleSymbols.Should()
            .HaveCount(3, "because false and nil are expected from the nullable boolean and b because the left symbols can be truthy");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because a1 can be false");
        possibleSymbols[0].Value.GetBoolean().Should().BeFalse("because a1 can be false");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Null, "because a1 can be nil");
        possibleSymbols.Should().Contain(b, "because the left symbols can be truthy");
    }
}

public class OrOperationsMultiPossibilities
{
    [Fact] // { a1, a2 } or b -> { a1, a2 }
    public void ContainsOnlySymbolsFromLeftIfTheyAreAllTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(2, "because we expect the two possibilities from the left operand");
        possibleSymbols.Should().Contain(a1, "because it is always truthy");
        possibleSymbols.Should().Contain(a2, "because it is always truthy");
    }

    [Fact] // { true, true } or b -> { true, true }
    public void ContainsOnlySymbolsFromLeftIfTheyAreAllTrue()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(2, "because we expect the two possibilities from the left operand");
        possibleSymbols.Should().Contain(a1, "because it is always truthy");
        possibleSymbols.Should().Contain(a2, "because it is always truthy");
    }
    
    [Fact] // { false, false } or b -> { b }
    public void ContainsOnlySymbolsFromRightIfAllLeftSymbolsAreFalse()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().ContainSingle("because only the right symbol is expected if the left are all false");
        possibleSymbols.Should().Contain(b, "because all left symbols are false");
    }

    [Fact] // { bool, bool } or b -> { true, true, b }
    public void ContainsTrueAndBIfAllLeftSymbolsAreUnknownBooleans()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(3, "because we expect two true from the left symbols and b because they can be truthy");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because it is can be true");
        possibleSymbols[0].Value.GetBoolean().Should().BeTrue("because it can be true");
        possibleSymbols[1].Value.Kind.Should().Be(ValueKind.Boolean, "because it is can be true");
        possibleSymbols[1].Value.GetBoolean().Should().BeTrue("because it can be true");
        possibleSymbols.Should().Contain(b, "because the left symbols can be falsy");
    }

    [Fact] // { a1?, a2? } or b -> { a1!, a2!, b }
    public void ContainsNonNullableVersionsOfLeftSymbolsAndTheRightSymbol()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var b = new  Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);
        
        possibleSymbols.Should().HaveCount(3, "because we expect the two possibilities from the left operand as well as the right symbol");
        possibleSymbols[0].Nullable.Should().BeFalse("because an 'or' operation filters null values");
        possibleSymbols[1].Nullable.Should().BeFalse("because an 'or' operation filters null values");
        possibleSymbols.Should().Contain(b, "because the left symbols can be falsy");        
    }

    [Fact] // { false, true } or b -> { true, b }
    public void ContainsLeftTrueSymbolsAndRightSymbolsIfLeftCanBeFalsy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(Value.FalseValue, DataType.BooleanType, false);
        var a2 = new Symbol(Value.TrueValue, DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(2, "because we expect the true from the left symbol and the right symbol");
        possibleSymbols.Should().Contain(a2, "because it is true");
        possibleSymbols.Should().Contain(b, "because the left symbols can be falsy");
    }

    [Fact] // { a1?, a2 } or b -> { a1!, a2, b }
    public void ContainsLeftTruthySymbolsAndRightSymbolsIfLeftCanBeFalsy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);

        possibleSymbols.Should().HaveCount(3, "because we expect a non null a1 and a2 from the left symbols and the right symbol");
        possibleSymbols[0].Nullable.Should().BeFalse("because an 'or' operation filters null values");
        possibleSymbols.Should().Contain(a2, "because it is truthy");
        possibleSymbols.Should().Contain(b, "because the left symbols can be falsy");
    }

    [Fact] // bool or { b1, b2 } -> { true, b1, b2 }
    public void ContainsAllRightSymbolsIfLeftSymbolCanBeFalsy()
    {
        var type = new DataType();
        
        var a = new Symbol(new Value(ValueKind.Unknown, 0), DataType.BooleanType, false);
        var b1 = new Symbol(new Value(ValueKind.Unknown, 0), type, false);
        var b2 = new Symbol(new Value(ValueKind.Unknown, 0), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a], [b1, b2]);

        possibleSymbols.Should()
            .HaveCount(3, "because we expect true from the left symbol and both symbols from the right operand");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because a can be true");
        possibleSymbols[0].Value.GetBoolean().Should().BeTrue("because a can be true");
        possibleSymbols.Should().Contain(b1, "because the left symbols can be falsy");
        possibleSymbols.Should().Contain(b2, "because the left symbols can be falsy");
    }
    
    [Fact] // { bool?, a2 } or b -> { true, a2, b }
    public void ContainsFalseAndNilFromNullableBooleanAndRightSymbolIfLeftCanBeTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, true);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbols = semanticAnalyzer.AnalyzeOrOperation([a1, a2], [b]);
        
        possibleSymbols.Should()
            .HaveCount(3, "because true is expected from the nullable boolean as well as a2 and b because the left symbols can be truthy");
        possibleSymbols[0].Value.Kind.Should().Be(ValueKind.Boolean, "because a1 can be false");
        possibleSymbols[0].Value.GetBoolean().Should().BeTrue("because a1 can be true");
        possibleSymbols.Should().Contain(a2, "because it is truthy");
        possibleSymbols.Should().Contain(b, "because the left symbols can be truthy");
    }
}

public class AndOrOperationsMultiPossibilities
{
    [Fact] // { a1, a2 } and b or c -> { b }
    public void ContainsOnlyBIfItAndLeftMostSymbolsAreAllTruthy()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);

        possibleSymbolsFromOr.Should().ContainSingle("because a1, a2 and b are always truthy");
        possibleSymbolsFromOr.Should().Contain(b, "because it is always truthy");
    }

    [Fact] // a and { b1, b2 } or c -> { b1, b2 }
    public void ContainsAllBSymbolsIfLeftMostAndTheyAreAllTruthy()
    {
        var type = new DataType();
        
        var a =  new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b1 =  new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var b2 = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c =  new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a], [b1, b2]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);

        possibleSymbolsFromOr.Should().HaveCount(2, "because we expect both b values");
        possibleSymbolsFromOr.Should().Contain(b1, "because it is always truthy");
        possibleSymbolsFromOr.Should().Contain(b2, "because it is always truthy");
    }

    [Fact] // { bool, bool } and b or c -> { b, c }
    public void ContainsOnlyBAndCIfAllConditionsAreUnknownBooleans()
    {
        var type = new DataType();
        
        var a1 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var a2 = new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false);
        var b = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        var c = new Symbol(new Value(ValueKind.Unknown, null), type, false);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        var possibleSymbolsFromAnd = semanticAnalyzer.AnalyzeAndOperation([a1, a2], [b]);
        var possibleSymbolsFromOr = semanticAnalyzer.AnalyzeOrOperation(possibleSymbolsFromAnd, [c]);
        
        possibleSymbolsFromOr.Should().HaveCount(2, "because we expect both b and c symbols");
        possibleSymbolsFromOr.Should().Contain(b, "because it is always truthy");
        possibleSymbolsFromOr.Should().Contain(c, "because it is always truthy");
    }
}