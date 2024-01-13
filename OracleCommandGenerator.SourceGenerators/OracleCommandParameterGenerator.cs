using Dentextist;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OracleCommandGenerator.Core;
using RoseLynn;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable enable annotations

namespace OracleCommandGenerator.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public sealed class OracleCommandParameterGenerator : IIncrementalGenerator
{
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: FilterOracleCommandParameterTypeDeclaration,
                transform: ParseOracleCommandParameterType)
            .Where(static s => s is not null);
        context.RegisterImplementationSourceOutput(provider, GenerateCommand);

        static bool FilterOracleCommandParameterTypeDeclaration(
            SyntaxNode node,
            CancellationToken cancellationToken)
        {
            return node
                is RecordDeclarationSyntax
                or StructDeclarationSyntax
                or ClassDeclarationSyntax;
        }

        static OracleCommandParameterType? ParseOracleCommandParameterType(
            GeneratorSyntaxContext context,
            CancellationToken cancellationToken)
        {
            var declaredSymbol = context.SemanticModel.GetDeclaredSymbol(context.Node, cancellationToken) as INamedTypeSymbol;
            return OracleCommandParameterType.ParseDeclaredSymbol(declaredSymbol);
        }
    }

    private static string DetermineDeclarationSyntax(OracleCommandParameterType commandType)
    {
        switch (commandType.TypeSymbol.TypeKind)
        {
            case TypeKind.Struct:
                if (commandType.TypeSymbol.IsRecord)
                    return TypeDeclarationKinds.RecordStruct;

                return TypeDeclarationKinds.Struct;

            case TypeKind.Class:
                if (commandType.TypeSymbol.IsRecord)
                    return TypeDeclarationKinds.Record; // Backcompat with C# 9.0

                return TypeDeclarationKinds.Class;

            default:
                return null;
        }
    }

    private readonly record struct GenerationState(
        OracleCommandParameterType Type,
        CSharpCodeBuilder CodeBuilder);

    private void GenerateCommand(
        SourceProductionContext context,
        OracleCommandParameterType commandType)
    {
        var fullSymbolName = commandType.TypeSymbol.GetFullSymbolName();

        var codeBuilder = new CSharpCodeBuilder(' ', 4);
        var generationState = new GenerationState(commandType, codeBuilder);

        if (commandType.TypeSymbol.ContainingNamespace.IsGlobalNamespace)
        {
            GeneratePartial();
        }
        else
        {
            codeBuilder.AppendLine($"namespace {fullSymbolName.FullNamespaceString}");
            using (var _ = codeBuilder.EnterBracketBlock())
            {
                GeneratePartial();
            }
        }

        var source = codeBuilder.ToString();

        var usingsSource = OracleCommandUsingsProvider.Instance.WithUsings(source);
        context.AddSource($"{fullSymbolName.FullNameString}.Boilerplate.g.cs", usingsSource);

        void GeneratePartial()
        {
            GeneratePartialDeclaration(commandType, codeBuilder, generationState);
        }
    }

    private static void GeneratePartialDeclaration(OracleCommandParameterType commandType, CSharpCodeBuilder codeBuilder, GenerationState generationState)
    {
        string declarationKind = DetermineDeclarationSyntax(commandType);
        codeBuilder.AppendLine($"partial {declarationKind} {commandType.TypeSymbol.Name}");
        using (var _ = codeBuilder.EnterBracketBlock())
        {
            GenerateSetParametersMethod(generationState);
            codeBuilder.AppendLine();
            GenerateSetInputParametersMethod(generationState);
            codeBuilder.AppendLine();
            GenerateReadOutputMethod(generationState);
        }
    }

    private static void GenerateSetParametersMethod(GenerationState generationState)
    {
        var (commandType, codeBuilder) = generationState;

        var header = GenerateSignatureHeader("void", commandType.BaseTypeKind);
        codeBuilder.AppendLine($"{header}SetParameters(OracleCommand command)");
        using (var _ = codeBuilder.EnterBracketBlock())
        {
            var parameterStatementLines = commandType.OracleParameters
                .Select(GenerateStatement)
                .ToArray();

            foreach (var statement in parameterStatementLines)
            {
                codeBuilder.AppendLine(statement);
            }
        }

        static string GenerateStatement(OracleCommandParameter parameter)
        {
            string sizeArgumentSuffix = "";
            if (parameter.Size is not null)
            {
                sizeArgumentSuffix = $", {parameter.Size}";
            }

            return $"command.Parameters.Add(nameof({parameter.Name}), {parameter.ParameterTypeExpression}{sizeArgumentSuffix}, ParameterDirection.{parameter.ParameterDirection}, null);";
        }
    }

    private static void GenerateSetInputParametersMethod(GenerationState generationState)
    {
        var (commandType, codeBuilder) = generationState;

        var header = GenerateSignatureHeader("void", commandType.BaseTypeKind);
        codeBuilder.AppendLine($"{header}SetInputParameters(OracleCommand command)");
        using (var _ = codeBuilder.EnterBracketBlock())
        {
            var parameterStatementLines = commandType.OracleParameters.Outgoing
                .Select(GenerateStatement)
                .ToArray();

            foreach (var statement in parameterStatementLines)
            {
                codeBuilder.AppendLine(statement);
            }
        }

        static string GenerateStatement(OracleCommandParameter parameter)
        {
            return $"command.Parameters[nameof({parameter.Name})].Value = {parameter.Name};";
        }
    }

    private static void GenerateReadOutputMethod(GenerationState generationState)
    {
        var (commandType, codeBuilder) = generationState;

        var header = GenerateSignatureHeader("void", commandType.BaseTypeKind);
        codeBuilder.AppendLine($"{header}ReadOutput(OracleCommand command)");
        using (var _ = codeBuilder.EnterBracketBlock())
        {
            var parameterStatementLines = commandType.OracleParameters.Incoming
                .Select(GenerateStatement)
                .ToArray();

            foreach (var statement in parameterStatementLines)
            {
                codeBuilder.AppendLine(statement);
            }
        }

        static string GenerateStatement(OracleCommandParameter parameter)
        {
            var displayType = parameter.Type
                .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            return $"{parameter.Name} = ({displayType})command.Parameters[nameof({parameter.Name})].Value;";
        }
    }

    private static string GenerateSignatureHeader(
        string returnType,
        OracleCommandParameterBaseType baseTypeKind)
    {
        return baseTypeKind switch
        {
            OracleCommandParameterBaseType.Interface
                => $"{returnType} IOracleCommandParameters.",

            OracleCommandParameterBaseType.Class or
            OracleCommandParameterBaseType.Record
                => $"protected override {returnType} ",

            _ => null,
        };
    }

    private sealed record OracleCommandParameterType(
        INamedTypeSymbol TypeSymbol,
        OracleCommandParameterBaseType BaseTypeKind,
        OracleCommandParameterCollection OracleParameters)
    {
        public static OracleCommandParameterType? ParseDeclaredSymbol(INamedTypeSymbol? declaredSymbol)
        {
            if (declaredSymbol is null)
                return null;

            bool isOracleCommandParameterType = declaredSymbol.AllInterfaces
                .Any(x => x.Name is WellKnownNames.IOracleCommandParameters);
            if (!isOracleCommandParameterType)
                return null;

            var baseTypeKind = OracleCommandParameterBaseType.Interface;

            bool inheritsClass = declaredSymbol.GetAllBaseTypes()
                .Any(x => x.Name is WellKnownNames.OracleCommandParameters);
            if (inheritsClass)
            {
                baseTypeKind = OracleCommandParameterBaseType.Class;
            }

            // typing
            // x.Name is WellKnownNames.
            // does not show shit about suggestions while accessing the name of the type

            var parameters =
                from member in declaredSymbol.GetAllMembersIncludingInherited()
                where member.IsFieldOrProperty()
                from attribute in member.GetAttributes()
                where OracleCommandParameter.IsOracleParameterRelatedAttribute(attribute)
                select OracleCommandParameter.FromAttribute(member, attribute);

            return new(declaredSymbol, baseTypeKind, new(parameters));
        }
    }

    private enum OracleCommandParameterBaseType
    {
        Interface,
        Class,

        // Future?
        Record,
    }

    private sealed class OracleCommandParameterCollection : List<OracleCommandParameter>
    {
        public IEnumerable<OracleCommandParameter> Outgoing => this.Where(
            p => p.ParameterDirection
                is ParameterDirection.Input
                or ParameterDirection.InputOutput);

        public IEnumerable<OracleCommandParameter> Incoming => this.Where(
            p => p.ParameterDirection
                is ParameterDirection.Output
                or ParameterDirection.InputOutput
                or ParameterDirection.ReturnValue);

        public OracleCommandParameterCollection() { }

        public OracleCommandParameterCollection(IEnumerable<OracleCommandParameter> parameters)
            : base(parameters) { }
    }

    private sealed record OracleCommandParameter(
        ISymbol ParameterMember,
        ParameterDirection ParameterDirection,
        ExpressionSyntax ParameterTypeExpression,
        int? Size)
    {
        public string Name => ParameterMember.Name;
        public ITypeSymbol Type => ParameterMember.GetSymbolType();

        public static OracleCommandParameter FromAttribute(ISymbol symbol, AttributeData attribute)
        {
            var attributeSyntax = attribute.ApplicationSyntaxReference.GetSyntax() as AttributeSyntax;
            var attributeArguments = attributeSyntax.ArgumentList;

            var direction = DirectionFromAttribute(attribute);
            var parameterTypeExpression = attributeArguments.Arguments[0].Expression;
            var sizeArgument = GetOptionalAttributeArgument(1, attributeArguments);

            var sizeExpression = sizeArgument?.Expression as LiteralExpressionSyntax;
            var sizeString = sizeExpression?.Token.ValueText;
            sizeString.TryParseNullableInt32(out var size);

            return new(symbol, direction, parameterTypeExpression, size);
        }

        private static AttributeArgumentSyntax GetOptionalAttributeArgument(int index, AttributeArgumentListSyntax attributeArgumentList)
        {
            if (attributeArgumentList.Arguments.Count > index)
            {
                return attributeArgumentList.Arguments[index];
            }

            return null;
        }

        public static bool IsOracleParameterRelatedAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass.GetAllBaseTypes().Any(Matches);

            static bool Matches(INamedTypeSymbol baseType)
            {
                return baseType.Name is WellKnownNames.OracleParameterAttributeBase;
            }
        }

        private static ParameterDirection DirectionFromAttribute(AttributeData attribute)
        {
            return attribute.AttributeClass.Name switch
            {
                WellKnownNames.OracleInputParameterAttribute => ParameterDirection.Input,
                WellKnownNames.OracleOutputParameterAttribute => ParameterDirection.Output,
                WellKnownNames.OracleInputOutputParameterAttribute => ParameterDirection.InputOutput,
                WellKnownNames.OracleReturnValueParameterAttribute => ParameterDirection.ReturnValue,
                _ => ParameterDirection.Unknown,
            };
        }
    }

    // Copy-pasted due to lack of dependency
    private enum ParameterDirection
    {
        Unknown = 0,

        Input = 1,
        Output = 2,
        InputOutput = 3,
        ReturnValue = 6,
    }

    private sealed class OracleCommandUsingsProvider : UsingsProviderBase
    {
        public static OracleCommandUsingsProvider Instance { get; } = new();

        private OracleCommandUsingsProvider() { }

        public override string DefaultNecessaryUsings => """
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            """;
    }
}
