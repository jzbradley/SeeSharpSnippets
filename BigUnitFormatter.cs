// Formats very large numbers with simple unit ratios (as with metric or imperial).
public class BigUnitFormatter
{
  public readonly record struct UnitValue(string Name, BigInteger Value);

  private readonly IReadOnlyList<UnitValue> _ratios;
  private readonly string _minimumUnit;
  public Func<UnitValue, string> UnitFormatter { set; private get; } = DefaultUnitFormatter;

  private static string DefaultUnitFormatter(UnitValue d)
  {
    return $"{d.Value} {d.Name}";
  }

  public Func<(IList<string> Parts, bool IsNegative), string> JoinFormatter { set; private get; }
    = DefaultJoinFormatter;

  private static string DefaultJoinFormatter((IList<string> Parts, bool IsNegative) units)
  {
    var result = string.Join(", ", units.Parts);
    return units.IsNegative ? $"-({result})" : result;
  }

  public BigUnitFormatter(params UnitValue[] ratios)
  {
    _minimumUnit = ratios[0].Name;
    var values = new List<UnitValue>();
    BigInteger current = 1;

    foreach (var (name, multiplier) in ratios)
    {
      current *= multiplier;
      values.Add(new (name, current));
    }

    values.Reverse();
    _ratios = values.AsReadOnly();
  }

  public string Format(BigInteger value)
  {
    if (value == 0)
      return UnitFormatter(new (_minimumUnit, BigInteger.Zero));
    
    var isNegative = BigInteger.IsNegative(value);
    var remaining = BigInteger.Abs(value);
    var parts = new List<string>();

    foreach (var ratio in _ratios)
    {
      if (remaining >= ratio.Value)
      {
        remaining = BigInteger.DivRem(remaining, ratio.Value, out var count);
        parts.Add($"{count} {ratio.Name}");
      }

      if (remaining.IsZero)
        break;
    }

    return JoinFormatter((parts, isNegative));
  }
}
