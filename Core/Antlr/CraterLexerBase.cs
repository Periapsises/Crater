using Antlr4.Runtime;

namespace Core.Antlr;

public abstract class CraterLexerBase : Lexer
{
    private int _startColumn;
    private int _startLine;

    protected CraterLexerBase(ICharStream input)
        : base(input)
    {
    }

    protected CraterLexerBase(ICharStream input, TextWriter output, TextWriter errorOutput)
        : base(input, output, errorOutput)
    {
    }

    protected void HandleComment()
    {
        _startLine = Line;
        _startColumn = Column - 2;

        var content = (ICharStream)InputStream;
        if (content.LA(1) == '[')
        {
            ReadLongString(content);
            return;
        }

        while (content.LA(1) != '\n' && content.LA(1) != '\r' && content.LA(1) != -1)
            content.Consume();
    }

    private void ReadLongString(ICharStream input)
    {
        var sepCount = GetSepCount(input);
        var exit = false;

        while (true)
        {
            var character = input.LA(1);
            input.Consume();

            switch (character)
            {
                case -1:
                    exit = true;
                    ErrorListenerDispatch.SyntaxError(ErrorOutput, this, 0, _startLine, _startColumn,
                        "unfinished long comment", null);
                    break;
                case ']':
                    if (GetSepCount(input) == sepCount)
                        exit = true;
                    break;
                default:
                    input.Consume();
                    break;
            }

            if (exit) break;
        }
    }

    private int GetSepCount(ICharStream input)
    {
        var count = 0;
        var character = input.LA(1);
        input.Consume();

        while (input.LA(1) == '=')
        {
            input.Consume();
            count++;
        }

        if (input.LA(1) != character)
            ErrorListenerDispatch.SyntaxError(ErrorOutput, this, 0, _startLine, _startColumn,
                "mismatched comment bracket", null);

        input.Consume();

        return count;
    }
}