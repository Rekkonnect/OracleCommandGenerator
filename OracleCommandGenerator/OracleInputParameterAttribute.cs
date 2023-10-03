using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public sealed class OracleInputParameterAttribute : OracleParameterAttributeBase
{
    public override ParameterDirection Direction => ParameterDirection.Input;

    public OracleInputParameterAttribute(OracleDbType parameterType)
        : base(parameterType) { }
    public OracleInputParameterAttribute(OracleDbType parameterType, int size)
        : base(parameterType, size) { }
}
