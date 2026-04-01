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
        db.Put("key", "value⁅記号⁆◉▶▸⸝↪️ ↩️ ℴ𝓬➺➢ᰔ  ヾ➠✅🈂️❓❗＼／：＊“≪≫￤；‘｀＃％＄＆＾～￤﴾﴿⁅⁆【】≪≫＋ー＊＝⚽ 𝑪𝒉𝒆𝒄𝒌 🌐🪩", encoding: System.Text.Encoding.UTF8);
        string value = db.Get("key", encoding: System.Text.Encoding.UTF8);
        Log(value, title: "value");
        db.Remove("key");
    }
}
catch (System.Exception ex) {
    Abort(ex);
}
