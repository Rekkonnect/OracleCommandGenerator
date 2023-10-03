using System;

namespace OracleCommandGenerator.SourceGenerators;

internal abstract class ChatbotGeneratedAttribute : Attribute { }
internal sealed class ChatGPTAttribute : ChatbotGeneratedAttribute { }
