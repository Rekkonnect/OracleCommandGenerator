using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property,
    AllowMultiple = true, Inherited = false)]
public abstract class OracleParameterAttributeBase : Attribute
{
    public OracleDbType ParameterType { get; }
    public int? Size { get; }

    public abstract ParameterDirection Direction { get; }

    protected OracleParameterAttributeBase(OracleDbType parameterType, int size)
        : this(parameterType, (int?)size) { }
    protected OracleParameterAttributeBase(OracleDbType parameterType)
        : this(parameterType, null) { }

    private OracleParameterAttributeBase(OracleDbType parameterType, int? size)
    {
        ParameterType = parameterType;
        Size = size;
    }
}
