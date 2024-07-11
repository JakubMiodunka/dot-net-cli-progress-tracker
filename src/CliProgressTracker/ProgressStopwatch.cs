using System.Text;


namespace CliProgressTracker;

/// <summary>
/// Utility meant to measure time statistics of monitored process.
/// </summary>
/// <remarks>
/// Currently implemented measurements are represented by System.DateTime and System.TimeSpan 
/// type properties and consists of process start time, estimated process duration, 
/// estimated end time and average time required to process one step.
/// Invalid state of those properties are indicated by System.DateTime.MinValue
/// and System.TimeSpan.MinValue.
/// </remarks>
internal sealed class ProgressStopwatch
{
    #region Constants
    private const char OpeningBracket = '[';
    private const char ClosingBracket = ']';
    private const char FieldSeparator = '|';
    #endregion

    #region Properties
    internal readonly DateTime RuntimeBegin;
    internal TimeSpan EstimatedRemainingRuntime { get; private set; }
    internal DateTime EstimatedRuntimeFinish { get; private set; }
    internal TimeSpan AverageTimePerStep { get; private set; }

    private readonly Process _trackedProcess;
    #endregion

    #region Utilities
    /// <summary>
    /// Generates string-based representation of provided System.DateTime object.
    /// </summary>
    /// <remarks>
    /// Format of generated string:
    ///     '<24h format hour>:<minutes>' or '--:--' if value of given the object is minimal. 
    /// </remarks>
    /// <param name="dateTime">
    /// System.DateTime object, which representation shall be generated.
    /// </param>
    /// <returns>
    /// String representation of given System.DateTime object.
    /// </returns>
    private static string AsString(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
        {
            return "--:--";
        }

        return dateTime.ToString(@"HH\:mm");
    }

    /// <summary>
    /// Generates string-based representation of provided System.TimeSpan object.
    /// </summary>
    /// <remarks>
    /// Format of generated string:
    ///     '<hours>:<minutes>:<seconds>' or '--:--' if value of given the object is minimal. 
    /// </remarks>
    /// <param name="timeSpan">
    /// System.TimeSpan object, which representation shall be generated.
    /// </param>
    /// <returns>
    /// String representation of given System.TimeSpan object.
    /// </returns>
    private static string AsString(TimeSpan timeSpan)
    {
        if (timeSpan == TimeSpan.MinValue)
        {
            return "--:--:--";
        }

        return $"{timeSpan.TotalHours:00}:{timeSpan.ToString(@"mm\:ss")}";
    }
    #endregion

    #region Class instantiation
    /// <summary>
    /// Creates a new progress stopwatch.
    /// </summary>
    /// <remarks>
    /// It is assumed, that process runtime begins in moment of stopwatch instance creation.
    /// </remarks>
    /// <param name="trackedProcess">
    /// Process, to which measurements taken by the stopwatch shall refer.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown, when at least one reference-type argument is a null reference.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown, when at least one argument will be considered as invalid.
    /// </exception>
    internal ProgressStopwatch(Process trackedProcess)
    {
        if (trackedProcess is null)
        {
            string argumentName = nameof(trackedProcess);
            const string ErrorMessage = "Provided process to track is a null reference:";
            throw new ArgumentNullException(argumentName, ErrorMessage);
        }

        if (trackedProcess.CurrentStep != 0)
        {
            string argumentName = nameof(trackedProcess);
            const string ErrorMessage = "Provided process not in its initial state:";
            throw new ArgumentException(ErrorMessage, argumentName);
        }

        RuntimeBegin = DateTime.Now;
        EstimatedRemainingRuntime = TimeSpan.MinValue;
        EstimatedRuntimeFinish = DateTime.MinValue;
        AverageTimePerStep = TimeSpan.MinValue;

        _trackedProcess = trackedProcess;
        _trackedProcess.AddCallback(Update);
    }
    #endregion

    #region Interactions
    /// <summary>
    /// Triggers the update of measurements taken by the stopwatch.
    /// </summary>
    private void Update()
    {
        if (_trackedProcess.CurrentStep == 0)
        {
            return;
        }

        DateTime currentTime = DateTime.Now;
        TimeSpan runtimeDuration = currentTime - RuntimeBegin;

        AverageTimePerStep = runtimeDuration / _trackedProcess.CurrentStep;
        EstimatedRemainingRuntime = (_trackedProcess.TotalSteps - _trackedProcess.CurrentStep) * AverageTimePerStep;
        EstimatedRuntimeFinish = currentTime + EstimatedRemainingRuntime;
    }

    /// <summary>
    /// Generates string-based representation of current measurements taken by the stopwatch.
    /// </summary>
    /// <remarks>
    /// Format of generated string:
    ///     '[<runtime begin>|<estimated runtime finish>|<average time per step>]'
    /// </remarks>
    /// <returns>
    /// String representation of current measurements taken by the stopwatch.
    /// </returns>
    public override string ToString()
    {
        var stopwatch = new StringBuilder();
        stopwatch.Append(OpeningBracket);

        stopwatch.Append(AsString(RuntimeBegin));
        stopwatch.Append(FieldSeparator);

        stopwatch.Append(AsString(EstimatedRuntimeFinish));
        stopwatch.Append(FieldSeparator);

        stopwatch.Append(AsString(AverageTimePerStep));
        stopwatch.Append(ClosingBracket);

        return stopwatch.ToString();
    }
    #endregion
}

