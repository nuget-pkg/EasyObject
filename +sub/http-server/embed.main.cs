//css_nuget EasyObject
//css_nuget EmbedIO
//css_nuget Stubble.Core
//using Grapevine;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Swan.Logging;
using Stubble.Core.Builders;
using Stubble.Core.Exceptions;
using Stubble.Core.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Global.EasyObject;

var server = new TestServer();
{
    Task task = server.Start();
    Thread.Sleep(1000);

    Process.Start("http://localhost:1234/api/test");
    task.Wait();
    //Console.WriteLine("Press enter to stop the server");
    //Console.ReadLine();
}

class TestServer
{
    public /*async*/ Task Start()
    {
        int port = -1;
        string? url = null;
        WebServer? server = null;
        while (true)
        {
            try
            {
                //port = Util.FreeTcpPort();
                //Util.Print(port);
                url = $"http://localhost:1234";
                server = CreateWebServer(url);
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
                if (server != null)
                {
                    server.Dispose();
                    server = null;
                }
            }
        }
        /*await*/return server.RunAsync();
    }
    private static WebServer CreateWebServer(string url)
    {
        var server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
            .WithWebApi("/", m => m.WithController<TestController>())
            ;

        server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();
        return server;
    }
}
class TestController : WebApiController
{
    [Route(HttpVerbs.Get, "/api/test")]
    public async Task ApiTest(/*string id*/)
    {
        //string playlistPath = @"G:\マイドライブ\.gas-youtube\.flat\_flat_playlist.json"
        string playlistPath = @"c:/home17/youtube-db/flat3.json";
        var playlist = FromFile(playlistPath);
        playlist = playlist["entries"];
        playlist = playlist.Shuffle();
        //playlist.Dump();
        var stubble = new StubbleBuilder()
            .Configure(builder =>
            {
                builder.AddToTemplateLoader(new DictionaryLoader(new Dictionary<string, string>
                {
                    //{ "Foo", "I'm Foo" },
                    //{ "Bar", "I'm Bar" }
                }));
            }).Build();
        //Assert.Equal("I'm Foo", stubble.Render("Foo", new { foo = "bar" }));
        //Assert.Equal("bar", stubble.Render("{{foo}}", new { foo = "bar" }));
        var text = "";
        const string template =
    """
    <tr>
      <td>
        <a href="https://www.youtube.com/playlist?list={{id}}" target="_blank"><img src="{{thumbnail}}" /></a>
      </td>
      <td>
        <a href="https://www.youtube.com/playlist?list={{id}}" target="_blank">
        i: {{i}}
        <br>
        id: {{id}}
        <br>
            title: {{title}}
        <br>
        </a>
      <td>
        <a href="/users/{{id}}" target="_blank">{{title}}</a>
      </td>
    </tr>
    """;
        foreach (var item in playlist.Dynamic)
        {
            //int i = item.i;
            int i = -1;
            string id = item.id;
            string title = item.title;
            string thumbnail = item.thumbnails[0].url;
            var rendered = stubble.Render(template, new { i, id, title, thumbnail });
            text += rendered;
        }
        await HttpContext.SendStringAsync(
$"""
 <!DOCTYPE html>
 <html lang="en">
 <head>
 <meta charset="utf-8">
 </head>
 <body>
 <table>
 {text}
 </table>
 </body>
 </html>
 """
, "text/html", System.Text.Encoding.UTF8);
    }

}
