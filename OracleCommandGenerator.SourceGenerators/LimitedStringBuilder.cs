using System;

namespace OracleCommandGenerator.SourceGenerators;

public class LimitedStringBuilder
{
    [ChatGPT]
    private char[] _buffer;
    [ChatGPT]
    private int _length;
    [ChatGPT]
    private int _capacity;

    [ChatGPT]
    public int Length => _length;
    [ChatGPT]
    public int Capacity => _capacity;

    // Environment.NewLine is banned
    public string NewLine { get; set; } = "\r\n";
    public double GrowFactor { get; set; } = 2;

    public LimitedStringBuilder()
        : this(16) { }

    [ChatGPT]
    public LimitedStringBuilder(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Initial capacity must be greater than zero.", nameof(initialCapacity));

        _buffer = new char[initialCapacity];
        _capacity = initialCapacity;
        _length = 0;
    }

    [ChatGPT]
    public LimitedStringBuilder Append(string text)
    {
        EnsureCapacity(text.Length);
        text.CopyTo(0, _buffer, _length, text.Length);
        _length += text.Length;
        return this;
    }

    [ChatGPT]
    public LimitedStringBuilder Append(char c)
    {
        EnsureCapacity(1);
        _buffer[_length] = c;
        _length++;
        return this;
    }

    public LimitedStringBuilder Append(char c, int count)
    {
        if (count <= 0)
            return this;

        EnsureCapacity(count);
        for (int i = 0; i < count; i++)
        {
            _buffer[_length + i] = c;
        }
        _length += count;
        return this;
    }

    [ChatGPT]
    public LimitedStringBuilder Append(Span<char> span)
    {
        EnsureCapacity(span.Length);
        span.CopyTo(new Span<char>(_buffer, _length, span.Length));
        _length += span.Length;
        return this;
    }

    [ChatGPT]
    public LimitedStringBuilder Append(ReadOnlySpan<char> span)
    {
        EnsureCapacity(span.Length);
        span.CopyTo(new Span<char>(_buffer, _length, span.Length));
        _length += span.Length;
        return this;
    }

    public LimitedStringBuilder AppendLine(string text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(char text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(Span<char> text)
    {
        return Append(text).AppendLine();
    }
    public LimitedStringBuilder AppendLine(ReadOnlySpan<char> text)
    {
        return Append(text).AppendLine();
    }

    public LimitedStringBuilder AppendLine()
    {
        return Append(NewLine);
    }

    [ChatGPT]
    public override string ToString()
    {
        return new string(_buffer, 0, _length);
    }

    [ChatGPT]
    private void EnsureCapacity(int additionalLength)
    {
        if (_length + additionalLength <= _capacity)
            return;

        int grownCapacity = (int)(_capacity * GrowFactor);
        int newCapacity = Math.Max(_length + additionalLength, grownCapacity);
        char[] newBuffer = new char[newCapacity];
        _buffer.CopyTo(newBuffer, 0);
        _buffer = newBuffer;
        _capacity = newCapacity;
    }
}
