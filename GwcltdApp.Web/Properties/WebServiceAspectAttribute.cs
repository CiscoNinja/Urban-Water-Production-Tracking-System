using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly)]
[MulticastAttributeUsage(MulticastTargets.Method)]
[Serializable]
public class WebServiceAspectAttribute : OnMethodBoundaryAspect
{
    public override void OnEntry(MethodExecutionArgs args)
    {
        args.MethodExecutionTag = Stopwatch.StartNew();
        dynamic client = args.Instance;
        Console.WriteLine("Found service call to: {0}", client.Client.Endpoint.Address);
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Stopwatch sw = (Stopwatch)args.MethodExecutionTag;
        sw.Stop();

        dynamic client = args.Instance;
        Console.WriteLine("Service call to \"{0}\" took {1} ms.", client.Client.Endpoint.Address, sw.ElapsedMilliseconds);
    }
}