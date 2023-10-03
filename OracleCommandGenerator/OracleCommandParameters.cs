using Oracle.ManagedDataAccess.Client;

namespace OracleCommandGenerator;

/// <summary>
/// Represents a parameter collection for an Oracle command.
/// Classes inheriting this class should declare fields or properties with the
/// designated attributes for marking them as Oracle parameters.
/// </summary>
public abstract class OracleCommandParameters : IOracleCommandParameters
{
    /// <summary>
    /// Gets the name of the command, which is the qualified name of the stored
    /// procedure/function that is going to be executed.
    /// </summary>
    public abstract string CommandName { get; }

    public virtual ParameterizedOracleCommand InitializeCommand(OracleConnection connection)
    {
        return (this as IOracleCommandParameters).InitializeCommand(connection);
    }

    public void InitializeCommandParameters(ParameterizedOracleCommand command)
    {
        SetParameters(command.Command);
    }

    void IOracleCommandParameters.SetParameters(OracleCommand command)
    {
        SetParameters(command);
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

    void IOracleCommandParameters.SetInputParameters(OracleCommand command)
    {
        SetInputParameters(command);
    }

    /// <summary>
    /// Sets the input parameters' values.
    /// </summary>
    /// <remarks>
    /// This method is to be overriden from generated code. To apply further
    /// normalization to the input parameters' values, use <see cref="NormalizeInputParameters(OracleCommand)"/>.
    /// </remarks>
    protected abstract void SetInputParameters(OracleCommand command);

    void IOracleCommandParameters.NormalizeInputProperties()
    {
        NormalizeInputProperties();
    }

    /// <summary>
    /// Normalizes the input properties' values.
    /// </summary>
    /// <remarks>
    /// This method is invoked before initializing the input parameters.
    /// It is meant to be overriden by hand.
    /// </remarks>
    protected virtual void NormalizeInputProperties() { }

    void IOracleCommandParameters.NormalizeInputParameters(OracleCommand command)
    {
        NormalizeInputParameters(command);
    }

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

    void IOracleCommandParameters.ReadOutput(OracleCommand command)
    {
        ReadOutput(command);
    }

    /// <summary>
    /// Reads the values from the output parameters.
    /// </summary>
    /// <remarks>
    /// This method is to be overriden from generated code.
    /// </remarks>
    protected abstract void ReadOutput(OracleCommand command);

    void IOracleCommandParameters.NormalizeOutputProperties()
    {
        NormalizeOutputProperties();
    }

    /// <summary>
    /// Normalizes the read output properties.
    /// </summary>
    protected virtual void NormalizeOutputProperties() { }
}
