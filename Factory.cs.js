// Template for C# `Factory.cs` code.

function csFactoryTemplate(length){
  const indices = Array.from({length},(_,k)=>k);
  const $index = indices.map(k=>k+1);
  const genericParameters = $index.map(k=>"T"+k);
  const args = $index.map(k=>"arg"+(k));
  const params = $index.map(k=>`T${k} arg${k}`);

  const $interface = `
public interface IFactoryBuilder
  <${genericParameters.map(gp=>"in "+gp).join(', ')}, out TProduct>
{
  IFactory<TProduct> Create
    (${params});

${$index.map(withInterface).join('\n')}
}`;
  const [mN,...mR] = indices.map(withMethod);
  const $methods = [...mR,mN].join("\n");

  return(
`
public static partial class Factory
{
  public static IFactory<TProduct> Create<${genericParameters}, TProduct>
    (Func<${genericParameters}, TProduct> constructor, ${params})
    => new Factory<TProduct>(() => constructor(${args.join(", ")}));
  public static IFactory<TProduct> ToFactory<${genericParameters}, TProduct>
    (this Func<${genericParameters}, TProduct> constructor, ${params})
    => new Factory<TProduct>(() => constructor(${args.join(", ")}));
  public static IFactoryBuilder<${genericParameters}, TProduct> ToBuilder<${genericParameters}, TProduct>
    (this Func<${genericParameters}, TProduct> constructor) => Build(constructor);
  public static IFactoryBuilder<${genericParameters}, TProduct> Build<${genericParameters}, TProduct>
    (Func<${genericParameters}, TProduct> constructor)
    => new FactoryBuilder<${genericParameters}, TProduct>(constructor);
}

${$interface}

class FactoryBuilder<${genericParameters}, TProduct>
  : IFactoryBuilder<${genericParameters}, TProduct>
{
  private readonly Func<${genericParameters}, TProduct> _constructor;
  public FactoryBuilder(Func<${genericParameters}, TProduct> constructor) { _constructor = constructor; }
  public IFactory<TProduct> Create(${params})
    => _constructor.ToFactory(${args.join(", ")});
${$methods}
}
`);
  function uncat(array, index) {
    return [array.slice(0,index),array.slice(index)];
  }
  function withInterface(i) {
    let [pl,[pa,...pr]] = uncat(genericParameters, i-1);
    let p = [...pl,...pr];
    return(`  IFactoryBuilder<${p}, TProduct> With(${pa} arg${i});`);
  }
  function withMethod(i) {
    let [il,[ia,...ir]] = uncat(indices, i-1);
    let [pl,,pr] = map(genericParameters);
    let $params = [...pl,...pr].join(", ");
    let [al,,ar] = map(args);
    let $args = [...al,...ar].join(", ");
    
    return(`
  public IFactoryBuilder <${$params}, TProduct> With(${params[ia]})
    => new FactoryBuilder<${$params}, TProduct>((${$args}) =>
      _constructor(${args.join(', ')}));`);

    function map(array){
      return [
        il.map(i=>array[i]),
        array[ia],
        ir.map(i=>array[i])
      ];
    }
  }
}
