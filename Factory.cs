// Methods for generating/currying factories & builders from Func<...>


namespace NCyplant.Initialization;


public static partial class Factory
{
  public static IFactory<TProduct> Create<TProduct>
    (Func<TProduct> constructor)
    => new Factory<TProduct>(constructor);

  public static IFactory<TProduct> Create<T, TProduct>
    (Func<T, TProduct> constructor, T arg)
    => new Factory<TProduct>(() => constructor(arg));

  public static IFactory<TProduct> Create<T1, T2, TProduct>
    (Func<T1, T2, TProduct> constructor, T1 arg1, T2 arg2)
    => new Factory<TProduct>(() => constructor(arg1, arg2));

  public static IFactory<TProduct> Create<T1, T2, T3, TProduct>
    (Func<T1, T2, T3, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3));

  public static IFactory<TProduct> Create<T1, T2, T3, T4, TProduct>
    (Func<T1, T2, T3, T4, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4));

  public static IFactory<TProduct> Create<T1, T2, T3, T4, T5, TProduct>
    (Func<T1, T2, T3, T4, T5, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5));

  public static IFactory<TProduct> ToFactory<TProduct>
    (this Func<TProduct> constructor)
    => new Factory<TProduct>(constructor);

  public static IFactory<TProduct> ToFactory<T, TProduct>
    (this Func<T, TProduct> constructor, T arg)
    => new Factory<TProduct>(() => constructor(arg));

  public static IFactory<TProduct> ToFactory<T1, T2, TProduct>
    (this Func<T1, T2, TProduct> constructor, T1 arg1, T2 arg2)
    => new Factory<TProduct>(() => constructor(arg1, arg2));

  public static IFactory<TProduct> ToFactory<T1, T2, T3, TProduct>
    (this Func<T1, T2, T3, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3));

  public static IFactory<TProduct> ToFactory<T1, T2, T3, T4, TProduct>
    (this Func<T1, T2, T3, T4, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4));

  public static IFactory<TProduct> ToFactory<T1, T2, T3, T4, T5, TProduct>
    (this Func<T1, T2, T3, T4, T5, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5));
}
  
  
  
public interface IFactory<out TProduct>
{
  TProduct Build();
}

class Factory<TProduct> : IFactory<TProduct>
{
  private readonly Func<TProduct> _constructor;
  protected internal Factory(Func<TProduct> constructor)
    => _constructor = constructor;
  public TProduct Build()
    => _constructor();
}

////// Factory Builder //////


public static partial class Factory
{
  public static IFactoryBuilder<TProduct> ToBuilder<TProduct>(this Func<TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<TProduct> Build<TProduct>
    (Func<TProduct> constructor)
    => new FactoryBuilder<TProduct>(constructor);
  
}

public interface IFactoryBuilder
  <out TProduct>
{
  IFactory<TProduct> Create
    ();
}

class FactoryBuilder<TProduct>
  : IFactoryBuilder<TProduct>
{
  private readonly Func<TProduct> _constructor;
  public FactoryBuilder(Func<TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create() => _constructor.ToFactory();
}

public static partial class Factory
{
  public static IFactoryBuilder<T, TProduct> ToBuilder<T, TProduct>(this Func<T, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T, TProduct> Build<T, TProduct>
    (Func<T, TProduct> constructor)
    => new FactoryBuilder<T, TProduct>(constructor);
}

public interface IFactoryBuilder
  <in T, out TProduct>
{
  IFactory<TProduct> Create
    (T arg);
  IFactoryBuilder<TProduct> With(T arg);
}

class FactoryBuilder<T, TProduct>
  : IFactoryBuilder<T, TProduct>
{
  private readonly Func<T, TProduct> _constructor;
  public FactoryBuilder(Func<T, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T arg) => _constructor.ToFactory(arg);

  public IFactoryBuilder<TProduct> With(T arg)
    => new FactoryBuilder<TProduct>(() =>
      _constructor(arg));
}
  
public static partial class Factory
{
  public static IFactoryBuilder<T1, T2, TProduct> ToBuilder<T1, T2, TProduct>(this Func<T1, T2, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1, T2, TProduct> Build<T1, T2, TProduct>
    (Func<T1, T2, TProduct> constructor)
    => new FactoryBuilder<T1, T2, TProduct>(constructor);
  
}

public interface IFactoryBuilder
  <in T1, in T2, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1, T2 arg2);

  IFactoryBuilder<T1, TProduct> With(T2 arg);
  IFactoryBuilder<T2, TProduct> With(T1 arg);
}

class FactoryBuilder<T1, T2, TProduct>
  : IFactoryBuilder<T1, T2, TProduct>
{
  private readonly Func<T1, T2, TProduct> _constructor;
  public FactoryBuilder(Func<T1, T2, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1, T2 arg2) => _constructor.ToFactory(arg1, arg2);

  public IFactoryBuilder<T1, TProduct> With(T2 arg)
    => new FactoryBuilder<T1, TProduct>((arg1) =>
      _constructor(arg1, arg));
  public IFactoryBuilder<T2, TProduct> With(T1 arg)
    => new FactoryBuilder<T2, TProduct>((arg1) =>
      _constructor(arg, arg1));
}
  
public static partial class Factory
{
  public static IFactoryBuilder<T1, T2, T3, TProduct> ToBuilder<T1, T2, T3, TProduct>(this Func<T1, T2, T3, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1, T2, T3, TProduct> Build<T1, T2, T3, TProduct>
    (Func<T1, T2, T3, TProduct> constructor)
    => new FactoryBuilder<T1, T2, T3, TProduct>(constructor);
}

public interface IFactoryBuilder
  <in T1, in T2, in T3, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1, T2 arg2, T3 arg3);

  IFactoryBuilder<T1, T2, TProduct> With(T3 arg);
  IFactoryBuilder<T1, T3, TProduct> With(T2 arg);
  IFactoryBuilder<T2, T3, TProduct> With(T1 arg);
}

class FactoryBuilder<T1, T2, T3, TProduct>
  : IFactoryBuilder<T1, T2, T3, TProduct>
{
  private readonly Func<T1, T2, T3, TProduct> _constructor;
  public FactoryBuilder(Func<T1, T2, T3, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1, T2 arg2, T3 arg3) => _constructor.ToFactory(arg1, arg2, arg3);

  public IFactoryBuilder<T1, T2, TProduct> With(T3 arg)
    => new FactoryBuilder<T1, T2, TProduct>((arg1, arg2) =>
      _constructor(arg1, arg2, arg));
  public IFactoryBuilder<T1, T3, TProduct> With(T2 arg)
    => new FactoryBuilder<T1, T3, TProduct>((arg1, arg2) =>
      _constructor(arg1, arg, arg2));
  public IFactoryBuilder<T2, T3, TProduct> With(T1 arg)
    => new FactoryBuilder<T2, T3, TProduct>((arg1, arg2) =>
      _constructor(arg, arg1, arg2));
}

public interface IFactoryBuilder
  <in T1, in T2, in T3, in T4, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1, T2 arg2, T3 arg3, T4 arg4);

  IFactoryBuilder<T1, T2, T3, TProduct> With(T4 arg);
  IFactoryBuilder<T1, T2, T4, TProduct> With(T3 arg);
  IFactoryBuilder<T1, T3, T4, TProduct> With(T2 arg);
  IFactoryBuilder<T2, T3, T4, TProduct> With(T1 arg);
}

public static partial class Factory
{
  public static IFactoryBuilder<T1, T2, T3, T4, TProduct> ToBuilder<T1, T2, T3, T4, TProduct>(this Func<T1, T2, T3, T4, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1, T2, T3, T4, TProduct> Build<T1, T2, T3, T4, TProduct>
    (Func<T1, T2, T3, T4, TProduct> constructor)
    => new FactoryBuilder<T1, T2, T3, T4, TProduct>(constructor);
}

class FactoryBuilder<T1, T2, T3, T4, TProduct>
  : IFactoryBuilder<T1, T2, T3, T4, TProduct>
{
  private readonly Func<T1, T2, T3, T4, TProduct> _constructor;
  public FactoryBuilder(Func<T1, T2, T3, T4, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _constructor.ToFactory(arg1, arg2, arg3, arg4);

  public IFactoryBuilder<T1, T2, T3, TProduct> With(T4 arg)
    => new FactoryBuilder<T1, T2, T3, TProduct>((arg1, arg2, arg3) =>
      _constructor(arg1, arg2, arg3, arg));
  public IFactoryBuilder<T1, T2, T4, TProduct> With(T3 arg)
    => new FactoryBuilder<T1, T2, T4, TProduct>((arg1, arg2, arg3) =>
      _constructor(arg1, arg2, arg, arg3));
  public IFactoryBuilder<T1, T3, T4, TProduct> With(T2 arg)
    => new FactoryBuilder<T1, T3, T4, TProduct>((arg1, arg2, arg3) =>
      _constructor(arg1, arg, arg2, arg3));
  public IFactoryBuilder<T2, T3, T4, TProduct> With(T1 arg)
    => new FactoryBuilder<T2, T3, T4, TProduct>((arg1, arg2, arg3) =>
      _constructor(arg, arg1, arg2, arg3));
}

public static partial class Factory
{
  public static IFactoryBuilder<T1, T2, T3, T4, T5, TProduct> ToBuilder<T1, T2, T3, T4, T5, TProduct>(this Func<T1, T2, T3, T4, T5, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1, T2, T3, T4, T5, TProduct> Build<T1, T2, T3, T4, T5, TProduct>
    (Func<T1, T2, T3, T4, T5, TProduct> constructor)
    => new FactoryBuilder<T1, T2, T3, T4, T5, TProduct>(constructor);
}

public interface IFactoryBuilder
  <in T1, in T2, in T3, in T4, in T5, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

  IFactoryBuilder<T1, T2, T3, T4, TProduct> With(T5 arg);
  IFactoryBuilder<T1, T2, T3, T5, TProduct> With(T4 arg);
  IFactoryBuilder<T1, T2, T4, T5, TProduct> With(T3 arg);
  IFactoryBuilder<T1, T3, T4, T5, TProduct> With(T2 arg);
  IFactoryBuilder<T2, T3, T4, T5, TProduct> With(T1 arg);
}

class FactoryBuilder<T1, T2, T3, T4, T5, TProduct>
  : IFactoryBuilder<T1, T2, T3, T4, T5, TProduct>
{
  private readonly Func<T1, T2, T3, T4, T5, TProduct> _constructor;
  public FactoryBuilder(Func<T1, T2, T3, T4, T5, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _constructor.ToFactory(arg1, arg2, arg3, arg4, arg5);

  public IFactoryBuilder<T1, T2, T3, T4, TProduct> With(T5 arg)
    => new FactoryBuilder<T1, T2, T3, T4, TProduct>((arg1, arg2, arg3, arg4) =>
      _constructor(arg1, arg2, arg3, arg4, arg));
  public IFactoryBuilder<T1, T2, T3, T5, TProduct> With(T4 arg)
    => new FactoryBuilder<T1, T2, T3, T5, TProduct>((arg1, arg2, arg3, arg4) =>
      _constructor(arg1, arg2, arg3, arg, arg4));
  public IFactoryBuilder<T1, T2, T4, T5, TProduct> With(T3 arg)
    => new FactoryBuilder<T1, T2, T4, T5, TProduct>((arg1, arg2, arg3, arg4) =>
      _constructor(arg1, arg2, arg, arg3, arg4));
  public IFactoryBuilder<T1, T3, T4, T5, TProduct> With(T2 arg)
    => new FactoryBuilder<T1, T3, T4, T5, TProduct>((arg1, arg2, arg3, arg4) =>
      _constructor(arg1, arg, arg2, arg3, arg4));
  public IFactoryBuilder<T2, T3, T4, T5, TProduct> With(T1 arg)
    => new FactoryBuilder<T2, T3, T4, T5, TProduct>((arg1, arg2, arg3, arg4) =>
      _constructor(arg, arg1, arg2, arg3, arg4));
}


public static partial class Factory
{
  public static IFactory<TProduct> Create<T1, T2, T3, T4, T5, T6, TProduct>
    (Func<T1, T2, T3, T4, T5, T6, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6));
  public static IFactory<TProduct> ToFactory<T1, T2, T3, T4, T5, T6, TProduct>
    (this Func<T1, T2, T3, T4, T5, T6, TProduct> constructor, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6));
  public static IFactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct> ToBuilder<T1, T2, T3, T4, T5, T6, TProduct>
    (this Func<T1, T2, T3, T4, T5, T6, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct> Build<T1, T2, T3, T4, T5, T6, TProduct>
    (Func<T1, T2, T3, T4, T5, T6, TProduct> constructor)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct>(constructor);
}

public interface IFactoryBuilder
  <in T1, in T2, in T3, in T4, in T5, in T6, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

  IFactoryBuilder<T1, T2, T3, T4, T5, TProduct> With(T6 arg);
  IFactoryBuilder<T1, T2, T3, T4, T6, TProduct> With(T5 arg);
  IFactoryBuilder<T1, T2, T3, T5, T6, TProduct> With(T4 arg);
  IFactoryBuilder<T1, T2, T4, T5, T6, TProduct> With(T3 arg);
  IFactoryBuilder<T1, T3, T4, T5, T6, TProduct> With(T2 arg);
  IFactoryBuilder<T2, T3, T4, T5, T6, TProduct> With(T1 arg);
}

class FactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct>
  : IFactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct>
{
  private readonly Func<T1, T2, T3, T4, T5, T6, TProduct> _constructor;
  public FactoryBuilder(Func<T1, T2, T3, T4, T5, T6, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _constructor.ToFactory(arg1, arg2, arg3, arg4, arg5, arg6);

  public IFactoryBuilder<T1, T2, T3, T4, T5, TProduct> With(T6 arg)
    => new FactoryBuilder<T1, T2, T3, T4, T5, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg));
  public IFactoryBuilder<T1, T2, T3, T4, T6, TProduct> With(T5 arg)
    => new FactoryBuilder<T1, T2, T3, T4, T6, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg1, arg2, arg3, arg4, arg, arg5));
  public IFactoryBuilder<T1, T2, T3, T5, T6, TProduct> With(T4 arg)
    => new FactoryBuilder<T1, T2, T3, T5, T6, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg1, arg2, arg3, arg, arg4, arg5));
  public IFactoryBuilder<T1, T2, T4, T5, T6, TProduct> With(T3 arg)
    => new FactoryBuilder<T1, T2, T4, T5, T6, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg1, arg2, arg, arg3, arg4, arg5));
  public IFactoryBuilder<T1, T3, T4, T5, T6, TProduct> With(T2 arg)
    => new FactoryBuilder<T1, T3, T4, T5, T6, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg1, arg, arg2, arg3, arg4, arg5));
  public IFactoryBuilder<T2, T3, T4, T5, T6, TProduct> With(T1 arg)
    => new FactoryBuilder<T2, T3, T4, T5, T6, TProduct>((arg1, arg2, arg3, arg4, arg5) =>
      _constructor(arg, arg1, arg2, arg3, arg4, arg5));
}



public static partial class Factory
{
  public static IFactory<TProduct> Create<T1,T2,T3,T4,T5,T6,T7, TProduct>
    (Func<T1,T2,T3,T4,T5,T6,T7, TProduct> constructor, T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
  public static IFactory<TProduct> ToFactory<T1,T2,T3,T4,T5,T6,T7, TProduct>
    (this Func<T1,T2,T3,T4,T5,T6,T7, TProduct> constructor, T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
  public static IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct> ToBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct>
    (this Func<T1,T2,T3,T4,T5,T6,T7, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct> Build<T1,T2,T3,T4,T5,T6,T7, TProduct>
    (Func<T1,T2,T3,T4,T5,T6,T7, TProduct> constructor)
    => new FactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct>(constructor);
}


public interface IFactoryBuilder
  <in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7);

  IFactoryBuilder<T2,T3,T4,T5,T6,T7, TProduct> With(T1 arg1);
  IFactoryBuilder<T1,T3,T4,T5,T6,T7, TProduct> With(T2 arg2);
  IFactoryBuilder<T1,T2,T4,T5,T6,T7, TProduct> With(T3 arg3);
  IFactoryBuilder<T1,T2,T3,T5,T6,T7, TProduct> With(T4 arg4);
  IFactoryBuilder<T1,T2,T3,T4,T6,T7, TProduct> With(T5 arg5);
  IFactoryBuilder<T1,T2,T3,T4,T5,T7, TProduct> With(T6 arg6);
  IFactoryBuilder<T1,T2,T3,T4,T5,T6, TProduct> With(T7 arg7);
}

class FactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct>
  : IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct>
{
  private readonly Func<T1,T2,T3,T4,T5,T6,T7, TProduct> _constructor;
  public FactoryBuilder(Func<T1,T2,T3,T4,T5,T6,T7, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7)
    => _constructor.ToFactory(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

  public IFactoryBuilder <T2, T3, T4, T5, T6, T7, TProduct> With(T1 arg1)
    => new FactoryBuilder<T2, T3, T4, T5, T6, T7, TProduct>((arg2, arg3, arg4, arg5, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T3, T4, T5, T6, T7, TProduct> With(T2 arg2)
    => new FactoryBuilder<T1, T3, T4, T5, T6, T7, TProduct>((arg1, arg3, arg4, arg5, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T2, T4, T5, T6, T7, TProduct> With(T3 arg3)
    => new FactoryBuilder<T1, T2, T4, T5, T6, T7, TProduct>((arg1, arg2, arg4, arg5, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T2, T3, T5, T6, T7, TProduct> With(T4 arg4)
    => new FactoryBuilder<T1, T2, T3, T5, T6, T7, TProduct>((arg1, arg2, arg3, arg5, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T2, T3, T4, T6, T7, TProduct> With(T5 arg5)
    => new FactoryBuilder<T1, T2, T3, T4, T6, T7, TProduct>((arg1, arg2, arg3, arg4, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T2, T3, T4, T5, T7, TProduct> With(T6 arg6)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T7, TProduct>((arg1, arg2, arg3, arg4, arg5, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));

  public IFactoryBuilder <T1, T2, T3, T4, T5, T6, TProduct> With(T7 arg7)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T6, TProduct>((arg1, arg2, arg3, arg4, arg5, arg6) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
}


public static partial class Factory
{
  public static IFactory<TProduct> Create<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
    (Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> constructor, T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7,T8 arg8)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
  public static IFactory<TProduct> ToFactory<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
    (this Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> constructor, T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7,T8 arg8)
    => new Factory<TProduct>(() => constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
  public static IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> ToBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
    (this Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> Build<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
    (Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> constructor)
    => new FactoryBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>(constructor);
}


public interface IFactoryBuilder
  <in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TProduct>
{
  IFactory<TProduct> Create
    (T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7,T8 arg8);

  IFactoryBuilder<T2,T3,T4,T5,T6,T7,T8, TProduct> With(T1 arg1);
  IFactoryBuilder<T1,T3,T4,T5,T6,T7,T8, TProduct> With(T2 arg2);
  IFactoryBuilder<T1,T2,T4,T5,T6,T7,T8, TProduct> With(T3 arg3);
  IFactoryBuilder<T1,T2,T3,T5,T6,T7,T8, TProduct> With(T4 arg4);
  IFactoryBuilder<T1,T2,T3,T4,T6,T7,T8, TProduct> With(T5 arg5);
  IFactoryBuilder<T1,T2,T3,T4,T5,T7,T8, TProduct> With(T6 arg6);
  IFactoryBuilder<T1,T2,T3,T4,T5,T6,T8, TProduct> With(T7 arg7);
  IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7, TProduct> With(T8 arg8);
}

class FactoryBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
  : IFactoryBuilder<T1,T2,T3,T4,T5,T6,T7,T8, TProduct>
{
  private readonly Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> _constructor;
  public FactoryBuilder(Func<T1,T2,T3,T4,T5,T6,T7,T8, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(T1 arg1,T2 arg2,T3 arg3,T4 arg4,T5 arg5,T6 arg6,T7 arg7,T8 arg8)
    => _constructor.ToFactory(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

  public IFactoryBuilder <T2, T3, T4, T5, T6, T7, T8, TProduct> With(T1 arg1)
    => new FactoryBuilder<T2, T3, T4, T5, T6, T7, T8, TProduct>((arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T3, T4, T5, T6, T7, T8, TProduct> With(T2 arg2)
    => new FactoryBuilder<T1, T3, T4, T5, T6, T7, T8, TProduct>((arg1, arg3, arg4, arg5, arg6, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T4, T5, T6, T7, T8, TProduct> With(T3 arg3)
    => new FactoryBuilder<T1, T2, T4, T5, T6, T7, T8, TProduct>((arg1, arg2, arg4, arg5, arg6, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T3, T5, T6, T7, T8, TProduct> With(T4 arg4)
    => new FactoryBuilder<T1, T2, T3, T5, T6, T7, T8, TProduct>((arg1, arg2, arg3, arg5, arg6, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T3, T4, T6, T7, T8, TProduct> With(T5 arg5)
    => new FactoryBuilder<T1, T2, T3, T4, T6, T7, T8, TProduct>((arg1, arg2, arg3, arg4, arg6, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T3, T4, T5, T7, T8, TProduct> With(T6 arg6)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T7, T8, TProduct>((arg1, arg2, arg3, arg4, arg5, arg7, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T3, T4, T5, T6, T8, TProduct> With(T7 arg7)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T6, T8, TProduct>((arg1, arg2, arg3, arg4, arg5, arg6, arg8) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));

  public IFactoryBuilder <T1, T2, T3, T4, T5, T6, T7, TProduct> With(T8 arg8)
    => new FactoryBuilder<T1, T2, T3, T4, T5, T6, T7, TProduct>((arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
      _constructor(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
}

// Additional arg lengths available via `Factory.cs.js`
