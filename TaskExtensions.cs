public static class TaskExtensions
{
  public static async Task<TOut> Then<TIn, TOut>(this Task<TIn> task, Func<TIn, TOut> func)
    => func(await task.ConfigureAwait(false));

  public static async Task Then<T>(this Task<T> task, Action<T> action)
    => action(await task.ConfigureAwait(false));

  public static async Task Then(this Task task, Action action)
  {
    await task.ConfigureAwait(false);
    action();
  }
  public static async Task<T> Then<T>(this Task task, Func<T> func)
  {
    await task.ConfigureAwait(false);
    return func();
  }

  public static async Task<T> Catch<T>(this Task<T> task, Func<Exception, T> fallback)
  {
    try
    {
      return await task.ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      return fallback(ex);
    }
  }
  public static async Task Catch<T>(this Task<T> task, Action<Exception> handler)
  {
    await Catch((Task)task, handler).ConfigureAwait(false);
  }
  public static async Task Catch(this Task task, Action<Exception> handler)
  {
    try
    {
      await task.ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      handler(ex);
    }
  }
  public static async Task<T> Catch<T>(this Task<T> task, Func<T> fallback)
  {
    try
    {
      return await task.ConfigureAwait(false);
    }
    catch
    {
      return fallback();
    }
  }


  public static async Task Finally<TIn>(this Task<TIn> task, Action action)
  {
    try
    {
      await task.ConfigureAwait(false);
    }
    finally
    {
      action();
    }
  }
  public static async Task Finally(this Task task, Action action)
  {
    try
    {
      await task.ConfigureAwait(false);
    }
    finally
    {
      action();
    }
  }
}
