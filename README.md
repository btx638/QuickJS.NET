# QuickJS.NET
C# bindings for QuickJS, a JavaScript interpreter written in C by Fabrice Bellard and Charlie Gordon.

**Warning: At present, both the original project `quickjs` and this project are still in the early stage of development. Please use this project carefully in the production environment.**

## Usage
```CSharp
static void Main()
{
  using (QuickJSRuntime runtime = new QuickJSRuntime())
  using (QuickJSContext context = runtime.CreateContext())
  {
    object result = context.Eval("2 + 2", "script.js", JSEvalFlags.Global);
    Console.WriteLine("2 + 2 = {0}", result);
  }
}
```

## Links
* The QuickJS Javascript Engine: https://bellard.org/quickjs/
* Unofficial QuickJS git mirror: https://github.com/horhof/quickjs
* The QuickJS precompiled binaries used for this project: https://github.com/vmas/QuickJS/releases

## License
[MIT](./LICENSE)
