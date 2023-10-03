using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public static class OracleParameterFactory
{
    public static OracleParameter GenerateParameter(string name, OracleDbType type, ParameterDirection parameterDirection, object? value = null)
    {
        return new(name, type)
        {
            Direction = parameterDirection,
            Value = value,
        };
    }
    public static OracleParameter GenerateParameter(string name, OracleDbType type, int size, ParameterDirection parameterDirection, object? value = null)
    {
        return new(name, type, size)
        {
            Direction = parameterDirection,
            Value = value,
        };
    }
}
