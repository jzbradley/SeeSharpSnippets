using System.Collections;

/// <summary>Represents a set of weak references.</summary>
/// <typeparam name="T">The reference type of items in the set.</typeparam>
public class WeakReferenceSet<T> : ICollection<T>
  where T : class
{
  sealed class WeakReferenceDictionary : Dictionary<int, WeakReferenceList> { }

  sealed class WeakReferenceList : List<WeakReference<T>> { }

  private readonly WeakReferenceDictionary _index = new();


  /// <summary>
  /// Counts the number of resolvable weak references in the set.
  /// </summary>
  /// <returns>The number of resolvable references found.</returns>
  public int Count()
  {
    // Since references could be dropped at any time, we have to count.

    var mappings = _index
      .Select(kvp=>(kvp.Key,kvp.Value))
      .ToList();
    var count = 0;
    foreach (var (hashCode, list) in mappings)
    {
      // Clean up dead references
      for (var j = list.Count - 1; j >= 0; j--)
        if (!list[j].TryGetTarget(out _))
          list.RemoveAt(j);

      if (list.Count == 0)
      {
        _index.Remove(hashCode);
        continue;
      }
      count += list.Count;
    }

    return count;
  }
  
  /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  
  /// <inheritdoc cref="ICollection{T}.CopyTo"/>
  void ICollection<T>.CopyTo(T[] array, int arrayIndex)
  {
    var items = this.ToList();
    items.CopyTo(array, arrayIndex);
  }
  
  /// <inheritdoc cref="ICollection{T}.Count"/>
  int ICollection<T>.Count => Count();
  
  /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
  bool ICollection<T>.IsReadOnly => false;
  
  /// <inheritdoc cref="ICollection{T}.Add"/>
  void ICollection<T>.Add(T item)
  {
    Add(item);
  }
  
  /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
  public IEnumerator<T> GetEnumerator()
  {
    var mappings = _index
      .Select(kvp=>(kvp.Key,kvp.Value))
      .ToList();
    
    foreach (var (hashCode, list) in mappings)
    {
      // Clean up dead references
      for (var j = list.Count - 1; j >= 0; j--)
      {
        if (!list[j].TryGetTarget(out var item))
          list.RemoveAt(j);
        else
          yield return item;
      }

      // Remove empty lists.
      if (list.Count == 0) _index.Remove(hashCode);
    }
  }
  /// <summary>
  /// Removes all weak references from the set.
  /// </summary>
  public void Clear() => _index.Clear();

  /// <summary>
  /// Checks if the set has a weak reference to the item.
  /// </summary>
  /// <param name="item">The item to be checked.</param>
  /// <returns>True if a weak reference to the item was found; otherwise false.</returns>
  public bool Contains(T item)
  {
    var hashCode = item.GetHashCode();
    if (!_index.TryGetValue(hashCode, out var list)) return false;
    for (var i = list.Count - 1; i >= 0; i--)
    {
      // Clean up dead references
      if (!list[i].TryGetTarget(out var target))
        list.RemoveAt(i);
      else if (ReferenceEquals(item, target))
        return true;
    }
    // Remove empty lists.
    if (list.Count==0) _index.Remove(hashCode);
    return false;
  }
  
  
  /// <summary>
  /// Removes an item's weak reference from the set.
  /// </summary>
  /// <param name="item">The item to be added.</param>
  /// <returns>True if the item was included in the set and removed; otherwise false.</returns>
  public bool Remove(T item)
  {
    var hashCode = item.GetHashCode();
    if (!_index.TryGetValue(hashCode, out var list)) return false;
    for (var i = list.Count - 1; i >= 0; i--)
    {
      // Clean up dead references
      if (!list[i].TryGetTarget(out var target)) list.RemoveAt(i);

      if (!ReferenceEquals(item, target)) continue;

      list.RemoveAt(i);

      return true;
    }
    // Remove empty lists.
    if (list.Count==0) _index.Remove(hashCode);
    return false;
  }

  /// <summary>
  /// Adds a weak reference to the item.
  /// </summary>
  /// <param name="item">The item to be added.</param>
  /// <returns>True if the item was not already present in the set; otherwise false.</returns>
  public bool Add(T item)
  {
    var hashCode = item.GetHashCode();
    if (!_index.TryGetValue(hashCode, out var list))
    {
      list = new() { new(item) };
      _index.Add(hashCode, list);
      return true;
    }
    for (var i = list.Count - 1; i >= 0; i--)
    {
      // Clean up dead references
      if (!list[i].TryGetTarget(out var target)) list.RemoveAt(i);

      if (ReferenceEquals(item, target)) return false;
    }
    list.Add(new (item));
    return true;
  }
}
