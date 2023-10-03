using Oracle.ManagedDataAccess.Client;

namespace OracleCommandGenerator;

public sealed class ConnectionOpenHandler : IAsyncDisposable
{
    private readonly OracleConnection _connection;

    public ConnectionOpenHandler(OracleConnection connection)
    {
        _connection = connection;
    }

    public async Task OpenAsync()
    {
        await _connection.OpenAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.CloseAsync();
    }

    public static async Task<ConnectionOpenHandler> CreateOpen(OracleConnection connection)
    {
        var instance = new ConnectionOpenHandler(connection);
        await instance.OpenAsync();
        return instance;
    }
}
