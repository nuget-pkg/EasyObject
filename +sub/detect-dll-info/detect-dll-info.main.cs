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
    Log(IsDotNetAssembly(@"C:\env\+cmd\wingen.exe"));
    //"C:\env\+cmd\wingen.exe"

    Log(GetDllArchitecture(@"C:\home17\cmd\busybox64u.exe"));
    Log(GetDllArchitecture(@"C:\home17\cmd\MyClass1.dll"));
    //"C:\home17\cmd\MyClass1.dll"
}
catch (Exception e)
{
    Abort(e);
}

bool IsDotNetAssembly(string fileName)
{
    using var stream = File.OpenRead(fileName);
    using var peReader = new PEReader(stream);
    // Returns true if the DLL has a CLR header (metadata)
    return peReader.HasMetadata;
}

Machine GetDllArchitecture(string fileName)
{
    using var stream = File.OpenRead(fileName);
    using var peReader = new PEReader(stream);
    return peReader.PEHeaders.CoffHeader.Machine;
    /*
     * Machine.Amd64 indicates 64-bit.
     * Machine.I386 indicates 32-bit (or potentially AnyCPU).
     * Machine.ARM or Machine.ARM64 indicates ARM architecture. 
     * To distinguish x86 from AnyCPU for .NET, you would inspect the CorHeader flags, similar to the CorFlags.exe tool. Native DLLs will correctly report their respective architecture, such as I386 (32-bit) or Amd64 (64-bit).
     */
}
