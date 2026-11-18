//css_nuget EasyObject
//css_nuget GenHTTP.Full
//css_nuget Stubble.Core
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


using GenHTTP.Engine.Internal;
using GenHTTP.Modules.Functional;
using GenHTTP.Modules.Practices;

// http://localhost:8080/users/42

//Process.Start("http://localhost:8080/api/test");

Console.WriteLine("http://localhost:8080/api/test");

var api = Inline.Create()
    //.Get("/users/:id", (int id) => new User(id, "Jane"));
    .Get("/api/test", () => {
        //new User(id, "Jane");
        return @"
 <!DOCTYPE html>
 <html lang=""en"">
 <head>
 <meta charset=""utf-8"">
 </head>
 <body>
 <table>
hello world ハロー©ワールド;
 </table>
 </body>
 </html>
 ";
    });


await Host.Create()
    .Handler(api)
    .Defaults()
    .RunAsync();

//public record User(int Id, string Name);


#if false
//[RestResource]
public class MyResource
{
    //[RestRoute("Get", "/api/test")]
    public async Task Test(IHttpContext context)
    {
        string playlistPath = @"G:\マイドライブ\.gas-youtube\.flat\_flat_playlist.json";
        var playlist = FromFile(playlistPath);
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
        title: {{title}}
        <br>
        thumb: {{thumbnail}}
        <br>
        </a>
    </tr>
    """;
        foreach(var item in playlist.Dynamic)
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
#endif