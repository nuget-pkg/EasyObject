//css_nuget EasyObject
//css_nuget YoutubeExplode
//css_nuget Grapevine
//css_nuget Stubble.Core
//css_nuget System.Data.SQLite
//css_nuget SQLitePCLRaw.bundle_green
//css_nuget Dapper
using YoutubeExplode;
using YoutubeExplode.Common;
using Grapevine;
using Stubble.Core.Builders;
using Stubble.Core.Exceptions;
using Stubble.Core.Loaders;
using Dapper;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static Global.EasyObject;
using Global;

var config = new SQLiteConnectionStringBuilder()
{
    DataSource = HyperOperatingSystem.FindGitRoot(HyperOperatingSystem.GetCwd())! + "/+sub/http-server/subble.db3"
    //DataSource = @"c:/home17/+sub/nuget.org/EasyObject/+sub/http-server/subble.db3"
};
using (var connection = new SQLiteConnection(config.ToString()))
{
    connection.Open();
    connection.Execute(
 @"CREATE TABLE if not exists user(
   id integer primary key,
   name text,
   age integer
   );");
    //var newUser = new User { Name = "new", Age = 12 };
    connection.Execute(@"insert into user (name, age) values (:name, :age);",
        new { name = "new", age = 27 });
    var users = connection.Query<User>(@"select * from user");
    foreach(var user in users)
    {
        Echo(user);
    }
}

//Environment.Exit(0);

using (var server = RestServerBuilder.UseDefaults().Build())
{
    server.Start();
    Thread.Sleep(1000);
    Process.Start("http://localhost:1234/api/test");

    Console.WriteLine("Press enter to stop the server");
    Console.ReadLine();
}

public class User
{
    public long? ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
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
        var stubble = new StubbleBuilder()
            .Configure(builder =>
            {
                builder.AddToTemplateLoader(new DictionaryLoader(new Dictionary<string, string>
                {
                    //{ "Foo", "I'm Foo" },
                    //{ "Bar", "I'm Bar" }
                }));
            }).Build();
        using var youtube = new YoutubeClient();
        var playlistUrl = id; // "https://youtube.com/playlist?list=PLa1F2ddGya_-UvuAqHAksYnB0qL9yWDO6";

        // Each batch corresponds to one request
        await foreach (var batch in youtube.Playlists.GetVideoBatchesAsync(playlistUrl))
        {
            foreach (var video in batch.Items)
            {
                var title = video.Title;
                var author = video.Author;
                var videoId = video.Id;
                text += stubble.Render("<tr><td>title: {{title}}</td><td>author: {{author}}</td></tr>",
                    new { title, author });
                text += stubble.Render("<tr><td><iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/{{videoId}}\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share\" referrerpolicy=\"strict-origin-when-cross-origin\" allowfullscreen></iframe></td></tr>",
                    new { title, author, videoId });
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
