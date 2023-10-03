using System;

namespace OracleCommandGenerator.SourceGenerators;

public ref struct SpanLineEnumerator
{
    private readonly ReadOnlySpan<char> _string;
    private int _start;
    private int _index;

    public ReadOnlySpan<char> Current { get; private set; }

    public SpanLineEnumerator(ReadOnlySpan<char> @string)
    {
        _string = @string;
    }

    public void Reset()
    {
        Current = default;
        _start = 0;
        _index = 0;
    }

    public bool MoveNext()
    {
        int length = _string.Length;
        for (; _index < length; _index++)
        {
            char c = _string[_index];
            if (c is '\r' or '\n')
            {
                Current = _string.Slice(_start, _index - _start);

                if (c is '\r' && _index + 1 < length && _string[_index + 1] == '\n')
                {
                    _index++; // Handle Windows line endings (CRLF)
                }
                _start = _index + 1;

                return true;
            }
        }

        if (_start < length)
        {
            Current = _string.Slice(_start, length - _start);

            // Do not enter this block again
            _start = length;
            return true;
        }

        return false;
    }

    public readonly SpanLineEnumerator GetEnumerator() => this;
}
