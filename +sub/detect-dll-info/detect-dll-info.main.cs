//+#nuget System.Reflection.Metadata;
//css_nuget EasyObject;

using Global;
using System;
using System.IO;
using static Global.EasyObject;
using System.Reflection.PortableExecutable;

SetupConsoleEncoding();

#if CS_SCRIPT
Console.WriteLine("Running as a CS_SCRIPT...");
#endif

try
{
    ShowDetail = false;
    ShowLineNumbers = false;
    Log("ハロー©");
    Log(new { args });
    Log(IsDotNetAssembly(@"C:\home17\cmd\busybox64u.exe"));
    Log(IsDotNetAssembly(@"C:\home17\cmd\MyClass1.dll"));
    //"C:\home17\cmd\MyClass1.dll"
}
catch (Exception e)
{
    Abort(e);
}

/*public static*/ bool IsDotNetAssembly(string fileName)
{
    using var stream = File.OpenRead(fileName);
    using var peReader = new PEReader(stream);
    // Returns true if the DLL has a CLR header (metadata)
    return peReader.HasMetadata;
}
