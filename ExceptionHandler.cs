public static class ExceptionHandler
{
	public static IAsyncExceptionHandler<T> AsyncHandler<T>(Func<T, Task<bool>> handleAction) where T : Exception
	{
		return new AsyncExceptionAction<T>(handleAction);
	}
	public static IExceptionHandler<T> Handler<T>(Func<T, bool> handleAction) where T : Exception
	{
		return new ExceptionAction<T>(handleAction);
	}

	public static T Scope<T>(this T handler, Action action)
		where T : IExceptionHandler
	{
		handler.Scope(action);
		return handler;
	}
	
	public static async Task<T> AsyncScope<T>(this T handler, Func<Task> action)
		where T : IAsyncExceptionHandler
	{
		await handler.ScopeAsync(action);
		return handler;
	}
	

	class ExceptionAction<T> : IExceptionHandler<T> where T : Exception
	{
		private readonly Func<T, bool> _handleAction;

		public ExceptionAction(Func<T, bool> handleAction) => _handleAction = handleAction;

		public bool Handle(T ex) => _handleAction(ex);
	}
	class AsyncExceptionAction<T> : IAsyncExceptionHandler<T> where T : Exception
	{
		private readonly Func<T, Task<bool>> _handleAction;

		public AsyncExceptionAction(Func<T, Task<bool>> handleAction)
		{
			_handleAction= handleAction;
		}

		public async Task<bool> HandleAsync(T ex)
		{
			return await _handleAction(ex);
		}
	}
}

public interface IExceptionHandler
{
    void Scope(Action action)
    {
        try
        {
            action();
        }
        catch(Exception ex)
        {
            if (!Handle(ex)) throw;
        }
    }

    bool Handle(Exception ex);
}

public interface IExceptionHandler<in T> : IExceptionHandler
    where T:Exception
{
    void IExceptionHandler.Scope(Action action)
    {
        try
        {
            action();
        }
        catch(T ex)
        {
            if (!Handle(ex)) throw;
        }
    }
    
    bool Handle(T ex);

    bool IExceptionHandler.Handle(Exception ex)
        => ex is T myTypeOfException && Handle(myTypeOfException);
}


public interface IAsyncExceptionHandler
{
	async Task ScopeAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (Exception ex)
		{
			if (!await HandleAsync(ex)) throw;
		}
	}

	Task<bool> HandleAsync(Exception ex);
}

public interface IAsyncExceptionHandler<in T> : IAsyncExceptionHandler
	where T : Exception
{
	async Task IAsyncExceptionHandler.ScopeAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (T ex)
		{
			if (!await HandleAsync(ex)) throw;
		}
	}
    
	Task<bool> HandleAsync(T ex);

	async Task<bool> IAsyncExceptionHandler.HandleAsync(Exception ex)
		=> ex is T myTypeOfException && await HandleAsync(myTypeOfException);
}
