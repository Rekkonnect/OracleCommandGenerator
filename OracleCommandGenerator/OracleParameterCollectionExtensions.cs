using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

public static class OracleParameterCollectionExtensions
{
    public static OracleParameter Add(this OracleParameterCollection parameters, string name, OracleDbType type, ParameterDirection parameterDirection, object? value = null)
    {
        var parameter = OracleParameterFactory.GenerateParameter(name, type, parameterDirection, value);
        parameters.Add(parameter);
        return parameter;
    }
    public static OracleParameter Add(this OracleParameterCollection parameters, string name, OracleDbType type, int size, ParameterDirection parameterDirection, object? value = null)
    {
        var parameter = OracleParameterFactory.GenerateParameter(name, type, size, parameterDirection, value);
        parameters.Add(parameter);
        return parameter;
    }

    public static OracleParameter AddInput(this OracleParameterCollection parameters, string name, OracleDbType type, object? value = null)
    {
        return parameters.Add(name, type, ParameterDirection.Input, value);
    }
    public static OracleParameter AddInput(this OracleParameterCollection parameters, string name, OracleDbType type, int size, object? value = null)
    {
        return parameters.Add(name, type, size, ParameterDirection.Input, value);
    }

    public static OracleParameter AddOutput(this OracleParameterCollection parameters, string name, OracleDbType type)
    {
        return parameters.Add(name, type, ParameterDirection.Output);
    }
    public static OracleParameter AddOutput(this OracleParameterCollection parameters, string name, OracleDbType type, int size)
    {
        return parameters.Add(name, type, size, ParameterDirection.Output);
    }

    public static OracleParameter AddInputOutput(this OracleParameterCollection parameters, string name, OracleDbType type, object? value = null)
    {
        return parameters.Add(name, type, ParameterDirection.InputOutput, value);
    }
    public static OracleParameter AddInputOutput(this OracleParameterCollection parameters, string name, OracleDbType type, int size, object? value = null)
    {
        return parameters.Add(name, type, size, ParameterDirection.InputOutput, value);
    }

    public static OracleParameter AddReturnValue(this OracleParameterCollection parameters, string name, OracleDbType type, object? value = null)
    {
        return parameters.Add(name, type, ParameterDirection.ReturnValue, value);
    }
    public static OracleParameter AddReturnValue(this OracleParameterCollection parameters, string name, OracleDbType type, int size, object? value = null)
    {
        return parameters.Add(name, type, size, ParameterDirection.ReturnValue, value);
    }

    // This simply enables using the params feature that is not available in the original signature
    public static void AddRangeParams(this OracleParameterCollection collection, params OracleParameter[] newParameters)
    {
        collection.AddRange(newParameters);
    }
}
