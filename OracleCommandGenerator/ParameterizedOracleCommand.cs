using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public class ParameterizedOracleCommand
{
    public OracleCommand Command { get; }

    public ParameterizedOracleCommand(string name, OracleConnection connection, CommandType commandType)
    {
        Command = new OracleCommand(name, connection)
        {
            CommandType = commandType
        };
    }

    public async Task ExecuteNonQueryAsync()
    {
        await using var x = await ConnectionOpenHandler.CreateOpen(Command.Connection);
        await Command.ExecuteNonQueryAsync();
    }
}
