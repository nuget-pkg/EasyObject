using Global;
using NUnit.Framework;
using Razorvine.Pickle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static Global.EasyObject;

// ReSharper disable once CheckNamespace
namespace Demo;

class Exchangeable1 : IExportToPlainObject {
    public object ExportToPlainObject() {
        return 123;
    }
}

class Exchangeable2 {
    public object ExportToPlainObject() {
        return 456;
    }
}

class Exchangeable3 : IExportToCommonJson {
    public string ExportToCommonJson() {
        return "[11, 22, 33]";
    }
}

class Exchangeable4 {
    public string ExportToCommonJson() {
        return "[111, 222, 333]";
    }
}

class MyData : EasyObject {
    public MyData(int n, string s) {
        ImportFromPlainObject(new { n, s });
    }
    public MyData(string json) {
        ImportFromCommonJson(json);
    }
    public int N {
        get {
            return Dynamic.n;
        }
    }
    public string S {
        get {
            return Dynamic.s;
        }
    }
}

class Program {
    static void Main(string[] args) {
        try {
            SetupConsoleEncoding();
            ShowDetail = true;
            //AllocConsole();
            Console.WriteLine("(1)");
            EasyObject eoNull = Null;
            Log(eoNull.ToJson());
            //var poc = new PlainObjectConverter();
            //Log(poc.Stringify(eoNull, indent: true));
            Log(eoNull.ToPrintable());
            Log(eoNull);
            Log(Null);
            Console.WriteLine("(2)");
            var eo = EasyObject.FromObject(new { a = 123 });
            Log(eo);
            Console.WriteLine("(3)");
            Log(eo.TypeValue, "eo.TypeValue");
            Console.WriteLine("(4)");
            Assert.That(eo.TypeValue, Is.EqualTo(@object));
            Console.WriteLine("(5)");
            EasyObject a = eo["a"];
            Console.WriteLine("(5.1)");
            Log(FullName(a));
            Log(a.GetType() == typeof(double));
            Console.WriteLine("(5.1.1)");
            //Log(a.ToObject());
            Console.WriteLine("(5.1.1.1)");
            //Log(poc.Stringify(a, true));
            Console.WriteLine("(5.1.2)");
            Log(a, "a");
            Console.WriteLine("(5.2)");
            Console.WriteLine(eo["a"]);
            Console.WriteLine("(6)");
            Assert.That(eo["a"].Cast<int>(), Is.EqualTo(123));
            Console.WriteLine("(7)");
            Assert.That(eo.Keys, Is.EqualTo(new List<string> { "a" }));
            Log(eo[0], "eo[0]");
            Assert.That(eo[0].TypeValue, Is.EqualTo(@null));
            Assert.That(eo[0].IsNull, Is.True);
            Log(eo[1], "eo[1]");
            Assert.That(eo[1].TypeValue, Is.EqualTo(@null));
            Assert.That(eo[1].IsNull, Is.True);
            foreach (var pair in eo.Dynamic) {
                Log(pair, "pair");
            }
            eo = EasyObject.FromObject(null);
            Log(eo.TypeValue, "eo.TypeValue");
            Assert.That(eo.TypeValue, Is.EqualTo(@null));
            eo["b"] = true;
            Assert.That(eo["b"].Cast<bool>(), Is.EqualTo(true));
            Log(eo["b"].TypeValue, "eo.b.TypeValue");
            Assert.That(eo["b"].TypeValue, Is.EqualTo(@boolean));
            eo[3] = 777;
            Log(eo[3].Cast<int>());
            Log(eo.TypeValue, "eo.TypeValue");
            Assert.That(eo.TypeValue, Is.EqualTo(EasyObject.array));
            Assert.That(eo.Count, Is.EqualTo(4));
            Assert.That(eo[0].TypeValue, Is.EqualTo(@null));
#if false
        Assert.That(() => { var n = eo[0].Cast<int>(); },
            Throws.TypeOf<System.InvalidCastException>()
            .With.Message.EqualTo("Null オブジェクトを値型に変換することはできません。")
            );
#endif
            //Assert.That((int?)eo[0], Is.EqualTo(null));
            Assert.That(eo[3].Cast<int>(), Is.EqualTo(777));
            foreach (var e in eo.Dynamic) {
                Log(e, "e");
            }
            var eo2 = EasyObject.FromObject(eo); // UnWrap() test
            EasyObject eo3 = EasyObject.FromJson("""
            { a: 123, b: [11, 22, 33] }
            """);
            Log(eo3, "eo3");
            Log(eo3["b"][1]);
            List<int> list = new List<int>();
            foreach (var e in eo3["b"].Dynamic) {
                list.Add((int)e);
            }

            Log(list, "list");
            Log(eo3["b"].TypeName);
            Log(eo3["b"].IsArray);
            Log(eo3["b"].IsNull);
            eo3["b"].Add(777);
            eo3.AsDictionary!["b"].Add(888);
            Log(eo3);
            var i = FromObject(123);
            Log(i.Cast<double>());
            var iList1 = eo3["b"].AsList!.Select(x => x.Cast<int>()).ToList();
            Log(iList1.GetType().FullName);
            Log(iList1.GetType().ToString());
            var dict = eo3.AsDictionary;
            Log(dict);
            Log(dict["a"].Cast<double>());
            foreach (var e in i.Dynamic) {
                Log(e);
            }
            string bigJson = File.ReadAllText("assets/qiita-9ea0c8fd43b61b01a8da.json");
            //Log(bigJson);
            var sw = new System.Diagnostics.Stopwatch();
            TimeSpan ts;
            sw.Start();
            for (int c = 0; c < 5; c++) {
                //var test = FromJson(bigJson);
            }
            sw.Stop();
            Console.WriteLine("■EasyObject");
            ts = sw.Elapsed;
            Console.WriteLine($"　{ts}");
            Console.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            Console.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
            sw.Start();
            for (int c = 0; c < 5; c++) {
                //JObject jsonObject = JObject.Parse(bigJson);
            }
            sw.Stop();
            Console.WriteLine("■Newtonsoft.Json");
            ts = sw.Elapsed;
            Console.WriteLine($"　{ts}");
            Console.WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            Console.WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
            //var list01_txt = File.ReadAllText("assets/list01.txt");
            var list01_txt = File.ReadAllText("assets/mydict.txt");
            Log(list01_txt);
            var list01_bytes = Convert.FromBase64String(list01_txt);
            var unpickler = new Unpickler();
            object result = unpickler.loads(list01_bytes);
            Log(result, "result");
            //var o = new PlainObjectConverter(forceAscii: false).Parse(result);
            //Log(o, "o");
            var pickler = new Pickler();
            //var bytes = pickler.dumps(o);
            //var ox = unpickler.loads(bytes);
            //Log(ox, "ox");
            //var eo_ox = EasyObject.FromObject(ox);
            //Log(eo_ox, "eo_ox");
            //Log(eo_ox.ToJson(true, true), "eo_ox.ToJson(true, true)");
            Log(DateTime.Now);

            string progJson = """
            #! /usr/bin/env program
            [11, null, "abc"]
            """;
            Log(EasyObject.FromJson(progJson));
            Log(EasyObject.FromJson(null));
            var array = EasyObject.NewArray(1, null, "abc", EasyObject.FromJson(progJson));
            Log(array, "array");
            var obj = EasyObject.NewObject("a", 111, "b", EasyObject.FromJson(progJson));
            Log(obj, "obj");
            // Test newLisp expression
            EasyObject assocList = EasyObject.FromJson("""
            ( ("a" 123) ("b" true) ("c" false) ("d" nil) )
            """);
            Log(assocList, "assocList");
            var member = assocList["a"];
            Log(member, "member");
            dynamic assocDyn = assocList;
            var member2 = assocDyn["a"];
            Log(member2, "member2");
            var member3 = assocDyn.a;
            Log(member3, "member3");
            var exc1 = new Exchangeable1();
            Log(exc1, "exc1");
            var exc2 = new Exchangeable2();
            Log(exc2, "exc2");
            var exc3 = new Exchangeable3();
            Log(exc3, "exc3");
            var exc4 = new Exchangeable4();
            Log(exc4, "exc4");
            var myData = new MyData(123, "xyz");
            Log(myData.N, "myData.N");
            Log(myData.S, "myData.S");
            var myData2 = new MyData("""{n: 456, s: "ABC"}""");
            Log(myData2 == null);
            Log(myData2!.RealData == null);
            Log(myData2.N, "myData2.N");
            Log(myData2.S, "myData2.S");
            Log(myData.ExportToCommonJson(), "myData.ExportToCommonJson()");
            Log(myData2.ExportToCommonJson(), "myData2.ExportToCommonJson()");

            string[] myArgs = ["apple", "melon", "peach"];
            var eoArgs = FromObject(myArgs);
            var first = eoArgs.Shift();
            Log(new { first, eoArgs });
            //myArgs = Array.ConvertAll(eoArgs.ToObject().ToArray() as object[], obj => obj?.ToString() ?? "");
            myArgs = eoArgs.AsStringArray;
            Log(new { myArgs });

            EasyObject ast;
            ast = FromJson(BabelOutput.AstJson);
            ast.Trim(hideKeys: ["loc", "start", "end"], maxDepth: 2);
            Log(ast, "ast(1)");

            ast = FromJson(BabelOutput.AstJson);
            ast.Trim(hideKeys: ["loc", "start", "end"], maxDepth: 3);
            Log(ast, "ast(2)");

            var noError = FromJson("\n", ignoreErrors: true);
            Log(noError, "noError");

            ast = FromJson(BabelOutput.AstJson);
            var xo = ast.ExportToDynamicObject();
            Log(xo, "xo");

            //Log(ast.ToJson(indent: true));
            Log(new { abc = 123, xyz = "abc" });
            Log(FullName(new { abc = 123, xyz = "abc" }));

            string trimmedJson = """
            {
              "x a": [
                1,
                2,
                3,
                [
                  "a",
                  "b",
                  "c",
                  [ 11, 22, 33]
                ]
              ],
              y: {
                a: 1111, b: 2222
              },
              _z123ABC: {
                a: 1111, b: 2222
              }
                        }
            """;
            EasyObject trimTest;

            trimTest = FromJson(trimmedJson);
            Log(trimTest, maxDepth: 1);
            trimTest.Trim(maxDepth: 1);
            Log(trimTest, "(1)");

            trimTest = FromJson(trimmedJson);
            Log(trimTest, maxDepth: 2);
            trimTest.Trim(maxDepth: 2);
            Log(trimTest, "(2)");

            trimTest = FromJson(trimmedJson);
            Log(trimTest, hideKeys: ["a"]);
            trimTest.Trim(hideKeys: ["a"]);
            Log(trimTest, "(3)");

            Log(trimTest.ToJson(keyAsSymbol: true));

            var parser = new EasyLanguageParser(numberAsDecimal: true, removeSurrogatePair: true);
            var result1 = parser.ParseJson("'🔥引火★★帝国🔥'");
            Log(result1, "result1");
            var parser2 = new EasyLanguageParser(numberAsDecimal: true, removeSurrogatePair: false);
            var result2 = parser2.ParseJson("'🔥引火★★帝国🔥'");
            Log(result2, "result2");

            var containSurrogate = FromObject(new { title = "🔥引火★★帝国🔥" });

            Log(containSurrogate);
            Log(containSurrogate, removeSurrogatePair: true);
            Log(containSurrogate, removeSurrogatePair: false);

            Log(containSurrogate);
            Log(containSurrogate, removeSurrogatePair: true);
            Log(containSurrogate, removeSurrogatePair: false);

            var jsonNoSurrogete = containSurrogate.ToJson(indent: true, removeSurrogatePair: true);
            Log(jsonNoSurrogete, "jsonNoSurrogete");
            var jsonWithSurrogete = containSurrogate.ToJson(indent: true, removeSurrogatePair: false);
            Log(jsonWithSurrogete, "jsonWithSurrogete");

            ShowDetail = false;

            var myArray = FromJson("[11, null, 'abc', { x:123, y:777 }]");
            Log(myArray, @"myArray (ORIGINAL)");
            Log(myArray.Shuffle(), @"myArray.Shuffle()");
            Log(myArray.Skip(1), @"myArray.Skip(1)");
            Log(myArray.Take(2), @"myArray.Take(2)");
            Log(myArray.AsStringArray, @"myArray.AsStringArray");
            Log(myArray.AsStringList, @"myArray.AsStringList");

            var myDictionary = FromJson("{a:11, b:null, c:'abc', d:{ x:123, y:777 }}");
            Log(myDictionary.Shuffle(), @"myDictionary.Shuffle()");
            Log(myDictionary.Skip(2), @"myDictionary.Skip(2)");
            Log(myDictionary.Take(2), @"myDictionary.Take(2)");
            Log(myDictionary.AsStringArray, @"myDictionary.AsStringArray");
            Log(myDictionary.AsStringList, @"myDictionary.AsStringList");

            Log(myArray.Reverse(), @"myArray.Reverse()");
            Log(myDictionary.Reverse(), @"myDictionary.Reverse()");

            string json = Utf8StringFromUrl("https://jsonplaceholder.typicode.com/todos/1");
            Log(json, "json");
            var todo = FromJson(json);
            Log(todo, "todo");

            var todo2 = FromUrl("https://jsonplaceholder.typicode.com/todos/1");
            Log(todo2, "todo2");

            //string embeddedJsonUrl = "https://raw.githubusercontent.com/nuget-pkg/Global.Sys/refs/tags/2026.0311.1056.12/Global.Sys.Demo/assets/text-embed-text-02.json";
            string embeddedJsonUrl = "https://github.com/nuget-pkg/Global.Sys/blob/2026.0311.1056.12/Global.Sys.Demo/assets/text-embed-text-02.json";
            var embeddedEO = FromUrl(embeddedJsonUrl);
            Log(embeddedEO, "embeddedEO(github)");

            embeddedJsonUrl = "https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0311.1156.53/text-embed-text-02.json?ref_type=tags";
            embeddedEO = FromUrl(embeddedJsonUrl);
            Log(embeddedEO, "embeddedEO(gitlab)");

            var embedded1 = ExtractFromFile("https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0311.1339.52/json-with-embedded-json.json?ref_type=tags");
            Log(embedded1, "embedded1(gitlab)");

            var embedded2 = ExtractFromFile("https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0311.1351.11/my-ls.exe?ref_type=tags");
            Log(embedded2, "embedded2(gitlab)");

            Console.WriteLine("[stdout] This is unicode: ⭕️ ☢ ☃☃☃ ☮");
            Console.Error.WriteLine("[stderr] This is unicode: ⭕️ ☢ ☃☃☃ ☮");

            Log("This is unicode(echo): ⭕️ ☢ ☃☃☃ ☮");
            Log("This is unicode(log): ⭕️ ☢ ☃☃☃ ☮");
            DebugOutput = false;
            Debug("This is unicode(debug1): ⭕️ ☢ ☃☃☃ ☮"); // now shown because DegutOutput is false here
            DebugOutput = true;
            Debug("This is unicode(debug2): ⭕️ ☢ ☃☃☃ ☮");

            ForceAscii = true;
            Log("This is unicode(echo): ⭕️ ☢ ☃☃☃ ☮");
            Log("This is unicode(log): ⭕️ ☢ ☃☃☃ ☮");
            DebugOutput = false;
            Debug("This is unicode(debug1): ⭕️ ☢ ☃☃☃ ☮"); // now shown because DegutOutput is false here
            DebugOutput = true;
            Debug("This is unicode(debug2): ⭕️ ☢ ☃☃☃ ☮");

            string progJson2 = """
            #! /usr/bin/env program
            (defvar $list [11, null, "abc"])
            """;

            UseAnsiConsole = true;

            var prog2 = FromJson(progJson2);
            //Log(prog2, "prog2");
            prog2.Dump(title: "prog2");

            var parsere3 = new CSharpEasyLanguageHandler(numberAsDecimal: true, removeSurrogatePair: false);

            string cljureCode01 = File.ReadAllText("assets/cljure_code01.clj");
            Log(cljureCode01, "cljureCode01");
            DumpObject(parsere3.ParseJsonSequence(cljureCode01), "cljureCode01(parsed)");

            string cljureCode02 = File.ReadAllText("assets/cljure_code02.clj");
            Log(cljureCode02, "cljureCode02");
            DumpObject(parsere3.ParseJsonSequence(cljureCode02), "cljureCode02(parsed)");

            Console.WriteLine("""[universal]THIS is unicode(log): [252ee4f0-d951-4ea4-bd3f-95e9af976141]2B55[252ee4f0-d951-4ea4-bd3f-95e9af976141]uuFE0F [252ee4f0-d951-4ea4-bd3f-95e9af976141]u2622 [252ee4f0-d951-4ea4-bd3f-95e9af976141]u2603[252ee4f0-d951-4ea4-bd3f-95e9af976141]uu2603[252ee4f0-d951-4ea4-bd3f-95e9af976141]uu2603 [252ee4f0-d951-4ea4-bd3f-95e9af976141]u262E[/universal]""");

            string xmlString = "<Root><Child>Content</Child></Root>";
            XDocument doc = XDocument.Parse(xmlString);
            Console.WriteLine(doc.Root?.Name);
            Log(doc.Root?.ToString());

            ForceAscii = false;
            var printed = FromJson(trimmedJson);
            DumpObject(printed, "⁅markup⁆[blue]printed[/]");
            printed.Dump("⁅red⁆printed⁅/⁆");
            Log("⁅blue⁆printed⁅/⁆", title: "⁅red⁆(?°□°)?⁅/⁆ ⁅blue⁆┻━┻⁅/⁆");
            Log("⁅blue⁆printed⁅/⁆", title: "⁅red⁆(?°□°)?⁅/⁆ ⁅blue⁆┻━┻⁅/⁆");
            WriteLine("⁅blue⁆⁅link=https://www.youtube.com/⁆Ctrl+click this link to visit YouTube⁅/⁆⁅/⁆!", title: "⁅red⁆(?°□°)?⁅/⁆ ⁅blue⁆┻━┻⁅/⁆");

            FromObject(parsere3.ParseJsonSequence(cljureCode02)).Dump(hideKeys: ["!"]);

            Echo("⁅markup⁆[green]This is green.[/]");
            Echo(new { args }, "args");
            Log("[END]");
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
}
