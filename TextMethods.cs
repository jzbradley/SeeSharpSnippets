using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Immutable;

namespace TextMethods;

public static class TextExtensions
{
  public static IEnumerable<Match> Match
    (this string text, [StringSyntax("Regex")] string pattern,
      TimeSpan timeout,
      RegexOptions options=RegexOptions.Multiline | RegexOptions.ExplicitCapture)
  {
    foreach (System.Text.RegularExpressions.Match match in Regex.Matches(text, pattern, options, timeout))
    {
      if (!match.Success) continue;
      yield return TextMethods.Match.From(match);
    }
  }

  internal static Range GetRange(this System.Text.RegularExpressions.Capture match) => new (match.Index, match.Index + (match.Length - 1));
}

public readonly record struct Match(Range Range, IImmutableDictionary<string, Group> Groups) : IReadOnlyDictionary<string, Group>
{
  internal static Match From(System.Text.RegularExpressions.Match match)
    => new(
      match.GetRange(),
      match.Groups
        .Cast<System.Text.RegularExpressions.Group>()
        .Where(g => g.Success)
        .ToImmutableDictionary(
          g => g.Name,
          Group.From
        )
    );

  public IEnumerator<KeyValuePair<string, Group>> GetEnumerator() => Groups.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Groups).GetEnumerator();

  public int Count => Groups.Count;

  public bool ContainsKey(string key) => Groups.ContainsKey(key);

  public bool TryGetValue(string key, out Group value) => Groups.TryGetValue(key, out value);

  public Group this[string key] => Groups[key];

  public IEnumerable<string> Keys => Groups.Keys;

  public IEnumerable<Group> Values => Groups.Values;
}

public readonly record struct Group(Range Range, string Name, IImmutableList<Capture> Captures) : IReadOnlyList<Capture>
{
  internal static Group From(System.Text.RegularExpressions.Group g)
    => new (
      g.GetRange(),
      g.Name,
      g.Captures
        .Select((c, i) => new Capture(c.GetRange(), i))
        .ToImmutableList()
    );

  public IEnumerator<Capture> GetEnumerator() => Captures.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Captures).GetEnumerator();

  public int Count => Captures.Count;

  public Capture this[int index] => Captures[index];
}

public readonly record struct Capture(Range Range, int Index);
