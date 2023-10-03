using NUnit.Framework;
using RoseLynn.Generators;
using System.Threading.Tasks;

namespace OracleCommandGenerator.SourceGenerators.Tests;

public sealed class OracleCommandParameterGeneratorTests
    : BaseIncrementalGeneratorTestContainer<OracleCommandParameterGenerator>
{
    /*
     * Uncovered cases:
     * - Inheritance
     */

    [Theory]
    [TestCase(TypeDeclarationKinds.Class, TypeDeclarationKinds.Class)]
    public async Task RealCodeTest_ClassBase(
        string sourceTypeKind,
        string generatedTypeKind)
    {
        const string @namespace = "OracleCommandGenerator.SourceGenerators.Tests.Input";
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        var source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            namespace {{@namespace}};

            public sealed partial {{sourceTypeKind}} {{@class}}
                : OracleCommandParameters
            {
                public override string CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public OracleDecimal AdjustmentPrice { get; private set; }
            
                [OracleInputOutputParameter(OracleDbType.Varchar2, 100)]
                public OracleString RecordId { get; private set; }
            
                [OracleReturnValueParameter(OracleDbType.Int32)]
                public OracleDecimal Available { get; private set; }
            }
            """;

        var expected = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            namespace {{@namespace}}
            {
                partial {{generatedTypeKind}} {{@class}}
                {
                    protected override void SetParameters(OracleCommand command)
                    {
                        command.Parameters.Add(nameof(CustomerSsn), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(NewPricingPlan), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(AdjustmentPrice), OracleDbType.Decimal, ParameterDirection.Output, null);
                        command.Parameters.Add(nameof(RecordId), OracleDbType.Varchar2, 100, ParameterDirection.InputOutput, null);
                        command.Parameters.Add(nameof(Available), OracleDbType.Int32, ParameterDirection.ReturnValue, null);
                    }

                    protected override void SetInputParameters(OracleCommand command)
                    {
                        command.Parameters[nameof(CustomerSsn)].Value = CustomerSsn;
                        command.Parameters[nameof(NewPricingPlan)].Value = NewPricingPlan;
                        command.Parameters[nameof(RecordId)].Value = RecordId;
                    }

                    protected override void ReadOutput(OracleCommand command)
                    {
                        AdjustmentPrice = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(AdjustmentPrice)].Value;
                        RecordId = (global::Oracle.ManagedDataAccess.Types.OracleString)command.Parameters[nameof(RecordId)].Value;
                        Available = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(Available)].Value;
                    }
                }
            }

            """;

        var mappings = new GeneratedSourceMappings
        {
            { $"{@namespace}.{@class}.Boilerplate.g.cs", expected }
        };

        await VerifyAsync(source, mappings);
    }

    [Theory]
    [TestCase(TypeDeclarationKinds.Class, TypeDeclarationKinds.Class)]
    [TestCase(TypeDeclarationKinds.Record, TypeDeclarationKinds.Record)]
    [TestCase(TypeDeclarationKinds.RecordClass, TypeDeclarationKinds.Record)]
    public async Task RealCodeTest_InterfaceBase(
        string sourceTypeKind,
        string generatedTypeKind)
    {
        const string @namespace = "OracleCommandGenerator.SourceGenerators.Tests.Input";
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        var source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            namespace {{@namespace}};

            public sealed partial {{sourceTypeKind}} {{@class}}
                : IOracleCommandParameters
            {
                public string CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public OracleDecimal AdjustmentPrice { get; private set; }
            
                [OracleInputOutputParameter(OracleDbType.Varchar2, 100)]
                public OracleString RecordId { get; private set; }
            
                [OracleReturnValueParameter(OracleDbType.Int32)]
                public OracleDecimal Available { get; private set; }
            }
            """;

        var expected = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            namespace {{@namespace}}
            {
                partial {{generatedTypeKind}} {{@class}}
                {
                    void IOracleCommandParameters.SetParameters(OracleCommand command)
                    {
                        command.Parameters.Add(nameof(CustomerSsn), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(NewPricingPlan), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(AdjustmentPrice), OracleDbType.Decimal, ParameterDirection.Output, null);
                        command.Parameters.Add(nameof(RecordId), OracleDbType.Varchar2, 100, ParameterDirection.InputOutput, null);
                        command.Parameters.Add(nameof(Available), OracleDbType.Int32, ParameterDirection.ReturnValue, null);
                    }

                    void IOracleCommandParameters.SetInputParameters(OracleCommand command)
                    {
                        command.Parameters[nameof(CustomerSsn)].Value = CustomerSsn;
                        command.Parameters[nameof(NewPricingPlan)].Value = NewPricingPlan;
                        command.Parameters[nameof(RecordId)].Value = RecordId;
                    }

                    void IOracleCommandParameters.ReadOutput(OracleCommand command)
                    {
                        AdjustmentPrice = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(AdjustmentPrice)].Value;
                        RecordId = (global::Oracle.ManagedDataAccess.Types.OracleString)command.Parameters[nameof(RecordId)].Value;
                        Available = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(Available)].Value;
                    }
                }
            }

            """;

        var mappings = new GeneratedSourceMappings
        {
            { $"{@namespace}.{@class}.Boilerplate.g.cs", expected }
        };

        await VerifyAsync(source, mappings);
    }

    [Test]
    public async Task RealCodeTest_Interface()
    {
        const string @namespace = "OracleCommandGenerator.SourceGenerators.Tests.Input";
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        var source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            namespace {{@namespace}};

            public partial interface {{@class}}
                : IOracleCommandParameters
            {
                public void Method();
                public string UnrelatedProperty { get; set; }

                string IOracleCommandParameters.CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public OracleDecimal AdjustmentPrice { get; }
            
                [OracleInputOutputParameter(OracleDbType.Varchar2, 100)]
                public OracleString RecordId { get; }
            
                [OracleReturnValueParameter(OracleDbType.Int32)]
                public OracleDecimal Available { get; }
            }
            """;

        // Interfaces should not generate anything
        var mappings = new GeneratedSourceMappings();
        await VerifyAsync(source, mappings);
    }

    [Theory]
    [TestCase(TypeDeclarationKinds.Struct, TypeDeclarationKinds.Struct)]
    [TestCase(TypeDeclarationKinds.RecordStruct, TypeDeclarationKinds.RecordStruct)]
    public async Task RealCodeTest_Struct(string sourceTypeKind, string generatedTypeKind)
    {
        const string @namespace = "OracleCommandGenerator.SourceGenerators.Tests.Input";
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        var source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            namespace {{@namespace}};

            public partial {{sourceTypeKind}} {{@class}}
                : IOracleCommandParameters
            {
                public string CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public OracleDecimal AdjustmentPrice { get; private set; }
            
                [OracleInputOutputParameter(OracleDbType.Varchar2, 100)]
                public OracleString RecordId { get; private set; }
            
                [OracleReturnValueParameter(OracleDbType.Int32)]
                public OracleDecimal Available { get; private set; }
            }
            """;

        var expected = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            namespace {{@namespace}}
            {
                partial {{generatedTypeKind}} {{@class}}
                {
                    void IOracleCommandParameters.SetParameters(OracleCommand command)
                    {
                        command.Parameters.Add(nameof(CustomerSsn), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(NewPricingPlan), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(AdjustmentPrice), OracleDbType.Decimal, ParameterDirection.Output, null);
                        command.Parameters.Add(nameof(RecordId), OracleDbType.Varchar2, 100, ParameterDirection.InputOutput, null);
                        command.Parameters.Add(nameof(Available), OracleDbType.Int32, ParameterDirection.ReturnValue, null);
                    }

                    void IOracleCommandParameters.SetInputParameters(OracleCommand command)
                    {
                        command.Parameters[nameof(CustomerSsn)].Value = CustomerSsn;
                        command.Parameters[nameof(NewPricingPlan)].Value = NewPricingPlan;
                        command.Parameters[nameof(RecordId)].Value = RecordId;
                    }

                    void IOracleCommandParameters.ReadOutput(OracleCommand command)
                    {
                        AdjustmentPrice = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(AdjustmentPrice)].Value;
                        RecordId = (global::Oracle.ManagedDataAccess.Types.OracleString)command.Parameters[nameof(RecordId)].Value;
                        Available = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(Available)].Value;
                    }
                }
            }

            """;

        var mappings = new GeneratedSourceMappings
        {
            { $"{@namespace}.{@class}.Boilerplate.g.cs", expected }
        };

        await VerifyAsync(source, mappings);
    }

    [Test]
    public async Task AliasTest()
    {
        const string @namespace = "OracleCommandGenerator.SourceGenerators.Tests.Input";
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        const string source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            using ODecimal = Oracle.ManagedDataAccess.Types.OracleDecimal;

            namespace {{@namespace}};

            public sealed partial class {{@class}}
                : OracleCommandParameters
            {
                public override string CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public ODecimal AdjustmentPrice { get; private set; }

                [OracleReturnValueParameter(OracleDbType.Int32)]
                public ODecimal Available { get; private set; }
            }
            """;

        const string expected = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            namespace {{@namespace}}
            {
                partial class {{@class}}
                {
                    protected override void SetParameters(OracleCommand command)
                    {
                        command.Parameters.Add(nameof(CustomerSsn), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(NewPricingPlan), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                        command.Parameters.Add(nameof(AdjustmentPrice), OracleDbType.Decimal, ParameterDirection.Output, null);
                        command.Parameters.Add(nameof(Available), OracleDbType.Int32, ParameterDirection.ReturnValue, null);
                    }

                    protected override void SetInputParameters(OracleCommand command)
                    {
                        command.Parameters[nameof(CustomerSsn)].Value = CustomerSsn;
                        command.Parameters[nameof(NewPricingPlan)].Value = NewPricingPlan;
                    }

                    protected override void ReadOutput(OracleCommand command)
                    {
                        AdjustmentPrice = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(AdjustmentPrice)].Value;
                        Available = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(Available)].Value;
                    }
                }
            }

            """;

        var mappings = new GeneratedSourceMappings
        {
            { $"{@namespace}.{@class}.Boilerplate.g.cs", expected }
        };

        await VerifyAsync(source, mappings);
    }

    [Test]
    public async Task NoNamespaaceTest()
    {
        const string @class = "CalculatePricingPlanAdjustmentCommandParameters";

        const string source = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;

            using ODecimal = Oracle.ManagedDataAccess.Types.OracleDecimal;

            public sealed partial class {{@class}}
                : OracleCommandParameters
            {
                public override string CommandName => "CUSTOMERS.CALCULATE_PRICING_PLAN_ADJUSTMENT";

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string CustomerSsn { get; set; }

                [OracleInputParameter(OracleDbType.Varchar2, 100)]
                public string NewPricingPlan { get; set; }

                [OracleOutputParameter(OracleDbType.Decimal)]
                public ODecimal AdjustmentPrice { get; private set; }

                [OracleReturnValueParameter(OracleDbType.Int32)]
                public ODecimal Available { get; private set; }
            }
            """;

        const string expected = $$"""
            using OracleCommandGenerator;
            using Oracle.ManagedDataAccess.Client;
            using Oracle.ManagedDataAccess.Types;
            using System;
            using System.Data;

            partial class {{@class}}
            {
                protected override void SetParameters(OracleCommand command)
                {
                    command.Parameters.Add(nameof(CustomerSsn), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                    command.Parameters.Add(nameof(NewPricingPlan), OracleDbType.Varchar2, 100, ParameterDirection.Input, null);
                    command.Parameters.Add(nameof(AdjustmentPrice), OracleDbType.Decimal, ParameterDirection.Output, null);
                    command.Parameters.Add(nameof(Available), OracleDbType.Int32, ParameterDirection.ReturnValue, null);
                }

                protected override void SetInputParameters(OracleCommand command)
                {
                    command.Parameters[nameof(CustomerSsn)].Value = CustomerSsn;
                    command.Parameters[nameof(NewPricingPlan)].Value = NewPricingPlan;
                }

                protected override void ReadOutput(OracleCommand command)
                {
                    AdjustmentPrice = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(AdjustmentPrice)].Value;
                    Available = (global::Oracle.ManagedDataAccess.Types.OracleDecimal)command.Parameters[nameof(Available)].Value;
                }
            }

            """;

        var mappings = new GeneratedSourceMappings
        {
            { $"{@class}.Boilerplate.g.cs", expected }
        };

        await VerifyAsync(source, mappings);
    }
}
