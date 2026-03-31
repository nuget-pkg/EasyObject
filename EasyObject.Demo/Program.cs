// ReSharper disable CheckNamespace
// ReSharper disable OperatorIsCanBeUsed
// ReSharper disable UnusedVariable
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable EmptyForStatement
// ReSharper disable ConvertToConstant.Local
// ReSharper disable InconsistentNaming
#pragma warning disable CS0162 // 到達できないコードが検出されました
#if MINIMAL
namespace Misc;
using EasyObject = Global.MiniEasyObject;
using static Global.MiniEasyObject;
#else
namespace EasyObjectDemo;
using static Global.EasyObject;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Global;
using static Global.EasySystem;
public class Program {
    public static void Main(string[] args) {
// ReSharper disable HeuristicUnreachableCode
        try {
            SetupConsoleEncoding();
            //ShowLineNumbers = false;
            ShowDetail = true;
            UseAnsiConsole = true;
            DebugOutput = true;
            Log("⭕️ハロー©⭕️");
            Break("after ⭕️ハロー©⭕️");
            ExitOnTrustViolation(new Exception("I FOUND A PROBLEM"), hint: new { TOM = "FOOLISH !!" }, 777);
            WriteLine("(1)");
            var eoNull = Null;
            Log(eoNull.ToJson());
            Log(eoNull.ToPrintable());
            Log(eoNull);
            Log(Null);
            WriteLine("(2)");
            var eo = FromObject(new { a = 123 });
            Log(eo);
            WriteLine("(3)");
            Log(eo.TypeValue, "eo.TypeValue");
            WriteLine("(4)");
            TrustIdentical(eo.TypeValue, @object);
            WriteLine("(5)");
            var a = eo["a"];
            WriteLine("(5.1)");
            Log(FullName(a));
            Log(a.GetType() == typeof(double));
            WriteLine("(5.1.1)");
            WriteLine("(5.1.1.1)");
            WriteLine("(5.1.2)");
            Log(a, "a");
            WriteLine("(5.2)");
            Log(eo["a"]);
            WriteLine("(6)");
            TrustIdentical(eo["a"].Cast<int>(), 123);
            WriteLine("(7)");
            TrustIdentical(eo.Keys, new List<string> { "a" });
            Log(eo[0], "eo[0]");
            TrustIdentical(eo[0].TypeValue, @null);
            TrustTrue(eo[0].IsNull);
            Log(eo[1], "eo[1]");
            TrustIdentical(eo[1].TypeValue, @null);
            TrustTrue(eo[1].IsNull);
            foreach (var pair in eo.Dynamic) Log(pair, "pair");
            eo = FromObject(null);
            Log(eo.TypeValue, "eo.TypeValue");
            TrustIdentical(eo.TypeValue, @null);
            eo["b"] = true;
            TrustIdentical(eo["b"].Cast<bool>(), true);
            Log(eo["b"].TypeValue, "eo.b.TypeValue");
            TrustIdentical(eo["b"].TypeValue, boolean);
            eo[3] = 777;
            Log(eo[3].Cast<int>());
            Log(eo.TypeValue, "eo.TypeValue");
            TrustIdentical(eo.TypeValue, EasyObject.array);
            TrustIdentical(eo.Count, 4);
            TrustIdentical(eo[0].TypeValue, @null);
#if false
        Trust.That(() => { var n = eo[0].Cast<int>(); },
            Throws.TypeOf<System.InvalidCastException>()
            .With.Message.EqualTo("Null オブジェクトを値型に変換することはできません。")
            );
#endif
            //Trust.That((int?)eo[0], Is.EqualTo(null));
            TrustIdentical(eo[3].Cast<int>(), 777);
            foreach (var e in eo.Dynamic) Log(e, "e");
            var eo2 = FromObject(eo); // UnWrap() test
            var eo3 = FromJson("""
                               { a: 123, b: [11, 22, 33] }
                               """);
            Log(eo3, "eo3");
            Log(eo3["b"][1]);
            var list = new List<int>();
            foreach (var e in eo3["b"].Dynamic) list.Add((int)e);
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
            foreach (var e in i.Dynamic) Log(e);
            ////var bigJson = File.ReadAllText("assets/qiita-9ea0c8fd43b61b01a8da.json");
            var bigJson =
                File.ReadAllText(EasySystem.GitProjectFile(GetCwd(), "assets",
                    "qiita-9ea0c8fd43b61b01a8da.json")!); /**/
            //Log(bigJson);
            var sw = new Stopwatch();
            TimeSpan ts;
            sw.Start();
            for (var c = 0; c < 5; c++) {
                //var test = FromJson(bigJson);
            }
            sw.Stop();
            WriteLine("■EasyObject");
            ts = sw.Elapsed;
            WriteLine($"　{ts}");
            WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
            sw.Start();
            for (var c = 0; c < 5; c++) {
                //JObject jsonObject = JObject.Parse(bigJson);
            }
            sw.Stop();
            WriteLine("■Newtonsoft.Json");
            ts = sw.Elapsed;
            WriteLine($"　{ts}");
            WriteLine($"　{ts.Hours}時間 {ts.Minutes}分 {ts.Seconds}秒 {ts.Milliseconds}ミリ秒");
            WriteLine($"　{sw.ElapsedMilliseconds}ミリ秒");
            //var list01_txt = File.ReadAllText("assets/list01.txt");
            //var list01_txt = File.ReadAllText("assets/mydict.txt");
            //Log(list01_txt);
            //var list01_bytes = Convert.FromBase64String(list01_txt);
            //var unpickler = new Unpickler();
            //object result = unpickler.loads(list01_bytes);
            //Log(result, "result");
            //var o = new PlainObjectConverter(forceAscii: false).Parse(result);
            //Log(o, "o");
            //var pickler = new Pickler();
            //var bytes = pickler.dumps(o);
            //var ox = unpickler.loads(bytes);
            //Log(ox, "ox");
            //var eo_ox = EasyObject.FromObject(ox);
            //Log(eo_ox, "eo_ox");
            //Log(eo_ox.ToJson(true, true), "eo_ox.ToJson(true, true)");
            Log(DateTime.Now);
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
            Log(myData2.RealData == null);
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

            //Log(ast.ToJson(indent: true));
            Log(new { abc = 123, xyz = "abc" });
            Log(FullName(new { abc = 123, xyz = "abc" }));
            var trimmedJson = """
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
            trimTest.Trim(1);
            Log(trimTest, "(1)");
            trimTest = FromJson(trimmedJson);
            Log(trimTest, maxDepth: 2);
            trimTest.Trim(2);
            Log(trimTest, "(2)");
            trimTest = FromJson(trimmedJson);
            Log(trimTest, hideKeys: ["a"]);
            trimTest.Trim(hideKeys: ["a"]);
            Log(trimTest, "(3)");
            Log(trimTest.ToJson(keyAsSymbol: true));
            var parser = new EasyLanguageParser(true, true);
            var result1 = parser.ParseJson("'🔥引火★★帝国🔥'");
            Log(result1, "result1");
            var parser2 = new EasyLanguageParser(true, false);
            var result2 = parser2.ParseJson("'🔥引火★★帝国🔥'");
            Log(result2, "result2");
            var containSurrogate = FromObject(new { title = "🔥引火★★帝国🔥" });
            Log(containSurrogate);
            Log(containSurrogate, removeSurrogatePair: true);
            Log(containSurrogate, removeSurrogatePair: false);
            Log(containSurrogate);
            Log(containSurrogate, removeSurrogatePair: true);
            Log(containSurrogate, removeSurrogatePair: false);
            var jsonNoSurrogete = containSurrogate.ToJson(true, removeSurrogatePair: true);
            Log(jsonNoSurrogete, "jsonNoSurrogete");
            var jsonWithSurrogete = containSurrogate.ToJson(true, removeSurrogatePair: false);
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

            //string json = Utf8StringFromUrl("https://jsonplaceholder.typicode.com/todos/1");
            //Log(json, "json");
            //var todo = FromJson(json);
            //Log(todo, "todo");

            //var todo2 = FromUrl("https://jsonplaceholder.typicode.com/todos/1");
            //Log(todo2, "todo2");
            WriteLine("[stdout] This is unicode: ⭕️ ☢ ☃☃☃ ☮");
            Console.Error.WriteLine("[stderr] This is unicode: ⭕️ ☢ ☃☃☃ ☮");
            double videoDuration = 9999;
            Log(videoDuration, "⁅markup⁆[red]Duration too long...skipping![/]");
            DebugOutput = true;
            Debug(new { a = 123, b = "xyz" },
                "⁅markup⁆[green]Debug[/] with [blue][link=https://en.wikipedia.org/wiki/ANSI_escape_code]Ansi Color(Ctrl-Click Me!)[/][/]");
            var messageToEscape = """[this is not markup tag...so print this message with square brackets]""";
            var safeMessage = MarkupSafeString(messageToEscape);
            Log(safeMessage, "safeMessage");
            Log($"⁅markup⁆[green]{safeMessage}[/]");
            WriteLine("⁅markup⁆[blue][link=https://www.youtube.com/]Ctrl+Click this link to visit YouTube[/][/]!",
                "⁅markup⁆[red](?°□°)?[/] [blue]┻━┻[/]");

            //Crash();
            var embeddedJsonUrl =
                "https://github.com/nuget-pkg/Global.Sys/blob/2026.0311.1056.12/Global.Sys.Demo/assets/text-embed-text-02.json";
            var embeddedEo = FromUrl(embeddedJsonUrl);
            Log(embeddedEo, "embeddedEO(github)");

            //https://github.com/nuget-pkg/nuget-assets/blob/2026.0325.0322.35/my-ls.exe
            embeddedJsonUrl = "https://github.com/nuget-pkg/nuget-assets/blob/2026.0325.0322.35/my-ls.exe";
            var text1 = EasyTextEmbedder.ExtractEmbeddedText(embeddedJsonUrl);
            Log(text1, "text1");
            var eo1 = FromJson(text1);
            Log(eo1);
            //Crash();
#if true //!TEST_MINI
            embeddedEo = ExtractFromFile(embeddedJsonUrl);
            Log(embeddedEo, "embeddedEO(nuget-assets::my-ls.exe)");
#endif
            var noError = FromJson("\n", true);
            Log(noError, "noError");
            Echo("⁅markup⁆[green]This is green.[/]");
            Echo(new { args }, "⁅markup⁆[green]args[/]");
            Log("⁅markup⁆[green]This is green.[/]");
            Log(new { args }, "⁅markup⁆[green]args[/]");
            var code =
                """
                //!?"'#%&^~\|`;:()[]{}<>, + - * / = ❝　❞←全角スペース
                namespace HelloWorldApp
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                            Console.WriteLine("Hello, World!?");
                        }
                    }
                }
                """;
            Log(UniversalTransformer.SafeSourceCode(code));
            var fname =
                """[1080p] ✅ 👀 🫧 💻 🌐 🎵 <xml>aaa</xml> ; {Title}!? x=11+22-33; ,(🔥引火帝国🔥):"name1" 'name2'?.txt""";
            Log(UniversalTransformer.SafeFileName(fname), "⁅markup⁆[blue]adjusted file name[/]");
            Log(UniversalTransformer.SafeFileName(fname, replaceSurrogate: ""),
                "⁅markup⁆[green]adjusted file name (keeping surrogate pairs)[/]");
            Log(UniversalTransformer.SafeFileName(fname, replaceSurrogate: "@"),
                "⁅markup⁆[purple]adjusted file name (spicifying surrogate pairs' replacement)[/]");

            //Log("This is unicode(echo): ⭕️ ☢ ☃☃☃ ☮");
            //Log("This is unicode(log): ⭕️ ☢ ☃☃☃ ☮");
            //DebugOutput = false;
            //Debug("This is unicode(debug1): ⭕️ ☢ ☃☃☃ ☮"); // now shown because DegutOutput is false here
            //DebugOutput = true;
            //Debug("This is unicode(debug2): ⭕️ ☢ ☃☃☃ ☮");

            //ForceAscii = true;
            //Log("This is unicode(echo): ⭕️ ☢ ☃☃☃ ☮");
            //Log("This is unicode(log): ⭕️ ☢ ☃☃☃ ☮");
            //DebugOutput = false;
            //Debug("This is unicode(debug1): ⭕️ ☢ ☃☃☃ ☮"); // now shown because DegutOutput is false here
            //DebugOutput = true;
            //Debug("This is unicode(debug2): ⭕️ ☢ ☃☃☃ ☮");
            ForceAscii = false;
            Echo("⁅markup⁆[green]This is unicode(before END): ⭕️ ☢ ☃☃☃ ☮[/]",
                "⁅markup⁆[cyan]Echo() does not emit SOURCE CODE LOCATION![/]");

            //DebugOutput = true;
            //ShowLineNumbers = false;
            //ForceAscii = false;
            //Log("⭕️🈂️❝END❞🈂️", "ShowLineNumbers = false");
            //ShowLineNumbers = false;
            //ForceAscii = false;
            //Debug("⭕️🈂️❝END❞🈂️", "Debug() shows line info even if `ShowLineNumbers == false`");
            void LinkTest(string title, string url) {
                //LogWebLink(title, url);
                EchoWebLink(title, url);
            }
            LinkTest(
                "⭕️⁅🌐⁆@⁅反転mirror⁆パイパイ仮面でどうかしらん？ / 宝鐘マリン FULL 踊ってみた【練習用】",
                "https://www.youtube.com/watch?v=sLpodTN4xhI&list=PLTvSv0jkjbk9-emLIV2vM-0p7CeMnTYG2"
            );
            LinkTest(
                "⭕️🈂️❝FG⁅ｼﾞﾝｷﾞｽｶﾝ⁆❞🈂️ファイターズガール「ジンギスカン」踊ってみた 歌詞付き",
                "https://www.youtube.com/watch?v=DHbIIBmqHsw&list=PLTvSv0jkjbk8wtAgpVJH1L21EgeMi_ULc"
            );
            LinkTest(
                "⭕️⁅🌐⁆@ラム:DANCING STAR 2026",
                "https://www.youtube.com/watch?v=wzcdhDyNmMM&list=PLTvSv0jkjbk8gtWLMLXLHYrWio5ciOi8c"
            );
            LinkTest(
                "⭕️⁅🌐⁆@エレクトロニック・ダンス・ミュージック",
                "https://www.youtube.com/watch?v=4B5IHILMWOM&list=PLTvSv0jkjbk_u4GZBJK74w7aWylX-8FSt"
            );
            LinkTest(
                "⭕️⁅🌐⁆@⁅CHANNEL：〘!!GREAT!!〙Blackpink Diaries⁆⭕️❝BLACKPINK➡️Ice Cream (2026 Official Music Video)❞",
                "https://www.youtube.com/watch?v=YwhhB8rKb6U&list=PLTvSv0jkjbk9vEyRq7pK_U8fbGrXirdAi"
            );
            LinkTest(
                "⭕️⁅🌐⁆@⁅CHANNEL：〘!!GREAT!!〙Alyssa' s Music Loop⁆⭕️❝🎙 More Than Is Good for Me ( Original ) ✨️ EDM - Electronic Dance Music ✨️ # 179❞",
                "https://www.youtube.com/watch?v=qrW3yK7AWjE&list=PLTvSv0jkjbk-8ABf2TXzCXWk7zn10Ute7"
            );
            DebugOutput = true;
            var versionTextPath = GitProjectFile(GetCwd(), "version.txt")!;
            EchoWebLink("version.txt", versionTextPath);

            // !! NEW FEATURE: YOU CAN PICK n ELEMENTS RANDOMELY !!
            var pickCandidates = NewArray(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            for (var p = 0; p < 5; p++) {
                var picked = pickCandidates.Pick(3);
                Echo(picked, $"piccked[{p}]", true);
            }
            var overLimit = pickCandidates.Pick(9999);
            Echo(overLimit, "overLimit", true);
            TrustTrue(11 + 22 == 33);
            pickCandidates = NewObject("a", 11, "b", NewArray(1.1, 1.2, 1.3), "c", null, "d", 777);
            DebugOutput = true;
            for (var p = 0; p < 5; p++) {
                var picked = pickCandidates.Pick(3);
                Echo(picked, $"piccked[{p}]", true);
            }
            overLimit = pickCandidates.Pick(9999);
            Echo(overLimit, "overLimit", true);
            //
            var playlistJsonPath = GitProjectFile(GetCwd(), "EasyObject.Demo", "assets", "youtube-playlists.json")!;
            Log(playlistJsonPath, title: "playlistJsonPath");
#if true
            // Since 2026.03.30 EasyObject's default JSON Parsing Engine is Newtonsoft.JSON
            var youtubePlaylists =
                FromFile(GitProjectFile(GetCwd(), "EasyObject.Demo", "assets", "youtube-playlists.json")!);
#else
            //var youtubePlaylists = NewtonsoftJsonUtil.DeserializeFromJson(File.ReadAllText(playlistJsonPath));
            var youtubePlaylists = NewtonsoftJsonUtil.FromJsonFile(playlistJsonPath);
#endif
            youtubePlaylists.Dump(maxDepth: 2);
            //Log(FullName(youtubePlaylists.RealData));
            //Log(youtubePlaylists.RealData!.GetType().FullName);
            //Abort();
            var playlistIdsOf5 = youtubePlaylists.Pick(5).AsStringList;
            Log(playlistIdsOf5, title: "playlistIdsOf5", compact: true);
            //Abort();
            for (var p = 0; p < playlistIdsOf5.Count; p++) {
                var playlistId = playlistIdsOf5[p];
                var playlistObject = youtubePlaylists[playlistId];
                playlistObject[playlistId].Dump(maxDepth: 1);
                string playlistTitle = playlistObject.Dynamic.title;
                int videoCount = playlistObject.Dynamic.videos.Count;
                Log(new { id = playlistId, title = playlistTitle, videoCount }, compact: true);
                if (videoCount > 0) {
                    var videos = playlistObject["videos"];
                    var video0 = videos[0];
                    Log(new {
                        id = playlistId,
                        title = playlistTitle, videoCount,
                        firstVideoId =
                            (string /* !! explicit cast is necessary when accessing through EasyObject.Dynamic !! */)
                            video0.Dynamic.id,
                        firstVideoTitle =
                            (string /* !! explicit cast is necessary when accessing through EasyObject.Dynamic !! */)
                            video0.Dynamic.title
                    }, compact: false);
                    var
                        videoId = video0["id"]
                            .Cast<string>(); // !! THIS IS HOW TO ACCESS OBJECT (DICTIONARY)'s PROPERTY DIRECTLY !!
                    var
                        videoTitle =
                            video0["title"]
                                .Cast<string>(); // !! THIS IS HOW TO ACCESS OBJECT (DICTIONARY)'s PROPERTY DIRECTLY !!
                    Log(new { videoId, videoTitle }, compact: true);
                    var watchUrl = $"https://www.youtube.com/watch?v={videoId}&list={playlistId}";
                    EchoWebLink(
                        $"≪PLAYLIST≫ ➠▶ {LimitStringLength(playlistTitle, 35, "▶!!!🈂TITLE-TOO-LONG🈂!!!▶")}({videoCount} VIDEOs) - ❝ ≪FIRST-VIDEO≫ ➠▶ {videoTitle} ❞",
                        watchUrl);
                }
            }
            WriteLine();
            Echo(
                "⁅markup⁆[red]!! PLEASE SET[/] [green]⁅ORERA BROWSER⁆[/] [red]ON PC AS DEFAULT[/]...[purple]IT AUTOMATICALLY STARTS PLAYING VIDEOS EVEN WHEN OPENED FROM LINK ON[/] [green]WINDOWS-TERMINAL ETC[/] [red]!![/]");
            // !! JSON ⇔ XML CONVERSION !!
            var xml01 = NewtonsoftJsonUtil.SerializeToToXml(new { doc = new { x = 1, y = "xyz" } });
            Log(xml01);
            var xml01Eo = NewtonsoftJsonUtil.DeserializeFromXml(xml01);
            Log(xml01Eo);
            //
            if (false) {
                Abort();
            }
            //
            if (false) TrustFalse(11 + 22 == 33); // !! THIS FAILS !!
            //
            if (false) {
                // !! THIS FAILS (WITH INTELLIGENT HINT...) !!
                var A = 11;
                var B = 22;
                TrustIdentical(A, B, new { A, B }, 123);
            }
            //
            if (false)
                // !! YOUR CAN EXIT (ABORT) PROGRAM ... WITH INTELLIGENT HINTING !!
                Abort(new {
                    abc = 123,
                    xyz = new {
                        test1 = new[] { "A", "B", "C ハロー©" }
                    }
                });
            //
            if (true) throw new NotImplementedException();
        }
        catch (Exception ex) {
            Abort(ex); // !! throw new NotImplementedException(); GOES HERE !!
        }
    }
    // ReSharper disable once UnusedMember.Local
    private static void LegacyLangParserDemo() {
        EasyObject.JsonParser = new CSharpEasyLanguageHandler(numberAsDecimal: true);
        var progJson = """
                       #! /usr/bin/env program
                       [11, null, "abc"]
                       """;
        Log(FromJson(progJson));
        Log(FromJson(null));
        var array = NewArray(1, null, "abc", FromJson(progJson));
        Log(array, "array");
        var obj = NewObject("a", 111, "b", FromJson(progJson));
        Log(obj, "obj");
        // Test newLisp expression
        var assocList = FromJson("""
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
        var progJson2 = """
                        #! /usr/bin/env program
                        (defvar $list [11, null, "abc"])
                        """;
        UseAnsiConsole = true;
        var prog2 = FromJson(progJson2);
        //Log(prog2, "prog2");
        prog2.Dump("prog2");
        var parsere3 = new CSharpEasyLanguageHandler(true);
        var cljureCode01 = File.ReadAllText(GitProjectFile(GetCwd(), "assets", "cljure_code01.clj")!);
        Log(cljureCode01, "cljureCode01");
        DumpObject(parsere3.ParseJsonSequence(cljureCode01), "cljureCode01(parsed)");
        var cljureCode02 = File.ReadAllText((GitProjectFile(GetCwd(), "assets", "cljure_code02.clj")!));
        Log(cljureCode02, "cljureCode02");
        DumpObject(parsere3.ParseJsonSequence(cljureCode02), "cljureCode02(parsed)");
        WriteLine(
            """[universal]THIS is unicode(log): [252ee4f0-d951-4ea4-bd3f-95e9af976141]2B55[252ee4f0-d951-4ea4-bd3f-95e9af976141]uuFE0F [252ee4f0-d951-4ea4-bd3f-95e9af976141]u2622 [252ee4f0-d951-4ea4-bd3f-95e9af976141]u2603[252ee4f0-d951-4ea4-bd3f-95e9af976141]uu2603[252ee4f0-d951-4ea4-bd3f-95e9af976141]uu2603 [252ee4f0-d951-4ea4-bd3f-95e9af976141]u262E[/universal]""");
        var xmlString = "<Root><Child>Content</Child></Root>";
        var doc = XDocument.Parse(xmlString);
        Log(doc.Root?.ToString());
        ForceAscii = false;
        ////var printed = FromJson(trimmedJson);
        ////DumpObject(printed, "⁅markup⁆[blue]printed[/]");
        ////printed.Dump("⁅markup⁆[red]printed[/]");
        Log("⁅markup⁆[blue]printed[/]", "⁅markup⁆[red](?°□°)?[/] [blue]┻━┻[/]");
        Log("⁅markup⁆[blue]printed[/]", "⁅markup⁆[red](?°□°)?[/] [blue]┻━┻[/]");
        FromObject(parsere3.ParseJsonSequence(cljureCode02)).Dump(hideKeys: ["!"]);
        EasyObject ast;
        ast = FromJson(Demo.BabelOutput.AstJson);
        var xo = ast.ExportToDynamicObject();
        Log(xo, "xo");
        ast = FromJson(Demo.BabelOutput.AstJson);
        ast.Trim(hideKeys: ["loc", "start", "end"], maxDepth: 3);
        Log(ast, "ast(1)");
        ast = FromJson(Demo.BabelOutput.AstJson);
        ast.Trim(hideKeys: ["loc", "start", "end"] /*, maxDepth: 2*/);
        Log(ast, "ast(2)", maxDepth: 2);
        Log(FromJson(Demo.BabelOutput.AstJson), "ast", true, 2, ["start", "end", "loc"]);
        Log(FromJson(Demo.BabelOutput.AstJson), "⁅markup⁆[green]ast[/]", true, 2,
            ["start", "end", "loc"]);
        ClearSettings();
    }
    internal class Exchangeable1 : IExportToPlainObject {
        public object ExportToPlainObject() {
            return 123;
        }
    }
    internal class Exchangeable2 {
        public object ExportToPlainObject() {
            return 456;
        }
    }
    internal class Exchangeable3 : IExportToCommonJson {
        public string ExportToCommonJson() {
            return "[11, 22, 33]";
        }
    }
    internal class Exchangeable4 {
        public string ExportToCommonJson() {
            return "[111, 222, 333]";
        }
    }
    internal class MyData : EasyObject {
        public MyData(int n, string s) {
            ImportFromPlainObject(new { n, s });
        }
        public MyData(string json) {
            ImportFromCommonJson(json);
        }
        public int N => Dynamic.n;
        public string S => Dynamic.s;
    }
}