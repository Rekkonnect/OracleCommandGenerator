using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using OracleCommandGenerator.SourceGenerators.Tests.Verifiers;
using RoseLynn.Testing;
using System.Collections.Generic;

namespace OracleCommandGenerator.SourceGenerators.Tests;

public abstract class BaseSourceGeneratorTestContainer<TSourceGenerator>
    : BaseSourceGeneratorTestContainer<TSourceGenerator, NUnitVerifier, CSharpSourceGeneratorVerifier<TSourceGenerator>.Test>

    where TSourceGenerator : ISourceGenerator, new()
{
    protected override IEnumerable<MetadataReference> DefaultMetadataReferences => OracleMetadataReferences.BaseReferences;

    protected override LanguageVersion LanguageVersion => LanguageVersion.CSharp9;
}
