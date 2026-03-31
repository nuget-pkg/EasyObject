#if false
#if MINIMAL
using EasyObject = Global.MiniEasyObject;
#endif
using System.Collections.Generic;
using System.Dynamic;
#if MINIMAL
using static Global.MiniEasyObject;
#else
using static EasyObject;
#endif
using System.Collections.Generic;
using System.IO;
namespace Global;
using System;
using System.Net.Http;
using System.Threading.Tasks;
public static partial class EasySystemBeta {
    public static string GetRidirectUrl(string url) {
        Task<string> task = GetRidirectUrlTask(url);
        task.Wait();
        return task.Result;
        // !! LOCAL FUNCTIONS BELLOW !!
        async Task<string> GetRidirectUrlTask(string url) {
            HttpClient client;
            HttpResponseMessage response;
            try {
                client = new HttpClient();
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) {
                return url;
            }
            string result = response.RequestMessage!.RequestUri!.ToString();
            response.Dispose();
            return result;
        }
    }
    public static string? FindExeRecursive(string rootDirectory, string exeName) {
        try {
            IEnumerable<string> exeFiles =
                Directory.EnumerateFiles(rootDirectory, "*.exe", SearchOption.AllDirectories);
            Console.WriteLine($"EXE files found under {rootDirectory}:");
            foreach (string file in exeFiles) {
                Console.WriteLine(file);
                string baseName = EasySystem.SafeBaseName(file);
                if (string.Equals(baseName, exeName, StringComparison.CurrentCultureIgnoreCase)) {
                    return file;
                }
            }
        }
        catch (UnauthorizedAccessException ex) {
            Console.Error.WriteLine($"Access denied to one or more directories: {ex.Message}");
        }
        catch (DirectoryNotFoundException ex) {
            Console.Error.WriteLine($"Directory not found: {ex.Message}");
        }
        catch (Exception ex) {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
#endif