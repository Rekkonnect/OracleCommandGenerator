using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using OracleCommandGenerator.SourceGenerators.Tests.Verifiers;
using RoseLynn.Testing;
using System.Collections.Generic;

namespace OracleCommandGenerator.SourceGenerators.Tests;

public abstract class BaseIncrementalGeneratorTestContainer<TSourceGenerator>
    : BaseIncrementalGeneratorTestContainer<TSourceGenerator, NUnitVerifier, CSharpIncrementalGeneratorVerifier<TSourceGenerator>.Test>

    where TSourceGenerator : IIncrementalGenerator, new()
{
    protected override IEnumerable<MetadataReference> DefaultMetadataReferences => OracleMetadataReferences.BaseReferences;

    protected override LanguageVersion LanguageVersion => LanguageVersion.CSharp11;
}
