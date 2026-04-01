//css_nuget EasyObject;

using Global;
using System;
using System.IO;
using System.Text;
using static Global.EasyObject;

using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Reflection.PortableExecutable;

SetupConsoleEncoding();

#if CS_SCRIPT
Console.WriteLine("Running as a CS_SCRIPT...");
#endif

try
{
    ShowDetail = true;
    Log("ハロー©");
    Log(new { args });
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
