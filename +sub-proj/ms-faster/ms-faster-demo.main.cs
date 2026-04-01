//css_nuget Microsoft.FASTER.Core
//css_nuget EasyObject
using FASTER.core;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;
using static Global.EasyObject;
using static Global.OpenSystem;

try
{
    SetupConsoleEncoding();
    UseAnsiConsole = true;

#if false
    using var settings = new FasterKVSettings<long, string>("c:/temp");
    using var store = new FasterKV<long, string>(settings);
#else
    using var settings = new FasterKVSettings<long, string>(null);
    using var store = new FasterKV<long, string>(settings);
#endif
    Break(store.EntryCount, title: "store.EntryCount");

    // You can also separately create your storage devices and provide them via FasterKVSettings. If you are using value (blittable) types such as long, int, and structs with value-type members, or special variable-length value types such as our SpanByte type, you only need one log device:
    using var log = Devices.CreateLogDevice("c:/temp/hlog.log");

    //If your key or values are serializable C# objects such as classes and strings, you need to create a separate object log device as well:
    using var objlog = Devices.CreateLogDevice("c:/temp/hlog.obj.log");

    // For pure in-memory operation, you can just use a special new NullDevice() instead.
    using var settings2 = new FasterKVSettings<long, long> { LogDevice = log, ObjectLogDevice = objlog };
    using var store2 = new FasterKV<long, string>(settings);


}
catch (System.Exception ex)
{
    Abort(ex);
}
