//css_nuget EasyObject;
//+#def     CS_SCRIPT
using System;
using System.Collections.Generic;

#if CS_SCRIPT
using static Global.EasyObject;
        UseAnsiConsole = true;
        List<string> questions = new List<string> { "question1", "question2", "question3" };
        Log(questions);
        string randomQuestion = EasyRandomPicker.PickRandomItem(questions);
        Console.WriteLine(randomQuestion);
#else
namespace Global;
#endif

public class EasyRandomPicker {
    private static readonly Random Rnd = new Random();
    public static T? PickRandomItem<T>(List<T> list) {
        if (list == null || list.Count == 0) {
            return default; // Or throw an exception
        }
        int index = Rnd.Next(list.Count);
        return list[index];
    }
}
