//css_nuget Spectre.Console;
using System.Collections.Generic;
using Spectre.Console;

namespace Columns;

public static class Program
{
    public static void Main()
    {
        var cards = new List<Panel>();
        foreach (var user in User.LoadUsers())
        {
            cards.Add(
                new Panel(GetCardContent(user))
                    .Header($"{user.Country}")
                    .RoundedBorder().Expand());
        }

        // Render all cards in columns
        AnsiConsole.Write(new Spectre.Console.Columns(cards));
    }

    private static string GetCardContent(User user)
    {
        var name = $"{user.FirstName} {user.LastName}";
        var city = $"{user.City}";

        return $"[b]{name}[/]\n[yellow]{city}[/]";
    }
}

public sealed class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public static List<User> LoadUsers()
    {
        return new List<User>
            {
                new User
                {
                    FirstName = "Andrea",
                    LastName = "Johansen",
                    City = "Hornbak",
                    Country = "Denmark",
                },
                new User
                {
                    FirstName = "Phil",
                    LastName = "Scott",
                    City = "Dayton",
                    Country = "United States",
                },
                new User
                {
                    FirstName = "Patrik",
                    LastName = "Svensson",
                    City = "Stockholm",
                    Country = "Sweden",
                },
                new User
                {
                    FirstName = "Freya",
                    LastName = "Thompson",
                    City = "Rotorua",
                    Country = "New Zealand",
                },
                new User
                {
                    FirstName = "????",
                    LastName = "?????",
                    City = "?????",
                    Country = "Iran",
                },
                new User
                {
                    FirstName = "Yara",
                    LastName = "Simon",
                    City = "Develier",
                    Country = "Switzerland",
                },
                new User
                {
                    FirstName = "Giray",
                    LastName = "Erbay",
                    City = "Karabuk",
                    Country = "Turkey",
                },
                new User
                {
                    FirstName = "Miodrag",
                    LastName = "Schaffer",
                    City = "Mockern",
                    Country = "Germany",
                },
                new User
                {
                    FirstName = "Carmela",
                    LastName = "Lo Castro",
                    City = "Firenze",
                    Country = "Italy",
                },
                new User
                {
                    FirstName = "Roberto",
                    LastName = "Sims",
                    City = "Mallow",
                    Country = "Ireland",
                },
            };
    }
}
