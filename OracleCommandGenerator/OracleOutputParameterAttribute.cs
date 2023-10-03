using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public sealed class OracleOutputParameterAttribute : OracleParameterAttributeBase
{
    public override ParameterDirection Direction => ParameterDirection.Output;

    public OracleOutputParameterAttribute(OracleDbType parameterType)
        : base(parameterType) { }
    public OracleOutputParameterAttribute(OracleDbType parameterType, int size)
        : base(parameterType, size) { }
}
