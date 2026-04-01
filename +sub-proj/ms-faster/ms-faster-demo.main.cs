//css_nuget Microsoft.FASTER.Core
//css_nuget EasyObject
using FASTER.core;
using System;
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
}
catch (System.Exception ex) {
    Abort(ex);
}
