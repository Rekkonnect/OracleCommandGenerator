using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public sealed class OracleInputOutputParameterAttribute : OracleParameterAttributeBase
{
    public override ParameterDirection Direction => ParameterDirection.InputOutput;

    public OracleInputOutputParameterAttribute(OracleDbType parameterType)
        : base(parameterType) { }
    public OracleInputOutputParameterAttribute(OracleDbType parameterType, int size)
        : base(parameterType, size) { }
}
