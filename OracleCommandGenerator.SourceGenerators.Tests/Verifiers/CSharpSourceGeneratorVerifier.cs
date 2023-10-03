using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using RoseLynn.Generators.Testing;
using System.Collections.Generic;

namespace OracleCommandGenerator.SourceGenerators.Tests.Verifiers;

public static class CSharpSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : ISourceGenerator, new()
{
    public class Test : CSharpSourceGeneratorTestEx<TSourceGenerator, NUnitVerifier>
    {
        public override IEnumerable<MetadataReference> AdditionalReferences => OracleMetadataReferences.BaseReferences;

        public Test()
        {
            ReferenceAssemblies = ReferenceAssemblies.Net.Net50;
        }
    }
}
