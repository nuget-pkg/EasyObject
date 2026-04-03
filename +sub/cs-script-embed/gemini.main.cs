//css_nuget CS-Script
//css_nuget EasyObject
//xxxcss_nuget CS-Script.Evaluator
using CSScripting;
using CSScriptLib;
using System;
using System.Linq;
using static Global.EasyObject;


//CSScript.Evaluator.With(
//    static eval => {
//        eval.IsCachingEnabled = false;
//        eval.ReferenceAssembliesFromCode = true; // コード内の //css_reference を有効にする
//    }
//    );

// 1. NuGetを自動解決するための設定
// これにより //css_nuget が有効になります
//CSScript.EvaluatorConfig.ReferencingAlgorithm = ReferencingAlgorithm.MetaData;

try
{
    // 実行したいコード (//css_nuget を含める)
    string code = @"
        //css_nuget Newtonsoft.Json
        using System;
        using Newtonsoft.Json;

        public class Script
        {
            public void Run()
            {
                var data = new { Message = ""Hello from NuGet!"", Time = DateTime.Now };
                Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
        }";

    //CSScript.Evaluator.With(
    //    static eval =>
    //    {
    //        eval.IsCachingEnabled = false;
    //        eval.CompileMethod = CompileMethod.CompileToMemory; // メモリ上でコンパイル
    //    }
    //    );
    var script = CSScript.Evaluator
        .ReferenceAssemblyByName("System.Runtime") // これがないと Newtonsoft.Json が見つからないことがあります
                     .ReferenceAssembliesFromCode(code);
    CSScript.Evaluator.With(static eval =>
    {
        eval.IsCachingEnabled = false;
        //eval.CompileMethod = CompileMethod.CompileToMemory; // メモリ上でコンパイル

    });
    var assembly = script.CompileMethod(code);
    Log(assembly != null);
    ExpectTrue(assembly != null);
    var classes = assembly!.GetTypes()
                          .Where(t => t.IsClass);

    foreach (var type in classes)
    {
        Console.WriteLine($"Found class: {type.FullName}");
    }
    //if (asm != null)
    //{
    //    //asm.GetAttached<Project>().Refs.ForEach(r => Log($"Referenced: {r}"));
    //    var project = asm.GetAttached<Project>();
    //    Log(project != null, "(project != null)");
    //}
    //script = script.LoadCode<dynamic>(code);
    //                     .LoadCode<dynamic>(code);

    // 2. コンパイル & インスタンス化
    // CompileMethod ではなく、全体をコンパイルして dynamic で受けるのが楽です
    //dynamic script = CSScript.Evaluator
    //                         .LoadCode(code);

    // 3. 実行
    //script.Run();
}
catch (Exception ex)
{
    Abort(ex);
    //Console.WriteLine($"Error: {ex.Message}");
}
