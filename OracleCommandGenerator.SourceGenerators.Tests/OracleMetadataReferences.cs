using Microsoft.CodeAnalysis;
using Oracle.ManagedDataAccess.Client;
using RoseLynn;
using System.Collections.Immutable;

namespace OracleCommandGenerator.SourceGenerators.Tests;

public static class OracleMetadataReferences
{
    public static readonly ImmutableArray<MetadataReference> BaseReferences;

    static OracleMetadataReferences()
    {
        BaseReferences = ImmutableArray.Create(new MetadataReference[]
        {
            MetadataReferenceFactory.CreateFromType<OracleCommandParameters>(),
            MetadataReferenceFactory.CreateFromType<OracleDbType>(),
        });
    }
}
