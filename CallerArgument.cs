using System.Runtime.CompilerServices;

public static class CallerArgument
{
  public static IEnumerable<(T? Value, string? Name)> Parameter<T>(
    this (T? Value, string? Name) first,
    T value,
    [CallerArgumentExpression("value")] string? paramName = null
  )
  {
    yield return first;
    yield return Parameter(value, paramName);
  }

  public static IEnumerable<(T? Value, string? Name)> Params<T>(
    this (T? Value, string? Name) first,
    T[] parameters,
    [CallerArgumentExpression("parameters")] string? paramName = null
  )
  {
    yield return first;
    foreach (var argument in Params(parameters, paramName))
      yield return argument;
  }

  public static IEnumerable<(T? Value, string? Name)> Parameter<T>(
    this IEnumerable<(T? Value, string? Name)> arguments,
    T value,
    [CallerArgumentExpression("value")] string? paramName = null
    )
  {
    foreach (var argument in arguments)
      yield return argument;
    yield return Parameter(value, paramName);
  }

  public static IEnumerable<(T? Value, string? Name)> Params<T>(
    this IEnumerable<(T? Value, string? Name)> arguments,
    T[] parameters,
    [CallerArgumentExpression("parameters")] string? paramName = null
    )
    => arguments.Concat(Params(parameters, paramName));

  public static (T? Value, string? Name) Parameter<T>(
    T value,
    [CallerArgumentExpression("value")] string? paramName = null
    )
    => (value, paramName);

  public static IEnumerable<(T? Value, string? Name)> Params<T>(T[] parameters,
    [CallerArgumentExpression("parameters")]
    string? paramName = null
    )
    => paramName == null 
      ? parameters.Select((value, i) => ((T?)value, $"[{i}]"))
      : parameters.Select((value, i) => ((T?)value, $"{paramName}[{i}]"));

}
