using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

[AttributeUsage(AttributeTargets.Assembly)]
[MulticastAttributeUsage(MulticastTargets.Method)]
[Serializable]
public class DatabaseAspectAttribute : OnMethodBoundaryAspect
{
    public override void OnEntry(MethodExecutionArgs args)
    {
        args.MethodExecutionTag = Stopwatch.StartNew();

        SqlCommand cmd = (SqlCommand)args.Instance;

        Console.WriteLine("Executing command: {0}", cmd.CommandText);
        Console.WriteLine("\t- Connection String: {0}", cmd.Connection.ConnectionString);

        List<string> parameters = new List<string>();

        for (int i = 0; i < cmd.Parameters.Count; i++)
            parameters.Add("\t- Parameter " + cmd.Parameters[i].ParameterName + ": " + cmd.Parameters[i].Value);

        string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);       
        string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Emails From POP3 Server.TXT";
        using (StreamWriter sw = File.CreateText(fileName))
        {
            if (parameters.Count > 0)
            {
                sw.WriteLine("Executing command: {0}", cmd.CommandText);
                sw.WriteLine("\t- Connection String: {0}", cmd.Connection.ConnectionString);
                sw.WriteLine(String.Join("\n", parameters));
                sw.WriteLine();
            }
            sw.Close();
        }
    }

    public override void OnExit(MethodExecutionArgs args)
    {
        Stopwatch sw = (Stopwatch)args.MethodExecutionTag;
        sw.Stop();

        SqlCommand cmd = (SqlCommand)args.Instance;

        Console.WriteLine("Command \"{0}\" took {1} ms.", cmd.CommandText, sw.ElapsedMilliseconds);
    }
}