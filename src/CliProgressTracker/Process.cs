namespace CliProgressTracker;

/// <summary>
/// Representation of tracked process state.
/// </summary>
internal sealed class Process
{
    #region Properties
    internal readonly int TotalSteps;
    internal int CurrentStep { get; private set; }

    private readonly List<Action> _callbacks;
    #endregion

    #region Class instantiation
    /// <summary>
    /// Creates a new process representation.
    /// </summary>
    /// <param name="totalSteps">
    /// Total steps, required to complete the process.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when value of at least one argument will be considered as invalid.
    /// </exception>
    internal Process(int totalSteps)
    {
        if (totalSteps <= 0)
        {
            string argumentName = nameof(totalSteps);
            string errorMessage = $"Invalid number of process steps: {totalSteps}";
            throw new ArgumentOutOfRangeException(argumentName, totalSteps, errorMessage);
        }

        TotalSteps = totalSteps;
        CurrentStep = 0;

        _callbacks = new List<Action>();
    }
    #endregion

    #region Interactions
    /// <summary>
    /// Adds given action to callback list.
    /// Actions from the list are invoked every time, when state of the process change.
    /// </summary>
    /// <param name="callback">
    /// Action, which shall be invoked every time when state of the process will change.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown as subscription attempt is performed, when process is not in its initial state.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when at least one reference-type argument is a null reference.
    /// </exception>
    internal void AddCallback(Action callback)
    {
        if (CurrentStep != 0)
        {
            const string ErrorMessage = "Subscription attempt, when process is not in its initial state:";
            throw new InvalidOperationException(ErrorMessage);
        }

        if (callback is null)
        {
            string argumentName = nameof(callback);
            const string ErrorMessage = "Specified callback is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
        }

        _callbacks.Add(callback);
    }

    /// <summary>
    /// Updates the state of the process.
    /// </summary>
    /// <param name="steps">
    /// Number of steps, performed by the process since last update.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown, when value of at least one argument will be considered as invalid.
    /// </exception>
    internal void Update(int steps)
    {
        if (steps < 0)
        {
            string argumentName = nameof(steps);
            string erorMessage = $"Invalid number of steps updating the process: {steps}";
            throw new ArgumentOutOfRangeException(argumentName, steps, erorMessage);
        }

        if (steps == 0)
        {
            return;
        }

        CurrentStep += steps;

        foreach (Action subscriber in _callbacks)
        {
            subscriber.Invoke();
        }
    }
    #endregion
}