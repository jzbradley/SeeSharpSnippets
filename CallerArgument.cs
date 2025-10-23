using System.Runtime.CompilerServices;

public static class CallerArgument
{
  public static IEnumerable<(T? Value, string? Name)> Single<T>(
    this (T? Value, string? Name) first,
    T value,
    [CallerArgumentExpression(nameof(value))] string? paramName = null
  )
  {
    yield return first;
    yield return Single(value, paramName);
  }

  public static IEnumerable<(T? Value, string? Name)> Aggregate<T>(
    this (T? Value, string? Name) first,
    T[] parameters,
    [CallerArgumentExpression(nameof(parameters))] string? paramName = null
  )
  {
    yield return first;
    foreach (var argument in Aggregate(parameters, paramName))
      yield return argument;
  }

  public static IEnumerable<(T? Value, string? Name)> Single<T>(
    this IEnumerable<(T? Value, string? Name)> arguments,
    T value,
    [CallerArgumentExpression(nameof(value))] string? paramName = null
    )
  {
    foreach (var argument in arguments)
      yield return argument;
    yield return Single(value, paramName);
  }

  public static IEnumerable<(T? Value, string? Name)> Aggregate<T>(
    this IEnumerable<(T? Value, string? Name)> arguments,
    T[] parameters,
    [CallerArgumentExpression(nameof(parameters))] string? paramName = null
    )
    => arguments.Concat(Aggregate(parameters, paramName));

  public static (T? Value, string? Name) Single<T>(
    T value,
    [CallerArgumentExpression(nameof(value))] string? paramName = null
    )
    => (value, paramName);

  public static IEnumerable<(T? Value, string? Name)> Aggregate<T>(T[] parameters,
    [CallerArgumentExpression(nameof(parameters))]
    string? paramName = null
    )
    => paramName == null 
      ? parameters.Select((value, i) => ((T?)value, $"[{i}]"))
      : parameters.Select((value, i) => ((T?)value, $"{paramName}[{i}]"));
}
