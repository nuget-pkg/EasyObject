//css_nuget EasyObject
//css_nuget YoutubeExplode
//css_nuget Grapevine
//css_nuget Stubble.Core
using YoutubeExplode;
using YoutubeExplode.Common;
using Grapevine;
using Stubble.Core.Builders;
using Stubble.Core.Exceptions;
using Stubble.Core.Loaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static Global.EasyObject;

using (var server = RestServerBuilder.UseDefaults().Build())
{
    server.Start();
    Thread.Sleep(1000);
    Process.Start("http://localhost:1234/api/test");

    Console.WriteLine("Press enter to stop the server");
    Console.ReadLine();
}

[RestResource]
public class MyResource
{

    [RestRoute("Get", "/users/{id:string}")]
    public async Task User(IHttpContext context)
    {
        Pass();
        var id = context.Request.PathParameters["id"];
        var text = $"<tr><td>id: {id}</td></tr>";
        using var youtube = new YoutubeClient();
        var playlistUrl = id; // "https://youtube.com/playlist?list=PLa1F2ddGya_-UvuAqHAksYnB0qL9yWDO6";

        // Each batch corresponds to one request
        await foreach (var batch in youtube.Playlists.GetVideoBatchesAsync(playlistUrl))
        {
            foreach (var video in batch.Items)
            {
                var title = video.Title;
                var author = video.Author;
                text += $"<tr><td>title: {title}</td><td>author: {author}</td></tr>";
            }
        }
        await context.Response.SendResponseAsync(
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
 """);
    }
    [RestRoute("Get", "/api/test")]
    public async Task Test(IHttpContext context)
    {
        string playlistPath = @"G:\マイドライブ\.gas-youtube\.flat\_flat_playlist.json";
        var playlist = FromFile(playlistPath);
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
        thumb: {{thumbnail}}
        <br>
        </a>
      <td>
        <a href="/users/{{id}}" target="_blank">{{title}}</a>
      </td>
    </tr>
    """;
        foreach (var item in playlist.Dynamic)
        {
            int i = item.i;
            string id = item.id;
            string title = item.title;
            string thumbnail = item.thumbnail;
            var rendered = stubble.Render(template, new { i, id, title, thumbnail });
            text += rendered;
        }
        await context.Response.SendResponseAsync(
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
 """);
    }
}
