using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleCommandGenerator;

/// <summary>
/// Represents a parameter collection for an Oracle command.
/// Types inheriting this interface should declare fields or properties with the
/// designated attributes for marking them as Oracle parameters.
/// </summary>
public interface IOracleCommandParameters
{
    public abstract string CommandName { get; }

    /// <summary>
    /// Initializes an <seealso cref="ParameterizedOracleCommand"/> from the given connection,
    /// which will be ready to be processed.
    /// </summary>
    /// <remarks>
    /// Defaults to creating a new instance as a stored procedure with the given
    /// command name.
    /// </remarks>
    public virtual ParameterizedOracleCommand InitializeCommand(OracleConnection connection)
    {
        return new(CommandName, connection, CommandType.StoredProcedure);
    }

    public void InitializeCommandParameters(ParameterizedOracleCommand command)
    {
        SetParameters(command.Command);
    }

    /// <summary>
    /// Sets all the input and output parameters to the command,
    /// declaring their directions, names, types and sizes but not initializing
    /// the input parameters' values.
    /// </summary>
    /// <remarks>
    /// This method is to be overriden from generated code.
    /// To initialize the input parameters' values, use <see cref="PrepareInput(OracleCommand)"/>.
    /// </remarks>
    protected abstract void SetParameters(OracleCommand command);

    /// <summary>
    /// Sets the input parameters' values.
    /// </summary>
    /// <remarks>
    /// This method is to be overriden from generated code. To apply further
    /// normalization to the input parameters' values, use <see cref="NormalizeInputParameters(OracleCommand)"/>.
    /// </remarks>
    protected abstract void SetInputParameters(OracleCommand command);

    /// <summary>
    /// Normalizes the input properties' values.
    /// </summary>
    /// <remarks>
    /// This method is invoked before initializing the input parameters.
    /// It is meant to be overriden by hand.
    /// </remarks>
    protected virtual void NormalizeInputProperties() { }

    /// <summary>
    /// Normalizes the input parameters' values.
    /// </summary>
    /// <remarks>
    /// This method is invoked after initializing the input parameters.
    /// It is meant to be overriden by hand.
    /// </remarks>
    protected virtual void NormalizeInputParameters(OracleCommand command) { }

    /// <summary>
    /// Prepares the input parameters for the specified command.
    /// First the input properties are normalized, based on their already initialized values,
    /// then the input parameters are generated, and finally the input parameters are normalized.
    /// </summary>
    private void PrepareInput(OracleCommand command)
    {
        NormalizeInputProperties();
        SetInputParameters(command);
        NormalizeInputParameters(command);
    }

    /// <summary>
    /// Prepares the input parameters for the specified command.
    /// First the input properties are normalized, based on their already initialized values,
    /// then the input parameters are generated, and finally the input parameters are normalized.
    /// </summary>
    public void PrepareInput(ParameterizedOracleCommand command)
    {
        PrepareInput(command.Command);
    }

    /// <summary>
    /// Parses the output parameters from the specified command.
    /// The values are first read from the command's parameter collection,
    /// and then normalized.
    /// </summary>
    private void ParseOutput(OracleCommand command)
    {
        ReadOutput(command);
        NormalizeOutputProperties();
    }

    /// <summary>
    /// Parses the output parameters from the specified command.
    /// The values are first read from the command's parameter collection,
    /// and then normalized.
    /// </summary>
    public void ParseOutput(ParameterizedOracleCommand command)
    {
        ParseOutput(command.Command);
    }

    /// <summary>
    /// Reads the values from the output parameters.
    /// </summary>
    /// <remarks>
    /// This method is to be overriden from generated code.
    /// </remarks>
    protected abstract void ReadOutput(OracleCommand command);

    /// <summary>
    /// Normalizes the read output properties.
    /// </summary>
    protected virtual void NormalizeOutputProperties() { }
}
