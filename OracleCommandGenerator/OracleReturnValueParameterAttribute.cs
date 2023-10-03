using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public sealed class OracleReturnValueParameterAttribute : OracleParameterAttributeBase
{
    public override ParameterDirection Direction => ParameterDirection.ReturnValue;

    public OracleReturnValueParameterAttribute(OracleDbType parameterType)
        : base(parameterType) { }
    public OracleReturnValueParameterAttribute(OracleDbType parameterType, int size)
        : base(parameterType, size) { }
}
