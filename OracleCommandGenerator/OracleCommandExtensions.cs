using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public static class OracleCommandExtensions
{
    public static void AddParameter(this OracleCommand command, string name, OracleDbType type, ParameterDirection parameterDirection)
    {
        command.Parameters.Add(name, type, parameterDirection);
    }
    public static void AddParameter(this OracleCommand command, string name, OracleDbType type, int size, ParameterDirection parameterDirection)
    {
        command.Parameters.Add(name, type, size, parameterDirection);
    }
}
