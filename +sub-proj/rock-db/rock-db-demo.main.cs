//css_nuget RocksDB
//css_nuget EasyObject
using RocksDbSharp;
using static Global.EasyObject;
using static Global.OpenSystem;

try
{
    SetupConsoleEncoding();
    UseAnsiConsole = true;
    string dbPath = GitProjectFile(GetCwd(), "test.db")!;
    Log(dbPath, "dbPath");
    var options = new DbOptions()
        .SetCreateIfMissing(true);
    using (var db = RocksDb.Open(options, dbPath))
    {
        // Using strings below, but can also use byte arrays for both keys and values
        db.Put("key", "value");
        string value = db.Get("key");
        db.Remove("key");
    }
}
catch (System.Exception ex) {
    Abort(ex);
}
