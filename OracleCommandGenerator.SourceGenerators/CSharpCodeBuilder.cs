using System;

namespace OracleCommandGenerator.SourceGenerators;

public sealed class CSharpCodeBuilder : IndentedStringBuilder
{
    public CSharpCodeBuilder(char indentationCharacter, int indentationSize)
        : base(indentationCharacter, indentationSize)
    {
    }

    public BracketBlock EnterBracketBlock(char open = '{', char close = '}')
    {
        return new(this, open, close);
    }

    public readonly struct BracketBlock
        : IDisposable
    {
        private readonly CSharpCodeBuilder _builder;

        private readonly char _close;

        [Obsolete(message: "Do not use the parameterless constructor", error: true)]
        public BracketBlock() { }

        public BracketBlock(CSharpCodeBuilder builder, char open, char close)
        {
            _builder = builder;
            _close = close;
            _builder.AppendLine(open);
            _builder.NestingLevel++;
        }

        public void Dispose()
        {
            _builder.NestingLevel--;
            _builder.AppendLine(_close);
        }
    }
}
