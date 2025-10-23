public static class GuardClauses
{
  public enum Behavior
  {
    /// <summary>
    /// Validate guard clauses in order, throwing the first clause to fail.
    /// </summary>
    FailFast,
    /// <summary>
    /// Validate all guard clauses, throwing `AggregateException` in the event of multiple failures.
    /// </summary>
    CollectAndThrow,
    /// <summary>
    /// Ignore guard clauses entirely. Useful for debugging exception handling.
    /// WARNING: This may lead to a multitude of unhandled and uncaught exceptions.
    /// </summary>
    Skip
  }

  public static Behavior DefaultBehavior = Behavior.FailFast;
  
  /// <summary>Provides data for an event that is raised when an exception is thrown by a guard clause.</summary>
  public class GuardClauseExceptionEventArgs : EventArgs
  {
    public Exception Exception { get; }

    /// <summary>Initializes a new instance of the <see cref="T:System.UnhandledExceptionEventArgs" /> class with the exception object and a common language runtime termination flag.</summary>
    /// <param name="exception">The exception that is not handled.</param>
    /// <param name="behavior">The behavior of the guard clause block.</param>
    public GuardClauseExceptionEventArgs(Exception exception, Behavior behavior)
    {
      Exception = exception;
      Behavior = behavior;
    }
    
    //private bool _isHandled;
    ///// <summary>
    ///// Set to `true` if the exception should not be thrown.
    ///// </summary>
    //public bool IsHandled
    //{
    //  get => _isHandled;
    //  set => _isHandled |= value;
    //}

    public Behavior Behavior { get; }
  }

  public delegate void GuardClauseEventHandler(GuardClauseExceptionEventArgs args);

  private static void OnCatch(GuardClauseExceptionEventArgs args)
  {
    foreach (var handler in Handlers)
    {
      handler.Invoke(args);
    }
  }

  private static readonly WeakReferenceSet<GuardClauseEventHandler> Handlers = new();

  /// <summary>
  /// Specifies event handlers for guard clauses.
  /// </summary>
  public static event GuardClauseEventHandler Catch
  {
    add => Handlers.Add(value);
    remove => Handlers.Remove(value);
  }

  /// <summary>
  /// Validate guard clauses using the method specified by `GuardClauses.DefaultBehavior`
  /// </summary>
  /// <example>
  /// <code>
  /// GuardClauses.Validate(
  ///   ()=>ArgumentNullException.ThrowIfNull(nullable),
  ///   ()=>ArgumentOutOfRangeException.ThrowIfZero(denominator),
  ///   ()=>double.IsNaN(frequency) ? new ArgumentOutOfRangeException(nameof(frequency)) : null
  /// );
  /// </code>
  /// </example>
  /// <param name="clauses">A list of delegates which either throw (`Action`) or optionally return (`Func&lt;Exception?&gt;`) exceptions.</param>
  public static void Validate(params Delegate[] clauses)
  {
    switch (DefaultBehavior)
    {
      case Behavior.Skip:
        return;
      case Behavior.CollectAndThrow:
        CollectAndThrow(clauses);
        return;
      case Behavior.FailFast:
      default:
        FailFast(clauses);
        return;
    }
  }
  
  /// <summary>
  /// Validate all guard clauses, throwing `AggregateException` in the event of multiple failures.
  /// </summary>
  /// <example>
  /// <code>
  /// GuardClauses.CollectAndThrow(
  ///   ()=>ArgumentNullException.ThrowIfNull(nullable),
  ///   ()=>ArgumentOutOfRangeException.ThrowIfZero(denominator),
  ///   ()=>double.IsNaN(frequency) ? new ArgumentOutOfRangeException(nameof(frequency)) : null
  /// );
  /// </code>
  /// </example>
  /// <param name="clauses">A list of delegates which either throw (`Action`) or optionally return (`Func&lt;Exception?&gt;`) exceptions.</param>
  public static void CollectAndThrow(params Delegate[] clauses)
  {
    List<Exception>? validationErrors = null;
    foreach (var clause in clauses)
    {
      var exception = TestClause(clause);
      if (exception==null) continue;

      (validationErrors ??= new()).Add(exception);
    }

    {
      Exception exception;
      switch (validationErrors?.Count)
      {
        default:
          return;
        case 1:
        {
          exception = validationErrors[0];
          break;
        }
        case > 1:
        {
          exception = new AggregateException(validationErrors);
          break;
        }
      }
      var args = new GuardClauseExceptionEventArgs(exception, Behavior.CollectAndThrow);
      OnCatch(args);
      // if (args.IsHandled) return;
      throw exception;
    }
  }
  
  /// <summary>
  /// Validate guard clauses in order, throwing the first clause to fail.
  /// </summary>
  /// <example>
  /// <code>
  /// GuardClauses.FailFast(
  ///   ()=>ArgumentNullException.ThrowIfNull(nullable),
  ///   ()=>ArgumentOutOfRangeException.ThrowIfZero(denominator),
  ///   ()=>double.IsNaN(frequency) ? new ArgumentOutOfRangeException(nameof(frequency)) : null
  /// );
  /// </code>
  /// </example>
  /// <param name="clauses">A list of delegates which either throw (`Action`) or optionally return (`Func&lt;Exception?&gt;`) exceptions.</param>
  public static void FailFast(params Delegate[] clauses)
  {
    
    foreach (var clause in clauses)
    {
      var exception = TestClause(clause);
      if (exception==null) continue;
      
      var args = new GuardClauseExceptionEventArgs(exception, Behavior.FailFast);
      OnCatch(args);
      // if (args.IsHandled) continue;
      throw exception;
    }
  }

  private static Exception? TestClause(Delegate clause)
  {
    Exception? exception = null;
    try
    {
      switch (clause)
      {
        case Action actionClause:
          actionClause();
          break;
        case Func<Exception?> funcClause:
          exception = funcClause();
          break;
      }
    }
    catch (Exception e)
    {
      exception = e;
    }

    return exception;
  }
}
